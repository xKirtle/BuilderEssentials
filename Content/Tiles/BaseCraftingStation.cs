using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Content.Tiles;

[Autoload(false)]
public abstract class BaseCraftingStation : ModTile
{
    public override string Texture => "BuilderEssentials/Assets/Tiles/" + GetType().Name;

    public virtual string DisplayName { get; protected set; }
    public virtual Color MapColor { get; protected set; }

    public override void SetStaticDefaults() {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileShine[Type] = 0;
        Main.tileShine2[Type] = true;
        Main.tileWaterDeath[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.WaterDeath = true;
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;

        SetTileObjectData();

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

        ModTranslation translation = CreateMapEntryName();
        translation.SetDefault(DisplayName);
        AddMapEntry(MapColor, translation);

        AdjTiles = AdjacentTiles();
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {

        Vector2 player = Main.LocalPlayer.Center;
        Vector2 tile = new Vector2(i, j).ToWorldCoordinates();

        float distance = Math.Min(Math.Abs(Vector2.Subtract(player, tile).Length()), 500f);
        float fluctuation = 1 - distance / 500;
        if (Framing.GetTileSafely(i, j).TileType == Type) {
            r = (float) MapColor.R / 255f * fluctuation;
            g = (float) MapColor.G / 255f * fluctuation;
            b = (float) MapColor.B / 255f * fluctuation;
        }
    }

    public abstract void SetTileObjectData();

    public abstract int[] AdjacentTiles();
}