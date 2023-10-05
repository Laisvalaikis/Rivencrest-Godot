using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class ThreadManager : Node
{
	public static ThreadManager Instance;
	public List<Thread> threads;
	public List<Thread> waitingThreads;
	public List<Thread> workingThreads;
	private int index = 0;
	private int waitingIndex = -1;
	private int workingIndex = -1;
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

		if (waitingThreads != null && (threads != null && threads.Count == 0) || waitingThreads != null && threads == null)
		{
			if ( waitingThreads.Count > 0 )
			{
				if (waitingIndex >= 0)
				{
					waitingThreads[waitingIndex].Start();
					if (workingThreads == null)
					{
						workingThreads = new List<Thread>();
					}

					workingThreads.Add(waitingThreads[waitingIndex]);
					waitingThreads.RemoveAt(waitingIndex);
					waitingIndex--;
				}
				else
				{
					waitingIndex = waitingThreads.Count - 1;
				}
			}
		}

		if (workingThreads != null && workingThreads.Count > 0)
		{
			if (workingIndex >= 0)
			{
				if (!workingThreads[workingIndex].IsAlive)
				{
					workingThreads[workingIndex].Join();
					workingThreads.RemoveAt(workingIndex);
					GD.PrintErr("Thread Finished Work");
				}

				workingIndex--;
			}
			else
			{
				workingIndex = waitingThreads.Count - 1;
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
	
	public static void InsertThreadAndWaitOthers(Thread thread)
	{
		if (Instance.waitingThreads != null)
		{
			Instance.waitingThreads.Add(thread);
		}
		else
		{
			Instance.waitingThreads = new List<Thread>();
			Instance.waitingThreads.Add(thread);
		}
	}
}
