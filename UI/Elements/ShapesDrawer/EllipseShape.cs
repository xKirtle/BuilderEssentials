using System;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class EllipseShape : BaseShape
    {
        private int itemToWorkWith;

        /// <summary></summary>
        /// <param name="itemType">Item Type that must be held by the player for the shape to be able to be modified</param>
        public EllipseShape(int itemType, UIState uiState)
        {
            itemToWorkWith = itemType;
            cs = new CoordsSelection(itemType, uiState);
            CanPlaceTiles = item => itemToWorkWith == item && selected[2] && !cs.RMBDown; //&& shapesDrawer has selected an itemType
        }
        
        //Adapted from http://members.chello.at/easyfilter/bresenham.html
        public void PlotEllipse(int x0, int y0, int x1, int y1, bool fill = false)
        {
            int initialY0 = y0;
            int initialY1 = y1;
            int initialX0 = x0;
            int initialX1 = x1;
            
            bool allQuads = !selected[3] && !selected[4];
            bool quadOne, quadTwo, quadThree, quadFour;
            quadOne = quadTwo = quadThree = quadFour = false;

            if (!allQuads)
            {
                quadOne = (selected[4] && selected[5]) || (selected[3] && !selected[5]);
                quadTwo = (selected[4] && !selected[5]) || (selected[3] && !selected[5]);
                quadThree = (selected[3] && selected[5]) || (selected[4] && !selected[5]);
                quadFour = (selected[3] && selected[5]) || (selected[4] && selected[5]);
                
                if (quadTwo && quadThree)
                    x0 = x0 + Math.Abs(x1 - x0);
                if (quadThree && quadFour)
                    y0 = y0 + Math.Abs(y1 - y0);
                if (quadOne && quadFour)
                    x0 = x0 - Math.Abs(x1 - x0);
                if (quadOne && quadTwo)
                    y0 = y0 - Math.Abs(y1 - y0);
            }

            int a = Math.Abs(x1-x0), b = Math.Abs(y1-y0), b1 = b&1; /* values of diameter */
            long dx = 4*(1-a)*b*b, dy = 4*(b1+1)*a*a; /* error increment */
            long err = dx+dy+b1*a*a, e2; /* error of 1.step */

            if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
            if (y0 > y1) y0 = y1; /* .. exchange them */
            y0 += (b+1)/2; y1 = y0-b1;   /* starting pixel */
            a *= 8*a; b1 = 8*b*b;

            do {
                if (fill && cs.LMBDown)
                {
                    if (allQuads)
                    {
                        PlotLine(x0, y0, x0, y1);
                        PlotLine(x1, y0, x1, y1);
                    }
                    else if (selected[3])
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
                else
                {
                    if (allQuads || quadOne)
                        DrawRectangle(x1, y0); /*   I. Quadrant */
                    if (allQuads || quadTwo)
                        DrawRectangle(x0, y0); /*  II. Quadrant */
                    if (allQuads || quadThree)
                        DrawRectangle(x0, y1); /* III. Quadrant */
                    if (allQuads || quadFour)
                        DrawRectangle(x1, y1); /*  IV. Quadrant */
                }

                e2 = 2*err;
                if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */ 
                if (e2 >= dx || 2*err > dy) { x0++; x1--; err += dx += b1; } /* x step */
            } while (x0 <= x1);
   
            while (y0-y1 < b) {  /* too early stop of flat ellipses a=1 */
                DrawRectangle(x0-1, y0); /* -> finish tip of ellipse */
                DrawRectangle(x1+1, y0++); 
                DrawRectangle(x0-1, y1);
                DrawRectangle(x1+1, y1--); 
            }

            if (fill && cs.LMBDown) return;
            
            //Draw line in axis if ellipse has a center in the X/Y axis
            Color tempColor = color;
            color = tempColor * 0.4f;
            int horDist = Math.Abs((int) (initialX1 - initialX0));
            int verDist = Math.Abs((int) (initialY1 - initialY0));
            int quad = cs.SelectedQuad((int) initialX0, (int) initialY0, (int) initialX1, (int) initialY1);

            //Vertical line
            if (horDist % 2 == 0 && (selected[3] || (!selected[3] && !selected[4])) && horDist > 3)
            {
                int fixedX = (int) (initialX0 + (quad == 0 || quad == 2 ? - horDist / 2 : + horDist / 2));
                PlotLine(fixedX, (int) initialY0, fixedX, (int) initialY1);
            }

            //Horizontal line
            if (verDist % 2 == 0 && (selected[4] || (!selected[3] && !selected[4])) && verDist > 3)
            {
                int fixedY = (int) (initialY0 + (quad == 2 || quad == 3 ? + verDist / 2 : - verDist / 2));
                PlotLine((int) (initialX0), fixedY, (int) (initialX1), fixedY);
            }
            color = tempColor;
        }

        internal void FixMirrorHalfShapesCoords()
        {
            if (!selected[3] && !selected[4]) return;
            
            if (!selected[5]) //if mirror is not selected
            {
                if (selected[3])
                    FixCoordsToQuad(ref cs.RMBStart, ref cs.RMBEnd, 3);
                else if (selected[4])
                    FixCoordsToQuad(ref cs.RMBStart, ref cs.RMBEnd, 2);
            }
            else
            {
                if (selected[3])
                    FixCoordsToQuad(ref cs.RMBStart, ref cs.RMBEnd, 1);
                else if (selected[4])
                    FixCoordsToQuad(ref cs.RMBStart, ref cs.RMBEnd, 3);
            }
        }

        //Fixing the coords to display the correct half shape (quadrant matters)
        //otherwise FixHalfShapesOffset() would make the half shape a single line a lot of times
        private void FixCoordsToQuad(ref Vector2 start, ref Vector2 end, int desiredQuad)
        {
            float lowestX = start.X < end.X ? start.X : end.X;
            float highestX = start.X > end.X ? start.X : end.X;
            float lowestY = start.Y < end.Y ? start.Y : end.Y;
            float highestY = start.Y > end.Y ? start.Y : end.Y;
            Vector2 topLeftCorner = new Vector2(lowestX, lowestY);
            Vector2 topRightCorner = new Vector2(highestX, lowestY);
            Vector2 bottomLeftCorner = new Vector2(lowestX, highestY);
            Vector2 bottomRightCorner = new Vector2(highestX, highestY);
            
            //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
            switch (desiredQuad)
            {
                case 0:
                    start = bottomRightCorner;
                    end = topLeftCorner;
                    break;
                case 1:
                    start = bottomLeftCorner;
                    end = topRightCorner;
                    break;
                case 2:
                    start = topRightCorner;
                    end = bottomLeftCorner;
                    break;
                case 3:
                    start = topLeftCorner;
                    end = bottomRightCorner;
                    break;
            }
        }
        
        //Preventing half shape from "mirroring" to the other quadrant
        private void FixHalfShapesOffset()
        {
            if (selected[3])
            {
                if (selected[5] && cs.RMBEnd.Y >= cs.RMBStart.Y)
                    cs.RMBEnd.Y = cs.RMBStart.Y;
                else if (!selected[5] && cs.RMBEnd.Y <= cs.RMBStart.Y)
                    cs.RMBEnd.Y = cs.RMBStart.Y;
            }
            else if (selected[4])
            {
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
            
            color = selected[2] ? Yellow : Blue;
            if (cs.RMBStart != cs.RMBEnd)
                PlotEllipse((int) cs.RMBStart.X, (int) cs.RMBStart.Y, (int) cs.RMBEnd.X, (int) cs.RMBEnd.Y, 
                    selected[2] && CanPlaceTiles(Main.LocalPlayer.HeldItem.type));
        }
    }
}