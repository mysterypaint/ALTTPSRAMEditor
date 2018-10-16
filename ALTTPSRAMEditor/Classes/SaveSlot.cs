using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALTTPSRAMEditor
{
    class SaveSlot
    {
        private byte[] data;
        private String playerName = "";
        private UInt16 total_checksum = 0;
        private Link player;
        Form1.SaveRegion saveRegion;

        public SaveSlot(byte[] data_in)
        {
            // Import this save slot's data from the larger global save data
            data = data_in.ToArray();

            // Determine which region this save comes from
            if (data[0x3E5] == 0xAA && data[0x3E6] == 0x55)
                saveRegion = Form1.SaveRegion.USA;
            else if (data[0x3E1] == 0xAA && data[0x3E2] == 0x55)
                saveRegion = Form1.SaveRegion.JPN;
            else
                saveRegion = Form1.SaveRegion.EUR;

            // Copy global save data's Item&Equipment data to this Save Slot

            byte[] itemsAndEquipment = new byte[0x24];
            
            for (int i = 0x0; i < 0x24; i++)
            {
                itemsAndEquipment[i] = data[0x340 + i];
            }

            // Initialize a player object upon creating this save slot.
            player = new Link(itemsAndEquipment);
            
            getRawPlayerName();
        }

        private void getRawPlayerName()
        {
            if (!SaveIsValid())
                return;
            UInt16[] pNameRaw;
            int j = 0;

            switch (saveRegion)
            {
                default:
                case Form1.SaveRegion.EUR:
                case Form1.SaveRegion.USA:
                    pNameRaw = new UInt16[6];
                    for (int i = 0x3D9; i <= 0x3E4; i += 2)
                    {
                        pNameRaw[j] = (UInt16)((data[i + 1] << 8) | data[i]);
                        j++;
                    }
                    break;
                case Form1.SaveRegion.JPN:
                    pNameRaw = new UInt16[4];
                    for (int i = 0x3D9; i < 0x3E1; i += 2)
                    {
                        Console.WriteLine(i + ": " + (UInt16)((data[i + 1] << 8) | data[i]));
                        pNameRaw[j] = (UInt16)((data[i + 1] << 8) | data[i]);
                        j++;
                    }
                    break;
            }
            convertPlayerName(pNameRaw);
        }

        private void convertPlayerName(UInt16[] pNameRaw)
        {
            int j = 1; // Char counter
            foreach (UInt16 i in pNameRaw)
            {
                switch (saveRegion)
                {
                    case Form1.SaveRegion.EUR:
                    case Form1.SaveRegion.USA:
                        if (j > 6)
                            break;
                        try
                        {
                            playerName += Form1.rawENChar[i];
                        }
                        catch (KeyNotFoundException)
                        {

                        }
                        break;
                    case Form1.SaveRegion.JPN:
                        if (j > 4)
                            break;
                        Console.WriteLine(j);
                        try
                        {
                            playerName += Form1.rawJPChar[i];
                        }
                        catch (KeyNotFoundException)
                        {

                        }
                        break;
                }
                j++;
            }
        }

        public Link GetPlayer()
        {
            return player;
        }

        public Form1.SaveRegion GetRegion()
        {
            return saveRegion;
        }

        public byte[] GetData()
        {
            return data.ToArray();
        }

        public void ValidateSave()
        {
            // This function signs the edited save to be valid to the game.
            // It sums up every 16-bit value in the save slot, except for the final one
            // And then it writes the calculated checksum to the final two bytes (in little-endian order)
            // Before it writes that value it calculates, it does this(roughly, not literally): UInt16 total = 0x5A5A - checksum
            UInt16 checksum = 0;
            for (int i = 0; i < 0x4fe; i += 2)
            {
                checksum += (UInt16) ((data[i + 1] << 8) | data[i]);
            }
            total_checksum = (UInt16) (0x5A5A - checksum); // Calculate as 32-bit integer, then convert it to a 16-bit unsigned int
            
            data[0x4FE] = (byte) (total_checksum & 0xff);
            data[0x4FF] = (byte) (total_checksum >> 8);
        }

        public bool SaveIsValid()
        {
            // Tests if a loaded save is valid or not.
            switch(saveRegion)
            {
                case Form1.SaveRegion.EUR:
                case Form1.SaveRegion.USA:
                    if ((data[0x3E5] != 0xAA) || (data[0x3E6] != 0x55))
                        return false;
                    break;
                case Form1.SaveRegion.JPN:
                    if ((data[0x3E1] != 0xAA) || (data[0x3E2] != 0x55))
                        return false;
                    break;
            }

            UInt16 checksum = 0;
            for (int i = 0x0; i < 0x500; i += 2)
            {
                UInt16 word = (UInt16)((data[i + 1] << 8) | data[i]);
                checksum += word;
            }
            return checksum == 0x5A5A; // The save is valid if the checksum total is exactly 0x5A5A
        }

        public void UpdatePlayer()
        {
            // Take player's equipment and update the local data
            for (int i = 0x0; i <= 0x23; i++)
            {
                data[0x340 + i] = (byte) player.GetItemEquipment(i);
            }
        }

        public String GetPlayerName()
        {
            return playerName;
        }

        public void SetPlayerName(String str)
        {
            playerName = str;
        }

        public void SetData(byte[] in_data)
        {
            data = in_data.ToArray();
        }

        public void ClearData()
        {
            Array.Clear(data, 0, data.Length);
        }
    }
}