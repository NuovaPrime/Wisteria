using Terraria.ModLoader;

namespace Wisteria.Common.Players
{
    public partial class WisteriaPlayer : ModPlayer
    {
        public void UpdateBreathingProgression()
        {
            if (breathingMastery < breathingMasteryMax)
            {
                breathingMastery += 0.0005555555555555556f; //haha smol number
            }
        }
    }
}