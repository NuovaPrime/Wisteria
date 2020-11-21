using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Wisteria.Players
{
    public partial class WisteriaPlayer : ModPlayer
    {
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
            Breath = 0;
            MaxBreath = 100;
            PlayerInitialized = true;
            BreathingStyle = BreathingStyles.BreathingStyleEnum.None;
            BreathingMastery = 0;
            BreathingSpeed = 0.05f;
            BreathingDecaySpeed = 0.20f;
        }
        public bool PlayerInitialized { get; set; }
    }
}
