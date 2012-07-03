using System;
using OpenTK.Input;
using System.Drawing;


namespace ventis
{
	// Handles the mouse cursor.
	public class Mouse
	{
		public readonly Texture PointerCursor = Graphics.LoadTexture("cursors/default.png");
		public readonly Texture BusyCursor = Graphics.LoadTexture("cursors/busy.png");
		public readonly Texture ConnectCursor = Graphics.LoadTexture("cursors/connect.png");
		public readonly Texture BackgroundCursor = Graphics.LoadTexture("cursors/background.png");
		public readonly Texture LinkCursor = Graphics.LoadTexture("cursors/link.png");
		
		public Texture Cursor { get; set; }
		
		public Point Position { get; private set; }
		
		public Mouse(MouseDevice md)
		{
			
			Cursor = PointerCursor;
			md.Move += (sender, e) => Position = e.Position;
			Tasks.Add(() => drawCursor(Cursor, Position.X, Position.Y));
		}
		
		private void updateCursor()
		{
				
		}
		
		private void drawCursor(Texture t, int x, int y)
		{
			Graphics.DrawQuad(t, x, y, x + t.Width, y + t.Height);
		}
	}
}

