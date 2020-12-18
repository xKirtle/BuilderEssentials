using System;
using BuilderEssentials.Items;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawerUI
{
    public class BezierCurve : BaseShape
    {
        public CoordsSelection cs;
        
        void plotBasicQuadBezier(int x0, int y0, int x1, int y1, int x2, int y2)
        {
            int sx = x0 < x2 ? 1 : -1, sy = y0 < y2 ? 1 : -1; /* step direction */
            double x = x0 - 2 * x1 + x2, y = y0 - 2 * y1 + y2, xy = 2 * x * y * sx * sy;
            double cur = sx * sy * (x * (y2 - y0) - y * (x2 - x0)) /
                         2; /* curvature */ /* compute error increments of P0 */
            double dx = (1 - 2 * Math.Abs(x0 - x1)) * y * y + Math.Abs(y0 - y1) * xy - 2 * cur * Math.Abs(y0 - y2);
            double dy = (1 - 2 * Math.Abs(y0 - y1)) * x * x + Math.Abs(x0 - x1) * xy +
                        2 * cur * Math.Abs(x0 - x2); /* compute error increments of P2 */
            double ex = (1 - 2 * Math.Abs(x2 - x1)) * y * y + Math.Abs(y2 - y1) * xy + 2 * cur * Math.Abs(y0 - y2);
            double ey = (1 - 2 * Math.Abs(y2 - y1)) * x * x + Math.Abs(x2 - x1) * xy -
                        2 * cur * Math.Abs(x0 - x2); /* sign of gradient must not change */
            if (cur == 0)
            {
                PlotLine(x0, y0, x2, y2);
                return;
            } /* straight line */

            x *= 2 * x;
            y *= 2 * y;
            if (cur < 0)
            {
                /* negated curvature */
                x = -x;
                dx = -dx;
                ex = -ex;
                xy = -xy;
                y = -y;
                dy = -dy;
                ey = -ey;
            } /* algorithm fails for almost straight line, check error values */

            if (dx >= -y || dy <= -x || ex <= -y || ey >= -x)
            {
                x1 = (x0 + 4 * x1 + x2) / 6;
                y1 = (y0 + 4 * y1 + y2) / 6; /* approximation */
                PlotLine(x0, y0, x1, y1);
                PlotLine(x1, y1, x2, y2);
                return;
            }

            dx -= xy;
            ex = dx + dy;
            dy = dy - xy; /* error of 1.step */
            for (;;)
            {
                /* plot curve */
                DrawRectangle(x0, y0);
                ey = 2 * ex - dy; /* save value for test of y step */
                if (2 * ex >= dx)
                {
                    /* x step */
                    if (x0 == x2) break;
                    x0 += sx;
                    dy -= xy;
                    ex += dx += y;
                }

                if (ey <= 0)
                {
                    /* y step */
                    if (y0 == y2) break;
                    y0 += sy;
                    dx -= xy;
                    ex += dy += x;
                }
            }
        }
        
        public BezierCurve()
        {
            cs = new CoordsSelection(ModContent.ItemType<ShapesDrawer>());
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            cs.UpdateCoords();
            color = Blue;

            plotBasicQuadBezier((int)cs.LMBEnd.X, (int)cs.LMBEnd.Y, (int)cs.MMBEnd.X, 
                (int)cs.MMBEnd.Y, (int)cs.RMBEnd.X, (int)cs.RMBEnd.Y);
        }
    }
}