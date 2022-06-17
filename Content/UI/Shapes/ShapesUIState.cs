using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Common;
using BuilderEssentials.Common.DataStructures;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class ShapesUISystem : UISystem<ShapesUIState>
{
    public override InterfaceScaleType InterfaceScaleType => InterfaceScaleType.Game;
}

public class ShapesUIState : ManagedUIState<BaseShapePanel>
{
    public override List<Type> PanelTypes() => new() {
        // typeof(FillWandPanel)
    };

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        // Only happens after Update() tries to run on each Panel
        for (int i = 0; i < PanelTypes().Count; i++) {
            var panel = GetUIPanel(i);
            panel.UpdateRegardlessOfVisibility();
        }
    }
}

public abstract class BaseShapePanel : UIElement
{
    public bool IsVisible => Parent != null;
    public virtual int[] ItemBoundToDisplay { get; protected set; } = { ItemID.None };
    public int SelectedTileType { get; private set; }
    public int SelectedItemType { get; private set; }
    public virtual bool CanPlaceItems() => false;
    public virtual bool CanPlotSelection() => true;

    //Use this to auto disable/enable elemnts of ItemBoundToDisplay
    public virtual void UpdateRegardlessOfVisibility() { }

    public void SetSelectedTileType(int itemType) {
        SelectedItemType = itemType;
        Item item = new();
        item.SetDefaults(itemType);
        SelectedTileType = item.createTile;
    }
    
    public abstract void PlotSelection();

    protected CoordSelection cs;
    private HistoryStack<List<Tuple<Point, int>>> lastPlacements;
    private UniqueQueue<Tuple<Point, int>> queuedPlacements;
    private const int HistoryNum = 5;
    public override void OnInitialize() {
        cs = new(ShapesUIState.GetInstance());
        lastPlacements = new(HistoryNum);
        queuedPlacements = new();
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        cs.UpdateCoords();

        if (CanPlotSelection())
            PlotSelection();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        if (CanPlaceItems())
            UnqueuePlacements();
    }

    protected void QueuePlacement(Point coords, int tileType)
        => queuedPlacements.Enqueue(new(coords, tileType));

    //TODO: Make Async?
    private void UnqueuePlacements() {
        if (queuedPlacements.Count == 0) return;
        
        List<Tuple<Point, int>> previousPlacement = new(queuedPlacements.Count);
        while (queuedPlacements.Count != 0) {
            //Get queued info
            Tuple<Point, int> tuple = queuedPlacements.Dequeue();
            Point coord = tuple.Item1;
            int tileType = tuple.Item2;
            
            //Save previous placement to history
            Tile tile = Framing.GetTileSafely(coord);
            previousPlacement.Add(new(coord, tile.TileType));
            
            //Place
            WorldGen.PlaceTile(coord.X, coord.Y, tileType, mute: true, forced: true);
        }
        
        lastPlacements.Push(previousPlacement);
    }

    protected void UndoPlacement() {
        List<Tuple<Point, int>> previousPlacement = lastPlacements.Pop();
        
        previousPlacement.ForEach(tuple =>
        {
            Point coord = tuple.Item1;
            int tileType = tuple.Item2;
            
            //Kirtle: maybe check if tileType > 0, else remove tile
            WorldGen.PlaceTile(coord.X, coord.Y, tileType, mute: true, forced: true);
        });
    }
}