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
    /// Whether the panel will be removed from the Parent UIState or not
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

    protected CoordSelection cs;
    private HistoryStack<List<Tuple<Point, Tile>>> historyPlacements;
    private UniqueQueue<Tuple<Point, Item>> queuedPlacements;
    // public bool doPlacement = false;
    // public bool doUndo = false;
    public override void OnInitialize() {
        SelectedItem = new(ItemID.None);
        cs = new(ShapesUIState.GetInstance());
        historyPlacements = new(ModContent.GetInstance<MainConfig>().MaxUndoNum);
        queuedPlacements = new();

        cs.LeftMouse.OnClick += _ => {
            Console.WriteLine($"Click left: {CanPlaceItems()}");
            if (IsHoldingBindingItem() && CanPlaceItems())
                DequeuePlacement();
        };
        cs.RightMouse.OnClick += _ => {
            Console.WriteLine($"Click Right");
            if (IsHoldingBindingItem())
                UndoPlacement();
        };
    }

    public void UpdateMaxUndoNum(int value) {
        if (historyPlacements == null) return;
        
        var oldHistoryPlacements = historyPlacements;
        HistoryStack<List<Tuple<Point, Tile>>> newHistoryPlacements = new(value);
        newHistoryPlacements.AddRange(oldHistoryPlacements.Items);
        historyPlacements = newHistoryPlacements;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        cs.UpdateCoords();
        
        queuedPlacements.Clear();
        PlotSelection();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        // cs.UpdateCoords();
    }

    protected void QueuePlacement(Point coords, Item item)
        => queuedPlacements.Enqueue(new(coords, item));

    //TODO: Make Async?
    public void DequeuePlacement() {
        Console.WriteLine("Dequeue called");
        if (queuedPlacements.Count == 0) return;

        Console.WriteLine("Queued Placements");

        if (!SelectionHasChanged()) {
            queuedPlacements.Clear();
            return;
        }

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
            WorldGen.PlaceTile(coord.X, coord.Y, item.createTile, mute: false, forced: true);
        }
        
        historyPlacements.Push(previousPlacement);
        Console.WriteLine($"Push -> New history size: {historyPlacements.Count}");
    }

    public void UndoPlacement() {
        //Kirtle: Do UI that allows a specific historyPlacement to be removed rather than behaving like a Stack?
        if (historyPlacements.Count == 0) return;
        
        //TODO: Need to store in dequeueing what was there before placement, and what was added in the placement
        
        //Check if the current tile at coord is exactly what it was placed on dequeuing,
        //and if it is, place what was there before

        List<Tuple<Point, Tile>> previousPlacement = historyPlacements.Pop();
        previousPlacement.ForEach(tuple => {
            Point coord = tuple.Item1;
            Tile tile = tuple.Item2;
            
            //TODO: Replace with old tile, instead of just killing last placement
            WorldGen.KillTile(coord.X, coord.Y);
        });

        Console.WriteLine($"Pop -> New history size: {historyPlacements.Count}");
    }
}