using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public static class ShapeHelpers
{
    public static Color SelectedColor = Blue;
    public static Color Blue = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
    public static Color Yellow = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f;
    public static Color Red = new Color(1f, 0f, 0f, .75f) * 0.8f;
    
    
    private static Asset<Texture2D> PixelTexture = TextureAssets.Extra[2];
    private static Rectangle Rectangle = new Rectangle(0, 0, 16, 16);
    
    public static void PlotPixel(int tileX, int tileY, Color color, float scale = 1f) {
        Vector2 position = Main.ReverseGravitySupport(new Vector2(tileX, tileY) * 16 - Main.screenPosition);
        position += new Vector2(8 - 8 * scale); //Make it not fix on the top left corner
        Main.spriteBatch.Draw(PixelTexture.Value, position, Rectangle, 
            color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    public static void PlotLine(Vector2 start, Vector2 end, Color color, float scale = 1f)
        => PlotLine(start.X, start.Y, end.X, end.Y, color, scale);
    public static void PlotLine(float x0, float y0, float x1, float y1, Color color, float scale = 1f) 
        => PlotLine((int) x0, (int) y0, (int) x1, (int) y1, color, scale);
    public static void PlotLine(int x0, int y0, int x1, int y1, Color color, float scale = 1f) {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;
        while (true) {
            PlotPixel(x0, y0, color, scale);
            e2 = 2 * err;
            if (e2 >= dy) {
                if (x0 == x1) break;
                err += dy;
                x0 += sx;
            }

            if (e2 <= dx) {
                if (y0 == y1) break;
                err += dx;
                y0 += sy;
            }
        }
    }

    public static void PlotRectangle(Vector2 start, Vector2 end, Color color, float scale = 1f, bool isFill = false)
        => PlotRectangle(start.X, start.Y, end.X, end.Y, color, scale, isFill);

    public static void PlotRectangle(float x0, float y0, float x1, float y1, Color color, float scale = 1f, bool isFill = false)
        => PlotRectangle((int) x0, (int) y0, (int) x1, (int) y1, color, scale, isFill);

    public static void PlotRectangle(int x0, int y0, int x1, int y1, Color color, float scale = 1f, bool isFill = false) {
        int dx = Math.Abs(x0 - x1);
        int dy = Math.Abs(y0 - y1);
        int minX = Math.Min(x0, x1);
        int minY = Math.Min(y0, y1);

        if (dx == 0 && dy == 0) return;

        if ((dx == 0 && dy != 0) || (dx != 0 && dy == 0)) {
            PlotLine(x0, y0, x1, y1, color, scale);
            return;
        }

        for (int x = minX; x <= minX + dx; x++) {
            PlotPixel(x, minY, color, scale);
            PlotPixel(x, minY + dy, color, scale);
        }

        for (int y = minY + 1; y <= minY + dy - 1; y++) {
            PlotPixel(minX , y, color, scale);
            PlotPixel(minX + dx, y, color, scale);
        }

        if (isFill) {
            for (int x = minX + 1; x <= minX + dx - 1; x++)
            for (int y = minY + 1; y <= minY + dy - 1; y++)
                PlotPixel(x, y, color, scale);
        }

        //Middle helper lines
        Color tempColor = color;
        color *= 0.4f;

        //Vertical line
        if (dx % 2 == 0 && dx > 3) {
            int fixedX = minX + dx / 2;
            PlotLine(fixedX, minY, fixedX, minY + dy, color, scale);
        }
        
        //Horizontal line
        if (dy % 2 == 0 && dy > 3) {
            int fixedY = minY + dy / 2;
            PlotLine(minX, fixedY, minX + dx, fixedY, color, scale);
        }

        color = tempColor;
    }
}