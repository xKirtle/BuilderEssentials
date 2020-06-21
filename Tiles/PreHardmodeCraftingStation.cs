using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Tiles
{
    class PreHardmodeCraftingStation : ModTile
    {
        public override void SetDefaults()
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
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Pre Hardmode Crafting Station");
            AddMapEntry(new Color(143, 208, 50), name);

            adjTiles = new int[] { TileID.WorkBenches, TileID.Furnaces, TileID.Hellforge, TileID.Anvils, TileID.Bottles, TileID.Pots,
                TileID.AlchemyTable, TileID.Sinks, TileID.Sawmill, TileID.Loom, TileID.Chairs, TileID.Tables, TileID.Tables2,
                TileID.CookingPots, TileID.TinkerersWorkbench, TileID.ImbuingStation, TileID.DyeVat, TileID.HeavyWorkBench,
                TileID.DemonAltar };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ItemType<Items.Placeable.PreHardmodeCraftingStation>());
        }
    }
}