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

namespace BuilderEssentials
{
    public class BuilderPlayer : ModPlayer
    {
        //Player
        public bool infiniteRange;
        public Item previousHeldItem;
        public Vector2 pointedTilePos;
        public Tile pointedTile;


        //Building Mode
        public List<Item> NormalAccessories;
        public List<Item> BuildingAccessories;

        public List<Item> NormalVanityAccessories;
        public List<Item> BuildingVanityAccessories;

        public List<Item> NormalArmor;
        public List<Item> BuildingArmor;

        public List<Item> NormalVanityArmor;
        public List<Item> BuildingVanityArmor;

        public List<Item> NormalMiscEquips;
        public List<Item> BuildingMiscEquips;

        public List<Item> NormalDyes;
        public List<Item> BuildingDyes;


        //Creative Wheel Stuff
        public List<int> creativeWheelSelectedIndex;
        public bool isCreativeWrenchEquiped;

        public override void Initialize()
        {
            infiniteRange = false;
            previousHeldItem = new Item();
            pointedTilePos = new Vector2();
            pointedTile = new Tile();

            NormalAccessories = new List<Item>(7);
            BuildingAccessories = new List<Item>(7);

            NormalVanityAccessories = new List<Item>(7);
            BuildingVanityAccessories = new List<Item>(7);

            NormalArmor = new List<Item>(3);
            BuildingArmor = new List<Item>(3);

            NormalVanityArmor = new List<Item>(3);
            BuildingVanityArmor = new List<Item>(3);

            NormalMiscEquips = new List<Item>(5);
            BuildingMiscEquips = new List<Item>(5);

            NormalDyes = new List<Item>(15);
            BuildingDyes = new List<Item>(15);


            //Creative Wheel Stuff
            creativeWheelSelectedIndex = new List<int>();
            isCreativeWrenchEquiped = false;

            EnsureSaveCompatibility();
            BuildingMode.UpdateConfigVariables();
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

                if (creativeWheelSelectedIndex.Contains(CreativeWheelItem.InfinityUpgrade.ToInt())
                && !player.HasBuff(BuffType<Buffs.InfinitePlacementBuff>()))
                    creativeWheelSelectedIndex.Remove(CreativeWheelItem.InfinityUpgrade.ToInt());

                RemoveUIPanels();
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.ToggleBuildingMode.JustPressed)
                BuildingMode.ToggleBuildingMode();

            if (BuilderEssentials.IncreaseFillToolSize.JustPressed && FillWand.fillSelectionSize < 6)
                ++FillWand.fillSelectionSize;

            if (BuilderEssentials.DecreaseFillToolSize.JustPressed && FillWand.fillSelectionSize > 1)
                --FillWand.fillSelectionSize;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "NormalAccessories", NormalAccessories },
                { "BuildingAccessories", BuildingAccessories },

                { "NormalVanityAccessories", NormalVanityAccessories },
                { "BuildingVanityAccessories", BuildingVanityAccessories },

                { "NormalArmor", NormalArmor },
                { "BuildingArmor", BuildingArmor },

                { "NormalVanityArmor", NormalVanityArmor },
                { "BuildingVanityArmor", BuildingVanityArmor },

                { "NormalMiscEquips", NormalMiscEquips },
                { "BuildingMiscEquips", BuildingMiscEquips },

                { "NormalDyes", NormalDyes },
                { "BuildingDyes", BuildingDyes },

                { "creativeWheelSelectedIndex", creativeWheelSelectedIndex},
            };
        }

        public override void Load(TagCompound tag)
        {
            //Building Mode
            if (tag.ContainsKey("NormalAccessories"))
                NormalAccessories = tag.Get<List<Item>>("NormalAccessories");

            if (tag.ContainsKey("BuildingAccessories"))
                BuildingAccessories = tag.Get<List<Item>>("BuildingAccessories");

            if (tag.ContainsKey("NormalVanityAccessories"))
                NormalVanityAccessories = tag.Get<List<Item>>("NormalVanityAccessories");

            if (tag.ContainsKey("BuildingVanityAccessories"))
                BuildingVanityAccessories = tag.Get<List<Item>>("BuildingVanityAccessories");

            if (tag.ContainsKey("NormalArmor"))
                NormalArmor = tag.Get<List<Item>>("NormalArmor");

            if (tag.ContainsKey("BuildingArmor"))
                BuildingArmor = tag.Get<List<Item>>("BuildingArmor");

            if (tag.ContainsKey("NormalVanityArmor"))
                NormalVanityArmor = tag.Get<List<Item>>("NormalVanityArmor");

            if (tag.ContainsKey("BuildingVanityArmor"))
                BuildingVanityArmor = tag.Get<List<Item>>("BuildingVanityArmor");

            if (tag.ContainsKey("NormalMiscEquips"))
                NormalMiscEquips = tag.Get<List<Item>>("NormalMiscEquips");

            if (tag.ContainsKey("BuildingMiscEquips"))
                BuildingMiscEquips = tag.Get<List<Item>>("BuildingMiscEquips");

            if (tag.ContainsKey("NormalDyes"))
                NormalDyes = tag.Get<List<Item>>("NormalDyes");

            if (tag.ContainsKey("BuildingDyes"))
                BuildingDyes = tag.Get<List<Item>>("BuildingDyes");

            //Creative Wheel
            if (tag.ContainsKey("creativeWheelSelectedIndex"))
                creativeWheelSelectedIndex = tag.Get<List<int>>("creativeWheelSelectedIndex");

            //Calling this again here after Init to prevent older saves to stay with "null" saved values
            EnsureSaveCompatibility();
        }

        public void EnsureSaveCompatibility()
        {
            FixOldSaveData(ref NormalAccessories);
            FixOldSaveData(ref BuildingAccessories);
            FixOldSaveData(ref NormalVanityAccessories);
            FixOldSaveData(ref BuildingVanityAccessories);
            FixOldSaveData(ref NormalArmor);
            FixOldSaveData(ref BuildingArmor);
            FixOldSaveData(ref NormalVanityArmor);
            FixOldSaveData(ref BuildingVanityArmor);
            FixOldSaveData(ref NormalMiscEquips);
            FixOldSaveData(ref BuildingMiscEquips);
            FixOldSaveData(ref NormalDyes);
            FixOldSaveData(ref BuildingDyes);
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
