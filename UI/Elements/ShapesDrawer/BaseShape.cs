using System;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class BaseShape : CustomUIElement
    {
        internal CoordsSelection cs;
        internal Func<int, bool> CanPlaceTiles;
        internal bool[] selected;
        internal Color color = default;
        internal Color Blue = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
        internal Color Yellow = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f;
        internal Color Red = new Color(1f, 0f, 0f, .75f) * 0.8f;
        internal int itemToWorkWith;

        public BaseShape(int itemType, UIState uiState)
        {
            itemToWorkWith = itemType;
            cs = new CoordsSelection(itemType, uiState);
        }

        internal virtual void PlotPixel(int x, int y, bool placeItem = false, int itemType = -1)
        {
            Asset<Texture2D> texture = TextureAssets.Extra[2];
            Rectangle value = new Rectangle(0, 0, 16, 16);
            Vector2 position = Main.ReverseGravitySupport(new Vector2(x, y) * 16f - Main.screenPosition, 16f);
            
            Main.spriteBatch.Draw(texture.Value, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        internal void PlotLine(int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;
            for (;;)
            {
                PlotPixel(x0, y0);
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

        internal virtual void Update()
        {
            selected = UIUIState.Instance.menuPanel?.selected;
        }
    }
}