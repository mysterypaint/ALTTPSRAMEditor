using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALTTPSRAMEditor
{
    public class Tile
    {
        private System.Drawing.Bitmap bitmap;

        public Tile(String filepath)
        {
            this.bitmap = new System.Drawing.Bitmap(filepath);
        }

        public System.Drawing.Bitmap getBitmap()
        {
            return bitmap;
        }
    }
}
