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
    class MultiCraftingStation : ModTile
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

            animationFrameHeight = 74;
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
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Multi Crafting Station");
            AddMapEntry(new Color(92, 92, 92), name);

            //Can't use Modded ItemTypes here?
            adjTiles = new int[] { TileID.WorkBenches, TileID.Furnaces, TileID.Hellforge, TileID.Anvils, TileID.AlchemyTable, TileID.Sinks,
            TileID.Sawmill, TileID.Loom, TileID.Chairs, TileID.Tables, TileID.Tables2, TileID.CookingPots, TileID.TinkerersWorkbench,
            TileID.ImbuingStation, TileID.DyeVat, TileID.HeavyWorkBench, TileID.DemonAltar, TileID.MythrilAnvil,
            TileID.AdamantiteForge, TileID.Bookcases, TileID.CrystalBall, TileID.Autohammer, TileID.LunarCraftingStation,
            TileID.Kegs, TileID.Blendomatic, TileID.MeatGrinder, TileID.BoneWelder, TileID.GlassKiln, TileID.HoneyDispenser,
            TileID.IceMachine, TileID.LivingLoom, TileID.SkyMill, TileID.Solidifier, TileID.FleshCloningVat, TileID.SteampunkBoiler,
            TileID.LihzahrdFurnace, TileID.WaterDrip, TileID.Waterfall, TileID.LavaDrip, TileID.Lavafall,
            TileID.HoneyDrip, TileID.Honeyfall, TileID.Campfire, TileID.Extractinator, TileID.SnowCloud, TileID.Tombstones};
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            //new frame each 5 ticks
            if (++frameCounter > 5)
            {
                frameCounter = 0;
                if (++frame > 27)
                    frame = 0;
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, ItemType<Items.Placeable.MultiCraftingStation>());
        }
    }
}
