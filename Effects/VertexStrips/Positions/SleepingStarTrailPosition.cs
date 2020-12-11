using Microsoft.Xna.Framework;
using Terraria;

namespace Wisteria.Effects.VertexStrips.Positions
{
    public class SleepingStarTrailPosition : ITrailPosition
    {
        public Vector2 GetNextTrailPosition(Projectile projectile)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f,
                projectile.height * 0.5f);
            return projectile.position + drawOrigin + Vector2.UnitY * projectile.gfxOffY;
        }
    }
}