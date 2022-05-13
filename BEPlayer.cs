using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials;

public class BEPlayer : ModPlayer
{
    public Vector2 PointedCoord => Main.MouseWorld;
    public Vector2 PointedTileCoord => new Vector2(Player.tileTargetX, Player.tileTargetY);
}