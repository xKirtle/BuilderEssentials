﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace BuilderEssentials.Content.UI;

public class ImprovedRulerPanel : BaseShapePanel
{
    public override bool IsHoldingBindingItem()
        => Main.LocalPlayer.HeldItem.IsAir && Main.LocalPlayer.GetModPlayer<BEPlayer>().IsImprovedRulerEquipped;

    public override bool CanPlaceItems() => false;

    public override bool SelectionHasChanged() => false;

    private HashSet<Vector2> visitedPlottedPixels = new();
    public override HashSet<Vector2> VisitedPlottedPixels => visitedPlottedPixels;

    private Vector2 rulerStart => cs.RightMouse.Start;
    private Vector2 rulerEnd => cs.RightMouse.End;
    private Vector2 curveStart => cs.LeftMouse.Start;
    private Vector2 curveEnd => cs.LeftMouse.End;

    private Color color = ShapeHelpers.Blue * 0.6f;
    public override void PlotSelection() {
        //Reset curve if ruler selection changes
        if (rulerStart == rulerEnd) {
            cs.LeftMouse.Start = curveEnd;
            return;
        }

        float minX = Math.Min(rulerStart.X, rulerEnd.X), minY = Math.Min(rulerStart.Y, rulerEnd.Y);
        string text = "";
        Vector2 pos = Vector2.Zero;

        if (curveStart == curveEnd) {
            ShapeHelpers.PlotLine(rulerStart, rulerEnd, color, this, 0.90f);
            text += "Length: ";
            pos = rulerEnd;
        }
        else {
            ShapeHelpers.PlotBezier(0.1f, rulerStart, curveEnd, rulerEnd, color, this, 0.9f);
            text += "Number of tiles: ";
            pos = curveEnd;
        }

        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, $"{text}{visitedPlottedPixels.Count}",
            pos * 16 - Main.screenPosition + new Vector2(18f, 18f), ShapeHelpers.Blue * 1.25f, 0f, Vector2.Zero, Vector2.One);
    }

    public override void UpdateRegardlessOfVisibility() {
        if (IsHoldingBindingItem() && !IsVisible || !IsHoldingBindingItem() && IsVisible)
            ShapesUIState.TogglePanelVisibility<ImprovedRulerPanel>();
    }
}