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

        // Items and Equipment
        const int bow = 0x0;
        const int boomerang = 0x1;
        const int hookshot = 0x2;
        const int bomb = 0x3;
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
        static int pos = 0;

        static SRAM sdat;
        static String fname = "";

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
            fd1.Filter = "SRAM|*.srm|All Files|*.*"; // Filter to show .srm files only.

            if (fd1.ShowDialog().Equals(DialogResult.OK))
            { // Prompt the user to open a file, then check if a valid file was opened.
                fname = fd1.FileName;

                try
                {   // Open the text file using a File Stream

                    byte[] bytes = File.ReadAllBytes(fname);
                    long fileSize = new System.IO.FileInfo(fname).Length;
                    if (fileSize == srm_randomizer_size || fileSize == srm_randomizer_size_2)
                        MessageBox.Show("Invalid SRAM File. (Randomizer saves aren't supported. Maybe one day...?)");
                    else if (fileSize == srm_size)
                    {
                        Console.WriteLine("Opened " + fname);
                        helperText.Text = "Opened " + fname;
                        sdat = new SRAM(bytes);

                        tableLayoutPanelInventory.Visible = true;
                        labelInventory.Visible = true;
                        numericUpDownRupeeCounter.Visible = true;
                        labelRupees.Visible = true;
                        fileNameBox.Visible = true;
                        labelFilename.Visible = true;

                        radioFile1.Enabled = true;
                        radioFile2.Enabled = true;
                        radioFile3.Enabled = true;
                        buttonCopy.Enabled = true;
                        buttonErase.Enabled = true;

                        SaveSlot savslot = sdat.GetSaveSlot(1);
                        updatePlayerName(savslot);
                        Link player = savslot.GetPlayer();
                        numericUpDownRupeeCounter.Value = player.GetRupeeValue();
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

        private void UpdateFilename(String str)
        {
            fileNameBox.Text = str;
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

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

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control) // Handle CTRL shortcuts
            {
                switch(e.KeyCode.ToString())
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
            if (radioFile1.Checked)
                selFile = 1;
            else if (radioFile2.Checked)
                selFile = 2;
            else if (radioFile3.Checked)
                selFile = 3;

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
                fileNameBox.Text = playerName;
                fileNameBox.Enabled = true;
            }
            else
            {
                fileNameBox.Enabled = false;
            }
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
            else
                savslot = sdat.GetSaveSlot(1);

            // Cap the player name to 6 characters because of in-game limitations; Store it to the save data.
            savslot.SetPlayerName(fileNameBox.Text.Substring(0,6));
            
            // Update the playerName textbox to emphasize the 6 character limit.
            updatePlayerName(savslot);
        }
        
        private void pictureBow_Click(object sender, EventArgs e)
        {
            groupBoxBowConfig.Visible = true;
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
            else
                savslot = sdat.GetSaveSlot(1);
            return savslot;
        }

        private void bowRadio(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                SaveSlot savslot = GetSaveSlot();
                Link player = savslot.GetPlayer();
                switch (btn.Name)
                {
                    case "bowOptionNone":
                        player.SetHasItemEquipment(bow, 0x0); // Give No Bow
                        break;
                    case "bowOption1":
                        player.SetHasItemEquipment(bow, 0x1); // Give Bow
                        break;
                    case "bowOption2":
                        player.SetHasItemEquipment(bow, 0x2); // Give Bow & Arrows
                        break;
                    case "bowOption3":
                        player.SetHasItemEquipment(bow, 0x3); // Give Silver Bow
                        break;
                    case "bowOption4":
                        player.SetHasItemEquipment(bow, 0x4); // Give Bow & Silver Arrows
                        break;
                }
            }
        }

        private void numericUpDownRupeeCounter_ValueChanged(object sender, EventArgs e)
        {
            SaveSlot savslot = GetSaveSlot();
            Link player = savslot.GetPlayer();
            UInt16 val = (UInt16) numericUpDownRupeeCounter.Value;
            player.SetRupees(val);
        }

        private void fileRadio(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.Checked)
            {
                SaveSlot savslot = GetSaveSlot();
                Link player = savslot.GetPlayer();
                updatePlayerName(savslot);
                numericUpDownRupeeCounter.Value = player.GetRupeeValue();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawImageRect(e);
        }

        public void DrawImageRect(PaintEventArgs e)
        {
            // Create image
            Image e_fnt = ALTTPSRAMEditor.Properties.Resources.en_font;

            Dictionary<char, int> enChar = new Dictionary<char, int>();
            enChar.Add(' ', 0);
            enChar.Add('A', 1);
            enChar.Add('B', 2);
            enChar.Add('C', 3);
            enChar.Add('D', 4);
            enChar.Add('E', 5);
            enChar.Add('F', 6);
            enChar.Add('G', 7);
            enChar.Add('H', 8);
            enChar.Add('I', 9);
            enChar.Add('J', 10);
            enChar.Add('K', 11);
            enChar.Add('L', 12);
            enChar.Add('M', 13);
            enChar.Add('N', 14);
            enChar.Add('O', 15);
            enChar.Add('P', 16);
            enChar.Add('Q', 17);
            enChar.Add('R', 18);
            enChar.Add('S', 19);
            enChar.Add('T', 20);
            enChar.Add('U', 21);
            enChar.Add('V', 22);
            enChar.Add('W', 23);
            enChar.Add('X', 24);
            enChar.Add('Y', 25);
            enChar.Add('Z', 26);
            enChar.Add('a', 28);
            enChar.Add('b', 29);
            enChar.Add('c', 30);
            enChar.Add('d', 31);
            enChar.Add('e', 32);
            enChar.Add('f', 33);
            enChar.Add('g', 34);
            enChar.Add('h', 35);
            enChar.Add('i', 36);
            enChar.Add('j', 37);
            enChar.Add('k', 38);
            enChar.Add('l', 39);
            enChar.Add('m', 40);
            enChar.Add('n', 41);
            enChar.Add('o', 42);
            enChar.Add('p', 43);
            enChar.Add('q', 44);
            enChar.Add('r', 45);
            enChar.Add('s', 46);
            enChar.Add('t', 47);
            enChar.Add('u', 48);
            enChar.Add('v', 49);
            enChar.Add('w', 50);
            enChar.Add('x', 51);
            enChar.Add('y', 52);
            enChar.Add('z', 53);

            String playerName = "CUtIePiE";

            foreach (char c in playerName)
            {
                DrawTile(e_fnt, enChar[c], e, pos);
                pos += 8;
            }
        }

        public static void DrawTile(Image source, int tileID, PaintEventArgs e, int pos)
        {
            int tileset_width = 27;
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
            e.Graphics.DrawImage(bmp, 300 + pos, 100);
        }
    }
}
