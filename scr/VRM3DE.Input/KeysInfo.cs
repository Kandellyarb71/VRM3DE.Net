using System.Collections.Generic;
using SFML.System;
using SFML.Window;

namespace VRM3DE.Input
{
    public interface IKeysInfo
    {
        public Dictionary<(Keyboard.Key, Keyboard.Key), Vector3f> MovingDirection { get; }
        public Dictionary<Keyboard.Key, bool> Focus { get; }
    }

    public struct KeysInfo : IKeysInfo
    {
        public static KeysInfo Standard { get; } = new KeysInfo(Keyboard.Key.Enter, Keyboard.Key.Escape, new[]
        {
            new KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f>((Keyboard.Key.W, Keyboard.Key.Up), new Vector3f(1, 0, 0)),
            new KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f>((Keyboard.Key.A, Keyboard.Key.Left), new Vector3f(0, 0, -1)),
            new KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f>((Keyboard.Key.S, Keyboard.Key.Down), new Vector3f(-1, 0, 0)),
            new KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f>((Keyboard.Key.D, Keyboard.Key.Right),  new Vector3f(0, 0, 1)),
            new KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f>((Keyboard.Key.Space, Keyboard.Key.Unknown), new Vector3f(0, 1, 0)),
            new KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f>((Keyboard.Key.LShift, Keyboard.Key.Unknown), new Vector3f(0, -1, 0))
        });
        public Dictionary<(Keyboard.Key, Keyboard.Key), Vector3f> MovingDirection { get; }
        public Dictionary<Keyboard.Key, bool> Focus { get; }

        public KeysInfo(Keyboard.Key focusKey, Keyboard.Key unFocusKey, KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f>[] movingKeys)
        {
            Focus = new Dictionary<Keyboard.Key, bool>();
            Focus.Add(focusKey, true);
            Focus.Add(unFocusKey, false);
            MovingDirection = new Dictionary<(Keyboard.Key, Keyboard.Key), Vector3f>();
            foreach (KeyValuePair<(Keyboard.Key, Keyboard.Key), Vector3f> pair in movingKeys)
            {
                MovingDirection.Add(pair.Key, pair.Value);
            }
        }
    }
}