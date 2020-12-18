using System;
using BuilderEssentials.UI.Elements;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawerUI
{
    public class BaseShape : CustomUIElement
    {
        public Color color;
        public Color Blue = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
        public Color Yellow = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f;
        public Color Red = new Color(1f, 0f, 0f, .75f) * 0.8f;

        public virtual void DrawRectangle(int x, int y)
        {
            Texture2D texture = Main.extraTexture[2];
            Rectangle value = new Rectangle(0, 0, 16, 16);
            Vector2 position = new Vector2(x, y) * 16 - Main.screenPosition;

            Main.spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void PlotLine(int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;
            for (;;)
            {
                DrawRectangle(x0, y0);
                e2 = 2 * err;
                if (e2 >= dy)
                {
                    if (x0 == x1) break;
                    err += dy;
                    x0 += sx;
                }

                if (e2 <= dx)
                {
                    if (y0 == y1) break;
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public virtual void PlotRectangle(int x0, int y0, int x1, int y1)
        {
            if (x0 == x1 && y0 == y1)
                return;
            else if (x0 == x1 || y0 == y1)
                PlotLine(x0, y0, x1, y1);
            else
            {
                int direction = y0 < y1 ? 1 : -1;
                PlotLine(x0, y0, x1, y0); // top
                PlotLine(x0, y1, x1, y1); // bottom
                PlotLine(x0, y0 + direction, x0, y1 - direction); //left
                PlotLine(x1, y0 + direction, x1, y1 - direction); //right
            }
        }

        public int SelectedQuarter(int x0, int y0, int x1, int y1)
        {
            //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
            int selectedQuarter = -1;

            if (x0 <= x1 && y0 <= y1)
                selectedQuarter = 3;
            else if (x0 <= x1 && y0 >= y1)
                selectedQuarter = 1;
            else if (x0 >= x1 && y0 >= y1)
                selectedQuarter = 0;
            else if (x0 >= x1 && y0 <= y1)
                selectedQuarter = 2;

            return selectedQuarter;
        }
    }
}