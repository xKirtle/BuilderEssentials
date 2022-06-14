using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Assets;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class PaintBrushState : BaseUIState
{
    public static PaintBrushState Instance;
    public PaintBrushPanel menuPanel;

    public override int[] BoundItemType => new int[]
        {ModContent.ItemType<PaintBrush>(), ModContent.ItemType<SpectrePaintBrush>()};

    public PaintBrushState() {
        Instance = this;
        menuPanel = new PaintBrushPanel();
        Append(menuPanel);
    }
    
    public override void Dispose() {
        Instance = null;
    }
}

public class PaintBrushPanel : UIElement
{
    private const float ParentWidth = 430f, ParentHeight = 360f;
    private UIImageButton[] colorElements;
    private UIImageButton[] toolElements;
    private UIImage[] noPaintOverlay;
    private UIImage colorOverlay;
    private UIImage toolOverlay;
    private Asset<Texture2D>[] toolTextures;
    private bool[] colorAvailable;
    private bool elementHovered;
    private int[] paints;
    public int colorIndex = -1;
    public int toolIndex;

    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(Main.screenWidth / 2 - ParentWidth, 0);
        Top.Set(Main.screenHeight / 2 - ParentHeight, 0);

        paints = new int[31];
        for (int i = 0; i < 27; i++) paints[i] = (1073 + i); //Basic && Deep colors type
        for (int i = 0; i < 3; i++) paints[i + 27] = (1966 + i); //Extra Color Effects type
        paints[30] = 4668; //Illuminant Paint

        colorElements = new UIImageButton[31];
        noPaintOverlay = new UIImage[31];
        colorAvailable = new bool[31];
        for (int i = 0; i < paints.Length; i++) {
            colorElements[i] = new UIImageButton(AssetsLoader.GetAssets(AssetsID.PaintBrushColors)[i]);
            noPaintOverlay[i] = new UIImage(AssetsLoader.GetAssets(AssetsID.PaintBrushColors)[31]);
        }

        colorOverlay = new UIImage(AssetsLoader.GetAssets(AssetsID.PaintBrushColors)[32]);
        colorOverlay.OnMouseOver += (__, _) => elementHovered = true;
        colorOverlay.OnMouseOut += (__, _) => elementHovered = false;
        
        toolTextures = AssetsLoader.GetAssets(AssetsID.PaintBrushTools)[..6];
        toolOverlay = new UIImage(AssetsLoader.GetAssets(AssetsID.PaintBrushTools)[6]);
        toolOverlay.OnMouseOver += (__, _) => elementHovered = true;
        toolOverlay.OnMouseOut += (__, _) => elementHovered = false;

        toolElements = new UIImageButton[3];
        for (int i = 0; i < toolElements.Length; i++)
            toolElements[i] = new UIImageButton(toolTextures[i]);
        
        int radius = 155;
        double angle = Math.PI / 12;
        const float ColorElementSize = 40f;

        //Outer semi circle
        for (int i = 0; i < 12; i++) {
            int index = i;
            
            Vector2 offset = new Vector2(ParentWidth - ColorElementSize, ParentHeight - ColorElementSize) / 2;
            double x = offset.X - (radius * Math.Cos(angle * (i + .48)) * 0.95);
            double y = offset.Y - (radius * Math.Sin(angle * (i + .48)) * 1);
            
            var left = new StyleDimension((float) x - ColorElementSize / 4, 0);
            var top = new StyleDimension((float) y + 40, 0);
            colorElements[i].Left = left;
            colorElements[i].Top = top;
            
            colorElements[i].SetVisibility(1f, 0.85f);
            colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
            colorElements[i].OnMouseOver += (__, _) => elementHovered = true;
            colorElements[i].OnMouseOut += (__, _) => elementHovered = false;
            noPaintOverlay[i].OnMouseOver += (__, _) => elementHovered = true;
            noPaintOverlay[i].OnMouseOut += (__, _) => elementHovered = false;
            
            Append(colorElements[i]);
        }
        
        radius = 190;

        for (int i = 12; i < 24; i++) {
            int index = i;
            Vector2 offset = new Vector2(ParentWidth - ColorElementSize, ParentHeight - ColorElementSize) / 2;
            double x = offset.X + (radius * Math.Cos(angle * (i + .48)) * 1);
            double y = offset.Y - (radius * Math.Sin(-angle * (i + .48)) * 1);

            var left = new StyleDimension((float) x - ColorElementSize / 4, 0);
            var top = new StyleDimension((float) y + 30, 0);
            colorElements[i].Left = left;
            colorElements[i].Top = top;
            
            colorElements[i].SetVisibility(1f, 0.85f);
            colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
            colorElements[i].OnMouseOver += (__, _) => elementHovered = true;
            colorElements[i].OnMouseOut += (__, _) => elementHovered = false;
            noPaintOverlay[i].OnMouseOver += (__, _) => elementHovered = true;
            noPaintOverlay[i].OnMouseOut += (__, _) => elementHovered = false;

            Append(colorElements[i]);
        }
        
        radius = 95;
        angle = Math.PI / 6;

