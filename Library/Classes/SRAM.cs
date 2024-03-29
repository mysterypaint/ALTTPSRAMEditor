﻿// ReSharper disable InconsistentNaming
namespace Library.Classes;

[Serializable]
public class SRAM
{
    private byte[] data;
    private const int slot1 = 0x0;
    private const int slot1m = 0xF00;
    private const int slot2 = 0x500;
    private const int slot2m = 0x1400;
    private const int slot3 = 0xA00;
    private const int slot3m = 0x1900;
    private readonly byte[] outsav = new byte[0x2000];
    //Addresses $1E00 to $1FFE in SRAM are not used.
    //private const int mempointer = 0x1FFE; // used as the offset to know where the memory will be stored in the SRAM
    //(02 is the first file, 04 the second and 06 the third) 
    //private int currsave = 00; // 00 - No File, 02 - File 1, 04 - File 2, 06 - File 3
    private static SaveSlot
        savslot1 = default!,
        savslot2 = default!,
        savslot3 = default!,
        savslot1m = default!,
        savslot2m = default!,
        savslot3m = default!,
        savslotTemp = default!;

    public TextCharacterData TextCharacterData { get; }

    /*
    * These offsets directly correspond to $7E:F for a particular save file is being played.
    * When the game is finished it writes the information into bank $70 in the corresponding slot + offsets presented here.
    * (e.g. For the second save file, the information will be saved to $70:0500 to $70:09FF, and mirrored at $70:1400 to $70:18FF.)
    */

    // ReSharper disable once ParameterTypeCanBeEnumerable.Local
    public SRAM(byte[] data_in, TextCharacterData textCharacterData)
    {
        data = [.. data_in];
        TextCharacterData = textCharacterData;

        // Initialize the save slot data based on the larger .srm chunk
        GenerateSaveSlot(slot1, slot2, 1);
        GenerateSaveSlot(slot2, slot3, 2);
        GenerateSaveSlot(slot3, slot1m, 3);

        // Initialize the mirror save data, too
        GenerateSaveSlot(slot1m, slot2m, 4);
        GenerateSaveSlot(slot2m, slot3m, 5);
        GenerateSaveSlot(slot3m, slot3m + 0x500, 6);
    }

    public static SaveSlot GetSaveSlot(int slot) => slot switch
    {
        2 => savslot2,
        3 => savslot3,
        4 => savslot1m,
        5 => savslot2m,
        6 => savslot3m,
        _ => savslot1,
    };

    private void GenerateSaveSlot(int start, int end, int thisSlot)
    {
        var in_dat = new byte[0x500];

        var j = 0;
        for (var i = start; i < end; i++)
        {
            in_dat[j] = data[i];
            j++;
        }

        _ = thisSlot switch
        {
            1 => savslot1 = new SaveSlot(in_dat, 1, TextCharacterData),
            2 => savslot2 = new SaveSlot(in_dat, 2, TextCharacterData),
            3 => savslot3 = new SaveSlot(in_dat, 3, TextCharacterData),
            4 => savslot1m = new SaveSlot(in_dat, 4, TextCharacterData),
            5 => savslot2m = new SaveSlot(in_dat, 5, TextCharacterData),
            6 => savslot3m = new SaveSlot(in_dat, 6, TextCharacterData),
            _ => null
        };
    }

    public byte[] MergeSaveData()
    {
        savslot1.UpdatePlayer();
        savslot1.CommitPlayerName();

        if (savslot1.GetIsValid())
        {
            savslot1.ValidateSave();
        }

        var currData = savslot1.GetData();
        Array.Copy(currData, 0, outsav, slot1, 0x500);
        Array.Copy(currData, 0, outsav, slot1m, 0x500); // Write the actual save slots to the mirror slots, just in case

        savslot2.UpdatePlayer();
        savslot2.CommitPlayerName();
        if (savslot2.GetIsValid())
        {
            savslot2.ValidateSave();
        }

        currData = savslot2.GetData();
        Array.Copy(currData, 0, outsav, slot2, 0x500);
        Array.Copy(currData, 0, outsav, slot2m, 0x500); // Write the actual save slots to the mirror slots, just in case

        savslot3.UpdatePlayer();
        savslot3.CommitPlayerName();
        if (savslot3.GetIsValid())
        {
            savslot3.ValidateSave();
        }

        currData = savslot3.GetData();
        Array.Copy(currData, 0, outsav, slot3, 0x500);
        Array.Copy(currData, 0, outsav, slot3m, 0x500); // Write the actual save slots to the mirror slots, just in case

        // Amend the garbage data from the original SRAM to avoid corruption
        Array.Copy(data, 0x1E00, outsav, 0x1E00, 0x200);

        data = outsav;
        return data;
    }

    public static string ByteArrayToString(byte[] ba)
    {
        var hex = new StringBuilder(ba.Length * 2);
        foreach (var b in ba)
        {
            hex.Append($"{b:x2}");
        }

        return hex.ToString();
    }

