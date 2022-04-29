using Library.Classes;
using System.Drawing.Drawing2D;
using static Library.Classes.Constants;
using static Library.Classes.Enums;

namespace ALTTPSRAMEditor
{
    public partial class Form1 : Form
    {
        private bool canRefresh = true;
        private bool fileOpen = false;
        private static readonly int[] bottleContents = new int[9];
        private static readonly System.Drawing.Bitmap[] bottleContentsImg = new System.Drawing.Bitmap[9];

        private static int pos = 0;
        private static int saveRegion = (int)SaveRegion.USA;
        private static SRAM sdat;
        private static string fname = "";
        private static string displayPlayerName = "";

        // Initialize some assets
        private readonly Image imgHeartContainerFull = ALTTPSRAMEditor.Properties.Resources.HeartContainerFull;
        private readonly Image imgHeartContainerPartial = ALTTPSRAMEditor.Properties.Resources.HeartContainerPartial;

        // Initialize the font data
        private readonly Image en_fnt = ALTTPSRAMEditor.Properties.Resources.en_font;
        private readonly Image jpn_fnt = ALTTPSRAMEditor.Properties.Resources.jpn_font;
        public static Dictionary<char, int> enChar = new Dictionary<char, int>();
        public static Dictionary<char, int> jpChar = new Dictionary<char, int>();

        public Form1() => InitializeComponent();

        public Dictionary<char, int> GetENChar() => enChar;

        public Dictionary<ushort, char> GetRawENChar() => AppState.rawENChar;

        public Dictionary<char, int> GetJPChar() => jpChar;

        public Dictionary<ushort, char> GetRawJPChar() => AppState.rawJPChar;

        private void opensrmToolStripMenuItem_Click(object sender, EventArgs e) => OpenSRM();

