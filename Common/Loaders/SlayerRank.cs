using Terraria.ModLoader;

namespace Wisteria.Common.Loaders
{
    public abstract class SlayerRank
    {
        public Mod Mod { get; internal set; }

        public string Name { get; internal set; }

        public int Type { get; internal set; }

        public virtual bool Autoload(ref string name) => Mod.Properties.Autoload;
    }
}