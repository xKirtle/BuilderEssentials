using System;
using System.Linq;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class AutoHammerState : BaseUIState
{
    private AutoHammerPanel menuPanel;

    public override void OnInitialize() {
        base.OnInitialize();

        menuPanel = new AutoHammerPanel();
        Append(menuPanel);
    }
    
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
    }

    public override void OnActivate() {
    }

    public override void OnDeactivate() {
        //Reset variables (or null them out to unallocate memory) to keep the UI ready to be activated again
    }
    
    public void Dispose() {
        //Dispose of static references such as textures
    }
}