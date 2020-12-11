using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Wisteria.Effects.VertexStrips
{
    public interface ITrailCap
    {
        int ExtraTris { get; }

        void AddCap(VertexPositionColorTexture[] array, ref int currentIndex, Color color, Vector2 position,
            Vector2 startNormal, float width);
    }

    public interface ITrailColor
    {
        Color GetColorAt(float distanceFromStart, float trailLength, List<Vector2> points);
    }

    public interface ITrailPosition
    {
        Vector2 GetNextTrailPosition(Projectile projectile);
    }

    public interface ITrailShader
    {
        string ShaderPass { get; }
        void ApplyShader(Effect effect, Trail trail, List<Vector2> positions);
    }
}