using System;
using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class ShapesUIState : UIState
{
    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public void Unload()
    {
        //Dispose of static references such as textures
    }

    public override void OnActivate()
    {
        //Retrieve most recent data to fill the UI
    }

    public override void OnDeactivate()
    {
        //Reset variables (or null them out to unallocate memory) to keep the UI ready to be activated again
    }
}