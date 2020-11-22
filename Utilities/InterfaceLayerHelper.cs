using System.Collections.Generic;
using Terraria.UI;

namespace Wisteria.Utilities
{
    public static class InterfaceLayerHelper
    {
        public static bool TryInsertLayer(this List<GameInterfaceLayer> layers, int index, GameInterfaceLayer interfaceLayer)
        {
            if (index != -1)
            {
                layers.Insert(index, interfaceLayer);

                return true;
            }

            return false;
        }
    }
}