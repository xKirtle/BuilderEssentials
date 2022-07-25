using System;
using BuilderEssentials.Assets;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class MultiWandPanel : BaseToggleablePanel
{
    public override bool IsHoldingBindingItem() => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<MultiWand>();

    private const float ParentWidth = 180f, ParentHeight = 160f;
    private const int ElementsCount = 6;
    private UIImageButton[] elements;
    private bool elementHovered;
    public int selectedIndex;
    private UIText hoverText;

    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(Main.screenWidth / 2 - ParentWidth, 0);
        Top.Set(Main.screenHeight / 2 - ParentHeight, 0);
        SetPadding(0);

        elements = new UIImageButton[ElementsCount];
        for (int i = 0; i < ElementsCount; i++)
            elements[i] = new UIImageButton(AssetsLoader.GetAssets($"{AssetsID.MultiWand}/MW{i}"));

        //Define our shape
        float offsetX = ParentWidth / 3f;
        float offsetY = (float) (Math.Sqrt(11) / 4) * ParentHeight;

        Vector2 elementOffset = new(22, 22);
        Vector2[] buttonPositions = new[] {
            new Vector2(offsetX, offsetY) - elementOffset, new Vector2(ParentWidth - offsetX, offsetY) - elementOffset,
            new Vector2(ParentWidth - 24, ParentHeight / 2) - elementOffset,
            new Vector2(ParentWidth - offsetX, ParentHeight - offsetY) - elementOffset,
            new Vector2(offsetX, ParentHeight - offsetY) - elementOffset, new Vector2(24, ParentHeight / 2) - elementOffset
        };

        for (int i = 0; i < ElementsCount; i++) {
            int index = i;
            elements[i].Left.Set(buttonPositions[i].X, 0f);
            elements[i].Top.Set(buttonPositions[i].Y, 0f);
            elements[i].OnClick += (__, _) => ElementOnClick(index);
            elements[i].OnMouseOver += (__, _) => ElementOnMouseOver(index);
            elements[i].OnMouseOut += (__, _) => ElementOnMouseOut();
        }

        elements[selectedIndex].SetVisibility(1f, 1f);

        //Append to the main panel
        for (int i = 0; i < ElementsCount; i++)
            Append(elements[i]);
    }

    private void ElementOnClick(int index) {
        for (int i = 0; i < ElementsCount; i++)
            elements[i].SetVisibility(.75f, .4f);

        elements[index].SetVisibility(1f, 1f);
        selectedIndex = index;
    }

    private readonly string[] text = {
        "[i/s:9]   - Places living wood", "[i/s:154]  - Places bones", "[i/s:9]   - Places leaves", "[i/s:1124]  - Places hives",
        "[i/s:620]   - Places living rich mahogany", "[i/s:620]   - Places rich mahogany leaves"
    };

    private void ElementOnMouseOver(int index) {
        elementHovered = true;
        Main.mouseText = true;
        hoverText = new UIText(text[index], 0.4f, true);
        Append(hoverText);
    }

    private void ElementOnMouseOut() {
        elementHovered = false;
        hoverText?.Remove();
    }

    public override void Update(GameTime gameTime) {
        if (IsMouseHovering && elementHovered || hoverText?.IsMouseHovering == true) {
            Main.LocalPlayer.mouseInterface = true;

            if (hoverText == null)
                return;
            PreventTextOffScreen(this, hoverText, BEPlayer.PointedScreenCoords, new Vector2(11, -22));
        }
    }
}