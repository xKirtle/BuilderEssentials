﻿using BuilderEssentials.Items;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIPanels.ShapesMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
        public CustomUIText replaceTileText;
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

            replaceTileText = new CustomUIText("");
            Append(replaceTileText);
            replaceTileText.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            int heldItemType = Main.LocalPlayer.HeldItem.type;
            if (heldItemType != ModContent.ItemType<ShapesDrawer>())
            {
                arrowPanel.Hide();
                menuPanel.Hide();
            }

            if (heldItemType != ModContent.ItemType<AutoHammer>())
                autoHammerWheel.Hide();
            
            if (heldItemType != ModContent.ItemType<MultiWand>())
                multiWandWheel.Hide();
            
            if (heldItemType != ModContent.ItemType<SpectrePaintTool>() &&
                heldItemType != ModContent.ItemType<PaintTool>())
                paintWheel.Hide();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            int heldItemType = Main.LocalPlayer.HeldItem.type;
            if (heldItemType == ModContent.ItemType<FillWand>() || heldItemType == ModContent.ItemType<ShapesDrawer>())
            {
                BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
                Vector2 cachedMouse = UIModSystem.cachedMouseCoords;
                replaceTileText.SetText($"Replace mode: {(mp.replaceTiles ? "On" : "Off")}");
                replaceTileText.Left.Set(cachedMouse.X + 44, 0);
                replaceTileText.Top.Set(cachedMouse.Y + 44, 0);
                replaceTileText.TextColor = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
                replaceTileText.Show();
            }
            else 
                replaceTileText.Hide();
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
            replaceTileText = null;
        }
    }
}