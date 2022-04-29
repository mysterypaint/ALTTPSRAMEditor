using System;
using System.Linq;

namespace ALTTPSRAMEditor
{
    [Serializable]
    internal class SaveSlot
    {
        private byte[] data;
        private string playerName = "";
        private ushort[] playerNameRaw;
        private ushort total_checksum = 0;
        private readonly Link player;
        private byte pendants;
        private byte crystals;
        private bool isValid;
        private int deathCounter = 0;
        private int liveSaveCounter = 0;
        private int slotIndex;
        private readonly Form1.SaveRegion saveRegion;
        private readonly byte[] itemsAndEquipment;

        public SaveSlot(byte[] data_in, int _slot)
        {
            // Import this save slot's data from the larger global save data
            data = data_in.ToArray();
            slotIndex = _slot;
            playerNameRaw = new ushort[6];

            // Determine which region this save comes from
            if (data[0x3E5] == 0xAA && data[0x3E6] == 0x55)
                saveRegion = Form1.SaveRegion.USA;
            else if (data[0x3E1] == 0xAA && data[0x3E2] == 0x55)
                saveRegion = Form1.SaveRegion.JPN;
            else
                saveRegion = Form1.SaveRegion.EUR;

            // Set death/live/save counter values
            deathCounter = 0;
            liveSaveCounter = 0;

            isValid = SaveIsValid();
            // Copy global save data's Item&Equipment data to this Save Slot

            itemsAndEquipment = new byte[0x4B];

            for (var i = 0x0; i < itemsAndEquipment.Length; i++)
            {
                itemsAndEquipment[i] = data[0x340 + i];
            }

            // Copy pendants and crystals to a private variable for this save slot
            pendants = itemsAndEquipment[0x34];
            crystals = itemsAndEquipment[0x3A];

            // Initialize a player object upon creating this save slot.
            player = new Link(itemsAndEquipment);

            getRawPlayerName();
        }

        public void ResetFileDeaths(bool showOnFileSelect)
        {
            var _deathCounterAddr = 0x0;
            var _liveSaveCounterAddr = 0x0;
            var _deathTotalsTableAddr = 0x0;

            switch (saveRegion)
            {
                default:
                case Form1.SaveRegion.EUR:
                case Form1.SaveRegion.USA:
                    _deathCounterAddr = 0x405;
                    _liveSaveCounterAddr = 0x403;
                    _deathTotalsTableAddr = 0x3E7;
                    break;
                case Form1.SaveRegion.JPN:
                    _deathCounterAddr = 0x401;
                    _liveSaveCounterAddr = 0x3FF;
                    _deathTotalsTableAddr = 0x3E3;
                    break;
            }
            for (var i = 0x0; i < 0x1B; i += 2)
            {
                data[_deathTotalsTableAddr + i] = 0x0;
                data[_deathTotalsTableAddr + i + 1] = 0x0;
            }

            data[_liveSaveCounterAddr] = 0x0;
            data[_liveSaveCounterAddr + 1] = 0x0;
            liveSaveCounter = 0;

            deathCounter = 0;

            if (showOnFileSelect)
            {
                data[_deathCounterAddr] = 0x0;
                data[_deathCounterAddr + 1] = 0x0;
            }
            else
            {
                data[_deathCounterAddr] = 0xFF;
                data[_deathCounterAddr + 1] = 0xFF;
            }

            if (isValid)
                ValidateSave();
        }

        public void CommitPlayerName()
        {
            // Set the actual player name in data[] just before writing to SRAM
            var j = 0;

            switch (saveRegion)
            {
                default:
                case Form1.SaveRegion.USA:
                case Form1.SaveRegion.EUR:
                    for (var i = 0x3D9; i <= 0x3E4; i += 2)
                    {
                        data[i] = (byte)(playerNameRaw[j] & 0xff);
                        data[i + 1] = (byte)(playerNameRaw[j] >> 8);
                        j++;
                    }
                    break;
                case Form1.SaveRegion.JPN:
                    for (var i = 0x3D9; i <= 0x3E0; i += 2)
                    {
                        data[i] = (byte)(playerNameRaw[j] & 0xff);
                        data[i + 1] = (byte)(playerNameRaw[j] >> 8);
                        j++;
                    }
                    break;
            }
        }

