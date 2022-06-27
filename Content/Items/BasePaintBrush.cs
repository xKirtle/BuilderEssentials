using System;
using System.Reflection;
using BuilderEssentials.Common;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(false)]
public abstract class BasePaintBrush : BaseItemToggleableUI
{
    public override ToggleableUiType ToggleableUiType => ToggleableUiType.PaintBrush;
    
    public override void SetDefaults() {
        base.SetDefaults();
        
        Item.height = Item.width = 44;
        Item.useTime = Item.useAnimation = 10;
        Item.useTurn = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.sellPrice(silver: 80);
        Item.rare = ItemRarityID.Red;
        Item.autoReuse = true;
    }
    
    public override Vector2? HoldoutOffset() => new Vector2(5, -8);

    public override void UpdateInventory(Player player) {
        if (player.whoAmI != Main.myPlayer) return;

        if (Main.LocalPlayer.GetModPlayer<BEPlayer>().InfinitePaint) return;
        
        var panel = ToggleableItemsUIState.GetUIPanel<PaintBrushPanel>();
        if (panel.colorIndex != -1)
            Item.tileWand = GetFirstSelectedPaintItem(player, (byte) (panel.colorIndex + 1)).type;
    }

    public override bool CanUseItem(Player player) {
        if (player.whoAmI != Main.myPlayer || !ItemHasRange()) return true;

        var panel = ToggleableItemsUIState.GetUIPanel<PaintBrushPanel>();
        byte selectedColor = (byte) (panel.colorIndex + 1);
        int toolIndex = panel.toolIndex;

        if (selectedColor == 0) return true;

        if (toolIndex != 2) {
            // MethodInfo tryPaintMethod = player.GetType().GetMethod("TryPainting",
            //     BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);
            // tryPaintMethod.Invoke(player, new object[] {Player.tileTargetX, Player.tileTargetY, toolIndex == 1, true});

            Tile tile = Framing.GetTileSafely(BEPlayer.PointedWorldCoords);
            if ((toolIndex == 0 && tile.TileColor == selectedColor) || 
                (toolIndex == 1 && tile.WallColor == selectedColor) ||
                (toolIndex == 0 && (!tile.HasTile || tile.TileType < 0)) ||
                (toolIndex == 1 && tile.WallType <= 0)) return true;

            PaintTileOrWall(selectedColor, toolIndex, BEPlayer.PointedTileCoords.ToPoint());

            MirrorPlacementDetours.MirrorPlacementAction(mirroredCoords =>
                PaintTileOrWall(selectedColor, toolIndex, mirroredCoords.ToPoint()));
        }
        else {
            ScrapPaint(BEPlayer.PointedTileCoords.ToPoint());
            
            MirrorPlacementDetours.MirrorPlacementAction(mirroredCoords => 
                ScrapPaint(mirroredCoords.ToPoint()));
        }

        return true;
    }

    public override bool? UseItem(Player player) {
        if (player.whoAmI == Main.myPlayer && IsPanelVisible())
            TogglePanel();

        return base.UseItem(player);
    }

    public static Item GetFirstSelectedPaintItem(Player player, byte color) {
        for (int i = 54; i < 58; i++) {
            if (player.inventory[i].stack > 0 && player.inventory[i].paint == color) {
                return player.inventory[i];
            }
        }
        
        for (int i = 0; i < 54; i++) {
            if (player.inventory[i].stack > 0 && player.inventory[i].paint == color) {
                return player.inventory[i];
            }
        }

        //Should be impossible since this is called after color selection was done
        //in the UI which does not allow to select a color we don't have
        return new Item();
    }
    public static int PaintItemTypeToColorIndex(int paintType) {
        //The outputed indexes are not the paint color byte values. For those just increment one.
        if (paintType >= 1073 && paintType <= 1099)
            return paintType - 1073;
        else if (paintType >= 1966 && paintType <= 1968)
            return paintType - 1939;
        else if (paintType >= 4668)
            return paintType - 4638;

        return -1; //it will never reach here
    }
    
    public static int ColorByteToPaintItemType(byte color) {
        if (color >= 1 && color <= 27)
            return (color - 1) + 1073;
        else if (color >= 28 && color <= 30)
            return (color - 1) + 1939;
        else if (color == 31)
            return (color - 1) + 4638;

        return -1; //it will never reach here
    }
    
    public static void PaintTileOrWall(byte color, int selectedTool, Point coords) {
        Tile tile = Framing.GetTileSafely(coords.X, coords.Y);
        bool canPaint = true;
        
        if (!Main.LocalPlayer.GetModPlayer<BEPlayer>().InfinitePaint) {
            Item paintItem = GetFirstSelectedPaintItem(Main.LocalPlayer, color);
            if (ItemLoader.ConsumeItem(paintItem, Main.LocalPlayer)) {
                if (paintItem.stack >= 1) {
                    paintItem.stack--;
                    if (paintItem.stack <= 0)
                        paintItem.SetDefaults();
                }
                else canPaint = false;
            }
        }
        
        if (!canPaint || color < 1 || color > 32 || selectedTool < 0 || selectedTool > 1) return;
        bool needSync = false;

        if (selectedTool == 0 && tile.HasTile && tile.TileType >= 0 && tile.TileColor != color) {
            WorldGen.paintEffect(coords.X, coords.Y, color, tile.TileColor);
            tile.TileColor = color;
            needSync = true;
        }
        else if (selectedTool == 1 && tile.WallType > 0 && tile.WallColor != color) {
            WorldGen.paintEffect(coords.X, coords.Y, color, tile.WallColor);
            tile.WallColor = color;
            needSync = true;
        }

        if (needSync && Main.netMode != NetmodeID.SinglePlayer)
            NetMessage.SendTileSquare(-1, coords.X, coords.Y, 1);
    }

    public static void ScrapPaint(Point coords) {
        Tile tile = Framing.GetTileSafely(coords.X, coords.Y);
        bool needSync = false;

        if (tile.TileColor != 0) {
            WorldGen.paintEffect(coords.X, coords.Y, 0, tile.TileColor);
            tile.TileColor = 0;
            needSync = true;
        }
        else if (tile.WallColor != 0) {
            WorldGen.paintEffect(coords.X, coords.Y, 0, tile.WallColor);
            tile.WallColor = 0;
            needSync = true;
        }

        if (needSync && Main.netMode != NetmodeID.SinglePlayer) {
            NetMessage.SendData(MessageID.PaintTile, number: coords.X, number2: coords.Y);
            NetMessage.SendData(MessageID.PaintWall, number: coords.X, number2: coords.Y);
        }
    }
}