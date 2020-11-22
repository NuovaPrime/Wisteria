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
using Wisteria.UI;

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

            if (Breath > 0)
                BreathUI.visible = true;
            else
                BreathUI.visible = false;
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
            Main.NewText("Breath is: " + Breath);
        }
        
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Wisteria.Instance.breathKey.Current && BreathCD <= 0)
            {
                IsBreathing = true;
                Breath += BreathingSpeed;
                player.velocity *= 0.95f;

                for (int i = 0; i < 4; i++)
                {      
                    Vector2 mouthPos = Main.LocalPlayer.Center + new Vector2(player.direction == 1 ? 10 : -10, -5);
                    Vector2 velocity = Vector2.UnitX.RotatedByRandom(MathHelper.ToRadians(30f)) * 3f * player.direction;
                    mouthPos += velocity * 5f;
                    velocity *= -1;
                    Dust dust = Dust.NewDustPerfect(mouthPos, 263, velocity, 0, new Color(255, 255, 255), 1.5f);
                    dust.scale *= 0.3f;
                    dust.noGravity = true;
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
