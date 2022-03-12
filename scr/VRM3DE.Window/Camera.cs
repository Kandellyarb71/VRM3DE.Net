using SFML.System;
using VRM3DE.Input;
using VRM3DE.Render;
using VRM3DE.Scene;
using VRM3DE.Transform;

namespace VRM3DE.Window
{
    public class Camera : ISceneObject
    {
        private readonly ITransform _transform;
        private readonly Renderer _renderer;
        private readonly Motion _motion;

        public Camera(ShaderInfo info, float moveSpeed, float rotationSpeed)
        {
            IProjection projection = new RenderProjection(MainWindow.Instance.Size, info);
            _transform = new RayTransform();
            _transform.Position = new Vector3f(-3f, 0, 0);
            _motion = new FirstPersonMotion(_transform, moveSpeed, rotationSpeed);
            _renderer = new TargetRenderer(projection);
        }

        public void OnUpdate()
        {
            _motion.Move(KeyboardInput.Instance, MainWindow.Instance);
            _renderer.RenderFrame(MainWindow.Instance, (RayTransform)_transform, MainWindow.Instance.Size);
        }
    }
}