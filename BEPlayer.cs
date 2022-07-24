using System;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.Items.Accessories;
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
    public bool IsImprovedRulerEquipped { get; set; }
    public bool FastPlacement { get; set; }
    public bool InfiniteRange { get; set; }
    public bool InfinitePlacement { get; set; }
    public bool InfinitePickupRange { get; set; }
    public ModItem EquippedWrenchInstance { get; set; }

    public override void ResetEffects() {
        InfinitePaint = IsImprovedRulerEquipped = FastPlacement = InfiniteRange = 
            InfinitePlacement = InfinitePickupRange = false;
        Player.defaultItemGrabRange = 42;
        EquippedWrenchInstance = null;
    }

    public override void ProcessTriggers(TriggersSet triggersSet) {
        if (BuilderEssentials.FWIncrease.JustPressed && FillWand.FillSelectionSize < 6)
            FillWand.FillSelectionSize++;
        
        if (BuilderEssentials.FWDecrease.JustPressed && FillWand.FillSelectionSize > 1)
            FillWand.FillSelectionSize--;

        if (BuilderEssentials.UndoPlacement.JustPressed) {
            Item heldItem = Main.LocalPlayer.HeldItem;

            //Hardcoded.. get a way to bind keybind to an item that's not being held..?
            if (heldItem.ModItem is FillWand)
                ShapesUIState.GetUIPanel<FillWandPanel>().UndoPlacement();
            else if (heldItem.ModItem is ShapesDrawer)
                ShapesUIState.GetUIPanel<ShapesDrawerPanel>().UndoPlacement();
        }
    }

    public override void PostUpdate() {
        MirrorPlacement.PlayerPostUpdate();
        BuildingWrench.DequeueRecipeChanges();
    }
    
    public override void PostUpdateEquips() {
        if (InfiniteRange) {
            Player.tileRangeX += Main.screenWidth / 16 / 2;
            Player.tileRangeY += Main.screenHeight / 16 / 2;
            Player.blockRange += Main.screenWidth / 16 / 2;
        }
        
        if (FastPlacement) {
            Player.tileSpeed += 500;
            Player.wallSpeed += 500;
        }
        
        if (InfinitePickupRange)
            Player.defaultItemGrabRange += ModContent.GetInstance<MainConfig>().InfinitePickupRangeValue;
    }
}