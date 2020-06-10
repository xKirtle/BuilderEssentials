using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Tiles
{
    class MultiCraftingStation : ModTile
    {
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.Width = 3;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.WorkBenches, TileID.Furnaces, TileID.Hellforge, TileID.Anvils, TileID.AlchemyTable, TileID.Sinks, 
				TileID.Sawmill, TileID.Loom, TileID.Chairs, TileID.Tables, TileID.Tables2, TileID.CookingPots, TileID.TinkerersWorkbench,
				TileID.ImbuingStation, TileID.DyeVat, TileID.HeavyWorkBench, TileID.DemonAltar, TileID.MythrilAnvil, 
				TileID.AdamantiteForge, TileID.Bookcases, TileID.CrystalBall, TileID.Autohammer, TileID.LunarCraftingStation,
				TileID.Kegs, TileID.Blendomatic, TileID.MeatGrinder, TileID.BoneWelder, TileID.GlassKiln, TileID.HoneyDispenser,
				TileID.IceMachine, TileID.LivingLoom, TileID.SkyMill, TileID.Solidifier, TileID.FleshCloningVat, TileID.SteampunkBoiler,
				TileID.LihzahrdFurnace, TileID.WaterDrip, TileID.Waterfall, TileID.LavaDrip, TileID.Lavafall,
				TileID.HoneyDrip, TileID.Honeyfall, TileID.Campfire, TileID.Extractinator};
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ItemType<Items.Placeable.MultiCraftingStation>());
		}
	}
}
