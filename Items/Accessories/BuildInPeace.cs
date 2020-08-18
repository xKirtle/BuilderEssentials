using BuilderEssentials.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    class BuildInPeace : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/BuildInPeace";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Disables all events in the game and sets the clock to 12pm daytime");

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 30;
            item.height = 22;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) 
            => player.AddBuff(ModContent.BuffType<BuildInPeaceBuff>(), 10);
    }
}
