using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        public static void FindFurniture(Tile tile, ref Item item)
        {
            switch (tile.type)
            {
                case TileID.Chairs:
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
                    break;
                case TileID.WorkBenches:
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
                    break;
                case TileID.Platforms:
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
                    break;
                case 21: //Chests
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
                    break;
                case TileID.DemonAltar:
                    switch (tile.frameX / 56)
                    {
                        //Maybe add my own item to allow for it to be picked up?
                        default:
                            break;
                    }
                    break;
                case TileID.Candles:
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
                    break;
                case TileID.Chandeliers:
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
                    }
                    break;
                case TileID.HangingLanterns:
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
                    break;
                case TileID.Beds:
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
                    break;
                case TileID.Pianos:
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
                    break;
                case TileID.Dressers:
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
                    break;
                case 89: //Sofas
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
                        case 3:
                            item.SetDefaults(ItemID.RichMahoganySofa);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.PearlwoodSofa);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.ShadewoodSofa);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.BlueDungeonSofa);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.GreenDungeonSofa);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.PinkDungeonSofa);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.GoldenSofa);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.ObsidianSofa);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.BoneSofa);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.CactusSofa);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.SpookySofa);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.SkywareSofa);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.HoneySofa);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.SteampunkSofa);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.MushroomSofa);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.GlassSofa);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.PumpkinSofa);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.LihzahrdSofa);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.PalmWoodBench);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.PalmWoodSofa);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.MushroomBench);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.BorealWoodSofa);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.SlimeSofa);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.FleshSofa);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.FrozenSofa);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.LivingWoodSofa);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.MartianSofa);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.MeteoriteSofa);
                            break;
                        case 31:
                            item.SetDefaults(ItemID.GraniteSofa);
                            break;
                        case 32:
                            item.SetDefaults(ItemID.MarbleSofa);
                            break;
                        case 33:
                            item.SetDefaults(ItemID.CrystalSofaHowDoesThatEvenWork);
                            break;
                        case 34:
                            item.SetDefaults(ItemID.DynastySofa);
                            break;
                    }
                    break;
                case TileID.Bathtubs:
                    switch (tile.frameY / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.Bathtub);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.CactusBathtub);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.EbonwoodBathtub);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.FleshBathtub);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.GlassBathtub);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.FrozenBathtub);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.RichMahoganyBathtub);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.PearlwoodBathtub);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.LihzahrdBathtub);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.SkywareBathtub);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.SpookyBathtub);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.HoneyBathtub);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.SteampunkBathtub);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.LivingWoodBathtub);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.ShadewoodBathtub);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.BoneBathtub);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.DynastyBathtub);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.PalmWoodBathtub);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.MushroomBathtub);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.BorealWoodBathtub);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.SlimeBathtub);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.BlueDungeonBathtub);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.GreenDungeonBathtub);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.PinkDungeonBathtub);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.PumpkinBathtub);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.ObsidianBathtub);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.GoldenBathtub);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.MartianBathtub);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.MeteoriteBathtub);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.GraniteBathtub);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.MarbleBathtub);
                            break;
                        case 31:
                            item.SetDefaults(ItemID.CrystalBathtub);
                            break;
                    }
                    break;
                case TileID.Lamps:
                    switch (tile.frameY / 54)
                    {
                        case 0:
                            item.SetDefaults(ItemID.TikiTorch);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.CactusLamp);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.EbonwoodLamp);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.FleshLamp);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.GlassLamp);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.FrozenLamp);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.RichMahoganyLamp);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.PearlwoodLamp);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.LihzahrdLamp);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.SkywareLamp);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.SpookyLamp);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.HoneyLamp);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.SteampunkLamp);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.LivingWoodLamp);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.ShadewoodLamp);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.GoldenLamp);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.BoneLamp);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.DynastyLamp);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.PalmWoodLamp);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.MushroomLamp);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.BorealWoodLamp);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.SlimeLamp);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.PumpkinLamp);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.ObsidianLamp);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.BlueDungeonLamp);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.GreenDungeonLamp);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.PinkDungeonLamp);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.MartianLamppost);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.MeteoriteLamp);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.MarbleLamp);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.CrystalLamp);
                            break;
                    }
                    break;
                case TileID.CookingPots:
                    switch (tile.frameX / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.CookingPot);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.Cauldron);
                            break;
                    }
                    break;
                case TileID.Candelabras:
                    switch (tile.frameY / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.Candelabra);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.CactusCandelabra);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.EbonwoodCandelabra);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.FleshCandelabra);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.HoneyCandelabra);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.SteampunkCandelabra);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.GlassCandelabra);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.RichMahoganyCandelabra);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.PearlwoodCandelabra);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.FrozenCandelabra);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.LihzahrdCandelabra);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.SkywareCandelabra);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.SpookyCandelabra);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.LivingWoodCandelabra);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.ShadewoodCandelabra);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.GoldenCandelabra);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.BoneCandelabra);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.DynastyCandelabra);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.PalmWoodCandelabra);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.MushroomCandelabra);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.BorealWoodCandelabra);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.SlimeCandelabra);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.BlueDungeonCandelabra);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.GreenDungeonCandelabra);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.PinkDungeonCandelabra);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.ObsidianCandelabra);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.PumpkinCandelabra);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.MartianTableLamp);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.MeteoriteCandelabra);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.GraniteCandelabra);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.MarbleCandelabra);
                            break;
                        case 31:
                            item.SetDefaults(ItemID.CrystalCandelabra);
                            break;
                    }
                    break;
                case TileID.Bookcases:
                    switch (tile.frameX / 70)
                    {
                        case 0:
                            item.SetDefaults(ItemID.Bookcase);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.BlueDungeonBookcase);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.GreenDungeonBookcase);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.PinkDungeonBookcase);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.ObsidianBookcase);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.GothicBookcase);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.CactusBookcase);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.EbonwoodBookcase);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.FleshBookcase);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.HoneyBookcase);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.SteampunkBookcase);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.GlassBookcase);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.RichMahoganyBookcase);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.PearlwoodBookcase);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.SpookyBookcase);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.SkywareBookcase);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.LihzahrdBookcase);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.FrozenBookcase);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.LivingWoodBookcase);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.ShadewoodBookcase);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.GoldenBookcase);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.BoneBookcase);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.DynastyBookcase);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.PalmWoodBookcase);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.MushroomBookcase);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.BorealWoodBookcase);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.SlimeBookcase);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.PumpkinBookcase);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.MartianHolobookcase);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.MeteoriteBookcase);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.GraniteBookcase);
                            break;
                        case 31:
                            item.SetDefaults(ItemID.MarbleBookcase);
                            break;
                        case 32:
                            item.SetDefaults(ItemID.CrystalBookCase);
                            break;
                    }
                    break;
                case TileID.GrandfatherClocks:
                    switch (tile.frameX / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.GrandfatherClock);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.DynastyClock);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.GoldenClock);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.GlassClock);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.HoneyBlock);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.SteampunkClock);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.BorealWoodClock);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.SlimeClock);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.BoneClock);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.CactusClock);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.EbonwoodClock);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.FrozenClock);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.LihzahrdClock);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.LivingWoodClock);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.RichMahoganyClock);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.FleshClock);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.MushroomClock);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.ObsidianClock);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.PalmWoodClock);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.PearlwoodClock);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.PumpkinClock);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.ShadewoodClock);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.SpookyClock);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.SkywareClock);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.MartianAstroClock);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.MeteoriteClock);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.GraniteClock);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.MarbleClock);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.CrystalClock);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.SkywareClock2);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.DungeonClockBlue);
                            break;
                        case 31:
                            item.SetDefaults(ItemID.DungeonClockGreen);
                            break;
                        case 32:
                            item.SetDefaults(ItemID.DungeonClockPink);
                            break;
                    }
                    break;
                case TileID.AdamantiteForge:
                    switch (tile.frameX / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.AdamantiteForge);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.TitaniumForge);
                            break;
                    }
                    break;
                case TileID.MythrilAnvil:
                    switch (tile.frameX / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.MythrilAnvil);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.OrichalcumAnvil);
                            break;
                    }
                    break;
                case TileID.PressurePlates:
                    switch (tile.frameY / 18)
                    {
                        case 0:
                            item.SetDefaults(ItemID.RedPressurePlate);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.GreenPressurePlate);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.GrayPressurePlate);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.BrownPressurePlate);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.BluePressurePlate);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.YellowPressurePlate);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.LihzahrdPressurePlate);
                            break;
                    }
                    break;
                case TileID.Traps:
                    switch (tile.frameY / 18)
                    {
                        case 0:
                            item.SetDefaults(ItemID.DartTrap);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.SuperDartTrap);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.FlameTrap);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.SpikyBallTrap);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.SpearTrap);
                            break;
                    }
                    break;
                case TileID.MusicBoxes:
                    switch (tile.frameY / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.MusicBoxOverworldDay);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.MusicBoxEerie);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.MusicBoxNight);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.MusicBoxTitle);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.MusicBoxUnderground);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.MusicBoxBoss1);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.MusicBoxJungle);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.MusicBoxCorruption);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.MusicBoxUndergroundHallow);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.MusicBoxBoss2);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.MusicBoxSnow);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.MusicBoxSpace);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.MusicBoxCrimson);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.MusicBoxBoss4);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.MusicBoxAltOverworldDay);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.MusicBoxRain);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.MusicBoxIce);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.MusicBoxDesert);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.MusicBoxOcean);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.MusicBoxDungeon);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.MusicBoxPlantera);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.MusicBoxBoss5);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.MusicBoxTemple);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.MusicBoxEclipse);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.MusicBoxMushrooms);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.MusicBoxPumpkinMoon);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.MusicBoxAltUnderground);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.MusicBoxFrostMoon);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.MusicBoxUndergroundCrimson);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.MusicBoxLunarBoss);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.MusicBoxMartians);
                            break;
                        case 31:
                            item.SetDefaults(ItemID.MusicBoxPirates);
                            break;
                        case 32:
                            item.SetDefaults(ItemID.MusicBoxHell);
                            break;
                        case 33:
                            item.SetDefaults(ItemID.MusicBoxTowers);
                            break;
                        case 34:
                            item.SetDefaults(ItemID.MusicBoxGoblins);
                            break;
                        case 35:
                            item.SetDefaults(ItemID.MusicBoxSandstorm);
                            break;
                        case 36:
                            item.SetDefaults(ItemID.MusicBoxDD2);
                            break;
                    }
                    break;
                case TileID.Timers:
                    switch (tile.frameX / 18)
                    {
                        case 0:
                            item.SetDefaults(ItemID.Timer1Second);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.Timer3Second);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.Timer5Second);
                            break;
                    }
                    break;
                case TileID.Sinks:
                    switch (tile.frameY / 38)
                    {
                        case 0:
                            item.SetDefaults(ItemID.WoodenSink);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.EbonwoodSink);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.RichMahoganySink);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.PearlwoodSink);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.BoneSink);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.FleshSink);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.LivingWoodSink);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.SkywareSink);
                            break;
                        case 8:
                            item.SetDefaults(ItemID.ShadewoodSink);
                            break;
                        case 9:
                            item.SetDefaults(ItemID.LihzahrdSink);
                            break;
                        case 10:
                            item.SetDefaults(ItemID.BlueDungeonSink);
                            break;
                        case 11:
                            item.SetDefaults(ItemID.GreenDungeonSink);
                            break;
                        case 12:
                            item.SetDefaults(ItemID.PinkDungeonSink);
                            break;
                        case 13:
                            item.SetDefaults(ItemID.ObsidianSink);
                            break;
                        case 14:
                            item.SetDefaults(ItemID.MetalSink);
                            break;
                        case 15:
                            item.SetDefaults(ItemID.GlassSink);
                            break;
                        case 16:
                            item.SetDefaults(ItemID.GoldenSink);
                            break;
                        case 17:
                            item.SetDefaults(ItemID.HoneySink);
                            break;
                        case 18:
                            item.SetDefaults(ItemID.SteampunkSink);
                            break;
                        case 19:
                            item.SetDefaults(ItemID.PumpkinSink);
                            break;
                        case 20:
                            item.SetDefaults(ItemID.SpookySink);
                            break;
                        case 21:
                            item.SetDefaults(ItemID.FrozenSink);
                            break;
                        case 22:
                            item.SetDefaults(ItemID.DynastySink);
                            break;
                        case 23:
                            item.SetDefaults(ItemID.PalmWoodSink);
                            break;
                        case 24:
                            item.SetDefaults(ItemID.MushroomSink);
                            break;
                        case 25:
                            item.SetDefaults(ItemID.BorealWoodSink);
                            break;
                        case 26:
                            item.SetDefaults(ItemID.SlimeSink);
                            break;
                        case 27:
                            item.SetDefaults(ItemID.CactusSink);
                            break;
                        case 28:
                            item.SetDefaults(ItemID.MartianSink);
                            break;
                        case 29:
                            item.SetDefaults(ItemID.MeteoriteSink);
                            break;
                        case 30:
                            item.SetDefaults(ItemID.GraniteSink);
                            break;
                        case 31:
                            item.SetDefaults(ItemID.MarbleSink);
                            break;
                        case 32:
                            item.SetDefaults(ItemID.CrystalSink);
                            break;
                    }
                    break;
                case TileID.WaterFountain:
                    switch (tile.frameX / 36)
                    {
                        case 0:
                            item.SetDefaults(ItemID.PureWaterFountain);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.DesertWaterFountain);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.JungleWaterFountain);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.IcyWaterFountain);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.CorruptWaterFountain);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.CrimsonWaterFountain);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.HallowedWaterFountain);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.BloodWaterFountain);
                            break;
                    }
                    break;
                case TileID.Campfire:
                    switch (tile.frameX / 54)
                    {
                        case 0:
                            item.SetDefaults(ItemID.Campfire);
                            break;
                        case 1:
                            item.SetDefaults(ItemID.CursedCampfire);
                            break;
                        case 2:
                            item.SetDefaults(ItemID.DemonCampfire);
                            break;
                        case 3:
                            item.SetDefaults(ItemID.FrozenCampfire);
                            break;
                        case 4:
                            item.SetDefaults(ItemID.IchorCampfire);
                            break;
                        case 5:
                            item.SetDefaults(ItemID.RainbowCampfire);
                            break;
                        case 6:
                            item.SetDefaults(ItemID.UltraBrightCampfire);
                            break;
                        case 7:
                            item.SetDefaults(ItemID.BoneCampfire);
                            break;
                    }
                    break;
            }
        }
    }
}
