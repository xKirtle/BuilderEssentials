﻿using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIPanels.ShapesMenu;
using BuilderEssentials.Utilities;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    public class UIStateLogic4 : UIState, ILoadable
    {
        public static UIStateLogic4 Instance;
        public static WrenchUpgradeButtons wrenchUpgrades;
        public static ArrowPanel arrowPanel;
        public static MenuPanel menuPanel;
        public override void OnInitialize()
        {
            Instance = this;
            
            wrenchUpgrades = new WrenchUpgradeButtons();
            Instance.Append(wrenchUpgrades);
            wrenchUpgrades.Hide();
            
            arrowPanel = new ArrowPanel();
            Instance.Append(arrowPanel);
            arrowPanel.Hide();
            
            menuPanel = new MenuPanel();
            Instance.Append(menuPanel);
            menuPanel.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            wrenchUpgrades?.Update();
        }

        public void Unload()
        {
            Instance = null;
            wrenchUpgrades = null;
            arrowPanel = null;
            menuPanel = null;
        }
    }
}