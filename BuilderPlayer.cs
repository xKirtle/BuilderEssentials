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
        public bool IsNormalAccessories;

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
        public int autoHammerSelectedIndex;
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

        void FillListWithEmptyItems(ref List<Item> list, int size)
        {
            for (int i = 0; i < size; i++)
                list.Add(new Item());
        }

        public override void Initialize()
        {
            infiniteRange = false;

            IsNormalAccessories = true;

            NormalAccessories = new List<Item>(7);
            FillListWithEmptyItems(ref NormalAccessories, 7);
            BuildingAccessories = new List<Item>(7);
            FillListWithEmptyItems(ref BuildingAccessories, 7);

            NormalVanityAccessories = new List<Item>(7);
            FillListWithEmptyItems(ref NormalVanityAccessories, 7);
            BuildingVanityAccessories = new List<Item>(7);
            FillListWithEmptyItems(ref BuildingVanityAccessories, 7);

            NormalArmor = new List<Item>(3);
            FillListWithEmptyItems(ref NormalArmor, 3);
            BuildingArmor = new List<Item>(3);
            FillListWithEmptyItems(ref BuildingArmor, 3);

            NormalVanityArmor = new List<Item>(3);
            FillListWithEmptyItems(ref NormalVanityArmor, 3);
            BuildingVanityArmor = new List<Item>(3);
            FillListWithEmptyItems(ref BuildingVanityArmor, 3);

            NormalMiscEquips = new List<Item>(5);
            FillListWithEmptyItems(ref NormalMiscEquips, 5);
            BuildingMiscEquips = new List<Item>(5);
            FillListWithEmptyItems(ref BuildingMiscEquips, 5);

            NormalDyes = new List<Item>(15);
            FillListWithEmptyItems(ref NormalDyes, 15);
            BuildingDyes = new List<Item>(15);
            FillListWithEmptyItems(ref BuildingDyes, 15);


            //Creative Wheel Stuff
            creativeWheelSelectedIndex = new List<int>();
            autoHammerSelectedIndex = 5; //full tile


            //Mirror Wand
            mirrorWandEffects = false;
        }

        public override void ResetEffects()
        {
            //Closing the creative wheel if the held item isn't air
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
                Tools.BuildingModeToggle();
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "IsNormalAccessories", IsNormalAccessories },

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
                { "autoHammerSelectedIndex", autoHammerSelectedIndex },
            };
        }

        public override void Load(TagCompound tag)
        {
            //Building Mode
            if (tag.ContainsKey("IsNormalAccessories"))
                IsNormalAccessories = tag.GetBool("IsNormalAccessories");

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

            if (tag.ContainsKey("autoHammerSelectedIndex"))
                autoHammerSelectedIndex = tag.GetInt("autoHammerSelectedIndex");
        }

        public override void OnEnterWorld(Player player)
        {
            //Loads (or populates) all lists on enter world
            Tools.BuildingModeToggle();
            Tools.BuildingModeToggle();
        }
    }
}
