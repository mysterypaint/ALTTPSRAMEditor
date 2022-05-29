namespace ALTTPSRAMEditor;

public partial class NameChangingFormJP : Form
{
    private Bitmap jp_fnt;
    private readonly StringBuilder currName;
    private readonly ushort[] currNameRaw;
    private readonly Dictionary<char, int> jpChar;
    private readonly Dictionary<ushort, char> rawJPChar;
    private int charPos = 0;
    private bool autoClose;
    private readonly MainForm mainForm;

    public NameChangingFormJP(MainForm _mainForm)
    {
        InitializeComponent();
        mainForm = _mainForm;
        jpChar = MainForm.GetJPChar();
        rawJPChar = MainForm.GetRawJPChar();
        autoClose = false;
        currName = new StringBuilder(mainForm.GetPlayerName()[..4]);
        currNameRaw = new ushort[6];
    }

    private void NameChangingFormJP_KeyDown(object sender, KeyEventArgs e)
    {
        // Close the Name Changing form if we hit Escape
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    private void NameChangingFormJP_Load(object sender, EventArgs e)
    {

        // Name Changing Form Initialization
        jp_fnt = new Bitmap(Properties.Resources.jpn_font);

        // Draw the name to the screen
        UpdateDisplayName();

        // Hiragana

        kbdHiraganaCharA.Image = GetCharTexture(jp_fnt, 5, false);
        kbdHiraganaCharI.Image = GetCharTexture(jp_fnt, 6, false);
        kbdHiraganaCharU.Image = GetCharTexture(jp_fnt, 7, false);
        kbdHiraganaCharE.Image = GetCharTexture(jp_fnt, 8, false);
        kbdHiraganaCharO.Image = GetCharTexture(jp_fnt, 9, false);

        kbdHiraganaCharKa.Image = GetCharTexture(jp_fnt, 10, false);
        kbdHiraganaCharKi.Image = GetCharTexture(jp_fnt, 11, false);
        kbdHiraganaCharKu.Image = GetCharTexture(jp_fnt, 12, false);
        kbdHiraganaCharKe.Image = GetCharTexture(jp_fnt, 13, false);
        kbdHiraganaCharKo.Image = GetCharTexture(jp_fnt, 14, false);

        kbdHiraganaCharSa.Image = GetCharTexture(jp_fnt, 15, false);
        kbdHiraganaCharShi.Image = GetCharTexture(jp_fnt, 16, false);
        kbdHiraganaCharSu.Image = GetCharTexture(jp_fnt, 17, false);
        kbdHiraganaCharSe.Image = GetCharTexture(jp_fnt, 18, false);
        kbdHiraganaCharSo.Image = GetCharTexture(jp_fnt, 19, false);

        kbdHiraganaCharTa.Image = GetCharTexture(jp_fnt, 20, false);
        kbdHiraganaCharChi.Image = GetCharTexture(jp_fnt, 21, false);
        kbdHiraganaCharTsu.Image = GetCharTexture(jp_fnt, 22, false);
        kbdHiraganaCharTe.Image = GetCharTexture(jp_fnt, 23, false);
        kbdHiraganaCharTo.Image = GetCharTexture(jp_fnt, 24, false);

        kbdHiraganaCharNa.Image = GetCharTexture(jp_fnt, 25, false);
        kbdHiraganaCharNi.Image = GetCharTexture(jp_fnt, 26, false);
        kbdHiraganaCharNu.Image = GetCharTexture(jp_fnt, 27, false);
        kbdHiraganaCharNe.Image = GetCharTexture(jp_fnt, 28, false);
        kbdHiraganaCharNo.Image = GetCharTexture(jp_fnt, 29, false);

        kbdHiraganaCharHa.Image = GetCharTexture(jp_fnt, 30, false);
        kbdHiraganaCharHi.Image = GetCharTexture(jp_fnt, 31, false);
        kbdHiraganaCharFu.Image = GetCharTexture(jp_fnt, 32, false);
        kbdHiraganaCharHe.Image = GetCharTexture(jp_fnt, 33, false);
        kbdHiraganaCharHo.Image = GetCharTexture(jp_fnt, 34, false);

        kbdHiraganaCharMa.Image = GetCharTexture(jp_fnt, 35, false);
        kbdHiraganaCharMi.Image = GetCharTexture(jp_fnt, 36, false);
        kbdHiraganaCharMu.Image = GetCharTexture(jp_fnt, 37, false);
        kbdHiraganaCharMe.Image = GetCharTexture(jp_fnt, 38, false);
        kbdHiraganaCharMo.Image = GetCharTexture(jp_fnt, 39, false);

        kbdHiraganaCharYa.Image = GetCharTexture(jp_fnt, 40, false);
        kbdHiraganaCharYu.Image = GetCharTexture(jp_fnt, 42, false);
        kbdHiraganaCharYo.Image = GetCharTexture(jp_fnt, 44, false);

        kbdHiraganaCharRa.Image = GetCharTexture(jp_fnt, 45, false);
        kbdHiraganaCharRi.Image = GetCharTexture(jp_fnt, 46, false);
        kbdHiraganaCharRu.Image = GetCharTexture(jp_fnt, 47, false);
        kbdHiraganaCharRe.Image = GetCharTexture(jp_fnt, 48, false);
        kbdHiraganaCharRo.Image = GetCharTexture(jp_fnt, 49, false);

        kbdHiraganaCharWa.Image = GetCharTexture(jp_fnt, 50, false);
        kbdHiraganaCharWo.Image = GetCharTexture(jp_fnt, 51, false);
        kbdHiraganaCharN.Image = GetCharTexture(jp_fnt, 52, false);
        kbdHiraganaCharHyphen.Image = GetCharTexture(jp_fnt, 54, false);

        kbdHiraganaCharGa.Image = GetCharTexture(jp_fnt, 55, false);
        kbdHiraganaCharGi.Image = GetCharTexture(jp_fnt, 56, false);
        kbdHiraganaCharGu.Image = GetCharTexture(jp_fnt, 57, false);
        kbdHiraganaCharGe.Image = GetCharTexture(jp_fnt, 58, false);
        kbdHiraganaCharGo.Image = GetCharTexture(jp_fnt, 59, false);


        kbdHiraganaCharZa.Image = GetCharTexture(jp_fnt, 60, false);
        kbdHiraganaCharJi.Image = GetCharTexture(jp_fnt, 61, false);
        kbdHiraganaCharZu.Image = GetCharTexture(jp_fnt, 62, false);
        kbdHiraganaCharZe.Image = GetCharTexture(jp_fnt, 63, false);
        kbdHiraganaCharZo.Image = GetCharTexture(jp_fnt, 64, false);

        kbdHiraganaCharDa.Image = GetCharTexture(jp_fnt, 65, false);
        kbdHiraganaCharDi.Image = GetCharTexture(jp_fnt, 66, false);
        kbdHiraganaCharDu.Image = GetCharTexture(jp_fnt, 67, false);
        kbdHiraganaCharDe.Image = GetCharTexture(jp_fnt, 68, false);
        kbdHiraganaCharDo.Image = GetCharTexture(jp_fnt, 69, false);

        kbdHiraganaCharBa.Image = GetCharTexture(jp_fnt, 70, false);
        kbdHiraganaCharBi.Image = GetCharTexture(jp_fnt, 71, false);
        kbdHiraganaCharBu.Image = GetCharTexture(jp_fnt, 72, false);
        kbdHiraganaCharBe.Image = GetCharTexture(jp_fnt, 73, false);
        kbdHiraganaCharBo.Image = GetCharTexture(jp_fnt, 74, false);

        kbdHiraganaCharPa.Image = GetCharTexture(jp_fnt, 75, false);
        kbdHiraganaCharPi.Image = GetCharTexture(jp_fnt, 76, false);
        kbdHiraganaCharPu.Image = GetCharTexture(jp_fnt, 77, false);
        kbdHiraganaCharPe.Image = GetCharTexture(jp_fnt, 78, false);
        kbdHiraganaCharPo.Image = GetCharTexture(jp_fnt, 79, false);

        kbdHiraganaCharSmallA.Image = GetCharTexture(jp_fnt, 80, false);
        kbdHiraganaCharSmallI.Image = GetCharTexture(jp_fnt, 81, false);
        kbdHiraganaCharSmallU.Image = GetCharTexture(jp_fnt, 82, false);
        kbdHiraganaCharSmallE.Image = GetCharTexture(jp_fnt, 83, false);
        kbdHiraganaCharSmallO.Image = GetCharTexture(jp_fnt, 84, false);

        kbdHiraganaCharSmallYa.Image = GetCharTexture(jp_fnt, 85, false);
        kbdHiraganaCharSmallYu.Image = GetCharTexture(jp_fnt, 86, false);
        kbdHiraganaCharSmallYo.Image = GetCharTexture(jp_fnt, 87, false);
        kbdHiraganaCharSmallTsu.Image = GetCharTexture(jp_fnt, 89, false);

        // Katakana

        kbdKatakanaCharA.Image = GetCharTexture(jp_fnt, 95, false);
        kbdKatakanaCharI.Image = GetCharTexture(jp_fnt, 96, false);
        kbdKatakanaCharU.Image = GetCharTexture(jp_fnt, 97, false);
        kbdKatakanaCharE.Image = GetCharTexture(jp_fnt, 98, false);
        kbdKatakanaCharO.Image = GetCharTexture(jp_fnt, 99, false);

        kbdKatakanaCharKa.Image = GetCharTexture(jp_fnt, 100, false);
        kbdKatakanaCharKi.Image = GetCharTexture(jp_fnt, 101, false);
        kbdKatakanaCharKu.Image = GetCharTexture(jp_fnt, 102, false);
        kbdKatakanaCharKe.Image = GetCharTexture(jp_fnt, 103, false);
        kbdKatakanaCharKo.Image = GetCharTexture(jp_fnt, 104, false);

        kbdKatakanaCharSa.Image = GetCharTexture(jp_fnt, 105, false);
        kbdKatakanaCharShi.Image = GetCharTexture(jp_fnt, 106, false);
        kbdKatakanaCharSu.Image = GetCharTexture(jp_fnt, 107, false);
        kbdKatakanaCharSe.Image = GetCharTexture(jp_fnt, 108, false);
        kbdKatakanaCharSo.Image = GetCharTexture(jp_fnt, 109, false);

        kbdKatakanaCharTa.Image = GetCharTexture(jp_fnt, 110, false);
        kbdKatakanaCharChi.Image = GetCharTexture(jp_fnt, 111, false);
        kbdKatakanaCharTsu.Image = GetCharTexture(jp_fnt, 112, false);
        kbdKatakanaCharTe.Image = GetCharTexture(jp_fnt, 113, false);
        kbdKatakanaCharTo.Image = GetCharTexture(jp_fnt, 114, false);

        kbdKatakanaCharNa.Image = GetCharTexture(jp_fnt, 115, false);
        kbdKatakanaCharNi.Image = GetCharTexture(jp_fnt, 116, false);
        kbdKatakanaCharNu.Image = GetCharTexture(jp_fnt, 117, false);
        kbdKatakanaCharNe.Image = GetCharTexture(jp_fnt, 118, false);
        kbdKatakanaCharNo.Image = GetCharTexture(jp_fnt, 119, false);

        kbdKatakanaCharHa.Image = GetCharTexture(jp_fnt, 120, false);
        kbdKatakanaCharHi.Image = GetCharTexture(jp_fnt, 121, false);
        kbdKatakanaCharFu.Image = GetCharTexture(jp_fnt, 122, false);
        kbdKatakanaCharHe.Image = GetCharTexture(jp_fnt, 123, false);
        kbdKatakanaCharHo.Image = GetCharTexture(jp_fnt, 124, false);

        kbdKatakanaCharMa.Image = GetCharTexture(jp_fnt, 125, false);
        kbdKatakanaCharMi.Image = GetCharTexture(jp_fnt, 126, false);
        kbdKatakanaCharMu.Image = GetCharTexture(jp_fnt, 127, false);
        kbdKatakanaCharMe.Image = GetCharTexture(jp_fnt, 128, false);
        kbdKatakanaCharMo.Image = GetCharTexture(jp_fnt, 129, false);

        kbdKatakanaCharYa.Image = GetCharTexture(jp_fnt, 130, false);
        kbdKatakanaCharYu.Image = GetCharTexture(jp_fnt, 132, false);
        kbdKatakanaCharYo.Image = GetCharTexture(jp_fnt, 134, false);

        kbdKatakanaCharRa.Image = GetCharTexture(jp_fnt, 135, false);
        kbdKatakanaCharRi.Image = GetCharTexture(jp_fnt, 136, false);
        kbdKatakanaCharRu.Image = GetCharTexture(jp_fnt, 137, false);
        kbdKatakanaCharRe.Image = GetCharTexture(jp_fnt, 138, false);
        kbdKatakanaCharRo.Image = GetCharTexture(jp_fnt, 139, false);

        kbdKatakanaCharWa.Image = GetCharTexture(jp_fnt, 140, false);
        kbdKatakanaCharWo.Image = GetCharTexture(jp_fnt, 141, false);
        kbdKatakanaCharN.Image = GetCharTexture(jp_fnt, 142, false);
        kbdKatakanaCharHyphen.Image = GetCharTexture(jp_fnt, 144, false);

        kbdKatakanaCharGa.Image = GetCharTexture(jp_fnt, 145, false);
        kbdKatakanaCharGi.Image = GetCharTexture(jp_fnt, 146, false);
        kbdKatakanaCharGu.Image = GetCharTexture(jp_fnt, 147, false);
        kbdKatakanaCharGe.Image = GetCharTexture(jp_fnt, 148, false);
        kbdKatakanaCharGo.Image = GetCharTexture(jp_fnt, 149, false);

        kbdKatakanaCharZa.Image = GetCharTexture(jp_fnt, 150, false);
        kbdKatakanaCharJi.Image = GetCharTexture(jp_fnt, 151, false);
        kbdKatakanaCharZu.Image = GetCharTexture(jp_fnt, 152, false);
        kbdKatakanaCharZe.Image = GetCharTexture(jp_fnt, 153, false);
        kbdKatakanaCharZo.Image = GetCharTexture(jp_fnt, 154, false);

        kbdKatakanaCharDa.Image = GetCharTexture(jp_fnt, 155, false);
        kbdKatakanaCharDi.Image = GetCharTexture(jp_fnt, 156, false);
        kbdKatakanaCharDu.Image = GetCharTexture(jp_fnt, 157, false);
        kbdKatakanaCharDe.Image = GetCharTexture(jp_fnt, 158, false);
        kbdKatakanaCharDo.Image = GetCharTexture(jp_fnt, 159, false);

        kbdKatakanaCharBa.Image = GetCharTexture(jp_fnt, 160, false);
        kbdKatakanaCharBi.Image = GetCharTexture(jp_fnt, 161, false);
        kbdKatakanaCharBu.Image = GetCharTexture(jp_fnt, 162, false);
        kbdKatakanaCharBe.Image = GetCharTexture(jp_fnt, 163, false);
        kbdKatakanaCharBo.Image = GetCharTexture(jp_fnt, 164, false);

        kbdKatakanaCharPa.Image = GetCharTexture(jp_fnt, 165, false);
        kbdKatakanaCharPi.Image = GetCharTexture(jp_fnt, 166, false);
        kbdKatakanaCharPu.Image = GetCharTexture(jp_fnt, 167, false);
        kbdKatakanaCharPe.Image = GetCharTexture(jp_fnt, 168, false);
        kbdKatakanaCharPo.Image = GetCharTexture(jp_fnt, 169, false);

        kbdKatakanaCharSmallA.Image = GetCharTexture(jp_fnt, 170, false);
        kbdKatakanaCharSmallI.Image = GetCharTexture(jp_fnt, 171, false);
        kbdKatakanaCharSmallU.Image = GetCharTexture(jp_fnt, 172, false);
        kbdKatakanaCharSmallE.Image = GetCharTexture(jp_fnt, 173, false);
        kbdKatakanaCharSmallO.Image = GetCharTexture(jp_fnt, 174, false);

        kbdKatakanaCharSmallYa.Image = GetCharTexture(jp_fnt, 175, false);
        kbdKatakanaCharSmallYu.Image = GetCharTexture(jp_fnt, 176, false);
        kbdKatakanaCharSmallYo.Image = GetCharTexture(jp_fnt, 177, false);
        kbdKatakanaCharSmallTsu.Image = GetCharTexture(jp_fnt, 179, false);

        // Romaji

        kbdRomajiCharA.Image = GetCharTexture(jp_fnt, 180, false);
        kbdRomajiCharB.Image = GetCharTexture(jp_fnt, 181, false);
        kbdRomajiCharC.Image = GetCharTexture(jp_fnt, 182, false);
        kbdRomajiCharD.Image = GetCharTexture(jp_fnt, 183, false);
        kbdRomajiCharE.Image = GetCharTexture(jp_fnt, 184, false);
        kbdRomajiCharF.Image = GetCharTexture(jp_fnt, 185, false);
        kbdRomajiCharG.Image = GetCharTexture(jp_fnt, 186, false);
        kbdRomajiCharH.Image = GetCharTexture(jp_fnt, 187, false);
        kbdRomajiCharI.Image = GetCharTexture(jp_fnt, 188, false);
        kbdRomajiCharJ.Image = GetCharTexture(jp_fnt, 189, false);
        kbdRomajiCharK.Image = GetCharTexture(jp_fnt, 190, false);
        kbdRomajiCharL.Image = GetCharTexture(jp_fnt, 191, false);
        kbdRomajiCharM.Image = GetCharTexture(jp_fnt, 192, false);
        kbdRomajiCharN.Image = GetCharTexture(jp_fnt, 193, false);
        kbdRomajiCharO.Image = GetCharTexture(jp_fnt, 194, false);
        kbdRomajiCharP.Image = GetCharTexture(jp_fnt, 195, false);
        kbdRomajiCharQ.Image = GetCharTexture(jp_fnt, 196, false);
        kbdRomajiCharR.Image = GetCharTexture(jp_fnt, 197, false);
        kbdRomajiCharS.Image = GetCharTexture(jp_fnt, 198, false);
        kbdRomajiCharT.Image = GetCharTexture(jp_fnt, 199, false);
        kbdRomajiCharU.Image = GetCharTexture(jp_fnt, 200, false);
        kbdRomajiCharV.Image = GetCharTexture(jp_fnt, 201, false);
        kbdRomajiCharW.Image = GetCharTexture(jp_fnt, 202, false);
        kbdRomajiCharX.Image = GetCharTexture(jp_fnt, 203, false);
        kbdRomajiCharY.Image = GetCharTexture(jp_fnt, 204, false);
        kbdRomajiCharZ.Image = GetCharTexture(jp_fnt, 205, false);

        kbdRomajiCharHyphen.Image = GetCharTexture(jp_fnt, 206, false);
        kbdRomajiCharTilde.Image = GetCharTexture(jp_fnt, 207, false);

        kbdJPMoveLeft.Image = GetCharTexture(jp_fnt, 90, false);
        kbdJPMoveRight.Image = GetCharTexture(jp_fnt, 91, false);
        Refresh();
    }

    private void UpdateDisplayName()
    {
        pictureJPNameChar0.Image = GetCharTexture(jp_fnt, jpChar[currName[0]], false);
        pictureJPNameChar1.Image = GetCharTexture(jp_fnt, jpChar[currName[1]], false);
        pictureJPNameChar2.Image = GetCharTexture(jp_fnt, jpChar[currName[2]], false);
        pictureJPNameChar3.Image = GetCharTexture(jp_fnt, jpChar[currName[3]], false);

        pictureJPCharHeart.Location = new Point(880 + charPos * 32, 36);
    }

    private static Image GetCharTexture(Bitmap jp_fnt, int tileID, bool hugLeft)
    {
        var tileset_width = 20; // Japanese Font
        var tile_w = 8;
        var tile_h = 16;
        var x = tileID % tileset_width * tile_w;
        var y = tileID / tileset_width * tile_h;
        var width = 8;
        var height = 16;
        var scale = 2;
        var crop = new Rectangle(x, y, width * scale, height * scale);
        var tex = new Bitmap(crop.Width, crop.Height);

        using (var gr = Graphics.FromImage(tex))
        {
            gr.InterpolationMode = InterpolationMode.NearestNeighbor;
            gr.PixelOffsetMode = PixelOffsetMode.Half;
            gr.DrawImage(jp_fnt, new Rectangle(0, 0, tex.Width * scale, tex.Height * scale), crop, GraphicsUnit.Pixel);
        }

        if (hugLeft)
        {
            return tex;
        }

        var bmp = new Bitmap(crop.Width * 2, crop.Height);

        using (var gr = Graphics.FromImage(bmp))
        {
            gr.InterpolationMode = InterpolationMode.NearestNeighbor;
            gr.PixelOffsetMode = PixelOffsetMode.Half;
            gr.Clear(Color.Black);
            gr.DrawImage(tex, 8, 2);
        }
        return bmp;
    }

    private void TypeChar(char c)
    {
        currName[charPos] = c;

        charPos++;
        if (charPos > 3)
        {
            charPos = 0;
        }

        // Draw the name to the screen
        UpdateDisplayName();
    }

    private void UpdatePlayerName()
    {
        // Update MainForm with the changed player name
        // If the name is too short, fill it with spaces
        for (var k = currName.Length; k < 4; k++)
        {
            currName[k] = '　';
        }

        mainForm.SetPlayerName(currName.ToString());
        var j = 0;
        for (var i = 0; i < currName.Length; i++)
        {
            currNameRaw[i] = rawJPChar.FirstOrDefault(x => x.Value == currName[i]).Key;
            j++;
        }
        mainForm.SetPlayerNameRaw(currNameRaw);
        mainForm.UpdatePlayerName();
    }

    private void NameChangingFormJP_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (autoClose)
        {
            return;
        }
        var dialogSave = MessageBox.Show("Would you like to save your changes?", "Save Changes?", MessageBoxButtons.YesNo);
        if (dialogSave == DialogResult.Yes)
        {
            UpdatePlayerName();
        }
        else
        {
            var dialogCloseConfirm = MessageBox.Show("Continue editing?", "Closing Name Changing Form (JPN)", MessageBoxButtons.YesNo);
            if (dialogCloseConfirm == DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }

    private void kbdJPMoveLeft_Click(object sender, EventArgs e)
    {
        charPos--;
        if (charPos < 0)
        {
            charPos = 3;
        }

        pictureJPCharHeart.Location = new Point(880 + charPos * 32, 36);
    }

    private void kbdJPMoveRight_Click(object sender, EventArgs e)
    {
        charPos++;
        if (charPos > 3)
        {
            charPos = 0;
        }

        pictureJPCharHeart.Location = new Point(880 + charPos * 32, 36);
    }

    private void kbdJPEnd_Click(object sender, EventArgs e)
    {
        UpdatePlayerName();
        autoClose = true;
        Close();
    }

    private void kbdHiraganaCharA_Click(object sender, EventArgs e) => TypeChar('あ');

    private void kbdHiraganaCharI_Click(object sender, EventArgs e) => TypeChar('い');

    private void kbdHiraganaCharU_Click(object sender, EventArgs e) => TypeChar('う');

    private void kbdHiraganaCharE_Click(object sender, EventArgs e) => TypeChar('え');

    private void kbdHiraganaCharO_Click(object sender, EventArgs e) => TypeChar('お');

    private void kbdHiraganaCharKa_Click(object sender, EventArgs e) => TypeChar('か');

    private void kbdHiraganaCharKi_Click(object sender, EventArgs e) => TypeChar('き');

    private void kbdHiraganaCharKu_Click(object sender, EventArgs e) => TypeChar('く');

    private void kbdHiraganaCharKe_Click(object sender, EventArgs e) => TypeChar('け');

    private void kbdHiraganaCharKo_Click(object sender, EventArgs e) => TypeChar('こ');

    private void kbdHiraganaCharSa_Click(object sender, EventArgs e) => TypeChar('さ');

    private void kbdHiraganaCharShi_Click(object sender, EventArgs e) => TypeChar('し');

    private void kbdHiraganaCharSu_Click(object sender, EventArgs e) => TypeChar('す');

    private void kbdHiraganaCharSe_Click(object sender, EventArgs e) => TypeChar('せ');

    private void kbdHiraganaCharSo_Click(object sender, EventArgs e) => TypeChar('そ');

    private void kbdHiraganaCharTa_Click(object sender, EventArgs e) => TypeChar('た');

    private void kbdHiraganaCharChi_Click(object sender, EventArgs e) => TypeChar('ち');

    private void kbdHiraganaCharTsu_Click(object sender, EventArgs e) => TypeChar('つ');

    private void kbdHiraganaCharTe_Click(object sender, EventArgs e) => TypeChar('て');

    private void kbdHiraganaCharTo_Click(object sender, EventArgs e) => TypeChar('と');

    private void kbdHiraganaCharNa_Click(object sender, EventArgs e) => TypeChar('な');

    private void kbdHiraganaCharNi_Click(object sender, EventArgs e) => TypeChar('に');

    private void kbdHiraganaCharNu_Click(object sender, EventArgs e) => TypeChar('ぬ');

    private void kbdHiraganaCharNe_Click(object sender, EventArgs e) => TypeChar('ね');

    private void kbdHiraganaCharNo_Click(object sender, EventArgs e) => TypeChar('の');

    private void kbdHiraganaCharHa_Click(object sender, EventArgs e) => TypeChar('は');

    private void kbdHiraganaCharHi_Click(object sender, EventArgs e) => TypeChar('ひ');

    private void kbdHiraganaCharFu_Click(object sender, EventArgs e) => TypeChar('ふ');

    private void kbdHiraganaCharHe_Click(object sender, EventArgs e) => TypeChar('へ');

    private void kbdHiraganaCharHo_Click(object sender, EventArgs e) => TypeChar('ほ');

    private void kbdHiraganaCharMa_Click(object sender, EventArgs e) => TypeChar('ま');

    private void kbdHiraganaCharMi_Click(object sender, EventArgs e) => TypeChar('み');

    private void kbdHiraganaCharMu_Click(object sender, EventArgs e) => TypeChar('む');

    private void kbdHiraganaCharMe_Click(object sender, EventArgs e) => TypeChar('め');

    private void kbdHiraganaCharMo_Click(object sender, EventArgs e) => TypeChar('も');

    private void kbdHiraganaCharYa_Click(object sender, EventArgs e) => TypeChar('や');

    private void kbdHiraganaCharYu_Click(object sender, EventArgs e) => TypeChar('ゆ');

    private void kbdHiraganaCharYo_Click(object sender, EventArgs e) => TypeChar('よ');

    private void kbdHiraganaCharRa_Click(object sender, EventArgs e) => TypeChar('ら');

    private void kbdHiraganaCharRi_Click(object sender, EventArgs e) => TypeChar('り');

    private void kbdHiraganaCharRu_Click(object sender, EventArgs e) => TypeChar('る');

    private void kbdHiraganaCharRe_Click(object sender, EventArgs e) => TypeChar('れ');

    private void kbdHiraganaCharRo_Click(object sender, EventArgs e) => TypeChar('ろ');

    private void kbdHiraganaCharWa_Click(object sender, EventArgs e) => TypeChar('わ');

    private void kbdHiraganaCharWo_Click(object sender, EventArgs e) => TypeChar('を');

    private void kbdHiraganaCharN_Click(object sender, EventArgs e) => TypeChar('ん');

    private void kbdHiraganaCharHyphen_Click(object sender, EventArgs e) => TypeChar('ー');

    private void kbdHiraganaCharGa_Click(object sender, EventArgs e) => TypeChar('が');

    private void kbdHiraganaCharGi_Click(object sender, EventArgs e) => TypeChar('ぎ');

    private void kbdHiraganaCharGu_Click(object sender, EventArgs e) => TypeChar('ぐ');

    private void kbdHiraganaCharGe_Click(object sender, EventArgs e) => TypeChar('げ');

    private void kbdHiraganaCharGo_Click(object sender, EventArgs e) => TypeChar('ご');

    private void kbdHiraganaCharZa_Click(object sender, EventArgs e) => TypeChar('ざ');

    private void kbdHiraganaCharJi_Click(object sender, EventArgs e) => TypeChar('じ');

    private void kbdHiraganaCharZu_Click(object sender, EventArgs e) => TypeChar('ず');

    private void kbdHiraganaCharZe_Click(object sender, EventArgs e) => TypeChar('ぜ');

    private void kbdHiraganaCharZo_Click(object sender, EventArgs e) => TypeChar('ぞ');

    private void kbdHiraganaCharDa_Click(object sender, EventArgs e) => TypeChar('だ');

    private void kbdHiraganaCharDi_Click(object sender, EventArgs e) => TypeChar('ぢ');

    private void kbdHiraganaCharDu_Click(object sender, EventArgs e) => TypeChar('づ');

    private void kbdHiraganaCharDe_Click(object sender, EventArgs e) => TypeChar('で');

    private void kbdHiraganaCharDo_Click(object sender, EventArgs e) => TypeChar('ど');

    private void kbdHiraganaCharBa_Click(object sender, EventArgs e) => TypeChar('ば');

    private void kbdHiraganaCharBi_Click(object sender, EventArgs e) => TypeChar('び');

    private void kbdHiraganaCharBu_Click(object sender, EventArgs e) => TypeChar('ぶ');

    private void kbdHiraganaCharBe_Click(object sender, EventArgs e) => TypeChar('べ');

    private void kbdHiraganaCharBo_Click(object sender, EventArgs e) => TypeChar('ぼ');

    private void kbdHiraganaCharPa_Click(object sender, EventArgs e) => TypeChar('ぱ');

    private void kbdHiraganaCharPi_Click(object sender, EventArgs e) => TypeChar('ぴ');

    private void kbdHiraganaCharPu_Click(object sender, EventArgs e) => TypeChar('ぷ');

    private void kbdHiraganaCharPe_Click(object sender, EventArgs e) => TypeChar('ぺ');

    private void kbdHiraganaCharPo_Click(object sender, EventArgs e) => TypeChar('ぽ');

    private void kbdHiraganaCharSmallA_Click(object sender, EventArgs e) => TypeChar('ぁ');

    private void kbdHiraganaCharSmallI_Click(object sender, EventArgs e) => TypeChar('ぃ');

    private void kbdHiraganaCharSmallU_Click(object sender, EventArgs e) => TypeChar('ぅ');

    private void kbdHiraganaCharSmallE_Click(object sender, EventArgs e) => TypeChar('ぇ');

    private void kbdHiraganaCharSmallO_Click(object sender, EventArgs e) => TypeChar('ぉ');

    private void kbdHiraganaCharSmallYa_Click(object sender, EventArgs e) => TypeChar('ゃ');

    private void kbdHiraganaCharSmallYu_Click(object sender, EventArgs e) => TypeChar('ゅ');

    private void kbdHiraganaCharSmallYo_Click(object sender, EventArgs e) => TypeChar('ょ');

    private void kbdHiraganaCharSmallTsu_Click(object sender, EventArgs e) => TypeChar('っ');

    private void kbdKatakanaCharA_Click(object sender, EventArgs e) => TypeChar('ア');

    private void kbdKatakanaCharI_Click(object sender, EventArgs e) => TypeChar('イ');

    private void kbdKatakanaCharU_Click(object sender, EventArgs e) => TypeChar('ウ');

    private void kbdKatakanaCharE_Click(object sender, EventArgs e) => TypeChar('エ');

    private void kbdKatakanaCharO_Click(object sender, EventArgs e) => TypeChar('オ');

    private void kbdKatakanaCharKa_Click(object sender, EventArgs e) => TypeChar('カ');

    private void kbdKatakanaCharKi_Click(object sender, EventArgs e) => TypeChar('キ');

    private void kbdKatakanaCharKu_Click(object sender, EventArgs e) => TypeChar('ク');

    private void kbdKatakanaCharKe_Click(object sender, EventArgs e) => TypeChar('ケ');

    private void kbdKatakanaCharKo_Click(object sender, EventArgs e) => TypeChar('コ');

    private void kbdKatakanaCharSa_Click(object sender, EventArgs e) => TypeChar('サ');

    private void kbdKatakanaCharShi_Click(object sender, EventArgs e) => TypeChar('シ');

    private void kbdKatakanaCharSu_Click(object sender, EventArgs e) => TypeChar('ス');

    private void kbdKatakanaCharSe_Click(object sender, EventArgs e) => TypeChar('セ');

    private void kbdKatakanaCharSo_Click(object sender, EventArgs e) => TypeChar('ソ');

    private void kbdKatakanaCharTa_Click(object sender, EventArgs e) => TypeChar('タ');

    private void kbdKatakanaCharChi_Click(object sender, EventArgs e) => TypeChar('チ');

    private void kbdKatakanaCharTsu_Click(object sender, EventArgs e) => TypeChar('ツ');

    private void kbdKatakanaCharTe_Click(object sender, EventArgs e) => TypeChar('テ');

    private void kbdKatakanaCharTo_Click(object sender, EventArgs e) => TypeChar('ト');

    private void kbdKatakanaCharNa_Click(object sender, EventArgs e) => TypeChar('ナ');

    private void kbdKatakanaCharNi_Click(object sender, EventArgs e) => TypeChar('ニ');

    private void kbdKatakanaCharNu_Click(object sender, EventArgs e) => TypeChar('ヌ');

    private void kbdKatakanaCharNe_Click(object sender, EventArgs e) => TypeChar('ネ');

    private void kbdKatakanaCharNo_Click(object sender, EventArgs e) => TypeChar('ノ');

    private void kbdKatakanaCharHa_Click(object sender, EventArgs e) => TypeChar('ハ');

    private void kbdKatakanaCharHi_Click(object sender, EventArgs e) => TypeChar('ヒ');

    private void kbdKatakanaCharFu_Click(object sender, EventArgs e) => TypeChar('フ');

    private void kbdKatakanaCharHe_Click(object sender, EventArgs e) => TypeChar('ヘ');

    private void kbdKatakanaCharHo_Click(object sender, EventArgs e) => TypeChar('ホ');

    private void kbdKatakanaCharMa_Click(object sender, EventArgs e) => TypeChar('マ');

    private void kbdKatakanaCharMi_Click(object sender, EventArgs e) => TypeChar('ミ');

    private void kbdKatakanaCharMu_Click(object sender, EventArgs e) => TypeChar('ム');

    private void kbdKatakanaCharMe_Click(object sender, EventArgs e) => TypeChar('メ');

    private void kbdKatakanaCharMo_Click(object sender, EventArgs e) => TypeChar('モ');

    private void kbdKatakanaCharYa_Click(object sender, EventArgs e) => TypeChar('ヤ');

    private void kbdKatakanaCharYu_Click(object sender, EventArgs e) => TypeChar('ユ');

    private void kbdKatakanaCharYo_Click(object sender, EventArgs e) => TypeChar('ヨ');

    private void kbdKatakanaCharRa_Click(object sender, EventArgs e) => TypeChar('ラ');

    private void kbdKatakanaCharRi_Click(object sender, EventArgs e) => TypeChar('リ');

    private void kbdKatakanaCharRu_Click(object sender, EventArgs e) => TypeChar('ル');

    private void kbdKatakanaCharRe_Click(object sender, EventArgs e) => TypeChar('レ');

    private void kbdKatakanaCharRo_Click(object sender, EventArgs e) => TypeChar('ロ');

    private void kbdKatakanaCharWa_Click(object sender, EventArgs e) => TypeChar('ワ');

    private void kbdKatakanaCharWo_Click(object sender, EventArgs e) => TypeChar('ヲ');

    private void kbdKatakanaCharN_Click(object sender, EventArgs e) => TypeChar('ン');

    private void kbdKatakanaCharHyphen_Click(object sender, EventArgs e) => TypeChar('ー');

    private void kbdKatakanaCharGa_Click(object sender, EventArgs e) => TypeChar('ガ');

    private void kbdKatakanaCharGi_Click(object sender, EventArgs e) => TypeChar('ギ');

    private void kbdKatakanaCharGu_Click(object sender, EventArgs e) => TypeChar('グ');

    private void kbdKatakanaCharGe_Click(object sender, EventArgs e) => TypeChar('ゲ');

    private void kbdKatakanaCharGo_Click(object sender, EventArgs e) => TypeChar('ゴ');

    private void kbdKatakanaCharZa_Click(object sender, EventArgs e) => TypeChar('ザ');

    private void kbdKatakanaCharJi_Click(object sender, EventArgs e) => TypeChar('ジ');

    private void kbdKatakanaCharZu_Click(object sender, EventArgs e) => TypeChar('ズ');

    private void kbdKatakanaCharZe_Click(object sender, EventArgs e) => TypeChar('ゼ');

    private void kbdKatakanaCharZo_Click(object sender, EventArgs e) => TypeChar('ゾ');

    private void kbdKatakanaCharDa_Click(object sender, EventArgs e) => TypeChar('ダ');

    private void kbdKatakanaCharDi_Click(object sender, EventArgs e) => TypeChar('ヂ');

    private void kbdKatakanaCharDu_Click(object sender, EventArgs e) => TypeChar('ヅ');

    private void kbdKatakanaCharDe_Click(object sender, EventArgs e) => TypeChar('デ');

    private void kbdKatakanaCharDo_Click(object sender, EventArgs e) => TypeChar('ド');

    private void kbdKatakanaCharBa_Click(object sender, EventArgs e) => TypeChar('バ');

    private void kbdKatakanaCharBi_Click(object sender, EventArgs e) => TypeChar('ビ');

    private void kbdKatakanaCharBu_Click(object sender, EventArgs e) => TypeChar('ブ');

    private void kbdKatakanaCharBe_Click(object sender, EventArgs e) => TypeChar('ベ');

    private void kbdKatakanaCharBo_Click(object sender, EventArgs e) => TypeChar('ボ');

    private void kbdKatakanaCharPa_Click(object sender, EventArgs e) => TypeChar('パ');

    private void kbdKatakanaCharPi_Click(object sender, EventArgs e) => TypeChar('ピ');

    private void kbdKatakanaCharPu_Click(object sender, EventArgs e) => TypeChar('プ');

    private void kbdKatakanaCharPe_Click(object sender, EventArgs e) => TypeChar('ペ');

    private void kbdKatakanaCharPo_Click(object sender, EventArgs e) => TypeChar('ポ');

    private void kbdKatakanaCharSmallA_Click(object sender, EventArgs e) => TypeChar('ァ');

    private void kbdKatakanaCharSmallI_Click(object sender, EventArgs e) => TypeChar('ィ');

    private void kbdKatakanaCharSmallU_Click(object sender, EventArgs e) => TypeChar('ゥ');

    private void kbdKatakanaCharSmallE_Click(object sender, EventArgs e) => TypeChar('ェ');

    private void kbdKatakanaCharSmallO_Click(object sender, EventArgs e) => TypeChar('ォ');

    private void kbdKatakanaCharSmallYa_Click(object sender, EventArgs e) => TypeChar('ャ');

    private void kbdKatakanaCharSmallYu_Click(object sender, EventArgs e) => TypeChar('ュ');

    private void kbdKatakanaCharSmallYo_Click(object sender, EventArgs e) => TypeChar('ョ');

    private void kbdKatakanaCharSmallTsu_Click(object sender, EventArgs e) => TypeChar('ッ');

    private void kbdRomajiCharA_Click(object sender, EventArgs e) => TypeChar('A');

    private void kbdRomajiCharB_Click(object sender, EventArgs e) => TypeChar('B');

    private void kbdRomajiCharC_Click(object sender, EventArgs e) => TypeChar('C');

    private void kbdRomajiCharD_Click(object sender, EventArgs e) => TypeChar('D');

    private void kbdRomajiCharE_Click(object sender, EventArgs e) => TypeChar('E');

    private void kbdRomajiCharF_Click(object sender, EventArgs e) => TypeChar('F');

    private void kbdRomajiCharG_Click(object sender, EventArgs e) => TypeChar('G');

    private void kbdRomajiCharH_Click(object sender, EventArgs e) => TypeChar('H');

    private void kbdRomajiCharI_Click(object sender, EventArgs e) => TypeChar('I');

    private void kbdRomajiCharJ_Click(object sender, EventArgs e) => TypeChar('J');

    private void kbdRomajiCharK_Click(object sender, EventArgs e) => TypeChar('K');

    private void kbdRomajiCharL_Click(object sender, EventArgs e) => TypeChar('L');

    private void kbdRomajiCharM_Click(object sender, EventArgs e) => TypeChar('M');

    private void kbdRomajiCharN_Click(object sender, EventArgs e) => TypeChar('N');

    private void kbdRomajiCharO_Click(object sender, EventArgs e) => TypeChar('O');

    private void kbdRomajiCharP_Click(object sender, EventArgs e) => TypeChar('P');

    private void kbdRomajiCharQ_Click(object sender, EventArgs e) => TypeChar('Q');

    private void kbdRomajiCharR_Click(object sender, EventArgs e) => TypeChar('R');

    private void kbdRomajiCharS_Click(object sender, EventArgs e) => TypeChar('S');

    private void kbdRomajiCharT_Click(object sender, EventArgs e) => TypeChar('T');

    private void kbdRomajiCharU_Click(object sender, EventArgs e) => TypeChar('U');

    private void kbdRomajiCharV_Click(object sender, EventArgs e) => TypeChar('V');

    private void kbdRomajiCharW_Click(object sender, EventArgs e) => TypeChar('W');

    private void kbdRomajiCharX_Click(object sender, EventArgs e) => TypeChar('X');

    private void kbdRomajiCharY_Click(object sender, EventArgs e) => TypeChar('Y');

    private void kbdRomajiCharZ_Click(object sender, EventArgs e) => TypeChar('Z');

    private void kbdRomajiCharHyphen_Click(object sender, EventArgs e) => TypeChar('ー');

    private void kbdRomajiCharTilde_Click(object sender, EventArgs e) => TypeChar('~');

    private void kbdJPSpace_Click(object sender, EventArgs e) => TypeChar('　');
}
