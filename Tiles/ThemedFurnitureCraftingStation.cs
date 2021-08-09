using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Tiles
{
    internal class ThemedFurnitureCraftingStation : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Themed Furniture Crafting Station");
            AddMapEntry(new Color(249, 163, 27), name);

            AdjTiles = new int[] { TileID.BoneWelder, TileID.GlassKiln, TileID.HoneyDispenser, TileID.IceMachine,
                TileID.LivingLoom, TileID.SkyMill, TileID.Solidifier, TileID.FleshCloningVat, TileID.SteampunkBoiler,
                TileID.LihzahrdFurnace, TileID.WaterDrip, TileID.Waterfall, TileID.LavaDrip, TileID.Lavafall,
                TileID.HoneyDrip, TileID.Honeyfall, TileID.SnowCloud };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.ThemedFurnitureCraftingStation>());
        }
    }
}