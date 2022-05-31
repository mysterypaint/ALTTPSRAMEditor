namespace ALTTPSRAMEditor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class ImageHelper
{
    public static Image GetCharTexture(Bitmap fnt, int tileId, SaveRegion saveRegion = SaveRegion.JPN, bool hugLeft = false)
    {
        var tileset_width = saveRegion switch
        {
            SaveRegion.JPN => 20, // Japanese Font
            SaveRegion.USA => 27, // English Font
            _ => 27,
        };
        var tile_w = 8;
        var tile_h = 16;
        var x = tileId % tileset_width * tile_w;
        var y = tileId / tileset_width * tile_h;
        var width = 8;
        var height = 16;
        var scale = 2;
        var crop = new Rectangle(x, y, width * scale, height * scale);
        var tex = new Bitmap(crop.Width, crop.Height);

        using (var gr = Graphics.FromImage(tex))
        {
            gr.InterpolationMode = InterpolationMode.NearestNeighbor;
            gr.PixelOffsetMode = PixelOffsetMode.Half;
            gr.DrawImage(fnt, new Rectangle(0, 0, tex.Width * scale, tex.Height * scale), crop, GraphicsUnit.Pixel);
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
}
