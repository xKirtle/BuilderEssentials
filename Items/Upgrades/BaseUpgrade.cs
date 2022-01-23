using BuilderEssentials.Utilities;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace BuilderEssentials.Items.Upgrades
{
    [Autoload(false)]
    internal class WrenchItemUpgrade : ModItem
    {
        string ItemTexture = "BuilderEssentials/Textures/Items/Upgrades/Upgrade";
        public override string Texture => ItemTexture + upgradeNumber;
        private int upgradeNumber;
        
        public static string[] upgradeNames =
        {
            "FastPlacement",
            "InfinitePlayerRange", 
            "PlacementAnywhere", 
            "InfinitePlacement",
            "InfinitePickupRange"
        };

        //Have to change the Name so instances differ when calling Mod.AddContent
        public override string Name => upgradeNames[upgradeNumber];

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Red;
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public WrenchItemUpgrade(int upgradeNumber)
        {
            this.upgradeNumber = upgradeNumber;
        }

        public override ModItem Clone(Item item)
        {
            WrenchItemUpgrade clone = (WrenchItemUpgrade) base.Clone(item);
            clone.upgradeNumber = upgradeNumber;
            return clone;
        }

        public int GetUpgradeNumber() => upgradeNumber;
    }
}