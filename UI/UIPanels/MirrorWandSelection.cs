using System;
using BuilderEssentials.Items;
using BuilderEssentials.UI.UIPanels.ShapesDrawerUI;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels
{
    public class MirrorWandSelection : BaseShape
    {
        public CoordsSelection cs;
        public bool horizontalMirror;
        public bool wideMirror;
        public bool validMirrorPlacement;
        public int selectedQuarter;

        public MirrorWandSelection()
        {
            cs = new CoordsSelection(ModContent.ItemType<MirrorWand>());
        }

        private void FixMirrorX(bool left = true)
        {
            if (left && !horizontalMirror && cs.LMBEnd.X < cs.LMBStart.X - 1)
                cs.LMBEnd.X = cs.LMBStart.X - 1;
            else if (!left && !horizontalMirror && cs.LMBEnd.X > cs.LMBStart.X + 1)
                cs.LMBEnd.X = cs.LMBStart.X + 1;
        }

        private void FixMirrorY(bool top = true)
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
            //TODO: Figure out a good way to know when to draw

            //Should I move this to Update()?
            cs.UpdateCoords();

            //Selection
            color = Blue;
            PlotRectangle((int) cs.RMBStart.X, (int) cs.RMBStart.Y, (int) cs.RMBEnd.X, (int) cs.RMBEnd.Y);

            //Mirror
            selectedQuarter = SelectedQuarter((int) cs.LMBStart.X, (int) cs.LMBStart.Y, (int) cs.LMBEnd.X,
                (int) cs.LMBEnd.Y);
            int distanceX = (int) (cs.LMBEnd.X - cs.LMBStart.X);
            int distanceY = (int) (cs.LMBEnd.Y - cs.LMBStart.Y);
            horizontalMirror = Math.Abs(distanceX) > Math.Abs(distanceY);

            FixMirrorX(!Convert.ToBoolean(selectedQuarter % 2));
            FixMirrorY(selectedQuarter >= 2 ? false : true);

            wideMirror = cs.LMBEnd.X == cs.LMBStart.X - 1 || cs.LMBEnd.X == cs.LMBStart.X + 1 ||
                         cs.LMBEnd.Y == cs.LMBStart.Y - 1 || cs.LMBEnd.Y == cs.LMBStart.Y + 1;
            validMirrorPlacement = IsMirrorAxisInsideSelection();

            color = Red;
            if (validMirrorPlacement)
                color = Yellow;

            PlotRectangle((int) cs.LMBStart.X, (int) cs.LMBStart.Y, (int) cs.LMBEnd.X, (int) cs.LMBEnd.Y);
        }
    }
}