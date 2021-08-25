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
            CanPlaceTiles = item => itemToWorkWith == item && selected[1] && !cs.RMBDown;
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
                    
                    PlotPixel(minX + i, minY + j);
                    if (isPlacing)
                        HelperMethods.PlaceTile(minX + i, minY + j, Items.ShapesDrawer.selectedItemType, sync: false);
                }
            }
            
            int syncSize = rectWidth > rectHeight ? rectWidth : rectHeight;
            syncSize += 1 + (syncSize % 2);
            
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, minX, minY, syncSize);
        }

        private int leftMouseTimer;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!selected[1]) return;
            cs.UpdateCoords();

            color = selected[2] ? Yellow : Blue;
            if (cs.RMBStart != cs.RMBEnd)
                PlotSelection(selected[2], cs.LMBDown && !Main.LocalPlayer.mouseInterface && ++leftMouseTimer == 2);
            
            if (!cs.LMBDown)
                leftMouseTimer = 0;
        }
    }
}