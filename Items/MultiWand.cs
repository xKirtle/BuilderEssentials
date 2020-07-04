using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class MultiWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Contains all wands");
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
            item.noMelee = false;
        }

        int mouseRightTimer = 0;
        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (BasePanel.wandsWheelPanel != null && !player.inventory[player.selectedItem].IsTheSameAs(item))
                {
                    BasePanel.wandsWheelPanel.Remove();
                    BasePanel.wandsWheelUIOpen = false;
                }

                if (Main.mouseRight && Tools.IsUIAvailable()
                        && (!player.mouseInterface || (BasePanel.wandsWheelUIOpen && BasePanel.wandsWheelPanel.IsMouseHovering))
                        && player.inventory[player.selectedItem].IsTheSameAs(item))
                {
                    if (++mouseRightTimer == 2)
                        BasePanel.wandsWheelUIOpen = !BasePanel.wandsWheelUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;
            }
        }
    }
}
