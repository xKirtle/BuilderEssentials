using System;
using System.Collections.Generic;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
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
            //TODO: Get this by classes extending BaseUIState?
            uiStates = new() {
                new AutoHammerState(),
                new MultiWandState(),
                new PaintBrushState() 
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
    
    public static void PreventElementOffScreen(UIElement element, Vector2 center, Vector2 size = default) {
        size = size == default ? new Vector2(element.Width.Pixels, element.Height.Pixels) : size;
        Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.UIScale;
        
        float offsetX = Utils.Clamp(center.X - size.X / 2, 0, screenSize.X - size.X);
        float offsetY = Utils.Clamp(center.Y - size.Y / 2, 0, screenSize.Y - size.Y);
        
        element.Left.Set(offsetX, 0f);
        element.Top.Set(offsetY, 0f);
    }

    //This method is wacky
    public static void PreventTextOffScreen(UIElement parent, UIText uiText, Vector2 center, Vector2 centerOffset) {
        Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        Vector2 textSize = FontAssets.MouseText.Value.MeasureString(uiText.Text);

        float offsetX = Utils.Clamp(center.X + centerOffset.X, 0, screenSize.X - textSize.X);
        offsetX -= parent.Left.Pixels - centerOffset.X;
        float offsetY = Utils.Clamp(center.Y + centerOffset.Y, 4f, screenSize.Y - textSize.Y + centerOffset.Y / 2);
        offsetY -= parent.Top.Pixels;
        
        uiText.Left.Set(offsetX, 0);
        uiText.Top.Set(offsetY, 0);
    }
}