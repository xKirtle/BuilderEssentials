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
using Terraria.GameContent.Creative;
using static BuilderEssentials.Utilities.HelperMethods;
using Terraria.Utilities;

namespace BuilderEssentials.Items.Accessories
{
    internal class BuildingWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/BuildingWrench";
        public List<bool> upgrades;
        public List<bool> unlockedUpgrades;

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.vanity = false;
            Item.width = 48;
            Item.height = 48;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Red;
            if (upgrades == null) upgrades = Enumerable.Repeat(false, (int)WrenchUpgrade.UpgradesCount).ToList();
            if (unlockedUpgrades == null) 
                unlockedUpgrades = Enumerable.Repeat(false, (int)WrenchUpgrade.UpgradesCount).ToList();
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = -1;
        }

        public override void Load()
        {
            //Programmatically add the upgrade Items
            for (int i = 0; i < (int)WrenchUpgrade.UpgradesCount; i++)
                Mod.AddContent(new WrenchItemUpgrade(i));
        }

        public override ModItem Clone(Item item)
        {
            BuildingWrench clone = (BuildingWrench) base.Clone(item);
            clone.upgrades = upgrades.ToList();
            clone.unlockedUpgrades = unlockedUpgrades.ToList();
            return base.Clone(item);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string[] tooltipText =
            {
                "Allows fast placement",
                "Allows infinite player range",
                "Allows tile placement anywhere on screen",
                "Allows infinite placement",
                "Allows infinite pickup range"
            };
        
            tooltips.Remove(tooltips.Find(x => x.Name == "Material"));

            if (unlockedUpgrades.All(x => x == false))
                tooltips.Add(new TooltipLine(Mod, "No Upgrades Added", "Add upgrades to be able to enable them!"));

            tooltips.Add(new TooltipLine(Mod, "UI Toggle Menu",
                "[c/FFCC00:Enable modifiers on the buttons above your hotbar!]"));
            
            for (int i = 0; i < (int) WrenchUpgrade.UpgradesCount; i++)
                if (unlockedUpgrades[i])
                    tooltips.Add(new TooltipLine(Mod, WrenchItemUpgrade.upgradeNames[i], tooltipText[i]));
        }
        
        public void SetUpgrade(WrenchUpgrade upgrade, bool state)
        {
            if ((int)upgrade < upgrades.Count && (int)upgrade >= 0 && unlockedUpgrades[(int)upgrade])
                upgrades[(int)upgrade] = state;
        }
        
        public bool GetUpgrade(WrenchUpgrade upgrade)
        {
            if ((int)upgrade < upgrades.Count && (int)upgrade >= 0) return false;
        
            return upgrades[(int)upgrade];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI != Main.myPlayer) return;
            Main.LocalPlayer.GetModPlayer<BEPlayer>().buildingWrenchEquipped = true;
            UIUIState.Instance.wrenchUpgrades.UpdateUpgrades(player, ref upgrades, ref unlockedUpgrades);
            UIUIState.Instance.wrenchUpgrades.Show();
        }
        
        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            // string text = "upgrades " + string.Join(", ", upgrades);
            // text += "\nunlocked " + string.Join(", ", unlockedUpgrades);
            // Main.NewText(text);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["upgrades"] = upgrades;
            tag["unlockedUpgrades"] = unlockedUpgrades;
        }

        public override void LoadData(TagCompound tag)
        {
            upgrades = tag.Get<List<bool>>("upgrades");
            unlockedUpgrades = tag.Get<List<bool>>("unlockedUpgrades");
        }
        
        public override void NetSend(BinaryWriter writer)
        {
            for (int i = 0; i < (int)WrenchUpgrade.UpgradesCount; i++)
            {
                writer.Write(upgrades[i]);
                writer.Write(unlockedUpgrades[i]);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            for (int i = 0; i < (int)WrenchUpgrade.UpgradesCount; i++)
            {
                upgrades[i] = reader.ReadBoolean();
                unlockedUpgrades[i] = reader.ReadBoolean();
            }
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
            
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
            
            for (int i = 0; i < (int)WrenchUpgrade.UpgradesCount; i++)
            {
                CreateRecipe()
                    .AddIngredient(this)
                    .AddIngredient(Mod, WrenchItemUpgrade.upgradeNames[i])
                    .AddTile(TileID.TinkerersWorkbench)
                    .AddConsumeItemCallback((Recipe recipe, int type, ref int amount) =>
                    {
                        //Previously the OnCraft hook?
                        
                        //TODO: When reloading the mod, wrench upgrades List is empty (as if it hasn't read LoadData yet)
                        BuildingWrench wrench = (BuildingWrench) recipe.requiredItem[0].ModItem;
                        string text = "upgrades " + string.Join(", ", wrench.upgrades);
                        text += "\nunlocked " + string.Join(", ", wrench.unlockedUpgrades);
                        Main.NewText(text);
                        
                        
                        int upgradeIndex = ((WrenchItemUpgrade) recipe.requiredItem[1].ModItem).GetUpgradeNumber();
                        // SetUpgrade((WrenchUpgrade) upgradeIndex, true); //Why set it immediately to true?
                        unlockedUpgrades[upgradeIndex] = true;
                    })
                    .Register();
                
                //TODO: Make condition to prevent wasting an already applied upgrade..
                //Create Localization/en-US.hjson
                //Mods: {    BuilderEssentials: {        RecipeConditions: {            Upgrade: "Must not have added the upgrade before"        }    }}
                //.AddCondition(NetworkText.FromKey("RecipeConditions.Upgrade"), condition => !unlockedUpgrades[i])
            }

            Recipe r;
            
            //Fast Placement
            r = CreateRecipe()
                .AddIngredient(ItemID.BrickLayer)
                .AddIngredient(ItemID.PortableCementMixer)
                .AddTile(TileID.Anvils);
            r.ReplaceResult(Mod, WrenchItemUpgrade.upgradeNames[0]);
            r.Register();
            
            //Infinite Player Range
            r = CreateRecipe()
                .AddIngredient(ItemID.ExtendoGrip)
                .AddIngredient(ItemID.Toolbelt)
                .AddIngredient(ItemID.SoulofMight, 15)
                .AddTile(TileID.MythrilAnvil);
            r.ReplaceResult(Mod, WrenchItemUpgrade.upgradeNames[1]);
            r.Register();
            
            //Placement Anywhere
            r = CreateRecipe()
                .AddIngredient(ItemID.Pwnhammer)
                .AddIngredient(ItemID.BuilderPotion, 5)
                .AddTile(TileID.Anvils);
            r.ReplaceResult(Mod, WrenchItemUpgrade.upgradeNames[2]);
            r.Register();

            //Infinite Placement
            r = CreateRecipe()
                .AddIngredient(ItemID.TempleKey, 3)
                .AddIngredient(ItemID.LihzahrdPowerCell, 5)
                .AddTile(TileID.AdamantiteForge);
            r.ReplaceResult(Mod, WrenchItemUpgrade.upgradeNames[3]);
            r.Register();

            //Infinite Pickup Range
            r = CreateRecipe()
                .AddIngredient(ItemID.DemoniteOre, 20)
                .AddTile(TileID.Anvils);
            r.ReplaceResult(Mod, WrenchItemUpgrade.upgradeNames[4]);
            r.Register();
            
            r = CreateRecipe()
                .AddIngredient(ItemID.CrimtaneOre, 20)
                .AddTile(TileID.Anvils);
            r.ReplaceResult(Mod, WrenchItemUpgrade.upgradeNames[4]);
            r.Register();
        }
    }
}