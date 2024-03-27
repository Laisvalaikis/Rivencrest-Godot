using System;
using Godot;
using Array = Godot.Collections.Array;

public partial class LoadingScene : Node
{
	[Export] 
	private Label _label;
	[Export] 
	private AnimationPlayer _fadeOut;
	[Export] 
	private AnimationPlayer _fadeIn;
	private Node _currentScene;
	private string _nextScene;
	private bool whileCheck = false;
	private string loadingPath;
	private Delegate animation;

	public override void _Ready()
	{
		base._Ready();
		_fadeIn.Play("FadeIn");
		_fadeIn.AnimationFinished += AnimationEnd;
	}

	public void AnimationEnd(StringName name)
	{
		LoadNextScene(_currentScene, _nextScene);
		// GD.Print("SSSSSAAA");
	}

	public void FadeOut()
	{
		_fadeOut.Play("FadeOut");
		_fadeOut.AnimationFinished += delegate(StringName name)
		{
			Free();
		};
	}

	public void SetLoadingInformation(Node currentScene, string nextScene)
	{
		_currentScene = currentScene;
		_nextScene = nextScene;
	}
	
	private void LoadNextScene(Node currentScene, string nextScene)
	{
		FreeUpScene(currentScene);
		_label.Show();
		string loadPath;
		loadPath = nextScene;

		Error loaderNextScene = Error.Ok;
		if (ResourceLoader.Exists(loadPath))
		{
			// currentScene.GetTree().Paused = true;
			loaderNextScene = ResourceLoader.LoadThreadedRequest(loadPath);
			loadingPath = loadPath;
			whileCheck = true;
		}

		if (loaderNextScene != Error.Ok)
		{
			GD.PrintErr("Attempt to load non-existence file");
		}
		
		
		
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (whileCheck && !IsInstanceValid(_currentScene))
		{

			ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(loadingPath, new Array());
			if (status == ResourceLoader.ThreadLoadStatus.InvalidResource)
			{
				GD.PrintErr("Resource is invalid");
				whileCheck = false;
			}

			if (status == ResourceLoader.ThreadLoadStatus.Failed)
			{
				GD.PrintErr("Error loading");
				whileCheck = false;
			}

			if (status == ResourceLoader.ThreadLoadStatus.InProgress)
			{

			}

			if (status == ResourceLoader.ThreadLoadStatus.Loaded)
			{
				PackedScene node = (PackedScene)ResourceLoader.LoadThreadedGet(loadingPath);
				Node scene = node.Instantiate();
				GetTree().Root.AddChild(scene);
				_label.Hide();
				FadeOut();
				GetTree().CurrentScene = scene;
				whileCheck = false;
			}

		}

	}

	private void FreeUpScene(Node current)
	{
		current.Free();
	}
	

}
