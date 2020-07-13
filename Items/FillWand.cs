using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    public class FillWand : ModItem
    {
        int toolRange;
        public static int fillSelectionSize = 0;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fills Holes");
        }

        public override void SetDefaults()
        {
            item.height = 32;
            item.width = 32;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            toolRange = 8;
        }

        public override void HoldItem(Player player)
        {
            //Main.NewText(fillSelectionSize);
        }
    }
}