using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    public class ImprovedRuler : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/ImprovedRuler";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Only works when equipped and player's empty handed." +
                               "\nHold Left Click to draw a line." +
                               "\nHold Right Click to curve the line.");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 42;
            item.height = 42;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI != Main.myPlayer) return;
            player.GetModPlayer<BEPlayer>().ImprovedRulerEquipped = true;
        }
    }
}