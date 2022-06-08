namespace BuilderEssentials.Content.UI;

public class AutoHammerState : BaseUIState
{
    public static AutoHammerState Instance;
    public AutoHammerPanel menuPanel;

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