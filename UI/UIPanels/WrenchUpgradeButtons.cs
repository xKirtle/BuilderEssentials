using System.Collections.Generic;
using System.Drawing;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIPanels
{
    public class WrenchUpgradeButtons : CustomUIPanel
    {
        private Texture2D OffTexture = ModContent.GetTexture("BuilderEssentials/Textures/UIElements/Upgrades/ToggleOFF");
        private Texture2D OnTexture = ModContent.GetTexture("BuilderEssentials/Textures/UIElements/Upgrades/ToggleON");
        private CustomUIImageButton[] elements;
        private UIText text;

        public WrenchUpgradeButtons()
        {
            Width.Set(90f, 0);
            Height.Set(20f, 0);
            Left.Set(40f, 0);
            Top.Set(3f, 0);
            BackgroundColor = Microsoft.Xna.Framework.Color.Transparent;
            BorderColor = Microsoft.Xna.Framework.Color.Transparent;
            SetPadding(0);

            elements = new CustomUIImageButton[HelperMethods.WrenchUpgrade.UpgradesCount.ToInt()];
            for (int i = 0; i < elements.Length; i++)
            {
                int index = i;
                CustomUIImageButton toggle = new CustomUIImageButton(OffTexture, 1f);
                toggle.Left.Set(OffTexture.Width * i + 7f, 0);
                toggle.Top.Set(1f, 0);
                toggle.OnMouseOver += (__, _) => { text = new UIText(names[index]); Append(text); };
                toggle.OnMouseOut += (__, _) => { text?.Remove(); text = null; };
                elements[i] = toggle;
                Append(toggle);
            }
        }

        string[] names =
        {
            "Fast Placement",
            "Infinite Placement Range",
            "Infinite Player Range",
            "Placement Anywhere",
            "Infinite Placement"
        };

        public void UpdateUpgrades(Player player, ref List<bool> upgrades, ref List<bool> unlockedUpgrades)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            if (upgrades[0]) //Fast placement
                mp.FastPlacement = true;
            if (upgrades[1]) //Inf Placement Range
                mp.InfinitePlacementRange = true;
            if (upgrades[2]) //Inf Player Range
                mp.InfinitePlayerRange = true;
            if (upgrades[3]) //Placement Anywhere
                mp.PlacementAnywhere = true;
            if (upgrades[4]) //Inf Placement
                mp.InfinitePlacement = true;

            //Updating UI upgrade values through the wrench
            for (int i = 0; i < upgrades.Count; i++)
            {
                if (!Visible) //Runs once before the wrench calls the Show() method
                {
                    //Updating UI behaviour and leaving unlocked upgrades toggled
                    elements[i].SetOpacity(unlockedUpgrades[i] ? 1f : .45f);
                    elements[i].SetToggleable(unlockedUpgrades[i]);
                    elements[i].SetToggled(unlockedUpgrades[i]);
                }
            }

            //Setting upgrade values through the UI
            for (int i = 0; i < upgrades.Count; i++)
                if (Visible && unlockedUpgrades[i]) 
                    upgrades[i] = elements[i].Toggled;
        }

        public void Update()
        {
            Left.Set(Main.playerInventory ? 115f : 40f, 0);

            if (text == null) return;
            text.Left.Set(Main.mouseX + 11f - Left.Pixels, 0);
            text.Top.Set(Main.mouseY + 11f - Top.Pixels / 2, 0);
        }
    }
}