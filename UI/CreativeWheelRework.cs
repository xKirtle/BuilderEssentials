using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace BuilderEssentials.UI
{
    public class CreativeWheelRework
    {
        static BuilderPlayer modPlayer;
        public static UIPanel CreativeWheelReworkPanel;
        public static float CreativeWheelReworkWidth;
        public static float CreativeWheelReworkHeight;
        private static List<UIImageButton> CreativeWheelElements;

        public static UIPanel CreateCreativeWheelReworkPanel(int mouseX, int mouseY, BasePanel basePanel)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            CreativeWheelReworkWidth = 250f;
            CreativeWheelReworkHeight = 250f;

            CreativeWheelReworkPanel = new UIPanel();
            CreativeWheelReworkPanel.VAlign = 0f;
            CreativeWheelReworkPanel.HAlign = 0f;
            CreativeWheelReworkPanel.Width.Set(CreativeWheelReworkWidth, 0);
            CreativeWheelReworkPanel.Height.Set(CreativeWheelReworkHeight, 0);
            CreativeWheelReworkPanel.Left.Set(mouseX - CreativeWheelReworkWidth / 2, 0); //mouseX - this.width/2
            CreativeWheelReworkPanel.Top.Set(mouseY - CreativeWheelReworkHeight / 2, 0); //mouseY - this.height/2
            CreativeWheelReworkPanel.BorderColor = Color.Green; //Color.Transparent;
            CreativeWheelReworkPanel.BackgroundColor = Color.Transparent;

            CreateLayout();
            basePanel.Append(CreativeWheelReworkPanel);

            return CreativeWheelReworkPanel;
        }

        private static void CreateLayout()
        {
            CreativeWheelElements = new List<UIImageButton>();
            for (int i = 0; i < BuilderEssentials.CreativeWheelElements.Count; i++)
                CreativeWheelElements.Add(new UIImageButton(BuilderEssentials.CreativeWheelElements[i]));

            double radius = 100;
            double angle = Math.PI / 3; //6 Button
            for (int i = 0; i < CreativeWheelElements.Count; i++)
            {
                //I add +3 in both x and y to rotate the whole "wheel" so the starting icon starts where I want it at
                double x = ((CreativeWheelReworkWidth / 2) - 22f) + (radius * Math.Cos(angle * (i + 4)));
                double y = ((CreativeWheelReworkHeight / 2) + 6) - (radius * Math.Sin(angle * (i + 4)));
                CreativeWheelElements[i].VAlign = 0f;
                CreativeWheelElements[i].HAlign = 0f;
                CreativeWheelElements[i].Left.Set((float)x, 0f);
                CreativeWheelElements[i].Top.Set((float)y, 0f);
                CreativeWheelElements[i].SetVisibility(.75f, .4f);
            }

            for (int i = 0; i < CreativeWheelElements.Count; i++)
                CreativeWheelReworkPanel.Append(CreativeWheelElements[i]);
        }
    }
}