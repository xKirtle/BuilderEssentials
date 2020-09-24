using BuilderEssentials.Items;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using static BuilderEssentials.Utilities.Tools;
using BuilderEssentials.UI.ItemsUI.Wheels;
using BuilderEssentials.UI;
using BuilderEssentials.UI.ItemsUI;

namespace BuilderEssentials
{
    public class BuilderPlayer : ModPlayer
    {
        //Player
        public bool infiniteRange;
        public Item previousHeldItem;
        public Vector2 pointedTilePos;
        public Tile pointedTile;

        //Creative Wheel Stuff
        public List<int> creativeWheelSelectedIndex;
        public bool isCreativeWrenchEquiped;

        public override void Initialize()
        {
            infiniteRange = false;
            previousHeldItem = new Item();
            pointedTilePos = new Vector2();
            pointedTile = new Tile();

            //Creative Wheel Stuff
            creativeWheelSelectedIndex = new List<int>();
            isCreativeWrenchEquiped = false;
        }

        public override void ResetEffects()
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Player.tileRangeX = 5;
                Player.tileRangeY = 4;
                Player.defaultItemGrabRange = 38;
                player.showItemIcon = false;
                isCreativeWrenchEquiped = false;
                infiniteRange = false;
                pointedTilePos = new Vector2(Player.tileTargetX, Player.tileTargetY);
                pointedTile = Main.tile[(int)pointedTilePos.X, (int)pointedTilePos.Y];
                ShapesMenu.SDEquipped = false;
                ImprovedRulerUI.IREquipped = false;

                if (creativeWheelSelectedIndex.Contains(CreativeWheelItem.InfinityUpgrade.ToInt())
                && !player.HasBuff(BuffType<Buffs.InfinitePlacementBuff>()))
                    creativeWheelSelectedIndex.Remove(CreativeWheelItem.InfinityUpgrade.ToInt());

                RemoveUIPanels();
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.IncreaseFillToolSize.JustPressed && FillWand.fillSelectionSize < 6)
                ++FillWand.fillSelectionSize;

            if (BuilderEssentials.DecreaseFillToolSize.JustPressed && FillWand.fillSelectionSize > 1)
                --FillWand.fillSelectionSize;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "creativeWheelSelectedIndex", creativeWheelSelectedIndex},
            };
        }

        public override void Load(TagCompound tag)
        {
            //Creative Wheel
            if (tag.ContainsKey("creativeWheelSelectedIndex"))
                creativeWheelSelectedIndex = tag.Get<List<int>>("creativeWheelSelectedIndex");
        }

        private void RemoveUIPanels()
        {
            var player = Main.LocalPlayer;
            if (player.HeldItem != previousHeldItem)
            {
                previousHeldItem = player.HeldItem;
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.HeldItem.type != ItemType<AutoHammer>())
                        AutoHammerWheel.RemovePanel();

                    if (player.HeldItem.type != ItemType<SuperPaintingTool>())
                        PaintWheel.RemovePanel();

                    if (player.HeldItem.type != ItemType<MultiWand>())
                        MultiWandWheel.RemovePanel();

                    if (!player.HeldItem.IsAir)
                        CreativeWheel.RemovePanel();

                    if (!ShapesMenu.SDEquipped)
                    {
                        ShapesMenu.ArrowPanel?.Remove();
                        ShapesMenu.ArrowPanel = null;
                        ShapesMenu.SMPanel?.Remove();
                        ShapesMenu.SMPanel = null;
                    }
                }

            }
        }
    }
}
