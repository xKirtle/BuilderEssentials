using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace BuilderEssentials.UI
{
    public class CreativeWheel
    {
        private static BuilderPlayer modPlayer;
        private static UIImageButton colorPicker;
        private static UIImageButton infinitePlacement;

        //TODO: ADD TOOLTIPS WHEN HOVERING ON CW ELEMENTS TO DISPLAY WHAT THEY DO / HOW THEY'RE USED

        public static UIPanel CreateCreativeWheelPanel(int mouseX, int mouseY, BasePanel basePanel)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            UIPanel creativeWheel = new UIPanel();
            creativeWheel.VAlign = 0f;
            creativeWheel.HAlign = 0f;
            creativeWheel.Width.Set(200f, 0);
            creativeWheel.Height.Set(200f, 0);
            creativeWheel.Left.Set(mouseX - 200 / 2, 0); //mouseX - this.width/2
            creativeWheel.Top.Set(mouseY - 200 / 2, 0); //mouseY - this.height/2
            creativeWheel.BorderColor = Color.Transparent; //Color.Red;
            creativeWheel.BackgroundColor = Color.Transparent;

            basePanel.Append(creativeWheel);

            UIImageButton colorPicker = CreateColorPicker();
            creativeWheel.Append(colorPicker);

            UIImageButton infinitePlacement = CreateInfinitePlacement();
            creativeWheel.Append(infinitePlacement);


            ResetVisibilityValuesForAllOptions();
            return creativeWheel;
        }

        private static UIImageButton CreateColorPicker()
        {
            colorPicker = new UIImageButton(BuilderEssentials.CWColorPicker);
            colorPicker.Left.Set(0, 0);
            colorPicker.Top.Set(67.5f, 0);
            colorPicker.SetVisibility(.75f, .4f);
            colorPicker.OnClick += ColorPicker_OnClick;

            return colorPicker;
        }

        private static UIImageButton CreateInfinitePlacement()
        {
            infinitePlacement = new UIImageButton(BuilderEssentials.CWInfinitePlacement);
            infinitePlacement.Left.Set(135f, 0f);
            infinitePlacement.Top.Set(67.5f, 0f);
            infinitePlacement.SetVisibility(.75f, .4f);
            infinitePlacement.OnClick += InfinitePlacement_OnClick;

            return infinitePlacement;
        }

        private static void ColorPicker_OnClick(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            if (modPlayer.colorPickerSelected)
                SetAllCWToFalse();
            else
            {
                SetAllCWToFalse();
                modPlayer.colorPickerSelected = true;
            }

            ResetVisibilityValuesForAllOptions();
        }

        private static void InfinitePlacement_OnClick(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            //InfinitePlacement doesn't need to reset all the others since it can be used at the same time
            if (!modPlayer.InfinitePlacementSelected)
                modPlayer.InfinitePlacementSelected = true;
            else
                modPlayer.InfinitePlacementSelected = false;

            ResetVisibilityValuesForAllOptions();
        }

        public static void ResetVisibilityValuesForAllOptions()
        {
            colorPicker.SetVisibility(.75f, .4f);
            if (modPlayer.colorPickerSelected)
                colorPicker.SetVisibility(1f, 1f);

            infinitePlacement.SetVisibility(.75f, .4f);
            if (modPlayer.InfinitePlacementSelected)
                infinitePlacement.SetVisibility(1f, 1f);
        }

        private static void SetAllCWToFalse()
        {
            modPlayer.colorPickerSelected = false;
        }
    }
}
