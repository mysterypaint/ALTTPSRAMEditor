// ReSharper disable LocalizableElement
using static ALTTPSRAMEditor.Properties.Resources;

namespace ALTTPSRAMEditor;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is a Windows Forms application."),
 SuppressMessage("Style", "IDE1006:Naming Styles")]
public partial class NameChangingFormJp : Form
{
    private Bitmap jpFnt = default!;
    private readonly StringBuilder currName;
    private readonly ushort[] currNameRaw;
    private readonly Dictionary<char, int> jpChar;
    private readonly Dictionary<ushort, char> rawJpChar;
    private int charPos;
    private bool autoClose;
    private readonly MainForm mainForm;

    public NameChangingFormJp(MainForm mainForm)
    {
        InitializeComponent();
        this.mainForm = mainForm;
        jpChar = mainForm.TextCharacterData.JpChar;
        rawJpChar = mainForm.TextCharacterData.RawJpChar;
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
        jpFnt = new Bitmap(jpn_font);

        // Draw the name to the screen
        UpdateDisplayName();

        // Hiragana
        kbdHiraganaCharA.Image = GetCharTexture(jpFnt, 05);
        kbdHiraganaCharI.Image = GetCharTexture(jpFnt, 06);
        kbdHiraganaCharU.Image = GetCharTexture(jpFnt, 07);
        kbdHiraganaCharE.Image = GetCharTexture(jpFnt, 08);
        kbdHiraganaCharO.Image = GetCharTexture(jpFnt, 09);

        kbdHiraganaCharKa.Image = GetCharTexture(jpFnt, 10);
        kbdHiraganaCharKi.Image = GetCharTexture(jpFnt, 11);
        kbdHiraganaCharKu.Image = GetCharTexture(jpFnt, 12);
        kbdHiraganaCharKe.Image = GetCharTexture(jpFnt, 13);
        kbdHiraganaCharKo.Image = GetCharTexture(jpFnt, 14);

        kbdHiraganaCharSa.Image = GetCharTexture(jpFnt, 15);
        kbdHiraganaCharShi.Image = GetCharTexture(jpFnt, 16);
        kbdHiraganaCharSu.Image = GetCharTexture(jpFnt, 17);
        kbdHiraganaCharSe.Image = GetCharTexture(jpFnt, 18);
        kbdHiraganaCharSo.Image = GetCharTexture(jpFnt, 19);

        kbdHiraganaCharTa.Image = GetCharTexture(jpFnt, 20);
        kbdHiraganaCharChi.Image = GetCharTexture(jpFnt, 21);
        kbdHiraganaCharTsu.Image = GetCharTexture(jpFnt, 22);
        kbdHiraganaCharTe.Image = GetCharTexture(jpFnt, 23);
        kbdHiraganaCharTo.Image = GetCharTexture(jpFnt, 24);

        kbdHiraganaCharNa.Image = GetCharTexture(jpFnt, 25);
        kbdHiraganaCharNi.Image = GetCharTexture(jpFnt, 26);
        kbdHiraganaCharNu.Image = GetCharTexture(jpFnt, 27);
        kbdHiraganaCharNe.Image = GetCharTexture(jpFnt, 28);
        kbdHiraganaCharNo.Image = GetCharTexture(jpFnt, 29);

        kbdHiraganaCharHa.Image = GetCharTexture(jpFnt, 30);
        kbdHiraganaCharHi.Image = GetCharTexture(jpFnt, 31);
        kbdHiraganaCharFu.Image = GetCharTexture(jpFnt, 32);
        kbdHiraganaCharHe.Image = GetCharTexture(jpFnt, 33);
        kbdHiraganaCharHo.Image = GetCharTexture(jpFnt, 34);

        kbdHiraganaCharMa.Image = GetCharTexture(jpFnt, 35);
        kbdHiraganaCharMi.Image = GetCharTexture(jpFnt, 36);
        kbdHiraganaCharMu.Image = GetCharTexture(jpFnt, 37);
        kbdHiraganaCharMe.Image = GetCharTexture(jpFnt, 38);
        kbdHiraganaCharMo.Image = GetCharTexture(jpFnt, 39);

        kbdHiraganaCharYa.Image = GetCharTexture(jpFnt, 40);
        kbdHiraganaCharYu.Image = GetCharTexture(jpFnt, 42);
        kbdHiraganaCharYo.Image = GetCharTexture(jpFnt, 44);

        kbdHiraganaCharRa.Image = GetCharTexture(jpFnt, 45);
        kbdHiraganaCharRi.Image = GetCharTexture(jpFnt, 46);
        kbdHiraganaCharRu.Image = GetCharTexture(jpFnt, 47);
        kbdHiraganaCharRe.Image = GetCharTexture(jpFnt, 48);
        kbdHiraganaCharRo.Image = GetCharTexture(jpFnt, 49);

        kbdHiraganaCharWa.Image = GetCharTexture(jpFnt, 50);
        kbdHiraganaCharWo.Image = GetCharTexture(jpFnt, 51);
        kbdHiraganaCharN.Image = GetCharTexture(jpFnt, 52);
        kbdHiraganaCharHyphen.Image = GetCharTexture(jpFnt, 54);

        kbdHiraganaCharGa.Image = GetCharTexture(jpFnt, 55);
        kbdHiraganaCharGi.Image = GetCharTexture(jpFnt, 56);
        kbdHiraganaCharGu.Image = GetCharTexture(jpFnt, 57);
        kbdHiraganaCharGe.Image = GetCharTexture(jpFnt, 58);
        kbdHiraganaCharGo.Image = GetCharTexture(jpFnt, 59);


        kbdHiraganaCharZa.Image = GetCharTexture(jpFnt, 60);
        kbdHiraganaCharJi.Image = GetCharTexture(jpFnt, 61);
        kbdHiraganaCharZu.Image = GetCharTexture(jpFnt, 62);
        kbdHiraganaCharZe.Image = GetCharTexture(jpFnt, 63);
        kbdHiraganaCharZo.Image = GetCharTexture(jpFnt, 64);

        kbdHiraganaCharDa.Image = GetCharTexture(jpFnt, 65);
        kbdHiraganaCharDi.Image = GetCharTexture(jpFnt, 66);
        kbdHiraganaCharDu.Image = GetCharTexture(jpFnt, 67);
        kbdHiraganaCharDe.Image = GetCharTexture(jpFnt, 68);
        kbdHiraganaCharDo.Image = GetCharTexture(jpFnt, 69);

        kbdHiraganaCharBa.Image = GetCharTexture(jpFnt, 70);
        kbdHiraganaCharBi.Image = GetCharTexture(jpFnt, 71);
        kbdHiraganaCharBu.Image = GetCharTexture(jpFnt, 72);
        kbdHiraganaCharBe.Image = GetCharTexture(jpFnt, 73);
        kbdHiraganaCharBo.Image = GetCharTexture(jpFnt, 74);

        kbdHiraganaCharPa.Image = GetCharTexture(jpFnt, 75);
        kbdHiraganaCharPi.Image = GetCharTexture(jpFnt, 76);
        kbdHiraganaCharPu.Image = GetCharTexture(jpFnt, 77);
        kbdHiraganaCharPe.Image = GetCharTexture(jpFnt, 78);
        kbdHiraganaCharPo.Image = GetCharTexture(jpFnt, 79);

        kbdHiraganaCharSmallA.Image = GetCharTexture(jpFnt, 80);
        kbdHiraganaCharSmallI.Image = GetCharTexture(jpFnt, 81);
        kbdHiraganaCharSmallU.Image = GetCharTexture(jpFnt, 82);
        kbdHiraganaCharSmallE.Image = GetCharTexture(jpFnt, 83);
        kbdHiraganaCharSmallO.Image = GetCharTexture(jpFnt, 84);

        kbdHiraganaCharSmallYa.Image = GetCharTexture(jpFnt, 85);
        kbdHiraganaCharSmallYu.Image = GetCharTexture(jpFnt, 86);
        kbdHiraganaCharSmallYo.Image = GetCharTexture(jpFnt, 87);
        kbdHiraganaCharSmallTsu.Image = GetCharTexture(jpFnt, 89);

        // Katakana

        kbdKatakanaCharA.Image = GetCharTexture(jpFnt, 95);
        kbdKatakanaCharI.Image = GetCharTexture(jpFnt, 96);
        kbdKatakanaCharU.Image = GetCharTexture(jpFnt, 97);
        kbdKatakanaCharE.Image = GetCharTexture(jpFnt, 98);
        kbdKatakanaCharO.Image = GetCharTexture(jpFnt, 99);

        kbdKatakanaCharKa.Image = GetCharTexture(jpFnt, 100);
        kbdKatakanaCharKi.Image = GetCharTexture(jpFnt, 101);
        kbdKatakanaCharKu.Image = GetCharTexture(jpFnt, 102);
        kbdKatakanaCharKe.Image = GetCharTexture(jpFnt, 103);
        kbdKatakanaCharKo.Image = GetCharTexture(jpFnt, 104);

        kbdKatakanaCharSa.Image = GetCharTexture(jpFnt, 105);
        kbdKatakanaCharShi.Image = GetCharTexture(jpFnt, 106);
        kbdKatakanaCharSu.Image = GetCharTexture(jpFnt, 107);
        kbdKatakanaCharSe.Image = GetCharTexture(jpFnt, 108);
        kbdKatakanaCharSo.Image = GetCharTexture(jpFnt, 109);

        kbdKatakanaCharTa.Image = GetCharTexture(jpFnt, 110);
        kbdKatakanaCharChi.Image = GetCharTexture(jpFnt, 111);
        kbdKatakanaCharTsu.Image = GetCharTexture(jpFnt, 112);
        kbdKatakanaCharTe.Image = GetCharTexture(jpFnt, 113);
        kbdKatakanaCharTo.Image = GetCharTexture(jpFnt, 114);

        kbdKatakanaCharNa.Image = GetCharTexture(jpFnt, 115);
        kbdKatakanaCharNi.Image = GetCharTexture(jpFnt, 116);
        kbdKatakanaCharNu.Image = GetCharTexture(jpFnt, 117);
        kbdKatakanaCharNe.Image = GetCharTexture(jpFnt, 118);
        kbdKatakanaCharNo.Image = GetCharTexture(jpFnt, 119);

        kbdKatakanaCharHa.Image = GetCharTexture(jpFnt, 120);
        kbdKatakanaCharHi.Image = GetCharTexture(jpFnt, 121);
        kbdKatakanaCharFu.Image = GetCharTexture(jpFnt, 122);
        kbdKatakanaCharHe.Image = GetCharTexture(jpFnt, 123);
        kbdKatakanaCharHo.Image = GetCharTexture(jpFnt, 124);

        kbdKatakanaCharMa.Image = GetCharTexture(jpFnt, 125);
        kbdKatakanaCharMi.Image = GetCharTexture(jpFnt, 126);
        kbdKatakanaCharMu.Image = GetCharTexture(jpFnt, 127);
        kbdKatakanaCharMe.Image = GetCharTexture(jpFnt, 128);
        kbdKatakanaCharMo.Image = GetCharTexture(jpFnt, 129);

        kbdKatakanaCharYa.Image = GetCharTexture(jpFnt, 130);
        kbdKatakanaCharYu.Image = GetCharTexture(jpFnt, 132);
        kbdKatakanaCharYo.Image = GetCharTexture(jpFnt, 134);

        kbdKatakanaCharRa.Image = GetCharTexture(jpFnt, 135);
        kbdKatakanaCharRi.Image = GetCharTexture(jpFnt, 136);
        kbdKatakanaCharRu.Image = GetCharTexture(jpFnt, 137);
        kbdKatakanaCharRe.Image = GetCharTexture(jpFnt, 138);
        kbdKatakanaCharRo.Image = GetCharTexture(jpFnt, 139);

        kbdKatakanaCharWa.Image = GetCharTexture(jpFnt, 140);
        kbdKatakanaCharWo.Image = GetCharTexture(jpFnt, 141);
        kbdKatakanaCharN.Image = GetCharTexture(jpFnt, 142);
        kbdKatakanaCharHyphen.Image = GetCharTexture(jpFnt, 144);

        kbdKatakanaCharGa.Image = GetCharTexture(jpFnt, 145);
        kbdKatakanaCharGi.Image = GetCharTexture(jpFnt, 146);
        kbdKatakanaCharGu.Image = GetCharTexture(jpFnt, 147);
        kbdKatakanaCharGe.Image = GetCharTexture(jpFnt, 148);
        kbdKatakanaCharGo.Image = GetCharTexture(jpFnt, 149);

        kbdKatakanaCharZa.Image = GetCharTexture(jpFnt, 150);
        kbdKatakanaCharJi.Image = GetCharTexture(jpFnt, 151);
        kbdKatakanaCharZu.Image = GetCharTexture(jpFnt, 152);
        kbdKatakanaCharZe.Image = GetCharTexture(jpFnt, 153);
        kbdKatakanaCharZo.Image = GetCharTexture(jpFnt, 154);

        kbdKatakanaCharDa.Image = GetCharTexture(jpFnt, 155);
        kbdKatakanaCharDi.Image = GetCharTexture(jpFnt, 156);
        kbdKatakanaCharDu.Image = GetCharTexture(jpFnt, 157);
        kbdKatakanaCharDe.Image = GetCharTexture(jpFnt, 158);
        kbdKatakanaCharDo.Image = GetCharTexture(jpFnt, 159);

        kbdKatakanaCharBa.Image = GetCharTexture(jpFnt, 160);
        kbdKatakanaCharBi.Image = GetCharTexture(jpFnt, 161);
        kbdKatakanaCharBu.Image = GetCharTexture(jpFnt, 162);
        kbdKatakanaCharBe.Image = GetCharTexture(jpFnt, 163);
        kbdKatakanaCharBo.Image = GetCharTexture(jpFnt, 164);

        kbdKatakanaCharPa.Image = GetCharTexture(jpFnt, 165);
        kbdKatakanaCharPi.Image = GetCharTexture(jpFnt, 166);
        kbdKatakanaCharPu.Image = GetCharTexture(jpFnt, 167);
        kbdKatakanaCharPe.Image = GetCharTexture(jpFnt, 168);
        kbdKatakanaCharPo.Image = GetCharTexture(jpFnt, 169);

        kbdKatakanaCharSmallA.Image = GetCharTexture(jpFnt, 170);
        kbdKatakanaCharSmallI.Image = GetCharTexture(jpFnt, 171);
        kbdKatakanaCharSmallU.Image = GetCharTexture(jpFnt, 172);
        kbdKatakanaCharSmallE.Image = GetCharTexture(jpFnt, 173);
        kbdKatakanaCharSmallO.Image = GetCharTexture(jpFnt, 174);

        kbdKatakanaCharSmallYa.Image = GetCharTexture(jpFnt, 175);
        kbdKatakanaCharSmallYu.Image = GetCharTexture(jpFnt, 176);
        kbdKatakanaCharSmallYo.Image = GetCharTexture(jpFnt, 177);
        kbdKatakanaCharSmallTsu.Image = GetCharTexture(jpFnt, 179);

        // Romaji

        kbdRomajiCharA.Image = GetCharTexture(jpFnt, 180);
        kbdRomajiCharB.Image = GetCharTexture(jpFnt, 181);
        kbdRomajiCharC.Image = GetCharTexture(jpFnt, 182);
        kbdRomajiCharD.Image = GetCharTexture(jpFnt, 183);
        kbdRomajiCharE.Image = GetCharTexture(jpFnt, 184);
        kbdRomajiCharF.Image = GetCharTexture(jpFnt, 185);
        kbdRomajiCharG.Image = GetCharTexture(jpFnt, 186);
        kbdRomajiCharH.Image = GetCharTexture(jpFnt, 187);
        kbdRomajiCharI.Image = GetCharTexture(jpFnt, 188);
        kbdRomajiCharJ.Image = GetCharTexture(jpFnt, 189);
        kbdRomajiCharK.Image = GetCharTexture(jpFnt, 190);
        kbdRomajiCharL.Image = GetCharTexture(jpFnt, 191);
        kbdRomajiCharM.Image = GetCharTexture(jpFnt, 192);
        kbdRomajiCharN.Image = GetCharTexture(jpFnt, 193);
        kbdRomajiCharO.Image = GetCharTexture(jpFnt, 194);
        kbdRomajiCharP.Image = GetCharTexture(jpFnt, 195);
        kbdRomajiCharQ.Image = GetCharTexture(jpFnt, 196);
        kbdRomajiCharR.Image = GetCharTexture(jpFnt, 197);
        kbdRomajiCharS.Image = GetCharTexture(jpFnt, 198);
        kbdRomajiCharT.Image = GetCharTexture(jpFnt, 199);
        kbdRomajiCharU.Image = GetCharTexture(jpFnt, 200);
        kbdRomajiCharV.Image = GetCharTexture(jpFnt, 201);
        kbdRomajiCharW.Image = GetCharTexture(jpFnt, 202);
        kbdRomajiCharX.Image = GetCharTexture(jpFnt, 203);
        kbdRomajiCharY.Image = GetCharTexture(jpFnt, 204);
        kbdRomajiCharZ.Image = GetCharTexture(jpFnt, 205);

        kbdRomajiCharHyphen.Image = GetCharTexture(jpFnt, 206);
        kbdRomajiCharTilde.Image = GetCharTexture(jpFnt, 207);

        kbdJPMoveLeft.Image = GetCharTexture(jpFnt, 90);
        kbdJPMoveRight.Image = GetCharTexture(jpFnt, 91);
        Refresh();
    }

