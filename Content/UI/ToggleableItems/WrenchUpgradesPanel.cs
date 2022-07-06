﻿using System;
using System.Linq;
using BuilderEssentials.Assets;
using BuilderEssentials.Common.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BuilderEssentials.Content.UI;

//Kirtle: Replace this with BuilderToggles whenever those get merged :(
public class WrenchUpgradesPanel : BaseToggleablePanel
{
    public override bool IsHoldingBindingItem() => Main.LocalPlayer.GetModPlayer<BEPlayer>().IsWrenchEquipped;

    private const float ParentWidth = 90f, ParentHeight = 40f;
    private const int ElementsCount = (int) WrenchUpgrades.Count;
    private UIImageButton[] elements;
    private bool elementHovered;
    private string text = "";
    public bool[] enabledUpgrades;
    
    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(15f, 0f);
        Top.Set(-ParentHeight - 15f, 1f);
        SetPadding(0);
        SetPadding(0);

        enabledUpgrades = new bool[ElementsCount];
        elements = new UIImageButton[ElementsCount];
        for (int i = 0; i < ElementsCount; i++) {
            int index = i;
            elements[i] = new UIImageButton(AssetsLoader.GetAssets(AssetsID.WrenchUpgradesToggle)[0]);
            elements[i].Width.Set(14f, 0);
            elements[i].Height.Set(14f, 0);
            elements[i].Left.Set(16f * i + 27f, 0);
            elements[i].Top.Set(1f, 0);

            elements[i].OnMouseOver += (__, _) => {
                elementHovered = true;
                string upgradeType = ((WrenchUpgrades) index).ToString();
                text = string.Concat(upgradeType.Select(c => Char.IsUpper(c) ? $" {c}" : $"{c}")).TrimStart(' ');
            };
            
            elements[i].OnMouseOut += (__, _) => {
                elementHovered = false;
                text = "";
            };

            elements[i].OnClick += (__, _) => ToggleUpgrade(index);
            
            Append(elements[i]);
        }
    }

    public void ToggleUpgrade(int index) {
        //TODO: add sound
        enabledUpgrades[index] = !enabledUpgrades[index];

        if (!enabledUpgrades[index])
            elements[index].SetVisibility(.75f, .4f);
        else
            elements[index].SetVisibility(1f, 1f);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        Left.Set(15f, 0f);
        Top.Set(-ParentHeight - 15f, 1f);
        
        if (IsMouseHovering && elementHovered)
            Main.LocalPlayer.mouseInterface = true;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        
        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, BEPlayer.PointedScreenCoords + new Vector2(18f, 18f), Color.White, 0f, Vector2.Zero, Vector2.One);
    }
}