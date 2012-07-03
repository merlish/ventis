using System;

namespace ventis
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var gw = new OpenTK.GameWindow(1280, 800);
			var mc = new MainClass();
			gw.RenderFrame += delegate(object sender, OpenTK.FrameEventArgs e) {
				mc.RenderFrame();
			};
			
			// change to data dir
			System.IO.Directory.SetCurrentDirectory(System.IO.Directory.GetCurrentDirectory() + "/../../data/");
			
			// set up graphics
			Graphics.Init(gw.Context, gw.WindowInfo);
			
			// set up initial tasks
			Tasks.Add(() => Console.WriteLine("tick"));
			
			// todo: use input queue?
			
			gw.Run();
		}
		
		public void RenderFrame()
		{
			Graphics.BeginRendering();
			Tasks.Run();
			Graphics.EndRendering();
		}
	}
}
