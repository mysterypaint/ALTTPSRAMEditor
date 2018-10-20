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
    public partial class NameChangingFormEN : Form
    {
        private Bitmap en_fnt;
        private StringBuilder currName;
        private UInt16[] currNameRaw;
        private Dictionary<UInt16, char> rawENChar;
        private bool saveChanges;
        private int charPos = 0;
        Form1 form1;

        public NameChangingFormEN(Form1 _form1)
        {
            InitializeComponent();
            form1 = _form1;
            rawENChar = form1.GetRawENChar();
            currName = new StringBuilder(form1.GetPlayerName().Substring(0,6));
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

        private void NameChangingForm_Load(object sender, EventArgs e)
        {
            // Name Changing Form Initialization
            en_fnt = new Bitmap(ALTTPSRAMEditor.Properties.Resources.en_font);
            saveChanges = false;

            //int tileID = 2;
            kbdENCharA.Image = GetCharTexture(en_fnt, 1);
            kbdENCharB.Image = GetCharTexture(en_fnt, 2);
            /*kbdENCharC.Image = GetCharTexture(en_fnt, 3);
            kbdENCharD.Image = GetCharTexture(en_fnt, 4);
            kbdENCharE.Image = GetCharTexture(en_fnt, 5);
            kbdENCharF.Image = GetCharTexture(en_fnt, 6);
            kbdENCharG.Image = GetCharTexture(en_fnt, 7);
            kbdENCharH.Image = GetCharTexture(en_fnt, 8);
            kbdENCharI.Image = GetCharTexture(en_fnt, 9);
            kbdENCharJ.Image = GetCharTexture(en_fnt, 10);
            kbdENCharK.Image = GetCharTexture(en_fnt, 11);
            kbdENCharL.Image = GetCharTexture(en_fnt, 12);
            kbdENCharM.Image = GetCharTexture(en_fnt, 13);
            kbdENCharN.Image = GetCharTexture(en_fnt, 14);
            kbdENCharO.Image = GetCharTexture(en_fnt, 15);
            kbdENCharP.Image = GetCharTexture(en_fnt, 16);
            kbdENCharQ.Image = GetCharTexture(en_fnt, 17);
            kbdENCharR.Image = GetCharTexture(en_fnt, 18);
            kbdENCharS.Image = GetCharTexture(en_fnt, 19);
            kbdENCharT.Image = GetCharTexture(en_fnt, 20);
            kbdENCharU.Image = GetCharTexture(en_fnt, 21);
            kbdENCharV.Image = GetCharTexture(en_fnt, 22);
            kbdENCharW.Image = GetCharTexture(en_fnt, 23);
            kbdENCharX.Image = GetCharTexture(en_fnt, 24);
            kbdENCharY.Image = GetCharTexture(en_fnt, 25);
            kbdENCharZ.Image = GetCharTexture(en_fnt, 26);*/
            Refresh();
        }

        private void NameChangingForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Close the Name Changing form if we hit Escape
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private static Image GetCharTexture(Bitmap en_fnt, int tileID)
        {
            int tileset_width = 27; // English Font
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
                gr.DrawImage(en_fnt, new Rectangle(0, 0, tex.Width * scale, tex.Height * scale), crop, GraphicsUnit.Pixel);
            }

            var bmp = new Bitmap(crop.Width * 2, crop.Height);

            using (var gr = Graphics.FromImage(bmp))
            {
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                gr.Clear(Color.Red);
                gr.DrawImage(tex, 8, 2);
            }
            return bmp;
        }

        private void kbdENCharA_Click(object sender, EventArgs e)
        {
            TypeChar('A');
        }

        private void NameChangingFormEN_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveChanges = true;
            if (saveChanges)
                UpdatePlayerName();
        }

        private void TypeChar(char c)
        {
            currName[charPos] = c;

            charPos++;
            if (charPos > 5)
                charPos = 0;
        }

        private void UpdatePlayerName()
        {
            // Update Form1 with the changed player name
            // If the name is too short, fill it with spaces
            for (int k = currName.Length; k < 6; k++)
                currName[k] = ' ';
            
            form1.SetPlayerName(currName.ToString());
            int j = 0;
            for (int i = 0; i < currName.Length; i++)
            {
                currNameRaw[i] = rawENChar.FirstOrDefault(x => x.Value == currName[i]).Key;
                Console.Write(currName[i] + ", "); Console.WriteLine(currNameRaw[i]);
                j++;
            }
            form1.SetPlayerNameRaw(currNameRaw);
            form1.UpdatePlayerName();
        }
    }
}