using System;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesMenu
{
    internal class ArrowPanel : CustomUIPanel
    {
        private const float ArrowWidth = 30f, ArrowHeight = 44f;
        public ArrowPanel()
        {
            Width.Set(ArrowWidth, 0);
            Height.Set(ArrowHeight, 0);
            Left.Set(-15f, 0);
            Top.Set(Main.screenHeight / 2, 0);
            BorderColor = Microsoft.Xna.Framework.Color.Transparent;
            BackgroundColor = Microsoft.Xna.Framework.Color.Transparent;
            OnMouseDown += (__, _) => { Hide(); UIUIState.Instance.menuPanel.Show(); };

            Asset<Texture2D> texture = HelperMethods.RequestTexture("BuilderEssentials/Textures/UIElements/ShapesMenu/ArrowPanel");
            UIImage arrowPanelImg = new UIImage(texture);
            arrowPanelImg.Width.Set(texture.Width(), 0);
            arrowPanelImg.Height.Set(texture.Width(), 0);
            arrowPanelImg.Left.Set(3f, 0);
            arrowPanelImg.Top.Set(-12f, 0);
            Append(arrowPanelImg);
        }

        public override void Update(GameTime gameTime)
        {
            //Fixing UI Scale positioning
            Top.Set(Main.screenHeight / 2, 0);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
        }
    }
}