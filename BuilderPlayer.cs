using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuilderEssentials
{
    public class BuilderPlayer : ModPlayer
    {
        //Accessory System
        public List<Item> NormalAccessories;
        public List<Item> BuildingAccessories;
        public List<Item> NormalVanityAccessories;
        public List<Item> BuildingVanityAccessories;
        public List<Item> NormalVanityClothes;
        public List<Item> BuildingVanityClothes;
        public bool IsNormalAccessories;


        //Creative Wheel Stuff
        public List<int> creativeWheelSelectedIndex;
        public int autoHammerSelectedIndex;
        public enum CreativeWheelItem : int
        {
            ItemPicker,
            InfinitePlacement,
            AutoHammer,
            PlacementAnywhere
        }


        //Paint Stuff
        public int paintingColorSelectedIndex;
        public int paintingToolSelected;
        public bool holdingPaintingTool;
        public override void Initialize()
        {
            NormalAccessories = new List<Item>(7);
            BuildingAccessories = new List<Item>(7);
            NormalVanityAccessories = new List<Item>(7);
            BuildingVanityAccessories = new List<Item>(7);
            NormalVanityClothes = new List<Item>(3);
            BuildingVanityClothes = new List<Item>(3);
            IsNormalAccessories = true;


            //Creative Wheel Stuff
            creativeWheelSelectedIndex = new List<int>();
            autoHammerSelectedIndex = 5; //full tile


            //Paint
            paintingColorSelectedIndex = 30; //No color
            paintingToolSelected = 0;
        }

        public override void ResetEffects()
        {
            Player.tileRangeX = 5;
            Player.tileRangeY = 4;
            player.showItemIcon = false;
            holdingPaintingTool = false;

            //Closing the creative wheel if the held item isn't air
            if (player.whoAmI == Main.myPlayer)
                if (!player.inventory[player.selectedItem].IsAir)
                {
                    if (BasePanel.creativeWheelPanel != null)
                    {
                        BasePanel.creativeWheelPanel.Remove();
                        BasePanel.creativeWheelUIOpen = false;
                        BasePanel.isCreativeWheelVisible = false;
                    }
                }
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
                {"creativeWheelSelectedIndex", creativeWheelSelectedIndex},
                {"autoHammerSelectedIndex", autoHammerSelectedIndex },
                {"paintingColorSelectedIndex", paintingColorSelectedIndex},
                {"paintingToolSelected", paintingToolSelected}
            };
        }

        public override void Load(TagCompound tag)
        {
            //Accessory System
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
            if (tag.ContainsKey("creativeWheelSelectedIndex"))
                creativeWheelSelectedIndex = tag.Get<List<int>>("creativeWheelSelectedIndex");

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
