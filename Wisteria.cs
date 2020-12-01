using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Wisteria.Players;
using Wisteria.UI;
using Wisteria.Utilities;
using static Wisteria.Players.WisteriaPlayer;

namespace Wisteria
{
    public class Wisteria : Mod
    {
        public SlayerRanks SlayerRank;
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
                string SlayerRankText = $"Slayer Rank: {SlayerRank}";
                int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Info Accessories Bar"));
                if (mouseTextIndex != -1)
                {
                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                        "Wisteria: SlayerRank",
                        delegate
                        {
                            if (Main.playerInventory == true) {
                                Utils.DrawBorderString(
                                Main.spriteBatch,
                                SlayerRankText,
                                new Vector2(Main.screenWidth - 500f, 5f),
                                new Color(255, 255, 255)); // set a color or sum idk
                            }
                            return true;

                        },
                        InterfaceScaleType.UI)
                    );
                }
            
        }
    }
}