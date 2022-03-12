using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using VRM3DE.Input;
using VRM3DE.Scene;

namespace VRM3DE.Window
{
	[GlobalObject]
	public sealed class MainWindow : RenderWindow, IMainWindow, IUpdateable
	{
		public static MainWindow Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new ArgumentException("MainWindow was used before assigned!");
				}
				return _instance;
			}
		}

		public bool IsFocused
		{
			get
			{
				return _isFocused;
			}
			set
			{
				_isFocused = value;
			}
		}

		public Vector2i MouseHiddenPosition
		{
			get
			{
				return (Vector2i)(Size / 2 + (Vector2u)Position);
			}
		}

		private static MainWindow? _instance;
		private bool _isFocused;

		private MainWindow(Vector2u resolution, string tittle) : base(new VideoMode(resolution.X, resolution.Y), tittle)
		{
			SetVerticalSyncEnabled(true);
			Closed += CloseWindow;
		}

		public static void Instantiate(Vector2u resolution, string tittle)
		{
			MainWindow window = new MainWindow(resolution, tittle);
			_instance = window;
		}

		public void OnUpdate()
		{
			DispatchEvents();
			UpdateFocus();
			Display();
		}

		private void CloseWindow(object? sender, EventArgs args)
		{
			Close();
		}

		private void UpdateFocus()
		{
			_isFocused = KeyboardInput.Instance.GetFocus(MouseHiddenPosition, _isFocused);
			SetMouseCursorVisible(!_isFocused);
		}
	}
}