        private void OpenSRM()
        {
            var fd1 = new OpenFileDialog
            {
                Filter = "SRAM|*.srm|SaveRAM|*.SaveRAM|All Files|*.*" // Filter to show.srm files only.
            };
            if (fd1.ShowDialog().Equals(DialogResult.OK))
            { // Prompt the user to open a file, then check if a valid file was opened.
                fname = fd1.FileName;

                try
                { // Open the text file using a File Stream
                    var bytes = File.ReadAllBytes(fname);
                    var fileSize = new System.IO.FileInfo(fname).Length;
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
                                    validFile = false;
                            }
                        }
                        else
                            validFile = false;

                        if (!validFile)
                            MessageBox.Show("Invalid SRAM File. (Randomizer saves aren't supported. Maybe one day...?)");
                        else
                            OpenSRMGoodSize(bytes);
                    }
                    else
                    {
                        MessageBox.Show("Invalid SRAM File.");
                    }
                }
                catch (System.IO.IOException)
                {
                    helperText.Text = "File reading conflict: " + fname + ".\nIs it open in another program?";
                }
                catch (Exception e)
                {
                    MessageBox.Show("The file could not be read:\n" + e.Message);
                }
            }
        }

        private void OpenSRMGoodSize(byte[] _bytes)
        {
            Console.Write("Opened " + fname);
            fileOpen = true;
            helperText.Text = "Opened " + fname;
            sdat = new SRAM(_bytes);
            radioFile1.Enabled = true;
            radioFile2.Enabled = true;
            radioFile3.Enabled = true;

            // Determine the overall region of the .srm and initialize the save slots
            var savslot = sdat.GetSaveSlot(1);
            var savslot2 = sdat.GetSaveSlot(2);
            var savslot3 = sdat.GetSaveSlot(3);
            if (savslot.GetRegion() == SaveRegion.USA || savslot2.GetRegion() == SaveRegion.USA || savslot3.GetRegion() == SaveRegion.USA)
            {
                Console.WriteLine(" - USA Save Detected");
                saveRegion = (int)SaveRegion.USA;
            }
            else if (savslot.GetRegion() == SaveRegion.JPN || savslot2.GetRegion() == SaveRegion.JPN || savslot3.GetRegion() == SaveRegion.JPN)
            {
                Console.WriteLine(" - JPN Save Detected");
                saveRegion = (int)SaveRegion.JPN;
            }
            else
                saveRegion = (int)SaveRegion.EUR;

            // Determine which save slot we have opened
            SaveSlot thisSlot;
            if (radioFile1.Checked)
            {
                thisSlot = savslot;
            }
            else if (radioFile2.Checked)
            {
                thisSlot = savslot2;
            }
            else
                thisSlot = savslot3;

            if (thisSlot.SaveIsValid())
            {
                UpdatePlayerName();
                var player = thisSlot.GetPlayer();
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
            if (fname.Equals("") || fname.Equals(null))
            {
                helperText.Text = "Load a file first!";
                return; // Abort saving if there isn't a valid file open.
            }

            var outputData = sdat.MergeSaveData();

            try
            {
                File.WriteAllBytes(fname, outputData);
                helperText.Text = "Saved file at " + fname;
                buttonWrite.Enabled = false;
            }
            catch (System.IO.IOException)
            {
                helperText.Text = "File writing conflict: " + fname + ".\nIs it open in another program?";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e) =>
            // Terminate the program if we select "Exit" in the Menu Bar
            System.Windows.Forms.Application.Exit();

        private void saveCTRLSToolStripMenuItem_Click(object sender, EventArgs e) => SaveSRM();

        private void Form1_Load(object sender, EventArgs e)
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

            bottleContentsImg[bottleContents[0]] = ALTTPSRAMEditor.Properties.Resources.D_Bottle;
            bottleContentsImg[bottleContents[1]] = ALTTPSRAMEditor.Properties.Resources.Bottle;
            bottleContentsImg[bottleContents[2]] = ALTTPSRAMEditor.Properties.Resources.Red_Potion;
            bottleContentsImg[bottleContents[3]] = ALTTPSRAMEditor.Properties.Resources.Green_Potion;
            bottleContentsImg[bottleContents[4]] = ALTTPSRAMEditor.Properties.Resources.Blue_Potion;
            bottleContentsImg[bottleContents[5]] = ALTTPSRAMEditor.Properties.Resources.Fairy;
            bottleContentsImg[bottleContents[6]] = ALTTPSRAMEditor.Properties.Resources.Bee;
            bottleContentsImg[bottleContents[7]] = ALTTPSRAMEditor.Properties.Resources.Bee;
            bottleContentsImg[bottleContents[8]] = ALTTPSRAMEditor.Properties.Resources.Mushroom;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control) // Handle CTRL shortcuts
            {
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
                        System.Windows.Forms.Application.Exit();
                        break;
                    default:
                        break;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var toolCredits = "ALTTP SRAM Editor\n- Created by mysterypaint 2018\n\nSpecial thanks to alttp.run for the reverse-engineering documentation. http://alttp.run/hacking/index.php?title=SRAM_Map";
            MessageBox.Show(toolCredits);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            SaveSlot savslot = null;

            if (radioFile1.Checked)
            {
                savslot = sdat.CreateFile(1, (SaveRegion)saveRegion);
                helperText.Text = "Created File 1!";
            }
            else if (radioFile2.Checked)
            {
                savslot = sdat.CreateFile(2, (SaveRegion)saveRegion);
                helperText.Text = "Created File 2!";
            }
            else if (radioFile3.Checked)
            {
                savslot = sdat.CreateFile(3, (SaveRegion)saveRegion);
                helperText.Text = "Created File 3!";
            }

            UpdatePlayerName();
            var player = savslot.GetPlayer();
            UpdateAllConfigurables(savslot);

            Refresh();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (radioFile1.Checked)
            {
                sdat.CopyFile(1);
                helperText.Text = "Copied File 1!";
            }
            else if (radioFile2.Checked)
            {
                sdat.CopyFile(2);
                helperText.Text = "Copied File 2!";
            }
            else if (radioFile3.Checked)
            {
                sdat.CopyFile(3);
                helperText.Text = "Copied File 3!";
            }

            buttonWrite.Enabled = true;
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            var savslot = sdat.WriteFile(1);
            if (radioFile1.Checked)
            {
                helperText.Text = "Wrote to File 1!";
            }
            else if (radioFile2.Checked)
            {
                savslot = sdat.WriteFile(2);
                helperText.Text = "Wrote to File 2!";
            }
            else if (radioFile3.Checked)
            {
                savslot = sdat.WriteFile(3);
                helperText.Text = "Wrote to File 3!";
            }
            UpdateAllConfigurables(savslot);
            Refresh();
        }

        private void buttonErase_Click(object sender, EventArgs e)
        {
            var selFile = 1;
            if (radioFile1.Checked) selFile = 1;
            else if (radioFile2.Checked) selFile = 2;
            else if (radioFile3.Checked) selFile = 3;
            var dialogResult = MessageBox.Show("You are about to PERMANENTLY ERASE File " + selFile + "! Are you sure you want to erase it? There is no undo!", "Erase File " + selFile + "?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                sdat.EraseFile(selFile);
                helperText.Text = "Erased File " + selFile + ".";
                var savslot = sdat.GetSaveSlot(selFile);
                savslot.SetIsValid(false);
                canRefresh = true;
                UpdateAllConfigurables(savslot);
            }
        }

        public void SetPlayerName(string _str) => GetSaveSlot().SetPlayerName(_str);

        public void SetPlayerNameRaw(ushort[] _newName) => GetSaveSlot().SetPlayerNameRaw(_newName);

        public string GetPlayerName() => GetSaveSlot().GetPlayerName();

        public void UpdatePlayerName()
        {
            var savslot = GetSaveSlot();
            if (!savslot.SaveIsValid())
            {
                displayPlayerName = "";
                buttonChangeName.Enabled = false;
                buttonResetDeaths.Enabled = false;
            }
            else
            {
                buttonChangeName.Enabled = true;
                buttonResetDeaths.Enabled = true;
            }
            displayPlayerName = savslot.GetPlayerName();
            Refresh(); // Update the screen, including the player name
        }

        public void UpdatePlayerName(string _str)
        {
            var savslot = GetSaveSlot();
            if (!savslot.SaveIsValid())
            {
                displayPlayerName = "";
                buttonChangeName.Enabled = false;
                buttonResetDeaths.Enabled = false;
            }
            else
            {
                buttonChangeName.Enabled = true;
                buttonResetDeaths.Enabled = true;
            }
            displayPlayerName = _str;
            Refresh(); // Update the screen, including the player name
        }

        private void buttonApplyChanges_Click(object sender, EventArgs e)
        {
            buttonWrite.Enabled = false;
            UpdatePlayerName();
        }

        private void UpdateArrowsMax()
        {
            switch (numericUpDownArrowUpgrades.Value)
            {
                default:
                    numericUpDownArrowsHeld.Maximum = 30;
                    break;
                case 1:
                    numericUpDownArrowsHeld.Maximum = 35;
                    break;
                case 2:
                    numericUpDownArrowsHeld.Maximum = 40;
                    break;
                case 3:
                    numericUpDownArrowsHeld.Maximum = 45;
                    break;
                case 4:
                    numericUpDownArrowsHeld.Maximum = 50;
                    break;
                case 5:
                    numericUpDownArrowsHeld.Maximum = 55;
                    break;
                case 6:
                    numericUpDownArrowsHeld.Maximum = 60;
                    break;
                case 7:
                    numericUpDownArrowsHeld.Maximum = 70;
                    break;
            }
        }

        private void UpdateBombsMax()
        {
            switch (numericUpDownBombUpgrades.Value)
            {
                default:
                    numericUpDownBombsHeld.Maximum = 10;
                    break;
                case 1:
                    numericUpDownBombsHeld.Maximum = 15;
                    break;
                case 2:
                    numericUpDownBombsHeld.Maximum = 20;
                    break;
                case 3:
                    numericUpDownBombsHeld.Maximum = 25;
                    break;
                case 4:
                    numericUpDownBombsHeld.Maximum = 30;
                    break;
                case 5:
                    numericUpDownBombsHeld.Maximum = 35;
                    break;
                case 6:
                    numericUpDownBombsHeld.Maximum = 40;
                    break;
                case 7:
                    numericUpDownBombsHeld.Maximum = 50;
                    break;
            }
        }

        private void UpdateAllConfigurables(SaveSlot savslot)
        {
            if (!savslot.SaveIsValid())
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
            if (player.GetCurrMagicUpgrade() >= 0x2)
                textQuarterMagic.Visible = true;
            else
                textQuarterMagic.Visible = false;

            // Magic Bar Upgrades

            switch (player.GetItemEquipment(magicUpgrades))
            {
                default:
                case 0x0:
                    pictureBoxMagicBar.Image = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar;
                    break;
                case 0x1:
                    pictureBoxMagicBar.Image = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar_halved;
                    break;
                case 0x2:
                    pictureBoxMagicBar.Image = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar_quarter;
                    break;
            }

            // Bow and Arrows
            switch (player.GetItemEquipment(bow))
            {
                default:
                case 0x0:
                    bowOptionNone.Checked = true;
                    pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.D_Bow;
                    break;
                case 0x1:
                    bowOption1.Checked = true;
                    pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow;
                    break;
                case 0x2:
                    bowOption2.Checked = true;
                    pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow_and_Arrow;
                    break;
                case 0x3:
                    bowOption3.Checked = true;
                    pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow_and_Light_Arrow;
                    break;
                case 0x4:
                    bowOption4.Checked = true;
                    pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow_and_Light_Arrow;
                    break;
            }

            numericUpDownArrowUpgrades.Value = player.GetCurrArrowUpgrades();
            UpdateArrowsMax();
            var _arrowCount = player.GetHeldArrows();
            if (_arrowCount > numericUpDownArrowsHeld.Maximum)
                _arrowCount = (int)numericUpDownArrowsHeld.Maximum;
            numericUpDownArrowsHeld.Value = _arrowCount;

            // Bombs
            numericUpDownBombUpgrades.Value = player.GetCurrBombUpgrades();
            UpdateBombsMax();
            var _bombCount = player.GetHeldBombs();
            if (_bombCount > numericUpDownBombsHeld.Maximum)
                _bombCount = (int)numericUpDownBombsHeld.Maximum;
            numericUpDownBombsHeld.Value = _bombCount;

            if (numericUpDownBombsHeld.Value <= 0)
                pictureBombs.Image = ALTTPSRAMEditor.Properties.Resources.D_Bomb;
            else
                pictureBombs.Image = ALTTPSRAMEditor.Properties.Resources.Bomb;

            // Boomerang
            switch (player.GetItemEquipment(boomerang))
            {
                default:
                case 0x0:
                    radioButtonNoBoomerang.Checked = true;
                    pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.D_Boomerang;
                    break;
                case 0x1:
                    radioButtonBlueBoomerang.Checked = true;
                    pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.Boomerang;
                    break;
                case 0x2:
                    radioButtonRedBoomerang.Checked = true;
                    pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.Magical_Boomerang;
                    break;
            }

            // Shovel and Flute
            switch (player.GetItemEquipment(shovelFlute))
            {
                default:
                case 0x0:
                    radioButtonNoShovelOrFlute.Checked = true;
                    pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.D_Shovel;
                    break;
                case 0x1:
                    radioButtonShovel.Checked = true;
                    pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.Shovel;
                    break;
                case 0x2:
                    radioButtonFlute.Checked = true;
                    pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.Flute;
                    break;
                case 0x3:
                    radioButtonFluteAndBird.Checked = true;
                    pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.Flute;
                    break;
            }

            // Glove Upgrades
            switch (player.GetItemEquipment(gloves))
            {
                default:
                case 0x0:
                    radioButtonNoGloves.Checked = true;
                    picturePowerGlove.Image = ALTTPSRAMEditor.Properties.Resources.D_Power_Glove;
                    break;
                case 0x1:
                    radioButtonPowerGloves.Checked = true;
                    picturePowerGlove.Image = ALTTPSRAMEditor.Properties.Resources.Power_Glove;
                    break;
                case 0x2:
                    radioButtonTitansMitts.Checked = true;
                    picturePowerGlove.Image = ALTTPSRAMEditor.Properties.Resources.Titan_s_Mitt;
                    break;
            }

            // Hookshot
            if (player.GetItemEquipment(hookshot) == 0x1)
                pictureHookshot.Image = ALTTPSRAMEditor.Properties.Resources.Hookshot;
            else
                pictureHookshot.Image = ALTTPSRAMEditor.Properties.Resources.D_Hookshot;

            // Fire Rod
            if (player.GetItemEquipment(fireRod) == 0x1)
                pictureFireRod.Image = ALTTPSRAMEditor.Properties.Resources.Fire_Rod;
            else
                pictureFireRod.Image = ALTTPSRAMEditor.Properties.Resources.D_Fire_Rod;

            // Ice Rod
            if (player.GetItemEquipment(iceRod) == 0x1)
                pictureIceRod.Image = ALTTPSRAMEditor.Properties.Resources.Ice_Rod;
            else
                pictureIceRod.Image = ALTTPSRAMEditor.Properties.Resources.D_Ice_Rod;

            // Bombos
            if (player.GetItemEquipment(bombosMedallion) == 0x1)
                pictureBombos.Image = ALTTPSRAMEditor.Properties.Resources.Bombos;
            else
                pictureBombos.Image = ALTTPSRAMEditor.Properties.Resources.D_Bombos;

            // Ether
            if (player.GetItemEquipment(etherMedallion) == 0x1)
                pictureEther.Image = ALTTPSRAMEditor.Properties.Resources.Ether;
            else
                pictureEther.Image = ALTTPSRAMEditor.Properties.Resources.D_Ether;

            // Quake
            if (player.GetItemEquipment(quakeMedallion) == 0x1)
                pictureQuake.Image = ALTTPSRAMEditor.Properties.Resources.Quake;
            else
                pictureQuake.Image = ALTTPSRAMEditor.Properties.Resources.D_Quake;

            // Lamp
            if (player.GetItemEquipment(lamp) == 0x1)
                pictureLamp.Image = ALTTPSRAMEditor.Properties.Resources.Lamp;
            else
                pictureLamp.Image = ALTTPSRAMEditor.Properties.Resources.D_Lamp;

            // Magic Hammer
            if (player.GetItemEquipment(magicHammer) == 0x1)
                pictureMagicHammer.Image = ALTTPSRAMEditor.Properties.Resources.Magic_Hammer;
            else
                pictureMagicHammer.Image = ALTTPSRAMEditor.Properties.Resources.D_Magic_Hammer;

            // Bug Catching Net
            if (player.GetItemEquipment(bugNet) == 0x1)
                pictureBugCatchingNet.Image = ALTTPSRAMEditor.Properties.Resources.Bug_Catching_Net;
            else
                pictureBugCatchingNet.Image = ALTTPSRAMEditor.Properties.Resources.D_Bug_Catching_Net;

            // Book of Mudora
            if (player.GetItemEquipment(book) == 0x1)
                pictureBookOfMudora.Image = ALTTPSRAMEditor.Properties.Resources.Book_of_Mudora;
            else
                pictureBookOfMudora.Image = ALTTPSRAMEditor.Properties.Resources.D_Book_of_Mudora;

            // Cane of Somaria
            if (player.GetItemEquipment(caneOfSomaria) == 0x1)
                pictureCaneOfSomaria.Image = ALTTPSRAMEditor.Properties.Resources.Cane_of_Somaria;
            else
                pictureCaneOfSomaria.Image = ALTTPSRAMEditor.Properties.Resources.D_Cane_of_Somaria;

            // Cane of Byrna
            if (player.GetItemEquipment(caneOfByrna) == 0x1)
                pictureCaneOfByrna.Image = ALTTPSRAMEditor.Properties.Resources.Cane_of_Byrna;
            else
                pictureCaneOfByrna.Image = ALTTPSRAMEditor.Properties.Resources.D_Cane_of_Byrna;

            // Magic Cape
            if (player.GetItemEquipment(magicCape) == 0x1)
                pictureMagicCape.Image = ALTTPSRAMEditor.Properties.Resources.Magic_Cape;
            else
                pictureMagicCape.Image = ALTTPSRAMEditor.Properties.Resources.D_Magic_Cape;

            // Magic Mirror
            if (player.GetItemEquipment(magicMirror) == 0x2)
                pictureMagicMirror.Image = ALTTPSRAMEditor.Properties.Resources.Magic_Mirror;
            else
                pictureMagicMirror.Image = ALTTPSRAMEditor.Properties.Resources.D_Magic_Mirror;

            // Moon Pearl
            if (player.GetItemEquipment(moonPearl) == 0x1)
                pictureMoonPearl.Image = ALTTPSRAMEditor.Properties.Resources.Moon_Pearl;
            else
                pictureMoonPearl.Image = ALTTPSRAMEditor.Properties.Resources.D_Moon_Pearl;

            var aflags = player.GetAbilityFlags(); // Grab the ability flags from this save slot

            if (GetBit(aflags, 0x2) && player.GetItemEquipment(pegasusBoots) > 0x0) // Test for Pegasus Boots
                pictureBoots.Image = ALTTPSRAMEditor.Properties.Resources.Pegasus_Boots;
            else
                pictureBoots.Image = ALTTPSRAMEditor.Properties.Resources.D_Pegasus_Boots;

            if (GetBit(aflags, 0x1) && player.GetItemEquipment(zorasFlippers) > 0x0) // Test for Zora's Flippers
                pictureZorasFlippers.Image = ALTTPSRAMEditor.Properties.Resources.Zora_s_Flippers;
            else
                pictureZorasFlippers.Image = ALTTPSRAMEditor.Properties.Resources.D_Zora_s_Flippers;

            switch (player.GetItemEquipment(mushroomPowder))
            {
                default:
                case 0x0:
                    radioButtonNoMushPowd.Checked = true;
                    pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.D_Mushroom;
                    break;
                case 0x1:
                    radioButtonMushroom.Checked = true;
                    pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.Mushroom;
                    break;
                case 0x2:
                    radioButtonPowder.Checked = true;
                    pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.Magic_Powder;
                    break;
            }

            switch (player.GetItemEquipment(sword))
            {
                default:
                case 0x0:
                    radioButtonNoSword.Checked = true;
                    pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.D_Fighter_s_Sword;
                    break;
                case 0x1:
                    radioButtonFighterSword.Checked = true;
                    pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Sword;
                    break;
                case 0x2:
                    radioButtonMasterSword.Checked = true;
                    pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Master_Sword;
                    break;
                case 0x3:
                    radioButtonTemperedSword.Checked = true;
                    pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Tempered_Sword;
                    break;
                case 0x4:
                    radioButtonGoldenSword.Checked = true;
                    pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Golden_Sword;
                    break;
            }

            switch (player.GetItemEquipment(shield))
            {
                default:
                case 0x0:
                    radioButtonNoShield.Checked = true;
                    pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.D_Fighter_s_Shield;
                    break;
                case 0x1:
                    radioButtonBlueShield.Checked = true;
                    pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Shield;
                    break;
                case 0x2:
                    radioButtonHerosShield.Checked = true;
                    pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Red_Shield;
                    break;
                case 0x3:
                    radioButtonMirrorShield.Checked = true;
                    pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Mirror_Shield;
                    break;
            }

            switch (player.GetItemEquipment(armor))
            {
                default:
                case 0x0:
                    radioButtonGreenMail.Checked = true;
                    pictureMail.Image = ALTTPSRAMEditor.Properties.Resources.Green_Tunic;
                    break;
                case 0x1:
                    radioButtonBlueMail.Checked = true;
                    pictureMail.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Tunic;
                    break;
                case 0x2:
                    radioButtonRedMail.Checked = true;
                    pictureMail.Image = ALTTPSRAMEditor.Properties.Resources.Red_Tunic;
                    break;
            }

            // Update the picture so it represents what the inventory bottle should actually have
            var _inventoryBottleFill = 0;
            if (player.GetItemEquipment(bottle1Contents) > 0)
                _inventoryBottleFill = player.GetItemEquipment(bottle1Contents);
            else if (player.GetItemEquipment(bottle2Contents) > 0)
                _inventoryBottleFill = player.GetItemEquipment(bottle2Contents);
            else if (player.GetItemEquipment(bottle3Contents) > 0)
                _inventoryBottleFill = player.GetItemEquipment(bottle3Contents);
            else if (player.GetItemEquipment(bottle4Contents) > 0)
                _inventoryBottleFill = player.GetItemEquipment(bottle4Contents);
            else
                pictureBottles.Image = ALTTPSRAMEditor.Properties.Resources.D_Bottle;

            if (_inventoryBottleFill == 1)
                _inventoryBottleFill = 9;
            if (_inventoryBottleFill - 1 < 0)
                _inventoryBottleFill = 1;

            pictureBottles.Image = bottleContentsImg[bottleContents[_inventoryBottleFill - 1]];

            // Fill the 1st bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
            var fillContents = player.GetItemEquipment(bottle1Contents);
            if (fillContents == 1)
                fillContents = 9;
            if (fillContents - 1 < 0)
            {
                comboBoxBottle1.SelectedIndex = 0;
                fillContents = 1;
            }
            else
                comboBoxBottle1.SelectedIndex = fillContents - 1;
            // Update the picture so it represents what the 1st bottle actually has
            pictureBottle1.Image = bottleContentsImg[bottleContents[fillContents - 1]];


            // Fill the 2nd bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
            fillContents = player.GetItemEquipment(bottle2Contents);
            if (fillContents == 1)
                fillContents = 9;
            if (fillContents - 1 < 0)
            {
                comboBoxBottle2.SelectedIndex = 0;
                fillContents = 1;
            }
            else
                comboBoxBottle2.SelectedIndex = fillContents - 1;
            // Update the picture so it represents what the 2nd bottle actually has
            pictureBottle2.Image = bottleContentsImg[bottleContents[fillContents - 1]];


            // Fill the 3rd bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
            fillContents = player.GetItemEquipment(bottle3Contents);
            if (fillContents == 1)
                fillContents = 9;
            if (fillContents - 1 < 0)
            {
                comboBoxBottle3.SelectedIndex = 0;
                fillContents = 1;
            }
            else
                comboBoxBottle3.SelectedIndex = fillContents - 1;
            // Update the picture so it represents what the 3rd bottle actually has
            pictureBottle3.Image = bottleContentsImg[bottleContents[fillContents - 1]];



            // Fill the 4th bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
            fillContents = player.GetItemEquipment(bottle4Contents);
            if (fillContents == 1)
                fillContents = 9;
            if (fillContents - 1 < 0)
            {
                comboBoxBottle4.SelectedIndex = 0;
                fillContents = 1;
            }
            else
                comboBoxBottle4.SelectedIndex = fillContents - 1;
            // Update the picture so it represents what the 4th bottle actually has
            pictureBottle4.Image = bottleContentsImg[bottleContents[fillContents - 1]];

            // Update the pendants
            var _pendants = GetSaveSlot().GetPendants();

            if (!GetBit(_pendants, greenPendant)) // Test for green pendant
                pictureGreenPendant.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Pendant;
            else
                pictureGreenPendant.Image = ALTTPSRAMEditor.Properties.Resources.Green_Pendant;

            if (!GetBit(_pendants, bluePendant)) // Test for blue pendant
                pictureBluePendant.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Pendant;
            else
                pictureBluePendant.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Pendant;

            if (!GetBit(_pendants, redPendant)) // Test for red pendant
                pictureRedPendant.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Pendant;
            else
                pictureRedPendant.Image = ALTTPSRAMEditor.Properties.Resources.Red_Pendant;

            // Update the crystals
            var _crystals = GetSaveSlot().GetCrystals();

            if (!GetBit(_crystals, crystalPoD)) // Test for PoD Crystal
                pictureCrystalPoD.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalPoD.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            if (!GetBit(_crystals, crystalSP)) // Test for SP Crystal
                pictureCrystalSP.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalSP.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            if (!GetBit(_crystals, crystalSW)) // Test for SW Crystal
                pictureCrystalSW.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalSW.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            if (!GetBit(_crystals, crystalTT)) // Test for TT Crystal
                pictureCrystalTT.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalTT.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            if (!GetBit(_crystals, crystalIP)) // Test for IP Crystal
                pictureCrystalIP.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalIP.Image = ALTTPSRAMEditor.Properties.Resources.Red_Crystal;

            if (!GetBit(_crystals, crystalMM)) // Test for MM Crystal
                pictureCrystalMM.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalMM.Image = ALTTPSRAMEditor.Properties.Resources.Red_Crystal;

            if (!GetBit(_crystals, crystalTR)) // Test for TR Crystal
                pictureCrystalTR.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalTR.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            // Update the playerName textbox
            UpdatePlayerName();
        }

        private void pictureBow_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBowConfig);

        private SaveSlot GetSaveSlot()
        {
            SaveSlot savslot;

            if (radioFile2.Checked)
            {
                savslot = sdat.GetSaveSlot(2);
            }
            else if (radioFile3.Checked)
            {
                savslot = sdat.GetSaveSlot(3);
            }
            else
            {
                savslot = sdat.GetSaveSlot(1);
            }
            return savslot;
        }

        private void bowRadio(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;

            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();

                switch (btn.Name)
                {
                    case "bowOptionNone":
                        player.SetHasItemEquipment(bow, 0x0); // Give No Bow
                        pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.D_Bow;
                        break;
                    case "bowOption1":
                        player.SetHasItemEquipment(bow, 0x1); // Give Bow
                        pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow;
                        break;
                    case "bowOption2":
                        player.SetHasItemEquipment(bow, 0x2); // Give Bow & Arrows
                        pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow_and_Arrow;
                        break;
                    case "bowOption3":
                        player.SetHasItemEquipment(bow, 0x3); // Give Silver Bow
                        pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow_and_Light_Arrow;
                        break;
                    case "bowOption4":
                        player.SetHasItemEquipment(bow, 0x4); // Give Bow & Silver Arrows
                        pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow_and_Light_Arrow;
                        break;
                }
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
            var btn = sender as RadioButton;

            if (btn != null && btn.Checked)
            {
                var savslot = GetSaveSlot();
                var player = savslot.GetPlayer();
                UpdatePlayerName();
                numericUpDownRupeeCounter.Value = player.GetRupeeValue();
                UpdateAllConfigurables(savslot);
                if (!savslot.SaveIsValid())
                {
                    helperText.Text = "Save slot " + savslot.ToString() + " is empty or invalid.";
                }
                else
                {
                    helperText.Text = "Editing Save slot " + savslot.ToString() + ".";
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!fname.Equals("") && fileOpen)
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
                e.Graphics.FillRectangle(rectBrush, new Rectangle(223 - border, 49 - border, (8 * 8) + (border * 2), 16 + (border * 2)));

                // Grab the player so we can get their info
                var player = savslot.GetPlayer();

                // Now we'll loop through and draw all the hearts as required to represent the player's health
                // First, create a blank canvas so we can draw to it (this is abstract, won't show up on the screen, it's just in the computer memory)
                var fillRect = new Rectangle(0, 0, 84, 20);
                var tex = new Bitmap(fillRect.Width, fillRect.Height);

                // Draw onto the blank canvas we created
                using (var gr = Graphics.FromImage(tex))
                {
                    // Apply some pixel-perfect settings before we draw anything...
                    gr.Clear(Color.Transparent);
                    gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                    gr.PixelOffsetMode = PixelOffsetMode.Half;

                    double heartContainers = player.GetHeartContainers();
                    for (var i = 0; i < heartContainers / 8; i++)
                    {
                        var xOff = (i % 10) * 8;
                        var yOff = (i / 10) * 8;
                        if (i >= (heartContainers / 8.0f - 1.0f) && heartContainers % 8 != 0)
                            gr.DrawImage(imgHeartContainerPartial, 2 + xOff, 2 + yOff);
                        else
                            gr.DrawImage(imgHeartContainerFull, 2 + xOff, 2 + yOff);
                    }
                }

                // Write all the contents of the canvas into the heart container preview PictureBox on the Form
                pictureBoxHeartContainerPreview.Image = tex;

                // Now, we'll draw the magic bar based on the upgrade/values the save slot has
                Image magicContainer;
                switch (player.GetCurrMagicUpgrade())
                {
                    default:
                    case 0x0:
                        magicContainer = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar;
                        break;
                    case 0x1:
                        magicContainer = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar_halved;
                        break;
                    case 0x2:
                        magicContainer = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar_quarter;
                        break;
                }

                // Create another blank canvas so we can draw to it
                fillRect = new Rectangle(0, 0, magicContainer.Width, magicContainer.Height);
                tex = new Bitmap(fillRect.Width, fillRect.Height);

                // Draw onto the blank canvas we created
                using (var gr = Graphics.FromImage(tex))
                {
                    // Apply some pixel-perfect settings before we draw anything...
                    gr.Clear(Color.Transparent);
                    gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                    gr.PixelOffsetMode = PixelOffsetMode.Half;

                    // Draw the empty magic bar container into the canvas (not to the screen)
                    var currMagic = player.GetCurrMagic();
                    gr.DrawImage(magicContainer, 0, 0);

                    // Using a colored rectangle, fill the magic bar with a bar representing magic, based on what the player's current magic value is
                    rectBrush.Color = ColorTranslator.FromHtml("#FF21C329");
                    fillRect = new Rectangle(0 + 4, 0 - ((currMagic + 3) / 4) + 40, 8, ((currMagic + 3) / 4));
                    gr.FillRectangle(rectBrush, fillRect);

                    // If necessary, draw the white "1-pixel-line" part of the magic bar fill, for aesthetic purposes
                    if (currMagic > 0)
                    {
                        rectBrush.Color = ColorTranslator.FromHtml("#FFFFFBFF");
                        fillRect = new Rectangle(5, -((currMagic + 3) / 4) + 40, 6, 1);
                        gr.FillRectangle(rectBrush, fillRect); // Could probably use a pen w/ DrawLine() here, but might as well recycle the rectangle
                    }
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
            if (saveRegion == (int)SaveRegion.JPN)
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
                    else if (c == '－' || c == '-')
                    {
                        letter = 'ー'; // Replace katanana － and/or Romaji - to hiragana ー because they're exactly the same in the game code
                    }
                    DrawTile(jpn_fnt, jpChar[letter], e, pos);
                    pos += 16;
                    i++;
                }
            }
            else if (saveRegion == (int)SaveRegion.USA)
            {
                pos = 0;
                foreach (var c in displayPlayerName)
                {
                    DrawTile(en_fnt, enChar[c], e, pos);
                    pos += 8;
                }
            }
        }

        public static void DrawTile(Image source, int tileID, PaintEventArgs e, int pos)
        {
            var tileset_width = 27; // English Font
            if (saveRegion == (int)SaveRegion.JPN)
                tileset_width = 20; // Japanese Font

            var tile_w = 8;
            var tile_h = 16;
            var x = (tileID % tileset_width) * tile_w;
            var y = (tileID / tileset_width) * tile_h;
            var width = 8;
            var height = 16;
            var crop = new Rectangle(x, y, width, height);
            var bmp = new Bitmap(crop.Width, crop.Height);

            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }

            e.Graphics.DrawImage(bmp, 223 + pos, 49);
        }

        private void buttonChangeName_Click(object sender, EventArgs e)
        {
            if (saveRegion == (int)SaveRegion.JPN)
            {
                var nameForm = new NameChangingFormJP(this);
                nameForm.ShowDialog();
            }
            else
            {
                var nameForm = new NameChangingFormEN(this);
                nameForm.ShowDialog();
            }
        }

        private void numericUpDownArrowsHeld_ValueChanged(object sender, EventArgs e)
        {
            var player = GetSaveSlot().GetPlayer();
            player.SetHasItemEquipment(arrowCount, (byte)numericUpDownArrowsHeld.Value); // Set the new arrow count value
        }

        private void pictureBox1_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBoomerangConfig);

        private void HideAllGroupBoxesExcept(GroupBox currentGroupBox)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(GroupBox))
                {
                    if (!c.Equals(currentGroupBox) && !c.Equals(groupInventory) && !c.Equals(groupFileSelect) && !c.Equals(groupPendantsCrystals) && !c.Equals(groupBoxHUD))
                        c.Visible = false;
                }
            }
            currentGroupBox.Visible = true;
        }

        private void boomerangRadio(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoBoomerang":
                        player.SetHasItemEquipment(boomerang, 0x0); // Give No Boomerang
                        pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.D_Boomerang;
                        break;
                    case "radioButtonBlueBoomerang":
                        player.SetHasItemEquipment(boomerang, 0x1); // Give Blue Boomerang
                        pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.Boomerang;
                        break;
                    case "radioButtonRedBoomerang":
                        player.SetHasItemEquipment(boomerang, 0x2); // Give Red Boomerang
                        pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.Magical_Boomerang;
                        break;
                }
            }
        }

        private void pictureHookshot_Click(object sender, EventArgs e) => ToggleItem(hookshot, 0x1, pictureHookshot, ALTTPSRAMEditor.Properties.Resources.Hookshot, ALTTPSRAMEditor.Properties.Resources.D_Hookshot);

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
            if (numericUpDownBombsHeld.Value <= 0)
                pictureBombs.Image = ALTTPSRAMEditor.Properties.Resources.D_Bomb;
            else
                pictureBombs.Image = ALTTPSRAMEditor.Properties.Resources.Bomb;
        }

        private void pictureBombs_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxBombs);

        private void pictureMushPowd_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxMushroomPowder);

        private void mushPowdRadio(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoMushPowd":
                        player.SetHasItemEquipment(mushroomPowder, 0x0); // Give Neither Mushroom nor Powder
                        pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.D_Mushroom;
                        break;
                    case "radioButtonMushroom":
                        player.SetHasItemEquipment(mushroomPowder, 0x1); // Give Mushroom
                        pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.Mushroom;
                        break;
                    case "radioButtonPowder":
                        player.SetHasItemEquipment(mushroomPowder, 0x2); // Give Magic Powder
                        pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.Magic_Powder;
                        break;
                }
            }
        }

        private void swordRadio(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoSword":
                        player.SetHasItemEquipment(sword, 0x0); // Give No Sword
                        pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.D_Fighter_s_Sword;
                        break;
                    case "radioButtonFighterSword":
                        player.SetHasItemEquipment(sword, 0x1); // Give Fighter's Sword
                        pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Sword;
                        break;
                    case "radioButtonMasterSword":
                        player.SetHasItemEquipment(sword, 0x2); // Give Master Sword
                        pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Master_Sword;
                        break;
                    case "radioButtonTemperedSword":
                        player.SetHasItemEquipment(sword, 0x3); // Give Tempered Sword
                        pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Tempered_Sword;
                        break;
                    case "radioButtonGoldenSword":
                        player.SetHasItemEquipment(sword, 0x4); // Give Golden Sword
                        pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Golden_Sword;
                        break;
                }
            }
        }

        private void pictureSword_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxSword);

        private void radioShield(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoShield":
                        player.SetHasItemEquipment(shield, 0x0); // Give No Shield
                        pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.D_Fighter_s_Shield;
                        break;
                    case "radioButtonBlueShield":
                        player.SetHasItemEquipment(shield, 0x1); // Give Fighter's Shield
                        pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Shield;
                        break;
                    case "radioButtonHerosShield":
                        player.SetHasItemEquipment(shield, 0x2); // Give Hero's Shield
                        pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Red_Shield;
                        break;
                    case "radioButtonMirrorShield":
                        player.SetHasItemEquipment(shield, 0x3); // Give Mirror Shield
                        pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Mirror_Shield;
                        break;
                }
            }
        }

        private void pictureShield_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxShield);

        private void pictureMail_Click(object sender, EventArgs e) => HideAllGroupBoxesExcept(groupBoxMails);

        private void mailRadio(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonGreenMail":
                        player.SetHasItemEquipment(armor, 0x0); // Give Green Mail
                        pictureMail.Image = ALTTPSRAMEditor.Properties.Resources.Green_Tunic;
                        break;
                    case "radioButtonBlueMail":
                        player.SetHasItemEquipment(armor, 0x1); // Give Blue Mail
                        pictureMail.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Tunic;
                        break;
                    case "radioButtonRedMail":
                        player.SetHasItemEquipment(armor, 0x2); // Give Red Mail
                        pictureMail.Image = ALTTPSRAMEditor.Properties.Resources.Red_Tunic;
                        break;
                }
            }
        }

        private void CheckForBottles()
        {
            var player = GetSaveSlot().GetPlayer();
            // Update the picture so it represents what the inventory bottle should actually have
            var _inventoryBottleFill = 0;

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
                _inventoryBottleFill = 9;
            if (_inventoryBottleFill - 1 < 0)
                _inventoryBottleFill = 1;

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
                pictureBoots.Image = ALTTPSRAMEditor.Properties.Resources.D_Pegasus_Boots;
                flags &= 0xFB; // To turn it off, bitwise and with b11111011
                player.SetHasItemEquipment(pegasusBoots, 0x0);
                player.SetHasItemEquipment(abilityFlags, flags);
            }
            else
            {
                pictureBoots.Image = ALTTPSRAMEditor.Properties.Resources.Pegasus_Boots;
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
                pictureZorasFlippers.Image = ALTTPSRAMEditor.Properties.Resources.D_Zora_s_Flippers;
                flags &= 0xFD; // To turn it off, bitwise and with b11111101
                player.SetHasItemEquipment(zorasFlippers, 0x0);
                player.SetHasItemEquipment(abilityFlags, flags);
            }
            else
            {
                pictureZorasFlippers.Image = ALTTPSRAMEditor.Properties.Resources.Zora_s_Flippers;
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

        private void pictureMagicMirror_Click(object sender, EventArgs e) => ToggleItem(magicMirror, 0x2, pictureMagicMirror, ALTTPSRAMEditor.Properties.Resources.Magic_Mirror, ALTTPSRAMEditor.Properties.Resources.D_Magic_Mirror);

        private void pictureFireRod_Click(object sender, EventArgs e) => ToggleItem(fireRod, 0x1, pictureFireRod, ALTTPSRAMEditor.Properties.Resources.Fire_Rod, ALTTPSRAMEditor.Properties.Resources.D_Fire_Rod);

        private void pictureIceRod_Click(object sender, EventArgs e) => ToggleItem(iceRod, 0x1, pictureIceRod, ALTTPSRAMEditor.Properties.Resources.Ice_Rod, ALTTPSRAMEditor.Properties.Resources.D_Ice_Rod);

        private void pictureBombos_Click(object sender, EventArgs e) => ToggleItem(bombosMedallion, 0x1, pictureBombos, ALTTPSRAMEditor.Properties.Resources.Bombos, ALTTPSRAMEditor.Properties.Resources.D_Bombos);

        private void pictureEther_Click(object sender, EventArgs e) => ToggleItem(etherMedallion, 0x1, pictureEther, ALTTPSRAMEditor.Properties.Resources.Ether, ALTTPSRAMEditor.Properties.Resources.D_Ether);

        private void pictureQuake_Click(object sender, EventArgs e) => ToggleItem(quakeMedallion, 0x1, pictureQuake, ALTTPSRAMEditor.Properties.Resources.Quake, ALTTPSRAMEditor.Properties.Resources.D_Quake);

        private void pictureLamp_Click(object sender, EventArgs e) => ToggleItem(lamp, 0x1, pictureLamp, ALTTPSRAMEditor.Properties.Resources.Lamp, ALTTPSRAMEditor.Properties.Resources.D_Lamp);

        private void pictureMagicHammer_Click(object sender, EventArgs e) => ToggleItem(magicHammer, 0x1, pictureMagicHammer, ALTTPSRAMEditor.Properties.Resources.Magic_Hammer, ALTTPSRAMEditor.Properties.Resources.D_Magic_Hammer);

        private void pictureBugCatchingNet_Click(object sender, EventArgs e) => ToggleItem(bugNet, 0x1, pictureBugCatchingNet, ALTTPSRAMEditor.Properties.Resources.Bug_Catching_Net, ALTTPSRAMEditor.Properties.Resources.D_Bug_Catching_Net);

        private void pictureBookOfMudora_Click(object sender, EventArgs e) => ToggleItem(book, 0x1, pictureBookOfMudora, ALTTPSRAMEditor.Properties.Resources.Book_of_Mudora, ALTTPSRAMEditor.Properties.Resources.D_Book_of_Mudora);

        private void pictureCaneOfSomaria_Click(object sender, EventArgs e) => ToggleItem(caneOfSomaria, 0x1, pictureCaneOfSomaria, ALTTPSRAMEditor.Properties.Resources.Cane_of_Somaria, ALTTPSRAMEditor.Properties.Resources.D_Cane_of_Somaria);

        private void pictureCaneOfByrna_Click(object sender, EventArgs e) => ToggleItem(caneOfByrna, 0x1, pictureCaneOfByrna, ALTTPSRAMEditor.Properties.Resources.Cane_of_Byrna, ALTTPSRAMEditor.Properties.Resources.D_Cane_of_Byrna);

        private void pictureMagicCape_Click(object sender, EventArgs e) => ToggleItem(magicCape, 0x1, pictureMagicCape, ALTTPSRAMEditor.Properties.Resources.Magic_Cape, ALTTPSRAMEditor.Properties.Resources.D_Magic_Cape);

        private void pictureMoonPearl_Click(object sender, EventArgs e) => ToggleItem(moonPearl, 0x1, pictureMoonPearl, ALTTPSRAMEditor.Properties.Resources.Moon_Pearl, ALTTPSRAMEditor.Properties.Resources.D_Moon_Pearl);

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
            var btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    case "radioButtonNoShovelOrFlute":
                        player.SetHasItemEquipment(shovelFlute, 0x0); // Give neither Shovel nor Flute
                        pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.D_Shovel;
                        break;
                    case "radioButtonShovel":
                        player.SetHasItemEquipment(shovelFlute, 0x1); // Give Shovel
                        pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.Shovel;
                        break;
                    case "radioButtonFlute":
                        player.SetHasItemEquipment(shovelFlute, 0x2); // Give Flute
                        pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.Flute;
                        break;
                    case "radioButtonFluteAndBird":
                        player.SetHasItemEquipment(shovelFlute, 0x3); // Give Flute and Bird
                        pictureShovelFlute.Image = ALTTPSRAMEditor.Properties.Resources.Flute;
                        break;
                }
            }
        }

        private void gloveUpgradesRadio(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                var player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    case "radioButtonNoGloves":
                        player.SetHasItemEquipment(gloves, 0x0); // Give neither Power Glove nor Titan's Mitts
                        picturePowerGlove.Image = ALTTPSRAMEditor.Properties.Resources.D_Power_Glove;
                        break;
                    case "radioButtonPowerGloves":
                        player.SetHasItemEquipment(gloves, 0x1); // Give Power Glove
                        picturePowerGlove.Image = ALTTPSRAMEditor.Properties.Resources.Power_Glove;
                        break;
                    case "radioButtonTitansMitts":
                        player.SetHasItemEquipment(gloves, 0x2); // Give Titan's Mitts
                        picturePowerGlove.Image = ALTTPSRAMEditor.Properties.Resources.Titan_s_Mitt;
                        break;
                }
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
            Image outImg;
            var heartPieces = GetSaveSlot().GetPlayer().GetHeartPieces();
            switch (heartPieces % 4)
            {
                default:
                case 0:
                    outImg = ALTTPSRAMEditor.Properties.Resources.Piece_of_Heart_Empty;
                    break;
                case 1:
                    outImg = ALTTPSRAMEditor.Properties.Resources.Piece_of_Heart_Quarter;
                    break;
                case 2:
                    outImg = ALTTPSRAMEditor.Properties.Resources.Piece_of_Heart_Half;
                    break;
                case 3:
                    outImg = ALTTPSRAMEditor.Properties.Resources.Piece_of_Heart_Three_Quarters;
                    break;
            }
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
                        player.SetHeartContainers(playerCurrHearts + 8);
                    player.IncrementHeartPieces();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (playerCurrHearts > 8 && playerCurrHeartPieces > 0)
                {
                    if (player.GetHeartPieces() % 4 == 0)
                        player.SetHeartContainers(playerCurrHearts - 8);
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

            if (GetBit(_pendants, greenPendant)) // Test for green pendant
                pictureGreenPendant.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Pendant;
            else
                pictureGreenPendant.Image = ALTTPSRAMEditor.Properties.Resources.Green_Pendant;

            ToggleCrystalPendant(false, greenPendant);
        }

        private void pictureBluePendant_Click(object sender, EventArgs e)
        {
            var _pendants = GetSaveSlot().GetPendants();

            if (GetBit(_pendants, bluePendant)) // Test for blue pendant
                pictureBluePendant.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Pendant;
            else
                pictureBluePendant.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Pendant;

            ToggleCrystalPendant(false, bluePendant);
        }

        private void pictureRedPendant_Click(object sender, EventArgs e)
        {
            var _pendants = GetSaveSlot().GetPendants();

            if (GetBit(_pendants, redPendant)) // Test for red pendant
                pictureRedPendant.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Pendant;
            else
                pictureRedPendant.Image = ALTTPSRAMEditor.Properties.Resources.Red_Pendant;

            ToggleCrystalPendant(false, redPendant);
        }

        private void pictureCrystalPoD_Click(object sender, EventArgs e)
        {
            var _crystals = GetSaveSlot().GetCrystals();

            if (GetBit(_crystals, crystalPoD)) // Test for PoD Crystal
                pictureCrystalPoD.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalPoD.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            ToggleCrystalPendant(true, crystalPoD);
        }

        private void pictureCrystalSP_Click(object sender, EventArgs e)
        {
            var _crystals = GetSaveSlot().GetCrystals();

            if (GetBit(_crystals, crystalSP)) // Test for SP Crystal
                pictureCrystalSP.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalSP.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            ToggleCrystalPendant(true, crystalSP);
        }

        private void pictureCrystalSW_Click(object sender, EventArgs e)
        {
            var _crystals = GetSaveSlot().GetCrystals();

            if (GetBit(_crystals, crystalSW)) // Test for SW Crystal
                pictureCrystalSW.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalSW.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            ToggleCrystalPendant(true, crystalSW);
        }

        private void pictureCrystalTT_Click(object sender, EventArgs e)
        {

            var _crystals = GetSaveSlot().GetCrystals();

            if (GetBit(_crystals, crystalTT)) // Test for TT Crystal
                pictureCrystalTT.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalTT.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

            ToggleCrystalPendant(true, crystalTT);
        }

        private void pictureCrystalIP_Click(object sender, EventArgs e)
        {

            var _crystals = GetSaveSlot().GetCrystals();

            if (GetBit(_crystals, crystalIP)) // Test for IP Crystal
                pictureCrystalIP.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalIP.Image = ALTTPSRAMEditor.Properties.Resources.Red_Crystal;

            ToggleCrystalPendant(true, crystalIP);
        }

        private void pictureCrystalMM_Click(object sender, EventArgs e)
        {

            var _crystals = GetSaveSlot().GetCrystals();

            if (GetBit(_crystals, crystalMM)) // Test for MM Crystal
                pictureCrystalMM.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalMM.Image = ALTTPSRAMEditor.Properties.Resources.Red_Crystal;

            ToggleCrystalPendant(true, crystalMM);
        }

        private void pictureCrystalTR_Click(object sender, EventArgs e)
        {

            var _crystals = GetSaveSlot().GetCrystals();

            if (GetBit(_crystals, crystalTR)) // Test for TR Crystal
                pictureCrystalTR.Image = ALTTPSRAMEditor.Properties.Resources.Clear_Crystal;
            else
                pictureCrystalTR.Image = ALTTPSRAMEditor.Properties.Resources.Blue_Crystal;

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
                    pictureBoxMagicBar.Image = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar_halved;
                    player.SetMagicUpgrade(0x1);
                    textQuarterMagic.Visible = false;
                    break;
                case 0x1:
                    pictureBoxMagicBar.Image = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar_quarter;
                    player.SetMagicUpgrade(0x2);
                    textQuarterMagic.Visible = true;
                    break;
                case 0x2:
                    pictureBoxMagicBar.Image = ALTTPSRAMEditor.Properties.Resources.lttp_magic_bar;
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
                _bombCount = (int)numericUpDownBombsHeld.Maximum;
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
                _bombCount = (int)numericUpDownArrowsHeld.Maximum;
            numericUpDownArrowsHeld.Value = _bombCount;
        }

        private void buttonResetDeaths_Click(object sender, EventArgs e)
        {
            var savslot = GetSaveSlot();
            var dialogResult = MessageBox.Show("Reset all deaths/saves for this save file?", "Reset Deaths/Saves?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var postGame = MessageBox.Show("Show the deaths on the File Select Screen?", "Show on File Select Screen?", MessageBoxButtons.YesNo);
                if (postGame == DialogResult.No)
                {
                    savslot.ResetFileDeaths(false);
                }
                else
                {
                    savslot.ResetFileDeaths(true);
                }
            }
        }
    }
}