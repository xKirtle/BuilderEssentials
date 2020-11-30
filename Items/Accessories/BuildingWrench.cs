using System.Collections.Generic;
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
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            if (upgrades == null) upgrades = Enumerable.Repeat(false, WrenchUpgrade.UpgradesCount.ToInt()).ToList();
            if (unlockedUpgrades == null) unlockedUpgrades = Enumerable.Repeat(false, WrenchUpgrade.UpgradesCount.ToInt()).ToList();
        }

        public override bool CloneNewInstances => true;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //TODO: Make Wrench do something on its own without upgrades?
            if (player.whoAmI != Main.myPlayer) return;
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            UIStateLogic4.wrenchUpgrades.UpdateUpgrades(player, ref upgrades, ref unlockedUpgrades);
            UIStateLogic4.wrenchUpgrades.Show();
        }

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
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
            //TODO: Lists are set to default when loading data
            
            if (tag.ContainsKey("upgrades"))
                upgrades = tag.Get<List<bool>>("upgrades");

            if (tag.ContainsKey("unlockedUpgrades"))
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
            recipe.AddIngredient(ItemID.ExtendoGrip);
            recipe.AddIngredient(ItemID.Toolbelt);
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
            
            //TODO: Come up with ugprades recipes
            // recipe = new ModRecipe(mod);
            // recipe.SetResult(upgradeItemTypes[0]);
            // recipe.AddRecipe();
        }
    }
}