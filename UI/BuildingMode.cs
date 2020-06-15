using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace BuilderEssentials.UI
{
    public static class BuildingMode
    {
        public static void BuildingModeAccessoriesToggle()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            CleanAcessoriesList();
            SaveCurrentAccessories();
            modPlayer.IsNormalAccessories = !modPlayer.IsNormalAccessories;
            LoadAccessories();
            UpdateButtonImage();
        }

        public static void CleanAcessoriesList()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            //TODO: Mod config where the player can choose if he wishes to have "2nd" vanity slots too
            if (modPlayer.IsNormalAccessories)
            {
                modPlayer.NormalAccessories.Clear();
                modPlayer.NormalVanityAccessories.Clear();
                modPlayer.NormalVanityClothes.Clear();
            }
            else
            {
                modPlayer.BuildingAccessories.Clear();
                modPlayer.BuildingVanityAccessories.Clear();
                modPlayer.BuildingVanityClothes.Clear();
            }
        }

        public static void SaveCurrentAccessories()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            //Normal and Vanity Accessories
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                Item accessory = modPlayer.player.armor[i];
                Item vanityAccessory = modPlayer.player.armor[i + 10];
                if (modPlayer.IsNormalAccessories)
                {
                    modPlayer.NormalAccessories.Add(accessory);
                    modPlayer.NormalVanityAccessories.Add(vanityAccessory);
                }
                else
                {
                    modPlayer.BuildingAccessories.Add(accessory);
                    modPlayer.BuildingVanityAccessories.Add(vanityAccessory);
                }
            }

            //Vanity Sets (&& armor set, in the future?)
            for (int i = 10; i < 13; i++)
            {
                Item vanityCloth = modPlayer.player.armor[i];
                if (modPlayer.IsNormalAccessories)
                    modPlayer.NormalVanityClothes.Add(vanityCloth);
                else
                    modPlayer.BuildingVanityClothes.Add(vanityCloth);
            }
        }

        public static void LoadAccessories()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            if (modPlayer.IsNormalAccessories)
            {
                for (int i = 0; i < modPlayer.NormalAccessories.Count; i++)
                {
                    modPlayer.player.armor[i + 3] = modPlayer.NormalAccessories[i];
                    modPlayer.player.armor[i + 13] = modPlayer.NormalVanityAccessories[i];
                }

                for (int i = 0; i < 3; i++)
                {
                    modPlayer.player.armor[i + 10] = modPlayer.NormalVanityClothes[i];
                }
            }
            else
            {
                if (modPlayer.BuildingAccessories.Count > 0)
                {
                    for (int i = 0; i < modPlayer.BuildingAccessories.Count; i++)
                    {
                        modPlayer.player.armor[i + 3] = modPlayer.BuildingAccessories[i];
                        modPlayer.player.armor[i + 13] = modPlayer.BuildingVanityAccessories[i];
                    }
                }

                if (modPlayer.BuildingVanityAccessories.Count > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        modPlayer.player.armor[i + 10] = modPlayer.BuildingVanityClothes[i];
                    }
                }
            }
        }
        public static void UpdateButtonImage()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (modPlayer.IsNormalAccessories)
                BasePanel.buildingModeButton.SetImage(BuilderEssentials.BuildingModeOff);
            else
                BasePanel.buildingModeButton.SetImage(BuilderEssentials.BuildingModeOn);
        }
    }
}
