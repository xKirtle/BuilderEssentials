using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI
{
    class SideMenu : UIPanel
    {
        //TODO: IMPLEMENT RECALCULATE() SO THAT RESIZING THE WINDOW DOESN'T MESS WITH THE PLACEMENT OF THE UIPANEL

        public static UIPanel SideMenuArrowPanel;
        private static float SideMenuArrowlWidth;
        private static float SideMenuArrowHeight;
        public static bool IsSideMenuArrowUIVisible;
        public static bool SideMenuArrowUIOpen;
        public static bool Hovering = SideMenuArrowPanel != null && SideMenuArrowPanel.IsMouseHovering && IsSideMenuArrowUIVisible;

        public static UIPanel CreateSideMenuArrowPanel(BasePanel basePanel)
        {
            SideMenuArrowlWidth = 30f;
            SideMenuArrowHeight = 44f;

            SideMenuArrowPanel = new UIPanel();
            SideMenuArrowPanel.VAlign = 0f;
            SideMenuArrowPanel.HAlign = 0f;
            SideMenuArrowPanel.Width.Set(SideMenuArrowlWidth, 0);
            SideMenuArrowPanel.Height.Set(SideMenuArrowHeight, 0);
            SideMenuArrowPanel.Left.Set(Main.screenWidth - 15, 0);
            SideMenuArrowPanel.Top.Set(Main.screenHeight / 2, 0);
            SideMenuArrowPanel.BorderColor = Color.Transparent;
            SideMenuArrowPanel.BackgroundColor = Color.Transparent;
            SideMenuArrowPanel.OnClick += (__, _) => { Main.NewText("Clicked"); };

            CreateLayout();
            basePanel.Append(SideMenuArrowPanel);

            return SideMenuArrowPanel;
        }

        private static void CreateLayout()
        {
            UIImage sideArrow = new UIImage(BuilderEssentials.SideMenu);
            sideArrow.VAlign = 0f;
            sideArrow.HAlign = 0f;
            sideArrow.Width.Set(15f, 0);
            sideArrow.Height.Set(44f, 0);
            sideArrow.Left.Set(-9f, 0);
            sideArrow.Top.Set(-11f, 0);

            SideMenuArrowPanel.Append(sideArrow);
        }
    }
}
