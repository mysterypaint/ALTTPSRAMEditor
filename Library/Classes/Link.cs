namespace Library.Classes;

[Serializable]
public class Link
{
    private readonly byte[] itemsAndEquipment;
    private readonly byte abilityFlags;
    private int heartPieces = 0;
    private int heartContainers = 3;
    private readonly int currHealth = 3;
    private int currMagic = 0;
    private readonly int bombsHeld = 0;
    private readonly int arrowsHeld = 0;
    private int currMagicUpgrade = 0;
    private int currArrowUpgrades = 0;
    private int currBombUpgrades = 0;
    private int selectedBottle = 1; // The bottle that is currently selected in the inventory screen

    public Link(byte[] itemsAndEquipmentInput)
    {
        itemsAndEquipment = itemsAndEquipmentInput.ToArray();
        abilityFlags = itemsAndEquipment[0x39];
        heartPieces = itemsAndEquipment[0x2B];
        heartContainers = itemsAndEquipment[0x2C];
        currHealth = itemsAndEquipment[0x2D];
        currMagic = itemsAndEquipment[0x2E];
        currBombUpgrades = itemsAndEquipment[0x30];
        bombsHeld = itemsAndEquipment[0x3];
        selectedBottle = itemsAndEquipment[0xF];

        var _bombsMax = 10;
        switch (currBombUpgrades)
        {
            default:
                break;
            case 1:
                _bombsMax = 15;
                break;
            case 2:
                _bombsMax = 20;
                break;
            case 3:
                _bombsMax = 25;
                break;
            case 4:
                _bombsMax = 30;
                break;
            case 5:
                _bombsMax = 35;
                break;
            case 6:
                _bombsMax = 40;
                break;
            case 7:
                _bombsMax = 50;
                break;
        }
        if (bombsHeld > _bombsMax)
            bombsHeld = _bombsMax;

        currArrowUpgrades = itemsAndEquipment[0x31];
        arrowsHeld = itemsAndEquipment[0x37];

        var _arrowsMax = 30;
        switch (currBombUpgrades)
        {
            default:
                break;
            case 1:
                _arrowsMax = 35;
                break;
            case 2:
                _arrowsMax = 40;
                break;
            case 3:
                _arrowsMax = 45;
                break;
            case 4:
                _arrowsMax = 50;
                break;
            case 5:
                _arrowsMax = 55;
                break;
            case 6:
                _arrowsMax = 60;
                break;
            case 7:
                _arrowsMax = 70;
                break;
        }
        if (arrowsHeld > _arrowsMax)
            arrowsHeld = _arrowsMax;
        currMagicUpgrade = itemsAndEquipment[0x3B];
    }

    public byte[] GetItemsAndEquipmentArray() => itemsAndEquipment;

    public byte GetAbilityFlags() => abilityFlags;

    public int GetSelectedBottle() => selectedBottle;

    public void SetSelectedBottle(int _val) => selectedBottle = _val;

    public int GetHeldBombs() => bombsHeld;

    public int GetHeldArrows() => arrowsHeld;

    public int GetCurrBombUpgrades() => currBombUpgrades;

    public int GetCurrArrowUpgrades() => currArrowUpgrades;

    public void SetCurrArrowUpgrades(int _val)
    {
        currArrowUpgrades = _val;
        itemsAndEquipment[0x31] = (byte)_val;
    }

    public void SetCurrBombUpgrades(int _val)
    {
        currBombUpgrades = _val;
        itemsAndEquipment[0x30] = (byte)_val;
    }

    public void SetHasItemEquipment(int addr, byte val) => itemsAndEquipment[addr] = val;

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
        itemsAndEquipment[0x3B] = (byte)val; // Set the Magic Upgrade value
    }

    public void IncrementHeartPieces()
    {
        heartPieces++;
        itemsAndEquipment[0x2B] = (byte)(heartPieces % 4);
    }

    public void DecrementHeartPieces()
    {
        heartPieces--;
        itemsAndEquipment[0x2B] = (byte)(heartPieces % 4);
    }

    public int GetHeartPieces() => heartPieces;

    public int GetHeartContainers() => heartContainers;

    public int GetCurrMagic() => currMagic;

    public int GetCurrMagicUpgrade() => currMagicUpgrade;

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

    public int GetRupeeValue() => (itemsAndEquipment[0x23] << 8) | itemsAndEquipment[0x22];

    public int GetItemEquipment(int addr) => itemsAndEquipment[addr];
}
