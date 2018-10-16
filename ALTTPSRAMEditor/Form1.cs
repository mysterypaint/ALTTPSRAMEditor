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

namespace ALTTPSRAMEditor {
    public partial class Form1 : Form {
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
        static bool jpnSave = false;
        static SRAM sdat;
        static String fname = "";

        // Initialize the font data
        Image en_fnt = ALTTPSRAMEditor.Properties.Resources.en_font;
        Image jpn_fnt = ALTTPSRAMEditor.Properties.Resources.jpn_font;
        Dictionary<char, int> enChar = new Dictionary<char, int>();
        Dictionary<char, int> jpChar = new Dictionary<char, int>();

        public Form1() {
            InitializeComponent();
        }

        private void opensrmToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenSRM();
        }

        private void OpenSRM() {
            OpenFileDialog fd1 = new OpenFileDialog();
            fd1.Filter = "SRAM|*.srm|All Files|*.*"; // Filter to show.srm files only.
            if (fd1.ShowDialog().Equals(DialogResult.OK)) { // Prompt the user to open a file, then check if a valid file was opened.
                fname = fd1.FileName;

                try { // Open the text file using a File Stream
                    byte [] bytes = File.ReadAllBytes(fname);
                    long fileSize = new System.IO.FileInfo(fname).Length;
                    if (fileSize == srm_randomizer_size || fileSize == srm_randomizer_size_2) MessageBox.Show("Invalid SRAM File. (Randomizer saves aren't supported. Maybe one day...?)");

                    else if (fileSize == srm_size) {
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

                    else {
                        MessageBox.Show("Invalid SRAM File.");
                    }
                }

                catch (Exception e) {
                    MessageBox.Show("The file could not be read:\n" + e.Message);
                }
            }
        }

        private void UpdateFilename(String str) {
            fileNameBox.Text = str;
        }

        private void SaveSRM() {
            if (fname.Equals("") || fname.Equals(null)) {
                helperText.Text = "Load a file first!";
                return; // Abort saving if there isn't a valid file open.
            }

            byte [] outputData = sdat.MergeSaveData();
            File.WriteAllBytes(fname, outputData);
            helperText.Text = "Saved file at " + fname;
            buttonWrite.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e) {
            // Terminate the program if we select "Exit" in the Menu Bar
            System.Windows.Forms.Application.Exit();
        }

        private void saveCTRLSToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveSRM();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // Define Font lookup tables
            // English
            enChar.Add(' ', 0);
            for (int i = 65; i <= 90; i++) enChar.Add((char)i, i - 64);
            for (int i = 97; i <= 122; i++) enChar.Add((char)i, i - 69);
            enChar.Add('-', 54);
            enChar.Add('.', 55);
            enChar.Add(',', 56);
            for (int i = 48; i <= 57; i++) enChar.Add((char)i, i + 11);
            enChar.Add('!', 69);
            enChar.Add('?', 70);
            enChar.Add('(', 71);
            enChar.Add(')', 72);

            // Japanese
            jpChar.Add('　', 1); // Space

            jpChar.Add('あ', 5); // a
            jpChar.Add('い', 6); // i
            jpChar.Add('う', 7); // u
            jpChar.Add('え', 8); // e
            jpChar.Add('お', 9); // o

            jpChar.Add('か', 10); // ka
            jpChar.Add('き', 11); // ki
            jpChar.Add('く', 12); // ku
            jpChar.Add('け', 13); // ke
            jpChar.Add('こ', 14); // ko

            jpChar.Add('さ', 15); // sa
            jpChar.Add('し', 16); // shi
            jpChar.Add('す', 17); // su
            jpChar.Add('せ', 18); // se
            jpChar.Add('そ', 19); // so

            jpChar.Add('た', 20); // ta
            jpChar.Add('ち', 21); // chi
            jpChar.Add('つ', 22); // tsu
            jpChar.Add('て', 23); // te
            jpChar.Add('と', 24); // to

            jpChar.Add('な', 25); // na
            jpChar.Add('に', 26); // ni
            jpChar.Add('ぬ', 27); // nu
            jpChar.Add('ね', 28); // ne
            jpChar.Add('の', 29); // no

            jpChar.Add('は', 30); // ha
            jpChar.Add('ひ', 31); // hi
            jpChar.Add('ふ', 32); // fu
            jpChar.Add('へ', 33); // he
            jpChar.Add('ほ', 34); // ho

            jpChar.Add('ま', 35); // ma
            jpChar.Add('み', 36); // mi
            jpChar.Add('む', 37); // mu
            jpChar.Add('め', 38); // me
            jpChar.Add('も', 39); // mo

            jpChar.Add('や', 40); // ya
            jpChar.Add('ゆ', 42); // yu
            jpChar.Add('よ', 44); // yo
            
            jpChar.Add('ら', 45); // ra
            jpChar.Add('り', 46); // ri
            jpChar.Add('る', 47); // ru
            jpChar.Add('れ', 48); // re
            jpChar.Add('ろ', 49); // ro

            jpChar.Add('わ', 50); // wa
            jpChar.Add('を', 51); // wo
            jpChar.Add('ん', 52); // n

            jpChar.Add('ー', 54);

            jpChar.Add('が', 55); // ga
            jpChar.Add('ぎ', 56); // gi
            jpChar.Add('ぐ', 57); // gu
            jpChar.Add('げ', 58); // ge
            jpChar.Add('ご', 59); // go
            
            jpChar.Add('ざ', 60); // za
            jpChar.Add('じ', 61); // ji
            jpChar.Add('ず', 62); // zu
            jpChar.Add('ぜ', 63); // ze
            jpChar.Add('ぞ', 64); // zo

            jpChar.Add('だ', 65); // da
            jpChar.Add('ぢ', 66); // di
            jpChar.Add('づ', 67); // du
            jpChar.Add('で', 68); // de
            jpChar.Add('ど', 69); // do

            jpChar.Add('ば', 70); // ba
            jpChar.Add('び', 71); // bi
            jpChar.Add('ぶ', 72); // bu
            jpChar.Add('べ', 73); // be
            jpChar.Add('ぼ', 74); // bo

            jpChar.Add('ぱ', 75); // pa
            jpChar.Add('ぴ', 76); // pi
            jpChar.Add('ぷ', 77); // pu
            jpChar.Add('ぺ', 78); // pe
            jpChar.Add('ぽ', 79); // po

            jpChar.Add('ぁ', 80); // ~a
            jpChar.Add('ぃ', 81); // ~i
            jpChar.Add('ぅ', 82); // ~u
            jpChar.Add('ぇ', 83); // ~e
            jpChar.Add('ぉ', 84); // ~o

            jpChar.Add('ゃ', 85); // ~ya
            jpChar.Add('ゅ', 86); // ~yu
            jpChar.Add('ょ', 87); // ~yo

            jpChar.Add('っ', 89); // ~tsu

            // Katakana
            jpChar.Add('ア', 95); // a
            jpChar.Add('イ', 96); // i
            jpChar.Add('ウ', 97); // u
            jpChar.Add('エ', 98); // e
            jpChar.Add('オ', 99); // o

            jpChar.Add('カ', 100); // ka
            jpChar.Add('キ', 101); // ki
            jpChar.Add('ク', 102); // ku
            jpChar.Add('ケ', 103); // ke
            jpChar.Add('コ', 104); // ko

            jpChar.Add('サ', 105); // sa
            jpChar.Add('シ', 106); // shi
            jpChar.Add('ス', 107); // su
            jpChar.Add('セ', 108); // se
            jpChar.Add('ソ', 109); // so

            jpChar.Add('タ', 110); // ta
            jpChar.Add('チ', 111); // chi
            jpChar.Add('ツ', 112); // tsu
            jpChar.Add('テ', 113); // te
            jpChar.Add('ト', 114); // to

            jpChar.Add('ナ', 115); // na
            jpChar.Add('ニ', 116); // ni
            jpChar.Add('ヌ', 117); // nu
            jpChar.Add('ネ', 118); // ne
            jpChar.Add('ノ', 119); // no

            jpChar.Add('ハ', 120); // ha
            jpChar.Add('ヒ', 121); // hi
            jpChar.Add('フ', 122); // fu
            jpChar.Add('ヘ', 123); // he
            jpChar.Add('ホ', 124); // ho

            jpChar.Add('マ', 125); // ma
            jpChar.Add('ミ', 126); // mi
            jpChar.Add('ム', 127); // mu
            jpChar.Add('メ', 128); // me
            jpChar.Add('モ', 129); // mo
            
            jpChar.Add('ヤ', 130); // ya
            jpChar.Add('ユ', 132); // yu
            jpChar.Add('ヨ', 134); // yo

            jpChar.Add('ラ', 135); // ra
            jpChar.Add('リ', 136); // ri
            jpChar.Add('ル', 137); // ru
            jpChar.Add('レ', 138); // re
            jpChar.Add('ロ', 139); // ro

            jpChar.Add('ワ', 140); // wa
            jpChar.Add('ヲ', 141); // wo
            jpChar.Add('ン', 142); // n

            jpChar.Add('－', 144);

            jpChar.Add('ガ', 145); // ga
            jpChar.Add('ギ', 146); // gi
            jpChar.Add('グ', 147); // gu
            jpChar.Add('ゲ', 148); // ge
            jpChar.Add('ゴ', 149); // go

            jpChar.Add('ザ', 150); // za
            jpChar.Add('ジ', 151); // ji
            jpChar.Add('ズ', 152); // zu
            jpChar.Add('ゼ', 153); // ze
            jpChar.Add('ゾ', 154); // zo

            jpChar.Add('ダ', 155); // da
            jpChar.Add('ヂ', 156); // di
            jpChar.Add('ヅ', 157); // du
            jpChar.Add('デ', 158); // de
            jpChar.Add('ド', 159); // do

            jpChar.Add('バ', 160); // ba
            jpChar.Add('ビ', 161); // bi
            jpChar.Add('ブ', 162); // bu
            jpChar.Add('ベ', 163); // be
            jpChar.Add('ボ', 164); // bo

            jpChar.Add('パ', 165); // pa
            jpChar.Add('ピ', 166); // pi
            jpChar.Add('プ', 167); // pu
            jpChar.Add('ペ', 168); // pe
            jpChar.Add('ポ', 169); // po

            jpChar.Add('ァ', 170); // ~a
            jpChar.Add('ィ', 171); // ~i
            jpChar.Add('ゥ', 172); // ~u
            jpChar.Add('ェ', 173); // ~e
            jpChar.Add('ォ', 174); // ~o

            jpChar.Add('ャ', 175); // ~ya
            jpChar.Add('ュ', 176); // ~yu
            jpChar.Add('ョ', 177); // ~yo

            jpChar.Add('ッ', 179); // ~tsu

            // Romaji
            jpChar.Add('A', 180);
            jpChar.Add('B', 181);
            jpChar.Add('C', 182);
            jpChar.Add('D', 183);
            jpChar.Add('E', 184);
            jpChar.Add('F', 185);
            jpChar.Add('G', 186);
            jpChar.Add('H', 187);
            jpChar.Add('I', 188);
            jpChar.Add('J', 189);
            jpChar.Add('K', 190);
            jpChar.Add('L', 191);
            jpChar.Add('M', 192);
            jpChar.Add('N', 193);
            jpChar.Add('O', 194);
            jpChar.Add('P', 195);
            jpChar.Add('Q', 196);
            jpChar.Add('R', 197);
            jpChar.Add('S', 198);
            jpChar.Add('T', 199);
            jpChar.Add('U', 200);
            jpChar.Add('V', 201);
            jpChar.Add('W', 202);
            jpChar.Add('X', 203);
            jpChar.Add('Y', 204);
            jpChar.Add('Z', 205);
            jpChar.Add('-', 206);
            jpChar.Add('~', 207);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control) // Handle CTRL shortcuts
            {
                switch(e.KeyCode.ToString()) {
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
                    default: break;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            String toolCredits = "ALTTP SRAM Editor\n- Created by mysterypaint 2018\n\nSpecial thanks to alttp.run for the reverse-engineering documentation. http://alttp.run/hacking/index.php?title=SRAM_Map";
            MessageBox.Show(toolCredits);
        }

        private void buttonCopy_Click(object sender, EventArgs e) {
            if (radioFile1.Checked) {
                sdat.CopyFile(1);
                helperText.Text = "Copied File 1!";
            }

            else if (radioFile2.Checked) {
                sdat.CopyFile(2);
                helperText.Text = "Copied File 2!";
            }

            else if (radioFile3.Checked) {
                sdat.CopyFile(3);
                helperText.Text = "Copied File 3!";
            }

            buttonWrite.Enabled = true;
        }

        private void buttonWrite_Click(object sender, EventArgs e) {
            if (radioFile1.Checked) {
                sdat.WriteFile(1);
                helperText.Text = "Wrote to File 1!";
            }

            else if (radioFile2.Checked) {
                sdat.WriteFile(2);
                helperText.Text = "Wrote to File 2!";
            }

            else if (radioFile3.Checked) {
                sdat.WriteFile(3);
                helperText.Text = "Wrote to File 3!";
            }
        }

        private void buttonErase_Click(object sender, EventArgs e) {
            int selFile = 1;
            if (radioFile1.Checked) selFile = 1;
            else if (radioFile2.Checked) selFile = 2;
            else if (radioFile3.Checked) selFile = 3;
            DialogResult dialogResult = MessageBox.Show("You are about to PERMANENTLY ERASE File " + selFile + "! Are you sure you want to erase it? There is no undo!", "Erase File " + selFile + "?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes) {
                sdat.EraseFile(selFile);
                helperText.Text = "Erased File " + selFile + ".";
            }
        }

        private void updatePlayerName(SaveSlot savslot) {
            if (!savslot.SaveIsValid()) {
                String playerName = savslot.GetPlayerName();
                fileNameBox.Text = playerName;
                fileNameBox.Enabled = true;
            }

            else {
                fileNameBox.Enabled = false;
            }
        }

        private void buttonApplyChanges_Click(object sender, EventArgs e) {
            buttonWrite.Enabled = false;
            // Determine which file we're editing, and then load its data.
            SaveSlot savslot;

            if (radioFile2.Checked) {
                savslot = sdat.GetSaveSlot(2);
            }

            else if (radioFile3.Checked) {
                savslot = sdat.GetSaveSlot(3);
            }

            else savslot = sdat.GetSaveSlot(1);
            // Cap the player name to 6 characters because of in-game limitations; Store it to the save data.
            savslot.SetPlayerName(fileNameBox.Text.Substring(0,6));
            // Update the playerName textbox to emphasize the 6 character limit.
            updatePlayerName(savslot);
        }

        private void pictureBow_Click(object sender, EventArgs e) {
            groupBoxBowConfig.Visible = true;
        }

        private SaveSlot GetSaveSlot() {
            SaveSlot savslot;

            if (radioFile2.Checked) {
                savslot = sdat.GetSaveSlot(2);
            }

            else if (radioFile3.Checked) {
                savslot = sdat.GetSaveSlot(3);
            }

            else savslot = sdat.GetSaveSlot(1);
            return savslot;
        }

        private void bowRadio(object sender, EventArgs e) {
            RadioButton btn = sender as RadioButton;

            if (btn != null && btn.Checked) {
                SaveSlot savslot = GetSaveSlot();
                Link player = savslot.GetPlayer();

                switch (btn.Name) {
                    case "bowOptionNone": player.SetHasItemEquipment(bow, 0x0); // Give No Bow
                    break;
                    case "bowOption1": player.SetHasItemEquipment(bow, 0x1); // Give Bow
                    break;
                    case "bowOption2": player.SetHasItemEquipment(bow, 0x2); // Give Bow & Arrows
                    break;
                    case "bowOption3": player.SetHasItemEquipment(bow, 0x3); // Give Silver Bow
                    break;
                    case "bowOption4": player.SetHasItemEquipment(bow, 0x4); // Give Bow & Silver Arrows
                    break;
                }
            }
        }

        private void numericUpDownRupeeCounter_ValueChanged(object sender, EventArgs e) {
            SaveSlot savslot = GetSaveSlot();
            Link player = savslot.GetPlayer();
            UInt16 val = (UInt16) numericUpDownRupeeCounter.Value;
            player.SetRupees(val);
        }

        private void fileRadio(object sender, EventArgs e) {
            RadioButton btn = sender as RadioButton;

            if (btn != null && btn.Checked) {
                SaveSlot savslot = GetSaveSlot();
                Link player = savslot.GetPlayer();
                updatePlayerName(savslot);
                numericUpDownRupeeCounter.Value = player.GetRupeeValue();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            DrawImageRect(e);
        }

        public void DrawImageRect(PaintEventArgs e) {
            jpnSave = false;
            if (jpnSave)
            {
                String playerName = "リンク";
                pos = 0;
                int i = 0;
                foreach (char c in playerName)
                {
                    char letter = c;
                    if (c == ' ')
                        letter = '　';
                    DrawTile(jpn_fnt, jpChar[letter], e, pos);
                    pos += 8;
                    i++;
                }
            }
            else
            {
                String playerName = "Link";
                pos = 0;
                foreach (char c in playerName)
                {
                    DrawTile(en_fnt, enChar[c], e, pos);
                    pos += 8;
                }
            }
        }

        public static void DrawTile(Image source, int tileID, PaintEventArgs e, int pos) {
            int tileset_width = 27; // English Font
            if (jpnSave)
                tileset_width = 20; // Japanese Font

            int tile_w = 8;
            int tile_h = 16;
            int x = (tileID % tileset_width) * tile_w;
            int y = (tileID / tileset_width) * tile_h;
            int width = 8;
            int height = 16;
            Rectangle crop = new Rectangle(x, y, width, height);
            var bmp = new Bitmap(crop.Width, crop.Height);

            using (var gr = Graphics.FromImage(bmp)) {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }

            e.Graphics.DrawImage(bmp, 223 + pos, 49);
        }
    }
}
