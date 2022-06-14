using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials;

public class BEPlayer : ModPlayer
{
    public static Vector2 PointedCoord => Main.MouseWorld;
    public static Vector2 CachedPointedCoord => ModContent.GetInstance<UISystem>().cachedMouseCoords;
    // public static Vector2 PointedTileCoord => new Vector2(Player.tileTargetX, Player.tileTargetY);
}