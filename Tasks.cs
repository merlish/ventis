using System;
using System.Collections.Generic;
using System.Threading;

namespace ventis
{
	public static class Tasks
	{
		static List<Action> tasks = new List<Action>();
		
		public static void Add(Action task) { tasks.Add(task); }
		public static void Remove(Action task) { tasks.Remove(task); }
		
		public static void Run()
		{
			tasks.ForEach(a => a());
		}
	}
}

