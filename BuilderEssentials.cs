using BuilderEssentials.UI;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using BuilderEssentials.Utilities;

namespace BuilderEssentials
{
    public class BuilderEssentials : Mod
    {
        public static Texture2D BuildingModeOff;
        public static Texture2D BuildingModeOn;
        public static List<Texture2D> CreativeWheelElements;
        public static List<Texture2D> CWAutoHammerElements;
        public static List<Texture2D> PaintColors;
        public static List<Texture2D> PaintTools;
        public static List<Texture2D> WandWheelElements;
        public static List<Texture2D> AutoHammerElements;
        internal static BasePanel BasePanel;
        internal static TransparentSelectionUI TransparentSelectionUI;
        internal static UserInterface UserInterface;
        internal static UserInterface TransparentSelectionInterface;
        internal static ModHotKey ToggleBuildingMode;
        internal static ModHotKey IncreaseFillToolSize;
        internal static ModHotKey DecreaseFillToolSize;

        public static bool autoReplaceStack;
        public static bool validMirrorWand;

        public override void Load()
        {
            ToggleBuildingMode = RegisterHotKey("Toggle Building Mode", "N");
            IncreaseFillToolSize = RegisterHotKey("Increase Fill Size Selection", "I");
            DecreaseFillToolSize = RegisterHotKey("Decrease Fill Tool Selection", "O");

            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                LoadTextures();

                TransparentSelectionUI = new TransparentSelectionUI();
                TransparentSelectionUI.Activate();
                TransparentSelectionInterface = new UserInterface();
                ShowExperimentalInterface();

                UserInterface = new UserInterface();
                BasePanel = new BasePanel();
                BasePanel.Activate();
                ShowUserInterface();
            }
        }

        public void LoadTextures()
        {
            BuildingModeOn = GetTexture("Textures/UIElements/BuildingModeOn");
            BuildingModeOff = GetTexture("Textures/UIElements/BuildingModeOff");

            CreativeWheelElements = new List<Texture2D>(5);
            CreativeWheelElements.Add(GetTexture("Textures/UIElements/CreativeWheel/CWColorPicker"));
            CreativeWheelElements.Add(GetTexture("Textures/UIElements/CreativeWheel/CWInfinitePlacement"));
            CreativeWheelElements.Add(GetTexture("Textures/UIElements/CreativeWheel/CWAutoHammer"));
            CreativeWheelElements.Add(GetTexture("Textures/UIElements/CreativeWheel/CWPlacementAnywhere"));
            CreativeWheelElements.Add(GetTexture("Textures/UIElements/CreativeWheel/CWInfinitePickupRange"));

            CWAutoHammerElements = new List<Texture2D>(6);
            for (int i = 0; i < 6; i++)
                CWAutoHammerElements.Add(GetTexture("Textures/UIElements/CreativeWheel/CWAutoHammer" + i));

            PaintColors = new List<Texture2D>(31);
            for (int i = 0; i < 31; i++)
                PaintColors.Add(GetTexture("Textures/UIElements/Paint/Paint" + i));

            PaintTools = new List<Texture2D>(3);
            for (int i = 0; i < 3; i++)
                PaintTools.Add(GetTexture("Textures/UIElements/Paint/PaintTool" + i));

            WandWheelElements = new List<Texture2D>(6);
            for (int i = 0; i < 6; i++)
                WandWheelElements.Add(GetTexture("Textures/UIElements/WandWheel/WandWheel" + i));

            AutoHammerElements = new List<Texture2D>(6);
            for (int i = 0; i < 6; i++)
                AutoHammerElements.Add(GetTexture("Textures/UIElements/AutoHammer/AH" + i));
        }

        public override void Unload()
        {
            //Hotkeys
            ToggleBuildingMode = null;
            IncreaseFillToolSize = null;
            DecreaseFillToolSize = null;

            //TODO: Unload static fields

            //UI
            BasePanel = null;
            TransparentSelectionUI = null;
            UserInterface = null;
            TransparentSelectionInterface = null;
            AutoHammerWheel.AutoHammerWheelPanel = null;
            CreativeWheel.CreativeWheelPanel = null;
            CreativeWheel.CreativeWheelElements = null;
            MultiWandWheel.MultiWandWheelPanel = null;
            MultiWandWheel.WandWheelElements = null;
            PaintWheel.PaintWheelPanel = null;
            TransparentSelectionUI.transparentSelectionUI = null;

            //Textures
            UnloadTextures();
        }

        private void UnloadTextures()
        {
            BuildingModeOn = null;
            BuildingModeOff = null;

            for (int i = 0; i < CreativeWheelElements?.Count; i++)
                CreativeWheelElements[i] = null;

            for (int i = 0; i < CWAutoHammerElements?.Count; i++)
                CWAutoHammerElements[i] = null;

            for (int i = 0; i < PaintColors?.Count; i++)
                PaintColors[i] = null;

            for (int i = 0; i < PaintTools?.Count; i++)
                PaintTools[i] = null;

            for (int i = 0; i < WandWheelElements?.Count; i++)
                WandWheelElements[i] = null;

            for (int i = 0; i < AutoHammerElements?.Count; i++)
                AutoHammerElements[i] = null;
        }

        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (UserInterface?.CurrentState != null)
                UserInterface.Update(gameTime);

