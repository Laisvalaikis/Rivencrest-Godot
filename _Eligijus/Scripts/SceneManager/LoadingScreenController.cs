// using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;


public partial class LoadingScreenController : Node
{
	private static LoadingScreenController _instance;
	[Export] 
	private Panel _darkScreen;
	[Export] 
	private Panel _loadingScreen;
	[Export] 
	private float _fadeLength;
	[Export] 
	private float _waitTime;

	private readonly Godot.Collections.Dictionary<string, string> GAME_SCENS = new Godot.Collections.Dictionary<string, string>
	{
		{ "game_world", "res://Scenes/Town.tscn" }
	};

	PackedScene loading_screen = GD.Load<PackedScene>("res://Scenes/LoadingScene.tscn");

	private StyleBoxFlat _styleBoxDarkScreen;
	private StyleBoxFlat _styleBoxLoadingScreen;
	private Array array;

	public override void _Ready()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	public static void LoadScene(Node currentScene, string nextScene)
	{
		Node loadingSceneInstance = _instance.loading_screen.Instantiate();
		_instance.GetTree().Root.AddChild(loadingSceneInstance); //CallDeferred("AddChild", loadingSceneInstance);
		
		string loadPath;
		if (_instance.GAME_SCENS.ContainsKey(nextScene))
		{
			loadPath = _instance.GAME_SCENS[nextScene];
		}
		else
		{
			loadPath = nextScene;
		}

		Error loaderNextScene = Error.Ok;
		if (ResourceLoader.Exists(loadPath))
		{
			loaderNextScene = ResourceLoader.LoadThreadedRequest(loadPath);
		}

		if (loaderNextScene != Error.Ok)
		{
			GD.PrintErr("Attempt to load non-existence file");
		}
		
		_instance.FreeUpScene(currentScene);

		while (true)
		{
			ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(loadPath, new Array());
			if (status == ResourceLoader.ThreadLoadStatus.InvalidResource)
			{
				GD.Print("Resource is invalid");
				break;
			}
			if (status == ResourceLoader.ThreadLoadStatus.Failed)
			{
				GD.Print("Error loading");
				break;
			}
			if (status == ResourceLoader.ThreadLoadStatus.InProgress)
			{
				
			}
			if (status == ResourceLoader.ThreadLoadStatus.Loaded)
			{
				PackedScene node = (PackedScene)ResourceLoader.LoadThreadedGet(loadPath);
				_instance.GetTree().Root.AddChild(node.Instantiate());//CallDeferred("AddChild", node.Instantiate());
				break;
			}
			
		}
		
	}

	private void FreeUpScene(Node currnet)
	{
		currnet.QueueFree();
	}

}