        for (int i = 24; i < 30; i++) {
            int index = i;
            Vector2 offset = new Vector2(ParentWidth - ColorElementSize, ParentHeight - ColorElementSize) / 2;
            double x = offset.X - (radius * Math.Cos(angle * (i + .50)) * 1.10);
            double y = offset.Y + (radius * Math.Sin(-angle * (i + .50)) * 1.25);

            var left = new StyleDimension((float) x - ColorElementSize / 4, 0);
            var top = new StyleDimension((float) y + 50, 0);
            colorElements[i].Left = left;
            colorElements[i].Top = top;
            
            colorElements[i].SetVisibility(1f, 0.85f);
            colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
            colorElements[i].OnMouseOver += (__, _) => elementHovered = true;
            colorElements[i].OnMouseOut += (__, _) => elementHovered = false;
            noPaintOverlay[i].OnMouseOver += (__, _) => elementHovered = true;
            noPaintOverlay[i].OnMouseOut += (__, _) => elementHovered = false;

            Append(colorElements[i]);
        }
        
        //Illuminant paint
        Vector2 offsetPaint = new Vector2(ParentWidth - ColorElementSize - 20, ParentHeight - ColorElementSize - 20) / 2;
        var leftOffset = new StyleDimension(offsetPaint.X, 0);
        var topOffset = new StyleDimension(offsetPaint.Y, 0);
        colorElements[30].Left = leftOffset;
        colorElements[30].Top = topOffset;
        
        colorElements[30].SetVisibility(1f, 0.85f);
        colorElements[30].OnMouseDown += (__, _) => ColorSelected(30);
        colorElements[30].OnMouseOver += (__, _) => elementHovered = true;
        colorElements[30].OnMouseOut += (__, _) => elementHovered = false;
        
        Append(colorElements[30]);
        
        for (int i = 0; i < toolElements.Length; i++) {
            int index = i;
            double x = (ParentWidth / 3) * i + 35f;
            double y = (ParentWidth / 2) + 15f;

            toolElements[i].Left.Set((float) x - ColorElementSize / 4, 0);
            toolElements[i].Top.Set((float) y, 0);

            toolElements[i].SetVisibility(1f, .8f);
            toolElements[i].OnMouseDown += (__, _) => ToolSelected(index);
            toolElements[i].OnMouseOver += (__, _) => elementHovered = true;
            toolElements[i].OnMouseOut += (__, _) => elementHovered = false;

            Append(toolElements[i]);
        }
        
        toolOverlay.Left = toolElements[toolIndex].Left;
        toolOverlay.Top = toolElements[toolIndex].Top;
        toolElements[toolIndex].SetVisibility(1f, 1f);
        Append(toolOverlay);
    }

    private void ColorSelected(int index) {
        if (!colorAvailable[index]) return;
        
        colorOverlay.Remove();
        for (int i = 0; i < colorElements.Length; i++)
            colorElements[i].SetVisibility(1f, 0.85f);

        if (colorIndex != index) {
            colorElements[index].SetVisibility(1f, 1f);
            colorOverlay.Left = colorElements[index].Left;
            colorOverlay.Top = colorElements[index].Top;
            Append(colorOverlay);
            colorIndex = index;
        }
        else colorIndex = -1;
    }

    private void ToolSelected(int index) {
        if (toolIndex == index) return;
        
        for (int i = 0; i < toolElements.Length; i++)
            toolElements[i].SetVisibility(1f, .85f);
        
        toolElements[index].SetVisibility(1f, 1f);
        toolOverlay.Left = toolElements[index].Left;
        toolOverlay.Top = toolElements[index].Top;
        toolIndex = index;
    }
    
    private void EvaluateAvailableColorsInInventory() {
        List<int> paintInInventory = new List<int>();
        foreach (Item item in Main.LocalPlayer.inventory) {
            if (paints.Contains(item.type))
                paintInInventory.Add(item.type);
        }
        
        paintInInventory.Distinct();
        paintInInventory.TrimExcess();
        int[] foundIndexes = new int[paintInInventory.Count];

        //Converting paint item types to actual indexes
        for (int i = 0; i < paintInInventory.Count; i++)
            foundIndexes[i] = BasePaintBrush.PaintItemTypeToColorIndex(paintInInventory[i]);

        //Reset all colors available
        for (int i = 0; i < colorAvailable.Length; i++)
            colorAvailable[i] = false;

        //Update colors available
        foreach (int index in foundIndexes)
            colorAvailable[index] = true;
    }

    private void UpdateCrossesOnColors() {
        EvaluateAvailableColorsInInventory();

        for (int i = 0; i < noPaintOverlay.Length; i++) {
            if (!colorAvailable[i]) {
                noPaintOverlay[i].Left = colorElements[i].Left;
                noPaintOverlay[i].Top = colorElements[i].Top;
                Append(noPaintOverlay[i]);

                if (colorIndex == i) {
                    colorElements[i].SetVisibility(1f, 0.85f);
                    colorOverlay.Remove();
                    colorIndex = -1;
                }
            }
            else {
                colorAvailable[i] = true;
                noPaintOverlay[i].Remove();
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        
        bool isSpectre = Main.LocalPlayer.HeldItem.type == ModContent.ItemType<SpectrePaintBrush>();
        for (int i = 0; i < toolElements.Length; i++)
            toolElements[i].SetImage(toolTextures[isSpectre ? i + toolElements.Length : i]);
            
        UpdateCrossesOnColors();
            
        if (elementHovered)
            Main.LocalPlayer.mouseInterface = true;
    }

    public override void OnActivate() {
        UISystem.PreventElementOffScreen(this, BEPlayer.CachedPointedCoord);
    }
    
    //TODO: Draw little circle with selected color under the cursor (down left) like Grand Design
    
}