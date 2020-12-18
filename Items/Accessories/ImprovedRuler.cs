using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    public class ImprovedRuler : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/ImprovedRuler";
        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 42;
            item.height = 42;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }
    }
}