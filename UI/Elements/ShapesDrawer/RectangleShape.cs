using System;
using System.Xml.Linq;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class RectangleShape : BaseShape
    {
        /// <summary></summary>
        /// <param name="itemType">Item Type that must be held by the player for the shape to be able to be modified</param>
        /// <param name="uiState">UIState to attach to</param>
        public RectangleShape(int itemType, UIState uiState) : base(itemType, uiState)
        {
            
        }

        internal void PlotSelection(Vector2 start, Vector2 end, bool isFill = false)
        {
            //Maybe place tiles with a BFS algorithm from the top left corner instead of all at once for big selections?

            int rectWidth = (int) Math.Abs(end.X - start.X);
            int rectHeight = (int) Math.Abs(end.Y - start.Y);
            if (rectWidth == 0 && rectHeight == 0) return;
            
            int minX = (int) (start.X < end.X ? start.X : end.X);
            int minY = (int) (start.Y < end.Y ? start.Y : end.Y);

            for (int i = 0; i <= rectWidth; i++)
            {
                for (int j = 0; j <= rectHeight; j++)
                {
                    bool render = (i == 0 || j == 0 || i == rectWidth || j == rectHeight);
                    if (!isFill && !render) continue;
                    PlotPixel(minX + i, minY + j, render);
                }
            }
            
            if (CanPlaceItems && Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, minX, minY, rectWidth * 2, rectHeight * 2);
                        
            CanPlaceItems = false;
            //Draw line in axis if rectangle has a center in the X/Y axis
            Color tempColor = color;
            color = tempColor * 0.4f;
            int quad = cs.SelectedQuad((int) start.X, (int) start.Y, (int) end.X, (int) end.Y);

            //Vertical line
            if (rectWidth % 2 == 0 && (selected[3] || (!selected[3] && !selected[4])) && rectWidth > 3)
            {
                int fixedX = (int) (start.X + (quad == 0 || quad == 2 ? - rectWidth / 2 : + rectWidth / 2));
                PlotLine(fixedX, (int) start.Y, fixedX, (int) end.Y);
            }

            //Horizontal line
            if (rectHeight % 2 == 0 && (selected[4] || (!selected[3] && !selected[4])) && rectHeight > 3)
            {
                int fixedY = (int) (start.Y + (quad == 2 || quad == 3 ? + rectHeight / 2 : - rectHeight / 2));
                PlotLine((int) (start.X), fixedY, (int) (end.X), fixedY);
            }
            color = tempColor;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!selected[1]) return;
            cs.UpdateCoords();
            
            color = selected[2] ? Yellow : Blue;
            if (cs.RMBStart != cs.RMBEnd)
                PlotSelection(cs.RMBStart, cs.RMBEnd, selected[2]);
        }
    }
}