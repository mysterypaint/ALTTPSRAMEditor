// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
using static ALTTPSRAMEditor.Properties.Resources;

namespace ALTTPSRAMEditor;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is a Windows Forms application."),
 SuppressMessage("Style", "IDE1006:Naming Styles")]
public partial class MainForm : Form
{
    private bool canRefresh = true;
    private bool fileOpen;

    // Define bottle array
    private static readonly int[] bottleContents =
        [
            (int)BottleContents.NONE,
            (int)BottleContents.EMPTY,
            (int)BottleContents.RED_POTION,
            (int)BottleContents.GREEN_POTION,
            (int)BottleContents.BLUE_POTION,
            (int)BottleContents.FAERIE,
            (int)BottleContents.BEE,
            (int)BottleContents.GOOD_BEE,
            (int)BottleContents.MUSHROOM
        ];

    private static readonly Bitmap[] bottleContentsImg =
        [
            D_Bottle,
            Bottle,
            Red_Potion,
            Green_Potion,
            Blue_Potion,
            Fairy,
            Bee,
            Bee,
            Mushroom
        ];

    private static int pos;
    private static SaveRegion saveRegion = SaveRegion.JPN;
    private static SRAM? sdat;
    private static string fname = string.Empty;
    private static string displayPlayerName = string.Empty;

    // Initialize some assets
    private readonly Image imgHeartContainerFull = HeartContainerFull;
    private readonly Image imgHeartContainerPartial = HeartContainerPartial;

    // Initialize the font data
    private readonly Image en_fnt = en_font;
    private readonly Image jpn_fnt = jpn_font;

    public TextCharacterData TextCharacterData { get; }

    public MainForm(TextCharacterData textCharacterData)
    {
        InitializeComponent();
        TextCharacterData = textCharacterData;
    }

    private SaveSlot GetSaveSlot() => radioFile2.Checked
    ? SRAM.GetSaveSlot(2)
    : radioFile3.Checked
        ? SRAM.GetSaveSlot(3)
        : SRAM.GetSaveSlot(1);

    private Link GetCurrentSlotPlayer() =>
        GetSaveSlot().GetPlayer();

    private void opensrmToolStripMenuItem_Click(object sender, EventArgs e) => OpenSRM();

    private void OpenSRM()
    {
        var fd1 = new OpenFileDialog
        {
            Filter = "SRAM|*.srm|SaveRAM|*.SaveRAM|All Files|*.*" // Filter to show.srm files only.
        };

        // Prompt the user to open a file, then check if a valid file was opened.
        if (!fd1.ShowDialog().Equals(DialogResult.OK))
        {
            return;
        }
        fname = fd1.FileName;

        try
        { 
            // Open the text file using a File Stream
            var bytes = File.ReadAllBytes(fname);
            var fileSize = new FileInfo(fname).Length;
            switch (fileSize)
            {
                case srm_size:
                    OpenSRMGoodSize(bytes);
                    break;
                case > srm_size:
                    {
                        var validFile = true;

                        if (fileSize <= 0x8000)
                        {
                            for (var i = 0x2000; i < 0x8000; i++)
                            {
                                if (bytes[i] != 0x0)
                                {
                                    validFile = false;
                                }
                            }
                        }
                        else
                        {
                            validFile = false;
                        }

                        if (!validFile)
                        {
                            MessageBox.Show("Invalid SRAM File. (Randomizer saves aren't supported. Maybe one day...?)");
                        }
                        else
                        {
                            OpenSRMGoodSize(bytes);
                        }

                        break;
                    }
                default:
                    MessageBox.Show("Invalid SRAM File.");
                    break;
            }
        }
        catch (IOException)
        {
            helperText.Text = $"""
                File reading conflict: {fname}.
                Is it open in another program?
                """;
        }
        catch (Exception e)
        {
            MessageBox.Show($"""
                The file could not be read:
                {e.Message}
                """);
        }
    }

    private void OpenSRMGoodSize(byte[] _bytes)
    {
        Console.Write($"Opened {fname}");
        fileOpen = true;
        helperText.Text = $"Opened {fname}";
        sdat = new SRAM(_bytes, TextCharacterData);
        radioFile1.Enabled = true;
        radioFile2.Enabled = true;
        radioFile3.Enabled = true;

        // Determine the overall region of the .srm and initialize the save slots
        var savslot = SRAM.GetSaveSlot(1);
        var savslot2 = SRAM.GetSaveSlot(2);
        var savslot3 = SRAM.GetSaveSlot(3);
        if (savslot.GetRegion() == SaveRegion.JPN || savslot2.GetRegion() == SaveRegion.JPN || savslot3.GetRegion() == SaveRegion.JPN)
        {
            Console.WriteLine(" - JPN Save Detected");
            saveRegion = SaveRegion.JPN;
        }
        else if (savslot.GetRegion() == SaveRegion.USA || savslot2.GetRegion() == SaveRegion.USA || savslot3.GetRegion() == SaveRegion.USA)
        {
            Console.WriteLine(" - USA Save Detected");
            saveRegion = SaveRegion.USA;
        }
        else
        {
            saveRegion = SaveRegion.EUR;
        }

        // Determine which save slot we have opened
        var thisSlot = radioFile1.Checked ? savslot : radioFile2.Checked ? savslot2 : savslot3;
        if (thisSlot.SaveIsValid())
        {
            UpdatePlayerName();
            thisSlot.GetPlayer();
            UpdateAllConfigurables(thisSlot);
        }
        else
        {
            HideAllGroupBoxesExcept(groupFileSelect);
            groupBoxHUD.Visible = false;
            groupInventory.Visible = false;
            groupPendantsCrystals.Visible = false;
            labelFilename.Visible = false;
            buttonChangeName.Visible = false;
            buttonResetDeaths.Visible = false;
            buttonCreate.Enabled = true;
            buttonCreate.Visible = true;
            buttonCopy.Visible = false;
        }
        Refresh(); // Update the screen, including the player name
    }

    private void SaveSRM()
    {
        if (sdat is null || string.IsNullOrEmpty(fname))
        {
            helperText.Text = "Load a file first!";
            return; // Abort saving if there isn't a valid file open.
        }

        var outputData = sdat.MergeSaveData();

        try
        {
            File.WriteAllBytes(fname, outputData);
            helperText.Text = $"Saved file at {fname}";
            buttonWrite.Enabled = false;
        }
        catch (IOException)
        {
            helperText.Text = $"""
                File writing conflict: {fname}.
                Is it open in another program?
                """;
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) =>
        // Terminate the program if we select "Exit" in the Menu Bar
        Application.Exit();

    private void saveCTRLSToolStripMenuItem_Click(object sender, EventArgs e) => SaveSRM();

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (!e.Control) // Handle CTRL shortcuts
        {
            return;
        }
        switch (e.KeyCode.ToString())
        {
            case "O": // CTRL+O: Open file
                OpenSRM();
                break;
            case "S": // CTRL+S: Save file
                SaveSRM();
                break;
            case "Q": // CTRL+Q: Quit program
                // Terminate the program if we select "Exit" in the Menu Bar
                Application.Exit();
                break;
        }
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        const string toolCredits = """
            ALTTP SRAM Editor
            - Created by mysterypaint 2018

            Special thanks to alttp.run for the reverse-engineering documentation. http://alttp.run/hacking/index.php?title=SRAM_Map
            """;
        MessageBox.Show(toolCredits);
    }

