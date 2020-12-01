using Terraria;
using Terraria.ModLoader;

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
            //Call mirror wand here
            base.PlaceInWorld(i, j, item);
        }
    }
}