// ReSharper disable InconsistentNaming
namespace Library.Classes;

public class SaveSlot
{
    private byte[] Data;
    private string PlayerName = string.Empty;
    private ushort[] PlayerNameRaw;
    private ushort TotalChecksum;
    private readonly Link Player;
    private byte Pendants;
    private byte Crystals;
    private bool IsValid;
    private int SlotIndex;
    private readonly TextCharacterData TextCharacterData;
    private readonly SaveRegion SaveRegion;
    private readonly byte[] ItemsAndEquipment;

    // ReSharper disable once ParameterTypeCanBeEnumerable.Local
    public SaveSlot(byte[] data_in, int _slot, TextCharacterData textCharacterData)
    {
        // Import this save slot's data from the larger global save data
        Data = [.. data_in];
        SlotIndex = _slot;
        this.TextCharacterData = textCharacterData;
        PlayerNameRaw = new ushort[6];

        // Determine which region this save comes from
        SaveRegion = Data[0x3E5] == 0xAA && Data[0x3E6] == 0x55
            ? SaveRegion.USA
            : Data[0x3E1] == 0xAA && Data[0x3E2] == 0x55
                ? SaveRegion.JPN
                : SaveRegion.EUR;

        IsValid = SaveIsValid();
        // Copy global save data's Item&Equipment data to this Save Slot

        ItemsAndEquipment = new byte[0x4B];

        for (var i = 0x0; i < ItemsAndEquipment.Length; i++)
        {
            ItemsAndEquipment[i] = Data[0x340 + i];
        }

        // Copy pendants and crystals to a private variable for this save slot
        Pendants = ItemsAndEquipment[0x34];
        Crystals = ItemsAndEquipment[0x3A];

        // Initialize a player object upon creating this save slot.
        Player = new Link(ItemsAndEquipment);

        GetRawPlayerName();
    }

    public void ResetFileDeaths(bool showOnFileSelect)
    {
        var (_deathCounterAddr, _liveSaveCounterAddr, _deathTotalsTableAddr) = SaveRegion switch
        {
            SaveRegion.JPN => (0x401, 0x3FF, 0x3E3),
            _ => (0x405, 0x403, 0x3E7)
        };
        for (var i = 0x0; i < 0x1B; i += 2)
        {
            Data[_deathTotalsTableAddr + i] = 0x0;
            Data[_deathTotalsTableAddr + i + 1] = 0x0;
        }

        Data[_liveSaveCounterAddr] = 0x0;
        Data[_liveSaveCounterAddr + 1] = 0x0;

        if (showOnFileSelect)
        {
            Data[_deathCounterAddr] = 0x0;
            Data[_deathCounterAddr + 1] = 0x0;
        }
        else
        {
            Data[_deathCounterAddr] = 0xFF;
            Data[_deathCounterAddr + 1] = 0xFF;
        }

        if (IsValid)
        {
            ValidateSave();
        }
    }

    public void CommitPlayerName()
    {
        // Set the actual player name in data[] just before writing to SRAM
        var j = 0;

        switch (SaveRegion)
        {
            default:
            case SaveRegion.USA:
            case SaveRegion.EUR:
                for (var i = 0x3D9; i <= 0x3E4; i += 2)
                {
                    Data[i] = (byte)(PlayerNameRaw[j] & 0xff);
                    Data[i + 1] = (byte)(PlayerNameRaw[j] >> 8);
                    j++;
                }
                break;
            case SaveRegion.JPN:
                for (var i = 0x3D9; i <= 0x3E0; i += 2)
                {
                    Data[i] = (byte)(PlayerNameRaw[j] & 0xff);
                    Data[i + 1] = (byte)(PlayerNameRaw[j] >> 8);
                    j++;
                }
                break;
        }
    }

    public void SetSaveSlot(int _slot) => SlotIndex = _slot;

    public void SetIsValid(bool _val) => IsValid = _val;

    public bool GetIsValid() => IsValid;

    public override string ToString() => SlotIndex.ToString();

