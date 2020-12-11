using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;

/*using Terraria.Localization;*/

using Terraria.ModLoader;
using Wisteria.Common.Loaders;
using Wisteria.UI;

namespace Wisteria.Common.Players
{
    public partial class WisteriaPlayer : ModPlayer
    {
        public bool isBreathing;
        public float breathingSpeed, breathingDecaySpeed, breathingMastery, breath, breathCD, maxBreath, breathingMasteryMax, lungCapacityMastery, lungCapacityMax;
        public int breathSoundTimer, decayTime = 0;
        public SoundEffectInstance breathingSound = null;
        public int BreathingStyle;
        public int SlayerRank;

        public void OnEnterWorldBreathing(Player player)
        {
            // TODO: Fix localization
            if (Wisteria.Instance.breathKey.GetAssignedKeys().Count == 0)
                Main.NewText(/*Language.GetTextValue("Mods.Wisteria.BreathKeyUnbound")*/ "Your breath key appears to be unbound. Breathing is a core mechanic in Wisteria, so be sure to bind it to a key!");

            for (int i = 0; i < SlayerRankLoader.SlayerRanks.Count; i++)
                Main.NewText(SlayerRankLoader.SlayerRanks[i].DisplayName.ToString());
        }

        public void PreUpdateBreathing()
        {
            if (breath > maxBreath)
                breath = maxBreath;

            if (breath < 0)
                breath = 0;

            isBreathing = false;

            if (breath > 0)
                BreathUI.Visible = true;
            else
                BreathUI.Visible = false;
        }

        public void PostUpdateBreathing()
        {
            if (breath > 0 && !isBreathing)
            {
                breathSoundTimer = 0;

                if (++decayTime > 30)
                {
                    decayTime = 0;
                    isBreathing = false;
                }
            }

            if (breath > 0 && !isBreathing)
                breath -= breathingDecaySpeed;

            if (isBreathing == true)
            {
                if (++breathSoundTimer == 1)
                    breathingSound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BreathIn"));

                if (breathSoundTimer >= 84)
                    breathSoundTimer = 0;
            }

            if (!isBreathing && breathingSound != null)
                breathingSound.Stop();

            breathingMasteryMax = SlayerRank * SlayerRank >= (int)SlayerRanks.Kanoto ? 150 : 100;
            breathingSpeed = 0.15f * (breathingMastery / 95f);

        }


        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Wisteria.Instance.breathKey.Current && breathCD <= 0 && SlayerRank != (int)SlayerRanks.None)
            {
                isBreathing = true;
                breath += breathingSpeed;
                player.velocity *= 0.95f;
                UpdateBreathingProgression();

                for (int i = 0; i < 4; i++)
                {
                    Vector2 velocity = Vector2.UnitX.RotatedByRandom(MathHelper.ToRadians(45f)) * 3f * player.direction;
                    Dust dust = Dust.NewDustPerfect(Main.LocalPlayer.Center + new Vector2((10f + Main.rand.NextFloat(-5f, 5f)) * player.direction, -5f) + velocity * 8f, DustID.PortalBolt, velocity * -1, 0, new Color(255, 255, 255), 1.5f);
                    dust.scale *= 0.4f;
                    dust.noGravity = true;
                }
            }
        }
    }
}