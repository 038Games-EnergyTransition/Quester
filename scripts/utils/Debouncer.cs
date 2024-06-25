using Godot;

/// <summary>
/// A debouncer class used for debouncing a callable function.
/// </summary>
[Tool]
public partial class Debouncer : RefCounted
{
    const double DEFAULT_DEBOUNCE_TIME = 0.25; // Seconds

    private SceneTree _tree;
    private double _debounceTime;
    private SceneTreeTimer _timer;

    public Debouncer(SceneTree tree, double time = DEFAULT_DEBOUNCE_TIME)
    {
        _tree = tree;
        _debounceTime = time;
    }

    /// <summary>
    /// Debounces a callable function.
    /// </summary>
    /// <param name="callable"></param>
    public void Debounce(Callable callable)
    {
        if (_timer != null)
        {
            DisconnectTimer();
            _timer = null;
        }
        _timer = _tree.CreateTimer(_debounceTime);
        _timer.Connect("timeout", callable); // TODO: One-shot signal
    }

    /// <summary>
    /// Disconnects the timer.
    /// </summary>
    public void DisconnectTimer()
    {
        foreach (var connection in _timer.GetSignalConnectionList("timeout"))
        {
            _timer.Disconnect("timeout", (Callable)connection["callable"]);
        }
    }
}
