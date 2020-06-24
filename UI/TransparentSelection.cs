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
        Vector2 areaOfSelection;

        public TransparentSelection()
        {
            //this.areaOfSelection = vector;
        }

        public override void Update(GameTime gameTime)
        {
            this.RecalculateChildren();
            this.Recalculate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Weird behaviour??
            //Coordinates can be wrong sometimes, could be what comes from Mirro Wand
            Vector2 start = MirrorWand.start;
            Vector2 end = MirrorWand.end;
            if (start.X != 0 && start.Y != 0 && MirrorWand.OperationComplete)
            {
                Texture2D texture = Main.extraTexture[2];
                Rectangle value = new Rectangle(0, 0, 16, 16);
                Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
                Vector2 position = new Vector2(start.X, start.Y) * 16 - Main.screenPosition;
                Vector2 position2 = new Vector2(start.X, start.Y) * 16 - Main.screenPosition;

                //Down Right
                if (start.X < end.X && start.Y < end.Y)
                {
                    float distanceX = (end.X - start.X) + 1;
                    float distanceY = (end.Y - start.Y) + 1;
                    Main.NewText("X: " + distanceX + " " + "y: " + distanceY);
                    Main.NewText(distanceX * distanceY);

                    //X Axis
                    for (int i = 0; i < distanceX; i++)
                    {
                        //Top X
                        position.Y = start.Y;
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Bottom X
                        position.Y = start.Y + distanceY * 16;
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        position.X += 16;
                    }

                    //Y Axis
                    for (int i = 0; i < distanceY; i++)
                    {
                        //Left Y
                        position2.X = start.X;
                        spriteBatch.Draw(texture, position2, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Right Y
                        position2.X = start.X + distanceX * 16;
                        spriteBatch.Draw(texture, position2, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        position2.Y += 16;
                    }
                }
            }
        }
    }
}