using System;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesMenu
{
    public class ArrowPanel : CustomUIPanel
    {
        private const float ArrowWidth = 30f, ArrowHeight = 44f;

        public ArrowPanel()
        {
            SetSize(ArrowWidth, ArrowHeight);
            SetOffset(-15f, Main.screenHeight / 2);
            BorderColor = Microsoft.Xna.Framework.Color.Transparent;
            BackgroundColor = Microsoft.Xna.Framework.Color.Transparent;
            OnMouseDown += (__, _) => { Hide(); UIStateLogic4.menuPanel.Show(); };

            Texture2D texture = ModContent.GetTexture("BuilderEssentials/Textures/UIElements/ShapesMenu/ArrowPanel");
            CustomUIImage arrowPanelImg = new CustomUIImage(texture, 1f);
            arrowPanelImg.Width.Set(texture.Width, 0);
            arrowPanelImg.Height.Set(texture.Height, 0);
            arrowPanelImg.Left.Set(3f, 0);
            arrowPanelImg.Top.Set(-12f, 0);

            Append(arrowPanelImg);
        }

        public override void Update(GameTime gameTime)
        {
            //Fixing UI Scale positioning
            Top.Set(Main.screenHeight / 2, 0);
        }
    }
}