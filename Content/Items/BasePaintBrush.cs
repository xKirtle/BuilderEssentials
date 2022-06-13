using BuilderEssentials.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(false)]
public abstract class BasePaintBrush : BaseItemToggleableUI
{
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
}