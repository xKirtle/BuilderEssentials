﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuilderEssentials.Items.Upgrades;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BuilderEssentials.Utilities;
using static BuilderEssentials.Utilities.HelperMethods;
using Terraria.Utilities;

namespace BuilderEssentials.Items.Accessories
{
    public class BuildingWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/BuildingWrench";
        public List<bool> upgrades;
        public List<bool> unlockedUpgrades;

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 48;
            item.height = 48;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = ItemRarityID.Red;
            if (upgrades == null) upgrades = Enumerable.Repeat(false, WrenchUpgrade.UpgradesCount.ToInt()).ToList();
            if (unlockedUpgrades == null)
                unlockedUpgrades = Enumerable.Repeat(false, WrenchUpgrade.UpgradesCount.ToInt()).ToList();
        }

        public override bool CloneNewInstances => true;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI != Main.myPlayer) return;
            UIStateLogic4.wrenchUpgrades.UpdateUpgrades(player, ref upgrades, ref unlockedUpgrades);
            UIStateLogic4.wrenchUpgrades.Show();
        }

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            UIStateLogic4.wrenchUpgrades.Hide();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string[] names =
            {
                "Fast Placement",
                "Infinite Placement Range",
                "Infinite Player Range",
                "Placement Anywhere",
                "Infinite Placement",
                "Infinite Pickup Range"
            };

            string[] tooltipText =
            {
                "Allows fast placement",
                "Allows inifnite placement range",
                "Allows infinite player range",
                "Allows tile placement anywhere on screen",
                "Allows infinite placement",
                "Allows infinite pickup range"
            };

            tooltips.Remove(tooltips.Find(x => x.Name == "Material"));

            if (unlockedUpgrades.All(x => x == false))
                tooltips.Add(new TooltipLine(mod, "No Upgrades Added", "Add upgrades to make use of this wrench!"));
            
            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
                if (unlockedUpgrades[i])
                    tooltips.Add(new TooltipLine(mod, names[i], tooltipText[i]));
        }

        public override void OnCraft(Recipe recipe)
        {
            int upgradeIndex = ((WrenchItemUpgrade) recipe.requiredItem[1].modItem).GetUpgradeNumber();
            SetUpgrade((WrenchUpgrade) upgradeIndex, true);
            unlockedUpgrades[upgradeIndex] = true;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"upgrades", upgrades},
                {"unlockedUpgrades", unlockedUpgrades}
            };
        }

        public override void Load(TagCompound tag)
        {
            upgrades = tag.Get<List<bool>>("upgrades");
            unlockedUpgrades = tag.Get<List<bool>>("unlockedUpgrades");
        }

        public override void NetSend(BinaryWriter writer)
        {
            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
            {
                writer.Write(upgrades[i]);
                writer.Write(unlockedUpgrades[i]);
            }
        }

        public override void NetRecieve(BinaryReader reader)
        {
            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
            {
                upgrades[i] = reader.ReadBoolean();
                unlockedUpgrades[i] = reader.ReadBoolean();
            }
        }

        public void SetUpgrade(WrenchUpgrade upgrade, bool state)
        {
            if (upgrade.ToInt() < upgrades.Count && upgrade.ToInt() >= 0 && unlockedUpgrades[upgrade.ToInt()])
                upgrades[upgrade.ToInt()] = state;
        }

        public bool GetUpgrade(WrenchUpgrade upgrade)
        {
            if (upgrade.ToInt() < upgrades.Count && upgrade.ToInt() >= 0) return false;

            return upgrades[upgrade.ToInt()];
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            int[] upgradeItemTypes =
            {
                mod.GetItem("FastPlacementUpgrade").item.type,
                mod.GetItem("InfinitePlacementRangeUpgrade").item.type,
                mod.GetItem("InfinitePlayerRangeUpgrade").item.type,
                mod.GetItem("PlacementAnywhereUpgrade").item.type,
                mod.GetItem("InfinitePlacementUpgrade").item.type,
                mod.GetItem("InfinitePickupRangeUpgrade").item.type
            };

            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
            {
                ModRecipe upgradeRecipe = new ModRecipe(mod);
                upgradeRecipe.AddIngredient(this);
                upgradeRecipe.AddIngredient(upgradeItemTypes[i]);
                upgradeRecipe.SetResult(this);
                upgradeRecipe.AddRecipe();
            }
            
            //Recipes probably aren't very balanced
            
            //Fast Placement
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BrickLayer);
            recipe.AddIngredient(ItemID.PortableCementMixer);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(upgradeItemTypes[0]);
            recipe.AddRecipe();
            
            //Infinite Placement Range
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ExtendoGrip);
            recipe.AddIngredient(ItemID.Toolbelt);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(upgradeItemTypes[1]);
            recipe.AddRecipe();
            
            //Infinite Player Range (should be much harder and late game)
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofMight, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(upgradeItemTypes[2]);
            recipe.AddRecipe();
            
            //Placement Anywhere (Late Pre HM or Early HM)
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pwnhammer);
            recipe.AddIngredient(ItemID.BuilderPotion, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(upgradeItemTypes[3]);
            recipe.AddRecipe();
            
            //Infinite Placement (Mid HM)
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TempleKey, 3);
            recipe.AddIngredient(ItemID.LihzahrdPowerCell, 5);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(upgradeItemTypes[4]);
            recipe.AddRecipe();
            
            //Infinite Pickup Range (Mid Pre HM)
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteOre, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(upgradeItemTypes[5]);
            recipe.AddRecipe();
            
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneOre, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(upgradeItemTypes[5]);
            recipe.AddRecipe();
        }
    }
}