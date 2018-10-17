using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALTTPSRAMEditor
{
    class Tilemap
    {
        Tile[] tiles;
        byte[] tiledata;

        public Tilemap(Tile[] tiles, byte[] tiledata)
        {
            this.tiles = tiles;
            this.tiledata = tiledata;
        }

        public void Draw(System.Drawing.Graphics g)
        {
            foreach (byte tile in tiledata)
            {
                g.DrawImage(tiles[tile - 1].getBitmap(), 24, 24);
            }
        }
    }
}