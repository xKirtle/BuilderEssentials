using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuilderEssentials.Items.Upgrades;
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

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 48;
            item.height = 48;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            if (upgrades == null) {
                upgrades = Enumerable.Repeat(false, WrenchUpgrade.UpgradesCount.ToInt()).ToList();
            }
        }

        public override bool CloneNewInstances => true;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI != Main.myPlayer) return;
            UpdateUpgrades(player);
        }
        
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //TODO: Remove the Material tooltip
            if (upgrades[0]) //Fast placement
                tooltips.Add(new TooltipLine(mod, "Fast Placement", "Allows fast placement"));
            if (upgrades[1]) //Inf Placement Range
                tooltips.Add(new TooltipLine(mod, "Infinite Placement Range", "Allows infinite placement range"));
            if (upgrades[2]) //Inf Player Range
                tooltips.Add(new TooltipLine(mod, "Infinite Player Range", "Allows infinite player range"));
            if (upgrades[3]) //Placement Anywhere
                tooltips.Add(new TooltipLine(mod, "Placement Anywhere", "Allows tile placement anywhere on screen"));
            if (upgrades[4]) //Inf Placement
                tooltips.Add(new TooltipLine(mod, "Infinite Placement", "Allows infinite placement"));
        }

        public void UpdateUpgrades(Player player)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            if (upgrades[0]) //Fast placement
                mp.FastPlacement = true;
            if (upgrades[1]) //Inf Placement Range
                mp.InfinitePlacementRange = true;
            if (upgrades[2]) //Inf Player Range
                mp.InfinitePlayerRange = true;
            if (upgrades[3]) //Placement Anywhere
                mp.PlacementAnywhere = true;
            if (upgrades[4]) //Inf Placement
                mp.InfinitePlacement = true;
        }

        public override void OnCraft(Recipe recipe)
        {
            WrenchUpgrade[] upgrades = new[]
            {
                WrenchUpgrade.FastPlacement, 
                WrenchUpgrade.InfPlacementRange, 
                WrenchUpgrade.InfPlayerRange,
                WrenchUpgrade.PlacementAnywhere, 
                WrenchUpgrade.InfPlacement
            };
            int upgradeIndex = ((BaseUpgrade) recipe.requiredItem[1].modItem).GetUpgradeNumber();
            Main.NewText(upgradeIndex);
            SetUpgrade(upgrades[upgradeIndex], true);
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"upgrades", upgrades}
            };
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("upgrades"))
                upgrades = tag.Get<List<bool>>("upgrades");
        }

        //TODO: Check if net code is needed for mp compatibility
        //https://github.com/tModLoader/tModLoader/blob/321a00a42ba89db68ec25ef3f57498df92a1b86f/ExampleMod/Items/ExampleCustomData.cs#L65

        public void SetUpgrade(WrenchUpgrade upgrade, bool state)
        {
            if (upgrade.ToInt() < upgrades.Count && upgrade.ToInt() >= 0)
                upgrades[upgrade.ToInt()] = state;
        }

        public bool GetUpgrade(WrenchUpgrade upgrade)
        {
            if (upgrade.ToInt() < upgrades.Count && upgrade.ToInt() >= 0) return false;

            return upgrades[upgrade.ToInt()];
        }
        
        public override void AddRecipes()
        {
            int[] upgradeItemTypes =
            {
                mod.GetItem("FastPlacementUpgrade").item.type,
                mod.GetItem("InfinitePlacementRangeUpgrade").item.type,
                mod.GetItem("InfinitePlayerRangeUpgrade").item.type,
                mod.GetItem("PlacementAnywhereUpgrade").item.type,
                mod.GetItem("InfinitePlacementUpgrade").item.type
            };
            
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ExtendoGrip);
            recipe.AddIngredient(ItemID.Toolbelt);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            for (int i = 0; i < 5; i++)
            {
                ModRecipe upgrade = new ModRecipe(mod);
                upgrade.AddIngredient(this);
                upgrade.AddIngredient(upgradeItemTypes[i]);
                upgrade.SetResult(this);
                upgrade.AddRecipe();
            }
        }
    }
}