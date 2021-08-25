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
        public RectangleShape(int itemType, UIState uiState) : base(itemType, uiState)
        {
            
        }

        internal void PlotSelection(bool isFill = false, bool isPlacing = false)
        {
            //Maybe place tiles with a BFS algorithm from the top left corner instead of all at once for big selections?

            if (isPlacing && Items.ShapesDrawer.selectedItemType <= 0) return;

            int rectWidth = (int) Math.Abs(cs.RMBEnd.X - cs.RMBStart.X);
            int rectHeight = (int) Math.Abs(cs.RMBEnd.Y - cs.RMBStart.Y);
            if (rectWidth == 0 && rectHeight == 0) return;
            
            int minX = (int) (cs.RMBStart.X < cs.RMBEnd.X ? cs.RMBStart.X : cs.RMBEnd.X);
            int minY = (int) (cs.RMBStart.Y < cs.RMBEnd.Y ? cs.RMBStart.Y : cs.RMBEnd.Y);

            for (int i = 0; i <= rectWidth; i++)
            {
                for (int j = 0; j <= rectHeight; j++)
                {
                    if (!isFill && !(i == 0 || j == 0 || i == rectWidth || j == rectHeight)) continue;
                    PlotPixel(minX + i, minY + j, isPlacing, Items.ShapesDrawer.selectedItemType);
                }
            }

            if (isPlacing)
            {
                int syncSize = rectWidth > rectHeight ? rectWidth : rectHeight;
                syncSize += 1 + (syncSize % 2);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, minX, minY, syncSize);
            }

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
            if (!selected[1]) return;
            cs.UpdateCoords();
            
            color = selected[2] ? Yellow : Blue;
            if (cs.RMBStart != cs.RMBEnd)
                PlotSelection(selected[2], Items.ShapesDrawer.canPlaceItems);
        }
    }
}