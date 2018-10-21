using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALTTPSRAMEditor
{
    public partial class NameChangingFormJP : Form
    {
        private Bitmap jp_fnt;
        private StringBuilder currName;
        private UInt16[] currNameRaw;
        private Dictionary<char, int> jpChar;
        private Dictionary<UInt16, char> rawJPChar;
        private int charPos = 0;
        private bool autoClose;
        Form1 form1;

        public NameChangingFormJP(Form1 _form1)
        {
            InitializeComponent();
            form1 = _form1;
            jpChar = form1.GetJPChar();
            rawJPChar = form1.GetRawJPChar();
            autoClose = false;
            currName = new StringBuilder(form1.GetPlayerName().Substring(0, 4));
            currNameRaw = new UInt16[6];
        }

        private void NameChangingFormJP_KeyDown(object sender, KeyEventArgs e)
        {
            // Close the Name Changing form if we hit Escape
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void NameChangingFormJP_Load(object sender, EventArgs e)
        {

            // Name Changing Form Initialization
            jp_fnt = new Bitmap(ALTTPSRAMEditor.Properties.Resources.jpn_font);

            // Draw the name to the screen
            UpdateDisplayName();

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

            kbdJPMoveLeft.Image = GetCharTexture(jp_fnt, 90, false);
            kbdJPMoveRight.Image = GetCharTexture(jp_fnt, 91, false);
            kbdJPEnd.Image = ALTTPSRAMEditor.Properties.Resources.jpn_font_end;
            Refresh();
        }

        private void UpdateDisplayName()
        {
            pictureJPNameChar0.Image = GetCharTexture(jp_fnt, jpChar[currName[0]], false);
            pictureJPNameChar1.Image = GetCharTexture(jp_fnt, jpChar[currName[1]], false);
            pictureJPNameChar2.Image = GetCharTexture(jp_fnt, jpChar[currName[2]], false);
            pictureJPNameChar3.Image = GetCharTexture(jp_fnt, jpChar[currName[3]], false);

            pictureJPCharHeart.Location = new Point(880 + (charPos * 32), 38);
        }

        private static Image GetCharTexture(Bitmap jp_fnt, int tileID, bool hugLeft)
        {
            int tileset_width = 20; // Japanese Font
            int tile_w = 8;
            int tile_h = 16;
            int x = (tileID % tileset_width) * tile_w;
            int y = (tileID / tileset_width) * tile_h;
            int width = 8;
            int height = 16;
            int scale = 2;
            Rectangle crop = new Rectangle(x, y, width * scale, height * scale);
            var tex = new Bitmap(crop.Width, crop.Height);

            using (var gr = Graphics.FromImage(tex))
            {
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                gr.DrawImage(jp_fnt, new Rectangle(0, 0, tex.Width * scale, tex.Height * scale), crop, GraphicsUnit.Pixel);
            }

            if (hugLeft)
                return tex;

            var bmp = new Bitmap(crop.Width * 2, crop.Height);

            using (var gr = Graphics.FromImage(bmp))
            {
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
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
                charPos = 0;

            // Draw the name to the screen
            UpdateDisplayName();
        }

        private void UpdatePlayerName()
        {
            // Update Form1 with the changed player name
            // If the name is too short, fill it with spaces
            for (int k = currName.Length; k < 4; k++)
                currName[k] = ' ';

            form1.SetPlayerName(currName.ToString());
            int j = 0;
            for (int i = 0; i < currName.Length; i++)
            {
                currNameRaw[i] = rawJPChar.FirstOrDefault(x => x.Value == currName[i]).Key;
                j++;
            }
            form1.SetPlayerNameRaw(currNameRaw);
            form1.UpdatePlayerName();
        }

        private void NameChangingFormJP_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            if (!autoClose)
            {
                DialogResult dialogSave = MessageBox.Show("Would you like to save your changes?", "Save Changes?", MessageBoxButtons.YesNo);
                if (dialogSave == DialogResult.Yes)
                {
                    UpdatePlayerName();
                }
                else
                {
                    DialogResult dialogCloseConfirm = MessageBox.Show("Continue editing?", "Closing Name Changing Form (JPN)", MessageBoxButtons.YesNo);
                    if (dialogCloseConfirm == DialogResult.Yes)
                        e.Cancel = true;
                }
            }*/
        }

        private void kbdJPMoveLeft_Click(object sender, EventArgs e)
        {
            charPos--;
            if (charPos < 0)
                charPos = 3;
            pictureJPCharHeart.Location = new Point(880 + (charPos * 32), 38);
        }

        private void kbdJPMoveRight_Click(object sender, EventArgs e)
        {
            charPos++;
            if (charPos > 3)
                charPos = 0;
            pictureJPCharHeart.Location = new Point(880 + (charPos * 32), 38);
        }

        private void kbdJPEnd_Click(object sender, EventArgs e)
        {
            UpdatePlayerName();
            autoClose = true;
            Close();
        }

        private void kbdHiraganaCharA_Click(object sender, EventArgs e)
        {
            TypeChar('あ');
        }

        private void kbdHiraganaCharI_Click(object sender, EventArgs e)
        {
            TypeChar('あ');
        }

        private void kbdHiraganaCharU_Click(object sender, EventArgs e)
        {
            TypeChar('あ');
        }
    }
}
