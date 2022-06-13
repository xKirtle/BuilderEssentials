using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.UI;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Common;

public enum UIStateType
{
    None,
    AutoHammer,
    MultiWand,
    PaintBrush,
    Count
}

public static class EnumExtensions
{
    public static BaseUIState GetInstance(this UIStateType uiStateType) {
        if (uiStateType == UIStateType.None || uiStateType == UIStateType.Count)
            return null;

        return ModContent.GetInstance<UISystem>().uiStates[(int) uiStateType];
    }
}