﻿using BuilderEssentials.UI.UIPanels;
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
        public PaintWheel paintWheel;
        public override void OnInitialize()
        {
            base.OnInitialize();
            Instance = this;

            arrowPanel = new ArrowPanel();
            Append(arrowPanel);
            arrowPanel.Hide();
            
            menuPanel = new MenuPanel();
            Append(menuPanel);
            menuPanel.Hide();
            
            autoHammerWheel = new AutoHammerWheel();
            Append(autoHammerWheel);
            autoHammerWheel.Hide();
            
            multiWandWheel = new MultiWandWheel();
            Append(multiWandWheel);
            multiWandWheel.Hide();

            paintWheel = new PaintWheel();
            Append(paintWheel);
            paintWheel.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            autoHammerWheel?.Update();
            multiWandWheel?.Update();
            paintWheel?.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            paintWheel?.UpdateColors();
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
            paintWheel = null;
        }
    }
}