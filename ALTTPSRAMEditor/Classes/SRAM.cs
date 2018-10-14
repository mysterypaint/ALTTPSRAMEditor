using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALTTPSRAMEditor
{
    class SRAM
    {
        private byte[] data;
        private const int slot1 = 0x0;
        private const int slot1m = 0xF00;
        private const int slot2 = 0x500;
        private const int slot2m = 0x1400;
        private const int slot3 = 0xA00;
        private const int slot3m = 0x1900;
        private byte[] copyData = new byte[0xF00];
    
        //Addresses $1E00 to $1FFE in SRAM are not used.
        private const int mempointer = 0x1FFE; // used as the offset to know where the memory will be stored in the SRAM (02 is the first file, 04 the second and 06 the third) 
        private int currsave = 00; // 00 - No File, 02 - File 1, 04 - File 2, 06 - File 3

        /*
         * These offsets directly correspond to $7E:F for a particular save file is being played.
         * When the game is finished it writes the information into bank $70 in the corresponding slot + offsets presented here.
         * (e.g. For the second save file, the information will be saved to $70:0500 to $70:09FF, and mirrored at $70:1400 to $70:18FF.)
         */

        public SRAM(byte[] data_in)
        {
            data = data_in;

            copyData[0] = 0xFE; // If first byte is a 0xFE, we never copied anything.
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public void CopyFile(int fileSlot)
        {
            int j = 0;
            int slotOffset = 0x0;

            switch (fileSlot)
            {
                default:
                case 1:
                    slotOffset = slot1;
                    break;
                case 2:
                    slotOffset = slot2;
                    break;
                case 3:
                    slotOffset = slot3;
                    break;
            }
            for (int i = 0; i < 0x500; i++)
            {
                copyData[i] = data[slotOffset + j];
                j++;
            }
        }
        
        public void WriteFile(int fileSlot)
        {
            int j = 0;
            int slotOffset = 0x0;

            switch (fileSlot)
            {
                default:
                case 1:
                    slotOffset = slot1;
                    break;
                case 2:
                    slotOffset = slot2;
                    break;
                case 3:
                    slotOffset = slot3;
                    break;
            }
            for (int i = 0; i < 0x500; i++)
            {
                data[slotOffset + j] = copyData[i];
                j++;
            }
        }

        public void EraseFile(int fileSlot)
        {
            int slotOffset = 0x0;
            switch (fileSlot)
            {
                default:
                    break;
                case 1:
                    slotOffset = slot1;
                    break;
                case 2:
                    slotOffset = slot2;
                    break;
                case 3:
                    slotOffset = slot3;
                    break;
            }
            for (int i = 0x0; i < 0x500; i++) {
                data[slotOffset + i] = 0x0; // Clear the save data
                data[0xF00 + slotOffset + i] = 0x0; // Clear the mirror slot data, too
            }
        }

        public byte[] GetSaveData()
        {
            return data;
        }

        public void ClearCopyData()
        {
            for (int i = 0; i < copyData.Length; i++)
                copyData[i] = 0x0;
            copyData[0] = 0xFE;
        }

        /*
        private String HexBlock(int start, int end)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(f_in));
            
            String str = null;
            for (int i = start; i <= end; i++)
            {
                br.BaseStream.Position = i;
                str = br.ReadByte().ToString("X2");
            }
            return str;
        }*/
    }
}
