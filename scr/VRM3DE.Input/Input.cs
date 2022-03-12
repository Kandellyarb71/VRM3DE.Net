using System;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using VRM3DE.Scene;

namespace VRM3DE.Input
{
    [GlobalObject]
    public abstract class Inputs
    {
        public abstract Vector3f GetMoveAxis();
        public abstract Vector2f GetMouseDrag(Vector2i mouseHiddenPosition, bool isFocused);
        public abstract bool GetFocus(Vector2i mouseHiddenPosition, bool isFocused);
    }

    [GlobalObject]
    public class KeyboardInput : Inputs
    {
        public static Inputs Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new ArgumentException("Input was used before assigned!");
                }
                return _instance;
            }
        }

        private static KeyboardInput? _instance;
        private readonly IKeysInfo _info;

        private KeyboardInput(IKeysInfo info)
        {
            _info = info;
        }

        public static void Instantiate(IKeysInfo info)
        {
            KeyboardInput input = new KeyboardInput(info);
            _instance = input;
        }

        public override Vector3f GetMoveAxis()
        {
            Vector3f moveAxis = new Vector3f();
            foreach (KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f> pair in _info.MovingDirection)
            {
                if (Keyboard.IsKeyPressed(pair.Key.Item1) || Keyboard.IsKeyPressed(pair.Key.Item2))
                {
                    moveAxis += pair.Value;
                }
            }
            return moveAxis;
        }

        public override Vector2f GetMouseDrag(Vector2i mouseHiddenPosition, bool isFocused)
        {
            if (!isFocused)
            {
                return new Vector2f();
            }
            Vector2f mouseDrag = (Vector2f)Mouse.GetPosition() - (Vector2f)mouseHiddenPosition;
            Mouse.SetPosition(mouseHiddenPosition);
            return mouseDrag;
        }

        public override bool GetFocus(Vector2i mouseHiddenPosition, bool isFocused)
        {
            foreach (KeyValuePair<Keyboard.Key, bool> pair in _info.Focus)
            {
                if (Keyboard.IsKeyPressed(pair.Key))
                {
                    isFocused = pair.Value;
                    if (pair.Value)
                    {
                        Mouse.SetPosition(mouseHiddenPosition);
                    }
                }
            }
            return isFocused;
        }
    }
}
