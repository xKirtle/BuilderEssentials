using System.ComponentModel;
using System.Runtime.Serialization;
using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.ModLoader.Config;

namespace BuilderEssentials.Common.Configs;

public class MainConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Label("Max Undo Times")]
    [Tooltip("The maximum amount of placements the game will remember and be able to undo")]
    [Range(0, 100), DefaultValue(20), DrawTicks]
    public int MaxUndoNum = 20;

    public override void OnChanged() {
        ShapesUIState.UpdateMaxUndoNum(MaxUndoNum);
    }
    
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context) {
        MaxUndoNum = Utils.Clamp(MaxUndoNum, 0, 100);
    }
}