using BuilderEssentials.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class FillWandSelection : BaseShape
    {
        /// <summary></summary>
        /// <param name="itemType">Item Type that must be held by the player for the shape to be able to be modified</param>
        /// <param name="uiState">UIState to attach to</param>
        public FillWandSelection(int itemType, UIState uiState, UIState textUiState = null) : base(itemType, uiState, textUiState)
        {
            
        }

        internal void PlotSelection()
        {
            int size = FillWand.fillSelectionSize;
            for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                PlotPixel(Player.tileTargetX + j, Player.tileTargetY + i - size + 1);
            
            if (CanPlaceItems)
            {
                int syncSize = size;
                syncSize += 1 + (syncSize % 2);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY - size, syncSize);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PlotSelection();
        }
    }
}