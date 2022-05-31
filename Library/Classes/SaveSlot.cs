namespace Library.Classes;

[Serializable]
public class SaveSlot
{
    private byte[] data;
    private string playerName = "";
    private ushort[] playerNameRaw;
    private ushort total_checksum = 0;
    private readonly Link player;
    private byte pendants;
    private byte crystals;
    private bool isValid;
    private int slotIndex;
    private readonly Enums.SaveRegion saveRegion;
    private readonly byte[] itemsAndEquipment;

    public SaveSlot(byte[] data_in, int _slot)
    {
        // Import this save slot's data from the larger global save data
        data = data_in.ToArray();
        slotIndex = _slot;
        playerNameRaw = new ushort[6];

        // Determine which region this save comes from
        saveRegion = data[0x3E5] == 0xAA && data[0x3E6] == 0x55
            ? Enums.SaveRegion.USA
            : data[0x3E1] == 0xAA && data[0x3E2] == 0x55 ? Enums.SaveRegion.JPN : Enums.SaveRegion.EUR;

        isValid = SaveIsValid();
        // Copy global save data's Item&Equipment data to this Save Slot

        itemsAndEquipment = new byte[0x4B];

        for (var i = 0x0; i < itemsAndEquipment.Length; i++)
        {
            itemsAndEquipment[i] = data[0x340 + i];
        }

        // Copy pendants and crystals to a private variable for this save slot
        pendants = itemsAndEquipment[0x34];
        crystals = itemsAndEquipment[0x3A];

        // Initialize a player object upon creating this save slot.
        player = new Link(itemsAndEquipment);

        GetRawPlayerName();
    }

    public void ResetFileDeaths(bool showOnFileSelect)
    {
        int _deathCounterAddr;
        int _liveSaveCounterAddr;
        int _deathTotalsTableAddr;
        switch (saveRegion)
        {
            default:
            case Enums.SaveRegion.EUR:
            case Enums.SaveRegion.USA:
                _deathCounterAddr = 0x405;
                _liveSaveCounterAddr = 0x403;
                _deathTotalsTableAddr = 0x3E7;
                break;
            case Enums.SaveRegion.JPN:
                _deathCounterAddr = 0x401;
                _liveSaveCounterAddr = 0x3FF;
                _deathTotalsTableAddr = 0x3E3;
                break;
        }
        for (var i = 0x0; i < 0x1B; i += 2)
        {
            data[_deathTotalsTableAddr + i] = 0x0;
            data[_deathTotalsTableAddr + i + 1] = 0x0;
        }

        data[_liveSaveCounterAddr] = 0x0;
        data[_liveSaveCounterAddr + 1] = 0x0;

        if (showOnFileSelect)
        {
            data[_deathCounterAddr] = 0x0;
            data[_deathCounterAddr + 1] = 0x0;
        }
        else
        {
            data[_deathCounterAddr] = 0xFF;
            data[_deathCounterAddr + 1] = 0xFF;
        }

        if (isValid)
        {
            ValidateSave();
        }
    }

    public void CommitPlayerName()
    {
        // Set the actual player name in data[] just before writing to SRAM
        var j = 0;

        switch (saveRegion)
        {
            default:
            case Enums.SaveRegion.USA:
            case Enums.SaveRegion.EUR:
                for (var i = 0x3D9; i <= 0x3E4; i += 2)
                {
                    data[i] = (byte)(playerNameRaw[j] & 0xff);
                    data[i + 1] = (byte)(playerNameRaw[j] >> 8);
                    j++;
                }
                break;
            case Enums.SaveRegion.JPN:
                for (var i = 0x3D9; i <= 0x3E0; i += 2)
                {
                    data[i] = (byte)(playerNameRaw[j] & 0xff);
                    data[i + 1] = (byte)(playerNameRaw[j] >> 8);
                    j++;
                }
                break;
        }
    }

    public void SetSaveSlot(int _slot) => slotIndex = _slot;

    public void SetIsValid(bool _val) => isValid = _val;

    public bool GetIsValid() => isValid;

