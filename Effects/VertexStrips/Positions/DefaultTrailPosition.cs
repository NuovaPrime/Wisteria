using Microsoft.Xna.Framework;
using Terraria;

namespace Wisteria.Effects.VertexStrips.Positions
{
    public class DefaultTrailPosition : ITrailPosition
    {
        public Vector2 GetNextTrailPosition(Projectile projectile) => projectile.Center;
    }
}