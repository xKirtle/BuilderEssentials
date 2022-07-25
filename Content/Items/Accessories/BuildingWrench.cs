using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Common.DataStructures;
using BuilderEssentials.Common.Enums;
using BuilderEssentials.Content.UI;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuilderEssentials.Content.Items.Accessories;

public class BuildingWrench : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/Accessories/" + GetType().Name;
    public int[] unlockedUpgrades;
    private MainConfig.EnabledUpgradeModulesConfig upgradesConfig => ModContent.GetInstance<MainConfig>().EnabledUpgradeModules;

    public override bool IsLoadingEnabled(Mod mod)
        => ModContent.GetInstance<MainConfig>().EnabledAccessories.BuildingWrench;

    public override void SetDefaults() {
        Item.accessory = true;
        Item.vanity = false;
        Item.width = Item.height = 48;
        Item.value = Item.sellPrice(silver: 20);
        Item.rare = ItemRarityID.Red;

        if (unlockedUpgrades == null) unlockedUpgrades = Enumerable.Repeat((int) UpgradeState.Locked, (int) WrenchUpgrades.Count).ToArray();
    }

    public override void Load() {
        //Programmatically add the wrench item upgrades
        for (int i = 0; i < (int) WrenchUpgrades.Count; i++)
            Mod.AddContent(new WrenchItemUpgrade(i));
    }

    public override ModItem Clone(Item newEntity) {
        BuildingWrench newInstance = (BuildingWrench) base.Clone(newEntity);
        newInstance.unlockedUpgrades = (int[]) unlockedUpgrades?.Clone();
        return newInstance;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        tooltips.Remove(tooltips.Find(x => x.Name == "Material"));

        if (unlockedUpgrades == null) return;

        if (unlockedUpgrades.All(x => x == 0)) {
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:NoUpgradesAdded", "Just a simple wrench. What did you expect it to do?"));
            return;
        }

        if (Main.LocalPlayer.GetModPlayer<BEPlayer>().EquippedWrenchInstance == null) {
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:UIToggleMenu",
                "[c/FFCC00:Equip this item to enable/disable upgrades!]"));
        }
        else {
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:UIToggleMenuEquipped",
                "[c/FFCC00:Enable/Disable wrench upgrades in the bottom left menu!]"));
        }

        for (int i = 0; i < (int) WrenchUpgrades.Count; i++) {
            if (!upgradesConfig.EnabledUpgrades[i]) continue;

            if (unlockedUpgrades[i] != 0) {
                string upgradeType = ((WrenchUpgrades) i).ToString();
                string tooltip = string.Concat(upgradeType.Select(c => char.IsUpper(c) ? $" {c}" : $"{c}")).TrimStart(' ');
                tooltips.Add(new TooltipLine(Mod, $"BuilderEssentials:{upgradeType}", tooltip));
            }
        }
    }

    public override void SaveData(TagCompound tag) => tag[nameof(unlockedUpgrades)] = unlockedUpgrades.ToList();
    public override void LoadData(TagCompound tag) => unlockedUpgrades = tag.Get<List<int>>(nameof(unlockedUpgrades)).ToArray();

    public void UnlockUpgrade(WrenchUpgrades upgrade) {
        if (upgrade == WrenchUpgrades.Count) return;
        unlockedUpgrades[(int) upgrade] = (int) UpgradeState.Unlocked;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (player.whoAmI != Main.myPlayer) return;
        BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
        mp.EquippedWrenchInstance = this;

        WrenchUpgradesPanel panel = ToggleableItemsUIState.GetUIPanel<WrenchUpgradesPanel>();
        mp.FastPlacement = panel.enabledUpgrades[0] && upgradesConfig.FastPlacement;
        mp.InfiniteRange = panel.enabledUpgrades[1] && upgradesConfig.InfiniteRange;
        mp.InfinitePlacement = panel.enabledUpgrades[2] && upgradesConfig.InfinitePlacement;
        mp.InfinitePickupRange = panel.enabledUpgrades[3] && upgradesConfig.InfinitePickupRange;
    }

    public static List<Recipe> upgradeRecipes = new List<Recipe>((int) WrenchUpgrades.Count);

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.LeadBar, 6)
            .AddTile(TileID.Anvils)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.IronBar, 6)
            .AddTile(TileID.Anvils)
            .Register();

        for (int i = 0; i < (int) WrenchUpgrades.Count; i++) {
            if (!upgradesConfig.EnabledUpgrades[i]) continue;

            int index = i;
            Recipe recipe = CreateRecipe()
                .AddIngredient(Type)
                .AddIngredient(Mod, ((WrenchUpgrades) index).ToString() + "Module")
                .AddTile(TileID.TinkerersWorkbench)
                .AddConsumeItemCallback((Recipe recipe, int type, ref int amount) => {
                    if (type != Item.type) return;

                    for (int j = 0; j < Main.LocalPlayer.inventory.Length; j++) {
                        Item item = Main.LocalPlayer.inventory[j];
                        if (item.type == Item.type) {
                            Enum.TryParse(recipe.requiredItem[1].Name.Replace(" ", "").Replace("Module", ""),
                                out WrenchUpgrades upgrade);
                            int[] previousUpgrades = (int[]) (item.ModItem as BuildingWrench).unlockedUpgrades.Clone();
                            queuedRecipeChanges.Enqueue(new Tuple<int[], WrenchUpgrades>(previousUpgrades, upgrade));
                            break;
                        }
                    }
                })
                .Register();

            (recipe.createItem.ModItem as BuildingWrench).UnlockUpgrade((WrenchUpgrades) index);
            upgradeRecipes.Add(recipe);
        }

        //Fast Placement
        if (upgradesConfig.FastPlacement) {
            CreateRecipe()
                .AddIngredient(ItemID.BrickLayer)
                .AddIngredient(ItemID.PortableCementMixer)
                .AddIngredient(ItemID.Sapphire, 5)
                .AddTile(TileID.Anvils)
                .Register().ReplaceResult(Mod, WrenchUpgrades.FastPlacement.ToString() + "Module");
        }

        //Infinite Range
        if (upgradesConfig.InfiniteRange) {
            CreateRecipe()
                .AddIngredient(ItemID.ExtendoGrip)
                .AddIngredient(ItemID.Toolbelt)
                .AddIngredient(ItemID.Ruby, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register().ReplaceResult(Mod, WrenchUpgrades.InfiniteRange.ToString() + "Module");
        }


        //Infinite Placement
        if (upgradesConfig.InfinitePlacement) {
            CreateRecipe()
                .AddIngredient(ItemID.TempleKey, 3)
                .AddIngredient(ItemID.LihzahrdPowerCell, 5)
                .AddIngredient(ItemID.Emerald, 5)
                .AddTile(TileID.AdamantiteForge)
                .Register().ReplaceResult(Mod, WrenchUpgrades.InfinitePlacement.ToString() + "Module");
        }

        //Infinite Pickup Range
        if (upgradesConfig.InfinitePickupRange) {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteOre, 20)
                .AddIngredient(ItemID.Topaz, 5)
                .AddTile(TileID.Anvils)
                .Register().ReplaceResult(Mod, WrenchUpgrades.InfinitePickupRange.ToString() + "Module");

            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneOre, 20)
                .AddIngredient(ItemID.Topaz, 5)
                .AddTile(TileID.Anvils)
                .Register().ReplaceResult(Mod, WrenchUpgrades.InfinitePickupRange.ToString() + "Module");
        }
    }

    public static UniqueQueue<Tuple<int[], WrenchUpgrades>> queuedRecipeChanges = new UniqueQueue<Tuple<int[], WrenchUpgrades>>();
    public static void DequeueRecipeChanges() {
        while (queuedRecipeChanges.Count != 0) {
            (int[] previousUpgrades, WrenchUpgrades upgrade) = queuedRecipeChanges.Dequeue();

            Item resultItem = Main.mouseItem;
            (resultItem.ModItem as BuildingWrench).unlockedUpgrades = previousUpgrades;
            (resultItem.ModItem as BuildingWrench).UnlockUpgrade(upgrade);
        }

        //Scan inventory for the first wrench it finds, if any
        //Disable/Enable recipes depending on which upgrades it has
        Item wrench = Main.LocalPlayer.inventory.SkipLast(1).FirstOrDefault(x => x.type == ModContent.ItemType<BuildingWrench>());
        if (wrench == null) return;

        int[] upgrades = (int[]) (wrench.ModItem as BuildingWrench).unlockedUpgrades.Clone();

        for (int i = 0; i < upgrades.Length; i++) {
            if (upgrades[i] != (int) UpgradeState.Locked && !upgradeRecipes[i].HasCondition(FalseCondition)) {
                upgradeRecipes[i].AddCondition(FalseCondition);
                Recipe.FindRecipes();
            }
            else if (upgrades[i] == (int) UpgradeState.Locked) {
                upgradeRecipes[i].RemoveCondition(FalseCondition);

                upgradeRecipes[i].requiredItem[0] = wrench;
                Item resultItem = new Item(wrench.type);
                (resultItem.ModItem as BuildingWrench).unlockedUpgrades = (int[]) upgrades.Clone();
                (resultItem.ModItem as BuildingWrench).UnlockUpgrade((WrenchUpgrades) i);
                upgradeRecipes[i].createItem = resultItem;

                Recipe.FindRecipes();
            }
        }
    }

    private static Recipe.Condition FalseCondition = new Recipe.Condition(NetworkText.Empty, recipe => false);
}