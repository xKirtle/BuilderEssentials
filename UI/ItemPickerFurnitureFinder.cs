using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BuilderEssentials.UI
{
    public class ItemPickerFurnitureFinder
    {
        public static void FindFurniture(int tileType, int frame, ref Item item)
        {
            if (tileType == TileID.Chairs)
            {
                switch (frame)
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
            if (tileType == TileID.WorkBenches)
            {
                switch (frame)
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
            if (tileType == TileID.Platforms)
            {
                switch (frame)
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
                        Main.NewText("Blue Dungeon Platform?");
                        break;
                    case 7:
                        Main.NewText("Ping Dungeon Platform?");
                        break;
                    case 8:
                        Main.NewText("Green Dungeon Platform?");
                        break;
                    case 9:
                        item.SetDefaults(ItemID.MetalShelf);
                        break;
                    case 10:
                        item.SetDefaults(ItemID.BrassShelf);
                        break;

                        //extracted 1.4 textures?


                }
            }
        }
    }
}
