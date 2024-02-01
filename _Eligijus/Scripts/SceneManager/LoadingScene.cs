using Godot;
using Godot.Collections;

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
	
	private readonly Dictionary<string, string> GAME_SCENS = new Dictionary<string, string>
	{
		{ "town", "res://Scenes/Town.tscn" }
	};

	public override void _Ready()
	{
		base._Ready();
		_fadeIn.AnimationFinished += delegate(StringName name) { LoadNextScene(_currentScene, _nextScene); };
	}

	public void FadeOut()
	{
		_fadeOut.Play("FadeOut");
		_fadeOut.AnimationFinished += delegate(StringName name) { QueueFree(); };
	}

	public void SetLoadingInformation(Node currentScene, string nextScene)
	{
		_currentScene = currentScene;
		_nextScene = nextScene;
	}
	
	private void LoadNextScene(Node currentScene, string nextScene)
	{
		_label.Show();
		string loadPath;
		if (GAME_SCENS.ContainsKey(nextScene))
		{
			loadPath = GAME_SCENS[nextScene];
		}
		else
		{
			loadPath = nextScene;
		}

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
		
		FreeUpScene(currentScene);
		
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (whileCheck)
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
				GetTree().Root.AddChild(node.Instantiate());
				_label.Hide();
				FadeOut();
				whileCheck = false;
			}

		}

	}

	private void FreeUpScene(Node current)
	{
		current.QueueFree();
	}
	

}
