using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.UI;

namespace BuilderEssentials.Common;

public class CoordSelection
{
    //Kirtle: Allow this to be changed in a ModConfig?
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

    public CoordSelection(UIState instance) {
        instance.OnRightMouseDown += OnRightMouseDown;
        instance.OnRightMouseUp += OnRightMouseUp;
        instance.OnMouseDown += OnMouseDown;
        instance.OnMouseUp += OnMouseUp;
        instance.OnMiddleMouseDown += OnMiddleMouseDown;
        instance.OnMiddleMouseUp += OnMiddleMouseUp;
    }
    
    private void OnRightMouseDown(UIMouseEvent evt, UIElement listeningelement)
    {
        RMBDown = true;
        RMBStart = RMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
    }

    private void OnRightMouseUp(UIMouseEvent evt, UIElement listeningelement) => RMBDown = false;

    private void OnMouseDown(UIMouseEvent evt, UIElement listeningelement)
    {
        LMBDown = true;
        LMBStart = LMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
    }

    private void OnMouseUp(UIMouseEvent evt, UIElement listeningelement) => LMBDown = false;

    private void OnMiddleMouseDown(UIMouseEvent evt, UIElement listeningelement)
    {
        MMBDown = true;
        MMBStart = MMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
    }
        
    private void OnMiddleMouseUp(UIMouseEvent evt, UIElement listeningelement) => MMBDown = false;

    private void SquareCoords(ref Vector2 start, ref Vector2 end)
    {
        int distanceX = (int) (end.X - start.X);
        int distanceY = (int) (end.Y - start.Y);

        //Turning rectangle into a square
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
    
    public void UpdateCoords()
    {
        if (RMBDown)
            RMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);

        if (LMBDown)
            LMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
            
        if (MMBDown)
            MMBEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
        
        shiftDown = Main.keyState.IsKeyDown(Keys.LeftShift);

        if (shiftDown)
        {
            if (RMBDown)
                SquareCoords(ref RMBStart, ref RMBEnd);
            else if (LMBDown)
                SquareCoords(ref LMBStart, ref LMBEnd);
            else if (MMBDown)
                SquareCoords(ref MMBStart, ref MMBEnd);
        }
    }
}