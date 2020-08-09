using BuilderEssentials.Utilities;
using System.Linq;
using System.Security.Policy;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class InfinitePaintBucket : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/InfinitePaintBucket";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Necessary to use the Super Painting Tool");
        }

        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            //item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DeepRedPaint, 999);
            recipe.AddIngredient(ItemID.DeepGreenPaint, 999);
            recipe.AddIngredient(ItemID.DeepBluePaint, 999);
            recipe.AddIngredient(ItemID.NegativePaint, 999);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        bool paintSelected;
        public override void UpdateInventory(Player player)
        {
            if (paintSelected)
            {
                //Works fine until inventory is full, not very good
                int index = Tools.FindNextEmptyInventorySlot();
                if (index == -1) index = player.inventory.Length;
                player.inventory[index].stack = 999;
                player.inventory[index].paint = 1;

                Main.NewText(index);
            }
        }


        //TODO: MAKE COLOR SELECTION UI (ALSO REPLACE ALTFUNCTIONUSE WITH UPDATEINVENTORY) AND FINISH THE GLOBAL ITEM USEITEM REPLACEMENT
        public override bool AltFunctionUse(Player player)
        {
            Main.NewText("asd");
            paintSelected = true;

            return true;
        }
    }

    public class asd : GlobalItem
    {
        public override bool UseItem(Item item, Player player)
        {
            if (item.type == ItemID.SpectrePaintbrush || item.type == ItemID.SpectrePaintRoller || item.type == ItemID.SpectrePaintScraper)
            {
                foreach (Item invItem in player.inventory)
                {
                    //if infinitepaintbucket is in inventory and has a color selected, manually paint pointed coords with selected byte color
                    //if paint is in inventory, manually paint pointed coords with the first paint (lowest inv index)
                }
            }

            return base.UseItem(item, player);
        }
    }
}
