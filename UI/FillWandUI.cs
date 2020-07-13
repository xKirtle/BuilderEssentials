using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.Items;

namespace BuilderEssentials.UI
{
    public class FillWandUI : UIElement
    {
        Texture2D texture = Main.extraTexture[2];
        Rectangle value = new Rectangle(0, 0, 16, 16);
        Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
        Vector2 position = new Vector2();

        public static Texture2D test = Main.tileTexture[0];
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<FillWand>())
            {
                for (int i = 0; i < FillWand.fillSelectionSize; i++)
                {
                    for (int j = 0; j < FillWand.fillSelectionSize; j++)
                    {
                        //Centers in the bottom left corner of the filling square
                        position = new Vector2(Player.tileTargetX + j, Player.tileTargetY + i - FillWand.fillSelectionSize + 1) * 16 - Main.screenPosition;
                        spriteBatch.Draw(test, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}