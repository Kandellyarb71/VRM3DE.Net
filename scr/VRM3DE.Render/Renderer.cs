using SFML.Graphics;
using SFML.System;
using VRM3DE.Transform;

namespace VRM3DE.Render
{
    public abstract class Renderer : ActionComponent
    {
        public abstract void RenderFrame(RenderTarget target, RayTransform transform, Vector2u resolution);
    }

    public class TargetRenderer : Renderer
    {
        private readonly IProjection _projection;

        public TargetRenderer(IProjection projection)
        {
            _projection = projection;
        }

        public override void RenderFrame(RenderTarget target, RayTransform transform, Vector2u resolution)
        {
            _projection.SetView(transform, resolution);
            target.Draw(_projection);
        }
    }
}