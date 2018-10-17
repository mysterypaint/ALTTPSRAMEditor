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
        private byte abilityFlags;

        public Link(byte[] itemsAndEquipmentInput)
        {
            itemsAndEquipment = itemsAndEquipmentInput.ToArray();
            abilityFlags = itemsAndEquipment[0x39];
        }

        public byte[] GetItemsAndEquipmentArray()
        {
            return itemsAndEquipment;
        }

        public byte GetAbilityFlags()
        {
            return abilityFlags;
        }

        public void SetHasItemEquipment(int addr, byte val)
        {
            itemsAndEquipment[addr] = val;
        }

        public void SetRupees(int val)
        {
            var bytes = new byte[2];
            bytes[0] = (byte)(val >> 8);  // 0x00
            bytes[1] = (byte)val;         // 0x10

            //var result = (bytes[0] << 8) | bytes[1];

            // Set actual rupee value
            itemsAndEquipment[0x20] = bytes[1];
            itemsAndEquipment[0x21] = bytes[0];

            // Set rupee "lag counter" value
            itemsAndEquipment[0x22] = bytes[1];
            itemsAndEquipment[0x23] = bytes[0];
        }

        public int GetRupeeValue() {
            return (itemsAndEquipment[0x23] << 8) | itemsAndEquipment[0x22];
        }

        public int GetItemEquipment(int addr)
        {
            return itemsAndEquipment[addr];
        }
    }
}
