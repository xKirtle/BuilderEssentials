using BuilderEssentials.Items;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawerUI
{
    public class RectangleShape : ShapesDrawerSelection
    {
        private bool[] selected = UIStateLogic1.menuPanel.selected;

        public override void PlotRectangle(int x0, int y0, int x1, int y1)
        {
            base.PlotRectangle(x0, y0, x1, y1);

            bool[] selected = UIStateLogic1.menuPanel.selected;
            if (ShapesDrawer.LMBDown && selected[2] && ShapesDrawer.selectedItemType != -1)
            {
                if (y0 < y1)
                {
                    while (y0 < y1)
                    {
                        y0++;
                        PlotLine(x0, y0, x1, y0);
                    }
                }
                else
                {
                    while (y0 > y1)
                    {
                        y0--;
                        PlotLine(x0, y0, x1, y0);
                    }
                }
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