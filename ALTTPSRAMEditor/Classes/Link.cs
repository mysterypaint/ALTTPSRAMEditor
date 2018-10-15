using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALTTPSRAMEditor
{
    class Link
    {
        private byte[] itemsAndEquipment;

        public Link(byte[] itemsAndEquipmentInput)
        {
            itemsAndEquipment = itemsAndEquipmentInput.ToArray();
        }

        public void SetHasItemEquipment(int addr, byte val)
        {
            itemsAndEquipment[addr] = val;
        }

        public int GetItemEquipment(int addr)
        {
            return itemsAndEquipment[addr];
        }
    }
}
