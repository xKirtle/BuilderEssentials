// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Input;
// using System;
// using System.Collections.Generic;
// using Terraria;
// using Terraria.UI;
// using Terraria.GameContent.UI.Elements;
// using Terraria.ModLoader;

// namespace BuilderEssentials.UI
// {
//     public class CreativeWheel
//     {
//         //TODO: REDESIGN THE WHOLE UI, IT'S MESSY AND I DON'T LIKE IT, TOO MANY METHODS!!!
//         private static BuilderPlayer modPlayer;
//         public static UIPanel creativeWheel;
//         private static UIImageButton colorPicker;
//         private static UIImageButton infinitePlacement;
//         private static UIImageButton autoHammer;
//         private static List<UIImageButton> autoHammerSlopes;
//         private static float autoHammerLeftValue;
//         private static float autoHammerTopValue;
//         private static float creativeWheelHeight;
//         private static float creativeWheelWidth;

//         //TODO: ADD TOOLTIPS WHEN HOVERING ON CW ELEMENTS TO DISPLAY WHAT THEY DO / HOW THEY'RE USED

//         public static UIPanel CreateCreativeWheelPanel(int mouseX, int mouseY, BasePanel basePanel)
//         {
//             modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

//             creativeWheelWidth = 200f;
//             creativeWheelHeight = 230f;

//             creativeWheel = new UIPanel();
//             creativeWheel.VAlign = 0f;
//             creativeWheel.HAlign = 0f;
//             creativeWheel.Width.Set(creativeWheelWidth, 0);
//             creativeWheel.Height.Set(creativeWheelHeight, 0);
//             creativeWheel.Left.Set(mouseX - 200 / 2, 0); //mouseX - this.width/2
//             creativeWheel.Top.Set(mouseY - 200 / 2, 0); //mouseY - this.height/2
//             creativeWheel.BorderColor = Color.Red; //Color.Transparent;
//             creativeWheel.BackgroundColor = Color.Transparent;

//             basePanel.Append(creativeWheel);

//             UIImageButton colorPicker = CreateColorPicker();
//             creativeWheel.Append(colorPicker);

//             UIImageButton infinitePlacement = CreateInfinitePlacement();
//             creativeWheel.Append(infinitePlacement);

//             UIImageButton autoHammer = CreateAutoHammer();
//             creativeWheel.Append(autoHammer);

//             ResetVisibilityValuesForAllOptions();
//             return creativeWheel;
//         }
//         //Make all 3 current buttons positions be defined by a circumference?
//         private static UIImageButton CreateColorPicker()
//         {
//             colorPicker = new UIImageButton(BuilderEssentials.CWColorPicker);
//             colorPicker.Left.Set(0, 0f);
//             colorPicker.Top.Set(67.5f, 0f);
//             colorPicker.SetVisibility(.75f, .4f);
//             colorPicker.OnClick += ColorPicker_OnClick;

//             return colorPicker;
//         }

//         private static UIImageButton CreateInfinitePlacement()
//         {
//             infinitePlacement = new UIImageButton(BuilderEssentials.CWInfinitePlacement);
//             infinitePlacement.Left.Set(135f, 0f);
//             infinitePlacement.Top.Set(67.5f, 0f);
//             infinitePlacement.SetVisibility(.75f, .4f);
//             infinitePlacement.OnClick += InfinitePlacement_OnClick;

//             return infinitePlacement;
//         }

//         private static UIImageButton CreateAutoHammer()
//         {
//             autoHammerLeftValue = 67.5f;
//             autoHammerTopValue = 110f;

//             autoHammer = new UIImageButton(BuilderEssentials.CWAutoHammer);
//             autoHammer.Left.Set(autoHammerLeftValue, 0f);
//             autoHammer.Top.Set(autoHammerTopValue, 0f);
//             autoHammer.SetVisibility(.75f, .4f);
//             autoHammer.OnClick += AutoHammer_OnClick;

//             return autoHammer;
//         }

//         private static void CreateAutoHammerSlopes()
//         {
//             autoHammerSlopes = new List<UIImageButton>();
//             for (int i = 0; i < 6; i++)
//                 autoHammerSlopes.Add(new UIImageButton(BuilderEssentials.CWAutoHammerElements[i]));


