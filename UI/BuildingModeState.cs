using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI
{
    class BuildingModeState : UIState
    {
        public static UIImageButton buildingModeButton;
        public static bool isBuildingModeButtonVisible;
        public static bool Hovering = buildingModeButton != null && buildingModeButton.IsMouseHovering && isBuildingModeButtonVisible;
        public override void OnInitialize()
        {
            buildingModeButton = new UIImageButton(BuilderEssentials.BuildingModeOff);
            buildingModeButton.Top.Set(40f, 0);
            buildingModeButton.Left.Set(510f, 0);
            buildingModeButton.OnClick += ChangeAccessories_OnClick;
            buildingModeButton.SetVisibility(0f, 0f);

            if (!BuilderEssentials.LoadoutsEnabled)
                Append(buildingModeButton);
        }

        public void ChangeAccessories_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (isBuildingModeButtonVisible)
                BuildingMode.ToggleBuildingMode();
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.playerInventory && !isBuildingModeButtonVisible)
            {
                buildingModeButton.SetVisibility(1f, .4f);
                isBuildingModeButtonVisible = true;
            }
            else if (!Main.playerInventory && isBuildingModeButtonVisible)
            {
                buildingModeButton.SetVisibility(0f, 0f);
                isBuildingModeButtonVisible = false;
            }
        }
    }
}
