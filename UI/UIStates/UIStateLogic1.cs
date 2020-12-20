using BuilderEssentials.Items;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIPanels.ShapesDrawerUI;
using BuilderEssentials.UI.UIPanels.ShapesMenu;
using BuilderEssentials.Utilities;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    public class UIStateLogic1 : UIState
    {
        public static UIStateLogic1 Instance;
        public static MultiWandWheel multiWandWheel;
        public static AutoHammerWheel autoHammerWheel;
        public static PaintWheel paintWheel;
        public static FillWandSelection fillWandSelection;
        public static MirrorWandSelection mirrorWandSelection;
        public static ArrowPanel arrowPanel;
        public static MenuPanel menuPanel;
        public static ShapesDrawerSelection shapesDrawerSelection;
        public static RectangleShape rectangleShape;
        public static EllipseShape ellipseShape;
        public static BezierCurve bezierCurve;

        public override void OnInitialize()
        {
            Instance = this;

            multiWandWheel = new MultiWandWheel();
            Instance.Append(multiWandWheel);
            multiWandWheel.Hide();
            
            autoHammerWheel = new AutoHammerWheel();
            Instance.Append(autoHammerWheel);
            autoHammerWheel.Hide();
            
            paintWheel = new PaintWheel();
            Instance.Append(paintWheel);
            paintWheel.Hide();
            
            fillWandSelection = new FillWandSelection();
            Instance.Append(fillWandSelection);
            fillWandSelection.Hide();
            
            mirrorWandSelection = new MirrorWandSelection();
            Instance.Append(mirrorWandSelection);
            mirrorWandSelection.Show();
            
            arrowPanel = new ArrowPanel();
            Instance.Append(arrowPanel);
            arrowPanel.Show();
            
            menuPanel = new MenuPanel();
            Instance.Append(menuPanel);
            menuPanel.Hide();
            
            shapesDrawerSelection = new ShapesDrawerSelection();
            Instance.Append(shapesDrawerSelection);
            shapesDrawerSelection.Show();

            rectangleShape = new RectangleShape();
            Instance.Append(rectangleShape);
            rectangleShape.Show();
            
            ellipseShape = new EllipseShape();
            Instance.Append(ellipseShape);
            ellipseShape.Show();

            bezierCurve = new BezierCurve();
            Instance.Append(bezierCurve);
            bezierCurve.Show();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            multiWandWheel?.Update();
            autoHammerWheel?.Update();
            paintWheel?.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            paintWheel?.UpdateColors();
        }

        // public void Unload()
        // {
        //     multiWandWheel = null;
        //     autoHammerWheel = null;
        //     paintWheel = null;
        //     fillWandSelection = null;
        //     mirrorWandSelection = null;
        //     arrowPanel = null;
        //     menuPanel = null;
        //     shapesDrawerSelection = null;
        //     rectangleShape = null;
        //     ellipseShape = null;
        //     bezierCurve = null;
        // }
    }
}