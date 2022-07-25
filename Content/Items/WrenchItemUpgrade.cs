using System.Collections.Generic;
using System.IO;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Common.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuilderEssentials.Content.Items;

[Autoload(false)]
public class WrenchItemUpgrade : ModItem
{
    public int UpgradeNumber { get; set; }
    private string ItemTexture = "BuilderEssentials/Assets/Items/WrenchUpgrades/Upgrade";
    public override string Texture => ItemTexture + UpgradeNumber;
    protected override bool CloneNewInstances => true;

    //Can't have repeated internal names when adding content
    public override string Name => ((WrenchUpgrades) UpgradeNumber).ToString() + "Module";

    public override bool IsLoadingEnabled(Mod mod)
        => ModContent.GetInstance<MainConfig>().EnabledUpgradeModules.EnabledUpgrades[UpgradeNumber];

    public override void SetDefaults() {
        Item.width = Item.height = 38;
        Item.value = Item.sellPrice(0);
        Item.rare = ItemRarityID.Red;
    }

    public WrenchItemUpgrade() { }
    public WrenchItemUpgrade(int upgradeNumber) => UpgradeNumber = upgradeNumber;

    public override ModItem Clone(Item newEntity) {
        WrenchItemUpgrade newInstance = (WrenchItemUpgrade) base.Clone(newEntity);
        newInstance.UpgradeNumber = UpgradeNumber;
        return newInstance;
    }
}