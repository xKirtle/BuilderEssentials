using System.Reflection;
using BuilderEssentials.UI.Elements.ShapesDrawer;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Utilities
{
    internal partial class HelperMethods
    {
        internal static void MirrorPlacement(int i, int j)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            MirrorWandSelection sel = GameUIState.Instance.mirrorWandSelection;
            CoordsSelection cs = sel.cs;
            
            //Mirror coords
            Vector2 mirrorStart = cs.LMBStart;
            Vector2 mirrorEnd = cs.LMBEnd;
            
            //Check if coords are within selection
            if (!sel.validMirrorPlacement || !HelperMethods.IsWithinRange(i, cs.RMBStart.X, cs.RMBEnd.X) ||
                !HelperMethods.IsWithinRange(j, cs.RMBStart.Y, cs.RMBEnd.Y)) return;
            
            //Check if coords intersect the mirror axis
            if (!HelperMethods.IsWithinRange(i, mirrorStart.X, mirrorEnd.X, true) &&
                !HelperMethods.IsWithinRange(j, mirrorStart.Y, mirrorEnd.Y, true)) return;
            
            if (!sel.horizontalMirror)
            {
                Main.NewText($"Before: [{Player.tileTargetX}/{Player.tileTargetY}]");
                float distanceToMirror = mirrorStart.X - i > mirrorEnd.X - i ? mirrorStart.X - i : mirrorEnd.X - i;
                Player.tileTargetX += (Player.tileTargetX < mirrorStart.X
                    ? (int) (distanceToMirror * 2) + (sel.wideMirror ? 2 : 1)
                    : (int) (-distanceToMirror * 2) - (sel.wideMirror ? 2 : 1)) - 1;
                Main.NewText($"After: [{Player.tileTargetX}/{Player.tileTargetY}]");
            }
        }
    }
}