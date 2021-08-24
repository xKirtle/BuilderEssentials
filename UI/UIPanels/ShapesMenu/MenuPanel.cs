using BuilderEssentials.UI.Elements;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesMenu
{
    internal class MenuPanel : CustomUIPanel
    {
        //TODO: Make clicking on menu options not trigger the ShapesDrawer or smt
        //TODO: Change shapes menu assets.
        //When selected, only borders should be yellow and when fill is enabled, shapes borders should then become yellow

        private const string texturePath = "BuilderEssentials/Textures/UIElements/ShapesMenu/";
        private const float MenuWidth = 213f, MenuHeight = 167f;
        public bool[] selected;
        private UIImage[] elements;

        public MenuPanel()
        {
            Width.Set(MenuWidth, 0);
            Height.Set(MenuHeight, 0);
            Left.Set(10f, 0);
            Top.Set((Main.screenHeight - MenuHeight) / 2, 0);
            BorderColor = Microsoft.Xna.Framework.Color.Transparent;
            BackgroundColor = Microsoft.Xna.Framework.Color.Transparent;

            Asset<Texture2D> backgroundTexture = HelperMethods.RequestTexture(texturePath + "Background");
            UIImage background = new UIImage(backgroundTexture);
            background.Width.Set(0f, 0);
            background.Height.Set(0f, 0);
            background.Left.Set(-12f, 0);
            background.Top.Set(-12f, 0);
            Append(background);

            Asset<Texture2D> closeCrossTexture = HelperMethods.RequestTexture(texturePath + "CloseCross");
            UIImage closeCross = new UIImage(closeCrossTexture);
            closeCross.Width.Set(19f, 0);
            closeCross.Height.Set(19f, 0);
            closeCross.Left.Set(MenuWidth - 35f, 0);
            closeCross.Top.Set(-7f, 0);
            closeCross.OnMouseDown += (__, _) => { Hide(); UIUIState.Instance.arrowPanel.Show(); };
            Append(closeCross);


            elements = new UIImage[6];
            selected = new bool[6];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int row = i;
                    int column = j;
                    //I have no idea why the texture names start at 1 instead of 0 but I'm too lazy to fix that
                    Asset<Texture2D> tempButtonTexture = HelperMethods.RequestTexture(texturePath + $"SM{(column + 1) + 3 * row}");
                    UIImage tempButton = new UIImage(tempButtonTexture);
                    tempButton.Width.Set(54f, 0);
                    tempButton.Height.Set(54f, 0);
                    tempButton.Left.Set(j * 64 + 3f, 0);
                    tempButton.Top.Set(24f + 63 * i, 0);
                    tempButton.OnMouseDown += (__, _) => ButtonClicked(column + 3 * row);
                    elements[column + 3 * row] = tempButton;
                    Append(tempButton);
                }
            }
            
            //Updating the disabled half shapes sprites
            elements[3].SetImage(HelperMethods.RequestTexture(texturePath + "SMDisabled4"));
            elements[4].SetImage(HelperMethods.RequestTexture(texturePath + "SMDisabled5"));
            
            //Updating all sprites based on selected[]. index doesn't matter
            SetUIImage(0);
        }
        
        void ButtonClicked(int index)
        {
            selected[index] = !selected[index];

            //Can't select both ellipse and rectangle at the same time
            if (index == 0)
                selected[1] = false;
            else if (index == 1)
                selected[0] = false;

            //Disable half shapes if ellipse is disabled
            if (!selected[0])
                selected[3] = selected[4] = false;

            //if ellipse is enabled and index clicked was a half shape, disable the other half shape
            if (selected[0] && (index == 3 || index == 4))
                selected[index == 3 ? 4 : 3] = false;
            
            //Update buttons based on selected[]
            SetUIImage(index);
            
            //Update CoordinateSelection
            if (selected[0] && (index == 3 || index == 4 || index == 5))
                GameUIState.Instance.ellipseShape.FixMirrorHalfShapesCoords();
        }
        
        void SetUIImage(int clickedIndex, bool updateAll = true)
        {
            int length = updateAll == false ? 1 : 6;
            for (int i = 0; i < length; i++)
            {
                int tempIndex = updateAll == false ? clickedIndex : i;

                string texture = texturePath + "SM";
                if (selected[2])
                    texture += "Fill";
                if (selected[tempIndex])
                    texture += "Alternate";
                if (selected[5] && (tempIndex == 3 || tempIndex == 4))
                    texture += "Mirror";
                if (!selected[0] && (tempIndex == 3 || tempIndex == 4))
                    texture += "Disabled";
                texture += $"{tempIndex + 1}";

                elements[tempIndex].SetImage(HelperMethods.RequestTexture(texture));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            //Fixing UI Scale positioning
            Top.Set((Main.screenHeight - MenuHeight) / 2, 0);
            
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
        }
    }
}