using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Wisteria.Effects.VertexStrips.Shaders
{
    public class ImageTrailShader : ITrailShader
    {
        public string ShaderPass => "BasicImagePass";

        protected Vector2 _coordMult;
        protected float _xOffset;
        protected float _yAnimSpeed;
        protected float _strength;
        private Texture2D _texture;

        public ImageTrailShader(Texture2D image, Vector2 coordMult, float strength = 1.0f, float yAnimSpeed = 0.0f)
        {
            _coordMult = coordMult;
            _strength = strength;
            _yAnimSpeed = yAnimSpeed;
            _texture = image;
        }

        public ImageTrailShader(Texture2D image, float xCoordinateMultiplier, float yCoordinateMultiplier, float strength = 1f, float yAnimSpeed = 0f) : 
            this(image, new Vector2(xCoordinateMultiplier, yCoordinateMultiplier), strength, yAnimSpeed)
        {
        }
        
        public void ApplyShader(Effect effect, Trail trail, List<Vector2> positions)
        {
            _xOffset -= _coordMult.X;
            effect.Parameters["imageTexture"].SetValue(_texture);
            effect.Parameters["coordOffset"].SetValue(new Vector2(_xOffset, Main.GlobalTime * _yAnimSpeed));
            effect.Parameters["coordMultiplier"].SetValue(_coordMult);
            effect.Parameters["strength"].SetValue(_strength);
            effect.CurrentTechnique.Passes[ShaderPass].Apply();
        }
    }
}