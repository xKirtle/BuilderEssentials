using BuilderEssentials.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace BuilderEssentials.Utilities
{
    public static partial class BuildingMode
    {
        public static bool IsNormalAccessories = true;
        //Variables updated through ModConfig
        public static bool accessories;
        public static bool vanityAccessories;
        public static bool armor;
        public static bool vanityArmor;
        public static bool miscEquips;
        public static bool dyes;

        public static void UpdateConfigVariables()
        {
            BuilderEssentialsConfig config = BuilderEssentialsConfig.Instance;
            accessories = config.accessories;
            vanityAccessories = config.vanityAccessories;
            armor = config.armor;
            vanityArmor = config.vanityArmor;
            miscEquips = config.miscEquips;
            dyes = config.dyes;
        }

        public static void ToggleBuildingMode()
        {
            SaveCurrentAccessories();
            IsNormalAccessories = !IsNormalAccessories;
            LoadAccessories();
            UpdateButtonImage();
        }

        public static void SaveCurrentAccessories()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            var player = Main.LocalPlayer;
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;

            if (IsNormalAccessories)
            {
                if (accessories)
                    for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                        modPlayer.NormalAccessories[i - 3] = player.armor[i];

                if (vanityAccessories)
                    for (int i = 13; i < 13 + maxAccessoryIndex; i++)
                        modPlayer.NormalVanityAccessories[i - 13] = player.armor[i];

                if (armor)
                    for (int i = 0; i < 3; i++)
                        modPlayer.NormalArmor[i] = player.armor[i];

                if (vanityArmor)
                    for (int i = 10; i < 13; i++)
                        modPlayer.NormalVanityArmor[i - 10] = player.armor[i];

                if (miscEquips)
                    for (int i = 0; i < 5; i++)
                        modPlayer.NormalMiscEquips[i] = player.miscEquips[i];

                if (dyes)
                    for (int i = 0; i < 15; i++)
                        modPlayer.NormalDyes[i] = ((i < 10) ? player.dye[i] : player.miscDyes[i - 10]);
            }
            else
            {
                if (accessories)
                    for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                        modPlayer.BuildingAccessories[i - 3] = player.armor[i];

                if (vanityAccessories)
                    for (int i = 13; i < 13 + maxAccessoryIndex; i++)
                        modPlayer.BuildingVanityAccessories[i - 13] = player.armor[i];

                if (armor)
                    for (int i = 0; i < 3; i++)
                        modPlayer.BuildingArmor[i] = player.armor[i];

                if (vanityArmor)
                    for (int i = 10; i < 13; i++)
                        modPlayer.BuildingVanityArmor[i - 10] = player.armor[i];

                if (miscEquips)
                    for (int i = 0; i < 5; i++)
                        modPlayer.BuildingMiscEquips[i] = player.miscEquips[i];

                if (dyes)
                    for (int i = 0; i < 15; i++)
                        modPlayer.BuildingDyes[i] = ((i < 10) ? player.dye[i] : player.miscDyes[i - 10]);
            }
        }

        public static void LoadAccessories()
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            var player = Main.LocalPlayer;
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;

            if (IsNormalAccessories)
            {
                if (accessories)
                    for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                        player.armor[i] = modPlayer.NormalAccessories[i - 3];

                if (vanityAccessories)
                    for (int i = 13; i < 13 + maxAccessoryIndex; i++)
                        player.armor[i] = modPlayer.NormalVanityAccessories[i - 13];

                if (armor)
                    for (int i = 0; i < 3; i++)
                        player.armor[i] = modPlayer.NormalArmor[i];

                if (vanityArmor)
                    for (int i = 10; i < 13; i++)
                        player.armor[i] = modPlayer.NormalVanityArmor[i - 10];

                if (miscEquips)
                    for (int i = 0; i < 5; i++)
                        player.miscEquips[i] = modPlayer.NormalMiscEquips[i];

                if (dyes)
                    for (int i = 0; i < 15; i++)
                        if (i < 10) player.dye[i] = modPlayer.NormalDyes[i];
                        else player.miscDyes[i - 10] = modPlayer.NormalDyes[i];
            }
            else
            {
                if (accessories)
                    for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                        player.armor[i] = modPlayer.BuildingAccessories[i - 3];

                if (vanityAccessories)
                    for (int i = 13; i < 13 + maxAccessoryIndex; i++)
                        player.armor[i] = modPlayer.BuildingVanityAccessories[i - 13];

                if (armor)
                    for (int i = 0; i < 3; i++)
                        player.armor[i] = modPlayer.BuildingArmor[i];

                if (vanityArmor)
                    for (int i = 10; i < 13; i++)
                        player.armor[i] = modPlayer.BuildingVanityArmor[i - 10];

                if (miscEquips)
                    for (int i = 0; i < 5; i++)
                        player.miscEquips[i] = modPlayer.BuildingMiscEquips[i];

                if (dyes)
                    for (int i = 0; i < 15; i++)
                        if (i < 10) player.dye[i] = modPlayer.BuildingDyes[i];
                        else player.miscDyes[i - 10] = modPlayer.BuildingDyes[i];
            }
        }

        public static void UpdateButtonImage()
        {
            Texture2D texture = IsNormalAccessories ? BuilderEssentials.BuildingModeOff : BuilderEssentials.BuildingModeOn;
            BasePanel.buildingModeButton.SetImage(texture);
        }
    }
}