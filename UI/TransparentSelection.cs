using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using BuilderEssentials.Items;
using System;

namespace BuilderEssentials.UI
{
    public class TransparentSelection : UIElement
    {
        float distanceX;
        float distanceY;

        public override void Update(GameTime gameTime)
        {

        }

        byte selectedQuarter = 4;
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Values are initialized with 0 and single click will make start == end
            if ((MirrorWand.start.X != MirrorWand.end.X || MirrorWand.start.Y != MirrorWand.end.Y))
            {
                //TODO: MAKE THE SPRITEBATCH NOT DRAW OVER UI ELEMENTS
                //layerDepth doesn't work as spriteBatch uses SpriteSortMode.Deferred which means last draw calls will draw over
                //previous draw calls. Need to call this draw method before the UI for the player inventory/accessories is drawn

                Texture2D texture = Main.extraTexture[2];
                Rectangle value = new Rectangle(0, 0, 16, 16);
                Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
                Vector2 position = new Vector2();

                //Retrieve which way the player is making the selection
                //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
                if (MirrorWand.firstValue)
                {
                    if (MirrorWand.start.X < MirrorWand.end.X && MirrorWand.start.Y < MirrorWand.end.Y)
                        selectedQuarter = 3;
                    else if (MirrorWand.start.X < MirrorWand.end.X && MirrorWand.start.Y > MirrorWand.end.Y)
                        selectedQuarter = 1;
                    else if (MirrorWand.start.X > MirrorWand.end.X && MirrorWand.start.Y > MirrorWand.end.Y)
                        selectedQuarter = 0;
                    else if (MirrorWand.start.X > MirrorWand.end.X && MirrorWand.start.Y < MirrorWand.end.Y)
                        selectedQuarter = 2;

                    distanceX = Math.Abs(MirrorWand.end.X - MirrorWand.start.X);
                    distanceY = Math.Abs(MirrorWand.end.Y - MirrorWand.start.Y);
                }

                if (selectedQuarter != 4) //Doesn't allow it to run once without clicking right click
                {
                    //X Axis
                    for (int i = 0; i < distanceX + 1; i++)
                    {
                        //Top:Bottom X
                        position = GetVectorBasedOnDirection(selectedQuarter, true, true, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Bottom:Top X
                        position = GetVectorBasedOnDirection(selectedQuarter, true, false, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }

                    //Y Axis
                    for (int i = 0; i < distanceY + 1; i++)
                    {
                        //Left:Right Y
                        position = GetVectorBasedOnDirection(selectedQuarter, false, true, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Right:Left Y
                        position = GetVectorBasedOnDirection(selectedQuarter, false, false, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        private Vector2 GetVectorBasedOnDirection(byte currentDirection, bool isAxisX, bool isTopOrLeft, int iteration)
        {
            Vector2 position = new Vector2();
            switch (currentDirection)
            {
                case 0:
                    TopLeft();
                    break;
                case 1:
                    UpRight();
                    break;
                case 2:
                    BottomLeft();
                    break;
                case 3:
                    BottomRight();
                    break;
            }

            void BottomRight()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X + iteration, MirrorWand.start.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X + iteration, MirrorWand.start.Y + distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X, MirrorWand.start.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X + distanceX, MirrorWand.start.Y + iteration) * 16 - Main.screenPosition;
                }
            }

            void UpRight()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X + iteration, MirrorWand.start.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X + iteration, MirrorWand.start.Y - distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X, MirrorWand.start.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X + distanceX, MirrorWand.start.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void TopLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X - iteration, MirrorWand.start.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X - iteration, MirrorWand.start.Y - distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X, MirrorWand.start.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X - distanceX, MirrorWand.start.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void BottomLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X - iteration, MirrorWand.start.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X - iteration, MirrorWand.start.Y + distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.start.X, MirrorWand.start.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.start.X - distanceX, MirrorWand.start.Y + iteration) * 16 - Main.screenPosition;
                }
            }
            return position;
        }
    }
}