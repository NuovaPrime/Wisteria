using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wisteria.Effects.VertexStrips.Caps
{
    public class NoTrailCap : ITrailCap
    {
        public int ExtraTris => 0;

        public void AddCap(VertexPositionColorTexture[] array, ref int currentIndex, Color color, Vector2 position,
            Vector2 startNormal, float width)
        {
            
        }
    }
}