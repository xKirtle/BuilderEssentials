using System.Collections.Generic;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials
{
    public class BuilderEssentials : Mod
    {
        internal static ModHotKey IncreaseFillToolSize;
        internal static ModHotKey DecreaseFillToolSize;

        public override void Load()
        {
            IncreaseFillToolSize = RegisterHotKey("Increase Fill Tool Selection Size", "I");
            DecreaseFillToolSize = RegisterHotKey("Decrease Fill Tool Selection Size", "O");

            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                UserInterfaceLogic1 = new UserInterface();
                UIStateLogic1 = new UIStateLogic1();
                UIStateLogic1.Activate();
                UserInterfaceLogic1.SetState(UIStateLogic1);

                UserInterfaceLogic4 = new UserInterface();
                UIStateLogic4 = new UIStateLogic4();
                UIStateLogic4.Activate();
                UserInterfaceLogic4.SetState(UIStateLogic4);
            }
        }

        internal UserInterface UserInterfaceLogic1;
        internal UIStateLogic1 UIStateLogic1;
        internal UserInterface UserInterfaceLogic4;
        internal UIStateLogic4 UIStateLogic4;
        private GameTime lastUpdateUIGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            lastUpdateUIGameTime = gameTime;
            
            if (UserInterfaceLogic1?.CurrentState != null)
                UserInterfaceLogic1.Update(gameTime);
            if (UserInterfaceLogic4?.CurrentState != null)
                UserInterfaceLogic4.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
            int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(
                    "Builder Essentials: UserInterfaceLogic1",
                    delegate
                    {
                        if (lastUpdateUIGameTime != null)
                            UserInterfaceLogic1?.Draw(Main.spriteBatch, lastUpdateUIGameTime);

                        return true;
                    },
                    InterfaceScaleType.UI));
            }
            
            interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 4"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(
                    "Builder Essentials: UserInterfaceLogic4",
                    delegate
                    {
                        if (lastUpdateUIGameTime != null)
                            UserInterfaceLogic4?.Draw(Main.spriteBatch, lastUpdateUIGameTime);

                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public override void AddRecipeGroups()
        {
            int[] woods =
            {
                ItemID.Wood, ItemID.RichMahogany, ItemID.Ebonwood, ItemID.Shadewood, ItemID.Pearlwood,
                ItemID.BorealWood, ItemID.PalmWood, ItemID.DynastyWood, ItemID.SpookyWood
            };
            HelperMethods.CreateRecipeGroup(woods, "Wood");

            int[] workbenches =
            {
                ItemID.BlueDungeonWorkBench, ItemID.BoneWorkBench, ItemID.BorealWoodWorkBench, ItemID.CactusWorkBench,
                ItemID.CrystalWorkbench, ItemID.DynastyWorkBench, ItemID.EbonwoodWorkBench, ItemID.FleshWorkBench,
                ItemID.FrozenWorkBench, ItemID.GlassWorkBench, ItemID.GoldenWorkbench, ItemID.GothicWorkBench,
                ItemID.GraniteWorkBench, ItemID.GreenDungeonWorkBench, ItemID.HoneyWorkBench, ItemID.LihzahrdWorkBench,
                ItemID.LivingWoodWorkBench, ItemID.MarbleWorkBench, ItemID.MartianWorkBench, ItemID.MeteoriteWorkBench,
                ItemID.MushroomWorkBench, ItemID.ObsidianWorkBench, ItemID.PalmWoodWorkBench, ItemID.PearlwoodWorkBench,
                ItemID.PinkDungeonWorkBench, ItemID.PumpkinWorkBench, ItemID.RichMahoganyWorkBench,
                ItemID.ShadewoodWorkBench, ItemID.SkywareWorkbench, ItemID.SlimeWorkBench, ItemID.SpookyWorkBench,
                ItemID.SteampunkWorkBench, ItemID.WorkBench
            };
            HelperMethods.CreateRecipeGroup(workbenches, "Workbench");

            int[] chairs =
            {
                ItemID.BlueDungeonChair, ItemID.BoneChair, ItemID.BorealWoodChair, ItemID.CactusChair,
                ItemID.CrystalChair, ItemID.DynastyChair, ItemID.EbonwoodChair, ItemID.FleshChair,
                ItemID.FrozenChair, ItemID.GlassChair, ItemID.GoldenChair, ItemID.GothicChair,
                ItemID.GraniteChair, ItemID.GreenDungeonChair, ItemID.HoneyChair, ItemID.LihzahrdChair,
                ItemID.LivingWoodChair, ItemID.MarbleChair, ItemID.MartianHoverChair, ItemID.MeteoriteChair,
                ItemID.MushroomChair, ItemID.ObsidianChair, ItemID.PalmWoodChair, ItemID.PearlwoodChair,
                ItemID.PineChair, ItemID.PinkDungeonChair, ItemID.PumpkinChair, ItemID.RichMahoganyChair,
                ItemID.ShadewoodChair, ItemID.SkywareChair, ItemID.SlimeChair, ItemID.SpookyChair,
                ItemID.SteampunkChair, ItemID.WoodenChair
            };
            HelperMethods.CreateRecipeGroup(chairs, "Chair");

            int[] tables =
            {
                ItemID.BlueDungeonTable, ItemID.BoneTable, ItemID.BorealWoodTable, ItemID.CactusTable,
                ItemID.CrystalTable, ItemID.DynastyTable, ItemID.EbonwoodTable, ItemID.FleshTable,
                ItemID.FrozenTable, ItemID.GlassTable, ItemID.GoldenTable, ItemID.GothicTable,
                ItemID.GraniteTable, ItemID.GreenDungeonTable, ItemID.HoneyTable, ItemID.LihzahrdTable,
                ItemID.LivingWoodTable, ItemID.MarbleTable, ItemID.MartianTable, ItemID.MeteoriteTable,
                ItemID.MushroomTable, ItemID.ObsidianTable, ItemID.PalmWoodTable, ItemID.PearlwoodTable,
                ItemID.PineTable, ItemID.PinkDungeonTable, ItemID.PumpkinTable, ItemID.RichMahoganyTable,
                ItemID.ShadewoodTable, ItemID.SkywareTable, ItemID.SlimeTable, ItemID.SpookyTable,
                ItemID.SteampunkTable, ItemID.WoodenTable
            };
            HelperMethods.CreateRecipeGroup(tables, "Table");

            int[] furnaces = {ItemID.Furnace, ItemID.Hellforge};
            HelperMethods.CreateRecipeGroup(furnaces, "Furnace");

            int[] anvils = {ItemID.IronAnvil, ItemID.LeadAnvil, ItemID.MythrilAnvil, ItemID.OrichalcumAnvil};
            HelperMethods.CreateRecipeGroup(anvils, "Anvil");

            int[] hardmodeAnvils = {ItemID.MythrilAnvil, ItemID.OrichalcumAnvil};
            HelperMethods.CreateRecipeGroup(hardmodeAnvils, "Hardmode Anvil");
            
            int[] alchemy =
            {
                ItemID.Bottle, ItemID.PinkVase, ItemID.PinkDungeonVase, ItemID.Mug,
                ItemID.DynastyCup, ItemID.HoneyCup, ItemID.SteampunkCup, ItemID.AlchemyTable
            };
            HelperMethods.CreateRecipeGroup(alchemy, "Bottle/Alchemy Item");

            int[] sinks =
            {
                ItemID.MetalSink, ItemID.BlueDungeonSink, ItemID.BoneSink, ItemID.BorealWoodSink,
                ItemID.CactusSink, ItemID.CrystalSink, ItemID.DynastySink, ItemID.EbonwoodSink,
                ItemID.FleshSink, ItemID.FrozenSink, ItemID.GlassSink, ItemID.GoldenSink,
                ItemID.GraniteSink, ItemID.GreenDungeonSink, ItemID.HoneySink, ItemID.LihzahrdSink,
                ItemID.LivingWoodSink, ItemID.MarbleSink, ItemID.MartianSink, ItemID.MeteoriteSink,
                ItemID.MushroomSink, ItemID.ObsidianSink, ItemID.PalmWoodSink, ItemID.PearlwoodSink,
                ItemID.PinkDungeonSink, ItemID.PumpkinSink, ItemID.RichMahoganySink, ItemID.ShadewoodSink,
                ItemID.SkywareSink, ItemID.SlimeSink, ItemID.SpookySink, ItemID.SteampunkSink, ItemID.WoodenSink
            };
            HelperMethods.CreateRecipeGroup(sinks, "Sink");

            int[] cookingPots = {ItemID.CookingPot, ItemID.Cauldron};
            HelperMethods.CreateRecipeGroup(cookingPots, "Cooking Pot");

            int[] forges = {ItemID.AdamantiteForge, ItemID.TitaniumForge};
            HelperMethods.CreateRecipeGroup(forges, "Forge");

            int[] bookcases =
            {
                ItemID.BlueDungeonBookcase, ItemID.BoneBookcase, ItemID.BorealWoodBookcase, ItemID.CactusBookcase,
                ItemID.CrystalBookCase, ItemID.DynastyBookcase, ItemID.EbonwoodBookcase, ItemID.FleshBookcase,
                ItemID.FrozenBookcase, ItemID.GlassBookcase, ItemID.GoldenBookcase, ItemID.GothicBookcase,
                ItemID.GraniteBookcase, ItemID.GreenDungeonBookcase, ItemID.HoneyBookcase, ItemID.LihzahrdBookcase,
                ItemID.LivingWoodBookcase, ItemID.MarbleBookcase, ItemID.MeteoriteBookcase, ItemID.MushroomBookcase,
                ItemID.ObsidianBookcase, ItemID.PalmWoodBookcase, ItemID.PearlwoodBookcase, ItemID.PinkDungeonBookcase,
                ItemID.PumpkinBookcase, ItemID.RichMahoganyBookcase, ItemID.ShadewoodBookcase, ItemID.SkywareBookcase,
                ItemID.SlimeBookcase, ItemID.SpookyBookcase, ItemID.SteampunkBookcase, ItemID.Bookcase
            };
            HelperMethods.CreateRecipeGroup(bookcases, "Bookcase");

            int[] campfires =
            {
                ItemID.Campfire, ItemID.BoneCampfire, ItemID.CursedCampfire, ItemID.DemonCampfire,
                ItemID.FrozenCampfire, ItemID.IchorCampfire, ItemID.RainbowCampfire, ItemID.UltraBrightCampfire
            };
            HelperMethods.CreateRecipeGroup(campfires, "Campfire");

            int[] magicMirrors = {ItemID.MagicMirror, ItemID.IceMirror};
            HelperMethods.CreateRecipeGroup(magicMirrors, "Magic Mirror");
        }
    }
}