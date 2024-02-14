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
        abilityFlags = itemsAndEquipment[AbilityFlagsAddress];
        heartPieces = itemsAndEquipment[HeartPiecesAddress];
        heartContainers = itemsAndEquipment[MaxHeartsAddress];
        currMagic = itemsAndEquipment[MagicPowerAddress];
        currBombUpgrades = itemsAndEquipment[BombUpgradesAddress];
        bombsHeld = itemsAndEquipment[BombCountAddress];
        selectedBottle = itemsAndEquipment[BottleAddress];

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

        currArrowUpgrades = itemsAndEquipment[ArrowUpgradesAddress];
        arrowsHeld = itemsAndEquipment[ArrowCountAddress];

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

        currMagicUpgrade = itemsAndEquipment[MagicUpgradesAddress];
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
        itemsAndEquipment[ArrowUpgradesAddress] = (byte)_val;
    }

    public void SetCurrBombUpgrades(int _val)
    {
        currBombUpgrades = _val;
        itemsAndEquipment[BombUpgradesAddress] = (byte)_val;
    }

    public void SetHasItemEquipment(int addr, byte val) => itemsAndEquipment[addr] = val;

    public void SetHeartContainers(int val)
    {
        heartContainers = val;
        itemsAndEquipment[MaxHeartsAddress] = (byte)val; // Max HP
        itemsAndEquipment[CurrHeartsAddress] = (byte)val; // Curr HP
    }

    public void SetMagic(int val)
    {
        currMagic = val;
        itemsAndEquipment[MagicPowerAddress] = (byte)val; // Max MP
        itemsAndEquipment[MagicLeftToFillAddress] = 0;// (byte)val; // MP left to fill
    }

    public void SetMagicUpgrade(int val)
    {
        currMagicUpgrade = val;
        itemsAndEquipment[MagicUpgradesAddress] = (byte)val; // Set the Magic Upgrade value
    }

    public void IncrementHeartPieces()
    {
        heartPieces++;
        itemsAndEquipment[HeartPiecesAddress] = (byte)(heartPieces % 4);
    }

    public void DecrementHeartPieces()
    {
        heartPieces--;
        itemsAndEquipment[HeartPiecesAddress] = (byte)(heartPieces % 4);
    }

    public int GetHeartPieces() => heartPieces;

    public int GetHeartContainers() => heartContainers;

    public int GetCurrMagic() => currMagic;

    public int GetCurrMagicUpgrade() => currMagicUpgrade;

    public void SetRupeesValue(ushort val)
    {
        var byteArray = BitConverter.GetBytes(val);
        byteArray.CopyTo(itemsAndEquipment, RupeesAddress);
    }

    public ushort GetRupeesValue() =>
        BitConverter.ToUInt16(itemsAndEquipment, RupeesAddress);

    public int GetItemEquipment(int addr) => itemsAndEquipment[addr];
}
