using System.Collections.Generic;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials
{
    public class UIModSystem : ModSystem
    {
        private GameTime lastUpdateUiGameTime;
        internal static UserInterface BaseUserInterface;
        internal static BaseUIState BaseUIState;
        internal static UserInterface SecUserInterface;
        internal static SecondaryUIState SecUIState;
        
        public override void Load()
        {
            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                BaseUIState = new BaseUIState();
                BaseUIState.Activate();
                BaseUserInterface = new UserInterface();
                BaseUserInterface.SetState(BaseUIState);

                SecUIState = new SecondaryUIState();
                SecUIState.Activate();
                SecUserInterface = new UserInterface();
                SecUserInterface.SetState(SecUIState);
            }
        }

        public override void Unload()
        {
            BaseUserInterface = null;
            BaseUIState = null;
            SecUserInterface = null;
            SecUIState = null;
        }
        
        public override void UpdateUI(GameTime gameTime)
        {
            lastUpdateUiGameTime = gameTime;
            if (BaseUserInterface?.CurrentState != null)
                BaseUserInterface.Update(gameTime);
            
            if (SecUserInterface?.CurrentState != null)
                SecUserInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
            int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 1"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer("Loadouts: Base UI",
                    delegate
                    {
                        if (lastUpdateUiGameTime != null && BaseUserInterface?.CurrentState != null)
                            BaseUserInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                        return true;
                    },
                    InterfaceScaleType.Game));
            }
            
            interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer("Builder Essentials: BelowCursor",
                    delegate
                    {
                        if (lastUpdateUiGameTime != null && SecUserInterface?.CurrentState != null)
                            SecUserInterface?.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }
    }
}