//             double radius = 36;
//             double angle = Math.PI / 3;
//             for (int i = 0; i < autoHammerSlopes.Count; i++)
//             {
//                 //I add +3 in both x and y to rotate the whole "wheel" so the starting icon starts where I want it at
//                 double x = ((creativeWheelWidth / 2) - 22f) + (radius * Math.Cos(angle * (i + 3)));
//                 double y = ((creativeWheelHeight / 2) + 6) - (radius * Math.Sin(angle * (i + 3)));
//                 autoHammerSlopes[i].VAlign = 0f;
//                 autoHammerSlopes[i].HAlign = 0f;
//                 autoHammerSlopes[i].Left.Set((float)x, 0f);
//                 autoHammerSlopes[i].Top.Set((float)y, 0f);
//                 autoHammerSlopes[i].SetVisibility(.75f, .4f);
//             }

//             for (int i = 0; i < 6; i++)
//             {
//                 int index = i;
//                 autoHammerSlopes[i].OnClick += (__, _) => SlopeClicked(index);
//                 creativeWheel.Append(autoHammerSlopes[i]);
//             }
//         }


//         private static void SlopeClicked(int index)
//         {
//             BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
//             modPlayer.autoHammerSelectedIndex = index;
//             ResetSlopeVisibility();
//         }

//         private static void RemoveAutoHammerSlopes()
//         {
//             if (autoHammerSlopes != null)
//                 for (int i = 0; i < 6; i++)
//                     autoHammerSlopes[i].Remove();
//         }

//         private static void ColorPicker_OnClick(UIMouseEvent evt, UIElement listeningElement)
//         {
//             if (modPlayer.colorPickerSelected)
//                 SetAllCWToFalse();
//             else
//             {
//                 SetAllCWToFalse();
//                 modPlayer.colorPickerSelected = true;
//             }

//             ResetVisibilityValuesForAllOptions();
//         }

//         private static void InfinitePlacement_OnClick(UIMouseEvent evt, UIElement listeningElement)
//         {
//             //InfinitePlacement doesn't need to reset all the others since it can be used at the same time
//             if (!modPlayer.InfinitePlacementSelected)
//                 modPlayer.InfinitePlacementSelected = true;
//             else
//                 modPlayer.InfinitePlacementSelected = false;

//             ResetVisibilityValuesForAllOptions();
//         }

//         private static void AutoHammer_OnClick(UIMouseEvent evt, UIElement listeningElement)
//         {
//             if (modPlayer.autoHammerSelected)
//             {
//                 SetAllCWToFalse();
//                 RemoveAutoHammerSlopes();
//             }
//             else
//             {
//                 SetAllCWToFalse();
//                 modPlayer.autoHammerSelected = true;
//                 CreateAutoHammerSlopes();
//                 ResetSlopeVisibility();
//             }

//             ResetVisibilityValuesForAllOptions();
//         }

//         public static void ResetVisibilityValuesForAllOptions()
//         {
//             colorPicker.SetVisibility(.75f, .4f);
//             if (modPlayer.colorPickerSelected)
//                 colorPicker.SetVisibility(1f, 1f);

//             infinitePlacement.SetVisibility(.75f, .4f);
//             if (modPlayer.InfinitePlacementSelected)
//                 infinitePlacement.SetVisibility(1f, 1f);

//             autoHammer.SetVisibility(.75f, .4f);
//             if (modPlayer.autoHammerSelected)
//             {
//                 autoHammer.SetVisibility(1f, 1f);
//                 RemoveAutoHammerSlopes();
//                 CreateAutoHammerSlopes();
//                 ResetSlopeVisibility();
//             }
//         }

//         private static void ResetSlopeVisibility()
//         {
//             modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
//             for (int i = 0; i < 6; i++)
//             {
//                 autoHammerSlopes[i].SetVisibility(.75f, .4f);
//                 if (i == modPlayer.autoHammerSelectedIndex)
//                     autoHammerSlopes[i].SetVisibility(1f, 1f);
//             }
//         }

//         private static void SetAllCWToFalse()
//         {
//             modPlayer.colorPickerSelected = false;
//             modPlayer.autoHammerSelected = false;
//         }
//     }
// }
