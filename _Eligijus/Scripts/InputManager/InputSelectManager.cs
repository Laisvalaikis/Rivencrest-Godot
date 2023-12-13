using Godot;
using Godot.Collections;

public partial class InputSelectManager : Node
{
    private static InputSelectManager _instance;
    public static InputScheme CurrentInputScheme { get; private set; } = InputScheme.KeyboardAndMouse;
    [Export] protected InputScheme controll = InputScheme.KeyboardAndMouse;
    [Export] private Array<InputManager> _inputManagers;
    public override void _EnterTree()
    {
        base._EnterTree();
        if (_instance is null)
        {
            _instance = this;
        }
        DisableAllInputs();
        ChangeInputScheme(controll);
    }

    public static void ChangeInputScheme(InputScheme inputScheme)
    {
        _instance._inputManagers[(int)_instance.controll].DisableInputManager();
        CurrentInputScheme = inputScheme;
        _instance.controll = inputScheme;
        _instance._inputManagers[(int)_instance.controll].EnableInputManager();
    }

    private void DisableAllInputs()
    {
        for (int i = 0; i < _inputManagers.Count; i++)
        {
            _inputManagers[i].DisableInputManager();
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (controll != CurrentInputScheme)
        {
            ChangeInputScheme(CurrentInputScheme);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (_instance == this)
        {
            _instance = null;
        }
    }
}