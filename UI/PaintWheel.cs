using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace BuilderEssentials.UI
{
    public class PaintWheel
    {
        private static BuilderPlayer modPlayer;
        public static UIPanel paintWheel;

        //TODO: ADD TOOLTIPS WHEN HOVERING ON CW ELEMENTS TO DISPLAY WHAT THEY DO / HOW THEY'RE USED

        private static float paintWheelWidth;
        private static float paintWheelHeight;
        private static List<UIImageButton> colorsList;
        private static List<UIImageButton> paintToolsList;
        private static bool AnyPaintToolSelected;
        public static UIPanel CreatePaintWheel(int mouseX, int mouseY, BasePanel basePanel)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            paintWheelWidth = 300f;
            paintWheelHeight = 200f;

            paintWheel = new UIPanel();
            paintWheel.VAlign = 0f;
            paintWheel.HAlign = 0f;
            paintWheel.Width.Set(paintWheelWidth, 0);
            paintWheel.Height.Set(paintWheelHeight, 0);
            paintWheel.Left.Set(mouseX - paintWheelWidth / 2, 0); //mouseX - this.width/2
            paintWheel.Top.Set(mouseY - paintWheelHeight / 2, 0); //mouseY - this.height/2
            paintWheel.BorderColor = Color.Red; //Color.Red;
            paintWheel.BackgroundColor = Color.Transparent;

            CreateColorsDisplay();
            for (int i = 0; i < colorsList.Count; i++)
                paintWheel.Append(colorsList[i]);

            ColorSelected(modPlayer.paintingColorSelectedIndex);

            for (int i = 0; i < paintToolsList.Count; i++)
                paintWheel.Append(paintToolsList[i]);

            ToolSelected(modPlayer.paintingToolSelected);


            Main.NewText("ColorsList: " + colorsList.Count);
            basePanel.Append(paintWheel);
            return paintWheel;
        }

        //Layout inspired by the great VipixToolBox Mod
        public static void CreateColorsDisplay()
        {
            //Initialize all colors into a list
            colorsList = new List<UIImageButton>();
            for (int i = 0; i < BuilderEssentials.PaintColors.Count; i++)
                colorsList.Add(new UIImageButton(BuilderEssentials.PaintColors[i]));

            paintToolsList = new List<UIImageButton>();
            for (int i = 0; i < 3; i++)
                paintToolsList.Add(new UIImageButton(BuilderEssentials.PaintTools[i]));

            // //Define them programatically in a circle
            // double radius = 125;
            // double angle = Math.PI / 15;
            // for (int i = 0; i < colorsList.Count; i++)
            // {
            //     double x = (paintWheelWidth / 2) + (radius * Math.Cos(angle * i));
            //     double y = (paintWheelHeight / 2) - (radius * Math.Sin(angle * i));
            //     colorsList[i].VAlign = 0f;
            //     colorsList[i].HAlign = 0f;
            //     colorsList[i].Left.Set((float)x, 0f);
            //     colorsList[i].Top.Set((float)y, 0f);
            //     colorsList[i].SetVisibility(.75f, .4f);
            // }

            float colorSize = 22f;
            //Define a semi-circle programatically for basic colors
            double radius = 90;
            double angle = Math.PI / 12;
            for (int i = 0; i < 12; i++)
            {
                double x = (paintWheelWidth / 2) - (radius * Math.Cos(-angle * (i + .5)) * 1.10);
                double y = (paintWheelHeight / 2) - (-radius * Math.Sin(-angle * (i + .5))) + 15;
                colorsList[i].VAlign = 0f;
                colorsList[i].HAlign = 0f;
                colorsList[i].Left.Set((float)x - colorSize, 0f);
                colorsList[i].Top.Set((float)y, 0f);
                colorsList[i].SetVisibility(.75f, .60f);
            }

            //Define another semi-circle for deep colors
            radius = 110;
            angle = Math.PI / 12;
            for (int i = 12; i < 24; i++)
            {
                double x = (paintWheelWidth / 2) - (radius * Math.Cos(-angle * ((i - 12) + .5)) * 1.15);
                double y = (paintWheelHeight / 2) - (-radius * Math.Sin(-angle * ((i - 12) + .5)) - 10);
                colorsList[i].VAlign = 0f;
                colorsList[i].HAlign = 0f;
                colorsList[i].Left.Set((float)x - colorSize, 0f);
                colorsList[i].Top.Set((float)y, 0f);
                colorsList[i].SetVisibility(.75f, .60f);
            }

            //Define another semi-circle for the remaining 6 colors
            radius = 65;
            angle = Math.PI / 6;
            for (int i = 24; i < 30; i++)
            {
                double x = (paintWheelWidth / 2) - (radius * Math.Cos(-angle * ((i - 24) + .5)) * 1);
                double y = (paintWheelHeight / 2) - (-radius * Math.Sin(-angle * ((i - 24) + .5)) - 20);
                colorsList[i].VAlign = 0f;
                colorsList[i].HAlign = 0f;
                colorsList[i].Left.Set((float)x - colorSize, 0f);
                colorsList[i].Top.Set((float)y, 0f);
                colorsList[i].SetVisibility(.75f, .60f);
            }

            //Last element, no color
            double noColorX = (paintWheelWidth / 2);
            double noColorY = (paintWheelHeight / 2);
            colorsList[30].VAlign = 0f;
            colorsList[30].HAlign = 0f;
            colorsList[30].Left.Set((float)noColorX - colorSize, 0f);
            colorsList[30].Top.Set((float)noColorY, 0f);
            colorsList[30].SetVisibility(.75f, .60f);

            for (int i = 0; i < 3; i++)
            {
                double x = (paintWheelWidth / 3) * i + 15f;
                double y = (paintWheelHeight / 2) + 35f;
                paintToolsList[i].VAlign = 0f;
                paintToolsList[i].HAlign = 0f;
                paintToolsList[i].Left.Set((float)x, 0f);
                paintToolsList[i].Top.Set((float)y, 0f);
                paintToolsList[i].SetVisibility(.75f, .4f);
            }

            for (int i = 0; i < colorsList.Count; i++)
            {
                int index = i;
                colorsList[i].OnClick += (__, _) => ColorSelected(index);
            }

            for (int i = 0; i < paintToolsList.Count; i++)
            {
                int index = i;
                paintToolsList[i].OnClick += (__, _) => ToolSelected(index);
            }
        }

        private static void ColorSelected(int index)
        {
            Main.NewText("Color: " + index);
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (index != modPlayer.paintingColorSelectedIndex)
            {
                modPlayer.paintingColorSelectedIndex = index;

                for (int i = 0; i < colorsList.Count; i++)
                    colorsList[i].SetVisibility(.75f, .60f);

                colorsList[index].SetVisibility(1f, 1f);
            }
        }

        private static void ToolSelected(int index)
        {
            Main.NewText("Tool: " + index);
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            //modPlayer.paintingToolSelected is initialized with a negative value
            if (index >= 0)
            {
                if (index != modPlayer.paintingToolSelected || !AnyPaintToolSelected)
                {
                    modPlayer.paintingToolSelected = index;
                    AnyPaintToolSelected = true;

                    for (int i = 0; i < paintToolsList.Count; i++)
                        paintToolsList[i].SetVisibility(.75f, .4f);

                    paintToolsList[index].SetVisibility(1f, 1f);
                }
                else
                {
                    paintToolsList[index].SetVisibility(.75f, .4f);
                    AnyPaintToolSelected = false;
                }
            }
        }
    }
}
