using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace Wisteria.Players
{
    public partial class WisteriaPlayer : ModPlayer
    {
        int breathSoundTimer = 0;
        SoundEffectInstance breathingSound = null;
        public void PreUpdateBreathing()
        {
            if (Breath > MaxBreath)
                Breath = MaxBreath;
            if (Breath < 0)
                Breath = 0;
            IsBreathing = false;
            
        }
        int decayTime = 0;
        public void PostUpdateBreathing()
        {
            if (Breath > 0 && !IsBreathing)
            {
                breathSoundTimer = 0;
                decayTime++;
                if (decayTime > 30)
                {
                    decayTime = 0;
                    IsBreathing = false;
                }
            }
            if (Breath > 0 && !IsBreathing)
            {
                Breath -= BreathingDecaySpeed;
            }
            if (IsBreathing == true)
            {
                breathSoundTimer++;
                if (breathSoundTimer == 1)
                {
                    breathingSound = Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/BreathIn"));

                }
                if (breathSoundTimer >= 84)
                    breathSoundTimer = 0;
            }
            if (!IsBreathing && breathingSound != null)
            {
                breathingSound.Stop();
            }
        }
        
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Wisteria.Instance.breathKey.Current && BreathCD <= 0)
            {
                IsBreathing = true;
                Breath += BreathingSpeed;

                for (int i = 0; i < 2; i++)
                {
                    Dust dust;
                    Vector2 position = Main.LocalPlayer.Center;
                    dust = Dust.NewDustDirect(position + new Vector2(0, -10), 0, -40, 263, 4.236842f, 0f, 0, new Color(255, 255, 255), 1.052632f);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.scale *= 0.5f;
                }
                

            }
        }
        public float BreathingSpeed { get; set; }
        public float BreathingDecaySpeed { get; set; }
        public float BreathingMastery { get; set; }
        public Enum BreathingStyle { get; set; }
        public bool IsBreathing { get; set; }
        public float Breath { get; set; }
        public float BreathCD { get; set; }
        public float MaxBreath { get; set; }
    }
}
