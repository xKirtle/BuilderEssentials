using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using BuilderEssentials.Items;
using BuilderEssentials.UI.Elements;

namespace BuilderEssentials.UI.UIPanels
{
    public class FillWandSelection : CustomUIElement
    {
        readonly Texture2D texture = Main.extraTexture[2];
        readonly Rectangle value = new Rectangle(0, 0, 16, 16);
        readonly Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < FillWand.fillSelectionSize; i++)
            {
                for (int j = 0; j < FillWand.fillSelectionSize; j++)
                {
                    //Centers in the bottom left corner of the filling square
                    Vector2 position = new Vector2(Player.tileTargetX + j, 
                        Player.tileTargetY + i - FillWand.fillSelectionSize + 1) * 16 - Main.screenPosition;
                    spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, .96f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}