    public static SaveSlot CreateFile(int fileSlot, SaveRegion _saveRegion, TextCharacterData textCharacterData)
    {
        // Create a clean save file to use, call it "Link" or "LINK" depending on region.
        var _new_save = new byte[0x500];
        _new_save[0x20D] = 0xF0;
        _new_save[0x20F] = 0xF0;
        _new_save[0x36C] = 0x18;
        _new_save[0x36D] = 0x18;
        _new_save[0x379] = 0xF8;

        switch (_saveRegion)
        {
            default:
            case SaveRegion.USA:
            case SaveRegion.EUR:
                _new_save[0x3D9] = 0x0B; // L
                _new_save[0x3DB] = 0xC0; // i
                _new_save[0x3DD] = 0x47; // n
                _new_save[0x3DF] = 0x44; // k
                _new_save[0x3E1] = 0xA9; // (Space)
                _new_save[0x3E3] = 0xA9; // (Space)
                _new_save[0x3E5] = 0xAA; // Everything past this is checksum verification
                _new_save[0x3E6] = 0x55;
                _new_save[0X405] = 0xFF;
                _new_save[0X406] = 0xFF;
                _new_save[0X4FE] = 0xEE;
                _new_save[0X4FF] = 0x17;
                break;
            case SaveRegion.JPN:
                _new_save[0x3D9] = 0x65; // L
                _new_save[0x3DA] = 0x01; //
                _new_save[0x3DB] = 0x62; // I
                _new_save[0x3DC] = 0x01; //
                _new_save[0x3DD] = 0x67; // N
                _new_save[0x3DE] = 0x01; //
                _new_save[0x3DF] = 0x64; // K
                _new_save[0x3E0] = 0x01; //
                _new_save[0x3E1] = 0xAA; // Everything past this is checksum verification
                _new_save[0x3E2] = 0x55;
                _new_save[0X401] = 0xFF;
                _new_save[0X402] = 0xFF;
                _new_save[0X4FE] = 0xEA;
                _new_save[0X4FF] = 0x2D;
                break;
        }

        SaveSlot savslot;
        switch (fileSlot)
        {
            default:
            // ReSharper disable once RedundantCaseLabel
            case 1:
                savslot1 = new SaveSlot(_new_save, 1, textCharacterData);
                savslot1m = savslot1;
                savslot = savslot1;
                break;
            case 2:
                savslot2 = new SaveSlot(_new_save, 2, textCharacterData);
                savslot2m = savslot2;
                savslot = savslot2;
                break;
            case 3:
                savslot3 = new SaveSlot(_new_save, 3, textCharacterData);
                savslot3m = savslot3;
                savslot = savslot3;
                break;
        }
        return savslot;
    }

    public static string CopyFile(int fileSlot, TextCharacterData textCharacterData)
    {
        var returnMessage = string.Empty;
        byte[] copyData;

        switch (fileSlot)
        {
            default:
            // ReSharper disable once RedundantCaseLabel
            case 1:
                if (savslot1.SaveIsValid())
                {
                    copyData = savslot1.GetData();
                }
                else
                {
                    returnMessage = "Save slot 1 is empty or corrupted. Copying from Mirror Data instead.";
                    copyData = savslot1m.GetData();
                }
                break;
            case 2:
                if (savslot2.SaveIsValid())
                {
                    copyData = savslot2.GetData();
                }
                else
                {
                    returnMessage = "Save slot 2 is empty or corrupted. Copying from Mirror Data instead.";
                    copyData = savslot2m.GetData();
                }
                break;
            case 3:
                if (savslot3.SaveIsValid())
                {
                    copyData = savslot3.GetData();
                }
                else
                {
                    returnMessage = "Save slot 3 is empty or corrupted. Copying from Mirror Data instead.";
                    copyData = savslot3m.GetData();
                }
                break;
        }

        if (copyData.Length > 0)
        {
            savslotTemp = new SaveSlot(copyData, fileSlot, textCharacterData);
        }

        return returnMessage;
    }

    public static SaveSlot WriteFile(int fileSlot, TextCharacterData textCharacterData)
    {
        switch (fileSlot)
        {
            default:
            // ReSharper disable once RedundantCaseLabel
            case 1:
                savslot1 = new SaveSlot(savslotTemp.GetData(), fileSlot, textCharacterData);
                savslot1.SetSaveSlot(1);
                savslot1m = savslot1;
                savslot1m.SetSaveSlot(1);
                return savslot1;
            case 2:
                savslot2 = new SaveSlot(savslotTemp.GetData(), fileSlot, textCharacterData);
                savslot2.SetSaveSlot(2);
                savslot2m = savslot2;
                savslot2m.SetSaveSlot(2);
                return savslot2;
            case 3:
                savslot3 = new SaveSlot(savslotTemp.GetData(), fileSlot, textCharacterData);
                savslot3.SetSaveSlot(3);
                savslot3m = savslot3;
                savslot3m.SetSaveSlot(3);
                return savslot3;
        }
    }

    public static void EraseFile(int fileSlot)
    {
        switch (fileSlot)
        {
            // ReSharper disable once RedundantEmptySwitchSection
            default: break;
            case 1:
                savslot1.ClearData();
                savslot1m.ClearData();
                break;
            case 2:
                savslot2.ClearData();
                savslot2m.ClearData();
                break;
            case 3:
                savslot3.ClearData();
                savslot3m.ClearData();
                break;
        }
    }
}
