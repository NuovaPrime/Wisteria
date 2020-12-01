using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Wisteria.Common.Loaders;
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
        public int slayerRank;

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
            Loader.Autoload();

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
            Loader.Unload();

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

            layers.TryInsertLayer(layers.FindIndex(layer => layer.Name.Equals("Vanilla: Info Accessories Bar")), new LegacyGameInterfaceLayer(
                "Wisteria: SlayerRank",
                delegate
                {
                    if (Main.playerInventory)
                        Utils.DrawBorderString(Main.spriteBatch, $"Slayer Rank: {slayerRank}", new Vector2(Main.screenWidth - 455f, 20f), new Color(255, 255, 255));

                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}