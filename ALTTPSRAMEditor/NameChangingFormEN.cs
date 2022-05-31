namespace ALTTPSRAMEditor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class NameChangingFormEn : Form
{
    private Bitmap enFnt = default!;
    private readonly StringBuilder currName;
    private readonly ushort[] currNameRaw;
    private readonly Dictionary<char, int> enChar;
    private readonly Dictionary<ushort, char> rawEnChar;
    private int charPos = 0;
    private bool autoClose;
    private readonly MainForm mainForm;

    public NameChangingFormEn(MainForm mainForm)
    {
        InitializeComponent();
        this.mainForm = mainForm;
        enChar = AppState.enChar;
        rawEnChar = AppState.rawEnChar;
        autoClose = false;
        currName = new StringBuilder(this.mainForm.GetPlayerName()[..6]);
        currNameRaw = new ushort[6];
    }

    private void NameChangingFormEn_KeyDown(object sender, KeyEventArgs e)
    {
        // Close the Name Changing form if we hit Escape
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    private void NameChangingFormEn_Load(object sender, EventArgs e)
    {
        // Name Changing Form Initialization
        enFnt = new Bitmap(Properties.Resources.en_font);

        // Draw the name to the screen
        UpdateDisplayName();

        kbdENCharA.Image = GetCharTexture(enFnt, 1, SaveRegion.USA);
        kbdENCharB.Image = GetCharTexture(enFnt, 2, SaveRegion.USA);
        kbdENCharC.Image = GetCharTexture(enFnt, 3, SaveRegion.USA);
        kbdENCharD.Image = GetCharTexture(enFnt, 4, SaveRegion.USA);
        kbdENCharE.Image = GetCharTexture(enFnt, 5, SaveRegion.USA);
        kbdENCharF.Image = GetCharTexture(enFnt, 6, SaveRegion.USA);
        kbdENCharG.Image = GetCharTexture(enFnt, 7, SaveRegion.USA);
        kbdENCharH.Image = GetCharTexture(enFnt, 8, SaveRegion.USA);
        kbdENCharI.Image = GetCharTexture(enFnt, 9, SaveRegion.USA);
        kbdENCharJ.Image = GetCharTexture(enFnt, 10, SaveRegion.USA);
        kbdENCharK.Image = GetCharTexture(enFnt, 11, SaveRegion.USA);
        kbdENCharL.Image = GetCharTexture(enFnt, 12, SaveRegion.USA);
        kbdENCharM.Image = GetCharTexture(enFnt, 13, SaveRegion.USA);
        kbdENCharN.Image = GetCharTexture(enFnt, 14, SaveRegion.USA);
        kbdENCharO.Image = GetCharTexture(enFnt, 15, SaveRegion.USA);
        kbdENCharP.Image = GetCharTexture(enFnt, 16, SaveRegion.USA);
        kbdENCharQ.Image = GetCharTexture(enFnt, 17, SaveRegion.USA);
        kbdENCharR.Image = GetCharTexture(enFnt, 18, SaveRegion.USA);
        kbdENCharS.Image = GetCharTexture(enFnt, 19, SaveRegion.USA);
        kbdENCharT.Image = GetCharTexture(enFnt, 20, SaveRegion.USA);
        kbdENCharU.Image = GetCharTexture(enFnt, 21, SaveRegion.USA);
        kbdENCharV.Image = GetCharTexture(enFnt, 22, SaveRegion.USA);
        kbdENCharW.Image = GetCharTexture(enFnt, 23, SaveRegion.USA);
        kbdENCharX.Image = GetCharTexture(enFnt, 24, SaveRegion.USA);
        kbdENCharY.Image = GetCharTexture(enFnt, 25, SaveRegion.USA);
        kbdENCharZ.Image = GetCharTexture(enFnt, 26, SaveRegion.USA);

        kbdENCharSmallA.Image = GetCharTexture(enFnt, 28, SaveRegion.USA);
        kbdENCharSmallB.Image = GetCharTexture(enFnt, 29, SaveRegion.USA);
        kbdENCharSmallC.Image = GetCharTexture(enFnt, 30, SaveRegion.USA);
        kbdENCharSmallD.Image = GetCharTexture(enFnt, 31, SaveRegion.USA);
        kbdENCharSmallE.Image = GetCharTexture(enFnt, 32, SaveRegion.USA);
        kbdENCharSmallF.Image = GetCharTexture(enFnt, 33, SaveRegion.USA);
        kbdENCharSmallG.Image = GetCharTexture(enFnt, 34, SaveRegion.USA);
        kbdENCharSmallH.Image = GetCharTexture(enFnt, 35, SaveRegion.USA);
        kbdENCharSmallI.Image = GetCharTexture(enFnt, 36, SaveRegion.USA);
        kbdENCharSmallJ.Image = GetCharTexture(enFnt, 37, SaveRegion.USA);
        kbdENCharSmallK.Image = GetCharTexture(enFnt, 38, SaveRegion.USA);
        kbdENCharSmallL.Image = GetCharTexture(enFnt, 39, SaveRegion.USA);
        kbdENCharSmallM.Image = GetCharTexture(enFnt, 40, SaveRegion.USA);
        kbdENCharSmallN.Image = GetCharTexture(enFnt, 41, SaveRegion.USA);
        kbdENCharSmallO.Image = GetCharTexture(enFnt, 42, SaveRegion.USA);
        kbdENCharSmallP.Image = GetCharTexture(enFnt, 43, SaveRegion.USA);
        kbdENCharSmallQ.Image = GetCharTexture(enFnt, 44, SaveRegion.USA);
        kbdENCharSmallR.Image = GetCharTexture(enFnt, 45, SaveRegion.USA);
        kbdENCharSmallS.Image = GetCharTexture(enFnt, 46, SaveRegion.USA);
        kbdENCharSmallT.Image = GetCharTexture(enFnt, 47, SaveRegion.USA);
        kbdENCharSmallU.Image = GetCharTexture(enFnt, 48, SaveRegion.USA);
        kbdENCharSmallV.Image = GetCharTexture(enFnt, 49, SaveRegion.USA);
        kbdENCharSmallW.Image = GetCharTexture(enFnt, 50, SaveRegion.USA);
        kbdENCharSmallX.Image = GetCharTexture(enFnt, 51, SaveRegion.USA);
        kbdENCharSmallY.Image = GetCharTexture(enFnt, 52, SaveRegion.USA);
        kbdENCharSmallZ.Image = GetCharTexture(enFnt, 53, SaveRegion.USA);

        kbdENCharHyphen.Image = GetCharTexture(enFnt, 54, SaveRegion.USA);
        kbdENCharPeriod.Image = GetCharTexture(enFnt, 55, SaveRegion.USA);
        kbdENCharComma.Image = GetCharTexture(enFnt, 56, SaveRegion.USA);

        kbdENChar0.Image = GetCharTexture(enFnt, 59, SaveRegion.USA);
        kbdENChar1.Image = GetCharTexture(enFnt, 60, SaveRegion.USA);
        kbdENChar2.Image = GetCharTexture(enFnt, 61, SaveRegion.USA);
        kbdENChar3.Image = GetCharTexture(enFnt, 62, SaveRegion.USA);
        kbdENChar4.Image = GetCharTexture(enFnt, 63, SaveRegion.USA);
        kbdENChar5.Image = GetCharTexture(enFnt, 64, SaveRegion.USA);
        kbdENChar6.Image = GetCharTexture(enFnt, 65, SaveRegion.USA);
        kbdENChar7.Image = GetCharTexture(enFnt, 66, SaveRegion.USA);
        kbdENChar8.Image = GetCharTexture(enFnt, 67, SaveRegion.USA);
        kbdENChar9.Image = GetCharTexture(enFnt, 68, SaveRegion.USA);
        kbdENCharExclamation.Image = GetCharTexture(enFnt, 69, SaveRegion.USA);
        kbdENCharQuestion.Image = GetCharTexture(enFnt, 70, SaveRegion.USA);
        kbdENCharParenthaseesLeft.Image = GetCharTexture(enFnt, 71, SaveRegion.USA);
        kbdENCharParenthaseesRight.Image = GetCharTexture(enFnt, 72, SaveRegion.USA);

        kbdENMoveLeft.Image = GetCharTexture(enFnt, 57, SaveRegion.USA);
        kbdENMoveRight.Image = GetCharTexture(enFnt, 58, SaveRegion.USA);
        Refresh();
    }

    private void UpdateDisplayName()
    {
        pictureENNameChar0.Image = GetCharTexture(enFnt, enChar[currName[0]], SaveRegion.USA, true);
        pictureENNameChar1.Image = GetCharTexture(enFnt, enChar[currName[1]], SaveRegion.USA, true);
        pictureENNameChar2.Image = GetCharTexture(enFnt, enChar[currName[2]], SaveRegion.USA, true);
        pictureENNameChar3.Image = GetCharTexture(enFnt, enChar[currName[3]], SaveRegion.USA, true);
        pictureENNameChar4.Image = GetCharTexture(enFnt, enChar[currName[4]], SaveRegion.USA, true);
        pictureENNameChar5.Image = GetCharTexture(enFnt, enChar[currName[5]], SaveRegion.USA, true);

        pictureENCharHeart.Location = new Point(62 + charPos * 32, 174);
    }

    private void TypeChar(char c)
    {
        currName[charPos] = c;

        charPos++;
        if (charPos > 5)
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
        for (var k = currName.Length; k < 6; k++)
        {
            currName[k] = ' ';
        }

        mainForm.SetPlayerName(currName.ToString());
        var j = 0;
        for (var i = 0; i < currName.Length; i++)
        {
            currNameRaw[i] = rawEnChar.FirstOrDefault(x => x.Value == currName[i]).Key;
            j++;
        }
        mainForm.SetPlayerNameRaw(currNameRaw);
        mainForm.UpdatePlayerName();
    }

    private void NameChangingFormEn_FormClosing(object sender, FormClosingEventArgs e)
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
            var dialogCloseConfirm = MessageBox.Show("Continue editing?", "Closing Name Changing Form (USA/EUR)", MessageBoxButtons.YesNo);
            if (dialogCloseConfirm == DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }

    private void kbdEnMoveLeft_Click(object sender, EventArgs e)
    {
        charPos--;
        if (charPos < 0)
        {
            charPos = 5;
        }

        pictureENCharHeart.Location = new Point(62 + charPos * 32, 174);
    }

    private void kbdEnMoveRight_Click(object sender, EventArgs e)
    {
        charPos++;
        if (charPos > 5)
        {
            charPos = 0;
        }

        pictureENCharHeart.Location = new Point(62 + charPos * 32, 174);
    }

    private void KbdEnChar_Click(object sender, EventArgs e)
    {
        char? charToType = (sender as PictureBox)?.Name switch
        {
            nameof(kbdENCharA) => 'A',
            nameof(kbdENCharB) => 'B',
            nameof(kbdENCharC) => 'C',
            nameof(kbdENCharD) => 'D',
            nameof(kbdENCharE) => 'E',
            nameof(kbdENCharF) => 'F',
            nameof(kbdENCharG) => 'G',
            nameof(kbdENCharH) => 'H',
            nameof(kbdENCharI) => 'I',
            nameof(kbdENCharJ) => 'J',
            nameof(kbdENCharK) => 'K',
            nameof(kbdENCharL) => 'L',
            nameof(kbdENCharM) => 'M',
            nameof(kbdENCharN) => 'N',
            nameof(kbdENCharO) => 'O',
            nameof(kbdENCharP) => 'P',
            nameof(kbdENCharQ) => 'Q',
            nameof(kbdENCharR) => 'R',
            nameof(kbdENCharS) => 'S',
            nameof(kbdENCharT) => 'T',
            nameof(kbdENCharU) => 'U',
            nameof(kbdENCharV) => 'V',
            nameof(kbdENCharW) => 'W',
            nameof(kbdENCharX) => 'X',
            nameof(kbdENCharY) => 'Y',
            nameof(kbdENCharZ) => 'Z',

            nameof(kbdENCharSmallA) => 'a',
            nameof(kbdENCharSmallB) => 'b',
            nameof(kbdENCharSmallC) => 'c',
            nameof(kbdENCharSmallD) => 'd',
            nameof(kbdENCharSmallE) => 'e',
            nameof(kbdENCharSmallF) => 'f',
            nameof(kbdENCharSmallG) => 'g',
            nameof(kbdENCharSmallH) => 'h',
            nameof(kbdENCharSmallI) => 'i',
            nameof(kbdENCharSmallJ) => 'j',
            nameof(kbdENCharSmallK) => 'k',
            nameof(kbdENCharSmallL) => 'l',
            nameof(kbdENCharSmallM) => 'm',
            nameof(kbdENCharSmallN) => 'n',
            nameof(kbdENCharSmallO) => 'o',
            nameof(kbdENCharSmallP) => 'p',
            nameof(kbdENCharSmallQ) => 'q',
            nameof(kbdENCharSmallR) => 'r',
            nameof(kbdENCharSmallS) => 's',
            nameof(kbdENCharSmallT) => 't',
            nameof(kbdENCharSmallU) => 'u',
            nameof(kbdENCharSmallV) => 'v',
            nameof(kbdENCharSmallW) => 'w',
            nameof(kbdENCharSmallX) => 'x',
            nameof(kbdENCharSmallY) => 'y',
            nameof(kbdENCharSmallZ) => 'z',

            nameof(kbdENChar0) => '0',
            nameof(kbdENChar1) => '1',
            nameof(kbdENChar2) => '2',
            nameof(kbdENChar3) => '3',
            nameof(kbdENChar4) => '4',
            nameof(kbdENChar5) => '5',
            nameof(kbdENChar6) => '6',
            nameof(kbdENChar7) => '7',
            nameof(kbdENChar8) => '8',
            nameof(kbdENChar9) => '9',

            nameof(kbdENCharHyphen) => '-',
            nameof(kbdENCharPeriod) => '.',
            nameof(kbdENCharComma) => ',',
            nameof(kbdENCharExclamation) => '!',
            nameof(kbdENCharQuestion) => '?',
            nameof(kbdENCharParenthaseesLeft) => '(',
            nameof(kbdENCharParenthaseesRight) => ')',
            nameof(kbdENSpace) => ' ',

            _ => null
        };
        if (charToType is not null)
        {
            TypeChar(charToType.Value);
        }
    }

    private void kbdEnEnd_Click(object sender, EventArgs e)
    {
        UpdatePlayerName();
        autoClose = true;
        Close();
    }
}
