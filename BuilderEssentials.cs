using BuilderEssentials.Assets;
using Terraria.ModLoader;

namespace BuilderEssentials
{
	public class BuilderEssentials : Mod
	{
		public override void Load() {
			AssetsLoader.AsyncLoadTextures();
		}

		public override void Unload() {
			AssetsLoader.UnloadTextures();
		}
	}
}