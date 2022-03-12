using System;
using SFML.System;
using VRM3DE.Input;
using VRM3DE.Scene;

namespace VRM3DE.Transform
{
    public abstract class Motion : ActionComponent
    {
        public abstract void Move(Inputs input, IMainWindow window);
    }

    public class FirstPersonMotion : Motion
    {
        private readonly float _moveSpeed;
        private readonly float _rotationSpeed;
        private readonly float _viewVerticalBorder;
        private readonly ITransform _transform;

        public FirstPersonMotion(ITransform transform, float moveSpeed, float rotationSpeed)
        {
            _transform = transform;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _viewVerticalBorder = 1.5555f;
        }

        public override void Move(Inputs input, IMainWindow window)
        {
            Vector2f rotationDrag = input.GetMouseDrag(window.MouseHiddenPosition, window.IsFocused);
            MathHelper.Normalize(ref rotationDrag, window.Size);
            _transform.Rotation += rotationDrag * _rotationSpeed;
            if (MathF.Abs(_transform.Rotation.Y) > _viewVerticalBorder)
            {
                _transform.Rotation = new Vector2f(_transform.Rotation.X, _viewVerticalBorder * MathF.Sign(_transform.Rotation.Y));
            }
            Vector3f positionDrag = input.GetMoveAxis();
            MathHelper.NormalizeOnPlane(ref positionDrag);
            MathHelper.TranslateOnPlane(ref positionDrag, _transform.Rotation.X);
            _transform.Position += positionDrag * _moveSpeed / 100;
        }
    }
}