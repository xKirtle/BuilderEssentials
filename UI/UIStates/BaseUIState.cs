using BuilderEssentials.UI.Elements.ShapesDrawer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    internal class BaseUIState : UIState, ILoadable
    {
        public static BaseUIState Instance;
        public RectangleShape rectangleShape;
        public override void OnInitialize()
        {
            Instance = this;
            base.OnInitialize();

            rectangleShape = new RectangleShape(ItemID.None, this);
            Append(rectangleShape);
            rectangleShape.Show();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rectangleShape?.Update();
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
        }
    }
}