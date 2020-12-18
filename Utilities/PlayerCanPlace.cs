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
            
            if (mp.PlacementAnywhere || mp.InfinitePlacement)
            {
                Item item = new Item();
                item.SetDefaults(heldItem.type);
                HelperMethods.PlaceTile(i, j, heldItem.type, true);
                HelperMethods.CanReduceItemStack(item.tileWand == -1 ? heldItem.type : heldItem.tileWand,
                    reduceStack: true);
                PlaceInWorld(i, j, item);

                return false;
            }

            return base.CanPlace(i, j, type);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();

            HelperMethods.MirrorPlacement(i, j, item.type);
            base.PlaceInWorld(i, j, item);

            //---------------------------Does not work yet---------------------------
            //I'm hardcoding this since vanilla also hardcodes their shitty frameX changes
            int[] directionFraming = new int[]
            {
                TileID.Chairs, TileID.Bathtubs, TileID.Beds, TileID.Mannequin, TileID.Womannequin
                //+ any other tile that mirrors with player direction
            };

            Tile tile = Framing.GetTileSafely(i, j);
            TileObjectData data = TileObjectData.GetTileData(tile);
            Vector2 topLeft = new Vector2(Player.tileTargetX, Player.tileTargetY) - data.Origin.ToVector2();

            if (directionFraming.Contains(item.createTile))
                HelperMethods.ChangeTileFraming(i, j, Main.LocalPlayer.direction == 1);
        }
    }

    public class PlacementAnywhereWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            //TODO: Apply Inf Placement to walls
            HelperMethods.MirrorPlacement(i, j, item.type);
            //base.PlaceInWorld(i, j, type, item);
        }
    }
}