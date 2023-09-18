using Godot;
using System;

public partial class ChangeScene : Node
{
	[Export] 
	private int sceneToLoad;
	public void SceneTransition()
	{
		UIStack.ClearStack();
		LoadingScreenController.LoadScene(sceneToLoad);
	}
	
	public void SceneTransition(int sceneIndex)
	{
		UIStack.ClearStack();
		LoadingScreenController.LoadScene(sceneIndex);
	}
	
}
