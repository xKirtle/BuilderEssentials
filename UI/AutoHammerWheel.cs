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
    class AutoHammerWheel : UIPanel
    {
        public static UIPanel AutoHammerWheelPanel;
        private static float AutoHammerWheelWidth;
        private static float AutoHammerWheelHeight;
        private static List<UIImageButton> AutoHammerElements;
        public static int selectedIndex = 5;//full tile
        public static bool IsAutoHammerUIVisible { get; set; }
        public static bool AutoHammerUIOpen { get; set; }
        public static bool Hovering = AutoHammerWheelPanel != null && AutoHammerWheelPanel.IsMouseHovering && IsAutoHammerUIVisible;

        public static UIPanel CreateAutoHammerWheelPanel(int mouseX, int mouseY, BasePanel basePanel)
        {
            AutoHammerWheelWidth = 250f;
            AutoHammerWheelHeight = 250f;

            AutoHammerWheelPanel = new UIPanel();
            AutoHammerWheelPanel.VAlign = 0f;
            AutoHammerWheelPanel.HAlign = 0f;
            AutoHammerWheelPanel.Width.Set(AutoHammerWheelWidth, 0);
            AutoHammerWheelPanel.Height.Set(AutoHammerWheelHeight, 0);
            AutoHammerWheelPanel.Left.Set(mouseX - AutoHammerWheelWidth / 2, 0);
            AutoHammerWheelPanel.Top.Set(mouseY - AutoHammerWheelHeight / 2, 0);
            AutoHammerWheelPanel.BorderColor = Color.Transparent; //Color.Transparent;
            AutoHammerWheelPanel.BackgroundColor = Color.Transparent;

            CreateLayout();
            basePanel.Append(AutoHammerWheelPanel);

            return AutoHammerWheelPanel;
        }

        private static void CreateLayout()
        {
            //Initialize the list that contains the WandWheel Elements, that are also intialized below
            AutoHammerElements = new List<UIImageButton>();
            for (int i = 0; i < BuilderEssentials.AutoHammerElements.Count; i++)
                AutoHammerElements.Add(new UIImageButton(BuilderEssentials.AutoHammerElements[i]));

            //Define them in a circle
            double radius = 60;
            double angle = Math.PI / 3; //6 Buttons
            for (int i = 0; i < AutoHammerElements.Count; i++)
            {
                int index = i; //Magic values to keep the rotation aligned the way I want, sorry
                double x = (AutoHammerWheelWidth / 2 - 35f) + (radius * Math.Cos(angle * (i + 3)));
                double y = (AutoHammerWheelHeight / 2 - 35f) - (radius * Math.Sin(angle * (i + 3)));
                AutoHammerElements[i].VAlign = 0f;
                AutoHammerElements[i].HAlign = 0f;
                AutoHammerElements[i].Left.Set((float)x, 0f);
                AutoHammerElements[i].Top.Set((float)y, 0f);
                AutoHammerElements[i].SetVisibility(.75f, .4f);
                AutoHammerElements[i].OnClick += (__, _) => MainElementClick(index);
            }

            //Correct display of previously toggled settings
            if (selectedIndex != -1)
                AutoHammerElements[selectedIndex].SetVisibility(1f, 1f);

            //Append them to the Main Panel
            for (int i = 0; i < AutoHammerElements.Count; i++)
                AutoHammerWheelPanel.Append(AutoHammerElements[i]);
        }

        private static void MainElementClick(int index)
        {
            for (int i = 0; i < AutoHammerElements.Count; i++)
                AutoHammerElements[i].SetVisibility(.75f, .4f);

            if (selectedIndex != index)
            {
                selectedIndex = index;
                AutoHammerElements[index].SetVisibility(1f, 1f);
            }
            else
                selectedIndex = -1;
        }
    }
}
