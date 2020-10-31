using System.Threading;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.Utilities;

namespace BuilderEssentials.Items
{
    public class MultiWand : ModItem
    {
        public int baseRange = 8;
        public int toolRange;
        public bool operationAllowed;
        
        public override string Texture => "BuilderEssentials/Textures/Items/MultiWand";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Contains all building wands!\nRight Click to open selection menu");

        public override void SetDefaults()
        {
            item.height = 44;
            item.width = 44;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            item.noMelee = false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(2, -9);

        public override void HoldItem(Player player)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (Main.netMode != NetmodeID.Server && mp.ValidCursorPos)
            {
                toolRange = baseRange; // or bigger if inf range?

                if (HelperMethods.ToolHasRange(toolRange))
                {
                    operationAllowed = true;
                    player.showItemIcon = true;
                    player.showItemIcon2 = item.type;
                }
                else
                {
                    operationAllowed = false;
                    player.showItemIcon = false;
                }
            }
        }
    }
}