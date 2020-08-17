using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.UI
{
    class ShapesMenu : UIState
    {
        public static ShapesMenu Instance;
        public override void OnInitialize()
        {
            Instance = this;

            CreateArrowPanel();
        }

        #region ArrowPanel
        public static UIPanel ArrowPanel;
        private static float ArrowWidth;
        private static float ArrowHeight;

        public static void CreateArrowPanel()
        {
            ArrowWidth = 30f;
            ArrowHeight = 44f;

            ArrowPanel = new UIPanel();
            ArrowPanel.Width.Set(ArrowWidth, 0);
            ArrowPanel.Height.Set(ArrowHeight, 0);
            ArrowPanel.Left.Set(-4f, 0);
            ArrowPanel.Top.Set(Main.screenHeight / 2, 0);
            ArrowPanel.BorderColor = Color.Transparent;
            ArrowPanel.BackgroundColor = Color.Transparent;
            ArrowPanel.OnClick += (__, _) => { ArrowPanel.Remove(); CreateShapesMenuPanel(); };

            UIImage ArrowPanelImage = new UIImage(GetTexture("BuilderEssentials/Textures/UIElements/SideMenu"));
            ArrowPanelImage.Width.Set(15f, 0);
            ArrowPanelImage.Height.Set(44f, 0);
            ArrowPanelImage.Left.Set(-9f, 0);
            ArrowPanelImage.Top.Set(-11f, 0);

            ArrowPanel.Append(ArrowPanelImage);
            Instance.Append(ArrowPanel);
        }
        #endregion

        #region ShapesMenuPanel
        public static DraggableUIPanel SMPanel;
        private static float SMWidth;
        private static float SMHeight;

        private static bool isFillEnabled = false;
        private static bool isMirrorEnabled = false;
        public static void CreateShapesMenuPanel()
        {
            SMWidth = 294f;
            SMHeight = 175f;

            SMPanel = new DraggableUIPanel();
            SMPanel.Width.Set(SMWidth, 0);
            SMPanel.Height.Set(SMHeight, 0);
            SMPanel.Left.Set(10f, 0);
            SMPanel.Top.Set(Main.screenHeight / 2 - SMHeight / 2, 0);
            SMPanel.BorderColor = Color.Red;
            SMPanel.BackgroundColor = Color.Transparent;
            SMPanel.OnMouseDown += (element, listener) => 
            {
                Vector2 SMPosition = new Vector2(SMPanel.Left.Pixels, SMPanel.Top.Pixels);
                Vector2 clickPos = Vector2.Subtract(element.MousePosition, SMPosition);
                DraggableUIPanel.canDrag = clickPos.Y >= 0 && clickPos.Y <= 25;
            };

            CreateLayout();

            ArrowPanel.Remove();
            Instance.Append(SMPanel);
        }

        private static void CreateLayout()
        {
            //Background
            UIImage SMBackground = new UIImage(GetTexture("BuilderEssentials/Textures/UIElements/ShapesMenu/Background"));
            SMBackground.Width.Set(0, 0);
            SMBackground.Height.Set(0f, 0);
            SMBackground.Left.Set(-12f, 0);
            SMBackground.Top.Set(-12f, 0);
            SMPanel.Append(SMBackground);

            //Cross to Close Menu
            UIImage closeMenuCross = new UIImage(GetTexture("BuilderEssentials/Textures/UIElements/ShapesMenu/CloseCross"));
            closeMenuCross.Width.Set(19f, 0);
            closeMenuCross.Height.Set(19f, 0);
            closeMenuCross.Left.Set(SMWidth - 35f, 0);
            closeMenuCross.Top.Set(-7f, 0);
            closeMenuCross.OnClick += (__, _) => { SMPanel.Remove(); CreateArrowPanel(); };
            SMPanel.Append(closeMenuCross);

            List<UIImage> ShapesMenuList = new List<UIImage>(8);
            bool[] optionSelected = new bool[8];

            string textureLocation = "BuilderEssentials/Textures/UIElements/ShapesMenu/";

            for (int i = 0; i < 4; i++) //Top Row
            {
                int index = i;
                UIImage tempUIImage = new UIImage(GetTexture(textureLocation + $"SM{i + 1}"));
                tempUIImage.Width.Set(36f, 0);
                tempUIImage.Height.Set(36f, 0);
                tempUIImage.Left.Set(i * 70 + 3f, 0);
                tempUIImage.Top.Set(30f, 0);
                tempUIImage.OnClick += (__, _) => SMClicked(index);

                optionSelected[index] = false;
                ShapesMenuList.Add(tempUIImage);
                SMPanel.Append(tempUIImage);
            }

            for (int i = 0; i < 4; i++) //Bottom Row
            {
                int index = i;
                UIImage tempUIImage = new UIImage(GetTexture(textureLocation + $"SM{i + 5}"));
                tempUIImage.Width.Set(36f, 0);
                tempUIImage.Height.Set(36f, 0);
                tempUIImage.Left.Set(i * 70 + 3f, 0);
                tempUIImage.Top.Set(95f, 0);
                tempUIImage.OnClick += (__, _) => SMClicked(index + 4);

                optionSelected[index + 4] = false;
                ShapesMenuList.Add(tempUIImage);
                SMPanel.Append(tempUIImage);
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

                    //Disables half circles/elipses if square/rectangle is selected or if both circle and elipse are not selected
                    if (index == 2 || index == 3 || (!optionSelected[0] && !optionSelected[1]))
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

        #endregion

        public override void Update(GameTime gameTime)
        {
            if ((SMPanel != null && SMPanel.IsMouseHovering) || (ArrowPanel != null && ArrowPanel.IsMouseHovering))
                Main.LocalPlayer.mouseInterface = true;

            if (DraggableUIPanel.canDrag)
                SMPanel.UpdatePosition();
        }
    }
}
