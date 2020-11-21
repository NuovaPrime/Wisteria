using Terraria;
using Terraria.ModLoader;

namespace Wisteria
{
	public class Wisteria : Mod
	{
		internal ModHotKey breathKey;
		public static Wisteria Instance;

        public Wisteria()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true
            };

            Instance = this;
        }
        public override void Load()
        {
            if (!Main.dedServ)
                breathKey = RegisterHotKey("Breath In", "F");
        }
        public override void Unload()
        {
            Instance = null;
        }
    }
}