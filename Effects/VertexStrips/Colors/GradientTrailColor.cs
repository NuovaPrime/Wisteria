using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Wisteria.Effects.VertexStrips.Colors
{
    public class GradientTrailColor : ITrailColor
    {
        private Color _startColor, _endColor;

        public GradientTrailColor(Color startColor, Color endColor)
        {
            _startColor = startColor;
            _endColor = endColor;
        }

        public Color GetColorAt(float distanceFromStart, float trailLength, List<Vector2> points)
        {
            float progress = distanceFromStart / trailLength;
            return Color.Lerp(_startColor, _endColor, progress) * (1f - progress);
        }
    }
}