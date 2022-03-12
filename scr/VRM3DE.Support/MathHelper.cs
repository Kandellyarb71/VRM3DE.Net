using System;
using SFML.System;

namespace VRM3DE
{
    public static class MathHelper
    {
        public static void NormalizeOnPlane(ref Vector3f vector)
        {
            float length = MathF.Sqrt(MathF.Pow(vector.X, 2) + MathF.Pow(vector.Z, 2));
            if (length == 0)
            {
                length = 1;
            }
            vector = new Vector3f(vector.X / length, vector.Y, vector.Z / length);
        }

        public static void Normalize(ref Vector2f vector, Vector2u normalizer)
        {
            vector = new Vector2f(vector.X / normalizer.X, vector.Y / normalizer.Y);
        }

        public static void TranslateOnPlane(ref Vector3f vector, float angle)
        {
            Vector2f vector2 = new Vector2f(vector.X, vector.Z);
            Vector2f vector3 = vector2;
            vector2.X = vector3.X * MathF.Cos(angle) - vector3.Y * MathF.Sin(angle);
            vector2.Y = vector3.X * MathF.Sin(angle) + vector3.Y * MathF.Cos(angle);
            vector = new Vector3f(vector2.X, vector.Y, vector2.Y);
        }
    }
}