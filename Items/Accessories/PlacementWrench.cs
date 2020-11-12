using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    public class PlacementWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/PlacementWrench";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Allows infinite range");

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 48;
            item.height = 48;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BEPlayer>().InfiniteRange = true;
            
            //TODO: Implement DrawBuilderAccToggles myself from vanilla so I can add more options to it
            //Main DrawBuilderAccToggles
            //On.Terraria.Main.DrawBuilderAccToggles

            // Texture2D builderIcons = Main.builderAccTexture; //ModContent.GetTexture("Terraria/UI/BuilderIcons");
            // builderIcons.Frame(10, 2, 16, 16); //returns a rectangle used in the sourceRect in sb drawing
            
            // string text = "";
            // foreach (var stat in player.builderAccStatus)
            // {
            //     text += " " + stat;
            // }
            //
            // Main.NewText(text);
        }
        
        public override void AddRecipes()
        {
            //TODO: Make this available more early game
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.ArchitectGizmoPack);
            modRecipe.AddIngredient(ItemID.LaserRuler);
            modRecipe.AddIngredient(ItemID.Toolbox);
            modRecipe.AddIngredient(ItemID.Toolbelt);
            modRecipe.AddTile(TileID.AdamantiteForge);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}