﻿using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Utilities
{
    public class FurnitureFinder
    {
        public static void FindFurniture(Tile tile, ref Item item)
        {
            //This is gonna take me ages, WIP
            if (tile.type == TileID.Chairs)
            {
                switch (tile.frameY / 40)
                {
                    case 0:
                        item.SetDefaults(ItemID.WoodenChair);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.Toilet);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.EbonwoodChair);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.FleshChair);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.MushroomChair);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.SkywareChair);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.ShadewoodChair);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.LihzahrdChair);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.BlueDungeonChair);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.GreenDungeonChair);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.PinkDungeonChair);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.ObsidianChair);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.GothicChair);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.GlassChair);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.GoldenChair);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.GoldenToilet);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.HoneyChair);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.SteampunkChair);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.PumpkinChair);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.SpookyChair);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.PineChair);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.DynastyChair);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.FrozenChair);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.PalmWoodChair);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.BarStool);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.BorealWoodChair);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.SlimeChair);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.MartianHoverChair);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.MeteoriteChair);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.GraniteChair);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.MarbleChair);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.CrystalChair);
                        break;
                }
            }
            if (tile.type == TileID.WorkBenches)
            {
                switch (tile.frameX / 36)
                {
                    case 0:
                        item.SetDefaults(ItemID.WorkBench);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.EbonwoodWorkBench);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.RichMahoganyWorkBench);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.PearlwoodWorkBench);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.BoneWorkBench);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.CactusWorkBench);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.FleshWorkBench);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.MushroomWorkBench);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.SlimeWorkBench);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.ShadewoodWorkBench);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.LihzahrdWorkBench);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.BlueDungeonWorkBench);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.GreenDungeonWorkBench);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.PinkDungeonWorkBench);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.ObsidianWorkBench);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.GothicWorkBench);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.PumpkinWorkBench);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.SpookyWorkBench);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.DynastyWorkBench);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.HoneyWorkBench);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.FrozenWorkBench);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.SteampunkWorkBench);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.PalmWoodWorkBench);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.BorealWoodWorkBench);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.SkywareWorkbench);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.GlassWorkBench);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.LivingWoodWorkBench);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.MartianWorkBench);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.MeteoriteWorkBench);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.GraniteWorkBench);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.MarbleWorkBench);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.CrystalWorkbench);
                        break;
                    case 32:
                        item.SetDefaults(ItemID.GoldenWorkbench);
                        break;


                }
            }
            if (tile.type == TileID.Platforms)
            {
                switch (tile.frameY / 18)
                {
                    case 0:
                        item.SetDefaults(ItemID.WoodPlatform);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.EbonwoodPlatform);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.RichMahoganyPlatform);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.PearlwoodPlatform);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.BonePlatform);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.ShadewoodPlatform);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.BlueBrickPlatform);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.PinkBrickPlatform);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.GreenBrickPlatform);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.MetalShelf);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.BrassShelf);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.WoodShelf);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.DungeonShelf);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.ObsidianPlatform);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.GlassPlatform);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.PumpkinPlatform);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.SpookyPlatform);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.PalmWoodPlatform);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.MushroomPlatform);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.BorealWoodPlatform);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.SlimePlatform);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.SteampunkPlatform);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.SkywarePlatform);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.LivingWoodPlatform);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.HoneyPlatform);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.CactusPlatform);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.MartianPlatform);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.MeteoritePlatform);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.GranitePlatform);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.MarblePlatform);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.CrystalPlatform);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.GoldenPlatform);
                        break;
                    case 32:
                        item.SetDefaults(ItemID.DynastyPlatform);
                        break;
                    case 33:
                        item.SetDefaults(ItemID.LihzahrdPlatform);
                        break;
                    case 34:
                        item.SetDefaults(ItemID.FleshPlatform);
                        break;
                    case 35:
                        item.SetDefaults(ItemID.FrozenPlatform);
                        break;
                }
            }
            if (tile.type == 21) //Chests. Fake chests are 441
            {
                switch (tile.frameX / 36)
                {
                    case 0:
                        item.SetDefaults(ItemID.Chest);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.GoldChest);
                        break;
                    case 2:
                        //Locked GoldChest?
                        goto case 1;
                    case 3:
                        item.SetDefaults(ItemID.ShadowChest);
                        break;
                    case 4:
                        //Locked Shadow Chest?
                        goto case 3;
                    case 5:
                        item.SetDefaults(ItemID.Barrel);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.TrashCan);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.EbonwoodChest);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.RichMahoganyChest);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.PearlwoodChest);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.IvyChest);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.IceChest);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.LivingWoodChest);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.SkywareChest);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.ShadewoodChest);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.WebCoveredChest);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.LihzahrdChest);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.WaterChest);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.JungleChest);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.CorruptionChest);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.CrimsonChest);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.HallowedChest);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.FrozenChest);
                        break;
                    //case 23-27 are locked biome chests
                    case 23:
                        goto case 18;
                    case 24:
                        goto case 19;
                    case 25:
                        goto case 20;
                    case 26:
                        goto case 21;
                    case 27:
                        goto case 22;
                    case 28:
                        item.SetDefaults(ItemID.DynastyChest);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.HoneyChest);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.SteampunkChest);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.PalmWoodChest);
                        break;
                    case 32:
                        item.SetDefaults(ItemID.MushroomChest);
                        break;
                    case 33:
                        item.SetDefaults(ItemID.BorealWoodChest);
                        break;
                    case 34:
                        item.SetDefaults(ItemID.SlimeChest);
                        break;
                    case 35:
                        item.SetDefaults(ItemID.GreenDungeonChest);
                        break;
                    case 36:
                        //Locked Green Dungeon variation
                        goto case 35;
                    case 37:
                        item.SetDefaults(ItemID.PinkDungeonChest);
                        break;
                    case 38:
                        //Locked Pink Dungeon variation
                        goto case 36;
                    case 39:
                        item.SetDefaults(ItemID.BlueDungeonChest);
                        break;
                    case 40:
                        //Locked Blue Dungeon variation
                        goto case 39;
                    case 41:
                        item.SetDefaults(ItemID.BoneChest);
                        break;
                    case 42:
                        item.SetDefaults(ItemID.CactusChest);
                        break;
                    case 43:
                        item.SetDefaults(ItemID.FleshChest);
                        break;
                    case 44:
                        item.SetDefaults(ItemID.ObsidianChest);
                        break;
                    case 45:
                        item.SetDefaults(ItemID.PumpkinChest);
                        break;
                    case 46:
                        item.SetDefaults(ItemID.SpookyChest);
                        break;
                    case 47:
                        item.SetDefaults(ItemID.GlassChest);
                        break;
                    case 48:
                        item.SetDefaults(ItemID.MartianChest);
                        break;
                    case 49:
                        item.SetDefaults(ItemID.MeteoriteChest);
                        break;
                    case 50:
                        item.SetDefaults(ItemID.MarbleChest);
                        break;
                    case 51:
                        item.SetDefaults(ItemID.CrystalChest);
                        break;
                    case 52:
                        item.SetDefaults(ItemID.GoldenChest);
                        break;
                }
            }
            if (tile.type == TileID.DemonAltar)
            {
                //Maybe add my own item to allow for it to be picked up?
                switch (tile.frameX / 56)
                {
                    default:
                        break;
                }
            }
            if (tile.type == TileID.Candles)
            {
                switch (tile.frameY / 22)
                {
                    case 0:
                        item.SetDefaults(ItemID.Candle);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.BlueDungeonCandle);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.GreenDungeonCandle);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.PinkDungeonCandle);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.CactusCandle);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.EbonwoodCandle);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.FleshCandle);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.GlassCandle);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.FrozenCandle);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.RichMahoganyCandle);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.PearlwoodCandle);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.LihzahrdCandle);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.SkywareCandle);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.PumpkinCandle);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.LivingWoodCandle);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.ShadewoodCandle);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.GoldenCandle);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.DynastyCandle);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.PalmWoodCandle);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.MushroomCandle);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.BorealWoodCandle);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.SlimeCandle);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.HoneyCandle);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.SteampunkCandle);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.SpookyCandle);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.ObsidianCandle);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.MartianHoverCandle);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.MeteoriteCandle);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.GraniteCandle);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.MarbleCandle);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.CrystalCandle);
                        break;


                }
            }
            if (tile.type == TileID.Chandeliers)
            {
                switch (tile.frameY / 54)
                {
                    case 0:
                        if (tile.frameX / 54 == 0)
                            item.SetDefaults(ItemID.CopperChandelier);
                        if (tile.frameX / 54 == 1)
                            item.SetDefaults(ItemID.CrystalChandelier);
                        //1.4 adds more chandeliers here
                        break;
                    case 1:
                        item.SetDefaults(ItemID.SilverChandelier);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.GoldChandelier);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.TinChandelier);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.TungstenChandelier);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.PlatinumChandelier);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.Jackelier);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.CactusChandelier);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.EbonwoodChandelier);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.FleshChandelier);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.HoneyChandelier);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.FrozenChandelier);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.RichMahoganyChandelier);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.PearlwoodChandelier);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.LihzahrdChandelier);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.SkywareChandelier);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.SpookyChandelier);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.GlassChandelier);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.LivingWoodChandelier);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.ShadewoodChandelier);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.GoldenChandelier);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.BoneChandelier);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.DynastyChandelier);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.PalmWoodChandelier);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.MushroomChandelier);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.BorealWoodChandelier);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.SlimeChandelier);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.BlueDungeonChandelier);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.GreenDungeonChandelier);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.PinkDungeonChandelier);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.SteampunkChandelier);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.PumpkinChandelier);
                        break;
                    case 32:
                        item.SetDefaults(ItemID.ObsidianChandelier);
                        break;
                    case 33:
                        item.SetDefaults(ItemID.MartianChandelier);
                        break;
                    case 34:
                        item.SetDefaults(ItemID.MeteoriteChandelier);
                        break;
                    case 35:
                        item.SetDefaults(ItemID.GraniteChandelier);
                        break;
                    case 36:
                        item.SetDefaults(ItemID.MarbleChandelier);
                        break;
                    case 37:
                        item.SetDefaults(ItemID.CrystalChandelier);
                        break;
                        //More in 1.4


                }
            }
            if (tile.type == TileID.HangingLanterns)
            {
                switch (tile.frameY / 36)
                {
                    case 0:
                        item.SetDefaults(ItemID.ChainLantern);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.BrassLantern);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.CagedLantern);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.CarriageLantern);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.AlchemyLantern);
                        break;
                    case 5:
                        //Diabolist Lamp?? No ItemID??
                        item.SetDefaults(1394);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.OilRagSconse);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.StarinaBottle);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.HangingJackOLantern);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.HeartLantern);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.CactusLantern);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.EbonwoodLantern);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.FleshLantern);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.HoneyLantern);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.SteampunkLantern);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.GlassLantern);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.RichMahoganyLantern);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.PearlwoodLantern);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.FrozenLantern);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.LihzahrdLantern);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.SkywareLantern);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.SpookyLantern);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.LivingWoodLantern);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.ShadewoodLantern);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.GoldenLantern);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.BoneLantern);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.DynastyLantern);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.PalmWoodLantern);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.MushroomLantern);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.BorealWoodLantern);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.SlimeLantern);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.PumpkinLantern);
                        break;
                    case 32:
                        item.SetDefaults(ItemID.ObsidianLantern);
                        break;
                    case 33:
                        item.SetDefaults(ItemID.MartianLantern);
                        break;
                    case 34:
                        item.SetDefaults(ItemID.MeteoriteLantern);
                        break;
                    case 35:
                        item.SetDefaults(ItemID.GraniteLantern);
                        break;
                    case 36:
                        item.SetDefaults(ItemID.MarbleLantern);
                        break;
                    case 37:
                        item.SetDefaults(ItemID.CrystalLantern);
                        break;
                }
            }
            if (tile.type == TileID.Beds)
            {
                switch (tile.frameY / 38)
                {
                    case 0:
                        item.SetDefaults(ItemID.Bed);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.EbonwoodBed);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.RichMahoganyBed);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.PearlwoodBed);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.ShadewoodBed);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.BlueDungeonBed);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.GreenDungeonBed);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.PinkDungeonBed);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.ObsidianBed);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.GlassBed);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.HoneyBed);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.SteampunkBed);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.CactusBed);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.FleshBed);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.FrozenBed);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.LihzahrdBed);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.SkywareBed);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.SpookyBed);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.LivingWoodBed);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.BoneBed);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.DynastyBed);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.PalmWoodBed);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.MushroomBed);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.BorealWoodBed);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.SlimeBed);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.PumpkinBed);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.MartianBed);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.MeteoriteBed);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.GraniteBed);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.MarbleBed);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.CrystalBed);
                        break;
                }
            }
            if (tile.type == TileID.Pianos)
            {
                switch (tile.frameX / 54)
                {
                    case 0:
                        item.SetDefaults(ItemID.Piano);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.EbonwoodPiano);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.RichMahoganyPiano);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.PearlwoodPiano);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.ShadewoodPiano);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.LivingWoodPiano);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.FleshPiano);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.FrozenPiano);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.GlassPiano);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.HoneyPiano);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.SteampunkPiano);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.BlueDungeonPiano);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.GreenDungeonPiano);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.PinkDungeonPiano);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.GoldenPiano);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.ObsidianPiano);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.BonePiano);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.CactusPiano);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.SpookyPiano);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.SkywarePiano);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.LihzahrdPiano);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.PalmWoodPiano);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.MushroomPiano);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.BorealWoodPiano);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.SlimePiano);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.PumpkinPiano);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.MartianPiano);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.MeteoritePiano);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.MarblePiano);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.GranitePiano);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.CrystalPiano);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.DynastyPiano);
                        break;
                }
            }
            if (tile.type == TileID.Dressers)
            {
                switch (tile.frameX / 54)
                {
                    case 0:
                        item.SetDefaults(ItemID.Dresser);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.EbonwoodDresser);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.RichMahoganyDresser);
                        break;
                    case 3:
                        item.SetDefaults(ItemID.PearlwoodDresser);
                        break;
                    case 4:
                        item.SetDefaults(ItemID.ShadewoodDresser);
                        break;
                    case 5:
                        item.SetDefaults(ItemID.BlueDungeonDresser);
                        break;
                    case 6:
                        item.SetDefaults(ItemID.GreenDungeonDresser);
                        break;
                    case 7:
                        item.SetDefaults(ItemID.PinkDungeonDresser);
                        break;
                    case 8:
                        item.SetDefaults(ItemID.GoldenDresser);
                        break;
                    case 9:
                        item.SetDefaults(ItemID.ObsidianDresser);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.BoneDresser);
                        break;
                    case 11:
                        item.SetDefaults(ItemID.CactusDresser);
                        break;
                    case 12:
                        item.SetDefaults(ItemID.SpookyDresser);
                        break;
                    case 13:
                        item.SetDefaults(ItemID.SkywareDresser);
                        break;
                    case 14:
                        item.SetDefaults(ItemID.HoneyDresser);
                        break;
                    case 15:
                        item.SetDefaults(ItemID.LihzahrdDresser);
                        break;
                    case 16:
                        item.SetDefaults(ItemID.PalmWoodDresser);
                        break;
                    case 17:
                        item.SetDefaults(ItemID.MushroomDresser);
                        break;
                    case 18:
                        item.SetDefaults(ItemID.BorealWoodDresser);
                        break;
                    case 19:
                        item.SetDefaults(ItemID.SlimeDresser);
                        break;
                    case 20:
                        item.SetDefaults(ItemID.PumpkinDresser);
                        break;
                    case 21:
                        item.SetDefaults(ItemID.SteampunkDresser);
                        break;
                    case 22:
                        item.SetDefaults(ItemID.GlassDresser);
                        break;
                    case 23:
                        item.SetDefaults(ItemID.FleshDresser);
                        break;
                    case 24:
                        item.SetDefaults(ItemID.MartianDresser);
                        break;
                    case 25:
                        item.SetDefaults(ItemID.MeteoriteDresser);
                        break;
                    case 26:
                        item.SetDefaults(ItemID.GraniteDresser);
                        break;
                    case 27:
                        item.SetDefaults(ItemID.MarbleDresser);
                        break;
                    case 28:
                        item.SetDefaults(ItemID.CrystalDresser);
                        break;
                    case 29:
                        item.SetDefaults(ItemID.DynastyDresser);
                        break;
                    case 30:
                        item.SetDefaults(ItemID.FrozenDresser);
                        break;
                    case 31:
                        item.SetDefaults(ItemID.LivingWoodDresser);
                        break;
                }
            }
            if (tile.type == 89) //Sofas
            {
                switch (tile.frameX / 54)
                {
                    case 0:
                        item.SetDefaults(ItemID.Bench);
                        break;
                    case 1:
                        item.SetDefaults(ItemID.Sofa);
                        break;
                    case 2:
                        item.SetDefaults(ItemID.EbonwoodSofa);
                        break;
                }
            }
        }
    }
}