        public void SetSaveSlot(int _slot) => slotIndex = _slot;

        public void SetIsValid(bool _val) => isValid = _val;

        public bool GetIsValid() => isValid;

        public override string ToString() => slotIndex.ToString();
        private void getRawPlayerName()
        {
            if (!SaveIsValid())
                return;
            var j = 0;

            switch (saveRegion)
            {
                default:
                case Form1.SaveRegion.EUR:
                case Form1.SaveRegion.USA:
                    for (var i = 0x3D9; i <= 0x3E4; i += 2)
                    {
                        playerNameRaw[j] = (ushort)((data[i + 1] << 8) | data[i]);
                        j++;
                    }
                    break;
                case Form1.SaveRegion.JPN:
                    playerNameRaw = new ushort[4];
                    for (var i = 0x3D9; i < 0x3E1; i += 2)
                    {
                        playerNameRaw[j] = (ushort)((data[i + 1] << 8) | data[i]);
                        j++;
                    }
                    break;
            }
            convertPlayerNameRawToString(playerNameRaw);
        }

        private void convertPlayerNameRawToString(ushort[] playerNameRaw)
        {
            var j = 1; // Char counter
            foreach (var i in playerNameRaw)
            {
                switch (saveRegion)
                {
                    case Form1.SaveRegion.EUR:
                    case Form1.SaveRegion.USA:
                        if (j > 6)
                            break;
                        playerName += Form1.rawENChar[i];
                        break;
                    case Form1.SaveRegion.JPN:
                        if (j > 4)
                            break;
                        playerName += Form1.rawJPChar[i];
                        break;
                }
                j++;
            }
        }

        public Link GetPlayer() => player;

        public ushort[] GetPlayerNameRaw() => playerNameRaw;

        public void SetPlayerNameRaw(ushort[] _newName)
        {
            var j = 0;
            switch (saveRegion)
            {
                default:
                case Form1.SaveRegion.EUR:
                case Form1.SaveRegion.USA:
                    for (var i = 0x3D9; i <= 0x3E4; i += 2)
                    {
                        playerNameRaw[j] = _newName[j];
                        j++;
                    }
                    break;
                case Form1.SaveRegion.JPN:
                    for (var i = 0x3D9; i <= 0x3DF; i += 2)
                    {
                        playerNameRaw[j] = _newName[j];
                        j++;
                    }
                    break;
            }
        }

        public Form1.SaveRegion GetRegion() => saveRegion;

        public byte[] GetData() => data.ToArray();

        public void ValidateSave()
        {
            // This function signs the edited save to be valid to the game.
            // It sums up every 16-bit value in the save slot, except for the final one
            // And then it writes the calculated checksum to the final two bytes (in little-endian order)
            // Before it writes that value it calculates, it does this(roughly, not literally): UInt16 total = 0x5A5A - checksum
            ushort checksum = 0;
            for (var i = 0; i < 0x4fe; i += 2)
            {
                checksum += (ushort)((data[i + 1] << 8) | data[i]);
            }
            total_checksum = (ushort)(0x5A5A - checksum); // Calculate as 32-bit integer, then convert it to a 16-bit unsigned int

            data[0x4FE] = (byte)(total_checksum & 0xff);
            data[0x4FF] = (byte)(total_checksum >> 8);
        }

        public bool SaveIsValid()
        {
            // Tests if a loaded save is valid or not.
            switch (saveRegion)
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

            ushort checksum = 0;
            for (var i = 0x0; i < 0x500; i += 2)
            {
                var word = (ushort)((data[i + 1] << 8) | data[i]);
                checksum += word;
            }
            return checksum == 0x5A5A; // The save is valid if the checksum total is exactly 0x5A5A
        }

