// using System;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Array = Godot.Collections.Array;


public partial class LoadingScreenController : Node
{
	private static LoadingScreenController _instance;
	PackedScene loading_screen = GD.Load<PackedScene>("res://Scenes/LoadingScene.tscn");
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
		LoadingScene loadingScene = (LoadingScene)loadingSceneInstance;
		loadingScene.SetLoadingInformation(currentScene, nextScene);
		_instance.GetTree().Root.CallDeferred("add_child", loadingSceneInstance);
	}
	

}
