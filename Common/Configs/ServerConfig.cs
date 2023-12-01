using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using BuilderEssentials.Content.UI;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader.Config;

namespace BuilderEssentials.Common.Configs;

public class ServerConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Range(0, 100), DefaultValue(20)]
    public int MaxUndoNum = 20;

    [Range(1, 100), DefaultValue(20)]
    public int InfinitePickupRangeFloat = 20;

    [JsonIgnore]
    public int InfinitePickupRangeValue => (int) ((float) InfinitePickupRangeFloat / 100f * MaxPickupRange);
    private const int MaxPickupRange = 3500;
    
    [OptionStrings(new string[] { "Under Inventory", "Bottom Left Corner"})]
    [DrawTicks, DefaultValue("Under Inventory")]
    public string SquirrelBuilderPosition;

    [JsonIgnore]
    public int SquirrelBuilderPositionIndex => (new List<string> { "Under Inventory", "Bottom Left Corner" }).IndexOf(SquirrelBuilderPosition);

    public override void OnChanged() {
        ShapesUIState.UpdateMaxUndoNum(MaxUndoNum);
        
        if (ToggleableItemsUIState.GetInstance() != null)
            ToggleableItemsUIState.GetUIPanel<ShapesDrawerMenuPanel>().SetPositioningFromConfig();
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context) {
        MaxUndoNum = Utils.Clamp(MaxUndoNum, 0, 100);
        InfinitePickupRangeFloat = Utils.Clamp(InfinitePickupRangeFloat, 0, 100);
    }

    [Header("ToggleContentHeader")]
    
    [ReloadRequired]
    public EnabledItemsConfig EnabledItems = new();

    [ReloadRequired]
    public EnabledTilesConfig EnabledTiles = new();

    [ReloadRequired]
    public EnabledAccessoriesConfig EnabledAccessories = new();

    [ReloadRequired]
    public EnabledUpgradeModulesConfig EnabledUpgradeModules = new();

    [SeparatePage]
    public class EnabledItemsConfig
    {
        [DefaultValue(true), ReloadRequired]
        public bool AutoHammer = true;

        [DefaultValue(true), ReloadRequired]
        public bool FillWand = true;

        [DefaultValue(true), ReloadRequired]
        public bool InfinitePaintBucket = true;

        [DefaultValue(true), ReloadRequired]
        public bool PaintBrush = true;

        [DefaultValue(true), ReloadRequired]
        public bool SpectrePaintBrush = true;

        [DefaultValue(true), ReloadRequired]
        public bool MultiWand = true;

        [DefaultValue(true), ReloadRequired]
        public bool MirrorWand = true;

        [DefaultValue(true), ReloadRequired]
        public bool ShapesDrawer = true;

        public override bool Equals(object obj) {
            if (obj is EnabledItemsConfig config) {
                return AutoHammer == config.AutoHammer && FillWand == config.FillWand &&
                    InfinitePaintBucket == config.InfinitePaintBucket && PaintBrush == config.PaintBrush &&
                    SpectrePaintBrush == config.SpectrePaintBrush && MultiWand == config.MultiWand &&
                    MirrorWand == config.MirrorWand && ShapesDrawer == config.ShapesDrawer;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() => new {
            AutoHammer, FillWand, InfinitePaintBucket, PaintBrush,
            SpectrePaintBrush, MultiWand, MirrorWand, ShapesDrawer
        }.GetHashCode();
    }

    [SeparatePage]
    public class EnabledTilesConfig
    {
        [DefaultValue(true), ReloadRequired]
        public bool PreHMCraftingStation = true;

        [DefaultValue(true), ReloadRequired]
        public bool HMCraftingStation = true;

        [DefaultValue(true), ReloadRequired]
        public bool SpecCraftingStation = true;

        [DefaultValue(true), ReloadRequired]
        public bool TFCraftingStation = true;

        [DefaultValue(true), ReloadRequired]
        public bool MultiCraftingStation = true;

        public override bool Equals(object obj) {
            if (obj is EnabledTilesConfig config) {
                return PreHMCraftingStation == config.PreHMCraftingStation &&
                    HMCraftingStation == config.HMCraftingStation &&
                    SpecCraftingStation == config.SpecCraftingStation &&
                    TFCraftingStation == config.TFCraftingStation &&
                    MultiCraftingStation == config.MultiCraftingStation;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() => new {
            PreHMCraftingStation, HMCraftingStation, SpecCraftingStation, TFCraftingStation,
            MultiCraftingStation
        }.GetHashCode();
    }

    [SeparatePage]
    public class EnabledAccessoriesConfig
    {
        [DefaultValue(true), ReloadRequired]
        public bool BuildInPeace = true;

        [DefaultValue(true), ReloadRequired]
        public bool BuildingWrench = true;

        [DefaultValue(true), ReloadRequired]
        public bool ImprovedRuler = true;

        public override bool Equals(object obj) {
            if (obj is EnabledAccessoriesConfig config) {
                return BuildInPeace == config.BuildInPeace &&
                    BuildingWrench == config.BuildingWrench &&
                    ImprovedRuler == config.ImprovedRuler;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() => new { BuildInPeace, BuildingWrench, ImprovedRuler }.GetHashCode();
    }

    [SeparatePage]
    public class EnabledUpgradeModulesConfig
    {
        [JsonIgnore]
        public bool[] EnabledUpgrades
            => new bool[] { FastPlacement, InfiniteRange, InfinitePlacement, InfinitePickupRange };

        [DefaultValue(true), ReloadRequired]
        public bool FastPlacement = true;

        [DefaultValue(true), ReloadRequired]
        public bool InfiniteRange = true;

        [DefaultValue(true), ReloadRequired]
        public bool InfinitePlacement = true;

        [DefaultValue(true), ReloadRequired]
        public bool InfinitePickupRange = true;

        public override bool Equals(object obj) {
            if (obj is EnabledUpgradeModulesConfig config) {
                return FastPlacement == config.FastPlacement &&
                    InfiniteRange == config.InfiniteRange &&
                    InfinitePlacement == config.InfinitePlacement &&
                    InfinitePickupRange == config.InfinitePickupRange;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() => new { FastPlacement, InfiniteRange, InfinitePlacement, InfinitePickupRange }.GetHashCode();
    }
}