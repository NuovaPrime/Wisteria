using Terraria;
using Wisteria.Common.Players;

namespace Wisteria.Utilities
{
    public static partial class ExtensionMethods
    {
        public static WisteriaPlayer GetWisteriaPlayer(this Player player) => player.GetModPlayer<WisteriaPlayer>();
    }
}