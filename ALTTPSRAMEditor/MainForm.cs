namespace ALTTPSRAMEditor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class MainForm : Form
{
    private bool canRefresh = true;
    private bool fileOpen = false;
    private static readonly int[] bottleContents = new int[9];
    private static readonly Bitmap[] bottleContentsImg = new Bitmap[9];

    private static int pos = 0;
    private static SaveRegion saveRegion = SaveRegion.JPN;
    private static SRAM? sdat;
    private static string fname = string.Empty;
    private static string displayPlayerName = string.Empty;

    // Initialize some assets
    private readonly Image imgHeartContainerFull = Properties.Resources.HeartContainerFull;
    private readonly Image imgHeartContainerPartial = Properties.Resources.HeartContainerPartial;

    // Initialize the font data
    private readonly Image en_fnt = Properties.Resources.en_font;
    private readonly Image jpn_fnt = Properties.Resources.jpn_font;

    public TextCharacterData TextCharacterData { get; init; }

    public MainForm(TextCharacterData textCharacterData)
    {
        InitializeComponent();
        TextCharacterData = textCharacterData;
    }

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
        { // Open the text file using a File Stream
            var bytes = File.ReadAllBytes(fname);
            var fileSize = new FileInfo(fname).Length;
            if (fileSize == srm_size)
            {
                OpenSRMGoodSize(bytes);
            }
            else if (fileSize > srm_size)
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
            }
            else
            {
                MessageBox.Show("Invalid SRAM File.");
            }
        }
        catch (IOException)
        {
            helperText.Text = $"File reading conflict: {fname}.\nIs it open in another program?";
        }
        catch (Exception e)
        {
            MessageBox.Show($"The file could not be read:\n{e.Message}");
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
            _ = thisSlot.GetPlayer();
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
            helperText.Text = $"File writing conflict: {fname}.\nIs it open in another program?";
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) { }

    private void exitToolStripMenuItem_Click_1(object sender, EventArgs e) =>
        // Terminate the program if we select "Exit" in the Menu Bar
        Application.Exit();

    private void saveCTRLSToolStripMenuItem_Click(object sender, EventArgs e) => SaveSRM();

    private void MainForm_Load(object sender, EventArgs e)
    {
        // Define bottle array
        bottleContents[0] = (int)BottleContents.NONE;
        bottleContents[1] = (int)BottleContents.EMPTY;
        bottleContents[2] = (int)BottleContents.RED_POTION;
        bottleContents[3] = (int)BottleContents.GREEN_POTION;
        bottleContents[4] = (int)BottleContents.BLUE_POTION;
        bottleContents[5] = (int)BottleContents.FAERIE;
        bottleContents[6] = (int)BottleContents.BEE;
        bottleContents[7] = (int)BottleContents.GOOD_BEE;
        bottleContents[8] = (int)BottleContents.MUSHROOM;

        bottleContentsImg[bottleContents[0]] = Properties.Resources.D_Bottle;
        bottleContentsImg[bottleContents[1]] = Properties.Resources.Bottle;
        bottleContentsImg[bottleContents[2]] = Properties.Resources.Red_Potion;
        bottleContentsImg[bottleContents[3]] = Properties.Resources.Green_Potion;
        bottleContentsImg[bottleContents[4]] = Properties.Resources.Blue_Potion;
        bottleContentsImg[bottleContents[5]] = Properties.Resources.Fairy;
        bottleContentsImg[bottleContents[6]] = Properties.Resources.Bee;
        bottleContentsImg[bottleContents[7]] = Properties.Resources.Bee;
        bottleContentsImg[bottleContents[8]] = Properties.Resources.Mushroom;
    }

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
            default:
                break;
        }
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var toolCredits = "ALTTP SRAM Editor\n- Created by mysterypaint 2018\n\nSpecial thanks to alttp.run for the reverse-engineering documentation. http://alttp.run/hacking/index.php?title=SRAM_Map";
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
        _ = savslot?.GetPlayer();
        UpdateAllConfigurables(savslot);

        Refresh();
    }

    private void buttonCopy_Click(object sender, EventArgs e)
    {
        if (radioFile1.Checked)
        {
            SRAM.CopyFile(1);
            helperText.Text = "Copied File 1!";
        }
        else if (radioFile2.Checked)
        {
            SRAM.CopyFile(2);
            helperText.Text = "Copied File 2!";
        }
        else if (radioFile3.Checked)
        {
            SRAM.CopyFile(3);
            helperText.Text = "Copied File 3!";
        }

        buttonWrite.Enabled = true;
    }

    private void buttonWrite_Click(object sender, EventArgs e)
    {
        var savslot = SRAM.WriteFile(1);
        if (radioFile1.Checked)
        {
            helperText.Text = "Wrote to File 1!";
        }
        else if (radioFile2.Checked)
        {
            savslot = SRAM.WriteFile(2);
            helperText.Text = "Wrote to File 2!";
        }
        else if (radioFile3.Checked)
        {
            savslot = SRAM.WriteFile(3);
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

        var dialogResult = MessageBox.Show($"You are about to PERMANENTLY ERASE File {selFile}! Are you sure you want to erase it? There is no undo!",
            $"Erase File {selFile}?", MessageBoxButtons.YesNo);

        if (dialogResult == DialogResult.Yes)
        {
            SRAM.EraseFile(selFile);
            helperText.Text = $"Erased File {selFile}.";
            var savslot = SRAM.GetSaveSlot(selFile);
            savslot.SetIsValid(false);
            canRefresh = true;
            UpdateAllConfigurables(savslot);
        }
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

    private void buttonApplyChanges_Click(object sender, EventArgs e)
    {
        buttonWrite.Enabled = false;
        UpdatePlayerName();
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
        numericUpDownRupeeCounter.Value = player.GetRupeeValue();
        numericUpDownHeartContainers.Value = player.GetHeartContainers();
        numericUpDownMagic.Value = player.GetCurrMagic();
        textQuarterMagic.Visible = player.GetCurrMagicUpgrade() >= 0x2;

        // Magic Bar Upgrades

        pictureBoxMagicBar.Image = player.GetItemEquipment(magicUpgrades) switch
        {
            0x1 => Properties.Resources.lttp_magic_bar_halved,
            0x2 => Properties.Resources.lttp_magic_bar_quarter,
            _ => Properties.Resources.lttp_magic_bar,
        };

        // Bow and Arrows
        switch (player.GetItemEquipment(bow))
        {
            default:
            case 0x0:
                bowOptionNone.Checked = true;
                pictureBow.Image = Properties.Resources.D_Bow;
                break;
            case 0x1:
                bowOption1.Checked = true;
                pictureBow.Image = Properties.Resources.Bow;
                break;
            case 0x2:
                bowOption2.Checked = true;
                pictureBow.Image = Properties.Resources.Bow_and_Arrow;
                break;
            case 0x3:
                bowOption3.Checked = true;
                pictureBow.Image = Properties.Resources.Bow_and_Light_Arrow;
                break;
            case 0x4:
                bowOption4.Checked = true;
                pictureBow.Image = Properties.Resources.Bow_and_Light_Arrow;
                break;
        }

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

        pictureBombs.Image = numericUpDownBombsHeld.Value <= 0 ? Properties.Resources.D_Bomb : (Image)Properties.Resources.Bomb;

        // Boomerang
        switch (player.GetItemEquipment(boomerang))
        {
            default:
            case 0x0:
                radioButtonNoBoomerang.Checked = true;
                pictureBox1.Image = Properties.Resources.D_Boomerang;
                break;
            case 0x1:
                radioButtonBlueBoomerang.Checked = true;
                pictureBox1.Image = Properties.Resources.Boomerang;
                break;
            case 0x2:
                radioButtonRedBoomerang.Checked = true;
                pictureBox1.Image = Properties.Resources.Magical_Boomerang;
                break;
        }

        // Shovel and Flute
        switch (player.GetItemEquipment(shovelFlute))
        {
            default:
            case 0x0:
                radioButtonNoShovelOrFlute.Checked = true;
                pictureShovelFlute.Image = Properties.Resources.D_Shovel;
                break;
            case 0x1:
                radioButtonShovel.Checked = true;
                pictureShovelFlute.Image = Properties.Resources.Shovel;
                break;
            case 0x2:
                radioButtonFlute.Checked = true;
                pictureShovelFlute.Image = Properties.Resources.Flute;
                break;
            case 0x3:
                radioButtonFluteAndBird.Checked = true;
                pictureShovelFlute.Image = Properties.Resources.Flute;
                break;
        }

        // Glove Upgrades
        switch (player.GetItemEquipment(gloves))
        {
            default:
            case 0x0:
                radioButtonNoGloves.Checked = true;
                picturePowerGlove.Image = Properties.Resources.D_Power_Glove;
                break;
            case 0x1:
                radioButtonPowerGloves.Checked = true;
                picturePowerGlove.Image = Properties.Resources.Power_Glove;
                break;
            case 0x2:
                radioButtonTitansMitts.Checked = true;
                picturePowerGlove.Image = Properties.Resources.Titan_s_Mitt;
                break;
        }

        // Hookshot
        pictureHookshot.Image = player.GetItemEquipment(hookshot) == 0x1 ? Properties.Resources.Hookshot : (Image)Properties.Resources.D_Hookshot;

        // Fire Rod
        pictureFireRod.Image = player.GetItemEquipment(fireRod) == 0x1 ? Properties.Resources.Fire_Rod : (Image)Properties.Resources.D_Fire_Rod;

        // Ice Rod
        pictureIceRod.Image = player.GetItemEquipment(iceRod) == 0x1 ? Properties.Resources.Ice_Rod : (Image)Properties.Resources.D_Ice_Rod;

        // Bombos
        pictureBombos.Image = player.GetItemEquipment(bombosMedallion) == 0x1 ? Properties.Resources.Bombos : (Image)Properties.Resources.D_Bombos;

        // Ether
        pictureEther.Image = player.GetItemEquipment(etherMedallion) == 0x1 ? Properties.Resources.Ether : (Image)Properties.Resources.D_Ether;

        // Quake
        pictureQuake.Image = player.GetItemEquipment(quakeMedallion) == 0x1 ? Properties.Resources.Quake : (Image)Properties.Resources.D_Quake;

        // Lamp
        pictureLamp.Image = player.GetItemEquipment(lamp) == 0x1 ? Properties.Resources.Lamp : (Image)Properties.Resources.D_Lamp;

        // Magic Hammer
        pictureMagicHammer.Image = player.GetItemEquipment(magicHammer) == 0x1 ? Properties.Resources.Magic_Hammer : (Image)Properties.Resources.D_Magic_Hammer;

        // Bug Catching Net
        pictureBugCatchingNet.Image = player.GetItemEquipment(bugNet) == 0x1 ? Properties.Resources.Bug_Catching_Net : (Image)Properties.Resources.D_Bug_Catching_Net;

        // Book of Mudora
        pictureBookOfMudora.Image = player.GetItemEquipment(book) == 0x1 ? Properties.Resources.Book_of_Mudora : (Image)Properties.Resources.D_Book_of_Mudora;

        // Cane of Somaria
        pictureCaneOfSomaria.Image = player.GetItemEquipment(caneOfSomaria) == 0x1 ? Properties.Resources.Cane_of_Somaria : (Image)Properties.Resources.D_Cane_of_Somaria;

        // Cane of Byrna
        pictureCaneOfByrna.Image = player.GetItemEquipment(caneOfByrna) == 0x1 ? Properties.Resources.Cane_of_Byrna : (Image)Properties.Resources.D_Cane_of_Byrna;

        // Magic Cape
        pictureMagicCape.Image = player.GetItemEquipment(magicCape) == 0x1 ? Properties.Resources.Magic_Cape : (Image)Properties.Resources.D_Magic_Cape;

        // Magic Mirror
        pictureMagicMirror.Image = player.GetItemEquipment(magicMirror) == 0x2 ? Properties.Resources.Magic_Mirror : (Image)Properties.Resources.D_Magic_Mirror;

        // Moon Pearl
        pictureMoonPearl.Image = player.GetItemEquipment(moonPearl) == 0x1 ? Properties.Resources.Moon_Pearl : (Image)Properties.Resources.D_Moon_Pearl;

        var aflags = player.GetAbilityFlags(); // Grab the ability flags from this save slot

        pictureBoots.Image = GetBit(aflags, 0x2) && player.GetItemEquipment(pegasusBoots) > 0x0
            ? Properties.Resources.Pegasus_Boots
            : (Image)Properties.Resources.D_Pegasus_Boots;

        pictureZorasFlippers.Image = GetBit(aflags, 0x1) && player.GetItemEquipment(zorasFlippers) > 0x0
            ? Properties.Resources.Zora_s_Flippers
            : (Image)Properties.Resources.D_Zora_s_Flippers;

        switch (player.GetItemEquipment(mushroomPowder))
        {
            default:
            case 0x0:
                radioButtonNoMushPowd.Checked = true;
                pictureMushPowd.Image = Properties.Resources.D_Mushroom;
                break;
            case 0x1:
                radioButtonMushroom.Checked = true;
                pictureMushPowd.Image = Properties.Resources.Mushroom;
                break;
            case 0x2:
                radioButtonPowder.Checked = true;
                pictureMushPowd.Image = Properties.Resources.Magic_Powder;
                break;
        }

        switch (player.GetItemEquipment(sword))
        {
            default:
            case 0x0:
                radioButtonNoSword.Checked = true;
                pictureSword.Image = Properties.Resources.D_Fighter_s_Sword;
                break;
            case 0x1:
                radioButtonFighterSword.Checked = true;
                pictureSword.Image = Properties.Resources.Fighter_s_Sword;
                break;
            case 0x2:
                radioButtonMasterSword.Checked = true;
                pictureSword.Image = Properties.Resources.Master_Sword;
                break;
            case 0x3:
                radioButtonTemperedSword.Checked = true;
                pictureSword.Image = Properties.Resources.Tempered_Sword;
                break;
            case 0x4:
                radioButtonGoldenSword.Checked = true;
                pictureSword.Image = Properties.Resources.Golden_Sword;
                break;
        }

        switch (player.GetItemEquipment(shield))
        {
            default:
            case 0x0:
                radioButtonNoShield.Checked = true;
                pictureShield.Image = Properties.Resources.D_Fighter_s_Shield;
                break;
            case 0x1:
                radioButtonBlueShield.Checked = true;
                pictureShield.Image = Properties.Resources.Fighter_s_Shield;
                break;
            case 0x2:
                radioButtonHerosShield.Checked = true;
                pictureShield.Image = Properties.Resources.Red_Shield;
                break;
            case 0x3:
                radioButtonMirrorShield.Checked = true;
                pictureShield.Image = Properties.Resources.Mirror_Shield;
                break;
        }

        switch (player.GetItemEquipment(armor))
        {
            default:
            case 0x0:
                radioButtonGreenMail.Checked = true;
                pictureMail.Image = Properties.Resources.Green_Tunic;
                break;
            case 0x1:
                radioButtonBlueMail.Checked = true;
                pictureMail.Image = Properties.Resources.Blue_Tunic;
                break;
            case 0x2:
                radioButtonRedMail.Checked = true;
                pictureMail.Image = Properties.Resources.Red_Tunic;
                break;
        }

        // Update the picture so it represents what the inventory bottle should actually have
        var _inventoryBottleFill = 0;
        if (player.GetItemEquipment(bottle1Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle1Contents);
        }
        else if (player.GetItemEquipment(bottle2Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle2Contents);
        }
        else if (player.GetItemEquipment(bottle3Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle3Contents);
        }
        else if (player.GetItemEquipment(bottle4Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle4Contents);
        }
        else
        {
            pictureBottles.Image = Properties.Resources.D_Bottle;
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
        var fillContents = player.GetItemEquipment(bottle1Contents);
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
        fillContents = player.GetItemEquipment(bottle2Contents);
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
        fillContents = player.GetItemEquipment(bottle3Contents);
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
        fillContents = player.GetItemEquipment(bottle4Contents);
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

        pictureGreenPendant.Image = !GetBit(_pendants, greenPendant) ? Properties.Resources.Clear_Pendant : (Image)Properties.Resources.Green_Pendant;

        pictureBluePendant.Image = !GetBit(_pendants, bluePendant) ? Properties.Resources.Clear_Pendant : (Image)Properties.Resources.Blue_Pendant;

        pictureRedPendant.Image = !GetBit(_pendants, redPendant) ? Properties.Resources.Clear_Pendant : (Image)Properties.Resources.Red_Pendant;

        // Update the crystals
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalPoD.Image = !GetBit(_crystals, crystalPoD) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        pictureCrystalSP.Image = !GetBit(_crystals, crystalSP) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        pictureCrystalSW.Image = !GetBit(_crystals, crystalSW) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        pictureCrystalTT.Image = !GetBit(_crystals, crystalTT) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        pictureCrystalIP.Image = !GetBit(_crystals, crystalIP) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Red_Crystal;

        pictureCrystalMM.Image = !GetBit(_crystals, crystalMM) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Red_Crystal;

        pictureCrystalTR.Image = !GetBit(_crystals, crystalTR) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        // Update the playerName textbox
        UpdatePlayerName();
    }

    private void pictureBow_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBowConfig);

    private SaveSlot GetSaveSlot() => radioFile2.Checked ? SRAM.GetSaveSlot(2) : radioFile3.Checked ? SRAM.GetSaveSlot(3) : SRAM.GetSaveSlot(1);

    private void bowRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();

        switch (btn.Name)
        {
            case "bowOptionNone":
                player.SetHasItemEquipment(bow, 0x0); // Give No Bow
                pictureBow.Image = Properties.Resources.D_Bow;
                break;
            case "bowOption1":
                player.SetHasItemEquipment(bow, 0x1); // Give Bow
                pictureBow.Image = Properties.Resources.Bow;
                break;
            case "bowOption2":
                player.SetHasItemEquipment(bow, 0x2); // Give Bow & Arrows
                pictureBow.Image = Properties.Resources.Bow_and_Arrow;
                break;
            case "bowOption3":
                player.SetHasItemEquipment(bow, 0x3); // Give Silver Bow
                pictureBow.Image = Properties.Resources.Bow_and_Light_Arrow;
                break;
            case "bowOption4":
                player.SetHasItemEquipment(bow, 0x4); // Give Bow & Silver Arrows
                pictureBow.Image = Properties.Resources.Bow_and_Light_Arrow;
                break;
        }
    }

    private void numericUpDownRupeeCounter_ValueChanged(object sender, EventArgs e)
    {
        var savslot = GetSaveSlot();
        var player = savslot.GetPlayer();
        var val = (ushort)numericUpDownRupeeCounter.Value;
        player.SetRupees(val);
    }

    private void fileRadio(object sender, EventArgs e)
    {
        // User clicked a radio button to change file save slots
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var savslot = GetSaveSlot();
        var player = savslot.GetPlayer();
        UpdatePlayerName();
        numericUpDownRupeeCounter.Value = player.GetRupeeValue();
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
                if (canRefresh)
                {
                    canRefresh = false;
                    Refresh();
                }
                return;
            }

            // Initialize the brush for drawing, then draw the black box behind the player name
            var rectBrush = new SolidBrush(Color.Black);
            var border = 2;
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
                0x1 => Properties.Resources.lttp_magic_bar_halved,
                0x2 => Properties.Resources.lttp_magic_bar_quarter,
                _ => Properties.Resources.lttp_magic_bar,
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

    public void DrawDisplayPlayerName(PaintEventArgs e)
    {
        if (saveRegion == SaveRegion.JPN)
        {
            pos = 0;
            var i = 0;
            foreach (var c in displayPlayerName)
            {
                var letter = c;
                if (c == ' ')
                {
                    letter = '　';
                }
                else if (c is '－' or '-')
                {
                    letter = 'ー'; // Replace katanana － and/or Romaji - to hiragana ー because they're exactly the same in the game code
                }
                DrawTile(jpn_fnt, TextCharacterData.JpChar[letter], e, pos);
                pos += 16;
                i++;
            }
        }
        else if (saveRegion == SaveRegion.USA)
        {
            pos = 0;
            foreach (var c in displayPlayerName)
            {
                DrawTile(en_fnt, TextCharacterData.EnChar[c], e, pos);
                pos += 8;
            }
        }
    }

    public static void DrawTile(Image source, int tileID, PaintEventArgs e, int pos)
    {
        var tileset_width = 27; // English Font
        if (saveRegion == SaveRegion.JPN)
        {
            tileset_width = 20; // Japanese Font
        }

        var tile_w = 8;
        var tile_h = 16;
        var x = tileID % tileset_width * tile_w;
        var y = tileID / tileset_width * tile_h;
        var width = 8;
        var height = 16;
        var crop = new Rectangle(x, y, width, height);
        var bmp = new Bitmap(crop.Width, crop.Height);

        using var gr = Graphics.FromImage(bmp);
        gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);

        e.Graphics.DrawImage(bmp, 223 + pos, 49);
    }

    private void buttonChangeName_Click(object sender, EventArgs e)
    {
        var nameForm = saveRegion == SaveRegion.JPN
            ? (Form)new NameChangingFormJp(this)
            : new NameChangingFormEn(this);
        nameForm.ShowDialog();
    }

    private void numericUpDownArrowsHeld_ValueChanged(object sender, EventArgs e)
    {
        var player = GetSaveSlot().GetPlayer();
        player.SetHasItemEquipment(arrowCount, (byte)numericUpDownArrowsHeld.Value); // Set the new arrow count value
    }

    private void pictureBox1_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBoomerangConfig);

    private void HideAllGroupBoxesExcept(GroupBox currentGroupBox)
    {
        foreach (Control c in Controls)
        {
            if (c.GetType() == typeof(GroupBox) && !c.Equals(currentGroupBox) && !c.Equals(groupInventory) && !c.Equals(groupFileSelect) && !c.Equals(groupPendantsCrystals) && !c.Equals(groupBoxHUD))
            {
                c.Visible = false;
            }
        }
        currentGroupBox.Visible = true;
    }

    private void boomerangRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();
        switch (btn.Name)
        {
            default:
            case "radioButtonNoBoomerang":
                player.SetHasItemEquipment(boomerang, 0x0); // Give No Boomerang
                pictureBox1.Image = Properties.Resources.D_Boomerang;
                break;
            case "radioButtonBlueBoomerang":
                player.SetHasItemEquipment(boomerang, 0x1); // Give Blue Boomerang
                pictureBox1.Image = Properties.Resources.Boomerang;
                break;
            case "radioButtonRedBoomerang":
                player.SetHasItemEquipment(boomerang, 0x2); // Give Red Boomerang
                pictureBox1.Image = Properties.Resources.Magical_Boomerang;
                break;
        }
    }

    private void pictureHookshot_Click(object sender, EventArgs e) => ToggleItem(hookshot, 0x1, pictureHookshot, Properties.Resources.Hookshot, Properties.Resources.D_Hookshot);

    private void ToggleItem(int addr, int enabledVal, PictureBox picObj, Bitmap imgOn, Bitmap imgOff)
    {
        var player = GetSaveSlot().GetPlayer();
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
        var player = GetSaveSlot().GetPlayer();
        player.SetHasItemEquipment(bombCount, (byte)numericUpDownBombsHeld.Value); // Set the new bomb count value

        // Update the UI picture if necessary
        pictureBombs.Image = numericUpDownBombsHeld.Value <= 0 ? Properties.Resources.D_Bomb : (Image)Properties.Resources.Bomb;
    }

    private void pictureBombs_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBombs);

    private void pictureMushPowd_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxMushroomPowder);

    private void mushPowdRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();
        switch (btn.Name)
        {
            default:
            case "radioButtonNoMushPowd":
                player.SetHasItemEquipment(mushroomPowder, 0x0); // Give Neither Mushroom nor Powder
                pictureMushPowd.Image = Properties.Resources.D_Mushroom;
                break;
            case "radioButtonMushroom":
                player.SetHasItemEquipment(mushroomPowder, 0x1); // Give Mushroom
                pictureMushPowd.Image = Properties.Resources.Mushroom;
                break;
            case "radioButtonPowder":
                player.SetHasItemEquipment(mushroomPowder, 0x2); // Give Magic Powder
                pictureMushPowd.Image = Properties.Resources.Magic_Powder;
                break;
        }
    }

    private void swordRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();
        switch (btn.Name)
        {
            default:
            case "radioButtonNoSword":
                player.SetHasItemEquipment(sword, 0x0); // Give No Sword
                pictureSword.Image = Properties.Resources.D_Fighter_s_Sword;
                break;
            case "radioButtonFighterSword":
                player.SetHasItemEquipment(sword, 0x1); // Give Fighter's Sword
                pictureSword.Image = Properties.Resources.Fighter_s_Sword;
                break;
            case "radioButtonMasterSword":
                player.SetHasItemEquipment(sword, 0x2); // Give Master Sword
                pictureSword.Image = Properties.Resources.Master_Sword;
                break;
            case "radioButtonTemperedSword":
                player.SetHasItemEquipment(sword, 0x3); // Give Tempered Sword
                pictureSword.Image = Properties.Resources.Tempered_Sword;
                break;
            case "radioButtonGoldenSword":
                player.SetHasItemEquipment(sword, 0x4); // Give Golden Sword
                pictureSword.Image = Properties.Resources.Golden_Sword;
                break;
        }
    }

    private void pictureSword_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxSword);

    private void radioShield(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();
        switch (btn.Name)
        {
            default:
            case "radioButtonNoShield":
                player.SetHasItemEquipment(shield, 0x0); // Give No Shield
                pictureShield.Image = Properties.Resources.D_Fighter_s_Shield;
                break;
            case "radioButtonBlueShield":
                player.SetHasItemEquipment(shield, 0x1); // Give Fighter's Shield
                pictureShield.Image = Properties.Resources.Fighter_s_Shield;
                break;
            case "radioButtonHerosShield":
                player.SetHasItemEquipment(shield, 0x2); // Give Hero's Shield
                pictureShield.Image = Properties.Resources.Red_Shield;
                break;
            case "radioButtonMirrorShield":
                player.SetHasItemEquipment(shield, 0x3); // Give Mirror Shield
                pictureShield.Image = Properties.Resources.Mirror_Shield;
                break;
        }
    }

    private void pictureShield_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxShield);

    private void pictureMail_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxMails);

    private void mailRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();
        switch (btn.Name)
        {
            default:
            case "radioButtonGreenMail":
                player.SetHasItemEquipment(armor, 0x0); // Give Green Mail
                pictureMail.Image = Properties.Resources.Green_Tunic;
                break;
            case "radioButtonBlueMail":
                player.SetHasItemEquipment(armor, 0x1); // Give Blue Mail
                pictureMail.Image = Properties.Resources.Blue_Tunic;
                break;
            case "radioButtonRedMail":
                player.SetHasItemEquipment(armor, 0x2); // Give Red Mail
                pictureMail.Image = Properties.Resources.Red_Tunic;
                break;
        }
    }

    private void CheckForBottles()
    {
        var player = GetSaveSlot().GetPlayer();
        // Update the picture so it represents what the inventory bottle should actually have
        int _inventoryBottleFill;
        if (player.GetItemEquipment(bottle1Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle1Contents);
            player.SetSelectedBottle(1);
        }
        else if (player.GetItemEquipment(bottle2Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle2Contents);
            player.SetSelectedBottle(2);
        }
        else if (player.GetItemEquipment(bottle3Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle3Contents);
            player.SetSelectedBottle(3);
        }
        else if (player.GetItemEquipment(bottle4Contents) > 0)
        {
            _inventoryBottleFill = player.GetItemEquipment(bottle4Contents);
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

        var player = GetSaveSlot().GetPlayer();
        player.SetHasItemEquipment(bottle1Contents, (byte)fillContents);
        CheckForBottles();
    }

    private void pictureBottles_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBottles);

    private void comboBoxBottle2_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
        var fillContents = bottleContents[comboBoxBottle2.SelectedIndex];

        // Update the picture so it represents what the bottle actually has
        pictureBottle2.Image = bottleContentsImg[fillContents];

        var player = GetSaveSlot().GetPlayer();
        player.SetHasItemEquipment(bottle2Contents, (byte)fillContents);
        CheckForBottles();
    }

    private void comboBoxBottle3_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
        var fillContents = bottleContents[comboBoxBottle3.SelectedIndex];

        // Update the picture so it represents what the bottle actually has
        pictureBottle3.Image = bottleContentsImg[fillContents];

        var player = GetSaveSlot().GetPlayer();
        player.SetHasItemEquipment(bottle3Contents, (byte)fillContents);
        CheckForBottles();
    }

    private void comboBoxBottle4_SelectionChangeCommitted(object sender, EventArgs e)
    {
        // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
        var fillContents = bottleContents[comboBoxBottle4.SelectedIndex];

        // Update the picture so it represents what the bottle actually has
        pictureBottle4.Image = bottleContentsImg[fillContents];

        var player = GetSaveSlot().GetPlayer();
        player.SetHasItemEquipment(bottle4Contents, (byte)fillContents);
        CheckForBottles();
    }

    private void pictureBoots_Click(object sender, EventArgs e)
    {
        var player = GetSaveSlot().GetPlayer();
        var flags = player.GetAbilityFlags();
        if (player.GetItemEquipment(pegasusBoots) == 1)
        {
            pictureBoots.Image = Properties.Resources.D_Pegasus_Boots;
            flags &= 0xFB; // To turn it off, bitwise and with b11111011
            player.SetHasItemEquipment(pegasusBoots, 0x0);
            player.SetHasItemEquipment(abilityFlags, flags);
        }
        else
        {
            pictureBoots.Image = Properties.Resources.Pegasus_Boots;
            flags |= 0x4; // Turn it on, bitwise or with b00000100
            player.SetHasItemEquipment(pegasusBoots, 0x1);
            player.SetHasItemEquipment(abilityFlags, flags);
        }
    }

    private void pictureZorasFlippers_Click(object sender, EventArgs e)
    {
        var player = GetSaveSlot().GetPlayer();
        var flags = player.GetAbilityFlags();
        if (player.GetItemEquipment(zorasFlippers) == 1)
        {
            pictureZorasFlippers.Image = Properties.Resources.D_Zora_s_Flippers;
            flags &= 0xFD; // To turn it off, bitwise and with b11111101
            player.SetHasItemEquipment(zorasFlippers, 0x0);
            player.SetHasItemEquipment(abilityFlags, flags);
        }
        else
        {
            pictureZorasFlippers.Image = Properties.Resources.Zora_s_Flippers;
            flags |= 0x2; // Turn it on, bitwise or with b00000010
            player.SetHasItemEquipment(zorasFlippers, 0x1);
            player.SetHasItemEquipment(abilityFlags, flags);
        }
    }

    public static bool GetBit(byte b, int bitNumber)
    {
        bitNumber++;
        return (b & (1 << bitNumber - 1)) != 0;
    }

    private void pictureMagicMirror_Click(object sender, EventArgs e) => ToggleItem(magicMirror, 0x2, pictureMagicMirror, Properties.Resources.Magic_Mirror, Properties.Resources.D_Magic_Mirror);

    private void pictureFireRod_Click(object sender, EventArgs e) => ToggleItem(fireRod, 0x1, pictureFireRod, Properties.Resources.Fire_Rod, Properties.Resources.D_Fire_Rod);

    private void pictureIceRod_Click(object sender, EventArgs e) => ToggleItem(iceRod, 0x1, pictureIceRod, Properties.Resources.Ice_Rod, Properties.Resources.D_Ice_Rod);

    private void pictureBombos_Click(object sender, EventArgs e) => ToggleItem(bombosMedallion, 0x1, pictureBombos, Properties.Resources.Bombos, Properties.Resources.D_Bombos);

    private void pictureEther_Click(object sender, EventArgs e) => ToggleItem(etherMedallion, 0x1, pictureEther, Properties.Resources.Ether, Properties.Resources.D_Ether);

    private void pictureQuake_Click(object sender, EventArgs e) => ToggleItem(quakeMedallion, 0x1, pictureQuake, Properties.Resources.Quake, Properties.Resources.D_Quake);

    private void pictureLamp_Click(object sender, EventArgs e) => ToggleItem(lamp, 0x1, pictureLamp, Properties.Resources.Lamp, Properties.Resources.D_Lamp);

    private void pictureMagicHammer_Click(object sender, EventArgs e) => ToggleItem(magicHammer, 0x1, pictureMagicHammer, Properties.Resources.Magic_Hammer, Properties.Resources.D_Magic_Hammer);

    private void pictureBugCatchingNet_Click(object sender, EventArgs e) => ToggleItem(bugNet, 0x1, pictureBugCatchingNet, Properties.Resources.Bug_Catching_Net, Properties.Resources.D_Bug_Catching_Net);

    private void pictureBookOfMudora_Click(object sender, EventArgs e) => ToggleItem(book, 0x1, pictureBookOfMudora, Properties.Resources.Book_of_Mudora, Properties.Resources.D_Book_of_Mudora);

    private void pictureCaneOfSomaria_Click(object sender, EventArgs e) => ToggleItem(caneOfSomaria, 0x1, pictureCaneOfSomaria, Properties.Resources.Cane_of_Somaria, Properties.Resources.D_Cane_of_Somaria);

    private void pictureCaneOfByrna_Click(object sender, EventArgs e) => ToggleItem(caneOfByrna, 0x1, pictureCaneOfByrna, Properties.Resources.Cane_of_Byrna, Properties.Resources.D_Cane_of_Byrna);

    private void pictureMagicCape_Click(object sender, EventArgs e) => ToggleItem(magicCape, 0x1, pictureMagicCape, Properties.Resources.Magic_Cape, Properties.Resources.D_Magic_Cape);

    private void pictureMoonPearl_Click(object sender, EventArgs e) => ToggleItem(moonPearl, 0x1, pictureMoonPearl, Properties.Resources.Moon_Pearl, Properties.Resources.D_Moon_Pearl);

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

    private void pictureShovelFlute_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxShovelFlute);

    private void picturePowerGlove_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxGloves);

    private void shovelFluteRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();
        switch (btn.Name)
        {
            case "radioButtonNoShovelOrFlute":
                player.SetHasItemEquipment(shovelFlute, 0x0); // Give neither Shovel nor Flute
                pictureShovelFlute.Image = Properties.Resources.D_Shovel;
                break;
            case "radioButtonShovel":
                player.SetHasItemEquipment(shovelFlute, 0x1); // Give Shovel
                pictureShovelFlute.Image = Properties.Resources.Shovel;
                break;
            case "radioButtonFlute":
                player.SetHasItemEquipment(shovelFlute, 0x2); // Give Flute
                pictureShovelFlute.Image = Properties.Resources.Flute;
                break;
            case "radioButtonFluteAndBird":
                player.SetHasItemEquipment(shovelFlute, 0x3); // Give Flute and Bird
                pictureShovelFlute.Image = Properties.Resources.Flute;
                break;
        }
    }

    private void gloveUpgradesRadio(object sender, EventArgs e)
    {
        if (sender is not RadioButton btn || !btn.Checked)
        {
            return;
        }
        var player = GetSaveSlot().GetPlayer();
        switch (btn.Name)
        {
            case "radioButtonNoGloves":
                player.SetHasItemEquipment(gloves, 0x0); // Give neither Power Glove nor Titan's Mitts
                picturePowerGlove.Image = Properties.Resources.D_Power_Glove;
                break;
            case "radioButtonPowerGloves":
                player.SetHasItemEquipment(gloves, 0x1); // Give Power Glove
                picturePowerGlove.Image = Properties.Resources.Power_Glove;
                break;
            case "radioButtonTitansMitts":
                player.SetHasItemEquipment(gloves, 0x2); // Give Titan's Mitts
                picturePowerGlove.Image = Properties.Resources.Titan_s_Mitt;
                break;
        }
    }

    private void pictureHeartPieces_Click(object sender, EventArgs e)
    {
    }

    private void pictureBoxMagicBar_MouseClick(object sender, MouseEventArgs e)
    {
    }

    private void UpdateHeartPieceUI()
    {
        var heartPieces = GetSaveSlot().GetPlayer().GetHeartPieces();
        Image outImg = (heartPieces % 4) switch
        {
            1 => Properties.Resources.Piece_of_Heart_Quarter,
            2 => Properties.Resources.Piece_of_Heart_Half,
            3 => Properties.Resources.Piece_of_Heart_Three_Quarters,
            _ => Properties.Resources.Piece_of_Heart_Empty,
        };
        pictureHeartPieces.Image = outImg;
        Refresh();
    }

    private void pictureHeartPieces_MouseClick(object sender, MouseEventArgs e)
    {
        var player = GetSaveSlot().GetPlayer();
        var playerCurrHearts = player.GetHeartContainers();
        var playerCurrHeartPieces = player.GetHeartPieces();
        if (e.Button == MouseButtons.Left)
        {
            if (playerCurrHearts <= 152 && playerCurrHeartPieces < 24)
            {
                if ((playerCurrHeartPieces + 1) % 4 == 0)
                {
                    player.SetHeartContainers(playerCurrHearts + 8);
                }

                player.IncrementHeartPieces();
            }
        }
        else if (e.Button == MouseButtons.Right)
        {
            if (playerCurrHearts > 8 && playerCurrHeartPieces > 0)
            {
                if (player.GetHeartPieces() % 4 == 0)
                {
                    player.SetHeartContainers(playerCurrHearts - 8);
                }

                player.DecrementHeartPieces();
            }
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

        pictureGreenPendant.Image = GetBit(_pendants, greenPendant) ? Properties.Resources.Clear_Pendant : (Image)Properties.Resources.Green_Pendant;

        ToggleCrystalPendant(false, greenPendant);
    }

    private void pictureBluePendant_Click(object sender, EventArgs e)
    {
        var _pendants = GetSaveSlot().GetPendants();

        pictureBluePendant.Image = GetBit(_pendants, bluePendant) ? Properties.Resources.Clear_Pendant : (Image)Properties.Resources.Blue_Pendant;

        ToggleCrystalPendant(false, bluePendant);
    }

    private void pictureRedPendant_Click(object sender, EventArgs e)
    {
        var _pendants = GetSaveSlot().GetPendants();

        pictureRedPendant.Image = GetBit(_pendants, redPendant) ? Properties.Resources.Clear_Pendant : (Image)Properties.Resources.Red_Pendant;

        ToggleCrystalPendant(false, redPendant);
    }

    private void pictureCrystalPoD_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalPoD.Image = GetBit(_crystals, crystalPoD) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        ToggleCrystalPendant(true, crystalPoD);
    }

    private void pictureCrystalSP_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalSP.Image = GetBit(_crystals, crystalSP) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        ToggleCrystalPendant(true, crystalSP);
    }

    private void pictureCrystalSW_Click(object sender, EventArgs e)
    {
        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalSW.Image = GetBit(_crystals, crystalSW) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        ToggleCrystalPendant(true, crystalSW);
    }

    private void pictureCrystalTT_Click(object sender, EventArgs e)
    {

        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalTT.Image = GetBit(_crystals, crystalTT) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        ToggleCrystalPendant(true, crystalTT);
    }

    private void pictureCrystalIP_Click(object sender, EventArgs e)
    {

        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalIP.Image = GetBit(_crystals, crystalIP) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Red_Crystal;

        ToggleCrystalPendant(true, crystalIP);
    }

    private void pictureCrystalMM_Click(object sender, EventArgs e)
    {

        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalMM.Image = GetBit(_crystals, crystalMM) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Red_Crystal;

        ToggleCrystalPendant(true, crystalMM);
    }

    private void pictureCrystalTR_Click(object sender, EventArgs e)
    {

        var _crystals = GetSaveSlot().GetCrystals();

        pictureCrystalTR.Image = GetBit(_crystals, crystalTR) ? Properties.Resources.Clear_Crystal : (Image)Properties.Resources.Blue_Crystal;

        ToggleCrystalPendant(true, crystalTR);
    }

    private void pictureBoxMagicBar_Click(object sender, EventArgs e)
    {
        var player = GetSaveSlot().GetPlayer();
        var currMagicUpgrade = player.GetCurrMagicUpgrade();
        switch (currMagicUpgrade)
        {
            default:
            case 0x0:
                pictureBoxMagicBar.Image = Properties.Resources.lttp_magic_bar_halved;
                player.SetMagicUpgrade(0x1);
                textQuarterMagic.Visible = false;
                break;
            case 0x1:
                pictureBoxMagicBar.Image = Properties.Resources.lttp_magic_bar_quarter;
                player.SetMagicUpgrade(0x2);
                textQuarterMagic.Visible = true;
                break;
            case 0x2:
                pictureBoxMagicBar.Image = Properties.Resources.lttp_magic_bar;
                player.SetMagicUpgrade(0x0);
                textQuarterMagic.Visible = false;
                break;
        }
        Refresh();
    }

    private void numericUpDownBombUpgrades_ValueChanged(object sender, EventArgs e)
    {
        // Get the player
        var player = GetSaveSlot().GetPlayer();
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
        var player = GetSaveSlot().GetPlayer();
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
