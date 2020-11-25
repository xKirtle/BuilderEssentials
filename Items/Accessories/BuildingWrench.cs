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
            if (upgrades == null) upgrades = Enumerable.Repeat(false, WrenchUpgrade.UpgradesCount.ToInt()).ToList();
        }

        public override bool CloneNewInstances => true;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //TODO: Make Wrench do something on its own without upgrades?
            if (player.whoAmI != Main.myPlayer) return;
            UpdateUpgrades(player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string[] names =
            {
                "Fast Placement",
                "Infinite Placement Range",
                "Infinite Player Range",
                "Placement Anywhere",
                "Infinite Placement"
            };

            string[] tooltipText =
            {
                "Allows fast placement",
                "Allows inifnite placement range",
                "Allows infinite player range",
                "Allows tile placement anywhere on screen",
                "Allows infinite placement"
            };

            tooltips.Remove(tooltips.Find(x => x.Name == "Material"));

            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
                if (upgrades[i])
                    tooltips.Add(new TooltipLine(mod, names[i], tooltipText[i]));
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
            int upgradeIndex = ((WrenchItemUpgrade) recipe.requiredItem[1].modItem).GetUpgradeNumber();
            SetUpgrade((WrenchUpgrade) upgradeIndex, true);
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

        public override void NetSend(BinaryWriter writer)
        {
            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
                writer.Write(upgrades[i]);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
                upgrades[i] = reader.ReadBoolean();
        }

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
                mod.GetItem("InfinitePlacementUpgrade").item.type
            };

            for (int i = 0; i < WrenchUpgrade.UpgradesCount.ToInt(); i++)
            {
                ModRecipe upgradeRecipe = new ModRecipe(mod);
                upgradeRecipe.AddIngredient(this);
                upgradeRecipe.AddIngredient(upgradeItemTypes[i]);
                upgradeRecipe.SetResult(this);
                upgradeRecipe.AddRecipe();
            }
        }
    }
}