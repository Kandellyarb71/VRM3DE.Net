using SFML.Graphics;
using SFML.System;
using VRM3DE.Transform;
using IDrawable = SFML.Graphics.Drawable;

namespace VRM3DE.Render
{
    public interface IProjection : IDrawable, IDataComponent
    {
        public void SetView(RayTransform transform, Vector2u resolution);
    }

    public struct RenderProjection : IProjection, IDataComponent<RenderProjection>
    {
        private readonly Shader _projectionShader;
        private readonly Sprite _projectionPlane;
        private readonly ShaderInfo _info;

        public RenderProjection(Vector2u resolution, ShaderInfo info)
        {
            _info = info;
            _projectionShader = new Shader(null, null, _info.Path);
            _projectionShader.SetUniform(_info.UniformNames[ShaderUniforms.Resolution], (Vector2f)resolution);
            Texture texture = new Texture(resolution.X, resolution.Y);
            _projectionPlane = new Sprite(texture);
        }
        
        public void Draw(RenderTarget target, RenderStates states)
        {
            states = new RenderStates(_projectionShader);
            target.Draw(_projectionPlane, states);
        }
        public void SetView(RayTransform transform, Vector2u resolution)
        {
            _projectionShader.SetUniform(_info.UniformNames[ShaderUniforms.Position], transform.Position);
            _projectionShader.SetUniform(_info.UniformNames[ShaderUniforms.Rotation], transform.Rotation);
            _projectionShader.SetUniform(_info.UniformNames[ShaderUniforms.Resolution], (Vector2f)resolution);
        }
    }
}