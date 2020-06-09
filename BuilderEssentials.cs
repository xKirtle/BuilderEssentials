using BuilderEssentials.UI;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace BuilderEssentials
{
    public class BuilderEssentials : Mod
    {
        public static Texture2D BuildingModeOff;
        public static Texture2D BuildingModeOn;
        public static bool UIOpen;
        internal static BasePanel BasePanel;
        internal static UserInterface UserInterface;

        public void LoadTextures()
        {
            BuildingModeOff = this.GetTexture("UI/Elements/BuildingModeOff");
            BuildingModeOn = this.GetTexture("UI/Elements/BuildingModeOn");
        }

        public override void Load()
        {
            LoadTextures();

            if (!Main.dedServ)
            {
                UserInterface = new UserInterface();

                BasePanel = new BasePanel();
                BasePanel.Activate();

                //ShowMyUI();
            }
        }

        public override void Unload()
        {
            BasePanel = null;
        }

        private GameTime _lastUpdateUiGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (UserInterface?.CurrentState != null)
                UserInterface.Update(gameTime);

            if (Main.playerInventory == true && !UIOpen)
                ShowMyUI();
            else if (Main.playerInventory == false && UIOpen)
                HideMyUI();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "MyMod: UserInterface",
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

        public static void ShowMyUI()
        {
            UserInterface?.SetState(BasePanel);
            UIOpen = true;
        }

        public static void HideMyUI()
        {
            UserInterface?.SetState(null);
            UIOpen = false;
        }
    }
}