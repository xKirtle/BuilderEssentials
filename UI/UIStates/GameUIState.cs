using BuilderEssentials.Items;
using BuilderEssentials.UI.Elements;
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
        public MirrorWandSelection mirrorWandSelection;
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

            bezierCurve = new BezierCurve(ItemID.None, this);
            Append(bezierCurve);
            bezierCurve.Show();

            //UIUIState was initialized before this UserInterface, so we can use its instance for the UIText
            fillWandSelection = new FillWandSelection(ModContent.ItemType<FillWand>(), this, UIUIState.Instance);
            Append(fillWandSelection);
            fillWandSelection.Hide();
            
            mirrorWandSelection = new MirrorWandSelection(ModContent.ItemType<MirrorWand>(), this);
            Append(mirrorWandSelection);
            mirrorWandSelection.Show();
        }

        public override void Update(GameTime gameTime)
        {
            //TODO: Same coords on mirror wand selection don't hide the text
            //shapes drawer, however, does
            
            base.Update(gameTime);
            rectangleShape?.Update();
            ellipseShape?.Update();
            mirrorWandSelection?.Update();
            fillWandSelection?.Update();
            
            fillWandSelection?.Hide();
            fillWandSelection?.uiText.Hide();
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
            mirrorWandSelection = null;
        }
    }
}