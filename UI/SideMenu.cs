using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.UI
{
    class SideMenu
    {
        public static UIPanel SideMenuPanel;
        private static float SideMenuWidth;
        private static float SideMenuHeight;
        public static bool IsSideMenuUIVisible;
        public static bool SideMenuUIOpen;
        public static bool Hovering = SideMenuPanel != null && SideMenuPanel.IsMouseHovering && IsSideMenuUIVisible;

        private static bool isFillEnabled = false;
        private static bool isMirrorEnabled = false;

        public static UIPanel CreateSideMenuPanel(BasePanel basePanel)
        {
            SideMenuWidth = 294f;
            SideMenuHeight = 175f;

            SideMenuPanel = new UIPanel();
            SideMenuPanel.VAlign = 0f;
            SideMenuPanel.HAlign = 0f;
            SideMenuPanel.Width.Set(SideMenuWidth, 0);
            SideMenuPanel.Height.Set(SideMenuHeight, 0);
            SideMenuPanel.Left.Set(10f, 0);
            SideMenuPanel.Top.Set(Main.screenHeight / 2 - SideMenuHeight / 2, 0);
            SideMenuPanel.BorderColor = Color.Red;
            SideMenuPanel.BackgroundColor = Color.Transparent;
            SideMenuPanel.OnClick += (__, _) => { };

            CreateLayout(basePanel);

            SideMenuArrow.SideMenuArrowPanel.Remove();
            basePanel.Append(SideMenuPanel);

            return SideMenuPanel;
        }

        private static void CreateLayout(BasePanel basePanel)
        {
            //Background
            UIImage SMBackground = new UIImage(GetTexture("BuilderEssentials/Textures/UIElements/ShapesMenu/Background"));
            SMBackground.VAlign = 0f;
            SMBackground.HAlign = 0f;
            SMBackground.Width.Set(0, 0);
            SMBackground.Height.Set(0f, 0);
            SMBackground.Left.Set(-12f, 0);
            SMBackground.Top.Set(-12f, 0);
            SideMenuPanel.Append(SMBackground);

            //Cross to Close Menu
            UIImage closeMenuCross = new UIImage(GetTexture("BuilderEssentials/Textures/UIElements/ShapesMenu/CloseCross"));
            closeMenuCross.VAlign = 0f;
            closeMenuCross.HAlign = 0f;
            closeMenuCross.Width.Set(19f, 0);
            closeMenuCross.Height.Set(19f, 0);
            closeMenuCross.Left.Set(SideMenuWidth - 35f, 0);
            closeMenuCross.Top.Set(-7f, 0);
            closeMenuCross.OnClick += (__, _) =>
            {
                SideMenuPanel.Remove();
                SideMenuArrow.CreateSideMenuArrowPanel(basePanel);
            };
            SideMenuPanel.Append(closeMenuCross);

            List<UIImage> ShapesMenuList = new List<UIImage>(8);
            bool[] optionSelected = new bool[8];

            string textureLocation = "BuilderEssentials/Textures/UIElements/ShapesMenu/";

            for (int i = 0; i < 4; i++) //Top Row
            {
                int index = i;
                UIImage tempUIImage = new UIImage(GetTexture(textureLocation + $"SM{i + 1}"));
                tempUIImage.VAlign = 0f;
                tempUIImage.HAlign = 0f;
                tempUIImage.Width.Set(36f, 0);
                tempUIImage.Height.Set(36f, 0);
                tempUIImage.Left.Set(i * 70 + 3f, 0);
                tempUIImage.Top.Set(30f, 0);
                tempUIImage.OnClick += (__, _) => SMClicked(index);

                optionSelected[index] = false;
                ShapesMenuList.Add(tempUIImage);
                SideMenuPanel.Append(tempUIImage);
            }

            for (int i = 0; i < 4; i++) //Bottom Row
            {
                int index = i;
                UIImage tempUIImage = new UIImage(GetTexture(textureLocation + $"SM{i + 5}"));
                tempUIImage.VAlign = 0f;
                tempUIImage.HAlign = 0f;
                tempUIImage.Width.Set(36f, 0);
                tempUIImage.Height.Set(36f, 0);
                tempUIImage.Left.Set(i * 70 + 3f, 0);
                tempUIImage.Top.Set(95f, 0);
                tempUIImage.OnClick += (__, _) => SMClicked(index + 4);

                optionSelected[index + 4] = false;
                ShapesMenuList.Add(tempUIImage);
                SideMenuPanel.Append(tempUIImage);
            }

            void SMClicked(int index)
            {
                //TODO: ADD SOME VISUAL CLUE THAT USER CANT SELECT HALF CIRCLES/ELIPSES WHILE SQUARE/RECTANGLE IS SELECTED
                optionSelected[index] = !optionSelected[index];
                isMirrorEnabled = optionSelected[4];
                isFillEnabled = optionSelected[7];

                for (int i = 0; i < 4; i++)
                {
                    if (i == index)
                        continue;

                    if (index < 4)
                    optionSelected[i] = false;

                    int tempIndex = i;
                    SetUIImage(tempIndex);
                    SetUIImage(tempIndex);
                }

                for (int i = 4; i < 8; i++)
                {
                    if (i == index)
                        continue;

                    //Allows only one at a time (half circles/elipses)
                    if (optionSelected[5] && optionSelected[6])
                    {
                        if (index != 5)
                            optionSelected[5] = false;
                        else
                            optionSelected[6] = false;
                    }

                    //Half circles/elipses cannot be toggled if selected is a square/rectangle
                    if ((index == 5 || index == 6) && (optionSelected[2] || optionSelected[3]))
                        optionSelected[index] = false;

                    //Disables half circles/elipses if square/rectangle is selected
                    if (index == 2 || index == 3)
                    {
                        optionSelected[5] = false;
                        optionSelected[6] = false;
                    }

                    int tempIndex = i;
                    SetUIImage(tempIndex);
                    SetUIImage(tempIndex);
                }

                SetUIImage(index);

                void SetUIImage(int someIndex)
                {
                    string texture = textureLocation + "SM";
                    if (isFillEnabled)
                        texture += "Fill";
                    if (optionSelected[someIndex])
                        texture += "Alternate";
                    if (isMirrorEnabled && (someIndex == 5 || someIndex == 6))
                        texture += "Mirror";
                    texture += $"{someIndex + 1}";

                    ShapesMenuList[someIndex].SetImage(GetTexture(texture));
                }
            }
        }
    }

    class SideMenuArrow
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
            UIImage sideArrow = new UIImage(GetTexture("BuilderEssentials/Textures/UIElements/SideMenu"));
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