using BuilderEssentials.UI.Elements;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesMenu
{
    public class MenuPanel : CustomUIPanel
    {
        private const string texturePath = "BuilderEssentials/Textures/UIElements/ShapesMenu/";
        private const float MenuWidth = 213f, MenuHeight = 167f;
        public bool[] selected;
        public CustomUIImage[] elements;

        public MenuPanel()
        {
            SetSize(MenuWidth, MenuHeight);
            SetOffset(10f, (Main.screenHeight - MenuHeight) / 2);
            BorderColor = Microsoft.Xna.Framework.Color.Transparent;
            BackgroundColor = Microsoft.Xna.Framework.Color.Transparent;

            Texture2D backgroundTexture = ModContent.GetTexture(texturePath + "Background");
            CustomUIImage background = new CustomUIImage(backgroundTexture, 1f);
            background.SetSize(Vector2.Zero);
            background.SetOffset(-12f, -12f);
            Append(background);

            Texture2D closeCrossTexture = ModContent.GetTexture(texturePath + "CloseCross");
            CustomUIImage closeCross = new CustomUIImage(closeCrossTexture, 1f);
            closeCross.SetSize(19f, 19f);
            closeCross.SetOffset(MenuWidth - 35f, -7f);
            closeCross.OnMouseDown += (__, _) => { Hide(); UIStateLogic4.arrowPanel.Show(); };
            Append(closeCross);


            elements = new CustomUIImage[6];
            selected = new bool[6];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int row = i;
                    int column = j;
                    //I have no idea why the texture names start at 1 instead of 0 but I'm too lazy to fix that
                    Texture2D tempButtonTexture = ModContent.GetTexture(texturePath + $"SM{(column + 1) + 3 * row}");
                    CustomUIImage tempButton = new CustomUIImage(tempButtonTexture, 1f);
                    tempButton.SetSize(54f, 54f);
                    tempButton.SetOffset(j * 64 + 3f, 24f + 63 * i);
                    tempButton.OnMouseDown += (__, _) => ButtonClicked(column + 3 * row);
                    elements[column + 3 * row] = tempButton;
                    Append(tempButton);
                }
            }
            
            //Updating the disabled half shapes sprites
            elements[3].SetImage(ModContent.GetTexture(texturePath + "SMDisabled4"));
            elements[4].SetImage(ModContent.GetTexture(texturePath + "SMDisabled5"));
            
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

                elements[tempIndex].SetImage(ModContent.GetTexture(texture));
            }
        }
    }
}