    private void buttonCreate_Click(object sender, EventArgs e)
    {
        SaveSlot? savslot = null;

        if (radioFile1.Checked)
        {
            savslot = SRAM.CreateFile(1, saveRegion, TextCharacterData);
            helperText.Text = "Created File 1!";
        }
        else if (radioFile2.Checked)
        {
            savslot = SRAM.CreateFile(2, saveRegion, TextCharacterData);
            helperText.Text = "Created File 2!";
        }
        else if (radioFile3.Checked)
        {
            savslot = SRAM.CreateFile(3, saveRegion, TextCharacterData);
            helperText.Text = "Created File 3!";
        }

        UpdatePlayerName();
        savslot?.GetPlayer();
        UpdateAllConfigurables(savslot);

        Refresh();
    }

    private void buttonCopy_Click(object sender, EventArgs e)
    {
        var message = string.Empty;
        if (radioFile1.Checked)
        {
            message = SRAM.CopyFile(1, TextCharacterData);
            helperText.Text = "Copied File 1!";
        }
        else if (radioFile2.Checked)
        {
            message = SRAM.CopyFile(2, TextCharacterData);
            helperText.Text = "Copied File 2!";
        }
        else if (radioFile3.Checked)
        {
            message = SRAM.CopyFile(3, TextCharacterData);
            helperText.Text = "Copied File 3!";
        }

        if (message is { Length: > 0 })
        {
            MessageBox.Show(message);
        }
        buttonWrite.Enabled = true;
    }

    private void buttonWrite_Click(object sender, EventArgs e)
    {
        var savslot = SRAM.WriteFile(1, TextCharacterData);
        if (radioFile1.Checked)
        {
            helperText.Text = "Wrote to File 1!";
        }
        else if (radioFile2.Checked)
        {
            savslot = SRAM.WriteFile(2, TextCharacterData);
            helperText.Text = "Wrote to File 2!";
        }
        else if (radioFile3.Checked)
        {
            savslot = SRAM.WriteFile(3, TextCharacterData);
            helperText.Text = "Wrote to File 3!";
        }
        UpdateAllConfigurables(savslot);
        Refresh();
    }

    private void buttonErase_Click(object sender, EventArgs e)
    {
        var selFile = 1;
        if (radioFile1.Checked)
        {
            selFile = 1;
        }
        else if (radioFile2.Checked)
        {
            selFile = 2;
        }
        else if (radioFile3.Checked)
        {
            selFile = 3;
        }

        var dialogResult = MessageBox.Show(
            $"You are about to PERMANENTLY ERASE File {selFile}! Are you sure you want to erase it? There is no undo!",
            $"Erase File {selFile}?",
            MessageBoxButtons.YesNo);

        if (dialogResult != DialogResult.Yes)
        {
            return;
        }

        SRAM.EraseFile(selFile);
        helperText.Text = $"Erased File {selFile}.";
        var savslot = SRAM.GetSaveSlot(selFile);
        savslot.SetIsValid(false);
        canRefresh = true;
        UpdateAllConfigurables(savslot);
    }

    public void SetPlayerName(string _str) => GetSaveSlot().SetPlayerName(_str);

    public void SetPlayerNameRaw(ushort[] _newName) => GetSaveSlot().SetPlayerNameRaw(_newName);

    public string GetPlayerName() => GetSaveSlot().GetPlayerName();

    public void UpdatePlayerName(string? _str = null)
    {
        var savslot = GetSaveSlot();
        if (!savslot.SaveIsValid())
        {
            displayPlayerName = string.Empty;
            buttonChangeName.Enabled = false;
            buttonResetDeaths.Enabled = false;
        }
        else
        {
            buttonChangeName.Enabled = true;
            buttonResetDeaths.Enabled = true;
        }
        displayPlayerName = _str ?? savslot.GetPlayerName();
        Refresh(); // Update the screen, including the player name
    }

    private void UpdateArrowsMax() => numericUpDownArrowsHeld.Maximum = numericUpDownArrowUpgrades.Value switch
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

    private void UpdateBombsMax() => numericUpDownBombsHeld.Maximum = numericUpDownBombUpgrades.Value switch
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

