using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIPanels.ShapesMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    internal class UIUIState : UIState, ILoadable
    {
        public static UIUIState Instance;
        public ArrowPanel arrowPanel;
        public MenuPanel menuPanel;
        public AutoHammerWheel autoHammerWheel;
        public MultiWandWheel multiWandWheel;
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
            
            autoHammerWheel = new AutoHammerWheel();
            Append(autoHammerWheel);
            autoHammerWheel.Hide();
            
            multiWandWheel = new MultiWandWheel();
            Append(multiWandWheel);
            multiWandWheel.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            autoHammerWheel?.Update();
            multiWandWheel?.Update();
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
            autoHammerWheel = null;
            multiWandWheel = null;
        }
    }
}