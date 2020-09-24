using BuilderEssentials.UI.ItemsUI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    class ImprovedRuler : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/ImprovedRuler";
        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 56;
            item.height = 56;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        public override void UpdateInventory(Player player)
        {
            ImprovedRulerUI.IREquipped = true;
        }

        public override void UpdateEquip(Player player)
        {
            ImprovedRulerUI.IREquipped = true;
        }
    }
}
