using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace Wisteria.Effects.VertexStrips.Colors
{
    public class RainbowTrailColor : ITrailColor
    {
        private float _speed, _distanceMultiplier, _saturation, _lightness;

        public RainbowTrailColor(float speed = 5.0f, float distanceMultiplier = 0.01f, float saturation = 1.0f,
            float lightness = 0.5f)
        {
            _speed = speed;
            _distanceMultiplier = distanceMultiplier;
            _saturation = saturation;
            _lightness = lightness;
        }

        public Color GetColorAt(float distanceFromStart, float trailLength, List<Vector2> points)
        {
            float progress = distanceFromStart / trailLength;
            float hue = (Main.GlobalTime * _speed + distanceFromStart * _distanceMultiplier) % MathHelper.TwoPi;
            return ColorFromHSL(hue, _saturation, _lightness) * (1f - progress);
        }

        private Color ColorFromHSL(float h, float s, float l)
        {
            h /= MathHelper.TwoPi;

            float r = 0, g = 0, b = 0;
            if (l != 0)
            {
                if (s == 0)
                    r = g = b = l;
                else
                {
                    float temp2;
                    if (l < 0.5f)
                        temp2 = l * (1f + s);
                    else
                        temp2 = l + s - (l * s);

                    float temp1 = 2f * l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 0.33333333f);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 0.33333333f);
                }
            }

            return new Color(r, g, b);
        }

        private float GetColorComponent(float temp1, float temp2, float temp3)
        {
            if (temp3 < 0f)
                temp3 += 1f;
            else if (temp3 > 1f)
                temp3 -= 1f;

            if (temp3 < 0.166666667f)
                return temp1 + (temp2 - temp1) * 6f * temp3;
            if (temp3 < 0.5f)
                return temp2;
            if (temp3 < 0.66666666f)
                return temp1 + ((temp2 - temp1) * (0.66666666f - temp3) * 6f);

            return temp1;
        }
    }
}