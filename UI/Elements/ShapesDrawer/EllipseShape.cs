using System;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class EllipseShape : BaseShape
    {
        /// <summary></summary>
        /// <param name="itemType">Item Type that must be held by the player for the shape to be able to be modified</param>
        /// <param name="uiState">UIState to attach to</param>
        public EllipseShape(int itemType, UIState uiState) : base(itemType, uiState)
        {
            
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

        //Adapted from http://members.chello.at/easyfilter/bresenham.html
        internal void PlotSelection(bool isFill = false)
        {
            int rectWidth = (int) Math.Abs(cs.RMBEnd.X - cs.RMBStart.X);
            int rectHeight = (int) Math.Abs(cs.RMBEnd.Y - cs.RMBStart.Y);
            if (rectWidth == 0 && rectHeight == 0) return;
            
            int minX = (int) (cs.RMBStart.X < cs.RMBEnd.X ? cs.RMBStart.X : cs.RMBEnd.X);
            int minY = (int) (cs.RMBStart.Y < cs.RMBEnd.Y ? cs.RMBStart.Y : cs.RMBEnd.Y);
            
            int x0 = (int) cs.RMBStart.X;
            int y0 = (int) cs.RMBStart.Y;
            int x1 = (int) cs.RMBEnd.X;
            int y1 = (int) cs.RMBEnd.Y;

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
                if (allQuads || quadOne)
                    PlotPixel(x1, y0); /*   I. Quadrant */
                if (allQuads || quadTwo)
                    PlotPixel(x0, y0); /*  II. Quadrant */
                if (allQuads || quadThree)
                    PlotPixel(x0, y1); /* III. Quadrant */
                if (allQuads || quadFour)
                    PlotPixel(x1, y1); /*  IV. Quadrant */

                if (isFill)
                {
                    //No half shape selected
                    if (allQuads)
                    {
                        //Horizontal
                        PlotLine(x0, y0, x1, y0, false);
                        PlotLine(x0, y1, x1, y1, false);
                        //Vertical
                        PlotLine(x0, y0, x0, y1, false);
                        PlotLine(x1, y0, x1, y1, false);

                        //Diagonals (sometimes a few blocks are left to be placed and this fills the remaining)
                        PlotLine(x0, y0, x1, y1, false);
                        PlotLine(x0, y1, x1, y0, false);
                    }
                    
                    // Half Shapes
                    if (selected[3])
                    {
                        int tempY = !selected[5] ? y0 : y1;
                        bool condition = !selected[5] ? tempY < y1 : tempY > y1;

                        do
                        {
                            PlotLine(x0, tempY, x1, tempY, false);
                            tempY += condition ? 1 : -1;
                        } while (condition);
                    }
                    else if (selected[4])
                    {
                        int tempX = !selected[5] ? x0 : x1;
                        bool condition = !selected[5] ? tempX > x1 : tempX < x1;

                        do
                        {
                            PlotLine(tempX, y0, tempX, y1, false);
                            tempX += condition ? -1 : 1;
                        } while (condition);
                    }
                }
                
                e2 = 2*err;
                if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */ 
                if (e2 >= dx || 2*err > dy) { x0++; x1--; err += dx += b1; } /* x step */
            } while (x0 <= x1);
   
            while (y0-y1 < b) {  /* too early stop of flat ellipses a=1 */
                PlotPixel(x0-1, y0); /* -> finish tip of ellipse */
                PlotPixel(x1+1, y0++); 
                PlotPixel(x0-1, y1);
                PlotPixel(x1+1, y1--); 
            }

            if (CanPlaceItems)
            {
                int syncSize = rectWidth > rectHeight ? rectWidth : rectHeight;
                syncSize += 1 + (syncSize % 2);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, minX, minY, syncSize);
            }

            CanPlaceItems = false;
            //Draw line in axis if rectangle has a center in the X/Y axis
            Color tempColor = color;
            color = tempColor * 0.4f;
            int quad = cs.SelectedQuad((int) cs.RMBStart.X, (int) cs.RMBStart.Y, (int) cs.RMBEnd.X, (int) cs.RMBEnd.Y);

            //Vertical line
            if (rectWidth % 2 == 0 && (selected[3] || (!selected[3] && !selected[4])) && rectWidth > 3)
            {
                int fixedX = (int) (cs.RMBStart.X + (quad == 0 || quad == 2 ? - rectWidth / 2 : + rectWidth / 2));
                PlotLine(fixedX, (int) cs.RMBStart.Y, fixedX, (int) cs.RMBEnd.Y);
            }

            //Horizontal line
            if (rectHeight % 2 == 0 && (selected[4] || (!selected[3] && !selected[4])) && rectHeight > 3)
            {
                int fixedY = (int) (cs.RMBStart.Y + (quad == 2 || quad == 3 ? + rectHeight / 2 : - rectHeight / 2));
                PlotLine((int) (cs.RMBStart.X), fixedY, (int) (cs.RMBEnd.X), fixedY);
            }
            color = tempColor;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!selected[0]) return;
            cs.UpdateCoords();
            
            if (cs.RMBDown && (selected[3] || selected[4]))
                FixHalfShapesOffset();
            
            color = selected[2] ? Yellow : Blue;
            if (cs.RMBStart != cs.RMBEnd)
                PlotSelection(selected[2]);
        }
    }
}