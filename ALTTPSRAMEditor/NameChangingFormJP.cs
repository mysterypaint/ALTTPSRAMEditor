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
        private String currName;
        private UInt16[] currNameRaw;
        private Dictionary<UInt16, char> rawJPChar;
        private bool saveChanges;
        Form1 form1;

        public NameChangingFormJP(Form1 _form1)
        {
            InitializeComponent();
            form1 = _form1;
            rawJPChar = form1.GetRawJPChar();
            currName = form1.GetPlayerName();
            currNameRaw = new UInt16[6];
            saveChanges = true;
            currName = "どしよう";
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
            jp_fnt = new Bitmap(ALTTPSRAMEditor.Properties.Resources.jpn_font);
            saveChanges = true;
            //int tileID = 2;

            Console.WriteLine(currName);
            //kbdENCharA.Image = GetCharTexture(jp_fnt, 1);
            //kbdENCharB.Image = GetCharTexture(jp_fnt, 2);
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
        

        private static Image GetCharTexture(Bitmap en_fnt, int tileID)
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

        private void UpdatePlayerName()
        {
            // Update Form1 with the changed player name
            // If the name is too short, fill it with spaces
            for (int k = currName.Length; k < 4; k++)
                currName += " ";

            currName = currName.Substring(0,4); // Chop off anything longer than 4 chars
            form1.SetPlayerName(currName);
            int i = 0;
            foreach (char c in currName)
            {
                currNameRaw[i] = rawJPChar.FirstOrDefault(x => x.Value == c).Key;
                i++;
            }
            form1.SetPlayerNameRaw(currNameRaw);
            form1.UpdatePlayerName(currName);
            form1.Refresh();
        }

        private void NameChangingFormJP_Load(object sender, EventArgs e)
        {

        }

        private void NameChangingFormJP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saveChanges)
                UpdatePlayerName();
        }
    }
}
