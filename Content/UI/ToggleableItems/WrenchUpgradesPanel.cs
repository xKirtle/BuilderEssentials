using System;
using System.Linq;
using BuilderEssentials.Assets;
using BuilderEssentials.Common.Enums;
using BuilderEssentials.Content.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BuilderEssentials.Content.UI;

//Kirtle: Replace this with BuilderToggles whenever those get merged :(
public class WrenchUpgradesPanel : BaseToggleablePanel
{
    public override bool IsHoldingBindingItem() => Main.LocalPlayer.GetModPlayer<BEPlayer>().EquippedWrenchInstance != null;

    private const float ParentWidth = 90f, ParentHeight = 40f;
    private const int ElementsCount = (int) WrenchUpgrades.Count;
    private UIImageButton[] elements = new UIImageButton[ElementsCount];
    private bool elementHovered;
    private string text = "";
    public bool[] enabledUpgrades = new bool[ElementsCount];
    
    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(15f, 0f);
        Top.Set(-ParentHeight - 15f, 1f);
        SetPadding(0);
        
        for (int i = 0; i < ElementsCount; i++) {
            int index = i;
            elements[i] = new UIImageButton(AssetsLoader.GetAssets($"{AssetsID.WrenchUpgradesToggle}/Toggle0"));
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
        BuildingWrench wrench = Main.LocalPlayer.GetModPlayer<BEPlayer>().EquippedWrenchInstance as BuildingWrench;
        if (wrench?.unlockedUpgrades[index] != (int) UpgradeState.Locked)
            enabledUpgrades[index] = !enabledUpgrades[index];
        
        if (!enabledUpgrades[index]) {
            elements[index].SetVisibility(.75f, .4f);
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
        else {
            elements[index].SetVisibility(1f, 1f);
            SoundEngine.PlaySound(SoundID.MenuOpen);
        }
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        Left.Set(15f, 0f);
        Top.Set(-ParentHeight - 15f, 1f);
        
        if (IsMouseHovering && elementHovered)
            Main.LocalPlayer.mouseInterface = true;
    }

    public override void UpdateRegardlessOfVisibility() {
        if ((IsHoldingBindingItem() && !IsVisible) || (!IsHoldingBindingItem() && IsVisible))
            ToggleableItemsUIState.TogglePanelVisibility<WrenchUpgradesPanel>();
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        
        //Attempting to visually reset enabled toggles in case the wrench is switched to a different one
        BuildingWrench wrench = Main.LocalPlayer.GetModPlayer<BEPlayer>().EquippedWrenchInstance as BuildingWrench;
        for (int i = 0; i < wrench?.unlockedUpgrades.Length; i++) {
            enabledUpgrades[i] = false;
            if (wrench.unlockedUpgrades[i] == (int) UpgradeState.Locked)
                elements[i].SetVisibility(.75f, .4f);
        }

        if (!String.IsNullOrEmpty(text))
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, BEPlayer.PointedScreenCoords + new Vector2(18f, 18f), Color.White, 0f, Vector2.Zero, Vector2.One);
    }
}