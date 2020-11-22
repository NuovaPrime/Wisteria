using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Wisteria.UI;
using Wisteria.Utilities;

namespace Wisteria
{
    public class Wisteria : Mod
    {
        public static Wisteria Instance { get; private set; }

        public static BreathUI BreathUI;
        public static UserInterface BreathUIInterface;

        public ModHotKey breathKey;

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

                BreathUI = new BreathUI();
                BreathUI.Activate();

                BreathUIInterface = new UserInterface();
                BreathUIInterface.SetState(BreathUI);
            }
        }

        public override void Unload()
        {
            Instance = null;
            BreathUI.Visible = false;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.TryInsertLayer(layers.FindIndex(layer => layer.Name.Contains("Hotbar")), new LegacyGameInterfaceLayer(
                "Wisteria: Menus",
                delegate
                {
                    if (BreathUI.Visible)
                        BreathUIInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);

                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}