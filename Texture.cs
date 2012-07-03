using System;
using System.Drawing;
namespace ventis
{
	public class Texture
	{
		public int Pointer { get; private set; }
		
		public Texture(int glptr) { Pointer = glptr; }
		
		
	}
}

