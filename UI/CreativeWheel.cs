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
    public class CreativeWheel
    {
        private static BuilderPlayer modPlayer;
        public static UIPanel creativeWheel;
        private static UIImageButton colorPicker;
        private static UIImageButton infinitePlacement;
        private static UIImageButton autoHammer;
        private static List<UIImageButton> autoHammerSlopes;
        private static float autoHammerLeftValue;
        private static float autoHammerTopValue;

        //TODO: ADD TOOLTIPS WHEN HOVERING ON CW ELEMENTS TO DISPLAY WHAT THEY DO / HOW THEY'RE USED

        public static UIPanel CreateCreativeWheelPanel(int mouseX, int mouseY, BasePanel basePanel)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            creativeWheel = new UIPanel();
            creativeWheel.VAlign = 0f;
            creativeWheel.HAlign = 0f;
            creativeWheel.Width.Set(200f, 0);
            creativeWheel.Height.Set(230f, 0);
            creativeWheel.Left.Set(mouseX - 200 / 2, 0); //mouseX - this.width/2
            creativeWheel.Top.Set(mouseY - 200 / 2, 0); //mouseY - this.height/2
            creativeWheel.BorderColor = Color.Transparent; //Color.Red;
            creativeWheel.BackgroundColor = Color.Transparent;

            basePanel.Append(creativeWheel);

            UIImageButton colorPicker = CreateColorPicker();
            creativeWheel.Append(colorPicker);

            UIImageButton infinitePlacement = CreateInfinitePlacement();
            creativeWheel.Append(infinitePlacement);

            UIImageButton autoHammer = CreateAutoHammer();
            creativeWheel.Append(autoHammer);

            ResetVisibilityValuesForAllOptions();
            return creativeWheel;
        }

        private static UIImageButton CreateColorPicker()
        {
            colorPicker = new UIImageButton(BuilderEssentials.CWColorPicker);
            colorPicker.Left.Set(0, 0f);
            colorPicker.Top.Set(67.5f, 0f);
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

        private static UIImageButton CreateAutoHammer()
        {
            autoHammerLeftValue = 67.5f;
            autoHammerTopValue = 110f;

            autoHammer = new UIImageButton(BuilderEssentials.CWAutoHammer);
            autoHammer.Left.Set(autoHammerLeftValue, 0f);
            autoHammer.Top.Set(autoHammerTopValue, 0f);
            autoHammer.SetVisibility(.75f, .4f);
            autoHammer.OnClick += AutoHammer_OnClick;

            return autoHammer;
        }

        private static void CreateAutoHammerSlopes()
        {
            autoHammerSlopes = new List<UIImageButton>();

            UIImageButton slope0 = new UIImageButton(BuilderEssentials.CWAutoHammerIndex[0]);
            slope0.Left.Set(autoHammerLeftValue - 25f, 0);
            slope0.Top.Set(autoHammerTopValue + 10f, 0);
            slope0.SetVisibility(.75f, .4f);
            slope0.OnClick += Slope_OnClick;
            autoHammerSlopes.Add(slope0);

            UIImageButton slope1 = new UIImageButton(BuilderEssentials.CWAutoHammerIndex[1]);
            slope1.Left.Set(autoHammerLeftValue + 46f, 0);
            slope1.Top.Set(autoHammerTopValue + 10f, 0);
            slope1.SetVisibility(.75f, .4f);
            slope1.OnClick += Slope_OnClick;
            autoHammerSlopes.Add(slope1);

            UIImageButton slope2 = new UIImageButton(BuilderEssentials.CWAutoHammerIndex[2]);
            slope2.Left.Set(autoHammerLeftValue + 32f, 0);
            slope2.Top.Set(autoHammerTopValue + 40f, 0);
            slope2.SetVisibility(.75f, .4f);
            slope2.OnClick += Slope_OnClick;
            autoHammerSlopes.Add(slope2);

            UIImageButton slope3 = new UIImageButton(BuilderEssentials.CWAutoHammerIndex[3]);
            slope3.Left.Set(autoHammerLeftValue - 10f, 0);
            slope3.Top.Set(autoHammerTopValue + 40f, 0);
            slope3.SetVisibility(.75f, .4f);
            slope3.OnClick += Slope_OnClick;
            autoHammerSlopes.Add(slope3);

            UIImageButton slope4 = new UIImageButton(BuilderEssentials.CWAutoHammerIndex[4]);
            slope4.Left.Set(autoHammerLeftValue + 32f, 0);
            slope4.Top.Set(autoHammerTopValue - 18f, 0);
            slope4.SetVisibility(.75f, .4f);
            slope4.OnClick += Slope_OnClick;
            autoHammerSlopes.Add(slope4);

            UIImageButton slope5 = new UIImageButton(BuilderEssentials.CWAutoHammerIndex[5]);
            slope5.Left.Set(autoHammerLeftValue - 10f, 0);
            slope5.Top.Set(autoHammerTopValue - 18f, 0);
            slope5.SetVisibility(.75f, .4f);
            slope5.OnClick += Slope_OnClick;
            autoHammerSlopes.Add(slope5);

            for (int i = 0; i < 6; i++)
                creativeWheel.Append(autoHammerSlopes[i]);
        }

        private static void Slope_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (listeningElement == autoHammerSlopes[0])
                modPlayer.autoHammerSelectedIndex = 0;
            else if (listeningElement == autoHammerSlopes[1])
                modPlayer.autoHammerSelectedIndex = 1;
            else if (listeningElement == autoHammerSlopes[2])
                modPlayer.autoHammerSelectedIndex = 2;
            else if (listeningElement == autoHammerSlopes[3])
                modPlayer.autoHammerSelectedIndex = 3;
            else if (listeningElement == autoHammerSlopes[4])
                modPlayer.autoHammerSelectedIndex = 4;
            else if (listeningElement == autoHammerSlopes[5])
                modPlayer.autoHammerSelectedIndex = 5;

            ResetSlopeVisibility();
        }

        private static void RemoveAutoHammerSlopes()
        {
            if (autoHammerSlopes != null)
                for (int i = 0; i < 6; i++)
                    autoHammerSlopes[i].Remove();
        }

        private static void ColorPicker_OnClick(UIMouseEvent evt, UIElement listeningElement)
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

        private static void InfinitePlacement_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            //InfinitePlacement doesn't need to reset all the others since it can be used at the same time
            if (!modPlayer.InfinitePlacementSelected)
                modPlayer.InfinitePlacementSelected = true;
            else
                modPlayer.InfinitePlacementSelected = false;

            ResetVisibilityValuesForAllOptions();
        }

        private static void AutoHammer_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (modPlayer.autoHammerSelected)
            {
                SetAllCWToFalse();
                RemoveAutoHammerSlopes();
            }
            else
            {
                SetAllCWToFalse();
                modPlayer.autoHammerSelected = true;
                CreateAutoHammerSlopes();
                ResetSlopeVisibility();
            }

            ResetVisibilityValuesForAllOptions();

            //When clicking here open another UIState to select which slope to use?
        }

        public static void ResetVisibilityValuesForAllOptions()
        {
            colorPicker.SetVisibility(.75f, .4f);
            if (modPlayer.colorPickerSelected)
                colorPicker.SetVisibility(1f, 1f);

            infinitePlacement.SetVisibility(.75f, .4f);
            if (modPlayer.InfinitePlacementSelected)
                infinitePlacement.SetVisibility(1f, 1f);

            autoHammer.SetVisibility(.75f, .4f);
            if (modPlayer.autoHammerSelected)
            {
                autoHammer.SetVisibility(1f, 1f);
                RemoveAutoHammerSlopes();
                CreateAutoHammerSlopes();
                ResetSlopeVisibility();
            }
        }

        private static void ResetSlopeVisibility()
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            for (int i = 0; i < 6; i++)
            {
                autoHammerSlopes[i].SetVisibility(.75f, .4f);
                if (i == modPlayer.autoHammerSelectedIndex)
                    autoHammerSlopes[i].SetVisibility(1f, 1f);
            }
        }

        private static void SetAllCWToFalse()
        {
            modPlayer.colorPickerSelected = false;
            modPlayer.autoHammerSelected = false;
        }
    }
}
