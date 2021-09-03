using System;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class MirrorWandSelection : RectangleShape
    {
        internal bool horizontalMirror;
        internal bool wideMirror;
        internal bool validMirrorPlacement;
        public MirrorWandSelection(int itemType, UIState uiState, UIState textUiState = null) : base(itemType, uiState, textUiState)
        {
            
        }

        internal bool IsMouseWithinSelection() =>
            HelperMethods.IsWithinRange(Player.tileTargetX, cs.RMBStart.X, cs.RMBEnd.X) &&
            HelperMethods.IsWithinRange(Player.tileTargetY, cs.RMBStart.Y, cs.RMBEnd.Y);
        
        private bool IsMirrorAxisInsideSelection() =>
            cs.LMBStart != cs.LMBEnd &&
            HelperMethods.IsWithinRange(cs.LMBStart.X, cs.RMBStart.X, cs.RMBEnd.X) &&
            HelperMethods.IsWithinRange(cs.LMBEnd.X, cs.RMBStart.X, cs.RMBEnd.X) &&
            HelperMethods.IsWithinRange(cs.LMBStart.Y, cs.RMBStart.Y, cs.RMBEnd.Y) &&
            HelperMethods.IsWithinRange(cs.LMBEnd.Y, cs.RMBStart.Y, cs.RMBEnd.Y);

        private void LimitMirrorSize()
        {
            int horDist = (int) Math.Abs(cs.LMBEnd.X - cs.LMBStart.X);
            int verDist = (int) Math.Abs(cs.LMBEnd.Y - cs.LMBStart.Y);
            horizontalMirror = horDist > verDist;
            
            //Limit x coords
            if (!horizontalMirror && cs.LMBEnd.X < cs.LMBStart.X - 1)
                cs.LMBEnd.X = cs.LMBStart.X - 1;
            else if (!horizontalMirror && cs.LMBEnd.X > cs.LMBStart.X + 1)
                cs.LMBEnd.X = cs.LMBStart.X + 1;
            
            //Limit y coords
            if (horizontalMirror && cs.LMBEnd.Y < cs.LMBStart.Y - 1)
                cs.LMBEnd.Y = cs.LMBStart.Y - 1;
            else if (horizontalMirror && cs.LMBEnd.Y > cs.LMBStart.Y + 1)
                cs.LMBEnd.Y = cs.LMBStart.Y + 1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            cs.UpdateCoords();
            
            //Mirror
            LimitMirrorSize();
            wideMirror = cs.LMBEnd.X == cs.LMBStart.X - 1 || cs.LMBEnd.X == cs.LMBStart.X + 1 ||
                         cs.LMBEnd.Y == cs.LMBStart.Y - 1 || cs.LMBEnd.Y == cs.LMBStart.Y + 1;
            validMirrorPlacement = IsMirrorAxisInsideSelection();
            
            color = validMirrorPlacement ? Yellow : Red;
            PlotSelection(cs.LMBStart, cs.LMBEnd);
            
            //Selected area
            color = Blue;
            PlotSelection(cs.RMBStart, cs.RMBEnd);
        }

        internal override void Update()
        {
            base.Update();
        }
    }
}