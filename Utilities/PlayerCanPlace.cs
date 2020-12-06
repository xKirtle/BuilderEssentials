using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Utilities
{
    public class PlacementAnywhereTile : GlobalTile
    {
        public override bool CanPlace(int i, int j, int type)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            Item heldItem = mp.player.HeldItem;

            if (mp.PlacementAnywhere || mp.InfinitePlacement)
            {
                Item item = new Item();
                item.SetDefaults(heldItem.type);
                HelperMethods.PlaceTile(i, j, heldItem.type);
                HelperMethods.CanReduceItemStack(item.tileWand == -1 ? heldItem.type : heldItem.tileWand, true);
                PlaceInWorld(i, j, item);

                return false;
            }

            return base.CanPlace(i, j, type);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            HelperMethods.MirrorPlacement(i, j, item.type);
            base.PlaceInWorld(i, j, item);


            // Tile tile = Framing.GetTileSafely(i, j);
            // //Main.NewText(TileObjectData.GetTileData(tile).Direction); PlaceLeft or PlaceRight
            // int style = 0;
            // int alternate = 0;
            // TileObjectData.GetTileInfo(tile, ref style, ref alternate);
            //
            // TileObjectData data = TileObjectData.GetTileData(tile);
            // data.Direction = TileObjectDirection.PlaceRight;
            //
            // Main.NewText($"{style} / {alternate}"); //style is chair type, alternate 0 is left

            //Main.NewText($"{tile.frameX}/{tile.frameY}");
        }
    }
}