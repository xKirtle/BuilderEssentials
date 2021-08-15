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
    public class UIModSystem : ModSystem
    {
        private GameTime lastUpdateUiGameTime;
        internal static UserInterface GameUserInterface;
        internal static GameUIState GameUIState;
        internal static UserInterface UIUserInterface;
        internal static UIUIState UIUIState;
        
        public override void Load()
        {
            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                GameUIState = new GameUIState();
                GameUIState.Activate();
                GameUserInterface = new UserInterface();
                GameUserInterface.SetState(GameUIState);

                UIUIState = new UIUIState();
                UIUIState.Activate();
                UIUserInterface = new UserInterface();
                UIUserInterface.SetState(UIUIState);
            }
        }

        public override void Unload()
        {
            GameUserInterface = null;
            GameUIState = null;
            UIUserInterface = null;
            UIUIState = null;
        }
        
        public override void UpdateUI(GameTime gameTime)
        {
            lastUpdateUiGameTime = gameTime;
            if (GameUserInterface?.CurrentState != null)
                GameUserInterface.Update(gameTime);
            
            if (UIUserInterface?.CurrentState != null)
                UIUserInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
            HelperMethods.InsertInterfaceLayer(ref layers, "Vanilla: Interface Logic 1", "Builder Essentials: Logic 1", 
                lastUpdateUiGameTime, GameUserInterface, InterfaceScaleType.Game);

            HelperMethods.InsertInterfaceLayer(ref layers, "Vanilla: Cursor", "Builder Essentials: Below Cursor",
                lastUpdateUiGameTime, UIUserInterface, InterfaceScaleType.UI);
        }
    }
}