using System.Collections.Generic;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials
{
    public class BuilderEssentials : Mod
    {
        public override void Load()
        {
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
    }
}