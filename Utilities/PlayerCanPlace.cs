using On.Terraria.DataStructures;
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

            //TODO: Placement anywhere is placing on top stuff like mushrooms and not dropping them
            if (mp.PlacementAnywhere || mp.InfinitePlacement)
            {
                Item item = new Item();
                item.SetDefaults(heldItem.type);
                HelperMethods.PlaceTile(i, j, heldItem.type, true);
                HelperMethods.CanReduceItemStack(item.tileWand == -1 ? heldItem.type : heldItem.tileWand, reduceStack: true);
                PlaceInWorld(i, j, item);

                return false;
            }

            return base.CanPlace(i, j, type);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            HelperMethods.MirrorPlacement(i, j, item.type);
            base.PlaceInWorld(i, j, item);

            //default placement assumes direction == -1
            if (Main.LocalPlayer.direction == 1)
                HelperMethods.InvertTilePlacement(i, j);
        }
    }
}