    private void UpdateAllConfigurables(SaveSlot? savslot)
    {
        if (savslot is null || !savslot.SaveIsValid())
        {
            HideAllGroupBoxesExcept(groupFileSelect);

            buttonCreate.Enabled = true;
            buttonCreate.Visible = true;
            buttonCopy.Visible = false;
            groupBoxHUD.Visible = false;
            groupInventory.Visible = false;
            numericUpDownRupeeCounter.Visible = false;
            labelRupees.Visible = false;
            labelFilename.Visible = false;
            buttonCopy.Enabled = false;
            buttonErase.Enabled = false;
            buttonChangeName.Visible = false;
            buttonResetDeaths.Visible = false;
            buttonChangeName.Enabled = false;
            buttonResetDeaths.Enabled = false;
            pictureBoxMagicBar.Visible = false;
            numericUpDownHeartContainers.Visible = false;
            labelHeartContainers.Visible = false;
            numericUpDownMagic.Visible = false;
            labelMagic.Visible = false;
            labelMagic.Enabled = false;
            groupPendantsCrystals.Visible = false;
            return;
        }

        buttonCreate.Enabled = false;
        buttonCreate.Visible = false;
        buttonCopy.Visible = true;
        groupBoxHUD.Visible = true;
        groupInventory.Visible = true;
        numericUpDownRupeeCounter.Visible = true;
        labelRupees.Visible = true;
        labelFilename.Visible = true;
        buttonCopy.Enabled = true;
        buttonErase.Enabled = true;
        buttonChangeName.Visible = true;
        buttonChangeName.Enabled = true;
        buttonResetDeaths.Visible = true;
        buttonResetDeaths.Enabled = true;
        pictureBoxMagicBar.Visible = true;
        numericUpDownHeartContainers.Visible = true;
        labelHeartContainers.Visible = true;
        numericUpDownMagic.Visible = true;
        labelMagic.Visible = true;
        labelMagic.Enabled = false;
        groupPendantsCrystals.Visible = true;

        var player = savslot.GetPlayer();
        displayPlayerName = savslot.GetPlayerName();
        numericUpDownRupeeCounter.Value = player.GetRupeesValue();
        numericUpDownHeartContainers.Value = player.GetHeartContainers();
        numericUpDownMagic.Value = player.GetCurrMagic();
        textQuarterMagic.Visible = player.GetCurrMagicUpgrade() >= 0x2;

        // Magic Bar Upgrades
        pictureBoxMagicBar.Image = player.GetItemEquipment(MagicUpgradesAddress) switch
        {
            0x1 => lttp_magic_bar_halved,
            0x2 => lttp_magic_bar_quarter,
            _ => lttp_magic_bar,
        };

        CheckBowAndArrows();

        numericUpDownArrowUpgrades.Value = player.GetCurrArrowUpgrades();
        UpdateArrowsMax();
        var _arrowCount = player.GetHeldArrows();
        if (_arrowCount > numericUpDownArrowsHeld.Maximum)
        {
            _arrowCount = (int)numericUpDownArrowsHeld.Maximum;
        }

        numericUpDownArrowsHeld.Value = _arrowCount;

        // Bombs
        numericUpDownBombUpgrades.Value = player.GetCurrBombUpgrades();
        UpdateBombsMax();
        var _bombCount = player.GetHeldBombs();
        if (_bombCount > numericUpDownBombsHeld.Maximum)
        {
            _bombCount = (int)numericUpDownBombsHeld.Maximum;
        }

        numericUpDownBombsHeld.Value = _bombCount;

        pictureBombs.Image = numericUpDownBombsHeld.Value <= 0
            ? D_Bomb
            : (Image)Bomb;

        CheckBoomerang();
        CheckShovelAndFlute();
        CheckGloves();

        // Hookshot
        pictureHookshot.Image = player.GetItemEquipment(HookshotAddress) == 0x1
            ? Hookshot
            : (Image)D_Hookshot;

        // Fire Rod
        pictureFireRod.Image = player.GetItemEquipment(FireRodAddress) == 0x1
            ? Fire_Rod
            : (Image)D_Fire_Rod;

        // Ice Rod
        pictureIceRod.Image = player.GetItemEquipment(IceRodAddress) == 0x1
            ? Ice_Rod
            : (Image)D_Ice_Rod;

        // Bombos
        pictureBombos.Image = player.GetItemEquipment(BombosMedallionAddress) == 0x1
            ? Bombos
            : (Image)D_Bombos;

        // Ether
        pictureEther.Image = player.GetItemEquipment(EtherMedallionAddress) == 0x1
            ? Ether
            : (Image)D_Ether;

        // Quake
        pictureQuake.Image = player.GetItemEquipment(QuakeMedallionAddress) == 0x1
            ? Quake
            : (Image)D_Quake;

        // Lamp
        pictureLamp.Image = player.GetItemEquipment(LampAddress) == 0x1
            ? Lamp
            : (Image)D_Lamp;

        // Magic Hammer
        pictureMagicHammer.Image = player.GetItemEquipment(MagicHammerAddress) == 0x1
            ? Magic_Hammer
            : (Image)D_Magic_Hammer;

        // Bug Catching Net
        pictureBugCatchingNet.Image = player.GetItemEquipment(BugNetAddress) == 0x1
            ? Bug_Catching_Net
            : (Image)D_Bug_Catching_Net;

        // Book of Mudora
        pictureBookOfMudora.Image = player.GetItemEquipment(BookAddress) == 0x1
            ? Book_of_Mudora
            : (Image)D_Book_of_Mudora;

        // Cane of Somaria
        pictureCaneOfSomaria.Image = player.GetItemEquipment(CaneOfSomariaAddress) == 0x1
            ? Cane_of_Somaria
            : (Image)D_Cane_of_Somaria;

        // Cane of Byrna
        pictureCaneOfByrna.Image = player.GetItemEquipment(CaneOfByrnaAddress) == 0x1
            ? Cane_of_Byrna
            : (Image)D_Cane_of_Byrna;

        // Magic Cape
        pictureMagicCape.Image = player.GetItemEquipment(MagicCapeAddress) == 0x1
            ? Magic_Cape
            : (Image)D_Magic_Cape;

        // Magic Mirror
        pictureMagicMirror.Image = player.GetItemEquipment(MagicMirrorAddress) == 0x2
            ? Magic_Mirror
            : (Image)D_Magic_Mirror;

        // Moon Pearl
        pictureMoonPearl.Image = player.GetItemEquipment(MoonPearlAddress) == 0x1
            ? Moon_Pearl
            : (Image)D_Moon_Pearl;

        var aflags = player.GetAbilityFlags(); // Grab the ability flags from this save slot

        pictureBoots.Image = GetBit(aflags, 0x4) && player.GetItemEquipment(PegasusBootsAddress) > 0x0
            ? Pegasus_Boots
            : (Image)D_Pegasus_Boots;

        pictureZorasFlippers.Image = GetBit(aflags, 0x1) && player.GetItemEquipment(ZorasFlippersAddress) > 0x0
            ? Zora_s_Flippers
            : (Image)D_Zora_s_Flippers;

        CheckMushroomPowder();
        CheckSword();
        CheckShield();
        CheckArmor();

        // Update the picture so it represents what the inventory bottle should actually have
        var _inventoryBottleFill = 0;
        if (player.GetItemEquipment(Bottle1ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle1ContentsAddress);
        }
        else if (player.GetItemEquipment(Bottle2ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle2ContentsAddress);
        }
        else if (player.GetItemEquipment(Bottle3ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle3ContentsAddress);
        }
        else if (player.GetItemEquipment(Bottle4ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle4ContentsAddress);
        }
        else
        {
            pictureBottles.Image = D_Bottle;
        }

        if (_inventoryBottleFill == 1)
        {
            _inventoryBottleFill = 9;
        }

        if (_inventoryBottleFill - 1 < 0)
        {
            _inventoryBottleFill = 1;
        }

        pictureBottles.Image = bottleContentsImg[bottleContents[_inventoryBottleFill - 1]];

        // Fill the 1st bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
        var fillContents = player.GetItemEquipment(Bottle1ContentsAddress);
        if (fillContents == 1)
        {
            fillContents = 9;
        }

        if (fillContents - 1 < 0)
        {
            comboBoxBottle1.SelectedIndex = 0;
            fillContents = 1;
        }
        else
        {
            comboBoxBottle1.SelectedIndex = fillContents - 1;
        }

        // Update the picture so it represents what the 1st bottle actually has
        pictureBottle1.Image = bottleContentsImg[bottleContents[fillContents - 1]];

        // Fill the 2nd bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
        fillContents = player.GetItemEquipment(Bottle2ContentsAddress);
        if (fillContents == 1)
        {
            fillContents = 9;
        }

        if (fillContents - 1 < 0)
        {
            comboBoxBottle2.SelectedIndex = 0;
            fillContents = 1;
        }
        else
        {
            comboBoxBottle2.SelectedIndex = fillContents - 1;
        }

        // Update the picture so it represents what the 2nd bottle actually has
        pictureBottle2.Image = bottleContentsImg[bottleContents[fillContents - 1]];

        // Fill the 3rd bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
        fillContents = player.GetItemEquipment(Bottle3ContentsAddress);
        if (fillContents == 1)
        {
            fillContents = 9;
        }

        if (fillContents - 1 < 0)
        {
            comboBoxBottle3.SelectedIndex = 0;
            fillContents = 1;
        }
        else
        {
            comboBoxBottle3.SelectedIndex = fillContents - 1;
        }

        // Update the picture so it represents what the 3rd bottle actually has
        pictureBottle3.Image = bottleContentsImg[bottleContents[fillContents - 1]];

        // Fill the 4th bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
        fillContents = player.GetItemEquipment(Bottle4ContentsAddress);
        if (fillContents == 1)
        {
            fillContents = 9;
        }

        if (fillContents - 1 < 0)
        {
            comboBoxBottle4.SelectedIndex = 0;
            fillContents = 1;
        }
        else
        {
            comboBoxBottle4.SelectedIndex = fillContents - 1;
        }

        // Update the picture so it represents what the 4th bottle actually has
        pictureBottle4.Image = bottleContentsImg[bottleContents[fillContents - 1]];

        // Update the pendants
        var _pendants = GetSaveSlot().GetPendants();

        pictureGreenPendant.Image = !GetBit(_pendants, GreenPendantAddress)
            ? Clear_Pendant
            : (Image)Green_Pendant;

        pictureBluePendant.Image = !GetBit(_pendants, BluePendantAddress)
            ? Clear_Pendant
            : (Image)Blue_Pendant;

        pictureRedPendant.Image = !GetBit(_pendants, RedPendantAddress)
            ? Clear_Pendant
            : (Image)Red_Pendant;

        // Update the crystals
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalPoD.Image = !GetBit(_crystals, CrystalPoD)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        pictureCrystalSP.Image = !GetBit(_crystals, CrystalSPAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        pictureCrystalSW.Image = !GetBit(_crystals, CrystalSWAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        pictureCrystalTT.Image = !GetBit(_crystals, CrystalTTAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        pictureCrystalIP.Image = !GetBit(_crystals, CrystalIPAddress)
            ? Clear_Crystal
            : (Image)Red_Crystal;

        pictureCrystalMM.Image = !GetBit(_crystals, CrystalMMAddress)
            ? Clear_Crystal
            : (Image)Red_Crystal;

        pictureCrystalTR.Image = !GetBit(_crystals, CrystalTRAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        // Update the playerName textbox
        UpdatePlayerName();

        void CheckBowAndArrows()
        {
            var (option, image) = player.GetItemEquipment(BowAddress) switch
            {
                0x1 => (bowOption1, Bow),
                0x2 => (bowOption2, Bow_and_Arrow),
                0x3 => (bowOption3, Bow_and_Light_Arrow),
                0x4 => (bowOption4, Bow_and_Light_Arrow),
                _ => (bowOptionNone, D_Bow),
            };
            option.Checked = true;
            pictureBow.Image = image;
        }

        void CheckBoomerang()
        {
            var (option, image) = player.GetItemEquipment(BoomerangAddress) switch
            {
                0x1 => (radioButtonBlueBoomerang, Boomerang),
                0x2 => (radioButtonRedBoomerang, Magical_Boomerang),
                _ => (radioButtonNoBoomerang, D_Boomerang),
            };
            option.Checked = true;
            pictureBoomerang.Image = image;
        }

        void CheckShovelAndFlute()
        {
            var (option, image) = player.GetItemEquipment(ShovelFluteAddress) switch
            {
                0x1 => (radioButtonShovel, Shovel),
                0x2 => (radioButtonFlute, Flute),
                0x3 => (radioButtonFluteAndBird, Flute),
                _ => (radioButtonNoShovelOrFlute, D_Shovel),
            };
            option.Checked = true;
            pictureShovelFlute.Image = image;
        }

        void CheckGloves()
        {
            var (option, image) = player.GetItemEquipment(GlovesAddress) switch
            {
                0x1 => (radioButtonPowerGloves, Power_Glove),
                0x2 => (radioButtonTitansMitts, Titan_s_Mitt),
                _ => (radioButtonNoGloves, D_Power_Glove),
            };
            option.Checked = true;
            picturePowerGlove.Image = image;
        }

        void CheckMushroomPowder()
        {
            var (option, image) = player.GetItemEquipment(MushroomPowderAddress) switch
            {
                0x1 => (radioButtonMushroom, Mushroom),
                0x2 => (radioButtonPowder, Magic_Powder),
                _ => (radioButtonNoMushPowd, D_Mushroom),
            };
            option.Checked = true;
            pictureMushPowd.Image = image;
        }

        void CheckSword()
        {
            var (option, image) = player.GetItemEquipment(SwordAddress) switch
            {
                0x1 => (radioButtonFighterSword, Fighter_s_Sword),
                0x2 => (radioButtonMasterSword, Master_Sword),
                0x3 => (radioButtonTemperedSword, Tempered_Sword),
                0x4 => (radioButtonGoldenSword, Golden_Sword),
                _ => (radioButtonNoSword, D_Fighter_s_Sword),
            };
            option.Checked = true;
            pictureSword.Image = image;
        }

        void CheckShield()
        {
            var (option, image) = player.GetItemEquipment(ShieldAddress) switch
            {
                0x1 => (radioButtonBlueShield, Fighter_s_Shield),
                0x2 => (radioButtonHerosShield, Red_Shield),
                0x3 => (radioButtonMirrorShield, Mirror_Shield),
                _ => (radioButtonNoShield, D_Fighter_s_Shield),
            };
            option.Checked = true;
            pictureShield.Image = image;
        }

        void CheckArmor()
        {
            var (option, image) = player.GetItemEquipment(ArmorAddress) switch
            {
                0x1 => (radioButtonBlueMail, Blue_Tunic),
                0x2 => (radioButtonRedMail, Red_Tunic),
                _ => (radioButtonGreenMail, Green_Tunic),
            };
            option.Checked = true;
            pictureMail.Image = image;
        }
    }

    private void pictureBow_Click(object sender, EventArgs e) =>
        HideAllGroupBoxesExcept(groupBoxBowConfig);

    private void bowRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(bowOptionNone) => (0x0, D_Bow), // Give No Bow
            nameof(bowOption1) => (0x1, Bow), // Give Bow
            nameof(bowOption2) => (0x2, Bow_and_Arrow), // Give Bow & Arrows
            nameof(bowOption3) => (0x3, Bow_and_Light_Arrow), // Give Silver Bow
            nameof(bowOption4) => (0x4, Bow_and_Light_Arrow), // Give Bow & Silver Arrows
            _ => (0, null)
        };
        player.SetHasItemEquipment(BowAddress, (byte)flag);
        pictureBow.Image = image;
    }

    private void numericUpDownRupeeCounter_ValueChanged(object sender, EventArgs e)
    {
        var player = GetCurrentSlotPlayer();
        var val = (ushort)numericUpDownRupeeCounter.Value;
        player.SetRupeesValue(val > 999 ? (ushort)999 : val);
    }

    private void fileRadio(object sender, EventArgs e)
    {
        // User clicked a radio button to change file save slots
        if (sender is not RadioButton { Checked: true })
        {
            return;
        }
        var savslot = GetSaveSlot();
        var player = savslot.GetPlayer();
        UpdatePlayerName();
        numericUpDownRupeeCounter.Value = player.GetRupeesValue();
        UpdateAllConfigurables(savslot);
        helperText.Text = !savslot.SaveIsValid()
            ? $"Save slot {savslot} is empty or invalid."
            : $"Editing Save slot {savslot}.";
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (!string.IsNullOrEmpty(fname) && fileOpen)
        {
            var savslot = GetSaveSlot();

            if (!savslot.SaveIsValid() || !savslot.GetIsValid())
            {
                textQuarterMagic.Visible = false;
                if (!canRefresh)
                {
                    return;
                }

                canRefresh = false;
                Refresh();
                return;
            }

            // Initialize the brush for drawing, then draw the black box behind the player name
            var rectBrush = new SolidBrush(Color.Black);
            const int border = 2;
            e.Graphics.FillRectangle(rectBrush, new Rectangle(223 - border, 49 - border, 8 * 8 + border * 2, 16 + border * 2));

            // Grab the player so we can get their info
            var player = savslot.GetPlayer();

            // Now we'll loop through and draw all the hearts as required to represent the player's health
            // First, create a blank canvas so we can draw to it (this is abstract, won't show up on the screen, it's just in the computer memory)
            var fillRect = new Rectangle(0, 0, 84, 20);
            var tex = new Bitmap(fillRect.Width, fillRect.Height);

            // Draw onto the blank canvas we created
            using var heartContainersGr = Graphics.FromImage(tex);
            // Apply some pixel-perfect settings before we draw anything...
            heartContainersGr.Clear(Color.Transparent);
            heartContainersGr.InterpolationMode = InterpolationMode.NearestNeighbor;
            heartContainersGr.PixelOffsetMode = PixelOffsetMode.Half;

            double heartContainers = player.GetHeartContainers();
            for (var i = 0; i < heartContainers / 8; i++)
            {
                var xOff = i % 10 * 8;
                var yOff = i / 10 * 8;
                if (i >= heartContainers / 8.0f - 1.0f && heartContainers % 8 != 0)
                {
                    heartContainersGr.DrawImage(imgHeartContainerPartial, 2 + xOff, 2 + yOff);
                }
                else
                {
                    heartContainersGr.DrawImage(imgHeartContainerFull, 2 + xOff, 2 + yOff);
                }
            }

            // Write all the contents of the canvas into the heart container preview PictureBox on the Form
            pictureBoxHeartContainerPreview.Image = tex;
            Image magicContainer = player.GetCurrMagicUpgrade() switch
            {
                0x1 => lttp_magic_bar_halved,
                0x2 => lttp_magic_bar_quarter,
                _ => lttp_magic_bar,
            };

            // Create another blank canvas so we can draw to it
            fillRect = new Rectangle(0, 0, magicContainer.Width, magicContainer.Height);
            tex = new Bitmap(fillRect.Width, fillRect.Height);

            // Draw onto the blank canvas we created
            using var magicMeterGr = Graphics.FromImage(tex);
            // Apply some pixel-perfect settings before we draw anything...
            magicMeterGr.Clear(Color.Transparent);
            magicMeterGr.InterpolationMode = InterpolationMode.NearestNeighbor;
            magicMeterGr.PixelOffsetMode = PixelOffsetMode.Half;

            // Draw the empty magic bar container into the canvas (not to the screen)
            var currMagic = player.GetCurrMagic();
            magicMeterGr.DrawImage(magicContainer, 0, 0);

            // Using a colored rectangle, fill the magic bar with a bar representing magic, based on what the player's current magic value is
            rectBrush.Color = ColorTranslator.FromHtml("#FF21C329");
            fillRect = new Rectangle(0 + 4, 0 - (currMagic + 3) / 4 + 40, 8, (currMagic + 3) / 4);
            magicMeterGr.FillRectangle(rectBrush, fillRect);

            // If necessary, draw the white "1-pixel-line" part of the magic bar fill, for aesthetic purposes
            if (currMagic > 0)
            {
                rectBrush.Color = ColorTranslator.FromHtml("#FFFFFBFF");
                fillRect = new Rectangle(5, -((currMagic + 3) / 4) + 40, 6, 1);
                magicMeterGr.FillRectangle(rectBrush, fillRect); // Could probably use a pen w/ DrawLine() here, but might as well recycle the rectangle
            }

            // Write all the contents of the canvas into the magic bar's PictureBox on the Form
            pictureBoxMagicBar.Image = tex;

            // Get rid of the brush to prevent memory leaks
            rectBrush.Dispose();
        }
        DrawDisplayPlayerName(e);
    }

    private void DrawDisplayPlayerName(PaintEventArgs e)
    {
        switch (saveRegion)
        {
            case SaveRegion.JPN:
                {
                    pos = 0;
                    foreach (var letter in displayPlayerName.Select(c => c switch
                             {
                                 ' ' => '　',
                                 '－' or '-' => 'ー',
                                 _ => c
                             }))
                    {
                        DrawTile(jpn_fnt, TextCharacterData.JpChar[letter], e, pos);
                        pos += 16;
                    }

                    break;
                }
            case SaveRegion.USA:
                {
                    pos = 0;
                    foreach (var c in displayPlayerName)
                    {
                        DrawTile(en_fnt, TextCharacterData.EnChar[c], e, pos);
                        pos += 8;
                    }

                    break;
                }
            case SaveRegion.EUR:
                break;
            default:
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentOutOfRangeException(nameof(saveRegion));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
        }
    }

    private static void DrawTile(Image source, int tileID, PaintEventArgs e, int _pos)
    {
        var tileset_width = 27; // English Font
        if (saveRegion == SaveRegion.JPN)
        {
            tileset_width = 20; // Japanese Font
        }

        const int tile_w = 8;
        const int tile_h = 16;
        var x = tileID % tileset_width * tile_w;
        var y = tileID / tileset_width * tile_h;
        const int width = 8;
        const int height = 16;
        var crop = new Rectangle(x, y, width, height);
        var bmp = new Bitmap(crop.Width, crop.Height);

        using var gr = Graphics.FromImage(bmp);
        gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);

        e.Graphics.DrawImage(bmp, 223 + _pos, 49);
    }

    private void buttonChangeName_Click(object sender, EventArgs e)
    {
        var nameForm = saveRegion == SaveRegion.JPN
            ? new NameChangingFormJp(this)
            : (Form)new NameChangingFormEn(this);
        nameForm.ShowDialog();
    }

    private void numericUpDownArrowsHeld_ValueChanged(object sender, EventArgs e)
    {
        var player = GetCurrentSlotPlayer();
        player.SetHasItemEquipment(ArrowCountAddress, (byte)numericUpDownArrowsHeld.Value); // Set the new arrow count value
    }

    private void pictureBox1_Click(object sender, EventArgs e) =>
        HideAllGroupBoxesExcept(groupBoxBoomerangConfig);

    private void HideAllGroupBoxesExcept(GroupBox currentGroupBox)
    {
        foreach (Control c in Controls)
        {
            if (c.GetType() == typeof(GroupBox) &&
                !c.Equals(currentGroupBox) &&
                !c.Equals(groupInventory) &&
                !c.Equals(groupFileSelect) &&
                !c.Equals(groupPendantsCrystals) &&
                !c.Equals(groupBoxHUD))
            {
                c.Visible = false;
            }
        }
        currentGroupBox.Visible = true;
    }

    private void boomerangRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(radioButtonNoBoomerang) => (0x0, D_Boomerang), // Give No Boomerang
            nameof(radioButtonBlueBoomerang) => (0x1, Boomerang), // Give Blue Boomerang
            nameof(radioButtonRedBoomerang) => (0x2, Magical_Boomerang), // Give Red Boomerang
            _ => (0, null)
        };
        player.SetHasItemEquipment(BoomerangAddress, (byte)flag);
        pictureBoomerang.Image = image;
    }

    private void pictureHookshot_Click(object sender, EventArgs e) =>
        ToggleItem(HookshotAddress, 0x1, pictureHookshot, Hookshot, D_Hookshot);

    private void ToggleItem(int addr, int enabledVal, PictureBox picObj, Bitmap imgOn, Bitmap imgOff)
    {
        var player = GetCurrentSlotPlayer();
        if (player.GetItemEquipment(addr) == enabledVal)
        {
            player.SetHasItemEquipment(addr, 0x0); // Give the item if we don't already have it
            picObj.Image = imgOff;
        }
        else
        {
            player.SetHasItemEquipment(addr, (byte)enabledVal); // Take the item because we must already have it
            picObj.Image = imgOn;
        }
    }

    private void numericUpDownBombsHeld_ValueChanged(object sender, EventArgs e)
    {
        var player = GetCurrentSlotPlayer();
        player.SetHasItemEquipment(BombCountAddress, (byte)numericUpDownBombsHeld.Value); // Set the new bomb count value

        // Update the UI picture if necessary
        pictureBombs.Image = numericUpDownBombsHeld.Value <= 0 ? D_Bomb : (Image)Bomb;
    }

    private void pictureBombs_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBombs);

    private void pictureMushPowd_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxMushroomPowder);

    private void mushPowdRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(radioButtonNoMushPowd) => (0x0, D_Mushroom), // Give Neither Mushroom nor Powder
            nameof(radioButtonMushroom) => (0x1, Mushroom), // Give Mushroom
            nameof(radioButtonPowder) => (0x2, Magic_Powder), // Give Magic Powder
            _ => (0, null)
        };
        player.SetHasItemEquipment(MushroomPowderAddress, (byte)flag);
        pictureMushPowd.Image = image;
    }

    private void swordRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(radioButtonNoSword) => (0x0, D_Fighter_s_Sword), // Give No Sword
            nameof(radioButtonFighterSword) => (0x1, Fighter_s_Sword), // Give Fighter's Sword
            nameof(radioButtonMasterSword) => (0x2, Master_Sword), // Give Master Sword
            nameof(radioButtonTemperedSword) => (0x3, Tempered_Sword), // Give Tempered Sword
            nameof(radioButtonGoldenSword) => (0x4, Golden_Sword), // Give Golden Sword
            _ => (0, null)
        };
        player.SetHasItemEquipment(SwordAddress, (byte)flag);
        pictureSword.Image = image;
    }

    private void pictureSword_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxSword);

    private void radioShield(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(radioButtonNoShield) => (0x0, D_Fighter_s_Shield), // Give No Shield
            nameof(radioButtonBlueShield) => (0x1, Fighter_s_Shield), // Give Fighter's Shield
            nameof(radioButtonHerosShield) => (0x2, Red_Shield), // Give Hero's Shield
            nameof(radioButtonMirrorShield) => (0x3, Mirror_Shield), // Give Mirror Shield
            _ => (0, null)
        };
        player.SetHasItemEquipment(ShieldAddress, (byte)flag);
        pictureShield.Image = image;
    }

    private void pictureShield_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxShield);

    private void pictureMail_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxMails);

    private void mailRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(radioButtonGreenMail) => (0x0, Green_Tunic), // Give Green Mail
            nameof(radioButtonBlueMail) => (0x1, Blue_Tunic), // Give Blue Mail
            nameof(radioButtonRedMail) => (0x2, Red_Tunic), // Give Red Mail
            _ => (0, null)
        };
        player.SetHasItemEquipment(ArmorAddress, (byte)flag);
        pictureMail.Image = image;
    }

    private void CheckForBottles()
    {
        var player = GetCurrentSlotPlayer();
        // Update the picture so it represents what the inventory bottle should actually have
        int _inventoryBottleFill;
        if (player.GetItemEquipment(Bottle1ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle1ContentsAddress);
            player.SetSelectedBottle(1);
        }
        else if (player.GetItemEquipment(Bottle2ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle2ContentsAddress);
            player.SetSelectedBottle(2);
        }
        else if (player.GetItemEquipment(Bottle3ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle3ContentsAddress);
            player.SetSelectedBottle(3);
        }
        else if (player.GetItemEquipment(Bottle4ContentsAddress) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(Bottle4ContentsAddress);
            player.SetSelectedBottle(4);
        }
        else
        {
            _inventoryBottleFill = 0;
            player.SetSelectedBottle(0);
        }

        if (_inventoryBottleFill == 1)
        {
            _inventoryBottleFill = 9;
        }

        if (_inventoryBottleFill - 1 < 0)
        {
            _inventoryBottleFill = 1;
        }

        pictureBottles.Image = bottleContentsImg[bottleContents[_inventoryBottleFill - 1]];
    }

    private void comboBoxBottle1_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
        var fillContents = bottleContents[comboBoxBottle1.SelectedIndex];

        // Update the picture so it represents what the bottle actually has
        pictureBottle1.Image = bottleContentsImg[fillContents];

        var player = GetCurrentSlotPlayer();
        player.SetHasItemEquipment(Bottle1ContentsAddress, (byte)fillContents);
        CheckForBottles();
    }

    private void pictureBottles_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBottles);

    private void comboBoxBottle2_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
        var fillContents = bottleContents[comboBoxBottle2.SelectedIndex];

        // Update the picture so it represents what the bottle actually has
        pictureBottle2.Image = bottleContentsImg[fillContents];

        var player = GetCurrentSlotPlayer();
        player.SetHasItemEquipment(Bottle2ContentsAddress, (byte)fillContents);
        CheckForBottles();
    }

    private void comboBoxBottle3_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
        var fillContents = bottleContents[comboBoxBottle3.SelectedIndex];

        // Update the picture so it represents what the bottle actually has
        pictureBottle3.Image = bottleContentsImg[fillContents];

        var player = GetCurrentSlotPlayer();
        player.SetHasItemEquipment(Bottle3ContentsAddress, (byte)fillContents);
        CheckForBottles();
    }

    private void comboBoxBottle4_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
        var fillContents = bottleContents[comboBoxBottle4.SelectedIndex];

        // Update the picture so it represents what the bottle actually has
        pictureBottle4.Image = bottleContentsImg[fillContents];

        var player = GetCurrentSlotPlayer();
        player.SetHasItemEquipment(Bottle4ContentsAddress, (byte)fillContents);
        CheckForBottles();
    }

    private void pictureBoots_Click(object sender, EventArgs e)
    {
        var player = GetCurrentSlotPlayer();
        var flags = player.GetAbilityFlags();
        if (player.GetItemEquipment(PegasusBootsAddress) == 1)
        {
            pictureBoots.Image = D_Pegasus_Boots;
            flags &= 0xFB; // To turn it off, bitwise and with b11111011
            player.SetHasItemEquipment(PegasusBootsAddress, 0x0);
            player.SetHasItemEquipment(AbilityFlagsAddress, flags);
        }
        else
        {
            pictureBoots.Image = Pegasus_Boots;
            flags |= 0x4; // Turn it on, bitwise or with b00000100
            player.SetHasItemEquipment(PegasusBootsAddress, 0x1);
            player.SetHasItemEquipment(AbilityFlagsAddress, flags);
        }
    }

    private void pictureZorasFlippers_Click(object sender, EventArgs e)
    {
        var player = GetCurrentSlotPlayer();
        var flags = player.GetAbilityFlags();
        if (player.GetItemEquipment(ZorasFlippersAddress) == 1)
        {
            pictureZorasFlippers.Image = D_Zora_s_Flippers;
            flags &= 0xFD; // To turn it off, bitwise and with b11111101
            player.SetHasItemEquipment(ZorasFlippersAddress, 0x0);
            player.SetHasItemEquipment(AbilityFlagsAddress, flags);
        }
        else
        {
            pictureZorasFlippers.Image = Zora_s_Flippers;
            flags |= 0x2; // Turn it on, bitwise or with b00000010
            player.SetHasItemEquipment(ZorasFlippersAddress, 0x1);
            player.SetHasItemEquipment(AbilityFlagsAddress, flags);
        }
    }

    private static bool GetBit(byte b, int bitNumber)
    {
        bitNumber++;
        return (b & 1 << bitNumber - 1) != 0;
    }

    private void pictureMagicMirror_Click(object sender, EventArgs e) =>
        ToggleItem(MagicMirrorAddress, 0x2, pictureMagicMirror, Magic_Mirror, D_Magic_Mirror);

    private void pictureFireRod_Click(object sender, EventArgs e) =>
        ToggleItem(FireRodAddress, 0x1, pictureFireRod, Fire_Rod, D_Fire_Rod);

    private void pictureIceRod_Click(object sender, EventArgs e) =>
        ToggleItem(IceRodAddress, 0x1, pictureIceRod, Ice_Rod, D_Ice_Rod);

    private void pictureBombos_Click(object sender, EventArgs e) =>
        ToggleItem(BombosMedallionAddress, 0x1, pictureBombos, Bombos, D_Bombos);

    private void pictureEther_Click(object sender, EventArgs e) =>
        ToggleItem(EtherMedallionAddress, 0x1, pictureEther, Ether, D_Ether);

    private void pictureQuake_Click(object sender, EventArgs e) =>
        ToggleItem(QuakeMedallionAddress, 0x1, pictureQuake, Quake, D_Quake);

    private void pictureLamp_Click(object sender, EventArgs e) =>
        ToggleItem(LampAddress, 0x1, pictureLamp, Lamp, D_Lamp);

    private void pictureMagicHammer_Click(object sender, EventArgs e) =>
        ToggleItem(MagicHammerAddress, 0x1, pictureMagicHammer, Magic_Hammer, D_Magic_Hammer);

    private void pictureBugCatchingNet_Click(object sender, EventArgs e) =>
        ToggleItem(BugNetAddress, 0x1, pictureBugCatchingNet, Bug_Catching_Net, D_Bug_Catching_Net);

    private void pictureBookOfMudora_Click(object sender, EventArgs e) =>
        ToggleItem(BookAddress, 0x1, pictureBookOfMudora, Book_of_Mudora, D_Book_of_Mudora);

    private void pictureCaneOfSomaria_Click(object sender, EventArgs e) =>
        ToggleItem(CaneOfSomariaAddress, 0x1, pictureCaneOfSomaria, Cane_of_Somaria, D_Cane_of_Somaria);

    private void pictureCaneOfByrna_Click(object sender, EventArgs e) =>
        ToggleItem(CaneOfByrnaAddress, 0x1, pictureCaneOfByrna, Cane_of_Byrna, D_Cane_of_Byrna);

    private void pictureMagicCape_Click(object sender, EventArgs e) =>
        ToggleItem(MagicCapeAddress, 0x1, pictureMagicCape, Magic_Cape, D_Magic_Cape);

    private void pictureMoonPearl_Click(object sender, EventArgs e) =>
        ToggleItem(MoonPearlAddress, 0x1, pictureMoonPearl, Moon_Pearl, D_Moon_Pearl);

    private void numericUpDownHeartContainers_ValueChanged(object sender, EventArgs e)
    {
        GetSaveSlot().GetPlayer().SetHeartContainers((int)numericUpDownHeartContainers.Value);
        Refresh();
    }

    private void numericUpDownMagic_ValueChanged(object sender, EventArgs e)
    {
        GetSaveSlot().GetPlayer().SetMagic((int)numericUpDownMagic.Value);
        Refresh();
    }

    private void pictureShovelFlute_Click(object sender, EventArgs e) =>
        HideAllGroupBoxesExcept(groupBoxShovelFlute);

    private void picturePowerGlove_Click(object sender, EventArgs e) =>
        HideAllGroupBoxesExcept(groupBoxGloves);

    private void shovelFluteRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(radioButtonNoShovelOrFlute) => (0x0, D_Shovel), // Give neither Shovel nor Flute
            nameof(radioButtonShovel) => (0x1, Shovel), // Give Shovel
            nameof(radioButtonFlute) => (0x2, Flute), // Give Flute
            nameof(radioButtonFluteAndBird) => (0x3, Flute), // Give Flute and Bird
            _ => (0, null)
        };
        player.SetHasItemEquipment(ShovelFluteAddress, (byte)flag);
        pictureShovelFlute.Image = image;
    }

    private void gloveUpgradesRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } btn)
        {
            return;
        }
        var player = GetCurrentSlotPlayer();
        var (flag, image) = btn.Name switch
        {
            nameof(radioButtonNoGloves) => (0x0, D_Power_Glove), // Give neither Power Glove nor Titan's Mitts
            nameof(radioButtonPowerGloves) => (0x1, Power_Glove), // Give Power Glove
            nameof(radioButtonTitansMitts) => (0x2, Titan_s_Mitt), // Give Titan's Mitts
            _ => (0, null)
        };
        player.SetHasItemEquipment(GlovesAddress, (byte)flag);
        picturePowerGlove.Image = image;
    }

    private void UpdateHeartPieceUI()
    {
        var heartPieces = GetSaveSlot().GetPlayer().GetHeartPieces();
        Image outImg = (heartPieces % 4) switch
        {
            1 => Piece_of_Heart_Quarter,
            2 => Piece_of_Heart_Half,
            3 => Piece_of_Heart_Three_Quarters,
            _ => Piece_of_Heart_Empty,
        };
        pictureHeartPieces.Image = outImg;
        Refresh();
    }

    private void pictureHeartPieces_MouseClick(object sender, MouseEventArgs e)
    {
        var player = GetCurrentSlotPlayer();
        var playerCurrHearts = player.GetHeartContainers();
        var playerCurrHeartPieces = player.GetHeartPieces();
        switch (e.Button)
        {
            case MouseButtons.Left:
                {
                    if (playerCurrHearts <= 152 && playerCurrHeartPieces < 24)
                    {
                        if ((playerCurrHeartPieces + 1) % 4 == 0)
                        {
                            player.SetHeartContainers(playerCurrHearts + 8);
                        }

                        player.IncrementHeartPieces();
                    }

                    break;
                }
            case MouseButtons.Right:
                {
                    if (playerCurrHearts > 8 && playerCurrHeartPieces > 0)
                    {
                        if (player.GetHeartPieces() % 4 == 0)
                        {
                            player.SetHeartContainers(playerCurrHearts - 8);
                        }

                        player.DecrementHeartPieces();
                    }

                    break;
                }
            default:
                throw new ArgumentOutOfRangeException($"{nameof(e)}.{nameof(e.Button)}");
        }
        UpdateHeartPieceUI();
        numericUpDownHeartContainers.Value = player.GetHeartContainers();
        //pictureHeartPieces
    }

    private void ToggleCrystalPendant(bool isCrystal, int _val)
    {
        if (!isCrystal)
        {
            GetSaveSlot().TogglePendant(_val);
        }
        else
        {
            GetSaveSlot().ToggleCrystal(_val);
        }
    }

    private void pictureGreenPendant_Click(object sender, EventArgs e)
    {
        var _pendants = GetSaveSlot().GetPendants();

        pictureGreenPendant.Image = GetBit(_pendants, GreenPendantAddress)
            ? Clear_Pendant
            : (Image)Green_Pendant;

        ToggleCrystalPendant(false, GreenPendantAddress);
    }

    private void pictureBluePendant_Click(object sender, EventArgs e)
    {
        var _pendants = GetSaveSlot().GetPendants();

        pictureBluePendant.Image = GetBit(_pendants, BluePendantAddress)
            ? Clear_Pendant
            : (Image)Blue_Pendant;

        ToggleCrystalPendant(false, BluePendantAddress);
    }

    private void pictureRedPendant_Click(object sender, EventArgs e)
    {
        var _pendants = GetSaveSlot().GetPendants();

        pictureRedPendant.Image = GetBit(_pendants, RedPendantAddress)
            ? Clear_Pendant
            : (Image)Red_Pendant;

        ToggleCrystalPendant(false, RedPendantAddress);
    }

    private void pictureCrystalPoD_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalPoD.Image = GetBit(_crystals, CrystalPoD)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        ToggleCrystalPendant(true, CrystalPoD);
    }

    private void pictureCrystalSP_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalSP.Image = GetBit(_crystals, CrystalSPAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        ToggleCrystalPendant(true, CrystalSPAddress);
    }

    private void pictureCrystalSW_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalSW.Image = GetBit(_crystals, CrystalSWAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        ToggleCrystalPendant(true, CrystalSWAddress);
    }

    private void pictureCrystalTT_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalTT.Image = GetBit(_crystals, CrystalTTAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        ToggleCrystalPendant(true, CrystalTTAddress);
    }

    private void pictureCrystalIP_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalIP.Image = GetBit(_crystals, CrystalIPAddress)
            ? Clear_Crystal
            : (Image)Red_Crystal;

        ToggleCrystalPendant(true, CrystalIPAddress);
    }

    private void pictureCrystalMM_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalMM.Image = GetBit(_crystals, CrystalMMAddress)
            ? Clear_Crystal
            : (Image)Red_Crystal;

        ToggleCrystalPendant(true, CrystalMMAddress);
    }

    private void pictureCrystalTR_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalTR.Image = GetBit(_crystals, CrystalTRAddress)
            ? Clear_Crystal
            : (Image)Blue_Crystal;

        ToggleCrystalPendant(true, CrystalTRAddress);
    }

    private void pictureBoxMagicBar_Click(object sender, EventArgs e)
    {
        var player = GetCurrentSlotPlayer();
        var currMagicUpgrade = player.GetCurrMagicUpgrade();

        var (magicBarImage, magicUpgrade, magicBarTextVisible) = currMagicUpgrade switch
        {
            0x1 => (lttp_magic_bar_quarter, 0x2, true),
            0x2 => (lttp_magic_bar, 0x0, false),
            _ => (lttp_magic_bar_halved, 0x1, false),
        };

        pictureBoxMagicBar.Image = magicBarImage;
        player.SetMagicUpgrade(magicUpgrade);
        textQuarterMagic.Visible = magicBarTextVisible;

        Refresh();
    }

    private void numericUpDownBombUpgrades_ValueChanged(object sender, EventArgs e)
    {
        // Get the player
        var player = GetCurrentSlotPlayer();
        player.SetCurrBombUpgrades((int)numericUpDownBombUpgrades.Value);

        // Update Max bombs
        UpdateBombsMax();
        var _bombCount = numericUpDownBombsHeld.Value;
        if (_bombCount > numericUpDownBombsHeld.Maximum)
        {
            _bombCount = (int)numericUpDownBombsHeld.Maximum;
        }

        numericUpDownBombsHeld.Value = _bombCount;
    }

    private void numericUpDownArrowUpgrades_ValueChanged(object sender, EventArgs e)
    {
        // Get the player
        var player = GetCurrentSlotPlayer();
        player.SetCurrArrowUpgrades((int)numericUpDownArrowUpgrades.Value);

        // Update Max arrows
        UpdateArrowsMax();
        var _bombCount = numericUpDownArrowsHeld.Value;
        if (_bombCount > numericUpDownArrowsHeld.Maximum)
        {
            _bombCount = (int)numericUpDownArrowsHeld.Maximum;
        }

        numericUpDownArrowsHeld.Value = _bombCount;
    }

    private void buttonResetDeaths_Click(object sender, EventArgs e)
    {
        var savslot = GetSaveSlot();
        var dialogResult = MessageBox.Show("Reset all deaths/saves for this save file?", "Reset Deaths/Saves?", MessageBoxButtons.YesNo);
        if (dialogResult != DialogResult.Yes)
        {
            return;
        }
        var postGame = MessageBox.Show("Show the deaths on the File Select Screen?", "Show on File Select Screen?", MessageBoxButtons.YesNo);
        savslot.ResetFileDeaths(postGame != DialogResult.No);
    }
}
