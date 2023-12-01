using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.UI;

namespace BuilderEssentials.Common.DataStructures;

public class MouseSelection
{
    public Vector2 Start { get; internal set; }
    public Vector2 End { get; internal set; }
    public bool IsDown { get; private set; }
    public event UIElement.ElementEvent OnClick;
    public event UIElement.ElementEvent OnMouseDown;
    public Func<bool> CanUpdateCoords = () => true;

    public void MouseDown(UIMouseEvent evt, UIElement element) {
        if (CanUpdateCoords() && !Main.LocalPlayer.mouseInterface) {
            Start = End = new Vector2(Player.tileTargetX, Player.tileTargetY);
            IsDown = true;
            // OnMouseDown?.Invoke(element);
        }
        OnMouseDown?.Invoke(element);
    }

    public void MouseUp(UIMouseEvent evt, UIElement element) {
        if (CanUpdateCoords() && !Main.LocalPlayer.mouseInterface) {
            End = new Vector2(Player.tileTargetX, Player.tileTargetY);
            if (Main.keyState.IsKeyDown(Keys.LeftShift))
                SquareCoords();
            IsDown = false;
            OnClick?.Invoke(element);
        }
    }

    public void UpdateCoords() {
        if (IsDown && CanUpdateCoords() && !Main.LocalPlayer.mouseInterface) {
            End = new Vector2(Player.tileTargetX, Player.tileTargetY);
            if (Main.keyState.IsKeyDown(Keys.LeftShift))
                SquareCoords();
        }
    }

    private void SquareCoords() {
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
    public MouseSelection LeftMouse { get; }
    public MouseSelection RightMouse { get; }
    public MouseSelection MiddleMouse { get; }
    public Func<bool> CanUpdateCoords;

    public CoordSelection(UIState instance, Func<bool> canUpdateCoords = null) {
        LeftMouse = new MouseSelection();
        RightMouse = new MouseSelection();
        MiddleMouse = new MouseSelection();

        instance.OnLeftMouseDown += LeftMouse.MouseDown;
        instance.OnLeftMouseUp += LeftMouse.MouseUp;
        instance.OnRightMouseDown += RightMouse.MouseDown;
        instance.OnRightMouseUp += RightMouse.MouseUp;
        instance.OnMiddleMouseDown += MiddleMouse.MouseDown;
        instance.OnMiddleMouseUp += MiddleMouse.MouseUp;

        CanUpdateCoords = canUpdateCoords ??= () => true;
        LeftMouse.CanUpdateCoords = CanUpdateCoords;
        RightMouse.CanUpdateCoords = CanUpdateCoords;
        MiddleMouse.CanUpdateCoords = CanUpdateCoords;
    }

    public void UpdateCoords() {
        RightMouse.UpdateCoords();
        LeftMouse.UpdateCoords();
        MiddleMouse.UpdateCoords();
    }

    public static bool IsWithinRange(float number, float value1, float value2, bool equal = false) {
        if (!equal)
            return number > value1 && number < value2 || number < value1 && number > value2;
        else
            return number >= value1 && number <= value2 || number <= value1 && number >= value2;
    }

    //TODO: A way to know in which quadrant the end of the selection is (based on starting point)
}