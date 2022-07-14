using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Common.DataStructures;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class ShapesUISystem : UISystem<ShapesUIState>
{
    public override InterfaceScaleType InterfaceScaleType => InterfaceScaleType.Game;
}

public class ShapesUIState : ManagedUIState<BaseShapePanel>
{
    public override List<Type> PanelTypes => new() {
        typeof(FillWandPanel),
        typeof(MirrorWandPanel),
        typeof(ImprovedRulerPanel),
        typeof(ShapesDrawerPanel)
    };

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        // Only happens after Update() tries to run on each Panel
        for (int i = 0; i < PanelTypes.Count; i++) {
            var panel = GetUIPanel(i);
            panel.UpdateRegardlessOfVisibility();
        }
    }

    public static void UpdateMaxUndoNum(int value) {
        for (int i = 0; i < GetPanelCount(); i++) {
            var panel = GetUIPanel(i);
            panel.UpdateMaxUndoNum(value);
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        ClearVisitedPlottedPixels();
    }

    private static void ClearVisitedPlottedPixels() {
        for (int i = 0; i < GetPanelCount(); i++) {
            var panel = GetUIPanel(i);
            panel.VisitedPlottedPixels?.Clear();
        }
    }
}

public abstract class BaseShapePanel : UIElement
{
    public bool IsVisible => Parent != null;
    
    /// <summary>
    /// Whether the panel will update its <see cref="CoordSelection"/> instance <see cref="cs"/>
    /// </summary>
    /// <returns>True if it's not going to be removed</returns>
    public abstract bool IsHoldingBindingItem();
    
    /// <summary>
    /// Item selected for shape placements
    /// </summary>
    public Item SelectedItem { get; private set; }
    
    /// <summary>
    /// Whether this <see cref="BaseShapePanel"/> can queue placements or not
    /// </summary>
    public abstract bool CanPlaceItems();

    //Use this to auto disable/enable elemnts of ItemBoundToDisplay
    
    /// <summary>
    /// Called after <see cref="Update"/> is called
    /// </summary>
    public virtual void UpdateRegardlessOfVisibility() { }

    /// <summary>
    /// Sets <see cref="SelectedItem"/>
    /// </summary>
    public void SetSelectedItem(int itemType) => SelectedItem.SetDefaults(itemType);

    /// <summary>
    /// Define draw behaviour here
    /// </summary>
    public abstract void PlotSelection();

    /// <summary>
    /// Whether we should make latest placement undoable when it's dequeued
    /// </summary>
    /// <returns>True if it can be undoable</returns>
    public abstract bool SelectionHasChanged();

    public abstract HashSet<Vector2> VisitedPlottedPixels { get; }

    internal CoordSelection cs;
    private HistoryStack<List<PlacementHistory>> historyPlacements;
    private UniqueQueue<Point> queuedPlacements;
    private bool undo = true;
    public override void OnInitialize() {
        SelectedItem = new(ItemID.None);
        cs = new(ShapesUIState.GetInstance(), () => IsHoldingBindingItem());
        historyPlacements = new(ModContent.GetInstance<MainConfig>().MaxUndoNum);
        queuedPlacements = new();

        cs.LeftMouse.OnClick += _ => {
            if (!Main.LocalPlayer.mouseInterface && CanPlaceItems()) {
                DequeuePlacement();
                undo = false;
            }
        };
    }

    public void UpdateMaxUndoNum(int value) {
        if (historyPlacements == null) return;
        
        var oldHistoryPlacements = historyPlacements;
        HistoryStack<List<PlacementHistory>> newHistoryPlacements = new(value);
        newHistoryPlacements.AddRange(oldHistoryPlacements.Items);
        historyPlacements = newHistoryPlacements;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        // ShapeHelpers.drawnCoordinates.Clear();
        cs.UpdateCoords();
        
        queuedPlacements.Clear();
        PlotSelection();
    }
    
    public void QueuePlacement(Point coords)
        => queuedPlacements.Enqueue(coords);

    public void DequeuePlacement() {
        if (queuedPlacements.Count == 0) return;

        if (!SelectionHasChanged() && !undo) {
            queuedPlacements.Clear();
            return;
        }

        List<PlacementHistory> previousPlacement = new(queuedPlacements.Count);

        while (queuedPlacements.Count != 0) {
            Point coordinate = queuedPlacements.Dequeue(); 
            Tile tile = Framing.GetTileSafely(coordinate);
            MinimalTile previousTile = new(tile.TileType, tile.WallType, tile.HasTile, TileObjectData.GetTileStyle(tile));
            bool tilePlaced = PlacementHelpers.PlaceTile(coordinate.X, coordinate.Y, SelectedItem);
            MinimalTile placedTile = new(tile.TileType, tile.WallType, tile.HasTile, SelectedItem.placeStyle);
            if (tilePlaced)
                previousPlacement.Add(new(coordinate, previousTile, placedTile, SelectedItem));
        }
        
        historyPlacements.Push(previousPlacement);
    }
    
    public void UndoPlacement() {
        //Kirtle: Do UI that allows a specific historyPlacement to be removed rather than behaving like a Stack?
        if (historyPlacements.Count == 0) return;
        
        undo = true;
        List<PlacementHistory> lastPlacement = historyPlacements.Pop();
        
        for (int i = 0; i < lastPlacement.Count; i++) {
            PlacementHistory last = lastPlacement[i];
            Point coords = last.Coordinate;
            MinimalTile beforePlacementTile = last.PreviousTile;
            MinimalTile placedTile = last.PlacedTile;
            Item placeItem = last.SelectedItem;

            Tile currentTile = Main.tile[coords.X, coords.Y];
            Item beforePlacementItem = new Item(ItemPicker.PickItem(beforePlacementTile));
            bool canUndoTile = currentTile.HasTile && currentTile.TileType == placedTile.TileType &&
                               (currentTile.TileType == placeItem.createTile || beforePlacementItem.IsAir);
            bool canUndoWall = placedTile.HasWall && currentTile.WallType == placedTile.WallType &&
                               (currentTile.WallType == placeItem.createWall || beforePlacementItem.IsAir);
            
            if (canUndoTile || canUndoWall) {
                if (PlacementHelpers.RemoveTile(coords.X, coords.Y, canUndoTile, canUndoWall, needPickPower: true) &&
                    (beforePlacementTile.HasTile || beforePlacementTile.HasWall) && beforePlacementItem.type > ItemID.None) {
                    PlacementHelpers.PlaceTile(coords.X, coords.Y, beforePlacementItem);
                }
            }
        }
    }
}