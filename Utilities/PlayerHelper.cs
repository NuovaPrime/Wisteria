using Terraria;
using Wisteria.Players;

namespace Wisteria.Utilities
{
    public static class PlayerHelper
    {
        public static WisteriaPlayer GetWisteriaPlayer(this Player player) => player.GetModPlayer<WisteriaPlayer>();
    }
}