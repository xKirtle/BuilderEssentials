using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using BuilderEssentials.Items;
using System;
using BuilderEssentials.Utilities;

namespace BuilderEssentials.UI
{
    public class TransparentSelectionUI : UIState
    {
        public static TransparentSelectionUI transparentSelectionUI;
        public override void OnInitialize()
        {
            UIElement transparentSelectionWand = new TransparentSelection();
            Append(transparentSelectionWand);

            UIElement fillWand = new FillWandUI();
            Append(fillWand);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

    public class TransparentSelection : UIElement
    {
        float distanceX;
        float distanceY;

        float distanceXLeftMouse;
        float distanceYLeftMouse;

        public static bool validPlacement = false;
        //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
        public static byte selectedQuarter = 4;
        //0:TopBottom; 1:BottomTop; 2:LeftRight; 3:RightLeft
        public static byte selectedLeftQuarter = 4;

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Values are initialized with 0 and single click will make start == end
            if ((MirrorWand.start.X != MirrorWand.end.X || MirrorWand.start.Y != MirrorWand.end.Y)
            && (MirrorWand.start != Vector2.Zero && MirrorWand.end != Vector2.Zero))
            {
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
                    //X Axis goes full lenght from start to end and then I remove two iterations from the Y axis to adjust it in the middle of both X Axis

                    //X Axis
                    for (int i = 0; i < distanceX + 1; i++)
                    {
                        //Top:Bottom X
                        position = GetVectorBasedOnQuarterSelection(selectedQuarter, true, true, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Bottom:Top X
                        position = GetVectorBasedOnQuarterSelection(selectedQuarter, true, false, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }

                    //Y Axis
                    for (int i = 1; i < distanceY; i++)
                    {
                        //Left:Right Y
                        position = GetVectorBasedOnQuarterSelection(selectedQuarter, false, true, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Right:Left Y
                        position = GetVectorBasedOnQuarterSelection(selectedQuarter, false, false, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }

            if ((MirrorWand.mouseLeftStart.X != MirrorWand.mouseLeftEnd.X || MirrorWand.mouseLeftStart.Y != MirrorWand.mouseLeftEnd.Y)
                && (MirrorWand.mouseLeftStart != Vector2.Zero && MirrorWand.mouseLeftEnd != Vector2.Zero))
            {
                Texture2D texture = Main.extraTexture[2];
                Rectangle value = new Rectangle(0, 0, 16, 16);
                Color color = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f;
                Vector2 position = new Vector2();

                //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
                if (MirrorWand.firstvalueLeft)
                {   
                    //Quarter
                    if (MirrorWand.mouseLeftStart.X < MirrorWand.mouseLeftEnd.X && MirrorWand.mouseLeftStart.Y < MirrorWand.mouseLeftEnd.Y)
                        selectedLeftQuarter = 3;
                    else if (MirrorWand.mouseLeftStart.X < MirrorWand.mouseLeftEnd.X && MirrorWand.mouseLeftStart.Y > MirrorWand.mouseLeftEnd.Y)
                        selectedLeftQuarter = 1;
                    else if (MirrorWand.mouseLeftStart.X > MirrorWand.mouseLeftEnd.X && MirrorWand.mouseLeftStart.Y > MirrorWand.mouseLeftEnd.Y)
                        selectedLeftQuarter = 0;
                    else if (MirrorWand.mouseLeftStart.X > MirrorWand.mouseLeftEnd.X && MirrorWand.mouseLeftStart.Y < MirrorWand.mouseLeftEnd.Y)
                        selectedLeftQuarter = 2;

                    distanceXLeftMouse = Math.Abs(MirrorWand.mouseLeftEnd.X - MirrorWand.mouseLeftStart.X);
                    distanceYLeftMouse = Math.Abs(MirrorWand.mouseLeftEnd.Y - MirrorWand.mouseLeftStart.Y);
                }

                //Checking if Mirror Axis is inside the selection
                if (!IsMirrorAxisInsideSelection())
                {
                    color = new Color(1f, 0f, 0f, .75f) * 0.8f;
                    validPlacement = false;
                }
                else
                    validPlacement = true;

                if (selectedLeftQuarter != 4) //Doesn't allow it to run once without clicking right click
                {
                    //X Axis goes full lenght from start to end and then I remove two iterations from the Y axis to adjust it in the middle of both X Axis

                    //X Axis
                    for (int i = 0; i < distanceXLeftMouse + 1; i++)
                    {
                        //Top:Bottom X
                        position = GetVectorBasedOnQuarterMirrorAxis(selectedLeftQuarter, true, true, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Bottom:Top X
                        position = GetVectorBasedOnQuarterMirrorAxis(selectedLeftQuarter, true, false, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }

                    //Y Axis
                    for (int i = 1; i < distanceYLeftMouse; i++)
                    {
                        //Left:Right Y
                        position = GetVectorBasedOnQuarterMirrorAxis(selectedLeftQuarter, false, true, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        //Right:Left Y
                        position = GetVectorBasedOnQuarterMirrorAxis(selectedLeftQuarter, false, false, i);
                        spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        private Vector2 GetVectorBasedOnQuarterSelection(byte currentQuarter, bool isAxisX, bool isTopOrLeft, int iteration)
        {
            Vector2 position = new Vector2();

            switch (currentQuarter)
            {
                case 0:
                    TopLeft();
                    break;
                case 1:
                    TopRight();
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

            void TopRight()
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

        private Vector2 GetVectorBasedOnQuarterMirrorAxis(byte currentQuarter, bool isAxisX, bool isTopOrLeft, int iteration)
        {
            Vector2 position = new Vector2();

            switch (currentQuarter)
            {
                case 0:
                    TopLeft();
                    break;
                case 1:
                    TopRight();
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
                        position = new Vector2(MirrorWand.mouseLeftStart.X + iteration, MirrorWand.mouseLeftStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X + iteration, MirrorWand.mouseLeftStart.Y + distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftStart.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X + distanceXLeftMouse, MirrorWand.mouseLeftStart.Y + iteration) * 16 - Main.screenPosition;
                }
            }

            void TopRight()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mouseLeftStart.X + iteration, MirrorWand.mouseLeftStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X + iteration, MirrorWand.mouseLeftStart.Y - distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftStart.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X + distanceXLeftMouse, MirrorWand.mouseLeftStart.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void TopLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mouseLeftStart.X - iteration, MirrorWand.mouseLeftStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X - iteration, MirrorWand.mouseLeftStart.Y - distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftStart.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X - distanceXLeftMouse, MirrorWand.mouseLeftStart.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void BottomLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mouseLeftStart.X - iteration, MirrorWand.mouseLeftStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X - iteration, MirrorWand.mouseLeftStart.Y + distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftStart.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mouseLeftStart.X - distanceXLeftMouse, MirrorWand.mouseLeftStart.Y + iteration) * 16 - Main.screenPosition;
                }
            }
            return position;
        }

        private bool IsMirrorAxisInsideSelection()
        {
            if (MirrorWand.VerticalLine) //X Mirror Axis
            {
                if (Tools.IsWithinRange(MirrorWand.mouseLeftStart.X, MirrorWand.start.X, MirrorWand.end.X) &&
                    Tools.IsWithinRange(MirrorWand.mouseLeftEnd.X, MirrorWand.start.X, MirrorWand.end.X))
                    return true;
            }
            else if (MirrorWand.HorizontalLine) //Y Mirror Axis
            {
                if (Tools.IsWithinRange(MirrorWand.mouseLeftStart.Y, MirrorWand.start.Y, MirrorWand.end.Y) &&
                    Tools.IsWithinRange(MirrorWand.mouseLeftEnd.Y, MirrorWand.start.Y, MirrorWand.end.Y))
                    return true;
            }

            return false;
        }
    }
}