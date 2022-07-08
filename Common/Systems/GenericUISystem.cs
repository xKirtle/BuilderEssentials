//Adapted from https://github.com/MutantWafflez/LivingWorldMod/blob/1.4_retarget/Common/Systems/UISystem.cs

using System.Collections.Generic;
using BuilderEssentials.Assets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Common.Systems;

[Autoload(Side = ModSide.Client)]
public abstract class UISystem<T> : ModSystem where T : UIState, new()
{
    public UserInterface userInterface;
    public T uiState;
    public virtual string VanillaInterfaceLayer { get; protected set; } = "Vanilla: Ruler";
    public virtual string InterfaceLayerName { get; protected set; } = typeof(T).Name;
    public virtual InterfaceScaleType InterfaceScaleType { get; protected set; } = InterfaceScaleType.UI;

    protected GameTime lastUpdateUiGameTime;

    public override void Load() {
        userInterface = new();
        uiState = new();
        userInterface.SetState(uiState);
    }

    public static Vector2 CachedScreenCoords;
    public override void UpdateUI(GameTime gameTime) {
        CachedScreenCoords = Main.MouseScreen;

        lastUpdateUiGameTime = gameTime;
        if (userInterface?.CurrentState != null)
            userInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals(VanillaInterfaceLayer));
        if (interfaceLayer != -1) {
            layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer($"Builder Essentials: {InterfaceLayerName}",
                delegate {
                    if (lastUpdateUiGameTime != null && userInterface?.CurrentState != null)
                        userInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                    return true;
                },
                InterfaceScaleType));
        }
    }
}