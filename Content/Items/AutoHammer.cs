using System;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.UI.UIStates;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class AutoHammer : BaseItemToggleableUI
{
    public override UIStateType UiStateType => UIStateType.AutoHammer;

    public override string Texture => "BuilderEssentials/Assets/Items/AutoHammer";

    protected override bool CloneNewInstances => true;

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Default Hammer");
        Tooltip.SetDefault("Better than a regular hammer!\n" +
                           "Right Click to open selection menu");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = Item.height = 44;
        Item.useTime = Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(copper: 16);
        Item.rare = ItemRarityID.Red;
        Item.damage = 26;
        Item.DamageType = DamageClass.Melee;
        Item.hammer = 80;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }
}