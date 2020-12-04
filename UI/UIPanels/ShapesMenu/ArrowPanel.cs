using BuilderEssentials.UI.Elements;
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
            Left.Set(-15f, 0);
            Top.Set(Main.screenHeight / 2, 0);
            BorderColor = Microsoft.Xna.Framework.Color.Transparent;
            BackgroundColor = Microsoft.Xna.Framework.Color.Transparent;
            OnMouseDown += (__, _) =>
            {
                Hide();
                //Show shapes menu
            };

            Texture2D texture = ModContent.GetTexture("BuilderEssentials/Textures/UIElements/ShapesMenu/ArrowPanel");
            CustomUIImage arrowPanelImg = new CustomUIImage(texture, 1f);
            arrowPanelImg.Width.Set(texture.Width, 0);
            arrowPanelImg.Height.Set(texture.Height, 0);
            arrowPanelImg.Left.Set(3f, 0);
            arrowPanelImg.Top.Set(-12f, 0);

            Append(arrowPanelImg);
        }
    }
}