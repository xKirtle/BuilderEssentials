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
            CanPlaceTiles =
                item => itemToWorkWith == item && selected[2] && !cs.RMBDown; //&& shapesDrawer has selected an itemType
        }

        internal void PlotRectangle(int x0, int y0, int x1, int y1)
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

                //Draw line in axis if rectangle has a center in the X/Y axis
                Color tempColor = color;
                color = tempColor * 0.4f;
                int horDist = Math.Abs((int) (x1 - x0));
                int verDist = Math.Abs((int) (y1 - y0));
                int quad = cs.SelectedQuad((int) x0, (int) y0, (int) x1, (int) y1);

                //Vertical line
                if (horDist % 2 == 0 && (selected[3] || (!selected[3] && !selected[4])) && horDist > 3)
                {
                    int fixedX = (int) (x0 + (quad == 0 || quad == 2 ? - horDist / 2 : + horDist / 2));
                    PlotLine(fixedX, (int) y0, fixedX, (int) y1);
                }

                //Horizontal line
                if (verDist % 2 == 0 && (selected[4] || (!selected[3] && !selected[4])) && verDist > 3)
                {
                    int fixedY = (int) (y0 + (quad == 2 || quad == 3 ? + verDist / 2 : - verDist / 2));
                    PlotLine((int) (x0), fixedY, (int) (x1), fixedY);
                }
                color = tempColor;
            }
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

            color = selected[2] ? Yellow : Blue;
            if (cs.RMBStart != cs.RMBEnd)
                PlotRectangle((int) cs.RMBStart.X, (int) cs.RMBStart.Y, (int) cs.RMBEnd.X, (int) cs.RMBEnd.Y);
        }
    }
}