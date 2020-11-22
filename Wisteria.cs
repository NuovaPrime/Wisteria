using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Wisteria.UI;

namespace Wisteria
{
	public class Wisteria : Mod
	{
		internal ModHotKey breathKey;
		public static Wisteria Instance;
        private static BreathUI breathUI;
        private static UserInterface breathUIInterface;

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
            {
                breathKey = RegisterHotKey("Breath In", "F");
                
                breathUI = new BreathUI();
                breathUI.Activate();
                breathUIInterface = new UserInterface();
                breathUIInterface.SetState(breathUI);
            }
        }
        public override void Unload()
        {
            Instance = null;
            BreathUI.visible = false;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index2 = layers.FindIndex(layer => layer.Name.Contains("Hotbar"));
            if (index2 != -1)
            {
                layers.Insert(index2, new LegacyGameInterfaceLayer(
                    "Wisteria: Menus",
                    delegate
                    {
                        if (BreathUI.visible)
                        {
                            breathUIInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}