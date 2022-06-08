using System;
using System.Threading;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class AutoHammer : BaseItemToggleableUI
{
    public override UIStateType UiStateType => UIStateType.AutoHammer;

    public override string Texture => "BuilderEssentials/Assets/Items/AutoHammer";

    protected override bool CloneNewInstances => true;

    public override void SetStaticDefaults() {
        DisplayName.SetDefault("Default Hammer");
        Tooltip.SetDefault("Better than a regular hammer!\n" +
                           "Right Click to open selection menu");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
        Item.width = Item.height = 44;
        Item.useTime = Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(silver: 80);
        Item.rare = ItemRarityID.Red;
        Item.damage = 26;
        Item.DamageType = DamageClass.Melee;
        Item.hammer = 80;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }
    
    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.Pwnhammer)
            .AddRecipeGroup("BuilderEssentials:Wood", 200)
            .Register();
    }

    public override bool CanUseItem(Player player) {
        if (player.whoAmI != Main.myPlayer) 
            return base.CanUseItem(player);

        var panel = AutoHammerState.Instance.menuPanel;
        //TODO: Check if tool has range?
        if (panel.selectedIndex != -1) {
            ChangeSlope(panel.blockType);
            return false;
        }

        return true;
    }

    internal static void ChangeSlope(BlockType blockType) {
        Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        
        if (Main.tileSolid[tile.TileType] && tile.TileType >= 0) {
            //If there are no changes, return so it doesn't play feedback sound and tries to sync unnecessary info

            int oldSlope = (int)tile.Slope;
            bool oldHalfBlock = tile.IsHalfBlock;

            //TODO: Don't use block types for this?? Setting a blockType != setting slope + halfblock
            tile.Slope = (int)blockType == 0 || (int)blockType > 1 ? (SlopeType)(blockType + 1) : SlopeType.Solid;
            tile.IsHalfBlock = (int) blockType == 1;

            if (oldSlope == (int) tile.Slope && oldHalfBlock == tile.IsHalfBlock) return;
            
            SoundEngine.PlaySound(SoundID.Dig);
            WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, false);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
        }
    }
}