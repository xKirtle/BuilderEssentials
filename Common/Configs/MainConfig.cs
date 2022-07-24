using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using BuilderEssentials.Content.UI;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader.Config;

namespace BuilderEssentials.Common.Configs;

public class MainConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Label("Max Undo Times")]
    [Tooltip("The maximum amount of placements the game will remember and be able to undo")]
    [Range(0, 100), DefaultValue(20)]
    public int MaxUndoNum = 20;
    
    [Label("Range % of the Inf. Pickup Range Upgrade Module")]
    // [Label("[i/s1:54]")]
    [Tooltip("Anything above 20% will start picking up items offscreen")]
    [Range(1, 100), DefaultValue(20)]
    public int InfinitePickupRangeFloat = 20;

    [JsonIgnore]
    public int InfinitePickupRangeValue => (int) (((float) InfinitePickupRangeFloat / 100f) * MaxPickupRange);
    private const int MaxPickupRange = 3500;

    public override void OnChanged() {
        ShapesUIState.UpdateMaxUndoNum(MaxUndoNum);
    }
    
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context) {
        MaxUndoNum = Utils.Clamp(MaxUndoNum, 0, 100);
        InfinitePickupRangeFloat = Utils.Clamp(InfinitePickupRangeFloat, 0, 100);
    }

    [Header("Enable or disable content from this mod")]
    
    [Label("Items"), ReloadRequired]
    public EnabledItemsConfig EnabledItems = new EnabledItemsConfig();

    [Label("Tiles"), ReloadRequired] 
    public EnabledTilesConfig EnabledTiles = new EnabledTilesConfig();
    
    [Label("Accessories"), ReloadRequired] 
    public EnabledAccessoriesConfig EnabledAccessories = new EnabledAccessoriesConfig();

    [SeparatePage]
    public class EnabledItemsConfig
    {
        [Label("Auto Hammer"), DefaultValue(true), ReloadRequired] 
        public bool AutoHammer = true;

        [Label("Fill Wand"), DefaultValue(true), ReloadRequired] 
        public bool FillWand = true;

        [Label("Infinite Paint Bucket"), DefaultValue(true), ReloadRequired] 
        public bool InfinitePaintBucket = true;

        [Label("Paint Brush"), DefaultValue(true), ReloadRequired] 
        public bool PaintBrush = true;

        [Label("Spectre Paint Brush"), DefaultValue(true), ReloadRequired] 
        public bool SpectrePaintBrush = true;

        [Label("Multi Wand"), DefaultValue(true), ReloadRequired] 
        public bool MultiWand = true;

        [Label("Mirror Wand"), DefaultValue(true), ReloadRequired] 
        public bool MirrorWand = true;

        [Label("Shapes Drawer"), DefaultValue(true), ReloadRequired] 
        public bool ShapesDrawer = true;

        public override bool Equals(object obj) {
            if (obj is EnabledItemsConfig config)
                return AutoHammer == config.AutoHammer && FillWand == config.FillWand &&
                       InfinitePaintBucket == config.InfinitePaintBucket && PaintBrush == config.PaintBrush &&
                       SpectrePaintBrush == config.SpectrePaintBrush && MultiWand == config.MultiWand &&
                       MirrorWand == config.MirrorWand && ShapesDrawer == config.ShapesDrawer;
            
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return new {
                AutoHammer, FillWand, InfinitePaintBucket, PaintBrush, 
                SpectrePaintBrush, MultiWand, MirrorWand, ShapesDrawer
            }.GetHashCode();
        }
    }

    [SeparatePage]
    public class EnabledTilesConfig
    {
        [Label("Pre Hardmode Crafting Station"), DefaultValue(true), ReloadRequired] 
        public bool PreHMCraftingStation = true;
        
        [Label("Hardmode Crafting Station"), DefaultValue(true), ReloadRequired] 
        public bool HMCraftingStation = true;
        
        [Label("Specialized Crafting Station"), DefaultValue(true), ReloadRequired] 
        public bool SpecCraftingStation = true;
        
        [Label("Themed Furniture Crafting Station"), DefaultValue(true), ReloadRequired] 
        public bool TFCraftingStation = true;
        
        [Label("Multi Crafting Station"), DefaultValue(true), ReloadRequired] 
        public bool MultiCraftingStation = true;
        
        public override bool Equals(object obj) {
            if (obj is EnabledTilesConfig config)
                return PreHMCraftingStation == config.PreHMCraftingStation &&
                       HMCraftingStation == config.HMCraftingStation &&
                       SpecCraftingStation == config.SpecCraftingStation &&
                       TFCraftingStation == config.TFCraftingStation &&
                       MultiCraftingStation == config.MultiCraftingStation;
            
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return new {
                PreHMCraftingStation, HMCraftingStation, SpecCraftingStation, TFCraftingStation, MultiCraftingStation
            }.GetHashCode();
        }
    }
    
    [SeparatePage]
    public class EnabledAccessoriesConfig
    {
        [Label("Build In Peace"), DefaultValue(true), ReloadRequired] 
        public bool BuildInPeace = true;

        [Label("Building Wrench"), DefaultValue(true), ReloadRequired] 
        public bool BuildingWrench = true;

        [Label("Improved Ruler"), DefaultValue(true), ReloadRequired] 
        public bool ImprovedRuler = true;
        
        public override bool Equals(object obj) {
            if (obj is EnabledAccessoriesConfig config)
                return BuildInPeace == config.BuildInPeace &&
                       BuildingWrench == config.BuildingWrench &&
                       ImprovedRuler == config.ImprovedRuler;

            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return new { BuildInPeace, BuildingWrench, ImprovedRuler }.GetHashCode();
        }
    }
}