using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials
{
    public class BEPlayer : ModPlayer
    {
        public bool ValidCursorPos => Player.tileTargetX > 0 && Player.tileTargetX < Main.maxTilesX &&
                                      Player.tileTargetY > 0 && Player.tileTargetY < Main.maxTilesY;

        public Vector2 PointedCoord => Main.MouseWorld;
        public Vector2 PointedTileCoord => new Vector2(Player.tileTargetX, Player.tileTargetY);
        public Tile PointedTile => Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        public bool InfiniteRange { get; set; }
        public bool InfinitePlacement { get; set; }
        public bool PlacementAnywhere { get; set; }

        public override void PreUpdate()
        {
            if (Main.netMode != NetmodeID.Server && ValidCursorPos)
            {
                InfiniteRange = true;
            }
        }

        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);
            //Update UI Wheel selected indexes and stuff
        }
    }
}