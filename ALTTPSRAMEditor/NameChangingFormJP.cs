namespace ALTTPSRAMEditor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class NameChangingFormJp : Form
{
    private Bitmap jpFnt = default!;
    private readonly StringBuilder currName;
    private readonly ushort[] currNameRaw;
    private readonly Dictionary<char, int> jpChar;
    private readonly Dictionary<ushort, char> rawJpChar;
    private int charPos = 0;
    private bool autoClose;
    private readonly MainForm mainForm;

    public NameChangingFormJp(MainForm mainForm)
    {
        InitializeComponent();
        this.mainForm = mainForm;
        jpChar = AppState.jpChar;
        rawJpChar = AppState.rawJpChar;
        autoClose = false;
        currName = new StringBuilder(this.mainForm.GetPlayerName()[..4]);
        currNameRaw = new ushort[6];
    }

    private void NameChangingFormJp_KeyDown(object sender, KeyEventArgs e)
    {
        // Close the Name Changing form if we hit Escape
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    private void NameChangingFormJp_Load(object sender, EventArgs e)
    {

        // Name Changing Form Initialization
        jpFnt = new Bitmap(Properties.Resources.jpn_font);

        // Draw the name to the screen
        UpdateDisplayName();

        // Hiragana

        kbdHiraganaCharA.Image = GetCharTexture(jpFnt, 5, SaveRegion.JPN);
        kbdHiraganaCharI.Image = GetCharTexture(jpFnt, 6, SaveRegion.JPN);
        kbdHiraganaCharU.Image = GetCharTexture(jpFnt, 7, SaveRegion.JPN);
        kbdHiraganaCharE.Image = GetCharTexture(jpFnt, 8, SaveRegion.JPN);
        kbdHiraganaCharO.Image = GetCharTexture(jpFnt, 9, SaveRegion.JPN);

        kbdHiraganaCharKa.Image = GetCharTexture(jpFnt, 10, SaveRegion.JPN);
        kbdHiraganaCharKi.Image = GetCharTexture(jpFnt, 11, SaveRegion.JPN);
        kbdHiraganaCharKu.Image = GetCharTexture(jpFnt, 12, SaveRegion.JPN);
        kbdHiraganaCharKe.Image = GetCharTexture(jpFnt, 13, SaveRegion.JPN);
        kbdHiraganaCharKo.Image = GetCharTexture(jpFnt, 14, SaveRegion.JPN);

        kbdHiraganaCharSa.Image = GetCharTexture(jpFnt, 15, SaveRegion.JPN);
        kbdHiraganaCharShi.Image = GetCharTexture(jpFnt, 16, SaveRegion.JPN);
        kbdHiraganaCharSu.Image = GetCharTexture(jpFnt, 17, SaveRegion.JPN);
        kbdHiraganaCharSe.Image = GetCharTexture(jpFnt, 18, SaveRegion.JPN);
        kbdHiraganaCharSo.Image = GetCharTexture(jpFnt, 19, SaveRegion.JPN);

        kbdHiraganaCharTa.Image = GetCharTexture(jpFnt, 20, SaveRegion.JPN);
        kbdHiraganaCharChi.Image = GetCharTexture(jpFnt, 21, SaveRegion.JPN);
        kbdHiraganaCharTsu.Image = GetCharTexture(jpFnt, 22, SaveRegion.JPN);
        kbdHiraganaCharTe.Image = GetCharTexture(jpFnt, 23, SaveRegion.JPN);
        kbdHiraganaCharTo.Image = GetCharTexture(jpFnt, 24, SaveRegion.JPN);

        kbdHiraganaCharNa.Image = GetCharTexture(jpFnt, 25, SaveRegion.JPN);
        kbdHiraganaCharNi.Image = GetCharTexture(jpFnt, 26, SaveRegion.JPN);
        kbdHiraganaCharNu.Image = GetCharTexture(jpFnt, 27, SaveRegion.JPN);
        kbdHiraganaCharNe.Image = GetCharTexture(jpFnt, 28, SaveRegion.JPN);
        kbdHiraganaCharNo.Image = GetCharTexture(jpFnt, 29, SaveRegion.JPN);

        kbdHiraganaCharHa.Image = GetCharTexture(jpFnt, 30, SaveRegion.JPN);
        kbdHiraganaCharHi.Image = GetCharTexture(jpFnt, 31, SaveRegion.JPN);
        kbdHiraganaCharFu.Image = GetCharTexture(jpFnt, 32, SaveRegion.JPN);
        kbdHiraganaCharHe.Image = GetCharTexture(jpFnt, 33, SaveRegion.JPN);
        kbdHiraganaCharHo.Image = GetCharTexture(jpFnt, 34, SaveRegion.JPN);

        kbdHiraganaCharMa.Image = GetCharTexture(jpFnt, 35, SaveRegion.JPN);
        kbdHiraganaCharMi.Image = GetCharTexture(jpFnt, 36, SaveRegion.JPN);
        kbdHiraganaCharMu.Image = GetCharTexture(jpFnt, 37, SaveRegion.JPN);
        kbdHiraganaCharMe.Image = GetCharTexture(jpFnt, 38, SaveRegion.JPN);
        kbdHiraganaCharMo.Image = GetCharTexture(jpFnt, 39, SaveRegion.JPN);

        kbdHiraganaCharYa.Image = GetCharTexture(jpFnt, 40, SaveRegion.JPN);
        kbdHiraganaCharYu.Image = GetCharTexture(jpFnt, 42, SaveRegion.JPN);
        kbdHiraganaCharYo.Image = GetCharTexture(jpFnt, 44, SaveRegion.JPN);

        kbdHiraganaCharRa.Image = GetCharTexture(jpFnt, 45, SaveRegion.JPN);
        kbdHiraganaCharRi.Image = GetCharTexture(jpFnt, 46, SaveRegion.JPN);
        kbdHiraganaCharRu.Image = GetCharTexture(jpFnt, 47, SaveRegion.JPN);
        kbdHiraganaCharRe.Image = GetCharTexture(jpFnt, 48, SaveRegion.JPN);
        kbdHiraganaCharRo.Image = GetCharTexture(jpFnt, 49, SaveRegion.JPN);

        kbdHiraganaCharWa.Image = GetCharTexture(jpFnt, 50, SaveRegion.JPN);
        kbdHiraganaCharWo.Image = GetCharTexture(jpFnt, 51, SaveRegion.JPN);
        kbdHiraganaCharN.Image = GetCharTexture(jpFnt, 52, SaveRegion.JPN);
        kbdHiraganaCharHyphen.Image = GetCharTexture(jpFnt, 54, SaveRegion.JPN);

        kbdHiraganaCharGa.Image = GetCharTexture(jpFnt, 55, SaveRegion.JPN);
        kbdHiraganaCharGi.Image = GetCharTexture(jpFnt, 56, SaveRegion.JPN);
        kbdHiraganaCharGu.Image = GetCharTexture(jpFnt, 57, SaveRegion.JPN);
        kbdHiraganaCharGe.Image = GetCharTexture(jpFnt, 58, SaveRegion.JPN);
        kbdHiraganaCharGo.Image = GetCharTexture(jpFnt, 59, SaveRegion.JPN);


        kbdHiraganaCharZa.Image = GetCharTexture(jpFnt, 60, SaveRegion.JPN);
        kbdHiraganaCharJi.Image = GetCharTexture(jpFnt, 61, SaveRegion.JPN);
        kbdHiraganaCharZu.Image = GetCharTexture(jpFnt, 62, SaveRegion.JPN);
        kbdHiraganaCharZe.Image = GetCharTexture(jpFnt, 63, SaveRegion.JPN);
        kbdHiraganaCharZo.Image = GetCharTexture(jpFnt, 64, SaveRegion.JPN);

        kbdHiraganaCharDa.Image = GetCharTexture(jpFnt, 65, SaveRegion.JPN);
        kbdHiraganaCharDi.Image = GetCharTexture(jpFnt, 66, SaveRegion.JPN);
        kbdHiraganaCharDu.Image = GetCharTexture(jpFnt, 67, SaveRegion.JPN);
        kbdHiraganaCharDe.Image = GetCharTexture(jpFnt, 68, SaveRegion.JPN);
        kbdHiraganaCharDo.Image = GetCharTexture(jpFnt, 69, SaveRegion.JPN);

        kbdHiraganaCharBa.Image = GetCharTexture(jpFnt, 70, SaveRegion.JPN);
        kbdHiraganaCharBi.Image = GetCharTexture(jpFnt, 71, SaveRegion.JPN);
        kbdHiraganaCharBu.Image = GetCharTexture(jpFnt, 72, SaveRegion.JPN);
        kbdHiraganaCharBe.Image = GetCharTexture(jpFnt, 73, SaveRegion.JPN);
        kbdHiraganaCharBo.Image = GetCharTexture(jpFnt, 74, SaveRegion.JPN);

        kbdHiraganaCharPa.Image = GetCharTexture(jpFnt, 75, SaveRegion.JPN);
        kbdHiraganaCharPi.Image = GetCharTexture(jpFnt, 76, SaveRegion.JPN);
        kbdHiraganaCharPu.Image = GetCharTexture(jpFnt, 77, SaveRegion.JPN);
        kbdHiraganaCharPe.Image = GetCharTexture(jpFnt, 78, SaveRegion.JPN);
        kbdHiraganaCharPo.Image = GetCharTexture(jpFnt, 79, SaveRegion.JPN);

        kbdHiraganaCharSmallA.Image = GetCharTexture(jpFnt, 80, SaveRegion.JPN);
        kbdHiraganaCharSmallI.Image = GetCharTexture(jpFnt, 81, SaveRegion.JPN);
        kbdHiraganaCharSmallU.Image = GetCharTexture(jpFnt, 82, SaveRegion.JPN);
        kbdHiraganaCharSmallE.Image = GetCharTexture(jpFnt, 83, SaveRegion.JPN);
        kbdHiraganaCharSmallO.Image = GetCharTexture(jpFnt, 84, SaveRegion.JPN);

        kbdHiraganaCharSmallYa.Image = GetCharTexture(jpFnt, 85, SaveRegion.JPN);
        kbdHiraganaCharSmallYu.Image = GetCharTexture(jpFnt, 86, SaveRegion.JPN);
        kbdHiraganaCharSmallYo.Image = GetCharTexture(jpFnt, 87, SaveRegion.JPN);
        kbdHiraganaCharSmallTsu.Image = GetCharTexture(jpFnt, 89, SaveRegion.JPN);

        // Katakana

        kbdKatakanaCharA.Image = GetCharTexture(jpFnt, 95, SaveRegion.JPN);
        kbdKatakanaCharI.Image = GetCharTexture(jpFnt, 96, SaveRegion.JPN);
        kbdKatakanaCharU.Image = GetCharTexture(jpFnt, 97, SaveRegion.JPN);
        kbdKatakanaCharE.Image = GetCharTexture(jpFnt, 98, SaveRegion.JPN);
        kbdKatakanaCharO.Image = GetCharTexture(jpFnt, 99, SaveRegion.JPN);

        kbdKatakanaCharKa.Image = GetCharTexture(jpFnt, 100, SaveRegion.JPN);
        kbdKatakanaCharKi.Image = GetCharTexture(jpFnt, 101, SaveRegion.JPN);
        kbdKatakanaCharKu.Image = GetCharTexture(jpFnt, 102, SaveRegion.JPN);
        kbdKatakanaCharKe.Image = GetCharTexture(jpFnt, 103, SaveRegion.JPN);
        kbdKatakanaCharKo.Image = GetCharTexture(jpFnt, 104, SaveRegion.JPN);

        kbdKatakanaCharSa.Image = GetCharTexture(jpFnt, 105, SaveRegion.JPN);
        kbdKatakanaCharShi.Image = GetCharTexture(jpFnt, 106, SaveRegion.JPN);
        kbdKatakanaCharSu.Image = GetCharTexture(jpFnt, 107, SaveRegion.JPN);
        kbdKatakanaCharSe.Image = GetCharTexture(jpFnt, 108, SaveRegion.JPN);
        kbdKatakanaCharSo.Image = GetCharTexture(jpFnt, 109, SaveRegion.JPN);

        kbdKatakanaCharTa.Image = GetCharTexture(jpFnt, 110, SaveRegion.JPN);
        kbdKatakanaCharChi.Image = GetCharTexture(jpFnt, 111, SaveRegion.JPN);
        kbdKatakanaCharTsu.Image = GetCharTexture(jpFnt, 112, SaveRegion.JPN);
        kbdKatakanaCharTe.Image = GetCharTexture(jpFnt, 113, SaveRegion.JPN);
        kbdKatakanaCharTo.Image = GetCharTexture(jpFnt, 114, SaveRegion.JPN);

        kbdKatakanaCharNa.Image = GetCharTexture(jpFnt, 115, SaveRegion.JPN);
        kbdKatakanaCharNi.Image = GetCharTexture(jpFnt, 116, SaveRegion.JPN);
        kbdKatakanaCharNu.Image = GetCharTexture(jpFnt, 117, SaveRegion.JPN);
        kbdKatakanaCharNe.Image = GetCharTexture(jpFnt, 118, SaveRegion.JPN);
        kbdKatakanaCharNo.Image = GetCharTexture(jpFnt, 119, SaveRegion.JPN);

        kbdKatakanaCharHa.Image = GetCharTexture(jpFnt, 120, SaveRegion.JPN);
        kbdKatakanaCharHi.Image = GetCharTexture(jpFnt, 121, SaveRegion.JPN);
        kbdKatakanaCharFu.Image = GetCharTexture(jpFnt, 122, SaveRegion.JPN);
        kbdKatakanaCharHe.Image = GetCharTexture(jpFnt, 123, SaveRegion.JPN);
        kbdKatakanaCharHo.Image = GetCharTexture(jpFnt, 124, SaveRegion.JPN);

        kbdKatakanaCharMa.Image = GetCharTexture(jpFnt, 125, SaveRegion.JPN);
        kbdKatakanaCharMi.Image = GetCharTexture(jpFnt, 126, SaveRegion.JPN);
        kbdKatakanaCharMu.Image = GetCharTexture(jpFnt, 127, SaveRegion.JPN);
        kbdKatakanaCharMe.Image = GetCharTexture(jpFnt, 128, SaveRegion.JPN);
        kbdKatakanaCharMo.Image = GetCharTexture(jpFnt, 129, SaveRegion.JPN);

        kbdKatakanaCharYa.Image = GetCharTexture(jpFnt, 130, SaveRegion.JPN);
        kbdKatakanaCharYu.Image = GetCharTexture(jpFnt, 132, SaveRegion.JPN);
        kbdKatakanaCharYo.Image = GetCharTexture(jpFnt, 134, SaveRegion.JPN);

        kbdKatakanaCharRa.Image = GetCharTexture(jpFnt, 135, SaveRegion.JPN);
        kbdKatakanaCharRi.Image = GetCharTexture(jpFnt, 136, SaveRegion.JPN);
        kbdKatakanaCharRu.Image = GetCharTexture(jpFnt, 137, SaveRegion.JPN);
        kbdKatakanaCharRe.Image = GetCharTexture(jpFnt, 138, SaveRegion.JPN);
        kbdKatakanaCharRo.Image = GetCharTexture(jpFnt, 139, SaveRegion.JPN);

        kbdKatakanaCharWa.Image = GetCharTexture(jpFnt, 140, SaveRegion.JPN);
        kbdKatakanaCharWo.Image = GetCharTexture(jpFnt, 141, SaveRegion.JPN);
        kbdKatakanaCharN.Image = GetCharTexture(jpFnt, 142, SaveRegion.JPN);
        kbdKatakanaCharHyphen.Image = GetCharTexture(jpFnt, 144, SaveRegion.JPN);

        kbdKatakanaCharGa.Image = GetCharTexture(jpFnt, 145, SaveRegion.JPN);
        kbdKatakanaCharGi.Image = GetCharTexture(jpFnt, 146, SaveRegion.JPN);
        kbdKatakanaCharGu.Image = GetCharTexture(jpFnt, 147, SaveRegion.JPN);
        kbdKatakanaCharGe.Image = GetCharTexture(jpFnt, 148, SaveRegion.JPN);
        kbdKatakanaCharGo.Image = GetCharTexture(jpFnt, 149, SaveRegion.JPN);

        kbdKatakanaCharZa.Image = GetCharTexture(jpFnt, 150, SaveRegion.JPN);
        kbdKatakanaCharJi.Image = GetCharTexture(jpFnt, 151, SaveRegion.JPN);
        kbdKatakanaCharZu.Image = GetCharTexture(jpFnt, 152, SaveRegion.JPN);
        kbdKatakanaCharZe.Image = GetCharTexture(jpFnt, 153, SaveRegion.JPN);
        kbdKatakanaCharZo.Image = GetCharTexture(jpFnt, 154, SaveRegion.JPN);

        kbdKatakanaCharDa.Image = GetCharTexture(jpFnt, 155, SaveRegion.JPN);
        kbdKatakanaCharDi.Image = GetCharTexture(jpFnt, 156, SaveRegion.JPN);
        kbdKatakanaCharDu.Image = GetCharTexture(jpFnt, 157, SaveRegion.JPN);
        kbdKatakanaCharDe.Image = GetCharTexture(jpFnt, 158, SaveRegion.JPN);
        kbdKatakanaCharDo.Image = GetCharTexture(jpFnt, 159, SaveRegion.JPN);

        kbdKatakanaCharBa.Image = GetCharTexture(jpFnt, 160, SaveRegion.JPN);
        kbdKatakanaCharBi.Image = GetCharTexture(jpFnt, 161, SaveRegion.JPN);
        kbdKatakanaCharBu.Image = GetCharTexture(jpFnt, 162, SaveRegion.JPN);
        kbdKatakanaCharBe.Image = GetCharTexture(jpFnt, 163, SaveRegion.JPN);
        kbdKatakanaCharBo.Image = GetCharTexture(jpFnt, 164, SaveRegion.JPN);

        kbdKatakanaCharPa.Image = GetCharTexture(jpFnt, 165, SaveRegion.JPN);
        kbdKatakanaCharPi.Image = GetCharTexture(jpFnt, 166, SaveRegion.JPN);
        kbdKatakanaCharPu.Image = GetCharTexture(jpFnt, 167, SaveRegion.JPN);
        kbdKatakanaCharPe.Image = GetCharTexture(jpFnt, 168, SaveRegion.JPN);
        kbdKatakanaCharPo.Image = GetCharTexture(jpFnt, 169, SaveRegion.JPN);

        kbdKatakanaCharSmallA.Image = GetCharTexture(jpFnt, 170, SaveRegion.JPN);
        kbdKatakanaCharSmallI.Image = GetCharTexture(jpFnt, 171, SaveRegion.JPN);
        kbdKatakanaCharSmallU.Image = GetCharTexture(jpFnt, 172, SaveRegion.JPN);
        kbdKatakanaCharSmallE.Image = GetCharTexture(jpFnt, 173, SaveRegion.JPN);
        kbdKatakanaCharSmallO.Image = GetCharTexture(jpFnt, 174, SaveRegion.JPN);

        kbdKatakanaCharSmallYa.Image = GetCharTexture(jpFnt, 175, SaveRegion.JPN);
        kbdKatakanaCharSmallYu.Image = GetCharTexture(jpFnt, 176, SaveRegion.JPN);
        kbdKatakanaCharSmallYo.Image = GetCharTexture(jpFnt, 177, SaveRegion.JPN);
        kbdKatakanaCharSmallTsu.Image = GetCharTexture(jpFnt, 179, SaveRegion.JPN);

        // Romaji

        kbdRomajiCharA.Image = GetCharTexture(jpFnt, 180, SaveRegion.JPN);
        kbdRomajiCharB.Image = GetCharTexture(jpFnt, 181, SaveRegion.JPN);
        kbdRomajiCharC.Image = GetCharTexture(jpFnt, 182, SaveRegion.JPN);
        kbdRomajiCharD.Image = GetCharTexture(jpFnt, 183, SaveRegion.JPN);
        kbdRomajiCharE.Image = GetCharTexture(jpFnt, 184, SaveRegion.JPN);
        kbdRomajiCharF.Image = GetCharTexture(jpFnt, 185, SaveRegion.JPN);
        kbdRomajiCharG.Image = GetCharTexture(jpFnt, 186, SaveRegion.JPN);
        kbdRomajiCharH.Image = GetCharTexture(jpFnt, 187, SaveRegion.JPN);
        kbdRomajiCharI.Image = GetCharTexture(jpFnt, 188, SaveRegion.JPN);
        kbdRomajiCharJ.Image = GetCharTexture(jpFnt, 189, SaveRegion.JPN);
        kbdRomajiCharK.Image = GetCharTexture(jpFnt, 190, SaveRegion.JPN);
        kbdRomajiCharL.Image = GetCharTexture(jpFnt, 191, SaveRegion.JPN);
        kbdRomajiCharM.Image = GetCharTexture(jpFnt, 192, SaveRegion.JPN);
        kbdRomajiCharN.Image = GetCharTexture(jpFnt, 193, SaveRegion.JPN);
        kbdRomajiCharO.Image = GetCharTexture(jpFnt, 194, SaveRegion.JPN);
        kbdRomajiCharP.Image = GetCharTexture(jpFnt, 195, SaveRegion.JPN);
        kbdRomajiCharQ.Image = GetCharTexture(jpFnt, 196, SaveRegion.JPN);
        kbdRomajiCharR.Image = GetCharTexture(jpFnt, 197, SaveRegion.JPN);
        kbdRomajiCharS.Image = GetCharTexture(jpFnt, 198, SaveRegion.JPN);
        kbdRomajiCharT.Image = GetCharTexture(jpFnt, 199, SaveRegion.JPN);
        kbdRomajiCharU.Image = GetCharTexture(jpFnt, 200, SaveRegion.JPN);
        kbdRomajiCharV.Image = GetCharTexture(jpFnt, 201, SaveRegion.JPN);
        kbdRomajiCharW.Image = GetCharTexture(jpFnt, 202, SaveRegion.JPN);
        kbdRomajiCharX.Image = GetCharTexture(jpFnt, 203, SaveRegion.JPN);
        kbdRomajiCharY.Image = GetCharTexture(jpFnt, 204, SaveRegion.JPN);
        kbdRomajiCharZ.Image = GetCharTexture(jpFnt, 205, SaveRegion.JPN);

        kbdRomajiCharHyphen.Image = GetCharTexture(jpFnt, 206, SaveRegion.JPN);
        kbdRomajiCharTilde.Image = GetCharTexture(jpFnt, 207, SaveRegion.JPN);

        kbdJPMoveLeft.Image = GetCharTexture(jpFnt, 90, SaveRegion.JPN);
        kbdJPMoveRight.Image = GetCharTexture(jpFnt, 91, SaveRegion.JPN);
        Refresh();
    }

    private void UpdateDisplayName()
    {
        pictureJPNameChar0.Image = GetCharTexture(jpFnt, jpChar[currName[0]], SaveRegion.JPN);
        pictureJPNameChar1.Image = GetCharTexture(jpFnt, jpChar[currName[1]], SaveRegion.JPN);
        pictureJPNameChar2.Image = GetCharTexture(jpFnt, jpChar[currName[2]], SaveRegion.JPN);
        pictureJPNameChar3.Image = GetCharTexture(jpFnt, jpChar[currName[3]], SaveRegion.JPN);

        pictureJPCharHeart.Location = new Point(880 + charPos * 32, 36);
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
            currNameRaw[i] = rawJpChar.FirstOrDefault(x => x.Value == currName[i]).Key;
            j++;
        }
        mainForm.SetPlayerNameRaw(currNameRaw);
        mainForm.UpdatePlayerName();
    }

    private void NameChangingFormJp_FormClosing(object sender, FormClosingEventArgs e)
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

    private void KbdJpChar_Click(object sender, EventArgs e)
    {
        char? charToType = (sender as PictureBox)?.Name switch
        {
            nameof(kbdHiraganaCharA) => 'あ',
            nameof(kbdHiraganaCharI) => 'い',
            nameof(kbdHiraganaCharU) => 'う',
            nameof(kbdHiraganaCharE) => 'え',
            nameof(kbdHiraganaCharO) => 'お',
            nameof(kbdHiraganaCharKa) => 'か',
            nameof(kbdHiraganaCharKi) => 'き',
            nameof(kbdHiraganaCharKu) => 'く',
            nameof(kbdHiraganaCharKe) => 'け',
            nameof(kbdHiraganaCharKo) => 'こ',
            nameof(kbdHiraganaCharSa) => 'さ',
            nameof(kbdHiraganaCharShi) => 'し',
            nameof(kbdHiraganaCharSu) => 'す',
            nameof(kbdHiraganaCharSe) => 'せ',
            nameof(kbdHiraganaCharSo) => 'そ',
            nameof(kbdHiraganaCharTa) => 'た',
            nameof(kbdHiraganaCharChi) => 'ち',
            nameof(kbdHiraganaCharTsu) => 'つ',
            nameof(kbdHiraganaCharTe) => 'て',
            nameof(kbdHiraganaCharTo) => 'と',
            nameof(kbdHiraganaCharNa) => 'な',
            nameof(kbdHiraganaCharNi) => 'に',
            nameof(kbdHiraganaCharNu) => 'ぬ',
            nameof(kbdHiraganaCharNe) => 'ね',
            nameof(kbdHiraganaCharNo) => 'の',
            nameof(kbdHiraganaCharHa) => 'は',
            nameof(kbdHiraganaCharHi) => 'ひ',
            nameof(kbdHiraganaCharFu) => 'ふ',
            nameof(kbdHiraganaCharHe) => 'へ',
            nameof(kbdHiraganaCharHo) => 'ほ',
            nameof(kbdHiraganaCharMa) => 'ま',
            nameof(kbdHiraganaCharMi) => 'み',
            nameof(kbdHiraganaCharMu) => 'む',
            nameof(kbdHiraganaCharMe) => 'め',
            nameof(kbdHiraganaCharMo) => 'も',
            nameof(kbdHiraganaCharYa) => 'や',
            nameof(kbdHiraganaCharYu) => 'ゆ',
            nameof(kbdHiraganaCharYo) => 'よ',
            nameof(kbdHiraganaCharRa) => 'ら',
            nameof(kbdHiraganaCharRi) => 'り',
            nameof(kbdHiraganaCharRu) => 'る',
            nameof(kbdHiraganaCharRe) => 'れ',
            nameof(kbdHiraganaCharRo) => 'ろ',
            nameof(kbdHiraganaCharWa) => 'わ',
            nameof(kbdHiraganaCharWo) => 'を',
            nameof(kbdHiraganaCharN) => 'ん',

            nameof(kbdHiraganaCharHyphen) => 'ー',

            nameof(kbdHiraganaCharGa) => 'が',
            nameof(kbdHiraganaCharGi) => 'ぎ',
            nameof(kbdHiraganaCharGu) => 'ぐ',
            nameof(kbdHiraganaCharGe) => 'げ',
            nameof(kbdHiraganaCharGo) => 'ご',
            nameof(kbdHiraganaCharZa) => 'ざ',
            nameof(kbdHiraganaCharJi) => 'じ',
            nameof(kbdHiraganaCharZu) => 'ず',
            nameof(kbdHiraganaCharZe) => 'ぜ',
            nameof(kbdHiraganaCharZo) => 'ぞ',
            nameof(kbdHiraganaCharDa) => 'だ',
            nameof(kbdHiraganaCharDi) => 'ぢ',
            nameof(kbdHiraganaCharDu) => 'づ',
            nameof(kbdHiraganaCharDe) => 'で',
            nameof(kbdHiraganaCharDo) => 'ど',
            nameof(kbdHiraganaCharBa) => 'ば',
            nameof(kbdHiraganaCharBi) => 'び',
            nameof(kbdHiraganaCharBu) => 'ぶ',
            nameof(kbdHiraganaCharBe) => 'べ',
            nameof(kbdHiraganaCharBo) => 'ぼ',
            nameof(kbdHiraganaCharPa) => 'ぱ',
            nameof(kbdHiraganaCharPi) => 'ぴ',
            nameof(kbdHiraganaCharPu) => 'ぷ',
            nameof(kbdHiraganaCharPe) => 'ぺ',
            nameof(kbdHiraganaCharPo) => 'ぽ',

            nameof(kbdHiraganaCharSmallA) => 'ぁ',
            nameof(kbdHiraganaCharSmallI) => 'ぃ',
            nameof(kbdHiraganaCharSmallU) => 'ぅ',
            nameof(kbdHiraganaCharSmallE) => 'ぇ',
            nameof(kbdHiraganaCharSmallO) => 'ぉ',
            nameof(kbdHiraganaCharSmallYa) => 'ゃ',
            nameof(kbdHiraganaCharSmallYu) => 'ゅ',
            nameof(kbdHiraganaCharSmallYo) => 'ょ',
            nameof(kbdHiraganaCharSmallTsu) => 'っ',

            nameof(kbdKatakanaCharA) => 'ア',
            nameof(kbdKatakanaCharI) => 'イ',
            nameof(kbdKatakanaCharU) => 'ウ',
            nameof(kbdKatakanaCharE) => 'エ',
            nameof(kbdKatakanaCharO) => 'オ',
            nameof(kbdKatakanaCharKa) => 'カ',
            nameof(kbdKatakanaCharKi) => 'キ',
            nameof(kbdKatakanaCharKu) => 'ク',
            nameof(kbdKatakanaCharKe) => 'ケ',
            nameof(kbdKatakanaCharKo) => 'コ',
            nameof(kbdKatakanaCharSa) => 'サ',
            nameof(kbdKatakanaCharShi) => 'シ',
            nameof(kbdKatakanaCharSu) => 'ス',
            nameof(kbdKatakanaCharSe) => 'セ',
            nameof(kbdKatakanaCharSo) => 'ソ',
            nameof(kbdKatakanaCharTa) => 'タ',
            nameof(kbdKatakanaCharChi) => 'チ',
            nameof(kbdKatakanaCharTsu) => 'ツ',
            nameof(kbdKatakanaCharTe) => 'テ',
            nameof(kbdKatakanaCharTo) => 'ト',
            nameof(kbdKatakanaCharNa) => 'ナ',
            nameof(kbdKatakanaCharNi) => 'ニ',
            nameof(kbdKatakanaCharNu) => 'ヌ',
            nameof(kbdKatakanaCharNe) => 'ネ',
            nameof(kbdKatakanaCharNo) => 'ノ',
            nameof(kbdKatakanaCharHa) => 'ハ',
            nameof(kbdKatakanaCharHi) => 'ヒ',
            nameof(kbdKatakanaCharFu) => 'フ',
            nameof(kbdKatakanaCharHe) => 'ヘ',
            nameof(kbdKatakanaCharHo) => 'ホ',
            nameof(kbdKatakanaCharMa) => 'マ',
            nameof(kbdKatakanaCharMi) => 'ミ',
            nameof(kbdKatakanaCharMu) => 'ム',
            nameof(kbdKatakanaCharMe) => 'メ',
            nameof(kbdKatakanaCharMo) => 'モ',
            nameof(kbdKatakanaCharYa) => 'ヤ',
            nameof(kbdKatakanaCharYu) => 'ユ',
            nameof(kbdKatakanaCharYo) => 'ヨ',
            nameof(kbdKatakanaCharRa) => 'ラ',
            nameof(kbdKatakanaCharRi) => 'リ',
            nameof(kbdKatakanaCharRu) => 'ル',
            nameof(kbdKatakanaCharRe) => 'レ',
            nameof(kbdKatakanaCharRo) => 'ロ',
            nameof(kbdKatakanaCharWa) => 'ワ',
            nameof(kbdKatakanaCharWo) => 'ヲ',
            nameof(kbdKatakanaCharN) => 'ン',

            nameof(kbdKatakanaCharHyphen) => 'ー',

            nameof(kbdKatakanaCharGa) => 'ガ',
            nameof(kbdKatakanaCharGi) => 'ギ',
            nameof(kbdKatakanaCharGu) => 'グ',
            nameof(kbdKatakanaCharGe) => 'ゲ',
            nameof(kbdKatakanaCharGo) => 'ゴ',
            nameof(kbdKatakanaCharZa) => 'ザ',
            nameof(kbdKatakanaCharJi) => 'ジ',
            nameof(kbdKatakanaCharZu) => 'ズ',
            nameof(kbdKatakanaCharZe) => 'ゼ',
            nameof(kbdKatakanaCharZo) => 'ゾ',
            nameof(kbdKatakanaCharDa) => 'ダ',
            nameof(kbdKatakanaCharDi) => 'ヂ',
            nameof(kbdKatakanaCharDu) => 'ヅ',
            nameof(kbdKatakanaCharDe) => 'デ',
            nameof(kbdKatakanaCharDo) => 'ド',
            nameof(kbdKatakanaCharBa) => 'バ',
            nameof(kbdKatakanaCharBi) => 'ビ',
            nameof(kbdKatakanaCharBu) => 'ブ',
            nameof(kbdKatakanaCharBe) => 'ベ',
            nameof(kbdKatakanaCharBo) => 'ボ',
            nameof(kbdKatakanaCharPa) => 'パ',
            nameof(kbdKatakanaCharPi) => 'ピ',
            nameof(kbdKatakanaCharPu) => 'プ',
            nameof(kbdKatakanaCharPe) => 'ペ',
            nameof(kbdKatakanaCharPo) => 'ポ',

            nameof(kbdKatakanaCharSmallA) => 'ァ',
            nameof(kbdKatakanaCharSmallI) => 'ィ',
            nameof(kbdKatakanaCharSmallU) => 'ゥ',
            nameof(kbdKatakanaCharSmallE) => 'ェ',
            nameof(kbdKatakanaCharSmallO) => 'ォ',
            nameof(kbdKatakanaCharSmallYa) => 'ャ',
            nameof(kbdKatakanaCharSmallYu) => 'ュ',
            nameof(kbdKatakanaCharSmallYo) => 'ョ',
            nameof(kbdKatakanaCharSmallTsu) => 'ッ',

            nameof(kbdRomajiCharA) => 'A',
            nameof(kbdRomajiCharB) => 'B',
            nameof(kbdRomajiCharC) => 'C',
            nameof(kbdRomajiCharD) => 'D',
            nameof(kbdRomajiCharE) => 'E',
            nameof(kbdRomajiCharF) => 'F',
            nameof(kbdRomajiCharG) => 'G',
            nameof(kbdRomajiCharH) => 'H',
            nameof(kbdRomajiCharI) => 'I',
            nameof(kbdRomajiCharJ) => 'J',
            nameof(kbdRomajiCharK) => 'K',
            nameof(kbdRomajiCharL) => 'L',
            nameof(kbdRomajiCharM) => 'M',
            nameof(kbdRomajiCharN) => 'N',
            nameof(kbdRomajiCharO) => 'O',
            nameof(kbdRomajiCharP) => 'P',
            nameof(kbdRomajiCharQ) => 'Q',
            nameof(kbdRomajiCharR) => 'R',
            nameof(kbdRomajiCharS) => 'S',
            nameof(kbdRomajiCharT) => 'T',
            nameof(kbdRomajiCharU) => 'U',
            nameof(kbdRomajiCharV) => 'V',
            nameof(kbdRomajiCharW) => 'W',
            nameof(kbdRomajiCharX) => 'X',
            nameof(kbdRomajiCharY) => 'Y',
            nameof(kbdRomajiCharZ) => 'Z',

            nameof(kbdRomajiCharHyphen) => 'ー',
            nameof(kbdRomajiCharTilde) => '~',

            nameof(kbdJPSpace) => '　',

            _ => null
        };
        if (charToType is not null)
        {
            TypeChar(charToType.Value);
        }
    }

    private void kbdJPEnd_Click(object sender, EventArgs e)
    {
        UpdatePlayerName();
        autoClose = true;
        Close();
    }
}
