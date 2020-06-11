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
    class InfiniteUpgrade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows infinite placement");
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
                    otherAccessory.modItem is InfinitePlacement)
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
            player.AddBuff(mod.BuffType("InfinitePlacementBuff"), 90);
        }

        public override void AddRecipes()
        {
            //Not really worried about balancing at this point
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.LunarBar, 40);
            modRecipe.AddIngredient(ItemID.FragmentNebula, 20);
            modRecipe.AddIngredient(ItemID.FragmentSolar, 20);
            modRecipe.AddIngredient(ItemID.FragmentStardust, 20);
            modRecipe.AddIngredient(ItemID.FragmentVortex, 20);
            modRecipe.AddTile(TileID.LunarCraftingStation);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
