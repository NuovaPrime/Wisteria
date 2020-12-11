using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Wisteria.Effects.VertexStrips.Colors
{
    public class StandardTrailColor : ITrailColor
    {
        private Color _color;

        public StandardTrailColor(Color color)
        {
            _color = color;
        }

        public Color GetColorAt(float distanceFromStart, float trailLength, List<Vector2> points)
        {
            float progress = distanceFromStart / trailLength;
            return _color * (1f - progress);
        }
    }
}