using BuilderEssentials.UI.UIPanels.ShapesMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    public class SecondaryUIState : UIState, ILoadable
    {
        public static SecondaryUIState Instance;
        public static ArrowPanel ArrowPanel;
        public static MenuPanel MenuPanel;
        public override void OnInitialize()
        {
            base.OnInitialize();
            Instance = this;

            ArrowPanel = new ArrowPanel();
            Instance.Append(ArrowPanel);
            //ArrowPanel.Hide();

            MenuPanel = new MenuPanel();
            Instance.Append(MenuPanel);
            MenuPanel.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
            ArrowPanel = null;
            MenuPanel = null;
        }
    }
}