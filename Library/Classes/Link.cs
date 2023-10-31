// ReSharper disable InconsistentNaming
namespace Library.Classes;

[Serializable]
public class Link
{
    private readonly byte[] itemsAndEquipment;
    private readonly byte abilityFlags;
    private int heartPieces;
    private int heartContainers;
    private int currMagic;
    private readonly int bombsHeld;
    private readonly int arrowsHeld;
    private int currMagicUpgrade;
    private int currArrowUpgrades;
    private int currBombUpgrades;
    private int selectedBottle; // The bottle that is currently selected in the inventory screen

    // ReSharper disable once ParameterTypeCanBeEnumerable.Local
    public Link(byte[] itemsAndEquipmentInput)
    {
        itemsAndEquipment = [.. itemsAndEquipmentInput];
        abilityFlags = itemsAndEquipment[0x39];
        heartPieces = itemsAndEquipment[0x2B];
        heartContainers = itemsAndEquipment[0x2C];
        currMagic = itemsAndEquipment[0x2E];
        currBombUpgrades = itemsAndEquipment[0x30];
        bombsHeld = itemsAndEquipment[0x3];
        selectedBottle = itemsAndEquipment[0xF];

        var _bombsMax = currBombUpgrades switch
        {
            1 => 15,
            2 => 20,
            3 => 25,
            4 => 30,
            5 => 35,
            6 => 40,
            7 => 50,
            _ => 10,
        };

        if (bombsHeld > _bombsMax)
        {
            bombsHeld = _bombsMax;
        }

        currArrowUpgrades = itemsAndEquipment[0x31];
        arrowsHeld = itemsAndEquipment[0x37];

        var _arrowsMax = currArrowUpgrades switch
        {
            1 => 35,
            2 => 40,
            3 => 45,
            4 => 50,
            5 => 55,
            6 => 60,
            7 => 70,
            _ => 30,
        };

        if (arrowsHeld > _arrowsMax)
        {
            arrowsHeld = _arrowsMax;
        }

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

    public int GetRupeeValue() => itemsAndEquipment[0x23] << 8 | itemsAndEquipment[0x22];

    public int GetItemEquipment(int addr) => itemsAndEquipment[addr];
}
