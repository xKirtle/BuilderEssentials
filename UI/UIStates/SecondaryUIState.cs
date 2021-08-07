using BuilderEssentials.UI.UIPanels.ShapesMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    internal class SecondaryUIState : UIState, ILoadable
    {
        public static SecondaryUIState Instance;
        public ArrowPanel arrowPanel;
        public MenuPanel menuPanel;
        public override void OnInitialize()
        {
            base.OnInitialize();
            Instance = this;

            arrowPanel = new ArrowPanel();
            Append(arrowPanel);
            arrowPanel.Show();
            
            menuPanel = new MenuPanel();
            Append(menuPanel);
            menuPanel.Hide();
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
            arrowPanel = null;
            menuPanel = null;
        }
    }
}