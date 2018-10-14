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
        public SaveSlot(byte[] data_in)
        {
            data = data_in;
        }

        public byte[] GetData()
        {
            return data;
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