            if (TransparentSelectionInterface?.CurrentState != null)
                TransparentSelectionInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
            int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(
                    "Builder Essentials: TransparentSelection",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && UserInterface?.CurrentState != null)
                        {
                            TransparentSelectionInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.Game));
            }

            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(
                    "Builder Essentials: UserInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && UserInterface?.CurrentState != null)
                        {
                            UserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }
        }

        public static void ShowUserInterface() => UserInterface?.SetState(BasePanel);
        public static void HideUserInterface() => UserInterface?.SetState(null);
        public static void ShowExperimentalInterface() => TransparentSelectionInterface?.SetState(TransparentSelectionUI);
        public static void HideExperimentalInterface() => TransparentSelectionInterface?.SetState(null);

        public override void PreSaveAndQuit()
        {
            //Makes sure player leaves as non Building Mode
            if (!BuildingMode.IsNormalAccessories)
                BuildingMode.ToggleBuildingMode();
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup workbench = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Workbench", new int[]
            {
                ItemID.BlueDungeonWorkBench,
                ItemID.BoneWorkBench,
                ItemID.BorealWoodWorkBench,
                ItemID.CactusWorkBench,
                ItemID.CrystalWorkbench,
                ItemID.DynastyWorkBench,
                ItemID.EbonwoodWorkBench,
                ItemID.FleshWorkBench,
                ItemID.FrozenWorkBench,
                ItemID.GlassWorkBench,
                ItemID.GoldenWorkbench,
                ItemID.GothicWorkBench,
                ItemID.GraniteWorkBench,
                ItemID.GreenDungeonWorkBench,
                ItemID.HoneyWorkBench,
                ItemID.LihzahrdWorkBench,
                ItemID.LivingWoodWorkBench,
                ItemID.MarbleWorkBench,
                ItemID.MartianWorkBench,
                ItemID.MeteoriteWorkBench,
                ItemID.MushroomWorkBench,
                ItemID.ObsidianWorkBench,
                ItemID.PalmWoodWorkBench,
                ItemID.PearlwoodWorkBench,
                ItemID.PinkDungeonWorkBench,
                ItemID.PumpkinWorkBench,
                ItemID.RichMahoganyWorkBench,
                ItemID.ShadewoodWorkBench,
                ItemID.SkywareWorkbench,
                ItemID.SlimeWorkBench,
                ItemID.SpookyWorkBench,
                ItemID.SteampunkWorkBench,
                ItemID.WorkBench
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Workbenches", workbench);

            RecipeGroup chairs = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Chair", new int[]
            {
                ItemID.BlueDungeonChair,
                ItemID.BoneChair,
                ItemID.BorealWoodChair,
                ItemID.CactusChair,
                ItemID.CrystalChair,
                ItemID.DynastyChair,
                ItemID.EbonwoodChair,
                ItemID.FleshChair,
                ItemID.FrozenChair,
                ItemID.GlassChair,
                ItemID.GoldenChair,
                ItemID.GothicChair,
                ItemID.GraniteChair,
                ItemID.GreenDungeonChair,
                ItemID.HoneyChair,
                ItemID.LihzahrdChair,
                ItemID.LivingWoodChair,
                ItemID.MarbleChair,
                ItemID.MartianHoverChair,
                ItemID.MeteoriteChair,
                ItemID.MushroomChair,
                ItemID.ObsidianChair,
                ItemID.PalmWoodChair,
                ItemID.PearlwoodChair,
                ItemID.PineChair,
                ItemID.PinkDungeonChair,
                ItemID.PumpkinChair,
                ItemID.RichMahoganyChair,
                ItemID.ShadewoodChair,
                ItemID.SkywareChair,
                ItemID.SlimeChair,
                ItemID.SpookyChair,
                ItemID.SteampunkChair,
                ItemID.WoodenChair
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Chairs", chairs);

            RecipeGroup tables = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Table", new int[]
            {
                ItemID.BlueDungeonTable,
                ItemID.BoneTable,
                ItemID.BorealWoodTable,
                ItemID.CactusTable,
                ItemID.CrystalTable,
                ItemID.DynastyTable,
                ItemID.EbonwoodTable,
                ItemID.FleshTable,
                ItemID.FrozenTable,
                ItemID.GlassTable,
                ItemID.GoldenTable,
                ItemID.GothicTable,
                ItemID.GraniteTable,
                ItemID.GreenDungeonTable,
                ItemID.HoneyTable,
                ItemID.LihzahrdTable,
                ItemID.LivingWoodTable,
                ItemID.MarbleTable,
                ItemID.MartianTable,
                ItemID.MeteoriteTable,
                ItemID.MushroomTable,
                ItemID.ObsidianTable,
                ItemID.PalmWoodTable,
                ItemID.PearlwoodTable,
                ItemID.PineTable,
                ItemID.PinkDungeonTable,
                ItemID.PumpkinTable,
                ItemID.RichMahoganyTable,
                ItemID.ShadewoodTable,
                ItemID.SkywareTable,
                ItemID.SlimeTable,
                ItemID.SpookyTable,
                ItemID.SteampunkTable,
                ItemID.WoodenTable
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Tables", tables);

            RecipeGroup furnaces = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Furnace", new int[]
            {
                ItemID.Furnace,
                ItemID.Hellforge
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Furnaces", furnaces);

            RecipeGroup anvils = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Anvil", new int[]
            {
                ItemID.IronAnvil,
                ItemID.LeadAnvil,
                ItemID.MythrilAnvil,
                ItemID.OrichalcumAnvil
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Anvils", anvils);

            RecipeGroup alchemy = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Bottle/Alchemy item", new int[]
            {
                ItemID.Bottle,
                ItemID.PinkVase,
                ItemID.PinkDungeonVase,
                ItemID.Mug,
                ItemID.DynastyCup,
                ItemID.HoneyCup,
                ItemID.SteampunkCup,
                ItemID.AlchemyTable
        });
            RecipeGroup.RegisterGroup("BuilderEssentials:AlchemyStations", alchemy);

            RecipeGroup sinks = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Sink", new int[]
            {
                ItemID.MetalSink,
                ItemID.BlueDungeonSink,
                ItemID.BoneSink,
                ItemID.BorealWoodSink,
                ItemID.CactusSink,
                ItemID.CrystalSink,
                ItemID.DynastySink,
                ItemID.EbonwoodSink,
                ItemID.FleshSink,
                ItemID.FrozenSink,
                ItemID.GlassSink,
                ItemID.GoldenSink,
                ItemID.GraniteSink,
                ItemID.GreenDungeonSink,
                ItemID.HoneySink,
                ItemID.LihzahrdSink,
                ItemID.LivingWoodSink,
                ItemID.MarbleSink,
                ItemID.MartianSink,
                ItemID.MeteoriteSink,
                ItemID.MushroomSink,
                ItemID.ObsidianSink,
                ItemID.PalmWoodSink,
                ItemID.PearlwoodSink,
                ItemID.PinkDungeonSink,
                ItemID.PumpkinSink,
                ItemID.RichMahoganySink,
                ItemID.ShadewoodSink,
                ItemID.SkywareSink,
                ItemID.SlimeSink,
                ItemID.SpookySink,
                ItemID.SteampunkSink,
                ItemID.WoodenSink
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Sinks", sinks);

            RecipeGroup cookingPots = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cooking Pot", new int[]
            {
                ItemID.CookingPot,
                ItemID.Cauldron
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:CookingPots", cookingPots);

            RecipeGroup forge = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Forge", new int[]
            {
                ItemID.AdamantiteForge,
                ItemID.TitaniumForge
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Forge", forge);

            RecipeGroup bookcase = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Bookcase", new int[]
            {
                ItemID.BlueDungeonBookcase,
                ItemID.BoneBookcase,
                ItemID.BorealWoodBookcase,
                ItemID.CactusBookcase,
                ItemID.CrystalBookCase,
                ItemID.DynastyBookcase,
                ItemID.EbonwoodBookcase,
                ItemID.FleshBookcase,
                ItemID.FrozenBookcase,
                ItemID.GlassBookcase,
                ItemID.GoldenBookcase,
                ItemID.GothicBookcase,
                ItemID.GraniteBookcase,
                ItemID.GreenDungeonBookcase,
                ItemID.HoneyBookcase,
                ItemID.LihzahrdBookcase,
                ItemID.LivingWoodBookcase,
                ItemID.MarbleBookcase,
                ItemID.MeteoriteBookcase,
                ItemID.MushroomBookcase,
                ItemID.ObsidianBookcase,
                ItemID.PalmWoodBookcase,
                ItemID.PearlwoodBookcase,
                ItemID.PinkDungeonBookcase,
                ItemID.PumpkinBookcase,
                ItemID.RichMahoganyBookcase,
                ItemID.ShadewoodBookcase,
                ItemID.SkywareBookcase,
                ItemID.SlimeBookcase,
                ItemID.SpookyBookcase,
                ItemID.SteampunkBookcase,
                ItemID.Bookcase
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Bookcase", bookcase);

            RecipeGroup hardmodeAnvils = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Hardmode Anvil", new int[]
            {
                ItemID.MythrilAnvil,
                ItemID.OrichalcumAnvil
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:HardmodeAnvils", hardmodeAnvils);

            RecipeGroup campfires = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Campfire", new int[]
            {
                ItemID.Campfire,
                ItemID.BoneCampfire,
                ItemID.CursedCampfire,
                ItemID.DemonCampfire,
                ItemID.FrozenCampfire,
                ItemID.IchorCampfire,
                ItemID.RainbowCampfire,
                ItemID.UltraBrightCampfire
            });
            RecipeGroup.RegisterGroup("BuilderEssentials:Campfires", campfires);
        }
    }
}