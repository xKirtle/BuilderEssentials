using BuilderEssentials.Assets;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.UI;

public class AutoHammerPanel : BaseToggleablePanel
{
    public override bool IsHoldingBindingItem() => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<AutoHammer>();

    private const float ParentWidth = 160f, ParentHeight = 132f;
    private const int ElementsCount = 6;
    private UIImageButton[] elements;
    private bool elementHovered;
    public int selectedIndex = -1;
    public SlopeType slopeType;
    public bool isHalfBlock;

    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(Main.screenWidth / 2 - ParentWidth, 0);
        Top.Set(Main.screenHeight / 2 - ParentHeight, 0);
        SetPadding(0);

        elements = new UIImageButton[ElementsCount];
        for (int i = 0; i < ElementsCount; i++)
            elements[i] = new UIImageButton(AssetsLoader.GetAssets($"{AssetsID.AutoHammer}/AH{i}"));

        //Define our shape
        Vector2[] buttonPositions = new[] {
            new Vector2(36, 0), new Vector2(88, 0), new Vector2(36, 88), new Vector2(88, 88), new Vector2(0, 44), new Vector2(124, 44)
        };

        for (int i = 0; i < ElementsCount; i++) {
            int index = i;
            elements[i].Left.Set(buttonPositions[i].X, 0f);
            elements[i].Top.Set(buttonPositions[i].Y, 0f);
            elements[i].OnClick += (__, _) => ElementOnClick(index);
            elements[i].OnMouseOver += (__, _) => elementHovered = true;
            elements[i].OnMouseOut += (__, _) => elementHovered = false;
        }

        //Append to the main panel
        for (int i = 0; i < ElementsCount; i++)
            Append(elements[i]);
    }

    private void ElementOnClick(int index) {
        for (int i = 0; i < ElementsCount; i++)
            elements[i].SetVisibility(.75f, .4f);

        if (selectedIndex != index) {
            elements[index].SetVisibility(1f, 1f);
            selectedIndex = index;
        }
        else
            selectedIndex = -1;

        //Assign slopeType and IsHalfBlock based on selectedIndex (and its respective UI image)
        SlopeType[] types = new[] {
            SlopeType.SlopeDownRight, SlopeType.SlopeDownLeft, SlopeType.SlopeUpRight, SlopeType.SlopeUpLeft, SlopeType.Solid
        };

        isHalfBlock = selectedIndex == 5;
        if (selectedIndex != -1 && !isHalfBlock)
            slopeType = types[selectedIndex];
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        if (IsMouseHovering && elementHovered)
            Main.LocalPlayer.mouseInterface = true;
    }
}