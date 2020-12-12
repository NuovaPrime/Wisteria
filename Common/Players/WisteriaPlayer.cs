using Terraria;
using Terraria.ModLoader;
using Wisteria.BreathingStyles;

namespace Wisteria.Common.Players
{
    public partial class WisteriaPlayer : ModPlayer
    {
        public bool playerInitialized;

        public override void PreUpdate()
        {
            PreUpdateBreathing();
        }

        public override void PostUpdate()
        {
            PostUpdateBreathing();
        }

        public override void Initialize()
        {
            if (!playerInitialized)
            {
                breath = 0;
                maxBreath = 100;
                BreathingStyle = (int)BreathingStyleEnum.None;
                breathingMastery = 0;
                breathingSpeed = 0.15f;
                breathingDecaySpeed = 0.25f;

                playerInitialized = true;
            }
        }

        public override void OnEnterWorld(Player player) => OnEnterWorldBreathing(player);
    }
}