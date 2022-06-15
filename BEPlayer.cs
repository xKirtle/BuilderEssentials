using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials;

public class BEPlayer : ModPlayer
{
    public static Vector2 PointedWorldCoords => Main.MouseWorld;
    public static Vector2 PointedScreenCoords => Main.MouseScreen;
    public static Vector2 CachedScreenCoords => ModContent.GetInstance<ToggleableItemsUISystem>().cachedScreenCoords;
    public static Vector2 PointedTileCoords => new Vector2(Player.tileTargetX, Player.tileTargetY);
    
    public bool InfinitePaint { get; set; }

    public override void ResetEffects() {
        InfinitePaint = false;
    }
}