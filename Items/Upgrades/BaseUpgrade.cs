using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Upgrades
{
    public class WrenchItemUpgrade : ModItem
    {
        string ItemTexture = "BuilderEssentials/Textures/Items/Upgrades/Upgrade";
        public override string Texture => ItemTexture + upgradeNumber;
        private int upgradeNumber;
        
        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.rare = ItemRarityID.Red;
        }    

        public WrenchItemUpgrade() { }

        public WrenchItemUpgrade(int upgradeNumber)
        {
            this.upgradeNumber = upgradeNumber;
        }

        public override bool CloneNewInstances => true;

        string[] itemNames =
        {
            "FastPlacement", 
            "InfinitePlacementRange", 
            "InfinitePlayerRange", 
            "PlacementAnywhere", 
            "InfinitePlacement",
            "InfinitePickupRange"
        };
        
        public override bool Autoload(ref string name)
        {
            for (int i = 0; i < HelperMethods.WrenchUpgrade.UpgradesCount.ToInt(); i++)
                mod.AddItem(itemNames[i] + "Upgrade", new WrenchItemUpgrade(i));

            return false;
        }

        public int GetUpgradeNumber() => upgradeNumber;
    }
}