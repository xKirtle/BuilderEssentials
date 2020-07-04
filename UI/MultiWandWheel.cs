using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace BuilderEssentials.UI
{
    class MultiWandWheel : UIPanel
    {
        static BuilderPlayer modPlayer;
        public static UIPanel MultiWandWheelPanel;
        public static float MultiWandWheelWidth;
        public static float MultiWandWheelHeight;
        public static List<UIImageButton> WandWheelElements;

        public static UIPanel CreateMultiWandWheelPanel(int mouseX, int mouseY, BasePanel basePanel)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            MultiWandWheelWidth = 250f;
            MultiWandWheelHeight = 250f;

            MultiWandWheelPanel = new UIPanel();
            MultiWandWheelPanel.VAlign = 0f;
            MultiWandWheelPanel.HAlign = 0f;
            MultiWandWheelPanel.Width.Set(MultiWandWheelWidth, 0);
            MultiWandWheelPanel.Height.Set(MultiWandWheelHeight, 0);
            MultiWandWheelPanel.Left.Set(mouseX - MultiWandWheelWidth / 2, 0);
            MultiWandWheelPanel.Top.Set(mouseY - MultiWandWheelHeight / 2, 0);
            MultiWandWheelPanel.BorderColor = Color.Transparent; //Color.Transparent;
            MultiWandWheelPanel.BackgroundColor = Color.Transparent;

            CreateLayout();
            basePanel.Append(MultiWandWheelPanel);

            return MultiWandWheelPanel;
        }

        private static void CreateLayout()
        {
            //Initialize the list that contains the WandWheel Elements, that are also intialized below
            WandWheelElements = new List<UIImageButton>();
            for (int i = 0; i < BuilderEssentials.WandWheelElements.Count; i++)
                WandWheelElements.Add(new UIImageButton(BuilderEssentials.WandWheelElements[i]));

            //Define them in a circle
            double radius = 60;
            double angle = Math.PI / 3; //6 Buttons
            for (int i = 0; i < WandWheelElements.Count; i++)
            {
                int index = i; //Magic values to keep the rotation aligned the way I want, sorry
                double x = (MultiWandWheelWidth / 2 - 35f) + (radius * Math.Cos(angle * (i + 3)));
                double y = (MultiWandWheelHeight / 2 - 35f) - (radius * Math.Sin(angle * (i + 3)));
                WandWheelElements[i].VAlign = 0f;
                WandWheelElements[i].HAlign = 0f;
                WandWheelElements[i].Left.Set((float)x, 0f);
                WandWheelElements[i].Top.Set((float)y, 0f);
                WandWheelElements[i].SetVisibility(.75f, .4f);
                WandWheelElements[i].OnClick += (__, _) => MainElementClick(index);
            }

            //Correct display of previously toggled settings
            WandWheelElements[modPlayer.wandWheelSelectedIndex].SetVisibility(1f, 1f);

            //Append them to the Main Panel
            for (int i = 0; i < WandWheelElements.Count; i++)
                MultiWandWheelPanel.Append(WandWheelElements[i]);
        }

        private static void MainElementClick(int index)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            for (int i = 0; i < WandWheelElements.Count; i++)
                WandWheelElements[i].SetVisibility(.75f, .4f);

            WandWheelElements[index].SetVisibility(1f, 1f);
            modPlayer.wandWheelSelectedIndex = index;

        }
    }
}
