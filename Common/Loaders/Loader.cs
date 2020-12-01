using System;
using System.Linq;
using Terraria.ModLoader;

namespace Wisteria.Common.Loaders
{
    internal static class Loader
    {
        internal static void Autoload()
        {
            // TODO: Hijack loading interface through reflection.
            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code != null)
                {
                    foreach (Type type in mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
                    {
                        // Don't attemp to autoload abstracts (duh) and don't attempt to load anything that doesn't have a default constructor.
                        if (type.IsAbstract || type.GetConstructor(new Type[0]) == null)
                            continue;

                        if (type.IsSubclassOf(typeof(SlayerRank)))
                            SlayerRankLoader.AutoloadSlayerRank(type, mod);
                    }
                }
            }
        }

        internal static void Unload() => SlayerRankLoader.Unload();
    }
}