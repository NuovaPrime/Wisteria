using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Wisteria.Common.Players
{
    public partial class WisteriaPlayer : ModPlayer
    {
        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add(nameof(breathingMastery), breathingMastery);
            //tag.Add(nameof(breathingStyle), breathingStyle);
            return tag;
        }
        public override void Load(TagCompound tag)
        {
            breathingMastery = tag.Get<float>(nameof(breathingMastery));
            //breathingStyle = tag.GetEnumerator();
        }
    }
}
