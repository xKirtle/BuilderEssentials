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
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class ShapesUISystem : UISystem<ShapesUIState>
{
    public override InterfaceScaleType InterfaceScaleType => InterfaceScaleType.Game;
}

public class ShapesUIState : ManagedUIState<BaseShapePanel>
{
    public override List<Type> PanelTypes() => new() {
        typeof(FillWandPanel)
    };

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        // Only happens after Update() tries to run on each Panel
        for (int i = 0; i < PanelTypes().Count; i++) {
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
}

public abstract class BaseShapePanel : UIElement
{
    public bool IsVisible => Parent != null;
    
    /// <summary>
    /// Item types that, when held by the player, won't make the panel be removed
    /// If the array is null, empty or its only value is 0, won't be removed automatically
    /// </summary>
    public virtual int[] ItemBoundToDisplay { get; protected set; } = { ItemID.None };
    
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

    protected CoordSelection cs;
    private HistoryStack<List<Tuple<Point, Tile>>> historyPlacements;
    private UniqueQueue<Tuple<Point, Item>> queuedPlacements;
    public bool doPlacement = false;
    public bool doUndo = false;
    public override void OnInitialize() {
        SelectedItem = new(ItemID.None);
        cs = new(ShapesUIState.GetInstance());
        historyPlacements = new(ModContent.GetInstance<MainConfig>().MaxUndoNum);
        queuedPlacements = new();
    }

    public void UpdateMaxUndoNum(int value) {
        if (historyPlacements == null) return;
        
        var oldHistoryPlacements = historyPlacements;
        HistoryStack<List<Tuple<Point, Tile>>> newHistoryPlacements = new(value);
        newHistoryPlacements.AddRange(oldHistoryPlacements.Items);
        historyPlacements = newHistoryPlacements;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        // if (doUndo)
        //     Console.WriteLine("Undo this tick");
        //
        // if (doPlacement)
        //     Console.WriteLine("Dequeue this tick, if CanPlace");
        
        if (doUndo) {
            // Console.WriteLine("Undo");
            UndoPlacement();
            doUndo = false;
        }
        
        base.Draw(spriteBatch);
        cs.UpdateCoords();
        PlotSelection();
        
        if (doPlacement) {
            if (CanPlaceItems()) {
                // Console.WriteLine("Dequeued");
                DequeuePlacement();
            }
            doPlacement = false;
        }
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }

    protected void QueuePlacement(Point coords, Item item)
        => queuedPlacements.Enqueue(new(coords, item));

    //TODO: Make Async?
    public void DequeuePlacement() {
        if (queuedPlacements.Count == 0) return;

        //TODO: Detect if coords in queuedPlacements are the same as the ones present in historyPlacements.Peek()
        //If they are, no point in trying to place the same stuff if i
        
        //Just check if the selection/mouse changed in anyway
        
        List<Tuple<Point, Tile>> previousPlacement = new(queuedPlacements.Count);

        while (queuedPlacements.Count != 0) {
            //Get queued info
            Tuple<Point, Item> tuple = queuedPlacements.Dequeue();
            Point coord = tuple.Item1;
            Item item = tuple.Item2;
            
            //Save previous placement to history
            Tile tile = Framing.GetTileSafely(coord);
            previousPlacement.Add(new(coord, tile));
            
            //Place
            //TODO: Choose between createTile and createWall and sync it
            WorldGen.PlaceTile(coord.X, coord.Y, item.createTile, mute: true, forced: true);
        }
        
        historyPlacements.Push(previousPlacement);
        // Console.WriteLine($"Push -> New history size: {historyPlacements.Count}");
    }

    public void UndoPlacement() {
        if (historyPlacements.Count == 0) return;
        
        List<Tuple<Point, Tile>> previousPlacement = historyPlacements.Pop();
        previousPlacement.ForEach(tuple =>
        {
            Point coord = tuple.Item1;
            Tile tile = tuple.Item2;
            
            //TODO: Replace with old tile, instead of just killing last placement
            WorldGen.KillTile(coord.X, coord.Y);
        });

        // Console.WriteLine($"Pop -> New history size: {historyPlacements.Count}");
    }
}