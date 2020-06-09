using BuilderEssentials.UI;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuilderEssentials
{
    public class BuilderPlayer : ModPlayer
    {
        public List<Item> NormalAccessories;
        public List<Item> BuildingAccessories;
        public bool IsNormalAccessories;

        public override void Initialize()
        {
            NormalAccessories = new List<Item>(7);
            BuildingAccessories = new List<Item>(7);
            IsNormalAccessories = true;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "IsNormalAccessories", IsNormalAccessories },
                { "NormalAccessories", NormalAccessories },
                { "BuildingAccessories", BuildingAccessories }
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
        }

        public override void OnEnterWorld(Player player)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (!modPlayer.IsNormalAccessories)
                BasePanel.button.SetImage(BuilderEssentials.BuildingModeOn);
            else
                BasePanel.button.SetImage(BuilderEssentials.BuildingModeOff);
        }
    }
}
