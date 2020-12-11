using Microsoft.Xna.Framework;
using Terraria;

namespace Wisteria.Effects.VertexStrips.Positions
{
    public class ZigZagTrailPosition : ITrailPosition
    {
        private int _zigType, _zigMove;
        private float _strength;

        public ZigZagTrailPosition(float strength)
        {
            _zigType = 0;
            _zigMove = 1;
            _strength = strength;
        }
        
        public Vector2 GetNextTrailPosition(Projectile projectile)
        {
            Vector2 offset = Vector2.Zero;
            if (_zigType == -1) offset = projectile.velocity;//.RotateLeft();
            else if (_zigType == 1) offset = projectile.velocity;//.RotateRight();
            if(_zigType != 0) offset.Normalize();

            _zigType += _zigMove;

            if (_zigType == 2)
            {
                _zigType = 0;
                _zigMove = -1;
            }
            else if (_zigType == -2)
            {
                _zigType = 0;
                _zigMove = 1;
            }

            return projectile.Center + offset * _strength;
        }
    }
}