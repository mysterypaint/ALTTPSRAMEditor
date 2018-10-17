using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ALTTPSRAMEditor
{
    public partial class Form1 : Form
    {
        const int srm_size = 0x2000;
        const int srm_randomizer_size = 16 * 1024;
        const int srm_randomizer_size_2 = 32 * 1024;

        // Items and Equipment (All these values are relative, just subtract 0x340 from their actual SRAM values
        const int bow = 0x0;
        const int boomerang = 0x1;
        const int hookshot = 0x2;
        const int bombCount = 0x3;
        const int mushroomPowder = 0x4;
        const int fireRod = 0x5;
        const int iceRod = 0x6;
        const int bombosMedallion = 0x7;
        const int etherMedallion = 0x8;
        const int quakeMedallion = 0x9;
        const int lamp = 0xA;
        const int magicHammer = 0xB;
        const int shovelFlute = 0xC;
        const int bugNet = 0xD;
        const int book = 0xE;
        const int bottle = 0xF;
        const int caneOfSomaria = 0x10;
        const int caneOfByrna = 0x11;
        const int magicCape = 0x12;
        const int magicMirror = 0x13;
        const int gloves = 0x14;
        const int pegasusBoots = 0x15;
        const int zorasFlippers = 0x16;
        const int moonPearl = 0x17;
        const int skipthis = 0x18;
        const int sword = 0x19;
        const int shield = 0x1A;
        const int armor = 0x1B;
        const int bottle1Contents = 0x1C;
        const int bottle2Contents = 0x1D;
        const int bottle3Contents = 0x1E;
        const int bottle4Contents = 0x1F;
        const int wallet = 0x20; // 2 bytes
        const int rupees = 0x22; // 2 bytes

        const int abilityFlags = 0x39;
        const int arrowCount = 0x37;
        const int bombUpgrades = 0x30;
        const int arrowUpgrades = 0x31;

        static int[] bottleContents = new int[9];
        static System.Drawing.Bitmap[] bottleContentsImg = new System.Drawing.Bitmap[9];

        public enum SaveRegion : int
        {
            USA, JPN, EUR
        };

        public enum BottleContents : int
        {
            NONE, MUSHROOM, EMPTY, RED_POTION, GREEN_POTION, BLUE_POTION, FAERIE, BEE, GOOD_BEE
        };

        static int pos = 0;
        static int saveRegion = (int) SaveRegion.USA;
        static SRAM sdat;
        static String fname = "";
        static String displayPlayerName = "";

        // Initialize the font data
        Image en_fnt = ALTTPSRAMEditor.Properties.Resources.en_font;
        Image jpn_fnt = ALTTPSRAMEditor.Properties.Resources.jpn_font;
        public static Dictionary<char, int> enChar = new Dictionary<char, int>();
        public static Dictionary<char, int> jpChar = new Dictionary<char, int>();
        public static Dictionary<UInt16, char> rawENChar = new Dictionary<UInt16, char>();
        public static Dictionary<UInt16, char> rawJPChar = new Dictionary<UInt16, char>();
        static Data dataHandler = new Data(enChar, jpChar, rawENChar, rawJPChar);

        public Form1()
        {
            InitializeComponent();
        }

        private void opensrmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSRM();
        }

        private void OpenSRM()
        {
            OpenFileDialog fd1 = new OpenFileDialog();
            fd1.Filter = "SRAM|*.srm|All Files|*.*"; // Filter to show.srm files only.
            if (fd1.ShowDialog().Equals(DialogResult.OK))
            { // Prompt the user to open a file, then check if a valid file was opened.
                fname = fd1.FileName;

                try
                { // Open the text file using a File Stream
                    byte[] bytes = File.ReadAllBytes(fname);
                    long fileSize = new System.IO.FileInfo(fname).Length;
                    if (fileSize == srm_randomizer_size || fileSize == srm_randomizer_size_2) MessageBox.Show("Invalid SRAM File. (Randomizer saves aren't supported. Maybe one day...?)");

                    else if (fileSize == srm_size)
                    {
                        Console.Write("Opened " + fname);
                        helperText.Text = "Opened " + fname;
                        sdat = new SRAM(bytes);
                        tableLayoutPanelInventory.Visible = true;
                        labelInventory.Visible = true;
                        numericUpDownRupeeCounter.Visible = true;
                        labelRupees.Visible = true;
                        labelFilename.Visible = true;
                        radioFile1.Enabled = true;
                        radioFile2.Enabled = true;
                        radioFile3.Enabled = true;
                        buttonCopy.Enabled = true;
                        buttonErase.Enabled = true;
                        SaveSlot savslot = sdat.GetSaveSlot(1);
                        buttonChangeName.Visible = true;
                        buttonChangeName.Enabled = true;

                        // Determine the overall region of the .srm
                        SaveSlot savslot2 = sdat.GetSaveSlot(2);
                        SaveSlot savslot3 = sdat.GetSaveSlot(3);
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

                        updatePlayerName(savslot);
                        Link player = savslot.GetPlayer();
                        UpdateAllConfigurables(savslot);
                        Refresh(); // Update the screen, including the player name
                    }
                    else
                    {
                        MessageBox.Show("Invalid SRAM File.");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("The file could not be read:\n" + e.Message);
                }
            }
        }

        private void SaveSRM()
        {
            if (fname.Equals("") || fname.Equals(null))
            {
                helperText.Text = "Load a file first!";
                return; // Abort saving if there isn't a valid file open.
            }

            byte[] outputData = sdat.MergeSaveData();
            File.WriteAllBytes(fname, outputData);
            helperText.Text = "Saved file at " + fname;
            buttonWrite.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Terminate the program if we select "Exit" in the Menu Bar
            System.Windows.Forms.Application.Exit();
        }

        private void saveCTRLSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSRM();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Define bottle array
            bottleContents[0] = (int) BottleContents.NONE;
            bottleContents[1] = (int) BottleContents.EMPTY;
            bottleContents[2] = (int) BottleContents.RED_POTION;
            bottleContents[3] = (int) BottleContents.GREEN_POTION;
            bottleContents[4] = (int) BottleContents.BLUE_POTION;
            bottleContents[5] = (int) BottleContents.FAERIE;
            bottleContents[6] = (int) BottleContents.BEE;
            bottleContents[7] = (int) BottleContents.GOOD_BEE;
            bottleContents[8] = (int) BottleContents.MUSHROOM;

            bottleContentsImg[bottleContents[0]] = ALTTPSRAMEditor.Properties.Resources.Bottle;
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
            String toolCredits = "ALTTP SRAM Editor\n- Created by mysterypaint 2018\n\nSpecial thanks to alttp.run for the reverse-engineering documentation. http://alttp.run/hacking/index.php?title=SRAM_Map";
            MessageBox.Show(toolCredits);
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
            if (radioFile1.Checked)
            {
                sdat.WriteFile(1);
                helperText.Text = "Wrote to File 1!";
            }
            else if (radioFile2.Checked)
            {
                sdat.WriteFile(2);
                helperText.Text = "Wrote to File 2!";
            }
            else if (radioFile3.Checked)
            {
                sdat.WriteFile(3);
                helperText.Text = "Wrote to File 3!";
            }
        }

        private void buttonErase_Click(object sender, EventArgs e)
        {
            int selFile = 1;
            if (radioFile1.Checked) selFile = 1;
            else if (radioFile2.Checked) selFile = 2;
            else if (radioFile3.Checked) selFile = 3;
            DialogResult dialogResult = MessageBox.Show("You are about to PERMANENTLY ERASE File " + selFile + "! Are you sure you want to erase it? There is no undo!", "Erase File " + selFile + "?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                sdat.EraseFile(selFile);
                helperText.Text = "Erased File " + selFile + ".";
            }
        }

        private void updatePlayerName(SaveSlot savslot)
        {
            if (!savslot.SaveIsValid())
            {
                String playerName = savslot.GetPlayerName();
                buttonChangeName.Enabled = false;
            }
            else
            {
                buttonChangeName.Enabled = true;
            }
            displayPlayerName = savslot.GetPlayerName();
            Refresh(); // Update the screen, including the player name
        }

        private void buttonApplyChanges_Click(object sender, EventArgs e)
        {
            buttonWrite.Enabled = false;
            // Determine which file we're editing, and then load its data.
            SaveSlot savslot;
            
            if (radioFile2.Checked)
            {
                savslot = sdat.GetSaveSlot(2);
            }
            else if (radioFile3.Checked)
            {
                savslot = sdat.GetSaveSlot(3);
            }
            else savslot = sdat.GetSaveSlot(1);

            updatePlayerName(savslot);
        }

        private void UpdateAllConfigurables(SaveSlot savslot)
        {
            Link player = savslot.GetPlayer();
            displayPlayerName = savslot.GetPlayerName();
            numericUpDownRupeeCounter.Value = player.GetRupeeValue();

            switch (player.GetItemEquipment(bow))
            {
                default:
                case 0x0:
                    bowOptionNone.Checked = true;
                    pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow;
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

            numericUpDownArrowsHeld.Value = player.GetItemEquipment(arrowCount);
            numericUpDownBombsHeld.Value = player.GetItemEquipment(bombCount);
            switch (player.GetItemEquipment(boomerang))
            {
                default:
                case 0x0:
                    radioButtonNoBoomerang.Checked = true;
                    pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.Boomerang;
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

            switch (player.GetItemEquipment(hookshot))
            {
                default:
                case 0x0:
                    radioButtonNoHookshot.Checked = true;
                    pictureHookshot.Image = ALTTPSRAMEditor.Properties.Resources.Hookshot;
                    break;
                case 0x1:
                    radioButtonHasHookshot.Checked = true;
                    pictureHookshot.Image = ALTTPSRAMEditor.Properties.Resources.Hookshot;
                    break;
            }

            switch (player.GetItemEquipment(mushroomPowder))
            {
                default:
                case 0x0:
                    radioButtonNoMushPowd.Checked = true;
                    pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.Mushroom;
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
                    pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Sword;
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
                    pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Shield;
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

            // Fill the 1st bottle with the value the file has; Remap to actual game values so they match the dropdown list, too
            int fillContents = player.GetItemEquipment(bottle1Contents);
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


            // Update the playerName textbox to emphasize the 6 character limit.
            updatePlayerName(savslot);
        }

        private void pictureBow_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxBowConfig);
        }

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
            else savslot = sdat.GetSaveSlot(1);
            return savslot;
        }

        private void bowRadio(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;

            if (btn != null && btn.Checked)
            {
                Link player = GetSaveSlot().GetPlayer();

                switch (btn.Name)
                {
                    case "bowOptionNone":
                        player.SetHasItemEquipment(bow, 0x0); // Give No Bow
                        pictureBow.Image = ALTTPSRAMEditor.Properties.Resources.Bow;
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
            SaveSlot savslot = GetSaveSlot();
            Link player = savslot.GetPlayer();
            UInt16 val = (UInt16)numericUpDownRupeeCounter.Value;
            player.SetRupees(val);
        }

        private void fileRadio(object sender, EventArgs e)
        {
            // User clicked a radio button to change file save slots
            RadioButton btn = sender as RadioButton;

            if (btn != null && btn.Checked)
            {
                SaveSlot savslot = GetSaveSlot();
                Link player = savslot.GetPlayer();
                updatePlayerName(savslot);
                numericUpDownRupeeCounter.Value = player.GetRupeeValue();
                UpdateAllConfigurables(savslot);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!fname.Equals(""))
            {
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                int border = 2;
                e.Graphics.FillRectangle(myBrush, new Rectangle(223 - border, 49 - border, (8 * 8) + (border * 2), 16 + (border * 2)));
                myBrush.Dispose(); // 223 + pos, 49);
            }

            try
            {
                DrawImageRect(e);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("KeyNotFoundException");
                displayPlayerName = "";
                SaveSlot savslot = GetSaveSlot();
                saveRegion = (int) savslot.GetRegion();
            }
        }

        public void DrawImageRect(PaintEventArgs e)
        {
            if (saveRegion == (int) SaveRegion.JPN)
            {
                pos = 0;
                int i = 0;
                foreach (char c in displayPlayerName)
                {
                    char letter = c;
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
                foreach (char c in displayPlayerName)
                {
                    DrawTile(en_fnt, enChar[c], e, pos);
                    pos += 8;
                }
            }
        }

        public static void DrawTile(Image source, int tileID, PaintEventArgs e, int pos)
        {
            int tileset_width = 27; // English Font
            if (saveRegion == (int)SaveRegion.JPN)
                tileset_width = 20; // Japanese Font

            int tile_w = 8;
            int tile_h = 16;
            int x = (tileID % tileset_width) * tile_w;
            int y = (tileID / tileset_width) * tile_h;
            int width = 8;
            int height = 16;
            Rectangle crop = new Rectangle(x, y, width, height);
            var bmp = new Bitmap(crop.Width, crop.Height);

            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }

            e.Graphics.DrawImage(bmp, 223 + pos, 49);
        }

        private void buttonChangeName_Click(object sender, EventArgs e)
        {
            var newForm = new NameChangingForm();
            newForm.ShowDialog();
        }

        private void numericUpDownArrowsHeld_ValueChanged(object sender, EventArgs e)
        {
            Link player = GetSaveSlot().GetPlayer();
            player.SetHasItemEquipment(arrowCount, (byte) numericUpDownArrowsHeld.Value); // Set the new arrow count value
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxBoomerangConfig);
        }

        private void HideAllGroupBoxesExcept(GroupBox currentGroupBox)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(GroupBox))
                {
                    if (!c.Equals(currentGroupBox) && !c.Equals(groupFileSelect))
                        c.Visible = false;
                }
            }
            currentGroupBox.Visible = true;
        }

        private void boomerangRadio(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                Link player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoBoomerang":
                        player.SetHasItemEquipment(boomerang, 0x0); // Give No Boomerang
                        pictureBox1.Image = ALTTPSRAMEditor.Properties.Resources.Boomerang;
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

        private void hookshotRadio(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                Link player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoHookshot":
                        player.SetHasItemEquipment(hookshot, 0x0); // Give No Hookshot
                        pictureHookshot.Image = ALTTPSRAMEditor.Properties.Resources.Hookshot;
                        break;
                    case "radioButtonHasHookshot":
                        player.SetHasItemEquipment(hookshot, 0x1); // Give Hookshot
                        pictureHookshot.Image = ALTTPSRAMEditor.Properties.Resources.Hookshot;
                        break;
                }
            }
        }

        private void pictureHookshot_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxHookshot);
        }

        private void numericUpDownBombsHeld_ValueChanged(object sender, EventArgs e)
        {
            Link player = GetSaveSlot().GetPlayer();
            player.SetHasItemEquipment(bombCount, (byte) numericUpDownBombsHeld.Value); // Set the new bomb count value
        }

        private void pictureBombs_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxBombs);
        }

        private void pictureMushPowd_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxMushroomPowder);
        }

        private void mushPowdRadio(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                Link player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoMushPowd":
                        player.SetHasItemEquipment(mushroomPowder, 0x0); // Give Neither Mushroom nor Powder
                        pictureMushPowd.Image = ALTTPSRAMEditor.Properties.Resources.Mushroom;
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
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                Link player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoSword":
                        player.SetHasItemEquipment(sword, 0x0); // Give No Sword
                        pictureSword.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Sword;
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

        private void pictureSword_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxSword);
        }

        private void radioShield(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                Link player = GetSaveSlot().GetPlayer();
                switch (btn.Name)
                {
                    default:
                    case "radioButtonNoShield":
                        player.SetHasItemEquipment(shield, 0x0); // Give No Shield
                        pictureShield.Image = ALTTPSRAMEditor.Properties.Resources.Fighter_s_Shield;
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

        private void pictureShield_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxShield);
        }

        private void pictureMail_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxMails);
        }

        private void mailRadio(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                Link player = GetSaveSlot().GetPlayer();
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

        private void comboBoxBottle1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
            int fillContents = bottleContents[comboBoxBottle1.SelectedIndex];

            // Update the picture so it represents what the bottle actually has
            pictureBottle1.Image = bottleContentsImg[fillContents];
            Link player = GetSaveSlot().GetPlayer();
            player.SetHasItemEquipment(bottle1Contents, (byte) fillContents);
        }

        private void pictureBottles_Click(object sender, EventArgs e)
        {
            HideAllGroupBoxesExcept(groupBoxBottles);
        }

        private void comboBoxBottle2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
            int fillContents = bottleContents[comboBoxBottle2.SelectedIndex];

            // Update the picture so it represents what the bottle actually has
            pictureBottle2.Image = bottleContentsImg[fillContents];
            Link player = GetSaveSlot().GetPlayer();
            player.SetHasItemEquipment(bottle2Contents, (byte)fillContents);
        }

        private void comboBoxBottle3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
            int fillContents = bottleContents[comboBoxBottle3.SelectedIndex];

            // Update the picture so it represents what the bottle actually has
            pictureBottle3.Image = bottleContentsImg[fillContents];
            Link player = GetSaveSlot().GetPlayer();
            player.SetHasItemEquipment(bottle3Contents, (byte)fillContents);
        }

        private void comboBoxBottle4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Fill the bottle with the value the user selected in the UI; Remap to actual game values by referring to the bottleContents[] array
            int fillContents = bottleContents[comboBoxBottle4.SelectedIndex];

            // Update the picture so it represents what the bottle actually has
            pictureBottle4.Image = bottleContentsImg[fillContents];
            Link player = GetSaveSlot().GetPlayer();
            player.SetHasItemEquipment(bottle4Contents, (byte)fillContents);
        }

        private void pictureBoots_Click(object sender, EventArgs e)
        {
            Link player = GetSaveSlot().GetPlayer();
            byte flags = player.GetAbilityFlags();
            if (player.GetItemEquipment(pegasusBoots) == 1)
            {
                Console.WriteLine("Boots Off");
                Console.WriteLine(flags.ToString() + " -> " + (flags & 0xFD).ToString());
                flags &= 0xFB; // To turn it off, bitwise and with b11111101
                player.SetHasItemEquipment(pegasusBoots, 0x0);
                player.SetHasItemEquipment(abilityFlags, flags);
            }
            else
            {
                Console.WriteLine("Boots On");
                Console.WriteLine(flags.ToString() + " -> " + (flags | 0x2).ToString());
                flags |= 0x4; // Turn it on, bitwise or with b00000010
                player.SetHasItemEquipment(pegasusBoots, 0x1);
                player.SetHasItemEquipment(abilityFlags, flags);
            }

        }

        public static bool GetBit(byte b, int bitNumber)
        {
            return (b & (1 << bitNumber - 1)) != 0;
        }
    }
}