using System;
using System.Xml.Linq;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class RectangleShape : BaseShape
    {
        private int itemToWorkWith;
        /// <summary></summary>
        /// <param name="itemType">Item Type that must be held by the player for the shape to be able to be modified</param>
        public RectangleShape(int itemType, UIState uiState)
        {
            itemToWorkWith = itemType;
            cs = new CoordsSelection(itemType, uiState);
            CanPlaceTiles = item => itemToWorkWith == item && selected[2]; //&& shapesDrawer has selected an itemType
        }

        internal override void PlotRectangle(int x0, int y0, int x1, int y1)
        {
            if (cs.LMBDown && CanPlaceTiles(Main.LocalPlayer.HeldItem.type))
            {
                int iterations = y0 < y1 ? y1 - y0 : y0 - y1;
                int diff = y0 < y1 ? 1 : -1;
                for (int i = 0; i <= iterations ; i++)
                {
                    PlotLine(x0, y0, x1, y0);
                    y0 += diff;
                }
            }
            else base.PlotRectangle(x0, y0, x1, y1);
        }

        internal override void DrawRectangle(int x, int y)
        {
            base.DrawRectangle(x, y);
            
            if (cs.LMBDown && CanPlaceTiles(Main.LocalPlayer.HeldItem.type))
            {
                //Place tiles here
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!selected[1]) return;
            cs.UpdateCoords();

            color = Blue;
            if (selected[2])
                color = Yellow;

            if (cs.RMBStart != cs.RMBEnd)
                PlotRectangle((int) cs.RMBStart.X, (int) cs.RMBStart.Y, (int) cs.RMBEnd.X, (int) cs.RMBEnd.Y);
        }
    }
}