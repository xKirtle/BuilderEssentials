using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using BuilderEssentials.Items;

namespace BuilderEssentials.UI
{
    public class TransparentSelection : UIElement
    {
        bool selectionDone = false;
        float distanceX;
        float distanceY;

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if ((MirrorWand.start.X != 0 || MirrorWand.start.Y != 0))
            {
                Texture2D texture = Main.extraTexture[2];
                Rectangle value = new Rectangle(0, 0, 16, 16);
                Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
                Vector2 position = new Vector2(MirrorWand.start.X, MirrorWand.start.Y) * 16 - Main.screenPosition;

                //TODO: ADD ALL 3 OTHER DIRECTIONS

                //Down Right
                if ((MirrorWand.start.X < MirrorWand.end.X && MirrorWand.start.Y < MirrorWand.end.Y))
                {
                    distanceX = (MirrorWand.end.X - MirrorWand.start.X);
                    distanceY = (MirrorWand.end.Y - MirrorWand.start.Y);

                    Main.NewText(distanceX + " " + distanceY);

                    //X Axis
                    for (int i = 0; i < distanceX + 1; i++)
                    {
                        //Top X
                        position = new Vector2(MirrorWand.start.X + i, MirrorWand.start.Y) * 16 - Main.screenPosition;
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Bottom X
                        position = new Vector2(MirrorWand.start.X + i, MirrorWand.start.Y + distanceY) * 16 - Main.screenPosition;
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }

                    //Y Axis
                    for (int i = 0; i < distanceY + 1; i++)
                    {
                        //Left Y
                        position = new Vector2(MirrorWand.start.X, MirrorWand.start.Y + i) * 16 - Main.screenPosition;
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Right Y
                        position = new Vector2(MirrorWand.start.X + distanceX, MirrorWand.start.Y + i) * 16 - Main.screenPosition;
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }

                    selectionDone = true;
                }
            }
        }
    }
}