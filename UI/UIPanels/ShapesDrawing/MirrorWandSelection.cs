using System;
using BuilderEssentials.Items;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawing
{
    public class MirrorWandSelection : BaseShape
    {
        private CoordsSelection cs;
        private bool horizontalMirror;
        private bool wideMirror;
        private bool validMirrorPlacement;

        public MirrorWandSelection()
        {
            cs = new CoordsSelection(ModContent.ItemType<MirrorWand>());
        }

        private void FixX(bool left = true)
        {
            if (left && !horizontalMirror && cs.LMBEnd.X < cs.LMBStart.X - 1)
                cs.LMBEnd.X = cs.LMBStart.X - 1;
            else if (!left && !horizontalMirror && cs.LMBEnd.X > cs.LMBStart.X + 1)
                cs.LMBEnd.X = cs.LMBStart.X + 1;
        }

        private void FixY(bool top = true)
        {
            if (top && horizontalMirror && cs.LMBEnd.Y < cs.LMBStart.Y - 1)
                cs.LMBEnd.Y = cs.LMBStart.Y - 1;
            else if (!top && horizontalMirror && cs.LMBEnd.Y > cs.LMBStart.Y + 1)
                cs.LMBEnd.Y = cs.LMBStart.Y + 1;
        }

        private bool IsMirrorAxisInsideSelection() =>
            cs.LMBStart != cs.LMBEnd && 
            HelperMethods.IsWithinRange(cs.LMBStart.X, cs.RMBStart.X, cs.RMBEnd.X) &&
            HelperMethods.IsWithinRange(cs.LMBEnd.X, cs.RMBStart.X, cs.RMBEnd.X) && 
            HelperMethods.IsWithinRange(cs.LMBStart.Y, cs.RMBStart.Y, cs.RMBEnd.Y) &&
            HelperMethods.IsWithinRange(cs.LMBEnd.Y, cs.RMBStart.Y, cs.RMBEnd.Y);
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.type != cs.itemType) return;

            //Should I move this to Update()?
            cs.UpdateCoords();

            //Selection
            color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f; //Blue
            PlotRectangle((int) cs.RMBStart.X, (int) cs.RMBStart.Y, (int) cs.RMBEnd.X, (int) cs.RMBEnd.Y);

            //Mirror
            int selectedQuarter = SelectedQuarter((int) cs.LMBStart.X, (int) cs.LMBStart.Y, (int) cs.LMBEnd.X,
                (int) cs.LMBEnd.Y);
            int distanceX = (int) (cs.LMBEnd.X - cs.LMBStart.X);
            int distanceY = (int) (cs.LMBEnd.Y - cs.LMBStart.Y);
            horizontalMirror = Math.Abs(distanceX) > Math.Abs(distanceY);

            bool c2 = true;
            if (selectedQuarter >= 2)
            {
                c2 = !c2;
                selectedQuarter -= 2;
            }

            FixX(!Convert.ToBoolean(selectedQuarter));
            FixY(c2);

            wideMirror = cs.LMBEnd.X == cs.LMBStart.X - 1 || cs.LMBEnd.X == cs.LMBStart.X + 1 ||
                         cs.LMBEnd.Y == cs.LMBStart.Y - 1 || cs.LMBEnd.Y == cs.LMBStart.Y + 1;
            validMirrorPlacement = IsMirrorAxisInsideSelection();
            
            color = new Color(1f, 0f, 0f, .75f) * 0.8f; //Red
            if (validMirrorPlacement)
                color = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f; //Yellow
            
            PlotRectangle((int) cs.LMBStart.X, (int) cs.LMBStart.Y, (int) cs.LMBEnd.X, (int) cs.LMBEnd.Y);
        }
    }
}