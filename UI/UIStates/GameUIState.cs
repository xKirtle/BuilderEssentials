using BuilderEssentials.Items;
using BuilderEssentials.UI.Elements.ShapesDrawer;
using BuilderEssentials.UI.UIPanels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    internal class GameUIState : UIState, ILoadable
    {
        public static GameUIState Instance;
        public RectangleShape rectangleShape;
        public EllipseShape ellipseShape;
        public BezierCurve bezierCurve;
        public FillWandSelection fillWandSelection;
        public override void OnInitialize()
        {
            Instance = this;
            base.OnInitialize();

            rectangleShape = new RectangleShape(ModContent.ItemType<ShapesDrawer>(), this);
            Append(rectangleShape);
            rectangleShape.Show();

            ellipseShape = new EllipseShape(ModContent.ItemType<ShapesDrawer>(), this);
            Append(ellipseShape);
            ellipseShape.Show();

            // bezierCurve = new BezierCurve(ModContent.ItemType<ShapesDrawer>(), this);
            // Append(bezierCurve);
            // bezierCurve.Show();

            fillWandSelection = new FillWandSelection(ModContent.ItemType<FillWand>(), this);
            Append(fillWandSelection);
            fillWandSelection.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rectangleShape?.Update();
            ellipseShape?.Update();
            
            fillWandSelection.Hide();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void Load(Mod mod)
        {
            
        }

        public void Unload()
        {
            Instance = null;
            rectangleShape = null;
            ellipseShape = null;
            bezierCurve = null;
            fillWandSelection = null;
        }
    }
}