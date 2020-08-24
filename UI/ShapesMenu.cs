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
        public static bool SDEquipped;
        public override void OnInitialize()
        {
            Instance = this;
            optionSelected = new bool[6];
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

        public static bool isFillEnabled = false;
        private static bool isMirrorEnabled = false;
        private static bool isHalfShapesEnabled = false;
        public static bool[] optionSelected;
        public static void CreateShapesMenuPanel()
        {
            SMWidth = 213f;
            SMHeight = 167f;

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

            List<UIImage> ShapesMenuList = new List<UIImage>(6);

            string textureLocation = "BuilderEssentials/Textures/UIElements/ShapesMenu/";

            for (int i = 0; i < 3; i++) //Top Row
            {
                int index = i;
                UIImage tempUIImage = new UIImage(GetTexture(textureLocation + $"SM{i + 1}"));
                tempUIImage.Width.Set(36f, 0);
                tempUIImage.Height.Set(36f, 0);
                tempUIImage.Left.Set(i * 64 + 3f, 0);
                tempUIImage.Top.Set(24f, 0);
                tempUIImage.OnMouseDown += (__, _) => SMClicked(index);

                ShapesMenuList.Add(tempUIImage);
                SMPanel.Append(tempUIImage);
            }

            for (int i = 0; i < 3; i++) //Bottom Row
            {
                int index = i;
                UIImage tempUIImage = new UIImage(GetTexture(textureLocation + $"SM{i + 4}"));
                tempUIImage.Width.Set(36f, 0);
                tempUIImage.Height.Set(36f, 0);
                tempUIImage.Left.Set(i * 64 + 3f, 0);
                tempUIImage.Top.Set(87f, 0);
                tempUIImage.OnMouseDown += (__, _) => SMClicked(index + 3);

                ShapesMenuList.Add(tempUIImage);
                SMPanel.Append(tempUIImage);
            }

            //Updating the Disabled half shapes sprites
            ShapesMenuList[3].SetImage(GetTexture(textureLocation + "SMDisabled4"));
            ShapesMenuList[4].SetImage(GetTexture(textureLocation + "SMDisabled5"));

            //Updating all sprites based on optionSelected[], index doesn't matter
            SetUIImage(0);


            void SMClicked(int index)
            {
                optionSelected[index] = !optionSelected[index];
                isFillEnabled = optionSelected[2];
                isMirrorEnabled = optionSelected[5];

                //Can't select both elipse and rectangle at the same time
                if (index == 0)
                    optionSelected[1] = false;
                else if (index == 1)
                    optionSelected[0] = false;

                //Disable half shapes if elipse is disabled
                if (!optionSelected[0])
                    optionSelected[3] = optionSelected[4] = false;

                //if elipse is enabled and index clicked was a half shape, disable the other half shape
                if (optionSelected[0] && (index == 3 || index == 4))
                    optionSelected[index == 3 ? 4 : 3] = false;

                //Check whether half shapes can be selected or not
                isHalfShapesEnabled = optionSelected[0];

                //Update buttons based on optionSelected[]
                SetUIImage(index);
            }

            void SetUIImage(int clickedIndex, bool updateAll = true)
            {
                int length = updateAll == false ? 1 : 6;
                for (int i = 0; i < length; i++)
                {
                    int tempIndex = updateAll == false ? clickedIndex : i;

                    string texture = textureLocation + "SM";
                    if (isFillEnabled)
                        texture += "Fill";
                    if (optionSelected[tempIndex])
                        texture += "Alternate";
                    if (isMirrorEnabled && (tempIndex == 3 || tempIndex == 4))
                        texture += "Mirror";
                    if (!isHalfShapesEnabled && (tempIndex == 3 || tempIndex == 4))
                        texture += "Disabled";
                    texture += $"{tempIndex + 1}";

                    ShapesMenuList[tempIndex].SetImage(GetTexture(texture));
                }
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (SMPanel == null && ArrowPanel == null && SDEquipped)
                CreateArrowPanel();

            if ((SMPanel != null && SMPanel.IsMouseHovering) || (ArrowPanel != null && ArrowPanel.IsMouseHovering))
                Main.LocalPlayer.mouseInterface = true;

            if (SMPanel != null && DraggableUIPanel.canDrag)
                SMPanel.UpdatePosition();
        }
    }
}
