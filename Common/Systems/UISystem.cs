using System;
using System.Collections.Generic;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Common.Systems;

public class UISystem : ModSystem
{
    internal List<BaseUIState> uiStates;
    internal UserInterface userInterface;
    internal Vector2 cachedMouseCoords;

    public void ChangeOrToggleUIState(UIStateType uiStateType) {
        int index = (int) uiStateType - 1;
        if (uiStateType == UIStateType.None || userInterface?.CurrentState == uiStates[index]) {
            if (uiStateType != UIStateType.None)
                uiStates[index].Deactivate();
            userInterface?.SetState(null);
        }
        else userInterface?.SetState(uiStates[index]);
    }

    public override void Load() {
        if (!Main.dedServ && Main.netMode != NetmodeID.Server) {
            uiStates = new() {
                new AutoHammerState(),
            };
            userInterface = new UserInterface();
        }
    }

    public override void Unload() {
        uiStates?.ForEach(uiState => {
            uiState.Dispose();
            uiState = null;
        });

        userInterface = null;
    }

    private GameTime lastUpdateUiGameTime;
    public override void UpdateUI(GameTime gameTime) {
        cachedMouseCoords = Main.MouseScreen;

        lastUpdateUiGameTime = gameTime;
        if (userInterface?.CurrentState != null)
            userInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
        if (interfaceLayer != -1) {
            layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer("Builder Essentials: Cursor",
                delegate {
                    if (lastUpdateUiGameTime != null && userInterface?.CurrentState != null)
                        userInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
    
    //TODO: Figure out why in the right/bottom edges there's a small radius where it messes it up slightly (very unnoticeable)
    public static void PreventOffScreen(UIElement element, Vector2 center, Vector2 size = default) {
        size = size == default ? new Vector2(element.Width.Pixels, element.Height.Pixels) : size;
        Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.UIScale;
        
        float offsetX = Utils.Clamp(center.X - size.X / 2, 0, screenSize.X - size.X);
        float offsetY = Utils.Clamp(center.Y - size.Y / 2, 0, screenSize.Y - size.Y);
        
        element.Left.Set(offsetX, 0f);
        element.Top.Set(offsetY, 0f);
    }
}