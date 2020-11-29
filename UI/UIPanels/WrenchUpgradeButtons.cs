using System.Drawing;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels
{
    public class WrenchUpgradeButtons : CustomUIPanel
    {
        private Texture2D OffTexture =
            ModContent.GetTexture("BuilderEssentials/Textures/UIElements/Upgrades/ToggleOFF");

        private Texture2D OnTexture = ModContent.GetTexture("BuilderEssentials/Textures/UIElements/Upgrades/ToggleON");
        private CustomUIImageButton[] elements;

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
                CustomUIImageButton imageButton = new CustomUIImageButton(OffTexture, 1f);
                imageButton.Left.Set(OffTexture.Width * i + 7f, 0);
                imageButton.Top.Set(2f, 0);
                imageButton.SetToggleable(true);
                elements[i] = imageButton;
                Append(imageButton);
            }
        }

        public void Update()
        {
            Left.Set(Main.playerInventory ? 115f : 40f, 0);
        }
    }
}