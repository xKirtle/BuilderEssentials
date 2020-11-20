using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BuilderEssentials.Utilities;
using static BuilderEssentials.Utilities.HelperMethods;

namespace BuilderEssentials.Items.Accessories
{
    public abstract class BaseWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/Wrench";
        public List<bool> upgrades;

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 48;
            item.height = 48;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            upgrades = Enumerable.Repeat(false, WrenchUpgrade.UpgradesCount.ToInt()).ToList();
        }

        public override bool CloneNewInstances => true;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
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
            //Main.NewText(text);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ExtendoGrip);
            recipe.AddIngredient(ItemID.Toolbelt);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
            upgrades = tag.Get<List<bool>>("upgrades");
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
    }
}