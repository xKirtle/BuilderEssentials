using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BuilderEssentials.UI
{
    public class BasePanel : UIState
    {
        public static UIImageButton buildingModeButton;
        public static Texture2D buttonTexture;
        public static UIPanel creativeWheelPanel;
        public static bool isBuildingModeButtonVisible;
        public static bool isCreativeWheelVisible;
        public static bool creativeWheelUIOpen;
        public override void OnInitialize()
        {
            buttonTexture = BuilderEssentials.BuildingModeOff;
            buildingModeButton = new UIImageButton(buttonTexture);
            buildingModeButton.VAlign = 0f; //0.03f
            buildingModeButton.HAlign = 0f; //0.272f
            buildingModeButton.Top.Set(40f, 0);
            buildingModeButton.Left.Set(510f, 0);
            buildingModeButton.OnClick += ChangeAccessories_OnClick;
            buildingModeButton.SetVisibility(0f, 0f);
            Append(buildingModeButton);
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.playerInventory == true && !isBuildingModeButtonVisible)
            {
                buildingModeButton.SetVisibility(1f, .4f);
                isBuildingModeButtonVisible = true;
            }
            else if (Main.playerInventory == false && isBuildingModeButtonVisible)
            {
                buildingModeButton.SetVisibility(0f, 0f);
                isBuildingModeButtonVisible = false;
            }

            if (buildingModeButton.IsMouseHovering && isBuildingModeButtonVisible)
                Main.LocalPlayer.mouseInterface = true;

            if (creativeWheelUIOpen && !isCreativeWheelVisible)
            {
                //Main.NewText("Open");
                creativeWheelPanel = CreativeWheel.CreateCreativeWheelPanel(Main.mouseX, Main.mouseY,this);
                isCreativeWheelVisible = true;
            }
            else if (!creativeWheelUIOpen && isCreativeWheelVisible)
            {
                //Main.NewText("Closed");
                creativeWheelPanel.Remove();
                isCreativeWheelVisible = false;
            }
        }

        public void ChangeAccessories_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (isBuildingModeButtonVisible)
                BuildingMode.BuildingModeAccessoriesToggle();
        }
    }
}
