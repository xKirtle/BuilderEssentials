﻿using System;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.UI;
using Terraria.DataStructures;

namespace BuilderEssentials.Utilities
{
    public class CoordsSelection
    {
        public int itemType = -1;
        public bool shiftDown;

        public bool RMBDown;
        public Vector2 RMBStart = Vector2.Zero;
        public Vector2 RMBEnd = Vector2.Zero;

        public bool LMBDown;
        public Vector2 LMBStart = Vector2.Zero;
        public Vector2 LMBEnd = Vector2.Zero;

        public bool MMBDown;
        public Vector2 MMBStart = Vector2.Zero;
        public Vector2 MMBEnd = Vector2.Zero;

        public CoordsSelection(int itemToWorkWith)
        {
            itemType = itemToWorkWith;
            
            UIStateLogic1.Instance.OnRightMouseDown += OnRightMouseDown;
            UIStateLogic1.Instance.OnRightMouseUp += OnRightMouseUp;
            UIStateLogic1.Instance.OnMouseDown += OnMouseDown;
            UIStateLogic1.Instance.OnMouseUp += OnMouseUp;
            UIStateLogic1.Instance.OnMiddleMouseDown += OnMiddleMouseDown;
            UIStateLogic1.Instance.OnMiddleMouseUp += OnMiddleMouseUp;
        }

        private void OnRightMouseDown(UIMouseEvent evt, UIElement listeningelement)
        {
            if (Main.LocalPlayer.HeldItem.type != itemType) return;

            RMBDown = true;
            RMBStart = RMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
        }

        private void OnRightMouseUp(UIMouseEvent evt, UIElement listeningelement)
        {
            RMBDown = false;
        }

        private void OnMouseDown(UIMouseEvent evt, UIElement listeningelement)
        {
            if (Main.LocalPlayer.HeldItem.type != itemType) return;

            LMBDown = true;
            LMBStart = LMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
        }

        private void OnMouseUp(UIMouseEvent evt, UIElement listeningelement)
        {
            LMBDown = false;
        }
        
        private void OnMiddleMouseDown(UIMouseEvent evt, UIElement listeningelement)
        {
            if (Main.LocalPlayer.HeldItem.type != itemType) return;

            MMBDown = true;
            MMBStart = MMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
        }
        
        private void OnMiddleMouseUp(UIMouseEvent evt, UIElement listeningelement)
        {
            MMBDown = false;
        }

        public void SquareCoords(ref Vector2 start, ref Vector2 end)
        {
            int distanceX = (int) (end.X - start.X);
            int distanceY = (int) (end.Y - start.Y);

            //Turning rectangles into a square
            if (Math.Abs(distanceX) < Math.Abs(distanceY)) //Horizontal
            {
                if (distanceX > 0) //I. and IV. Quadrant
                    end.X = start.X + Math.Abs(distanceY);
                else //II. and III. Quadrant
                    end.X = start.X - Math.Abs(distanceY);
            }
            else //Vertical
            {
                if (distanceY > 0) //III. and IV. Quadrant
                    end.Y = start.Y + Math.Abs(distanceX);
                else //I. and II. Quadrant
                    end.Y = start.Y - Math.Abs(distanceX);
            }
        }

        public void UpdateCoords(bool bezierSelection = false)
        {
            if (Main.LocalPlayer.HeldItem.type != itemType)
            { RMBDown = LMBDown = MMBDown = false; return; }

            if (RMBDown)
                RMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);

            if (LMBDown)
                LMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
            
            if (MMBDown)
                MMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);

            if (bezierSelection && LMBDown && LMBStart != RMBEnd)
                RMBEnd = LMBStart;
            
            shiftDown = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            if (!shiftDown) return;
            
            if (RMBDown) 
                SquareCoords(ref RMBStart, ref RMBEnd);
            else if (LMBDown)
                SquareCoords(ref LMBStart, ref LMBEnd);
            else if (MMBDown)
                SquareCoords(ref MMBStart, ref MMBEnd);
        }
    }
}