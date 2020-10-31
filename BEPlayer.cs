using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials
{
    public class BEPlayer : ModPlayer
    {
        public bool ValidCursorPos => Player.tileTargetX != 0 && Player.tileTargetY != 0;

        public Vector2 pointedCoord;
        public Vector2 pointedTileCoord;
        public Tile pointedTile;
        public override void PreUpdate()
        {
            if (Main.netMode != NetmodeID.Server && ValidCursorPos)
            {
                pointedCoord = Main.MouseWorld;
                pointedTileCoord = new Vector2(Player.tileTargetX, Player.tileTargetY);
                pointedTile = Framing.GetTileSafely(pointedTileCoord);
            }
        }
    }
}