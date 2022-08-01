using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Content.Tiles;

[Autoload(true)]
public class MultiCraftingStation : BaseCraftingStation
{
    public override string DisplayName => "Multi Crafting Station";
    public override Color MapColor => Color.White * 0.65f;

    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().EnabledTiles.MultiCraftingStation;

    public override void SetTileObjectData() {
        DustType = DustID.Sandnado;

        AnimationFrameHeight = 74;
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

    public override int[] AdjacentTiles() => Enumerable.Range(0, TileLoader.TileCount).ToArray();

    public override void AnimateTile(ref int frame, ref int frameCounter) {
        frameNumber = frame;
        //new frame each 5 ticks
        if (++frameCounter > 5) {
            frameCounter = 0;
            if (++frame > 27)
                frame = 0;
        }
    }

    private static Color[] colors = { new(36, 151, 64), new(191, 55, 64), new(185, 92, 31), new(39, 137, 205) };

    private int colorCounter = 0;
    private int frameNumber = 0;
    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData) {
        if (drawData.tileFrameX == 36 && drawData.tileFrameY == 54 && (frameNumber + 5) % 7 == 0) {
            Vector2 player = Main.LocalPlayer.Center;
            Vector2 tile = new Vector2(i, j).ToWorldCoordinates();
            float distance = Math.Min(Math.Abs(Vector2.Subtract(player, tile).Length()), 500f);
            float fluctuation = 1 - distance / 500;

            Vector2 coord = new(i * 16, j * 16);
            //TODO: Change dust colos based on which gem is hitting
            for (int k = 0; k < 16 * fluctuation; k++) {
                Dust.NewDustPerfect(coord + new Vector2(k, Main.rand.Next(-8, 8)), 158,
                    new Vector2(Math.Max(0.3f, 0.6f * (k % 4)), -0.3f), 0, colors[colorCounter]);

                Dust.NewDustPerfect(coord - new Vector2(k, Main.rand.Next(-8, 8)), 158,
                    new Vector2(Math.Min(-0.3f, -0.6f * (k % 4)), -0.3f), 0, colors[colorCounter]);
            }

            colorCounter = (colorCounter + 1) % colors.Length;
        }
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16,
        32, 16, ModContent.ItemType<MultiCraftingItem>());
}