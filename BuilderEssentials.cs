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
                // BaseUserInterface = new UserInterface();
                // BaseUIState = new BaseUIState();
                // BaseUIState.Activate();
                // BaseUserInterface.SetState(BaseUIState);

                ItemsUserInterface = new UserInterface();
                ItemsUIState = new ItemsUIState();
                ItemsUIState.Activate();
                ItemsUserInterface.SetState(ItemsUIState);
            }
        }

        // internal UserInterface BaseUserInterface;
        // internal BaseUIState BaseUIState;
        internal UserInterface ItemsUserInterface;
        internal ItemsUIState ItemsUIState;
        private GameTime lastUpdateUIGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            lastUpdateUIGameTime = gameTime;
            if (ItemsUserInterface?.CurrentState != null)
                ItemsUserInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
            int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 2"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(
                    "Builder Essentials: UserInterface",
                    delegate
                    {
                        if (lastUpdateUIGameTime != null)
                        {
                            //BaseUserInterface?.Draw(Main.spriteBatch, lastUpdateUIGameTime);
                            ItemsUserInterface?.Draw(Main.spriteBatch, lastUpdateUIGameTime);
                        }

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
            HelperMethods.CreateRecipeGroup(woods, "Woods");
        }
    }
}