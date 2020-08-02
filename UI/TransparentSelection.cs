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
            if ((MirrorWand.selectionStart.X != MirrorWand.selectionEnd.X || MirrorWand.selectionStart.Y != MirrorWand.selectionEnd.Y)
            && (MirrorWand.selectionStart != Vector2.Zero && MirrorWand.selectionEnd != Vector2.Zero))
            {
                Texture2D texture = Main.extraTexture[2];
                Rectangle value = new Rectangle(0, 0, 16, 16);
                Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
                Vector2 position = new Vector2();

                //Retrieve which way the player is making the selection
                //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
                if (MirrorWand.firstSelectionValue)
                {
                    if (MirrorWand.selectionStart.X < MirrorWand.selectionEnd.X && MirrorWand.selectionStart.Y < MirrorWand.selectionEnd.Y)
                        selectedQuarter = 3;
                    else if (MirrorWand.selectionStart.X < MirrorWand.selectionEnd.X && MirrorWand.selectionStart.Y > MirrorWand.selectionEnd.Y)
                        selectedQuarter = 1;
                    else if (MirrorWand.selectionStart.X > MirrorWand.selectionEnd.X && MirrorWand.selectionStart.Y > MirrorWand.selectionEnd.Y)
                        selectedQuarter = 0;
                    else if (MirrorWand.selectionStart.X > MirrorWand.selectionEnd.X && MirrorWand.selectionStart.Y < MirrorWand.selectionEnd.Y)
                        selectedQuarter = 2;

                    distanceX = Math.Abs(MirrorWand.selectionEnd.X - MirrorWand.selectionStart.X);
                    distanceY = Math.Abs(MirrorWand.selectionEnd.Y - MirrorWand.selectionStart.Y);
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

            //MIRROR AXIS
            if ((MirrorWand.mirrorStart.X != MirrorWand.mirrorEnd.X || MirrorWand.mirrorStart.Y != MirrorWand.mirrorEnd.Y)
                && (MirrorWand.mirrorStart != Vector2.Zero && MirrorWand.mirrorEnd != Vector2.Zero))
            {
                Texture2D texture = Main.extraTexture[2];
                Rectangle value = new Rectangle(0, 0, 16, 16);
                Color color = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f;
                Vector2 position = new Vector2();

                //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
                if (MirrorWand.firstMirrorValue)
                {   
                    //Quarter
                    if (MirrorWand.mirrorStart.X < MirrorWand.mirrorEnd.X && MirrorWand.mirrorStart.Y < MirrorWand.mirrorEnd.Y)
                        selectedLeftQuarter = 3;
                    else if (MirrorWand.mirrorStart.X < MirrorWand.mirrorEnd.X && MirrorWand.mirrorStart.Y > MirrorWand.mirrorEnd.Y)
                        selectedLeftQuarter = 1;
                    else if (MirrorWand.mirrorStart.X > MirrorWand.mirrorEnd.X && MirrorWand.mirrorStart.Y > MirrorWand.mirrorEnd.Y)
                        selectedLeftQuarter = 0;
                    else if (MirrorWand.mirrorStart.X > MirrorWand.mirrorEnd.X && MirrorWand.mirrorStart.Y < MirrorWand.mirrorEnd.Y)
                        selectedLeftQuarter = 2;

                    distanceXLeftMouse = Math.Abs(MirrorWand.mirrorEnd.X - MirrorWand.mirrorStart.X);
                    distanceYLeftMouse = Math.Abs(MirrorWand.mirrorEnd.Y - MirrorWand.mirrorStart.Y);
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
                        position = new Vector2(MirrorWand.selectionStart.X + iteration, MirrorWand.selectionStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X + iteration, MirrorWand.selectionStart.Y + distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.selectionStart.X, MirrorWand.selectionStart.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X + distanceX, MirrorWand.selectionStart.Y + iteration) * 16 - Main.screenPosition;
                }
            }

            void TopRight()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.selectionStart.X + iteration, MirrorWand.selectionStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X + iteration, MirrorWand.selectionStart.Y - distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.selectionStart.X, MirrorWand.selectionStart.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X + distanceX, MirrorWand.selectionStart.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void TopLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.selectionStart.X - iteration, MirrorWand.selectionStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X - iteration, MirrorWand.selectionStart.Y - distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.selectionStart.X, MirrorWand.selectionStart.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X - distanceX, MirrorWand.selectionStart.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void BottomLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.selectionStart.X - iteration, MirrorWand.selectionStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X - iteration, MirrorWand.selectionStart.Y + distanceY) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.selectionStart.X, MirrorWand.selectionStart.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.selectionStart.X - distanceX, MirrorWand.selectionStart.Y + iteration) * 16 - Main.screenPosition;
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
                        position = new Vector2(MirrorWand.mirrorStart.X + iteration, MirrorWand.mirrorStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X + iteration, MirrorWand.mirrorStart.Y + distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mirrorStart.X, MirrorWand.mirrorStart.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X + distanceXLeftMouse, MirrorWand.mirrorStart.Y + iteration) * 16 - Main.screenPosition;
                }
            }

            void TopRight()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mirrorStart.X + iteration, MirrorWand.mirrorStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X + iteration, MirrorWand.mirrorStart.Y - distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mirrorStart.X, MirrorWand.mirrorStart.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X + distanceXLeftMouse, MirrorWand.mirrorStart.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void TopLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mirrorStart.X - iteration, MirrorWand.mirrorStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X - iteration, MirrorWand.mirrorStart.Y - distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mirrorStart.X, MirrorWand.mirrorStart.Y - iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X - distanceXLeftMouse, MirrorWand.mirrorStart.Y - iteration) * 16 - Main.screenPosition;
                }
            }

            void BottomLeft()
            {
                if (isAxisX)
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mirrorStart.X - iteration, MirrorWand.mirrorStart.Y) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X - iteration, MirrorWand.mirrorStart.Y + distanceYLeftMouse) * 16 - Main.screenPosition;
                }
                else
                {
                    if (isTopOrLeft)
                        position = new Vector2(MirrorWand.mirrorStart.X, MirrorWand.mirrorStart.Y + iteration) * 16 - Main.screenPosition;
                    else
                        position = new Vector2(MirrorWand.mirrorStart.X - distanceXLeftMouse, MirrorWand.mirrorStart.Y + iteration) * 16 - Main.screenPosition;
                }
            }
            return position;
        }

        private bool IsMirrorAxisInsideSelection()
        {
            if (MirrorWand.VerticalLine) //X Mirror Axis
            {
                if (Tools.IsWithinRange(MirrorWand.mirrorStart.X, MirrorWand.selectionStart.X, MirrorWand.selectionEnd.X) &&
                    Tools.IsWithinRange(MirrorWand.mirrorEnd.X, MirrorWand.selectionStart.X, MirrorWand.selectionEnd.X))
                    return true;
            }
            else if (MirrorWand.HorizontalLine) //Y Mirror Axis
            {
                if (Tools.IsWithinRange(MirrorWand.mirrorStart.Y, MirrorWand.selectionStart.Y, MirrorWand.selectionEnd.Y) &&
                    Tools.IsWithinRange(MirrorWand.mirrorEnd.Y, MirrorWand.selectionStart.Y, MirrorWand.selectionEnd.Y))
                    return true;
            }

            return false;
        }
    }
}