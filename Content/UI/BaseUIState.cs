using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public abstract class BaseUIState : UIState, IDisposable
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

    public override void OnActivate()
    {
        //Retrieve most recent data to fill the UI
    }

    public override void OnDeactivate()
    {
        //Reset variables (or null them out to unallocate memory) to keep the UI ready to be activated again
    }
    
    public void Dispose()
    {
        //Dispose of static references such as textures
    }
}