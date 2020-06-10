using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        public override void Update(GameTime gameTime)
        {
            if (button.IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
            else
                Main.LocalPlayer.mouseInterface = false;
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

        public void SaveCurrentAccessories()
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            //Normal and Vanity Accessories
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                Item accessory = modPlayer.player.armor[i];
                Item vanityAccessory = modPlayer.player.armor[i+10];
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

        public void LoadAccessories()
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

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
                for (int i = 0; i < modPlayer.BuildingAccessories.Count; i++)
                {
                    modPlayer.player.armor[i + 3] = modPlayer.BuildingAccessories[i];
                    modPlayer.player.armor[i + 13] = modPlayer.BuildingVanityAccessories[i];
                }

                for (int i = 0; i < 3; i++)
                {
                    modPlayer.player.armor[i + 10] = modPlayer.BuildingVanityClothes[i];
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
