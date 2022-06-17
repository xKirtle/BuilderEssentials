using System;
using BuilderEssentials.Common;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class FillWand : BuilderEssentialsItem
{
    public static int FillSelectionSize = 3;
    
    protected override bool CloneNewInstances => true;
    public override int ItemRange => 10;

    public override void SetStaticDefaults() {
        Tooltip.SetDefault(
            "Fills or creates holes!" +
            "\nLeft Click to place" +
            "\nRigth Click to remove" +
            "\nMiddle Click to select working tile" +
            "\n[c/FFCC00:Use hotkeys to increase/decrease selection size]" +
            "\n[c/FF0000:Does not support multi tiles]"
        );
            
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }
    
    public override void SetDefaults() {
        Item.height = Item.width = 46;
        Item.useTime = Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.sellPrice(gold: 1, silver: 40);
        Item.rare = ItemRarityID.Red;
        Item.autoReuse = true;
        Item.noMelee = true;
    }
    
    public override Vector2? HoldoutOffset() => new Vector2(-2, -7);

    public override void HoldItem(Player player) {
        
        if (player.whoAmI != Main.myPlayer || Main.netMode == NetmodeID.Server) return;
        
        var panel = ShapesUIState.GetUIPanel<FillWandPanel>();

        if (Main.mouseMiddle && !player.mouseInterface) {
            Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
            var itemType = ItemPicker.PickItem(tile, false, false);
            panel.SetSelectedTileType(itemType);
        }
        
        player.cursorItemIconEnabled = ItemHasRange();
        player.cursorItemIconID = Type;
        if (!player.cursorItemIconEnabled) return;
        
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = panel.SelectedItemType;
    }

    public override bool? UseItem(Player player) {
        return true;
    }
    
    //CanUseItem -> UseItem -> Panel PlotSelection
}