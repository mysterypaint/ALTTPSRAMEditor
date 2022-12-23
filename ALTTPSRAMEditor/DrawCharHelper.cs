// ReSharper disable InconsistentNaming
namespace ALTTPSRAMEditor;

public static class DrawCharHelper
{
    public static Image GetCharTexture(Bitmap fnt, int tileId, SaveRegion saveRegion = SaveRegion.JPN, bool hugLeft = false)
    {
        var tileset_width = saveRegion switch
        {
            SaveRegion.JPN => 20, // Japanese Font
            SaveRegion.USA => 27, // English Font
            _ => 27,
        };
        const int tile_w = 8;
        const int tile_h = 16;
        var x = tileId % tileset_width * tile_w;
        var y = tileId / tileset_width * tile_h;
        const int width = 8;
        const int height = 16;
        const int scale = 2;
        var crop = new Rectangle(x, y, width * scale, height * scale);
        var tex = new Bitmap(crop.Width, crop.Height);

        using var charGr = Graphics.FromImage(tex);
        charGr.InterpolationMode = InterpolationMode.NearestNeighbor;
        charGr.PixelOffsetMode = PixelOffsetMode.Half;
        charGr.DrawImage(fnt, new Rectangle(0, 0, tex.Width * scale, tex.Height * scale), crop, GraphicsUnit.Pixel);

        if (hugLeft)
        {
            return tex;
        }

        var bmp = new Bitmap(crop.Width * 2, crop.Height);

        using var hugRightGr = Graphics.FromImage(bmp);
        hugRightGr.InterpolationMode = InterpolationMode.NearestNeighbor;
        hugRightGr.PixelOffsetMode = PixelOffsetMode.Half;
        hugRightGr.Clear(Color.Black);
        hugRightGr.DrawImage(tex, 8, 2);
        return bmp;
    }
}