    public override string ToString() => slotIndex.ToString();
    private void GetRawPlayerName()
    {
        if (!SaveIsValid())
        {
            return;
        }

        var j = 0;

        switch (saveRegion)
        {
            default:
            case Enums.SaveRegion.EUR:
            case Enums.SaveRegion.USA:
                for (var i = 0x3D9; i <= 0x3E4; i += 2)
                {
                    playerNameRaw[j] = (ushort)((data[i + 1] << 8) | data[i]);
                    j++;
                }
                break;
            case Enums.SaveRegion.JPN:
                playerNameRaw = new ushort[4];
                for (var i = 0x3D9; i < 0x3E1; i += 2)
                {
                    playerNameRaw[j] = (ushort)((data[i + 1] << 8) | data[i]);
                    j++;
                }
                break;
        }
        ConvertPlayerNameRawToString(playerNameRaw);
    }

    private void ConvertPlayerNameRawToString(ushort[] playerNameRaw)
    {
        var j = 1; // Char counter
        foreach (var i in playerNameRaw)
        {
            switch (saveRegion)
            {
                case Enums.SaveRegion.EUR:
                case Enums.SaveRegion.USA:
                    if (j > 6)
                    {
                        break;
                    }

                    playerName += AppState.rawEnChar[i];
                    break;
                case Enums.SaveRegion.JPN:
                    if (j > 4)
                    {
                        break;
                    }

                    playerName += AppState.rawJpChar[i];
                    break;
            }
            j++;
        }
    }

    public Link GetPlayer() => player;

    public ushort[] GetPlayerNameRaw() => playerNameRaw;

    public void SetPlayerNameRaw(ushort[] _newName)
    {
        var j = 0;
        switch (saveRegion)
        {
            default:
            case Enums.SaveRegion.EUR:
            case Enums.SaveRegion.USA:
                for (var i = 0x3D9; i <= 0x3E4; i += 2)
                {
                    playerNameRaw[j] = _newName[j];
                    j++;
                }
                break;
            case Enums.SaveRegion.JPN:
                for (var i = 0x3D9; i <= 0x3DF; i += 2)
                {
                    playerNameRaw[j] = _newName[j];
                    j++;
                }
                break;
        }
    }

    public Enums.SaveRegion GetRegion() => saveRegion;

    public byte[] GetData() => data.ToArray();

    public void ValidateSave()
    {
        // This function signs the edited save to be valid to the game.
        // It sums up every 16-bit value in the save slot, except for the final one
        // And then it writes the calculated checksum to the final two bytes (in little-endian order)
        // Before it writes that value it calculates, it does this(roughly, not literally): UInt16 total = 0x5A5A - checksum
        ushort checksum = 0;
        for (var i = 0; i < 0x4fe; i += 2)
        {
            checksum += (ushort)((data[i + 1] << 8) | data[i]);
        }
        total_checksum = (ushort)(0x5A5A - checksum); // Calculate as 32-bit integer, then convert it to a 16-bit unsigned int

        data[0x4FE] = (byte)(total_checksum & 0xff);
        data[0x4FF] = (byte)(total_checksum >> 8);
    }

    public bool SaveIsValid()
    {
        // Tests if a loaded save is valid or not.
        switch (saveRegion)
        {
            case Enums.SaveRegion.EUR:
            case Enums.SaveRegion.USA:
                if (data[0x3E5] != 0xAA || data[0x3E6] != 0x55)
                {
                    return false;
                }

                break;
            case Enums.SaveRegion.JPN:
                if (data[0x3E1] != 0xAA || data[0x3E2] != 0x55)
                {
                    return false;
                }

                break;
        }

        ushort checksum = 0;
        for (var i = 0x0; i < 0x500; i += 2)
        {
            var word = (ushort)((data[i + 1] << 8) | data[i]);
            checksum += word;
        }
        return checksum == 0x5A5A; // The save is valid if the checksum total is exactly 0x5A5A
    }

