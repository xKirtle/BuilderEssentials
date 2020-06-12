using Terraria;
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
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = false;
            animationFrameHeight = 54;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Width = 2;
            //TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            //TileObjectData.newTile.StyleWrapLimit = 2;
            //TileObjectData.newTile.StyleMultiplier = 2;
            //TileObjectData.newTile.StyleHorizontal = true;
            //TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            //TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            //TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.WorkBenches, TileID.Furnaces, TileID.Hellforge, TileID.Anvils, TileID.AlchemyTable, TileID.Sinks,
                TileID.Sawmill, TileID.Loom, TileID.Chairs, TileID.Tables, TileID.Tables2, TileID.CookingPots, TileID.TinkerersWorkbench,
                TileID.ImbuingStation, TileID.DyeVat, TileID.HeavyWorkBench, TileID.DemonAltar };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ItemType<Items.Placeable.PreHardmodeCraftingStation>());
        }
    }
}
