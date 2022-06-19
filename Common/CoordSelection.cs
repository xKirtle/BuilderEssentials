using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.UI;

namespace BuilderEssentials.Common;

public class MouseSelection
{
    public Vector2 Start { get; private set; }
    public Vector2 End { get; private set; }
    public bool IsDown { get; private set; }
    public event UIElement.ElementEvent OnClick;
    
    public void MouseDown(UIMouseEvent evt, UIElement element) {
        Start = End = new Vector2(Player.tileTargetX, Player.tileTargetY);
        IsDown = true;
    }

    public void MouseUp(UIMouseEvent evt, UIElement element) {
        End = new Vector2(Player.tileTargetX, Player.tileTargetY);
        IsDown = false;
        OnClick?.Invoke(element);
    }

    public void UpdateCoords(bool shiftDown) {
        if (IsDown) {
            End = new Vector2(Player.tileTargetX, Player.tileTargetY);
            if (shiftDown) SquareCoords();
        }
    }

    void SquareCoords() {
        int distanceX = (int) (End.X - Start.X);
        int distanceY = (int) (End.Y - Start.Y);

        //Turning rectangle into a square
        if (Math.Abs(distanceX) < Math.Abs(distanceY)) {
            //Horizontal
            float endX;
            if (distanceX > 0) //I. and IV. Quadrant
                endX = Start.X + Math.Abs(distanceY);
            else //II. and III. Quadrant
                endX = Start.X - Math.Abs(distanceY);

            End = new Vector2(endX, End.Y);
        }
        else {
            //Vertical
            float endY;
            if (distanceY > 0) //III. and IV. Quadrant
                endY = Start.Y + Math.Abs(distanceX);
            else //I. and II. Quadrant
                endY = Start.Y - Math.Abs(distanceX);

            End = new Vector2(End.X, endY);
        }
    }
}

public class CoordSelection
{
    //Kirtle: Allow this to be changed in a ModConfig?
    private bool shiftDown;
    public MouseSelection RightMouse { get; }
    public MouseSelection LeftMouse { get; }
    public MouseSelection MiddleMouse { get; }

    public CoordSelection(UIState instance) {
        RightMouse = new();
        LeftMouse = new();
        MiddleMouse = new();
        
        instance.OnRightMouseDown += RightMouse.MouseDown;
        instance.OnRightMouseUp += RightMouse.MouseUp;
        instance.OnMouseDown += LeftMouse.MouseDown;
        instance.OnMouseUp += LeftMouse.MouseUp;
        instance.OnMiddleMouseDown += MiddleMouse.MouseDown;
        instance.OnMiddleMouseUp += MiddleMouse.MouseUp;
    }

    public void UpdateCoords() {
        shiftDown = Main.keyState.IsKeyDown(Keys.LeftShift);
        RightMouse.UpdateCoords(shiftDown);
        LeftMouse.UpdateCoords(shiftDown);
        MiddleMouse.UpdateCoords(shiftDown);
    }
}