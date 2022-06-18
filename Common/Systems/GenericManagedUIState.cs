using System;
using System.Collections.Generic;
using BuilderEssentials.Common;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Common.Systems;

//T is the base UIElement type for the UIState
[Autoload(Side = ModSide.Client)]
public abstract class ManagedUIState<T> : UIState, IDisposable where T : UIElement
{
    private static ManagedUIState<T> instance;
    private List<T> uiStateElements = new();
    public abstract List<Type> PanelTypes();

    public override void OnInitialize() {
        instance = this;
    
        for (var i = 0; i < PanelTypes().Count; i++)
            uiStateElements.Add((T) Activator.CreateInstance(PanelTypes()[i]));
    }

    public virtual void Dispose() {
        for (int i = 0; i < uiStateElements.Count; i++) {
            uiStateElements[i].Deactivate();
            uiStateElements[i] = null;
        }

        uiStateElements = null;
        instance = null;
    }

    public static ManagedUIState<T> GetInstance() => instance;

    private static int IndexOfPanelType<U>()
        => GetInstance().PanelTypes().IndexOf(typeof(U));
    
    private static T GetElementAtIndex(int index) => (T) GetInstance().uiStateElements[index];

    public static U GetUIPanel<U>() where U : T
        => (U) GetElementAtIndex(IndexOfPanelType<U>());

    public static T GetUIPanel(int index)
        => GetElementAtIndex(index);

    public static void TogglePanelVisibility<U>() where U : T
        => ActuallyTogglePanelVisibility(GetUIPanel<U>());

    public static void TogglePanelVisibility(int index)
        => ActuallyTogglePanelVisibility(GetUIPanel(index));

    private static void ActuallyTogglePanelVisibility(T panel) {
        if (panel.Parent == null) {
            GetInstance().Append(panel);
            panel.Activate();
        }
        else {
            panel.Remove();
            panel.Deactivate();
        }
    }

    //ModConfig OnChanged ends up calling this, where instance might be null at that point
    public static int GetPanelCount() => GetInstance()?.PanelTypes().Count ?? 0;
}