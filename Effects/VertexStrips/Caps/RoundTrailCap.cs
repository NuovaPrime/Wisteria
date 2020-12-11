using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Wisteria.Effects.VertexStrips.Caps
{
    public class RoundTrailCap : ITrailCap
    {
        public int ExtraTris => 20;

        public void AddCap(VertexPositionColorTexture[] array, ref int currentIndex, Color color, Vector2 position,
            Vector2 startNormal, float width)
        {
            float halfWidth = width * 0.5f;
            float arcStart = startNormal.ToRotation();
            float arcAmount = MathHelper.Pi;
            int segments = ExtraTris;
            float theta = arcAmount / segments;
            float cos = (float)Math.Cos(theta);
            float sin = (float)Math.Sin(theta);
            float t;
            float x = (float)Math.Cos(arcStart) * halfWidth;
            float y = (float)Math.Sin(arcStart) * halfWidth;

            position -= Main.screenPosition;

            VertexPositionColorTexture center =
                new VertexPositionColorTexture(new Vector3(position.X, position.Y, 0.0f), color, Vector2.One * 0.5f);
            VertexPositionColorTexture prev =
                new VertexPositionColorTexture(new Vector3(position.X + x, position.Y + y, 0.0f), color, Vector2.One);

            for (int i = 0; i < segments; i++)
            {
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
                
                VertexPositionColorTexture next =
                    new VertexPositionColorTexture(new Vector3(position.X + x, position.Y + y, 0.0f), color, Vector2.One);

                array[currentIndex++] = center;
                array[currentIndex++] = prev;
                array[currentIndex++] = next;

                prev = next;
            }
        }
    }
}