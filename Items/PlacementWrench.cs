using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class PlacementWrench : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows infinite range");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        protected (int index, Item accessory) FindDifferentEquippedExclusiveAccessory()
        {
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                Item otherAccessory = Main.LocalPlayer.armor[i];
                if (!otherAccessory.IsAir &&
                    !item.IsTheSameAs(otherAccessory) &&
                    otherAccessory.modItem is PlacementWrench)
                {
                    return (i, otherAccessory);
                }
            }
            return (-1, null);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot < 10)
            {
                int index = FindDifferentEquippedExclusiveAccessory().index;
                if (index != -1)
                    return slot == index;
            }
            return base.CanEquipAccessory(player, slot);
        }

        public override bool CanRightClick()
        {
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            for (int i = 13; i < 13 + maxAccessoryIndex; i++)
            {
                if (Main.LocalPlayer.armor[i].type == item.type)
                    return false;
            }

            if (FindDifferentEquippedExclusiveAccessory().accessory != null)
                return true;

            return base.CanRightClick();
        }

        public override void RightClick(Player player)
        {
            var (index, accessory) = FindDifferentEquippedExclusiveAccessory();
            if (accessory != null)
            {
                Main.LocalPlayer.QuickSpawnClonedItem(accessory);
                Main.LocalPlayer.armor[index] = item.Clone();
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.blockRange += 55;
            player.wallSpeed += 10;
            player.tileSpeed += 50;
            Player.tileRangeX = 65;
            Player.tileRangeY = 55;
        }

        public override void AddRecipes()
        {
            //Not really worried about balancing at this point
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.ArchitectGizmoPack, 1);
            modRecipe.AddIngredient(ItemID.LaserRuler, 1);
            modRecipe.AddIngredient(ItemID.Toolbox, 1);
            modRecipe.AddIngredient(ItemID.Toolbelt, 1);
            modRecipe.AddTile(TileID.AdamantiteForge);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
