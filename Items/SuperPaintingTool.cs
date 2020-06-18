using System.Collections.Generic;
using BuilderEssentials.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class SuperPaintingTool : ModItem
    {
        public List<int> paints;
        public override void SetDefaults()
        {
            paints = new List<int>();
            for (int i = 0; i < 27; i++) //Basic && Deep colors
                paints.Add(1073 + i);
            for (int i = 0; i < 3; i++)
                paints.Add(1966 + i);   //Extra Effects

            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        int mouseRightTimer = 0;
        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.mouseRight && player.talkNPC == -1 && !Main.HoveringOverAnNPC && !player.showItemIcon && !Main.editSign
                        && !Main.editChest && !Main.blockInput && !player.dead && !Main.gamePaused && Main.hasFocus && !player.CCed
                        && !player.mouseInterface && player.inventory[player.selectedItem].IsTheSameAs(this.item))
                {
                    if (++mouseRightTimer == 2)
                        BasePanel.paintingUIOpen = !BasePanel.paintingUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;
            }
        }
    }

    public class PaintGlobalItem : GlobalItem
    {
        public override bool UseItem(Item item, Player player)
        {

            //if item id is a paint tool and there is paint in the inventory, reduce the stack by 1?
            //if infinitepaintbucket is on the inventory, allow any color
            return true;
        }
    }
}