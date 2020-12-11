using System.Collections.Generic;
using System.Linq;
using Wisteria.Effects.VertexStrips.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Wisteria.Effects.VertexStrips
{
    //stolen from dbt with love, thank you tyndareus
    public class Trail
    {
        /// <summary>
        /// Literally just for the Singleton usage of TrailManager, hopefully it works like that o.o
        /// </summary>
        public string UnlocalizedName => "Trail";
        
        /// <summary>
        /// This Trails projectile
        /// </summary>
        public Projectile Projectile { get; private set; }
        
        /// <summary>
        /// Whether this trail is dead; when it is dissolved
        /// </summary>
        public bool Dead { get; private set; }

        /// <summary>
        /// The projectile type thats being followed
        /// </summary>
        private int _originalProjectileType;

        /// <summary>
        /// Self references for individual functionality in provided areas
        /// </summary>
        private ITrailCap _trailCap;
        private ITrailColor _trailColor;
        private ITrailPosition _trailPosition;
        private ITrailShader _trailShader;

        /// <summary>
        /// Parameters of the trail
        /// </summary>
        private float _widthStart;
        private float _currentLength;
        private float _maxLength;

        /// <summary>
        /// Points on the trail
        /// </summary>
        private List<Vector2> _points;

        /// <summary>
        /// Dissolving parameters
        /// </summary>
        private bool _dissolving;
        private float _dissolveSpeed;
        private float _originalMaxLength;
        private float _originalWidth;

        public Trail()
        {
            Dead = false;
            _points = new List<Vector2>();
            _trailShader = new DefaultTrailShader();
        }

        #region Factory Methods
        /// <summary>
        /// Sets this trails color behaviour
        /// </summary>
        /// <param name="color"></param>
        public void SetTrailColor(ITrailColor color) => _trailColor = color;
        
        /// <summary>
        /// Sets this trails cap behaviour
        /// </summary>
        /// <param name="cap"></param>
        public void SetTrailCap(ITrailCap cap) => _trailCap = cap;
        
        /// <summary>
        /// Sets this trails shader behaviour
        /// </summary>
        /// <param name="shader"></param>
        public void SetTrailShader(ITrailShader shader) => _trailShader = shader;
        
        /// <summary>
        /// Sets this trail position behaviour
        /// </summary>
        /// <param name="position"></param>
        public void SetTrailPosition(ITrailPosition position) => _trailPosition = position;
        
        /// <summary>
        /// Sets this trails start width
        /// </summary>
        /// <param name="width"></param>
        public void SetTrailWidth(float width) => _widthStart = width;
        
        /// <summary>
        /// Sets this trails max length
        /// </summary>
        /// <param name="length"></param>
        public void SetTrailLength(float length) => _maxLength = length;
        
        /// <summary>
        /// Sets this trails projectile, and the original projectile type
        /// </summary>
        /// <param name="p"></param>
        public void SetProjectile(Projectile p)
        {
            Projectile = p;
            _originalProjectileType = p.type;
        }
        #endregion
        
        public void StartDissolve(float speed)
        {
            _dissolving = true;
            _dissolveSpeed = speed;
            _originalWidth = _widthStart;
            _originalMaxLength = _maxLength;
        }

        public void Update()
        {
            if (_dissolving)
            {
                _maxLength -= _dissolveSpeed;
                _widthStart = (_maxLength / _originalMaxLength) * _originalWidth;

                if (_maxLength <= 0.0f)
                {
                    Dead = true;
                    return;
                }

                TrimToLength(_maxLength);
                return;
            }

            if (!Projectile.active || Projectile.type != _originalProjectileType)
            {
                StartDissolve(_maxLength / 10f);
            }

            Vector2 point = (_trailPosition?.GetNextTrailPosition(Projectile) ?? Vector2.One);

            if (_points.Count == 0)
            {
                _points.Add(point);
                return;
            }

            float distance = Vector2.Distance(point, _points[0]);
            _points.Insert(0, point);

            if (_currentLength + distance > _maxLength)
            {
                TrimToLength(_maxLength);
            }
            else
            {
                _currentLength += distance;
            }
        }

        public void Draw(Effect effect, GraphicsDevice device)
        {
            if (Dead) return;
            if (_points.Count <= 1) return;

            float trailLength = 0;
            for (int i = 1; i < _points.Count; i++)
            {
                trailLength += Vector2.Distance(_points[i - 1], _points[i]);
            }

            int currentIndex = 0;
            VertexPositionColorTexture[] vertices =
                new VertexPositionColorTexture[(_points.Count - 1) * 6 + (_trailCap?.ExtraTris * 3 ?? 0)];

            //Definitely don't want this
            void AddVertex(Vector2 position, Color color, Vector2 uv)
            {
                vertices[currentIndex++] =
                    new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0.0f), color, uv);
            }

            float currentDistance = 0.0f;
            float halfWidth = _widthStart * 0.5f;

            Vector2 startNormal = CurveNormal(_points, 0),
                prevClockwise = _points[0] + startNormal * halfWidth,
                prevCClockwise = _points[0] - startNormal * halfWidth;

            Color previousColor = (_trailColor?.GetColorAt(0.0f, trailLength, _points) ?? new Color());

            _trailCap?.AddCap(vertices, ref currentIndex, previousColor, _points[0], startNormal, _widthStart);

            for (int i = 1; i < _points.Count; i++)
            {
                currentDistance += Vector2.Distance(_points[i - 1], _points[i]);
                float pointWidth = halfWidth * (1f - (i / (float) (_points.Count - 1)));
                
                Vector2 normal = CurveNormal(_points, i);
                Vector2 cw = _points[i] + normal * pointWidth;
                Vector2 ccw = _points[i] - normal * pointWidth;
                Color color = (_trailColor?.GetColorAt(currentDistance, trailLength, _points) ?? new Color());

                AddVertex(cw, color, Vector2.UnitX * i);
                AddVertex(prevClockwise, previousColor, Vector2.UnitX * (i - 1));
                AddVertex(prevCClockwise, previousColor, new Vector2(i - 1, 1f));
                
                AddVertex(cw, color, Vector2.UnitX * i);
                AddVertex(prevCClockwise, previousColor, new Vector2(i - 1, 1f));
                AddVertex(ccw, color, new Vector2(i, 1f));

                prevClockwise = cw;
                prevCClockwise = ccw;
                previousColor = color;
            }

            int width = device.Viewport.Width,
                height = device.Viewport.Height;

            Vector2 zoom = Main.GameViewMatrix.Zoom;
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
                          Matrix.CreateTranslation((float)width / 2, (float)height / -2, 0) * 
                          Matrix.CreateRotationZ(MathHelper.Pi) *
                          Matrix.CreateScale(zoom.X, zoom.Y, 1f);
            Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            effect.Parameters["WorldViewProjection"].SetValue(view * projection);

            _trailShader?.ApplyShader(effect, this, _points);
            device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, (_points.Count - 1) * 2 + (_trailCap?.ExtraTris ?? 0));
        }

        /// <summary>
        /// Reduces the length of the trail to the next closest
        /// </summary>
        /// <param name="length"></param>
        private void TrimToLength(float length)
        {
            if (_points.Count == 0) return;

            _currentLength = length;

            int firstPointOver = -1;
            float newLength = 0;

            for (int i = 1; i < _points.Count; i++)
            {
                newLength += Vector2.Distance(_points[i], _points[i - 1]);
                if (newLength > length)
                {
                    firstPointOver = i;
                    break;
                }
            }

            if (firstPointOver == -1) return;

            float leftOverLength = newLength - length;
            Vector2 between = _points[firstPointOver] - _points[firstPointOver - 1];
            float distance = between.Length() - leftOverLength;
            between.Normalize();

            int remove = _points.Count - firstPointOver;
            _points.RemoveRange(firstPointOver, remove);
            _points.Add(_points.Last() + between * distance);
        }

        /// <summary>
        /// Curves the normal of provided point
        /// </summary>
        /// <param name="points"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector2 CurveNormal(List<Vector2> points, int index)
        {
            if (points.Count == 1) return points[0];

            if (index == 0)
            {
                return Vector2.Normalize(points[1] - points[0]);//.RotateRight();
            }

            return index == points.Count - 1
                ? Vector2.Normalize(points[index] - points[index - 1])//.RotateRight()
                : Vector2.Normalize(points[index + 1] - points[index - 1]);//.RotateRight();
        }
    }
}