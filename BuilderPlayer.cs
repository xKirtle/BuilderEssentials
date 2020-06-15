using BuilderEssentials.UI;
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
        

        public bool colorPickerSelected;
        public bool InfinitePlacementSelected;

        public override void Initialize()
        {
            NormalAccessories = new List<Item>(7);
            BuildingAccessories = new List<Item>(7);
            NormalVanityAccessories = new List<Item>(7);
            BuildingVanityAccessories = new List<Item>(7);
            NormalVanityClothes = new List<Item>(3);
            BuildingVanityClothes = new List<Item>(3);
            IsNormalAccessories = true;
            colorPickerSelected = false;
            InfinitePlacementSelected = false;
        }

        public override void ResetEffects()
        {
            Player.tileRangeX = 5;
            Player.tileRangeY = 4;
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
                {"InfinitePlacementSelected", InfinitePlacementSelected }
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
