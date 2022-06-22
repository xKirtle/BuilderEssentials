using System;
using System.Reflection;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class MirrorPlacement
{
    
    
    public static void PlaceTile() {
        //PlaceThing_Tiles_PlaceIt
        MethodInfo placeit = Main.LocalPlayer.GetType().GetMethod("PlaceThing_Tiles_PlaceIt",
            BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);
        // MethodInfo placeit = Main.LocalPlayer.GetType().GetMethod("PlaceThing_Tiles",
        //     BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

        if (Main.LocalPlayer.HeldItem.createTile < TileID.Dirt && Main.LocalPlayer.HeldItem.createWall < WallID.Stone) return;
        
        Console.WriteLine("First Invoke");
        // placeit.Invoke(Main.LocalPlayer, new object[] { });
        placeit.Invoke(Main.LocalPlayer, new object[] {false, default(TileObject)});
        Console.WriteLine("After First Invoke");

        var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
        CoordSelection cs = panel.cs;
        Vector2 selectionStart = cs.RightMouse.Start;
        Vector2 selectionEnd = cs.RightMouse.End;
        Vector2 mirrorStart = cs.LeftMouse.Start;
        Vector2 mirrorEnd = cs.LeftMouse.End;

        if (!panel.IsMouseWithinSelection() || !panel.validMirrorPlacement) return;
        Console.WriteLine("Within Selection");

        //Not the mirror, the selection no?
        if (!cs.IsWithinRange(Player.tileTargetX, selectionStart.X, selectionEnd.X, true) &&
            !cs.IsWithinRange(Player.tileTargetY, selectionStart.Y, selectionEnd.Y, true)) return;

        Console.WriteLine("Within Range");

        int oldTargetX = 0, oldTargetY = 0, oldDir = 0;
        if (!panel.horizontalMirror) {
            Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
            TileObjectData data = TileObjectData.GetTileData(tile);
            
            int offsetX = 0;
            if (data != null) {
                //figure out multi tile origin offset fix math    
            }
            
            float minMirrorX = Math.Min(mirrorStart.X, mirrorEnd.X);
            bool leftOfTheMirror = Player.tileTargetX < minMirrorX;
            float distanceToMirror = Math.Abs(Player.tileTargetX - mirrorStart.X) < Math.Abs(Player.tileTargetX - mirrorEnd.X) 
                ? Math.Abs(Player.tileTargetX - mirrorStart.X) 
                : Math.Abs(Player.tileTargetX - mirrorEnd.X);
            
            oldTargetX = Player.tileTargetX;
            Player.tileTargetX += 
                (int) (distanceToMirror * 2 + (panel.wideMirror ? 1 : 0) + offsetX * (leftOfTheMirror ? 1 : -1));
            
            oldDir = Main.LocalPlayer.direction;
            Main.LocalPlayer.direction *= -1;
        }
        
        Console.WriteLine("Second Invoke");
        placeit.Invoke(Main.LocalPlayer, new object[] {false, default(TileObject)});
        Console.WriteLine("After Second Invoke");
        // placeit.Invoke(Main.LocalPlayer, new object[] { });
        
        Player.tileTargetX = oldTargetX;
        Main.LocalPlayer.direction = oldDir;
    }
}