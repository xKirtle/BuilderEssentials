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
        public List<Item> NormalAccessories;
        public List<Item> BuildingAccessories;
        public List<Item> NormalVanityAccessories;
        public List<Item> BuildingVanityAccessories;
        public List<Item> NormalVanityClothes;
        public List<Item> BuildingVanityClothes;
        public bool IsNormalAccessories;
        public bool InfinitePlacement;


        //Creative Wheel Stuff
        public bool colorPickerSelected;
        public bool InfinitePlacementSelected;
        public bool autoHammerSelected;
        public int autoHammerSelectedIndex;


        //Paint Stuff
        public int paintingColorSelectedIndex;
        public int paintingToolSelected;


        public override void Initialize()
        {
            NormalAccessories = new List<Item>(7);
            BuildingAccessories = new List<Item>(7);
            NormalVanityAccessories = new List<Item>(7);
            BuildingVanityAccessories = new List<Item>(7);
            NormalVanityClothes = new List<Item>(3);
            BuildingVanityClothes = new List<Item>(3);
            IsNormalAccessories = true;
            InfinitePlacement = false;


            //Creative Wheel Stuff
            colorPickerSelected = false;
            InfinitePlacementSelected = false;
            autoHammerSelected = false;
            autoHammerSelectedIndex = 5; //full tile


            //Paint
            paintingColorSelectedIndex = 30; //No color
            paintingToolSelected = 0;
        }

        public override void ResetEffects()
        {
            //TODO: Fix Creative Wrench features still working after you remove the accessory
            InfinitePlacement = false;
            Player.tileRangeX = 5;
            Player.tileRangeY = 4;
            player.showItemIcon = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.ToggleBuildingMode.JustPressed)
                BuildingMode.BuildingModeAccessoriesToggle();
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "IsNormalAccessories", IsNormalAccessories },
                { "NormalAccessories", NormalAccessories },
                { "BuildingAccessories", BuildingAccessories },
                {"NormalVanityAccessories", NormalVanityAccessories },
                {"BuildingVanityAccessories", BuildingVanityAccessories },
                {"NormalVanityClothes", NormalVanityClothes },
                {"BuildingVanityClothes", BuildingVanityClothes },
                {"colorPickerSelected", colorPickerSelected },
                {"InfinitePlacementSelected", InfinitePlacementSelected },
                {"autoHammerSelected", autoHammerSelected },
                {"autoHammerSelectedIndex", autoHammerSelectedIndex },
                {"paintingColorSelectedIndex", paintingColorSelectedIndex},
                {"paintingToolSelected", paintingToolSelected}
            };
        }

        public override void Load(TagCompound tag)
        {
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

            if (tag.ContainsKey("NormalVanityClothes"))
                NormalVanityClothes = tag.Get<List<Item>>("NormalVanityClothes");

            if (tag.ContainsKey("BuildingVanityClothes"))
                BuildingVanityClothes = tag.Get<List<Item>>("BuildingVanityClothes");

            //Creative Wheel
            if (tag.ContainsKey("colorPickerSelected"))
                colorPickerSelected = tag.GetBool("colorPickerSelected");

            if (tag.ContainsKey("InfinitePlacementSelected"))
                InfinitePlacementSelected = tag.GetBool("InfinitePlacementSelected");

            if (tag.ContainsKey("autoHammerSelected"))
                autoHammerSelected = tag.GetBool("autoHammerSelected");

            if (tag.ContainsKey("autoHammerSelectedIndex"))
                autoHammerSelectedIndex = tag.GetInt("autoHammerSelectedIndex");

            //Paint
            if (tag.ContainsKey("paintingColorSelectedIndex"))
                paintingColorSelectedIndex = tag.GetInt("paintingColorSelectedIndex");

            if (tag.ContainsKey("paintingToolSelected"))
                paintingToolSelected = tag.GetInt("paintingToolSelected");
        }

        public override void OnEnterWorld(Player player)
        {
            if (!IsNormalAccessories)
                BasePanel.buildingModeButton.SetImage(BuilderEssentials.BuildingModeOn);
            else
                BasePanel.buildingModeButton.SetImage(BuilderEssentials.BuildingModeOff);
        }
    }
}
