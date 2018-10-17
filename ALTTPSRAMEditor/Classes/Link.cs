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
        private int heartPieces = 0;
        private int heartContainers = 3;
        private int currHealth = 3;
        private int currMagic = 0;
        private int currMagicUpgrade = 0;

        public Link(byte[] itemsAndEquipmentInput)
        {
            itemsAndEquipment = itemsAndEquipmentInput.ToArray();
            abilityFlags = itemsAndEquipment[0x39];
            heartPieces = itemsAndEquipment[0x2B];
            heartContainers = itemsAndEquipment[0x2C];
            currHealth = itemsAndEquipment[0x2D];
            currMagic = itemsAndEquipment[0x2E];
            currMagicUpgrade = itemsAndEquipment[0x3B];
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

        public void SetHeartContainers(int val)
        {
            heartContainers = val;
            itemsAndEquipment[0x2C] = (byte)val; // Max HP
            itemsAndEquipment[0x2D] = (byte)val; // Curr HP
        }

        public void SetMagic(int val)
        {
            currMagic = val;
            itemsAndEquipment[0x2E] = (byte)val; // Max MP
            itemsAndEquipment[0x33] = 0;// (byte)val; // MP left to fill
        }

        public void SetMagicUpgrade(int val)
        {
            currMagicUpgrade = val;
            itemsAndEquipment[0x3B] = (byte) val; // Set the Magic Upgrade value
        }

        public void IncrementHeartPieces()
        {
            heartPieces++;
            itemsAndEquipment[0x2B] = (byte) (heartPieces % 4);
        }

        public void DecrementHeartPieces()
        {
            heartPieces--;
            itemsAndEquipment[0x2B] = (byte) (heartPieces % 4);
        }

        public int GetHeartPieces()
        {
            return heartPieces;
        }

        public int GetHeartContainers()
        {
            return heartContainers;
        }

        public int GetCurrMagic()
        {
            return currMagic;
        }

        public int GetCurrMagicUpgrade()
        {
            return currMagicUpgrade;
        }

        public void SetRupees(int val)
        {
            var bytes = new byte[2];
            bytes[0] = (byte)(val >> 8); // 0x00
            bytes[1] = (byte)val; // 0x10

            //var result = (bytes[0] << 8) | bytes[1];

            // Set actual rupee value
            itemsAndEquipment[0x20] = bytes[1];
            itemsAndEquipment[0x21] = bytes[0];

            // Set rupee "lag counter" value
            itemsAndEquipment[0x22] = bytes[1];
            itemsAndEquipment[0x23] = bytes[0];
        }

        public int GetRupeeValue()
        {
            return (itemsAndEquipment[0x23] << 8) | itemsAndEquipment[0x22];
        }

        public int GetItemEquipment(int addr)
        {
            return itemsAndEquipment[addr];
        }
    }
}