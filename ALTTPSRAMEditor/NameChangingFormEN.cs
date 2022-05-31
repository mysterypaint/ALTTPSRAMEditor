namespace ALTTPSRAMEditor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class NameChangingFormEN : Form
{
    private Bitmap en_fnt = default!;
    private readonly StringBuilder currName;
    private readonly ushort[] currNameRaw;
    private readonly Dictionary<char, int> enChar;
    private readonly Dictionary<ushort, char> rawENChar;
    private int charPos = 0;
    private bool autoClose;
    private readonly MainForm mainForm;

    public NameChangingFormEN(MainForm _mainForm)
    {
        InitializeComponent();
        mainForm = _mainForm;
        enChar = MainForm.GetENChar();
        rawENChar = MainForm.GetRawENChar();
        autoClose = false;
        currName = new StringBuilder(mainForm.GetPlayerName()[..6]);
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

    private void NameChangingForm_Load(object sender, EventArgs e)
    {
        // Name Changing Form Initialization
        en_fnt = new Bitmap(Properties.Resources.en_font);

        // Draw the name to the screen
        UpdateDisplayName();

        kbdENCharA.Image = GetCharTexture(en_fnt, 1, false);
        kbdENCharB.Image = GetCharTexture(en_fnt, 2, false);
        kbdENCharC.Image = GetCharTexture(en_fnt, 3, false);
        kbdENCharD.Image = GetCharTexture(en_fnt, 4, false);
        kbdENCharE.Image = GetCharTexture(en_fnt, 5, false);
        kbdENCharF.Image = GetCharTexture(en_fnt, 6, false);
        kbdENCharG.Image = GetCharTexture(en_fnt, 7, false);
        kbdENCharH.Image = GetCharTexture(en_fnt, 8, false);
        kbdENCharI.Image = GetCharTexture(en_fnt, 9, false);
        kbdENCharJ.Image = GetCharTexture(en_fnt, 10, false);
        kbdENCharK.Image = GetCharTexture(en_fnt, 11, false);
        kbdENCharL.Image = GetCharTexture(en_fnt, 12, false);
        kbdENCharM.Image = GetCharTexture(en_fnt, 13, false);
        kbdENCharN.Image = GetCharTexture(en_fnt, 14, false);
        kbdENCharO.Image = GetCharTexture(en_fnt, 15, false);
        kbdENCharP.Image = GetCharTexture(en_fnt, 16, false);
        kbdENCharQ.Image = GetCharTexture(en_fnt, 17, false);
        kbdENCharR.Image = GetCharTexture(en_fnt, 18, false);
        kbdENCharS.Image = GetCharTexture(en_fnt, 19, false);
        kbdENCharT.Image = GetCharTexture(en_fnt, 20, false);
        kbdENCharU.Image = GetCharTexture(en_fnt, 21, false);
        kbdENCharV.Image = GetCharTexture(en_fnt, 22, false);
        kbdENCharW.Image = GetCharTexture(en_fnt, 23, false);
        kbdENCharX.Image = GetCharTexture(en_fnt, 24, false);
        kbdENCharY.Image = GetCharTexture(en_fnt, 25, false);
        kbdENCharZ.Image = GetCharTexture(en_fnt, 26, false);

        kbdENCharSmallA.Image = GetCharTexture(en_fnt, 28, false);
        kbdENCharSmallB.Image = GetCharTexture(en_fnt, 29, false);
        kbdENCharSmallC.Image = GetCharTexture(en_fnt, 30, false);
        kbdENCharSmallD.Image = GetCharTexture(en_fnt, 31, false);
        kbdENCharSmallE.Image = GetCharTexture(en_fnt, 32, false);
        kbdENCharSmallF.Image = GetCharTexture(en_fnt, 33, false);
        kbdENCharSmallG.Image = GetCharTexture(en_fnt, 34, false);
        kbdENCharSmallH.Image = GetCharTexture(en_fnt, 35, false);
        kbdENCharSmallI.Image = GetCharTexture(en_fnt, 36, false);
        kbdENCharSmallJ.Image = GetCharTexture(en_fnt, 37, false);
        kbdENCharSmallK.Image = GetCharTexture(en_fnt, 38, false);
        kbdENCharSmallL.Image = GetCharTexture(en_fnt, 39, false);
        kbdENCharSmallM.Image = GetCharTexture(en_fnt, 40, false);
        kbdENCharSmallN.Image = GetCharTexture(en_fnt, 41, false);
        kbdENCharSmallO.Image = GetCharTexture(en_fnt, 42, false);
        kbdENCharSmallP.Image = GetCharTexture(en_fnt, 43, false);
        kbdENCharSmallQ.Image = GetCharTexture(en_fnt, 44, false);
        kbdENCharSmallR.Image = GetCharTexture(en_fnt, 45, false);
        kbdENCharSmallS.Image = GetCharTexture(en_fnt, 46, false);
        kbdENCharSmallT.Image = GetCharTexture(en_fnt, 47, false);
        kbdENCharSmallU.Image = GetCharTexture(en_fnt, 48, false);
        kbdENCharSmallV.Image = GetCharTexture(en_fnt, 49, false);
        kbdENCharSmallW.Image = GetCharTexture(en_fnt, 50, false);
        kbdENCharSmallX.Image = GetCharTexture(en_fnt, 51, false);
        kbdENCharSmallY.Image = GetCharTexture(en_fnt, 52, false);
        kbdENCharSmallZ.Image = GetCharTexture(en_fnt, 53, false);

        kbdENCharHyphen.Image = GetCharTexture(en_fnt, 54, false);
        kbdENCharPeriod.Image = GetCharTexture(en_fnt, 55, false);
        kbdENCharComma.Image = GetCharTexture(en_fnt, 56, false);

        kbdENChar0.Image = GetCharTexture(en_fnt, 59, false);
        kbdENChar1.Image = GetCharTexture(en_fnt, 60, false);
        kbdENChar2.Image = GetCharTexture(en_fnt, 61, false);
        kbdENChar3.Image = GetCharTexture(en_fnt, 62, false);
        kbdENChar4.Image = GetCharTexture(en_fnt, 63, false);
        kbdENChar5.Image = GetCharTexture(en_fnt, 64, false);
        kbdENChar6.Image = GetCharTexture(en_fnt, 65, false);
        kbdENChar7.Image = GetCharTexture(en_fnt, 66, false);
        kbdENChar8.Image = GetCharTexture(en_fnt, 67, false);
        kbdENChar9.Image = GetCharTexture(en_fnt, 68, false);
        kbdENCharExclamation.Image = GetCharTexture(en_fnt, 69, false);
        kbdENCharQuestion.Image = GetCharTexture(en_fnt, 70, false);
        kbdENCharParenthaseesLeft.Image = GetCharTexture(en_fnt, 71, false);
        kbdENCharParenthaseesRight.Image = GetCharTexture(en_fnt, 72, false);

        kbdENMoveLeft.Image = GetCharTexture(en_fnt, 57, false);
        kbdENMoveRight.Image = GetCharTexture(en_fnt, 58, false);
        Refresh();
    }

    private void UpdateDisplayName()
    {
        pictureENNameChar0.Image = GetCharTexture(en_fnt, enChar[currName[0]], true);
        pictureENNameChar1.Image = GetCharTexture(en_fnt, enChar[currName[1]], true);
        pictureENNameChar2.Image = GetCharTexture(en_fnt, enChar[currName[2]], true);
        pictureENNameChar3.Image = GetCharTexture(en_fnt, enChar[currName[3]], true);
        pictureENNameChar4.Image = GetCharTexture(en_fnt, enChar[currName[4]], true);
        pictureENNameChar5.Image = GetCharTexture(en_fnt, enChar[currName[5]], true);

        pictureENCharHeart.Location = new Point(62 + charPos * 32, 174);
    }

    private void NameChangingForm_KeyDown(object sender, KeyEventArgs e)
    {
        // Close the Name Changing form if we hit Escape
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    private static Image GetCharTexture(Bitmap en_fnt, int tileID, bool hugLeft)
    {
        var tileset_width = 27; // English Font
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
            gr.DrawImage(en_fnt, new Rectangle(0, 0, tex.Width * scale, tex.Height * scale), crop, GraphicsUnit.Pixel);
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
            currNameRaw[i] = rawENChar.FirstOrDefault(x => x.Value == currName[i]).Key;
            j++;
        }
        mainForm.SetPlayerNameRaw(currNameRaw);
        mainForm.UpdatePlayerName();
    }

    private void kbdENMoveLeft_Click(object sender, EventArgs e)
    {
        charPos--;
        if (charPos < 0)
        {
            charPos = 5;
        }

        pictureENCharHeart.Location = new Point(62 + charPos * 32, 174);
    }

    private void kbdENMoveRight_Click(object sender, EventArgs e)
    {
        charPos++;
        if (charPos > 5)
        {
            charPos = 0;
        }

        pictureENCharHeart.Location = new Point(62 + charPos * 32, 174);
    }

    private void KbdEnCharClick(object sender, EventArgs e)
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

    private void kbdENEnd_Click(object sender, EventArgs e)
    {
        UpdatePlayerName();
        autoClose = true;
        Close();
    }

    private void NameChangingFormEN_FormClosing(object sender, FormClosingEventArgs e)
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
}
