using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class ThreadManager : Node
{
	public static ThreadManager Instance;
	public List<Thread> threads;
	private int index = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		base._EnterTree();
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (threads != null)
		{
			if (index >= 0)
			{
				if (!threads[index].IsAlive)
				{
					threads[index].Join();
					threads.RemoveAt(index);
					GD.PrintErr("Thread Finished Work");
				}

				index--;
			}
			else
			{
				index = threads.Count - 1;
			}
		}
	}

	public static void InsertThread(Thread thread)
	{
		if (Instance.threads != null)
		{
			Instance.threads.Add(thread);
		}
		else
		{
			Instance.threads = new List<Thread>();
			Instance.threads.Add(thread);
		}
	}
}
