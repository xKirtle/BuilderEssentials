using System;
using System.Collections.Generic;
using BuilderEssentials.Content.UI.UIStates;
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

    public void ChangeOrToggleUIState(UIStateType uiStateType)
    {
        int index = (int) uiStateType - 1;
        if (uiStateType == UIStateType.None || userInterface?.CurrentState == uiStates[index])
        {
            if (uiStateType != UIStateType.None)
                uiStates[index].Deactivate();
            userInterface?.SetState(null);
        }
        else
        {
            uiStates[index].Activate();
            userInterface?.SetState(uiStates[index]);
        }
    }

    public override void Load()
    {
        if (!Main.dedServ && Main.netMode != NetmodeID.Server)
        {
            uiStates = new()
            {
                new AutoHammerUIState(),
            };
            userInterface = new UserInterface();
        }
    }

    public override void Unload()
    {
        uiStates?.ForEach(uiState =>
        {
            uiState.Dispose();
            uiState = null;
        });

        userInterface = null;
    }

    private GameTime lastUpdateUiGameTime;
    public override void UpdateUI(GameTime gameTime)
    {
        lastUpdateUiGameTime = gameTime;
        if (userInterface?.CurrentState != null)
            userInterface.Update(gameTime);

        cachedMouseCoords = new Vector2(Main.mouseX, Main.mouseY);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        //https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values
        int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
        if (interfaceLayer != -1)
        {
            layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer("Builder Essentials: Cursor",
                delegate
                {
                    if (lastUpdateUiGameTime != null && userInterface?.CurrentState != null)
                        userInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);

                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}