    public void UpdatePlayer()
    {
        // Take player's equipment and update the local data
        var itemsAndEquipment = player.GetItemsAndEquipmentArray();

        // Update pendant and crystal data before merging this save
        itemsAndEquipment[0x34] = pendants;
        itemsAndEquipment[0x3A] = crystals;
        itemsAndEquipment[0xF] = (byte)player.GetSelectedBottle();
        var len = itemsAndEquipment.Length;
        for (var i = 0x0; i < len; i++)
        {
            data[0x340 + i] = itemsAndEquipment[i];
        }
    }

    public static bool GetBit(byte b, int bitNumber)
    {
        bitNumber++;
        return (b & (1 << bitNumber - 1)) != 0;
    }

    public byte GetPendants() => pendants;

    public byte GetCrystals() => crystals;

    public void TogglePendant(int _val)
    {
        switch (_val)
        {
            default:
            case Constants.greenPendant:
                if (GetBit(pendants, Constants.greenPendant))
                {
                    pendants &= 0xFB; // Turn it off if we already have the pendant
                }
                else
                {
                    pendants |= 0x4; // Turn it on if we don't already have the pendant
                }

                break;
            case Constants.bluePendant:
                if (GetBit(pendants, Constants.bluePendant))
                {
                    pendants &= 0xFD; // Turn it off if we already have the pendant
                }
                else
                {
                    pendants |= 0x2; // Turn it on if we don't already have the pendant
                }

                break;
            case Constants.redPendant:
                if (GetBit(pendants, Constants.redPendant))
                {
                    pendants &= 0xFE; // Turn it off if we already have the pendant
                }
                else
                {
                    pendants |= 0x1; // Turn it on if we don't already have the pendant
                }

                break;
        }

        // Update pendant data for this save slot
        itemsAndEquipment[0x34] = pendants;
    }

    public void ToggleCrystal(int _val)
    {
        switch (_val)
        {
            default:
            case Constants.crystalPoD:
                if (GetBit(crystals, Constants.crystalPoD))
                {
                    crystals &= 0xFD; // Turn it off if we already have the crystal
                }
                else
                {
                    crystals |= 0x2; // Turn it on if we don't already have the crystal
                }

                break;
            case Constants.crystalSP:
                if (GetBit(crystals, Constants.crystalSP))
                {
                    crystals &= 0xEF; // Turn it off if we already have the crystal
                }
                else
                {
                    crystals |= 0x10; // Turn it on if we don't already have the crystal
                }

                break;
            case Constants.crystalSW:
                if (GetBit(crystals, Constants.crystalSW))
                {
                    crystals &= 0xBF; // Turn it off if we already have the crystal
                }
                else
                {
                    crystals |= 0x40; // Turn it on if we don't already have the crystal
                }

                break;
            case Constants.crystalTT:
                if (GetBit(crystals, Constants.crystalTT))
                {
                    crystals &= 0xDF; // Turn it off if we already have the crystal
                }
                else
                {
                    crystals |= 0x20; // Turn it on if we don't already have the crystal
                }

                break;
            case Constants.crystalIP:
                if (GetBit(crystals, Constants.crystalIP))
                {
                    crystals &= 0xFB; // Turn it off if we already have the crystal
                }
                else
                {
                    crystals |= 0x4; // Turn it on if we don't already have the crystal
                }

                break;
            case Constants.crystalMM:
                if (GetBit(crystals, Constants.crystalMM))
                {
                    crystals &= 0xFE; // Turn it off if we already have the crystal
                }
                else
                {
                    crystals |= 0x1; // Turn it on if we don't already have the crystal
                }

                break;
            case Constants.crystalTR:
                if (GetBit(crystals, Constants.crystalTR))
                {
                    crystals &= 0xF7; // Turn it off if we already have the crystal
                }
                else
                {
                    crystals |= 0x8; // Turn it on if we don't already have the crystal
                }

                break;
        }

        // Update crystal data for this save slot
        itemsAndEquipment[0x3A] = crystals;
    }

    public string GetPlayerName() => playerName;

    public void SetPlayerName(string str) => playerName = str;

    public void SetData(byte[] in_data) => data = in_data.ToArray();

    public void ClearData()
    {
        Array.Clear(data, 0, data.Length - 2); // Invalidate the data by storing 0 to everything besides the checksum check
        isValid = false;
    }
}
