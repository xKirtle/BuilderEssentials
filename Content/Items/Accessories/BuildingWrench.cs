using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public int[] upgrades;
    private bool isEquipped;

    public override void SetStaticDefaults() => Tooltip.SetDefault("Just a simple wrench. What did you expect it to do?");
    
    public override void SetDefaults() {
        Item.accessory = true;
        Item.vanity = false;
        Item.width = Item.height = 48;
        Item.value = Item.sellPrice(silver: 20);
        Item.rare = ItemRarityID.Red;
        
        if (upgrades == null) upgrades = Enumerable.Repeat((int) UpgradeState.Locked, (int) WrenchUpgrades.Count).ToArray();
    }

    public override void Load() {
        
        //Programmatically add the wrench item upgrades
        for (int i = 0; i < (int) WrenchUpgrades.Count; i++)
            Mod.AddContent(new WrenchItemUpgrade(i));
    }

    public override ModItem Clone(Item newEntity) {
        BuildingWrench newInstance = (BuildingWrench)base.Clone(newEntity);
        newInstance.upgrades = (int[]) upgrades?.Clone();
        return newInstance;
    }

    public override void OnCreate(ItemCreationContext context) {
        // upgrades = Enumerable.Repeat((int) UpgradeState.Locked, (int) WrenchUpgrades.Count).ToArray();
    }

    //Called on item hover, for example
    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        tooltips.Remove(tooltips.Find(x => x.Name == "Material"));

        if (upgrades == null) return;

        if (upgrades.All(x => x == 0)) {
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:NoUpgradesAdded", "Just a simple wrench. What did you expect it to do?"));
            return;
        }

        if (!isEquipped)
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:UIToggleMenu",
            "[c/FFCC00:Equip this item to enable/disable upgrades!]"));
        else
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:UIToggleMenuEquipped",
                "[c/FFCC00:Enable/Disable wrench upgrades in the bottom left menu!]"));

        for (int i = 0; i < (int) WrenchUpgrades.Count; i++) {
            if (upgrades[i] != 0) {
                string upgradeType = ((WrenchUpgrades) i).ToString();
                string tooltip = string.Concat(upgradeType.Select(c => Char.IsUpper(c) ? $" {c}" : $"{c}")).TrimStart(' ');
                tooltips.Add(new TooltipLine(Mod, $"BuilderEssentials:{upgradeType}", tooltip));
            }
        }
    }
    
    public override void SaveData(TagCompound tag) => tag[nameof(upgrades)] = upgrades.ToList();
    public override void LoadData(TagCompound tag) => upgrades = tag.Get<List<int>>(nameof(upgrades)).ToArray();

    public void UnlockUpgrade(WrenchUpgrades upgrade) {
        if (upgrade == WrenchUpgrades.Count) return;

        if (upgrades[(int) upgrade] == (int) UpgradeState.Locked)
            upgrades[(int) upgrade] = (int) UpgradeState.Disabled;
    }

    public bool ToggleUpgrade(WrenchUpgrades upgrade) {
        if (upgrade == WrenchUpgrades.Count || upgrades[(int) upgrade] == (int) UpgradeState.Locked) return false;

        int newState = ((int) upgrades[(int) upgrade] % 2) + 1;
        upgrades[(int) upgrade] = newState;

        return upgrades[(int) upgrade] == (int) UpgradeState.Enabled;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (player.whoAmI != Main.myPlayer) return;
        player.GetModPlayer<BEPlayer>().IsWrenchEquipped = true;

        var panel = ToggleableItemsUIState.GetUIPanel<WrenchUpgradesPanel>();
        if (!panel.IsVisible)
            ToggleableItemsUIState.TogglePanelVisibility<WrenchUpgradesPanel>();
        
        isEquipped = true;
    }

    public override void UpdateInventory(Player player) => isEquipped = false;

    public static List<Recipe> upgradeRecipes = new ((int) WrenchUpgrades.Count);
    
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
            int index = i;
            
            //Literally impossible to access the item instance before it is consumed..
            Recipe recipe = CreateRecipe()
                .AddIngredient(Type)
                .AddIngredient(Mod, ((WrenchUpgrades) index).ToString() + "Module")
                .AddTile(TileID.TinkerersWorkbench)
                .AddConsumeItemCallback(((Recipe recipe, int type, ref int amount) => {
                    if (type != Item.type) return; 

                    for (int j = 0; j < Main.LocalPlayer.inventory.Length; j++) {
                        Item item = Main.LocalPlayer.inventory[j];
                        if (item.type == Item.type) {
                            WrenchUpgrades.TryParse(recipe.requiredItem[1].Name.Replace(" ", "").Replace("Module", ""),
                                out WrenchUpgrades upgrade);
                            int[] previousUpgrades = (int[]) (item.ModItem as BuildingWrench).upgrades.Clone();
                            queuedRecipeChanges.Enqueue(new Tuple<int[], WrenchUpgrades>(previousUpgrades, upgrade));
                            break;
                        }
                    }
                }))
                .Register();
            
                (recipe.createItem.ModItem as BuildingWrench).UnlockUpgrade((WrenchUpgrades) index);
                upgradeRecipes.Add(recipe);
        }
        
        //Fast Placement
        CreateRecipe()
            .AddIngredient(ItemID.BrickLayer)
            .AddIngredient(ItemID.PortableCementMixer)
            .AddTile(TileID.Anvils)
            .Register().ReplaceResult(Mod, WrenchUpgrades.FastPlacement.ToString() + "Module");
        
        //Infinite Range
        CreateRecipe()
            .AddIngredient(ItemID.ExtendoGrip)
            .AddIngredient(ItemID.Toolbelt)
            .AddTile(TileID.MythrilAnvil)
            .Register().ReplaceResult(Mod, WrenchUpgrades.InfiniteRange.ToString() + "Module");
        
        //Infinite Placement
        CreateRecipe()
            .AddIngredient(ItemID.TempleKey, 3)
            .AddIngredient(ItemID.LihzahrdPowerCell, 5)
            .AddTile(TileID.AdamantiteForge)
            .Register().ReplaceResult(Mod, WrenchUpgrades.InfinitePlacement.ToString() + "Module");
        
        //Infinite Pickup Range
        CreateRecipe()
            .AddIngredient(ItemID.DemoniteOre, 20)
            .AddTile(TileID.Anvils)
            .Register().ReplaceResult(Mod, WrenchUpgrades.InfinitePickupRange.ToString() + "Module");
        
        CreateRecipe()
            .AddIngredient(ItemID.CrimtaneOre, 20)
            .AddTile(TileID.Anvils)
            .Register().ReplaceResult(Mod, WrenchUpgrades.InfinitePickupRange.ToString() + "Module");
    }

    public static UniqueQueue<Tuple<int[], WrenchUpgrades>> queuedRecipeChanges = new();
    public static void DequeueRecipeChanges() {
        while (queuedRecipeChanges.Count != 0) {
            (int[] previousUpgrades, WrenchUpgrades upgrade) = queuedRecipeChanges.Dequeue();
            
            Item resultItem = Main.mouseItem;
            (resultItem.ModItem as BuildingWrench).upgrades = previousUpgrades;
            (resultItem.ModItem as BuildingWrench).UnlockUpgrade(upgrade);
        }
        
        //Scan inventory for the first wrench it finds, if any
        //Disable/Enable recipes depending on which upgrades it has
        Item wrench = Main.LocalPlayer.inventory.SkipLast(1).FirstOrDefault(x => x.type == ModContent.ItemType<BuildingWrench>());
        if (wrench == null) return;

        int[] upgrades = (int[]) (wrench.ModItem as BuildingWrench).upgrades.Clone();

        for (int i = 0; i < upgrades.Length; i++) {
            if (upgrades[i] != (int) UpgradeState.Locked && !upgradeRecipes[i].HasCondition(FalseCondition)) {
                upgradeRecipes[i].AddCondition(FalseCondition);
                Recipe.FindRecipes();
            }
            else if (upgrades[i] == (int) UpgradeState.Locked) {
                upgradeRecipes[i].RemoveCondition(FalseCondition);
                
                upgradeRecipes[i].requiredItem[0] = wrench;
                Item resultItem = new Item(wrench.type);
                (resultItem.ModItem as BuildingWrench).upgrades = (int[]) upgrades.Clone();
                (resultItem.ModItem as BuildingWrench).UnlockUpgrade((WrenchUpgrades)i);
                upgradeRecipes[i].createItem = resultItem;
                
                Recipe.FindRecipes();
            }
        }
    }

    private static Recipe.Condition FalseCondition = new Recipe.Condition(NetworkText.Empty, recipe => false);
}