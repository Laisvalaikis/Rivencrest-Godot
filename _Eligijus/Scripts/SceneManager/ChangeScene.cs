using Godot;
using System;

public partial class ChangeScene : Node
{
	[Export] 
	private string sceneToLoad;
	[Export] 
	private Node mainNode;
	public void SceneTransition()
	{
		UIStack.ClearStack();
		LoadingScreenController.LoadScene(mainNode, sceneToLoad);
	}
	
	public void SceneTransition(string scene)
	{
		UIStack.ClearStack();
		LoadingScreenController.LoadScene(mainNode, scene);
	}
	
}

