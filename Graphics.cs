using System;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL;

namespace ventis
{
	public static class Graphics
	{
		private static OpenTK.Graphics.IGraphicsContext context;
		private static OpenTK.Platform.IWindowInfo windowInfo;
		
		public static void Init(OpenTK.Graphics.IGraphicsContext context, OpenTK.Platform.IWindowInfo wininf)
		{
			Graphics.context = context;
			Graphics.windowInfo = wininf;
			
			init = true;
		}
		
		private static bool init = false;
		private static bool r = false;
		private static void check(bool not_r = false)
		{
			if (!init)
				throw new Exception("you need to call Graphics.Init before calling any other Graphics command");
			if (!not_r && !r)
				throw new Exception("you need to call Graphics.BeginRendering before calling any Graphics drawing commands");
		}
		
		public static void BeginRendering()
		{
			check(true);
			context.MakeCurrent(windowInfo);
			
			r = true;
			
			GL.ClearColor(0f, 0.5f, 0.5f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}
		
		public static void EndRendering()
		{
			check();
			context.SwapBuffers();
			r = false;
		}
		
		public static Texture LoadTexture(string filename)
		{
			return UploadNewTexture(new Bitmap(filename));
		}
		
		public static Texture NewTexture()
		{
			check(true);
			return new Texture(GL.GenTexture());
		}
		
		public static void UploadTexture(Texture tex, System.Drawing.Bitmap bitmap)
		{
			check(true);
            
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, tex.Pointer);
        
        	lock (bitmap)
            {
                BitmapData bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
       			             
                // Actually upload the data.
				GL.TexImage2D(TextureTarget.Texture2D, 0, (int)PixelInternalFormat.Rgba8, bd.Width, bd.Height, 
				              0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bd.Scan0);

                bitmap.UnlockBits(bd);
            }
            
            GL.Disable(EnableCap.Texture2D);
		}
		
		public static Texture UploadNewTexture(Bitmap bmp)
		{
			var t = NewTexture();
			UploadTexture(t, bmp);
			return t;
		}
		
		public static void DestroyTexture(Texture tex)
		{
			check(true);
			GL.DeleteTexture(tex.Pointer);
			tex.Pointer = 0;
		}
		
		public static Texture RenderText(string text, SizeF layoutArea, Color c)
		{
			var bmp = new Bitmap(Math.Round(layoutArea.Width), Math.Round(layoutArea.Height));
			var g = System.Drawing.Graphics.FromImage(bmp);
			g.DrawString(text, SystemFonts.MenuFont, new SolidBrush(c), new RectangleF(PointF.Empty, layoutArea));
			g.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);
			
			return UploadNewTexture(bmp);
		}
		
		public static void DrawQuad(Texture tex, float sx, float sy, float fx, float fy, float colorr = 1.0, float colorg = 1.0, float colorb = 1.0, float depth = 0.0, float alpha = 1.0, float tsx = 0.0, float tsy = 0.0, float tfx = 1.0, float tfy = 1.0)
		{
			check();
			
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, tex.Pointer);
			
			GL.Color4(colorr, colorg, colorb, alpha);
			
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			
			GL.Begin(BeginMode.Quads);
			
			// Top-left!
			GL.TexCoord2(tsx, tsy);
			GL.Vertex3(sx, sy, depth);
			// Top-right!
			GL.TexCoord2(tfx, tsy);
			GL.Vertex3(fx, sy, depth);
			// Bottom-right!
			GL.TexCoord2(tfx, tfy);
			GL.Vertex3(fx, fy, depth);
			// Bottom-left!
			GL.TexCoord2(tsx, tfy);
			GL.Vertex3(sx, fy, depth);
			
			GL.End();
			
			GL.Disable(EnableCap.Texture2D);
		}
	}
}

