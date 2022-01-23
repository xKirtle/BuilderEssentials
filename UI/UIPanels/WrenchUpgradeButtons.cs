using System.Collections.Generic;
using System.Drawing;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIPanels
{
    internal class WrenchUpgradeButtons : CustomUIPanel
    {
        private Asset<Texture2D> OffTexture =
            ModContent.Request<Texture2D>("BuilderEssentials/Textures/UIElements/Upgrades/ToggleOFF");
        private Asset<Texture2D> OnTexture = 
            ModContent.Request<Texture2D>("BuilderEssentials/Textures/UIElements/Upgrades/ToggleON");
        private CustomUIImageButton[] elements;
        private UIText text;

        public WrenchUpgradeButtons()
        {
            Width.Set(90f, 0);
            Height.Set(40f, 0);
            Left.Set(40f, 0);
            Top.Set(3f, 0); 
            BackgroundColor = Microsoft.Xna.Framework.Color.Transparent;
            BorderColor = Microsoft.Xna.Framework.Color.Transparent;
            SetPadding(0);

            elements = new CustomUIImageButton[(int)HelperMethods.WrenchUpgrade.UpgradesCount];
            for (int i = 0; i < elements.Length; i++)
            {
                int index = i;
                CustomUIImageButton toggle = new CustomUIImageButton(OffTexture);
                toggle.Left.Set(OffTexture.Value.Width * i + 7f, 0);
                toggle.Top.Set(1f, 0);
                toggle.OnMouseOver += (__, _) =>
                {
                    text = new UIText(names[index]);
                    Append(text);
                };
                toggle.OnMouseOut += (__, _) =>
                {
                    text?.Remove();
                    text = null;
                };
                elements[i] = toggle;
                Append(toggle);
            }
        }

        string[] names =
        {
            "Fast Placement",
            "Infinite Player Range",
            "Placement Anywhere",
            "Infinite Placement",
            "Infinite Pickup Range"
        };

        public void UpdateUpgrades(Player player, ref List<bool> upgrades, ref List<bool> unlockedUpgrades)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            if (upgrades[0]) //Fast placement
                mp.FastPlacement = true;
            if (upgrades[1]) //Inf Player Range
                mp.InfinitePlayerRange = true;
            if (upgrades[2]) //Placement Anywhere
                mp.PlacementAnywhere = true;
            if (upgrades[3]) //Inf Placement
                mp.InfinitePlacement = true;
            if (upgrades[4]) //Inf Pickup Range
                mp.InfinitePickupRange = true;

            //Updating UI upgrade values through the wrench
            for (int i = 0; i < upgrades.Count; i++)
            {
                //Runs once before the wrench calls the Show() method
                if (Visible) continue;

                //Updating UI behaviour and leaving unlocked upgrades toggled
                elements[i].SetToggleable(unlockedUpgrades[i]);
                elements[i].SetOpacity(upgrades[i] ? 1f : .45f);
                elements[i].SetToggled(upgrades[i]);
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