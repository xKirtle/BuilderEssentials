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
public class HMCraftingStation : BaseCraftingStation
{
    public override string DisplayName => "Hardmode Crafting Station";
    public override Color MapColor => new Color(191, 55, 64);

    public override bool IsLoadingEnabled(Mod mod) {
        return ModContent.GetInstance<MainConfig>().EnabledTiles.HMCraftingStation;
    }

    public override void SetTileObjectData() {
        DustType = DustID.RedTorch;

        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.Origin = new Point16(1, 3);
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
        TileObjectData.newTile.Height = 4;
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.Width = 4;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
        TileObjectData.addTile(Type);
    }

    public override int[] AdjacentTiles() {
        return new int[] {
            TileID.MythrilAnvil, TileID.AdamantiteForge,
            TileID.Bookcases, TileID.CrystalBall, TileID.Autohammer, TileID.LunarCraftingStation
        };
    }


    public override void KillMultiTile(int i, int j, int frameX, int frameY) {
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<HMCraftingItem>());
    }
}