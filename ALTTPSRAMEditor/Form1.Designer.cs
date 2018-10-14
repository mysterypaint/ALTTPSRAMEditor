namespace ALTTPSRAMEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opensrmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCTRLSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupFileSelect = new System.Windows.Forms.GroupBox();
            this.buttonErase = new System.Windows.Forms.Button();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.radioFile3 = new System.Windows.Forms.RadioButton();
            this.radioFile2 = new System.Windows.Forms.RadioButton();
            this.radioFile1 = new System.Windows.Forms.RadioButton();
            this.helperText = new System.Windows.Forms.Label();
            this.fileNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxDungeonSelector = new System.Windows.Forms.ListBox();
            this.textBoxDeathsQuest = new System.Windows.Forms.TextBox();
            this.buttonApplyChanges = new System.Windows.Forms.Button();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelDeaths = new System.Windows.Forms.Label();
            this.textBoxDeathsPost = new System.Windows.Forms.TextBox();
            this.labelDeathsPostGame = new System.Windows.Forms.Label();
            this.checkedListBoxDungeonItems = new System.Windows.Forms.CheckedListBox();
            this.labelInventory = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelArrows = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButtonWArrows = new System.Windows.Forms.RadioButton();
            this.pictureBow = new System.Windows.Forms.PictureBox();
            this.radioButtonBowNone = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonRedBoomerang = new System.Windows.Forms.RadioButton();
            this.radioButtonBlueBoomerang = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioButtonNoBoomerang = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelBombs = new System.Windows.Forms.Label();
            this.labelDungeonKeys = new System.Windows.Forms.Label();
            this.textBoxDungeonKeys = new System.Windows.Forms.TextBox();
            this.labelRupees = new System.Windows.Forms.Label();
            this.numericUpDownRupeeCounter = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownArrows = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownBombs = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            this.groupFileSelect.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBow)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRupeeCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownArrows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBombs)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(949, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opensrmToolStripMenuItem,
            this.saveCTRLSToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // opensrmToolStripMenuItem
            // 
            this.opensrmToolStripMenuItem.Name = "opensrmToolStripMenuItem";
            this.opensrmToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.opensrmToolStripMenuItem.Text = "Open (Ctrl+O)";
            this.opensrmToolStripMenuItem.Click += new System.EventHandler(this.opensrmToolStripMenuItem_Click);
            // 
            // saveCTRLSToolStripMenuItem
            // 
            this.saveCTRLSToolStripMenuItem.Name = "saveCTRLSToolStripMenuItem";
            this.saveCTRLSToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.saveCTRLSToolStripMenuItem.Text = "Save (Ctrl+S)";
            this.saveCTRLSToolStripMenuItem.Click += new System.EventHandler(this.saveCTRLSToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exitToolStripMenuItem.Text = "Exit (Alt+F4)";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // groupFileSelect
            // 
            this.groupFileSelect.Controls.Add(this.buttonErase);
            this.groupFileSelect.Controls.Add(this.buttonWrite);
            this.groupFileSelect.Controls.Add(this.buttonCopy);
            this.groupFileSelect.Controls.Add(this.radioFile3);
            this.groupFileSelect.Controls.Add(this.radioFile2);
            this.groupFileSelect.Controls.Add(this.radioFile1);
            this.groupFileSelect.Location = new System.Drawing.Point(12, 27);
            this.groupFileSelect.Name = "groupFileSelect";
            this.groupFileSelect.Size = new System.Drawing.Size(200, 107);
            this.groupFileSelect.TabIndex = 4;
            this.groupFileSelect.TabStop = false;
            this.groupFileSelect.Text = "Current File";
            // 
            // buttonErase
            // 
            this.buttonErase.Enabled = false;
            this.buttonErase.Location = new System.Drawing.Point(95, 65);
            this.buttonErase.Name = "buttonErase";
            this.buttonErase.Size = new System.Drawing.Size(75, 23);
            this.buttonErase.TabIndex = 9;
            this.buttonErase.Text = "Erase";
            this.buttonErase.UseVisualStyleBackColor = true;
            this.buttonErase.Click += new System.EventHandler(this.buttonErase_Click);
            // 
            // buttonWrite
            // 
            this.buttonWrite.Enabled = false;
            this.buttonWrite.Location = new System.Drawing.Point(95, 42);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(75, 23);
            this.buttonWrite.TabIndex = 8;
            this.buttonWrite.Text = "Write";
            this.buttonWrite.UseVisualStyleBackColor = true;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Enabled = false;
            this.buttonCopy.Location = new System.Drawing.Point(95, 19);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 23);
            this.buttonCopy.TabIndex = 7;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // radioFile3
            // 
            this.radioFile3.AutoSize = true;
            this.radioFile3.Enabled = false;
            this.radioFile3.Location = new System.Drawing.Point(6, 65);
            this.radioFile3.Name = "radioFile3";
            this.radioFile3.Size = new System.Drawing.Size(50, 17);
            this.radioFile3.TabIndex = 6;
            this.radioFile3.TabStop = true;
            this.radioFile3.Text = "File 3";
            this.radioFile3.UseVisualStyleBackColor = true;
            this.radioFile3.CheckedChanged += new System.EventHandler(this.radioFile3_CheckedChanged);
            // 
            // radioFile2
            // 
            this.radioFile2.AutoSize = true;
            this.radioFile2.Enabled = false;
            this.radioFile2.Location = new System.Drawing.Point(6, 42);
            this.radioFile2.Name = "radioFile2";
            this.radioFile2.Size = new System.Drawing.Size(50, 17);
            this.radioFile2.TabIndex = 5;
            this.radioFile2.TabStop = true;
            this.radioFile2.Text = "File 2";
            this.radioFile2.UseVisualStyleBackColor = true;
            this.radioFile2.CheckedChanged += new System.EventHandler(this.radioFile2_CheckedChanged);
            // 
            // radioFile1
            // 
            this.radioFile1.AutoSize = true;
            this.radioFile1.Checked = true;
            this.radioFile1.Enabled = false;
            this.radioFile1.Location = new System.Drawing.Point(6, 19);
            this.radioFile1.Name = "radioFile1";
            this.radioFile1.Size = new System.Drawing.Size(50, 17);
            this.radioFile1.TabIndex = 4;
            this.radioFile1.TabStop = true;
            this.radioFile1.Text = "File 1";
            this.radioFile1.UseVisualStyleBackColor = true;
            this.radioFile1.CheckedChanged += new System.EventHandler(this.radioFile1_CheckedChanged);
            // 
            // helperText
            // 
            this.helperText.AutoSize = true;
            this.helperText.Location = new System.Drawing.Point(9, 428);
            this.helperText.Name = "helperText";
            this.helperText.Size = new System.Drawing.Size(254, 13);
            this.helperText.TabIndex = 5;
            this.helperText.Text = "A Link to the Past SRAM Editor v1.0 by mysterypaint";
            // 
            // fileNameBox
            // 
            this.fileNameBox.Enabled = false;
            this.fileNameBox.Location = new System.Drawing.Point(218, 49);
            this.fileNameBox.Name = "fileNameBox";
            this.fileNameBox.Size = new System.Drawing.Size(100, 20);
            this.fileNameBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Filename";
            // 
            // listBoxDungeonSelector
            // 
            this.listBoxDungeonSelector.FormattingEnabled = true;
            this.listBoxDungeonSelector.Items.AddRange(new object[] {
            "Sewers",
            "Hyrule Castle",
            "Eastern Palace",
            "Desert Palace",
            "Hyrule Castle 2",
            "Swamp Palace",
            "Dark Palace",
            "Misery Mire",
            "Skull Woods",
            "Ice Palace",
            "Tower of Hera",
            "Thieves Town (Gargoyle\'s Domain)",
            "Turtle Rock",
            "Ganon\'s Tower"});
            this.listBoxDungeonSelector.Location = new System.Drawing.Point(12, 153);
            this.listBoxDungeonSelector.Name = "listBoxDungeonSelector";
            this.listBoxDungeonSelector.Size = new System.Drawing.Size(122, 134);
            this.listBoxDungeonSelector.TabIndex = 8;
            // 
            // textBoxDeathsQuest
            // 
            this.textBoxDeathsQuest.Location = new System.Drawing.Point(12, 293);
            this.textBoxDeathsQuest.Name = "textBoxDeathsQuest";
            this.textBoxDeathsQuest.Size = new System.Drawing.Size(100, 20);
            this.textBoxDeathsQuest.TabIndex = 9;
            // 
            // buttonApplyChanges
            // 
            this.buttonApplyChanges.Location = new System.Drawing.Point(12, 352);
            this.buttonApplyChanges.Name = "buttonApplyChanges";
            this.buttonApplyChanges.Size = new System.Drawing.Size(100, 23);
            this.buttonApplyChanges.TabIndex = 10;
            this.buttonApplyChanges.Text = "Apply Changes";
            this.buttonApplyChanges.UseVisualStyleBackColor = true;
            this.buttonApplyChanges.Click += new System.EventHandler(this.buttonApplyChanges_Click);
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Location = new System.Drawing.Point(9, 137);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(29, 13);
            this.labelArea.TabIndex = 11;
            this.labelArea.Text = "Area";
            // 
            // labelDeaths
            // 
            this.labelDeaths.AutoSize = true;
            this.labelDeaths.Location = new System.Drawing.Point(118, 296);
            this.labelDeaths.Name = "labelDeaths";
            this.labelDeaths.Size = new System.Drawing.Size(151, 13);
            this.labelDeaths.TabIndex = 12;
            this.labelDeaths.Text = "Deaths / Saves (During quest)";
            // 
            // textBoxDeathsPost
            // 
            this.textBoxDeathsPost.Location = new System.Drawing.Point(12, 326);
            this.textBoxDeathsPost.Name = "textBoxDeathsPost";
            this.textBoxDeathsPost.Size = new System.Drawing.Size(100, 20);
            this.textBoxDeathsPost.TabIndex = 13;
            // 
            // labelDeathsPostGame
            // 
            this.labelDeathsPostGame.AutoSize = true;
            this.labelDeathsPostGame.Location = new System.Drawing.Point(118, 329);
            this.labelDeathsPostGame.Name = "labelDeathsPostGame";
            this.labelDeathsPostGame.Size = new System.Drawing.Size(140, 13);
            this.labelDeathsPostGame.TabIndex = 14;
            this.labelDeathsPostGame.Text = "Deaths / Saves (PostGame)";
            // 
            // checkedListBoxDungeonItems
            // 
            this.checkedListBoxDungeonItems.FormattingEnabled = true;
            this.checkedListBoxDungeonItems.Items.AddRange(new object[] {
            "Compass",
            "Big Key",
            "Dungeon Map"});
            this.checkedListBoxDungeonItems.Location = new System.Drawing.Point(138, 153);
            this.checkedListBoxDungeonItems.Name = "checkedListBoxDungeonItems";
            this.checkedListBoxDungeonItems.Size = new System.Drawing.Size(120, 49);
            this.checkedListBoxDungeonItems.TabIndex = 15;
            // 
            // labelInventory
            // 
            this.labelInventory.AutoSize = true;
            this.labelInventory.Location = new System.Drawing.Point(328, 33);
            this.labelInventory.Name = "labelInventory";
            this.labelInventory.Size = new System.Drawing.Size(51, 13);
            this.labelInventory.TabIndex = 17;
            this.labelInventory.Text = "Inventory";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.2F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 202F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(331, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.0241F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.97591F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(606, 373);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownArrows);
            this.groupBox1.Controls.Add(this.labelArrows);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButtonWArrows);
            this.groupBox1.Controls.Add(this.pictureBow);
            this.groupBox1.Controls.Add(this.radioButtonBowNone);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(137, 97);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bow";
            // 
            // labelArrows
            // 
            this.labelArrows.AutoSize = true;
            this.labelArrows.Location = new System.Drawing.Point(83, 74);
            this.labelArrows.Name = "labelArrows";
            this.labelArrows.Size = new System.Drawing.Size(35, 13);
            this.labelArrows.TabIndex = 26;
            this.labelArrows.Text = "Count";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(33, 52);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(104, 17);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Bow w/S.Arrows";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButtonWArrows
            // 
            this.radioButtonWArrows.AutoSize = true;
            this.radioButtonWArrows.Location = new System.Drawing.Point(33, 36);
            this.radioButtonWArrows.Name = "radioButtonWArrows";
            this.radioButtonWArrows.Size = new System.Drawing.Size(94, 17);
            this.radioButtonWArrows.TabIndex = 3;
            this.radioButtonWArrows.TabStop = true;
            this.radioButtonWArrows.Text = "Bow w/Arrows";
            this.radioButtonWArrows.UseVisualStyleBackColor = true;
            // 
            // pictureBow
            // 
            this.pictureBow.Image = global::ALTTPSRAMEditor.Properties.Resources.Bow_and_Arrow;
            this.pictureBow.ImageLocation = "";
            this.pictureBow.InitialImage = global::ALTTPSRAMEditor.Properties.Resources.Bow_and_Arrow;
            this.pictureBow.Location = new System.Drawing.Point(6, 19);
            this.pictureBow.Name = "pictureBow";
            this.pictureBow.Size = new System.Drawing.Size(21, 17);
            this.pictureBow.TabIndex = 0;
            this.pictureBow.TabStop = false;
            // 
            // radioButtonBowNone
            // 
            this.radioButtonBowNone.AutoSize = true;
            this.radioButtonBowNone.Location = new System.Drawing.Point(33, 19);
            this.radioButtonBowNone.Name = "radioButtonBowNone";
            this.radioButtonBowNone.Size = new System.Drawing.Size(51, 17);
            this.radioButtonBowNone.TabIndex = 2;
            this.radioButtonBowNone.TabStop = true;
            this.radioButtonBowNone.Text = "None";
            this.radioButtonBowNone.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonRedBoomerang);
            this.groupBox2.Controls.Add(this.radioButtonBlueBoomerang);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.radioButtonNoBoomerang);
            this.groupBox2.Location = new System.Drawing.Point(146, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(100, 82);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Boomerang";
            // 
            // radioButtonRedBoomerang
            // 
            this.radioButtonRedBoomerang.AutoSize = true;
            this.radioButtonRedBoomerang.Location = new System.Drawing.Point(33, 52);
            this.radioButtonRedBoomerang.Name = "radioButtonRedBoomerang";
            this.radioButtonRedBoomerang.Size = new System.Drawing.Size(45, 17);
            this.radioButtonRedBoomerang.TabIndex = 4;
            this.radioButtonRedBoomerang.TabStop = true;
            this.radioButtonRedBoomerang.Text = "Red";
            this.radioButtonRedBoomerang.UseVisualStyleBackColor = true;
            // 
            // radioButtonBlueBoomerang
            // 
            this.radioButtonBlueBoomerang.AutoSize = true;
            this.radioButtonBlueBoomerang.Location = new System.Drawing.Point(33, 36);
            this.radioButtonBlueBoomerang.Name = "radioButtonBlueBoomerang";
            this.radioButtonBlueBoomerang.Size = new System.Drawing.Size(46, 17);
            this.radioButtonBlueBoomerang.TabIndex = 3;
            this.radioButtonBlueBoomerang.TabStop = true;
            this.radioButtonBlueBoomerang.Text = "Blue";
            this.radioButtonBlueBoomerang.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ALTTPSRAMEditor.Properties.Resources.Boomerang;
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.InitialImage = global::ALTTPSRAMEditor.Properties.Resources.Boomerang;
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 17);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // radioButtonNoBoomerang
            // 
            this.radioButtonNoBoomerang.AutoSize = true;
            this.radioButtonNoBoomerang.Location = new System.Drawing.Point(33, 19);
            this.radioButtonNoBoomerang.Name = "radioButtonNoBoomerang";
            this.radioButtonNoBoomerang.Size = new System.Drawing.Size(51, 17);
            this.radioButtonNoBoomerang.TabIndex = 2;
            this.radioButtonNoBoomerang.TabStop = true;
            this.radioButtonNoBoomerang.Text = "None";
            this.radioButtonNoBoomerang.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDownBombs);
            this.groupBox3.Controls.Add(this.pictureBox2);
            this.groupBox3.Controls.Add(this.labelBombs);
            this.groupBox3.Location = new System.Drawing.Point(354, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(196, 97);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bombs";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ALTTPSRAMEditor.Properties.Resources.Bomb;
            this.pictureBox2.ImageLocation = "";
            this.pictureBox2.InitialImage = global::ALTTPSRAMEditor.Properties.Resources.Bomb;
            this.pictureBox2.Location = new System.Drawing.Point(6, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(21, 17);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // labelBombs
            // 
            this.labelBombs.AutoSize = true;
            this.labelBombs.Location = new System.Drawing.Point(63, 42);
            this.labelBombs.Name = "labelBombs";
            this.labelBombs.Size = new System.Drawing.Size(35, 13);
            this.labelBombs.TabIndex = 24;
            this.labelBombs.Text = "Count";
            // 
            // labelDungeonKeys
            // 
            this.labelDungeonKeys.AutoSize = true;
            this.labelDungeonKeys.Location = new System.Drawing.Point(188, 211);
            this.labelDungeonKeys.Name = "labelDungeonKeys";
            this.labelDungeonKeys.Size = new System.Drawing.Size(30, 13);
            this.labelDungeonKeys.TabIndex = 20;
            this.labelDungeonKeys.Text = "Keys";
            // 
            // textBoxDungeonKeys
            // 
            this.textBoxDungeonKeys.Location = new System.Drawing.Point(138, 208);
            this.textBoxDungeonKeys.Name = "textBoxDungeonKeys";
            this.textBoxDungeonKeys.Size = new System.Drawing.Size(44, 20);
            this.textBoxDungeonKeys.TabIndex = 19;
            // 
            // labelRupees
            // 
            this.labelRupees.AutoSize = true;
            this.labelRupees.Location = new System.Drawing.Point(274, 80);
            this.labelRupees.Name = "labelRupees";
            this.labelRupees.Size = new System.Drawing.Size(44, 13);
            this.labelRupees.TabIndex = 22;
            this.labelRupees.Text = "Rupees";
            // 
            // numericUpDownRupeeCounter
            // 
            this.numericUpDownRupeeCounter.Location = new System.Drawing.Point(218, 78);
            this.numericUpDownRupeeCounter.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDownRupeeCounter.Name = "numericUpDownRupeeCounter";
            this.numericUpDownRupeeCounter.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownRupeeCounter.TabIndex = 23;
            // 
            // numericUpDownArrows
            // 
            this.numericUpDownArrows.Location = new System.Drawing.Point(33, 71);
            this.numericUpDownArrows.Maximum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.numericUpDownArrows.Name = "numericUpDownArrows";
            this.numericUpDownArrows.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownArrows.TabIndex = 24;
            // 
            // numericUpDownBombs
            // 
            this.numericUpDownBombs.Location = new System.Drawing.Point(6, 40);
            this.numericUpDownBombs.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownBombs.Name = "numericUpDownBombs";
            this.numericUpDownBombs.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownBombs.TabIndex = 27;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(949, 465);
            this.Controls.Add(this.numericUpDownRupeeCounter);
            this.Controls.Add(this.labelRupees);
            this.Controls.Add(this.labelDungeonKeys);
            this.Controls.Add(this.textBoxDungeonKeys);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelInventory);
            this.Controls.Add(this.checkedListBoxDungeonItems);
            this.Controls.Add(this.labelDeathsPostGame);
            this.Controls.Add(this.textBoxDeathsPost);
            this.Controls.Add(this.labelDeaths);
            this.Controls.Add(this.labelArea);
            this.Controls.Add(this.buttonApplyChanges);
            this.Controls.Add(this.textBoxDeathsQuest);
            this.Controls.Add(this.listBoxDungeonSelector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileNameBox);
            this.Controls.Add(this.helperText);
            this.Controls.Add(this.groupFileSelect);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "A Link to the Past SRAM Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupFileSelect.ResumeLayout(false);
            this.groupFileSelect.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBow)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRupeeCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownArrows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBombs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opensrmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCTRLSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupFileSelect;
        private System.Windows.Forms.RadioButton radioFile3;
        private System.Windows.Forms.RadioButton radioFile2;
        private System.Windows.Forms.RadioButton radioFile1;
        private System.Windows.Forms.Button buttonErase;
        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Label helperText;
        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxDungeonSelector;
        private System.Windows.Forms.TextBox textBoxDeathsQuest;
        private System.Windows.Forms.Button buttonApplyChanges;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelDeaths;
        private System.Windows.Forms.TextBox textBoxDeathsPost;
        private System.Windows.Forms.Label labelDeathsPostGame;
        private System.Windows.Forms.CheckedListBox checkedListBoxDungeonItems;
        private System.Windows.Forms.Label labelInventory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBow;
        private System.Windows.Forms.RadioButton radioButtonBowNone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButtonWArrows;
        private System.Windows.Forms.Label labelDungeonKeys;
        private System.Windows.Forms.TextBox textBoxDungeonKeys;
        private System.Windows.Forms.Label labelRupees;
        private System.Windows.Forms.Label labelBombs;
        private System.Windows.Forms.Label labelArrows;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonRedBoomerang;
        private System.Windows.Forms.RadioButton radioButtonBlueBoomerang;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radioButtonNoBoomerang;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownRupeeCounter;
        private System.Windows.Forms.NumericUpDown numericUpDownArrows;
        private System.Windows.Forms.NumericUpDown numericUpDownBombs;
    }
}

