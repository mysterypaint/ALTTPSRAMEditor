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

        public SaveSlot(byte[] data_in)
        {
            data = data_in;
            for (int i = 0x3D9; i <= 0x3E4; i+=2)
            {
                char outChar = '0';
                switch (data[i])
                {
                    default:
                    case 0x0:
                        outChar = 'A';
                        break;
                    case 0x1:
                        outChar = 'B';
                        break;
                    case 0x2:
                        outChar = 'C';
                        break;
                    case 0x3:
                        outChar = 'D';
                        break;
                    case 0x4:
                        outChar = 'E';
                        break;
                    case 0x5:
                        outChar = 'F';
                        break;
                    case 0x6:
                        outChar = 'G';
                        break;
                    case 0x7:
                        outChar = 'H';
                        break;
                    case 0x8:
                        outChar = 'I';
                        break;
                    case 0x9:
                        outChar = 'J';
                        break;
                    case 0xA:
                        outChar = 'K';
                        break;
                    case 0xB:
                        outChar = 'L';
                        break;
                    case 0xC:
                        outChar = 'M';
                        break;
                    case 0xD:
                        outChar = 'N';
                        break;
                    case 0xE:
                        outChar = 'O';
                        break;
                    case 0xF:
                        outChar = 'P';
                        break;
                    case 0x10:
                        outChar = '?';
                        break;
                    case 0x20:
                        outChar = 'Q';
                        break;
                    case 0x21:
                        outChar = 'R';
                        break;
                    case 0x22:
                        outChar = 'S';
                        break;
                    case 0x23:
                        outChar = 'T';
                        break;
                    case 0x24:
                        outChar = 'U';
                        break;
                    case 0x25:
                        outChar = 'V';
                        break;
                    case 0x26:
                        outChar = 'W';
                        break;
                    case 0x27:
                        outChar = 'X';
                        break;
                    case 0x28:
                        outChar = 'Y';
                        break;
                    case 0x29:
                        outChar = 'Z';
                        break;
                    case 0xA9:
                    case 0xA7:
                        outChar = ' ';
                        break;
                }
                playerName += outChar;
            }
            
        }

        public byte[] GetData()
        {
            return data;
        }

        public bool IsEmpty()
        {
            for (int i = 0x1; i < 0x500; i++)
            {
                if (data[i] != 0x0)
                    return false;
            }
            return true;
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
            data = in_data;
        }

        public void ClearData()
        {
            Array.Clear(data, 0, data.Length);
        }
    }
}