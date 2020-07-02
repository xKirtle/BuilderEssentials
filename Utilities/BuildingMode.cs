using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuilderEssentials.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        //Variables updated through ModConfig
        public static bool accessories;
        public static bool vanityAccessories;
        public static bool armor;
        public static bool vanityArmor;
        public static bool miscEquips;
        public static bool dyes;

        public static void BuildingModeToggle()
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
            if (modPlayer.IsNormalAccessories)
            {
                if (accessories)
                    modPlayer.NormalAccessories.Clear();
                if (vanityAccessories)
                    modPlayer.NormalVanityAccessories.Clear();
                if (armor)
                    modPlayer.NormalArmor.Clear();
                if (vanityArmor)
                    modPlayer.NormalVanityArmor.Clear();
                if (miscEquips)
                    modPlayer.NormalMiscEquips.Clear();
                if (dyes)
                    modPlayer.NormalDyes.Clear();
            }
            else
            {
                if (accessories)
                    modPlayer.BuildingAccessories.Clear();
                if (vanityAccessories)
                    modPlayer.BuildingVanityAccessories.Clear();
                if (armor)
                    modPlayer.BuildingArmor.Clear();
                if (vanityArmor)
                    modPlayer.BuildingVanityArmor.Clear();
                if (miscEquips)
                    modPlayer.BuildingMiscEquips.Clear();
                if (dyes)
                    modPlayer.BuildingDyes.Clear();
            }
        }

        public static void SaveCurrentAccessories()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            var player = Main.LocalPlayer;
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            //Normal and Vanity Accessories
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                Item accessory = player.armor[i]; //3-9
                Item vanityAccessory = player.armor[i + 10]; //13-19
                if (modPlayer.IsNormalAccessories)
                {
                    if (accessories)
                        modPlayer.NormalAccessories.Add(accessory);
                    if (vanityAccessories)
                        modPlayer.NormalVanityAccessories.Add(vanityAccessory);
                }
                else
                {
                    if (accessories)
                        modPlayer.BuildingAccessories.Add(accessory);
                    if (vanityAccessories)
                        modPlayer.BuildingVanityAccessories.Add(vanityAccessory);
                }
            }

            //Armor and Vanity Sets
            for (int i = 0; i < 3; i++)
            {
                Item armorItem = player.armor[i]; //0-2
                Item vanityArmorItem = player.armor[i + 10]; //10-12
                if (modPlayer.IsNormalAccessories)
                {
                    if (armor)
                        modPlayer.NormalArmor.Add(armorItem);
                    if (vanityArmor)
                        modPlayer.NormalVanityArmor.Add(vanityArmorItem);
                }
                else
                {
                    if (armor)
                        modPlayer.BuildingArmor.Add(armorItem);
                    if (vanityArmor)
                        modPlayer.BuildingVanityArmor.Add(vanityArmorItem);
                }
            }

            //Misc Equips
            for (int i = 0; i < 5; i++)
            {
                if (miscEquips)
                {
                    if (modPlayer.IsNormalAccessories)
                        modPlayer.NormalMiscEquips.Add(player.miscEquips[i]);
                    else
                        modPlayer.BuildingMiscEquips.Add(player.miscEquips[i]);
                }
            }


            //Dyes
            for (int i = 0; i < 15; i++)
            {
                if (dyes)
                {
                    if (i < 10) //Armor + Accessories Dyes
                    {
                        if (modPlayer.IsNormalAccessories)
                            modPlayer.NormalDyes.Add(player.dye[i]);
                        else
                            modPlayer.BuildingDyes.Add(player.dye[i]);
                    }
                    else //Misc Equipement Dyes
                    {
                        if (modPlayer.IsNormalAccessories)
                            modPlayer.NormalDyes.Add(player.miscDyes[i - 10]);
                        else
                            modPlayer.BuildingDyes.Add(player.miscDyes[i - 10]);
                    }
                }
            }
        }

        public static void LoadAccessories()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            var player = Main.LocalPlayer;

            //Accessories
            for (int i = 3; i < 3 + modPlayer.NormalAccessories.Count; i++)
            {
                if (modPlayer.IsNormalAccessories)
                {
                    if (accessories)
                        player.armor[i] = modPlayer.NormalAccessories[i - 3];
                    if (vanityAccessories)
                        player.armor[i + 10] = modPlayer.NormalVanityAccessories[i - 3];
                }
                else
                {
                    if (accessories)
                        player.armor[i] = modPlayer.BuildingAccessories[i - 3];
                    if (vanityAccessories)
                        player.armor[i + 10] = modPlayer.BuildingVanityAccessories[i - 3];
                }
            }

            //Armor and Vanity Sets
            for (int i = 0; i < 3; i++)
            {
                if (modPlayer.IsNormalAccessories)
                {
                    if (armor)
                        player.armor[i] = modPlayer.NormalArmor[i];
                    if (vanityArmor)
                        player.armor[i + 10] = modPlayer.NormalVanityArmor[i];
                }
                else
                {
                    if (armor)
                        player.armor[i] = modPlayer.BuildingArmor[i];
                    if (vanityArmor)
                        player.armor[i + 10] = modPlayer.BuildingVanityArmor[i];
                }
            }


            //Misc Equips
            for (int i = 0; i < 5; i++)
            {
                if (miscEquips)
                {
                    if (modPlayer.IsNormalAccessories)
                        player.miscEquips[i] = modPlayer.NormalMiscEquips[i];
                    else
                        player.miscEquips[i] = modPlayer.BuildingMiscEquips[i];
                }
            }


            //Dyes
            for (int i = 0; i < 15; i++)
            {
                if (dyes)
                {
                    if (i < 10)
                    {
                        if (modPlayer.IsNormalAccessories)
                            player.dye[i] = modPlayer.NormalDyes[i];
                        else
                            player.dye[i] = modPlayer.BuildingDyes[i];
                    }
                    else
                    {
                        if (modPlayer.IsNormalAccessories)
                            player.miscDyes[i - 10] = modPlayer.NormalDyes[i];
                        else
                            player.miscDyes[i - 10] = modPlayer.BuildingDyes[i];
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
