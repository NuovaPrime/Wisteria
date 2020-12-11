using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wisteria.Effects.VertexStrips.Shaders
{
    public class DefaultTrailShader : ITrailShader
    {
        public string ShaderPass => "DefaultPass";
        public void ApplyShader(Effect effect, Trail trail, List<Vector2> positions) => effect.CurrentTechnique.Passes[ShaderPass].Apply();
    }
}