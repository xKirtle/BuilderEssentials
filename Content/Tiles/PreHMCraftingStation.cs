using System;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Content.Tiles;

[Autoload(true)]
public class PreHMCraftingStation : BaseCraftingStation
{
    public override string DisplayName => "Pre Hardmode Crafting Station";
    public override Color MapColor => new Color(36, 151, 64);

    public override bool IsLoadingEnabled(Mod mod)
        => ModContent.GetInstance<MainConfig>().EnabledTiles.PreHMCraftingStation;

    public override void SetTileObjectData() {
        DustType = DustID.CursedTorch;

        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.Origin = new Point16(1, 2);
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.Width = 4;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
        TileObjectData.addTile(Type);
    }

    public override int[] AdjacentTiles() => new int[] {
        TileID.WorkBenches, TileID.Furnaces, TileID.Hellforge,
        TileID.Anvils, TileID.Bottles, TileID.Pots, TileID.AlchemyTable, TileID.Sinks, TileID.Sawmill, TileID.Loom,
        TileID.Chairs, TileID.Tables, TileID.Tables2, TileID.CookingPots, TileID.TinkerersWorkbench, TileID.ImbuingStation,
        TileID.DyeVat, TileID.HeavyWorkBench, TileID.DemonAltar
    };


    public override void KillMultiTile(int i, int j, int frameX, int frameY) {
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<PreHMCraftingItem>());
    }
}