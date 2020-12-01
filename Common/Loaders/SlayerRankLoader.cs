using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace Wisteria.Common.Loaders
{
    public static class SlayerRankLoader
    {
        internal static readonly IDictionary<string, SlayerRank> slayerRanks = new Dictionary<string, SlayerRank>();
        public static readonly IList<SlayerRank> SlayerRanks = new List<SlayerRank>();

        public static int SlayerRankCount { get; private set; } = 0;

        internal static void AutoloadSlayerRank(Type type, Mod mod)
        {
            SlayerRank slayerRank = (SlayerRank)Activator.CreateInstance(type);
            slayerRank.Mod = mod;
            string name = type.Name;

            if (slayerRank.Autoload(ref name))
                AddSlayerRank(name, slayerRank);
        }

        public static void AddSlayerRank(string name, SlayerRank slayerRank)
        {
            if (!(bool)typeof(Mod).GetField("loading", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(slayerRank.Mod))
                throw new Exception("AddSlayerRank can only be loaded automatically by Wisteria or called from Mod.Load");

            if (slayerRanks.ContainsKey(name))
                throw new Exception($"You tried to add 2 SlayerRanks with the same name ({name})! Maybe two classes share a classname but in different namespaces while autoloading or you manually called AddSlayerRank with two slayer ranks of the same name?");

            // slayerRank.Mod doesn't need to be set again.
            slayerRank.Name = name;
            slayerRank.Type = SlayerRankCount++;

            slayerRanks[name] = slayerRank;
            SlayerRanks.Add(slayerRank);
            ContentInstance.Register(slayerRank);
        }

        internal static void Unload() => SlayerRanks.Clear();

        public static SlayerRank GetSlayerRank(int type) => SlayerRanks[type];

        public static SlayerRank GetSlayerRank(string name) => slayerRanks.TryGetValue(name, out SlayerRank slayerRank) ? slayerRank : null;

        public static int SlayerRankType(string name) => GetSlayerRank(name)?.Type ?? 0;
    }
}