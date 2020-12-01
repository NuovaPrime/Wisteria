using System.Text.RegularExpressions;
using Terraria.ModLoader;

namespace Wisteria.Common.Loaders
{
    public abstract class SlayerRank
    {
        public Mod Mod { get; internal set; }

        public string Name { get; internal set; }

        public int Type { get; internal set; }

        public ModTranslation DisplayName => Mod.CreateTranslation($"SlayerRankName.{Name}");

        public virtual bool Autoload(ref string name) => Mod.Properties.Autoload;

        public virtual void AutoStaticDefaults()
        {
            if (DisplayName.IsDefault())
                DisplayName.SetDefault(Regex.Replace(Name, "([A-Z])", " $1").Trim());
        }

        public virtual void SetStaticDefaults()
        {
        }

        /// <summary>
        /// Allows you to do things when the player ranks up. <br />
        /// </summary>
        public virtual void OnRankUp()
        {
        }
    }
}