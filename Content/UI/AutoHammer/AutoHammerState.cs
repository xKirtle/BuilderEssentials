using BuilderEssentials.Content.Items;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.UI;

public class AutoHammerState : BaseUIState
{
    public static AutoHammerState Instance;
    public AutoHammerPanel menuPanel;
    public override int BoundItemType => ModContent.GetInstance<AutoHammer>().Type;

    public AutoHammerState()
    {
        Instance = this;
        menuPanel = new AutoHammerPanel();
        Append(menuPanel);
    }
    
    public override void Dispose() {
        Instance = null;
    }
}