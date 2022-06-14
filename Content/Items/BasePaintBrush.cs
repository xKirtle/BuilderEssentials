using BuilderEssentials.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(false)]
public abstract class BasePaintBrush : BaseItemToggleableUI
{
    public override UIStateType UiStateType => UIStateType.PaintBrush;
    
    public override void SetDefaults() {
        base.SetDefaults();
        
        Item.height = 44;
        Item.width = 44;
        Item.useTime = 1;
        Item.useAnimation = 1;
        Item.useTurn = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.sellPrice(silver: 80);
        Item.rare = ItemRarityID.Red;
        Item.autoReuse = true;
    }
    
    public override Vector2? HoldoutOffset() => new Vector2(5, -8);

    public override void HoldItem(Player player) {
        base.HoldItem(player);
    }
    
    public static int PaintItemTypeToColorIndex(int paintType)
    {
        //The outputed indexes are not the paint color byte values. For those just increment one.

        if (paintType >= 1073 && paintType <= 1099)
            return paintType - 1073;
        else if (paintType >= 1966 && paintType <= 1968)
            return paintType - 1939;
        else if (paintType >= 4668)
            return paintType - 4638;

        return -1; //it will never reach here
    }
    
    public static int ColorByteToPaintItemType(byte color)
    {
        if (color >= 1 && color <= 27)
            return (color - 1) + 1073;
        else if (color >= 28 && color <= 30)
            return (color - 1) + 1939;
        else if (color == 31)
            return (color - 1) + 4638;

        return -1; //it will never reach here
    }
}