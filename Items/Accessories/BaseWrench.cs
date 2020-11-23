using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public abstract class BaseWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/Wrench";
        private TagCompound data;
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

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //tooltips.RemoveAll(x => x.text != "");

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

        public override void UpdateInventory(Player player)
        {
            string text = "";
            upgrades.ForEach(x => text += (x.ToInt() + " "));
            Main.NewText(text);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ExtendoGrip);
            recipe.AddIngredient(ItemID.Toolbelt);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe upgrade0 = new ModRecipe(mod);
            upgrade0.AddIngredient(this);
            upgrade0.AddIngredient(ItemID.Sapphire);
            upgrade0.SetResult(this);
            upgrade0.AddRecipe();

            ModRecipe upgrade1 = new ModRecipe(mod);
            upgrade1.AddIngredient(this);
            upgrade1.AddIngredient(ItemID.Ruby);
            upgrade1.SetResult(this);
            upgrade1.AddRecipe();

            ModRecipe upgrade2 = new ModRecipe(mod);
            upgrade2.AddIngredient(this);
            upgrade2.AddIngredient(ItemID.Emerald);
            upgrade2.SetResult(this);
            upgrade2.AddRecipe();

            ModRecipe upgrade3 = new ModRecipe(mod);
            upgrade3.AddIngredient(this);
            upgrade3.AddIngredient(ItemID.Topaz);
            upgrade3.SetResult(this);
            upgrade3.AddRecipe();

            ModRecipe upgrade4 = new ModRecipe(mod);
            upgrade4.AddIngredient(this);
            upgrade4.AddIngredient(ItemID.Amethyst);
            upgrade4.SetResult(this);
            upgrade4.AddRecipe();
        }

        public override void OnCraft(Recipe recipe)
        {
            switch (recipe.requiredItem[1].type)
            {
                case ItemID.Sapphire:
                    SetUpgrade(WrenchUpgrade.FastPlacement, true);
                    break;
                case ItemID.Ruby:
                    SetUpgrade(WrenchUpgrade.InfPlacementRange, true);
                    break;
                case ItemID.Emerald:
                    SetUpgrade(WrenchUpgrade.InfPlayerRange, true);
                    break;
                case ItemID.Topaz:
                    SetUpgrade(WrenchUpgrade.PlacementAnywhere, true);
                    break;
                case ItemID.Amethyst:
                    SetUpgrade(WrenchUpgrade.InfPlacement, true);
                    break;
            }
        }

        public override TagCompound Save()
        {
            data = new TagCompound
            {
                {"upgrades", upgrades}
            };

            return data;
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("upgrades"))
                upgrades = tag.Get<List<bool>>("upgrades");
        }

        //do net code?
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
    }
}