    private void UpdateDisplayName()
    {
        pictureJPNameChar0.Image = GetCharTexture(jpFnt, jpChar[currName[0]]);
        pictureJPNameChar1.Image = GetCharTexture(jpFnt, jpChar[currName[1]]);
        pictureJPNameChar2.Image = GetCharTexture(jpFnt, jpChar[currName[2]]);
        pictureJPNameChar3.Image = GetCharTexture(jpFnt, jpChar[currName[3]]);

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
        for (var i = 0; i < currName.Length; i++)
        {
            currNameRaw[i] = rawJpChar.FirstOrDefault(x => x.Value == currName[i]).Key;
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
        var dialogSave = MessageBox.Show(
            "Would you like to save your changes?",
            "Save Changes?",
            MessageBoxButtons.YesNo);
        if (dialogSave == DialogResult.Yes)
        {
            UpdatePlayerName();
        }
        else
        {
            var dialogCloseConfirm = MessageBox.Show(
                "Continue editing?",
                "Closing Name Changing Form (JPN)",
                MessageBoxButtons.YesNo);
            if (dialogCloseConfirm == DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }

    private void kbdJpMoveLeft_Click(object sender, EventArgs e)
    {
        charPos--;
        if (charPos < 0)
        {
            charPos = 3;
        }

        pictureJPCharHeart.Location = new Point(880 + charPos * 32, 36);
    }

    private void kbdJpMoveRight_Click(object sender, EventArgs e)
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

    private void kbdJpEnd_Click(object sender, EventArgs e)
    {
        UpdatePlayerName();
        autoClose = true;
        Close();
    }
}
