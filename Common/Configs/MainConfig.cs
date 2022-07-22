using System.ComponentModel;
using System.Runtime.Serialization;
using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.ModLoader.Config;

namespace BuilderEssentials.Common.Configs;

public class MainConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Label("Max Undo Times")]
    [Tooltip("The maximum amount of placements the game will remember and be able to undo")]
    [Range(0, 100), DefaultValue(20), DrawTicks]
    public int MaxUndoNum = 20;

    public override void OnChanged() {
        ShapesUIState.UpdateMaxUndoNum(MaxUndoNum);
    }
    
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context) {
        MaxUndoNum = Utils.Clamp(MaxUndoNum, 0, 100);
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
    }
}