        public void UpdatePlayer()
        {
            // Take player's equipment and update the local data
            var itemsAndEquipment = player.GetItemsAndEquipmentArray();

            // Update pendant and crystal data before merging this save
            itemsAndEquipment[0x34] = pendants;
            itemsAndEquipment[0x3A] = crystals;
            itemsAndEquipment[0xF] = (byte)player.GetSelectedBottle();
            var len = itemsAndEquipment.Length;
            for (var i = 0x0; i < len; i++)
            {
                data[0x340 + i] = itemsAndEquipment[i];
            }
        }

        public byte GetPendants() => pendants;

        public byte GetCrystals() => crystals;

        public void TogglePendant(int _val)
        {
            switch (_val)
            {
                default:
                case Form1.greenPendant:
                    if (Form1.GetBit(pendants, Form1.greenPendant))
                        pendants &= 0xFB; // Turn it off if we already have the pendant
                    else
                        pendants |= 0x4; // Turn it on if we don't already have the pendant
                    break;
                case Form1.bluePendant:
                    if (Form1.GetBit(pendants, Form1.bluePendant))
                        pendants &= 0xFD; // Turn it off if we already have the pendant
                    else
                        pendants |= 0x2; // Turn it on if we don't already have the pendant
                    break;
                case Form1.redPendant:
                    if (Form1.GetBit(pendants, Form1.redPendant))
                        pendants &= 0xFE; // Turn it off if we already have the pendant
                    else
                        pendants |= 0x1; // Turn it on if we don't already have the pendant
                    break;
            }

            // Update pendant data for this save slot
            itemsAndEquipment[0x34] = pendants;
        }

        public void ToggleCrystal(int _val)
        {
            switch (_val)
            {
                default:
                case Form1.crystalPoD:
                    if (Form1.GetBit(crystals, Form1.crystalPoD))
                        crystals &= 0xFD; // Turn it off if we already have the crystal
                    else
                        crystals |= 0x2; // Turn it on if we don't already have the crystal
                    break;
                case Form1.crystalSP:
                    if (Form1.GetBit(crystals, Form1.crystalSP))
                        crystals &= 0xEF; // Turn it off if we already have the crystal
                    else
                        crystals |= 0x10; // Turn it on if we don't already have the crystal
                    break;
                case Form1.crystalSW:
                    if (Form1.GetBit(crystals, Form1.crystalSW))
                        crystals &= 0xBF; // Turn it off if we already have the crystal
                    else
                        crystals |= 0x40; // Turn it on if we don't already have the crystal
                    break;
                case Form1.crystalTT:
                    if (Form1.GetBit(crystals, Form1.crystalTT))
                        crystals &= 0xDF; // Turn it off if we already have the crystal
                    else
                        crystals |= 0x20; // Turn it on if we don't already have the crystal
                    break;
                case Form1.crystalIP:
                    if (Form1.GetBit(crystals, Form1.crystalIP))
                        crystals &= 0xFB; // Turn it off if we already have the crystal
                    else
                        crystals |= 0x4; // Turn it on if we don't already have the crystal
                    break;
                case Form1.crystalMM:
                    if (Form1.GetBit(crystals, Form1.crystalMM))
                        crystals &= 0xFE; // Turn it off if we already have the crystal
                    else
                        crystals |= 0x1; // Turn it on if we don't already have the crystal
                    break;
                case Form1.crystalTR:
                    if (Form1.GetBit(crystals, Form1.crystalTR))
                        crystals &= 0xF7; // Turn it off if we already have the crystal
                    else
                        crystals |= 0x8; // Turn it on if we don't already have the crystal
                    break;
            }

            // Update crystal data for this save slot
            itemsAndEquipment[0x3A] = crystals;
        }

        public string GetPlayerName() => playerName;

        public void SetPlayerName(string str) => playerName = str;

        public void SetData(byte[] in_data) => data = in_data.ToArray();

        public void ClearData()
        {
            Array.Clear(data, 0, data.Length - 2); // Invalidate the data by storing 0 to everything besides the checksum check
            isValid = false;
        }
    }
}