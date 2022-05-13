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
    internal UserInterface userInterface;
    internal ShapesUIState shapesUiState;
    internal Vector2 cachedMouseCoords;
    
    public override void Load()
    {
        if (!Main.dedServ && Main.netMode != NetmodeID.Server)
        {
            userInterface = new UserInterface();
            shapesUiState = new ShapesUIState();
            // userInterface.SetState(shapesUiState);
        }
    }

    public override void Unload()
    {
        shapesUiState.Unload();
        shapesUiState = null;
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