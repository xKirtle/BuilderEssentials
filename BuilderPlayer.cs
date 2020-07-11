using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuilderEssentials
{
    public class BuilderPlayer : ModPlayer
    {
        //Player
        public bool infiniteRange;


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
        public enum CreativeWheelItem
        {
            ItemPicker,
            InfinitePlacement,
            AutoHammer,
            PlacementAnywhere,
            InfinitePickupRange,

            //Non important order (independent items)
            InfinityUpgrade
        }

        //Mirror Wand
        public bool mirrorWandEffects;

        public override void Initialize()
        {
            infiniteRange = false;

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


            //Mirror Wand
            mirrorWandEffects = false;
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
                infiniteRange = false;

                if (!player.HeldItem.IsAir)
                {
                    if (CreativeWheel.CreativeWheelPanel != null)
                    {
                        CreativeWheel.CreativeWheelPanel.Remove();
                        CreativeWheel.CreativeWheelUIOpen = false;
                        CreativeWheel.IsCreativeWheelVisible = false;
                    }
                }

                if (creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade)
                && !player.HasBuff(mod.BuffType("InfinitePlacementBuff")))
                    creativeWheelSelectedIndex.Remove((int)CreativeWheelItem.InfinityUpgrade);

                if (mirrorWandEffects)
                    mirrorWandEffects = false;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.ToggleBuildingMode.JustPressed)
                BuildingMode.ToggleBuildingMode();
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
            Tools.FixOldSaveData(ref NormalAccessories);
            Tools.FixOldSaveData(ref BuildingAccessories);
            Tools.FixOldSaveData(ref NormalVanityAccessories);
            Tools.FixOldSaveData(ref BuildingVanityAccessories);
            Tools.FixOldSaveData(ref NormalArmor);
            Tools.FixOldSaveData(ref BuildingArmor);
            Tools.FixOldSaveData(ref NormalVanityArmor);
            Tools.FixOldSaveData(ref BuildingVanityArmor);
            Tools.FixOldSaveData(ref NormalMiscEquips);
            Tools.FixOldSaveData(ref BuildingMiscEquips);
            Tools.FixOldSaveData(ref NormalDyes);
            Tools.FixOldSaveData(ref BuildingDyes);
        }
    }
}