    private void GetRawPlayerName()
    {
        if (!SaveIsValid())
        {
            return;
        }

        var j = 0;

        switch (SaveRegion)
        {
            default:
            case SaveRegion.EUR:
            case SaveRegion.USA:
                for (var i = 0x3D9; i <= 0x3E4; i += 2)
                {
                    PlayerNameRaw[j] = (ushort)(Data[i + 1] << 8 | Data[i]);
                    j++;
                }
                break;
            case SaveRegion.JPN:
                PlayerNameRaw = new ushort[4];
                for (var i = 0x3D9; i < 0x3E1; i += 2)
                {
                    PlayerNameRaw[j] = (ushort)(Data[i + 1] << 8 | Data[i]);
                    j++;
                }
                break;
        }
        ConvertPlayerNameRawToString(PlayerNameRaw);
    }

    // ReSharper disable once ParameterTypeCanBeEnumerable.Local
    private void ConvertPlayerNameRawToString(ushort[] _playerNameRaw)
    {
        var j = 1; // Char counter
        foreach (var i in _playerNameRaw)
        {
            switch (SaveRegion)
            {
                case SaveRegion.EUR:
                case SaveRegion.USA:
                    if (j > 6)
                    {
                        break;
                    }

                    PlayerName += TextCharacterData.RawEnChar[i];
                    break;
                case SaveRegion.JPN:
                    if (j > 4)
                    {
                        break;
                    }

                    PlayerName += TextCharacterData.RawJpChar[i];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_playerNameRaw));
            }
            j++;
        }
    }

    public Link GetPlayer() => Player;

    public ushort[] GetPlayerNameRaw() => PlayerNameRaw;

    public void SetPlayerNameRaw(ushort[] _newName)
    {
        var j = 0;
        switch (SaveRegion)
        {
            default:
            case SaveRegion.EUR:
            case SaveRegion.USA:
                for (var i = 0x3D9; i <= 0x3E4; i += 2)
                {
                    PlayerNameRaw[j] = _newName[j];
                    j++;
                }
                break;
            case SaveRegion.JPN:
                for (var i = 0x3D9; i <= 0x3DF; i += 2)
                {
                    PlayerNameRaw[j] = _newName[j];
                    j++;
                }
                break;
        }
    }

    public SaveRegion GetRegion() => SaveRegion;

    public byte[] GetData() => [.. Data];

    public void ValidateSave()
    {
        // This function signs the edited save to be valid to the game.
        // It sums up every 16-bit value in the save slot, except for the final one
        // And then it writes the calculated checksum to the final two bytes (in little-endian order)
        // Before it writes that value it calculates, it does this(roughly, not literally): UInt16 total = 0x5A5A - checksum
        ushort checksum = 0;
        for (var i = 0; i < 0x4fe; i += 2)
        {
            checksum += (ushort)(Data[i + 1] << 8 | Data[i]);
        }
        TotalChecksum = (ushort)(0x5A5A - checksum); // Calculate as 32-bit integer, then convert it to a 16-bit unsigned int

        Data[0x4FE] = (byte)(TotalChecksum & 0xff);
        Data[0x4FF] = (byte)(TotalChecksum >> 8);
    }

    public bool SaveIsValid()
    {
        // Tests if a loaded save is valid or not.
        switch (SaveRegion)
        {
            case SaveRegion.EUR:
            case SaveRegion.USA:
                if (Data[0x3E5] != 0xAA || Data[0x3E6] != 0x55)
                {
                    return false;
                }

                break;
            case SaveRegion.JPN:
                if (Data[0x3E1] != 0xAA || Data[0x3E2] != 0x55)
                {
                    return false;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(SaveRegion));
        }

        ushort checksum = 0;
        for (var i = 0x0; i < 0x500; i += 2)
        {
            var word = (ushort)(Data[i + 1] << 8 | Data[i]);
            checksum += word;
        }
        return checksum == 0x5A5A; // The save is valid if the checksum total is exactly 0x5A5A
    }

    public void UpdatePlayer()
    {
        // Take player's equipment and update the local data
        var _itemsAndEquipment = Player.GetItemsAndEquipmentArray();

        // Update pendant and crystal data before merging this save
        _itemsAndEquipment[0x34] = Pendants;
        _itemsAndEquipment[0x3A] = Crystals;
        _itemsAndEquipment[0xF] = (byte)Player.GetSelectedBottle();
        var len = _itemsAndEquipment.Length;
        for (var i = 0x0; i < len; i++)
        {
            Data[0x340 + i] = _itemsAndEquipment[i];
        }
    }

    public static bool GetBit(byte b, int bitNumber)
    {
        bitNumber++;
        return (b & 1 << bitNumber - 1) != 0;
    }

    public byte GetPendants() => Pendants;

    public byte GetCrystals() => Crystals;

    public void TogglePendant(int _val)
    {
        switch (_val)
        {
            default:
            // ReSharper disable once RedundantCaseLabel
            case GreenPendantAddress:
                if (GetBit(Pendants, GreenPendantAddress))
                {
                    Pendants &= 0xFB; // Turn it off if we already have the pendant
                }
                else
                {
                    Pendants |= 0x4; // Turn it on if we don't already have the pendant
                }

                break;
            case BluePendantAddress:
                if (GetBit(Pendants, BluePendantAddress))
                {
                    Pendants &= 0xFD; // Turn it off if we already have the pendant
                }
                else
                {
                    Pendants |= 0x2; // Turn it on if we don't already have the pendant
                }

                break;
            case RedPendantAddress:
                if (GetBit(Pendants, RedPendantAddress))
                {
                    Pendants &= 0xFE; // Turn it off if we already have the pendant
                }
                else
                {
                    Pendants |= 0x1; // Turn it on if we don't already have the pendant
                }

                break;
        }

        // Update pendant data for this save slot
        ItemsAndEquipment[0x34] = Pendants;
    }

    public void ToggleCrystal(int _val)
    {
        switch (_val)
        {
            default:
            // ReSharper disable once RedundantCaseLabel
            case CrystalPoD:
                if (GetBit(Crystals, CrystalPoD))
                {
                    Crystals &= 0xFD; // Turn it off if we already have the crystal
                }
                else
                {
                    Crystals |= 0x2; // Turn it on if we don't already have the crystal
                }

                break;
            case CrystalSPAddress:
                if (GetBit(Crystals, CrystalSPAddress))
                {
                    Crystals &= 0xEF; // Turn it off if we already have the crystal
                }
                else
                {
                    Crystals |= 0x10; // Turn it on if we don't already have the crystal
                }

                break;
            case CrystalSWAddress:
                if (GetBit(Crystals, CrystalSWAddress))
                {
                    Crystals &= 0xBF; // Turn it off if we already have the crystal
                }
                else
                {
                    Crystals |= 0x40; // Turn it on if we don't already have the crystal
                }

                break;
            case CrystalTTAddress:
                if (GetBit(Crystals, CrystalTTAddress))
                {
                    Crystals &= 0xDF; // Turn it off if we already have the crystal
                }
                else
                {
                    Crystals |= 0x20; // Turn it on if we don't already have the crystal
                }

                break;
            case CrystalIPAddress:
                if (GetBit(Crystals, CrystalIPAddress))
                {
                    Crystals &= 0xFB; // Turn it off if we already have the crystal
                }
                else
                {
                    Crystals |= 0x4; // Turn it on if we don't already have the crystal
                }

                break;
            case CrystalMMAddress:
                if (GetBit(Crystals, CrystalMMAddress))
                {
                    Crystals &= 0xFE; // Turn it off if we already have the crystal
                }
                else
                {
                    Crystals |= 0x1; // Turn it on if we don't already have the crystal
                }

                break;
            case CrystalTRAddress:
                if (GetBit(Crystals, CrystalTRAddress))
                {
                    Crystals &= 0xF7; // Turn it off if we already have the crystal
                }
                else
                {
                    Crystals |= 0x8; // Turn it on if we don't already have the crystal
                }

                break;
        }

        // Update crystal data for this save slot
        ItemsAndEquipment[0x3A] = Crystals;
    }

    public string GetPlayerName() => PlayerName;

    public void SetPlayerName(string str) => PlayerName = str;

    // ReSharper disable once ParameterTypeCanBeEnumerable.Global
    public void SetData(byte[] in_data) => Data = [.. in_data];

    public void ClearData()
    {
        Array.Clear(Data, 0, Data.Length - 2); // Invalidate the data by storing 0 to everything besides the checksum check
        IsValid = false;
    }
}
