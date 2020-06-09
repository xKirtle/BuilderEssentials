using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI
{
    public class BasePanel : UIState
    {
        public static UIImageButton button;
        public static Texture2D buttonTexture;
        public BuilderPlayer modPlayer;
        public override void OnInitialize()
        {
            buttonTexture = BuilderEssentials.BuildingModeOff;
            button = new UIImageButton(buttonTexture);
            //Bad positionning on screens != 1080p?
            button.VAlign = 0.03f;
            button.HAlign = 0.272f;
            button.OnClick += ChangeAccessories_OnClick;
            Append(button);
        }

        public void ChangeAccessories_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            CleanAcessoriesList();
            SaveCurrentAccessories();
            modPlayer.IsNormalAccessories = !modPlayer.IsNormalAccessories;
            LoadAccessories();
            UpdateButtonImage();
        }

        public void CleanAcessoriesList()
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            if (modPlayer.IsNormalAccessories)
                modPlayer.NormalAccessories.Clear();
            else
                modPlayer.BuildingAccessories.Clear();
        }

        //Vanity Accessories start on index 13
        public void SaveCurrentAccessories()
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                Item accessory = modPlayer.player.armor[i];
                if (modPlayer.IsNormalAccessories)
                    modPlayer.NormalAccessories.Add(accessory);
                else
                    modPlayer.BuildingAccessories.Add(accessory);
            }
        }

        public void LoadAccessories()
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            if (modPlayer.IsNormalAccessories)
                for (int i = 0; i < modPlayer.NormalAccessories.Count; i++)
                {
                    modPlayer.player.armor[i + 3] = modPlayer.NormalAccessories[i];
                    //var myItem = modPlayer.NormalAccessories[i];
                    //Main.LocalPlayer.armor[i + 3] = myItem;
                }
            else
            {
                for (int i = 0; i < modPlayer.BuildingAccessories.Count; i++)
                {
                    modPlayer.player.armor[i + 3] = modPlayer.BuildingAccessories[i];
                    //var myItem = modPlayer.BuildingAccessories[i];
                    //Main.LocalPlayer.armor[i + 3] = myItem;
                }
            }
        }
        public void UpdateButtonImage()
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (modPlayer.IsNormalAccessories)
                button.SetImage(BuilderEssentials.BuildingModeOff);
            else
                button.SetImage(BuilderEssentials.BuildingModeOn);
        }
    }
}
