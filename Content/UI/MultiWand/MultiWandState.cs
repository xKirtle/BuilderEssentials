using BuilderEssentials.Content.Items;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.UI;

public class MultiWandState : BaseUIState
{
    public static MultiWandState Instance;
    public MultiWandPanel menuPanel;
    public override int BoundItemType => ModContent.GetInstance<MultiWand>().Type;

    public MultiWandState()
    {
        Instance = this;
        menuPanel = new MultiWandPanel();
        Append(menuPanel);
    }
    
    public override void Dispose() {
        Instance = null;
    }
}