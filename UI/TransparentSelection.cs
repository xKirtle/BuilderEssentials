using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria;
using BuilderEssentials.Items;
using System;
using BuilderEssentials.Utilities;
using BuilderEssentials.UI.ShapesDrawing;
using Microsoft.Xna.Framework.Input;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.UI
{
    public class TransparentSelectionUI : UIState
    {
        public static TransparentSelectionUI Instance;
        public override void OnInitialize()
        {
            Instance = this;

            OnRightMouseDown += DragStartSelection;
            OnRightMouseUp += DragEndSelection;

            OnMouseDown += DragStartMirror;
            OnMouseUp += DragEndMirror;

            UIElement transparentSelectionWand = new TransparentSelection();
            Append(transparentSelectionWand);

            UIElement fillWand = new FillWandUI();
            Append(fillWand);
        }

        public bool shiftPressed;

        public bool draggingSel;
        public Vector2 startSel = Vector2.Zero;
        public Vector2 endSel = Vector2.Zero;
        private void DragStartSelection(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.HeldItem.type == ItemType<MirrorWand>())
            {
                draggingSel = true;
                startSel = endSel = new Vector2(Player.tileTargetX, Player.tileTargetY);
            }
        }
        private void DragEndSelection(UIMouseEvent evt, UIElement listeningElement) => draggingSel = false;

        public bool draggingMirror;
        public Vector2 startMirror = Vector2.Zero;
        public Vector2 endMirror = Vector2.Zero;
        public bool horizontalSelection;
        public bool validMirrorPlacement;
        private void DragStartMirror(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.HeldItem.type == ItemType<MirrorWand>())
            {
                draggingMirror = true;
                startMirror = endMirror = new Vector2(Player.tileTargetX, Player.tileTargetY);
            }
        }
        private void DragEndMirror(UIMouseEvent evt, UIElement listeningElement) => draggingMirror = false;

        public override void Update(GameTime gameTime)
        {
            if (draggingSel)
                endSel = new Vector2(Player.tileTargetX, Player.tileTargetY);

            if (draggingMirror)
                endMirror = new Vector2(Player.tileTargetX, Player.tileTargetY);

            //Disable ingame shift while shiftPressed?
            shiftPressed = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
        }
    }

    class TransparentSelection : BaseShape
    {
        TransparentSelectionUI ts = TransparentSelectionUI.Instance;

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Selection
            if (ts.startSel != ts.endSel)
                DrawRectangle((int)ts.startSel.X, (int)ts.startSel.Y, (int)ts.endSel.X, (int)ts.endSel.Y);

            //Mirror

            //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
            int selectedQuarter = SelectedQuarter((int)ts.startMirror.X, (int)ts.startMirror.Y, (int)ts.endMirror.X, (int)ts.endMirror.Y);

            int distanceX = (int)(ts.endMirror.X - ts.startMirror.X);
            int distanceY = (int)(ts.endMirror.Y - ts.startMirror.Y);
            ts.horizontalSelection = Math.Abs(distanceX) > Math.Abs(distanceY);

            //selectedQuarter 0 and 1 is the same as 2 and 3 inversed
            //true true, false true | true false, false false.
            bool c2 = true;
            if (selectedQuarter >= 2)
            {
                c2 = !c2;
                selectedQuarter -= 2;
            }

            FixX(!Convert.ToBoolean(selectedQuarter));
            FixY(c2);

            void FixX(bool left = true)
            {
                if (left && !ts.horizontalSelection && ts.endMirror.X < ts.startMirror.X - 1)
                    ts.endMirror.X = ts.startMirror.X - 1;
                else if (!left && !ts.horizontalSelection && ts.endMirror.X > ts.startMirror.X + 1)
                    ts.endMirror.X = ts.startMirror.X + 1;
            }

            void FixY(bool top = true)
            {
                if (top && ts.horizontalSelection && ts.endMirror.Y < ts.startMirror.Y - 1)
                    ts.endMirror.Y = ts.startMirror.Y - 1;
                else if (!top && ts.horizontalSelection && ts.endMirror.Y > ts.startMirror.Y + 1)
                    ts.endMirror.Y = ts.startMirror.Y + 1;
            }


            ts.validMirrorPlacement = IsMirrorAxisInsideSelection();

            int newDistanceX = (int)(ts.endMirror.X - ts.startMirror.X);
            int newDistanceY = (int)(ts.endMirror.Y - ts.startMirror.Y);
            if (ts.startMirror != ts.endMirror &&
                ((!ts.horizontalSelection && Math.Abs(newDistanceX) >= 1) || (ts.horizontalSelection && Math.Abs(newDistanceY) >= 1)))
                DrawRectangle((int)ts.startMirror.X, (int)ts.startMirror.Y, (int)ts.endMirror.X, (int)ts.endMirror.Y);
            else if (ts.startMirror != ts.endMirror)
                DrawLine((int)ts.startMirror.X, (int)ts.startMirror.Y, (int)ts.endMirror.X, (int)ts.endMirror.Y);
        }

        private bool IsMirrorAxisInsideSelection()
        {
            return Tools.IsWithinRange(ts.startMirror.X, ts.startSel.X, ts.endSel.X) && Tools.IsWithinRange(ts.endMirror.X, ts.startSel.X, ts.endSel.X)
            && Tools.IsWithinRange(ts.startMirror.Y, ts.startSel.Y, ts.endSel.Y) && Tools.IsWithinRange(ts.endMirror.Y, ts.startSel.Y, ts.endSel.Y);
        }
    }
}