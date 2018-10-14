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
        public SRAM(byte[] data_in)
        {
            data = data_in;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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
