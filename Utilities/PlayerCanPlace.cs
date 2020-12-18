using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
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

            //TODO: FIX THESE 2 IF CONDITIONS (REMOVING FROM THE WRONG STACK WHEN PLACING ITEMS);
            if (HelperMethods.ValidTilePlacement(i, j) || mp.InfinitePlacement)
            {
                if (mp.InfinitePlacement && !HelperMethods.ValidTilePlacement(i, j))
                    return false;
                
                HelperMethods.PlaceTile(i, j, heldItem.type, true);
                PlaceInWorld(i, j, heldItem);
                return false;
            }

            return base.CanPlace(i, j, type);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();

            HelperMethods.MirrorPlacement(i, j, item.type);

            if (!mp.PlacementAnywhere) return;

            //I'm hardcoding this since vanilla also hardcodes their shitty frameX changes
            int[] directionFraming = new int[]
            {
                TileID.Chairs, TileID.Bathtubs, TileID.Beds, TileID.Mannequin, TileID.Womannequin
                //+ any other tile that mirrors with player direction
            };

            Tile tile = Framing.GetTileSafely(i, j);
            TileObjectData data = TileObjectData.GetTileData(tile);

            if (directionFraming.Contains(item.createTile))
                HelperMethods.ChangeTileFraming(i, j, Main.LocalPlayer.direction == 1);
        }
    }

    public class PlacementAnywhereWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            item.consumable = !Main.LocalPlayer.GetModPlayer<BEPlayer>().InfinitePlacement;
            HelperMethods.MirrorPlacement(i, j, item.type);
            //base.PlaceInWorld(i, j, type, item);
        }
    }
}