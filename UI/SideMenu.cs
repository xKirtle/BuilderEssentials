using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI
{
    class SideMenu : UIPanel
    {
        public static UIPanel SideMenuPanel;
        private static float SideMenuWidth;
        private static float SideMenuHeight;
        public static bool IsSideMenuUIVisible;
        public static bool SideMenuUIOpen;
        public static bool Hovering = SideMenuPanel != null && SideMenuPanel.IsMouseHovering && IsSideMenuUIVisible;

        public static UIPanel CreateSideMenuPanel(BasePanel basePanel)
        {
            SideMenuWidth = 250f;
            SideMenuHeight = 250f;

            SideMenuPanel = new UIPanel();
            SideMenuPanel.VAlign = 0f;
            SideMenuPanel.HAlign = 0f;
            SideMenuPanel.Width.Set(SideMenuWidth, 0);
            SideMenuPanel.Height.Set(SideMenuHeight, 0);
            SideMenuPanel.Left.Set(Main.screenWidth / 2 - SideMenuWidth, 0);
            SideMenuPanel.Top.Set(Main.screenHeight / 2 - SideMenuHeight, 0);
            SideMenuPanel.BorderColor = Color.Red;
            SideMenuPanel.BackgroundColor = Color.Black;
            SideMenuPanel.OnClick += (__, _) => { };

            SideMenuArrow.SideMenuArrowPanel.Remove();
            basePanel.Append(SideMenuPanel);

            return SideMenuPanel;
        }
    }

    class SideMenuArrow : UIPanel
    { 
        public static UIPanel SideMenuArrowPanel;
        private static float SideMenuArrowWidth;
        private static float SideMenuArrowHeight;
        public static bool IsSideMenuArrowUIVisible;
        public static bool SideMenuArrowUIOpen;
        public static bool Hovering = SideMenuArrowPanel != null && SideMenuArrowPanel.IsMouseHovering && IsSideMenuArrowUIVisible;

        public static UIPanel CreateSideMenuArrowPanel(BasePanel basePanel)
        {
            SideMenuArrowWidth = 30f;
            SideMenuArrowHeight = 44f;

            SideMenuArrowPanel = new UIPanel();
            SideMenuArrowPanel.VAlign = 0f;
            SideMenuArrowPanel.HAlign = 0f;
            SideMenuArrowPanel.Width.Set(SideMenuArrowWidth, 0);
            SideMenuArrowPanel.Height.Set(SideMenuArrowHeight, 0);
            SideMenuArrowPanel.Left.Set(-4f, 0);
            SideMenuArrowPanel.Top.Set(Main.screenHeight / 2, 0);
            SideMenuArrowPanel.BorderColor = Color.Transparent;
            SideMenuArrowPanel.BackgroundColor = Color.Transparent;
            SideMenuArrowPanel.OnClick += (__, _) => { SideMenu.CreateSideMenuPanel(basePanel); };

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
