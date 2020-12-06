using System;
using BuilderEssentials.Items;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawerUI
{
    public class EllipseShape : ShapesDrawerSelection
    {
        private bool[] selected = UIStateLogic1.menuPanel.selected;

        public void PlotEllipse(int x0, int y0, int x1, int y1)
        {
            bool[] selected = UIStateLogic1.menuPanel.selected;
            bool quadOne = (selected[4] && selected[5]) || (selected[3] && !selected[5]);
            bool quadTwo = (selected[4] && !selected[5]) || (selected[3] && !selected[5]);
            bool quadThree = (selected[3] && selected[5]) || (selected[4] && !selected[5]);
            bool quadFour = (selected[3] && selected[5]) || (selected[4] && selected[5]);
            bool noQuad = !quadOne && !quadTwo && !quadThree && !quadFour;

            int a = Math.Abs(x1 - x0), b = Math.Abs(y1 - y0), b1 = b & 1; //values of diameter
            long dx = 4 * (1 - a) * b * b, dy = 4 * (b1 + 1) * a * a; //error increment 
            long err = dx + dy + b1 * a * a, e2; //error of 1.step

            if (x0 > x1)
            {
                x0 = x1;
                x1 += a;
            } //if called with swapped points

            if (y0 > y1) y0 = y1; //exchange them
            y0 += (b + 1) / 2;
            y1 = y0 - b1; //starting pixel
            a *= 8 * a;
            b1 = 8 * b * b;

            do
            {
                if (noQuad || quadOne)
                    DrawRectangle(x1, y0); //   I. Quadrant
                if (noQuad || quadTwo)
                    DrawRectangle(x0, y0); //  II. Quadrant
                if (noQuad || quadThree)
                    DrawRectangle(x0, y1); // III. Quadrant
                if (noQuad || quadFour)
                    DrawRectangle(x1, y1); //  IV. Quadrant

                //Fill
                if (cs.LMBDown && selected[2] && ShapesDrawer.selectedItemType != -1)
                {
                    //No half shape selected
                    if (noQuad)
                    {
                        //Horizontal
                        PlotLine(x0, y0, x1, y0);
                        PlotLine(x0, y1, x1, y1);
                        //Vertical
                        PlotLine(x0, y0, x0, y1);
                        PlotLine(x1, y0, x1, y1);

                        //Diagonals (sometimes a few blocks are left to be placed and this fills the remaining)
                        PlotLine(x0, y0, x1, y1);
                        PlotLine(x0, y1, x1, y0);
                    }

                    //Half Shapes
                    if (selected[3])
                    {
                        int tempY = !selected[5] ? y0 : y1;
                        bool condition = !selected[5] ? tempY < y1 : tempY > y1;

                        do
                        {
                            PlotLine(x0, tempY, x1, tempY);
                            tempY += condition ? 1 : -1;
                        } while (condition);
                    }
                    else if (selected[4])
                    {
                        int tempX = !selected[5] ? x0 : x1;
                        bool condition = !selected[5] ? tempX > x1 : tempX < x1;

                        do
                        {
                            PlotLine(tempX, y0, tempX, y1);
                            tempX += condition ? -1 : 1;
                        } while (condition);
                    }
                }

                e2 = 2 * err;
                if (e2 <= dy)
                {
                    y0++;
                    y1--;
                    err += dy += a;
                } //y step

                if (e2 >= dx || 2 * err > dy)
                {
                    x0++;
                    x1--;
                    err += dx += b1;
                } //x step
            } while (x0 <= x1);

            while (y0 - y1 < b)
            {
                //too early stop of flat ellipses a=1
                DrawRectangle(x0 - 1, y0); //-> finish tip of ellipse
                DrawRectangle(x1 + 1, y0++);
                DrawRectangle(x0 - 1, y1);
                DrawRectangle(x1 + 1, y1--);
            }
        }
        
        void FixHalfShapesOffset()
        {
            //TODO: FIX HALF SHAPES HORIZONTAL/VERTICAL OFFSETS (KEEP LEFT TOP CORNER FIXED)

            if (selected[3])
            {
                //Preventing half shape from "mirroring" to the other quadrant
                if (selected[5] && cs.RMBEnd.Y >= cs.RMBStart.Y)
                    cs.RMBEnd.Y = cs.RMBStart.Y;
                else if (!selected[5] && cs.RMBEnd.Y <= cs.RMBStart.Y)
                    cs.RMBEnd.Y = cs.RMBStart.Y;
            }
            else if (selected[4])
            {
                //Preventing half shape from "mirroring" to the other quadrant
                if (selected[5] && cs.RMBEnd.X <= cs.RMBStart.X)
                    cs.RMBEnd.X = cs.RMBStart.X;
                else if (!selected[5] && cs.RMBEnd.X >= cs.RMBStart.X)
                    cs.RMBEnd.X = cs.RMBStart.X;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!selected[0]) return;
            cs.UpdateCoords();
            
            if (cs.RMBDown && (selected[3] || selected[4]))
                FixHalfShapesOffset();

            color = Blue;
            if (selected[2])
                color = Yellow;
            
            if (cs.RMBStart != cs.RMBEnd)
                PlotEllipse((int) cs.RMBStart.X, (int) cs.RMBStart.Y, (int) cs.RMBEnd.X, (int) cs.RMBEnd.Y);
        }
    }
}