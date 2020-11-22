using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Wisteria.Players
{
    public partial class WisteriaPlayer : ModPlayer
    {
        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add(nameof(BreathingMastery), BreathingMastery);
            //tag.Add(nameof(BreathingStyle), BreathingStyle);
            return tag;
        }
        public override void Load(TagCompound tag)
        {
            BreathingMastery = tag.Get<float>(nameof(BreathingMastery));
            //BreathingStyle = tag.GetEnumerator();
        }
    }
}
