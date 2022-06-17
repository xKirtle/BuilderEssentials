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
    public static Color SelectedColor = default;
    public static Color Blue = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
    public static Color Yellow = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f;
    public static Color Red = new Color(1f, 0f, 0f, .75f) * 0.8f;
    
    
    private static Asset<Texture2D> PixelTexture = TextureAssets.Extra[2];
    private static Rectangle Rectangle = new Rectangle(0, 0, 16, 16);
    
    public static void PlotPixel(int tileX, int tileY) {
        SelectedColor = Blue;
        Vector2 position = new Vector2(tileX, tileY) * 16 - Main.screenPosition;
        Main.spriteBatch.Draw(PixelTexture.Value, position, Rectangle, 
            SelectedColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }

    public static void PlotLine(Vector2 start, Vector2 end)
        => PlotLine(start.X, start.Y, end.X, end.Y);
    public static void PlotLine(float x0, float y0, float x1, float y1) 
        => PlotLine((int) x0, (int) y0, (int) x1, (int) y1);
    public static void PlotLine(int x0, int y0, int x1, int y1) {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;
        while (true) {
            PlotPixel(x0, y0);
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
}