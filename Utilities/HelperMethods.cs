using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;

namespace BuilderEssentials.Utilities
{
    internal static partial class HelperMethods
    {
        internal static bool ValidTilePlacement(int posX, int posY)
        {
            int[] treeTypes =
            {
                //[TAG 1.4] Implements new trees
                TileID.Trees,
                TileID.PalmTree,
                TileID.PineTree,
                TileID.MushroomTrees,
                TileID.ChristmasTree,
            };

            Tile middle = Framing.GetTileSafely(posX, posY);
            Tile top = Framing.GetTileSafely(posX, posY - 1);
            Tile right = Framing.GetTileSafely(posX + 1, posY);
            Tile bottom = Framing.GetTileSafely(posX, posY + 1);
            Tile left = Framing.GetTileSafely(posX - 1, posY);

            return !middle.active() || !Main.tileSolid[middle.type] &&
                treeTypes.Contains(middle.type) &&
                (
                    (top.active() && Main.tileSolid[top.type]) ||
                    (right.active() && Main.tileSolid[right.type]) ||
                    (bottom.active() && Main.tileSolid[bottom.type]) ||
                    (left.active() && Main.tileSolid[left.type]) ||
                    middle.wall != 0
                );
        }

        internal static bool ToolHasRange(int range)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            return Vector2.Distance(Main.LocalPlayer.Center, mp.pointedCoord) < range * 16 &&
                   ValidTilePlacement((int) mp.pointedTileCoord.X, (int) mp.pointedTileCoord.Y);
        }
    }
}