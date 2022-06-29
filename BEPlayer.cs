using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace BuilderEssentials;

public class BEPlayer : ModPlayer
{
    public static Vector2 PointedWorldCoords => Main.MouseWorld;
    public static Vector2 PointedScreenCoords => Main.MouseScreen;
    public static Vector2 PointedTileCoords => new Vector2(Player.tileTargetX, Player.tileTargetY);
    
    public bool InfinitePaint { get; set; }

    public override void ResetEffects() {
        InfinitePaint = false;
    }

    public override void ProcessTriggers(TriggersSet triggersSet) {
        if (BuilderEssentials.FWIncrease.JustPressed && FillWand.FillSelectionSize < 6)
            FillWand.FillSelectionSize++;
        
        if (BuilderEssentials.FWDecrease.JustPressed && FillWand.FillSelectionSize > 1)
            FillWand.FillSelectionSize--;
    }

    public override void PostUpdate() {
        MirrorPlacementDetours.PlayerPostUpdate();
    }
}