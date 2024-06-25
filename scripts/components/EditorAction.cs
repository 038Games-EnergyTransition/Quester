#if TOOLS
using System;
using Godot;

[Tool]
public partial class EditorAction : HBoxContainer
{
    [Export]
    public string IconName { get; set; }

    [Export]
    public string ActionText { get; set; }

    private bool _initialized = false;

    private enum STATE
    {
        FADE_IN,
        START_TIMER,
        WAIT,
        FADE_OUT
    }

    private STATE _state = STATE.FADE_IN;

    public override void _Ready()
    {
        setAlpha(0.0f);

        (FindChild("Timer") as Timer).Timeout += () =>
        {
            _state = STATE.FADE_OUT;
        };
    }

    public override void _Process(double delta)
    {
        switch (_state)
        {
            case STATE.FADE_IN:
                setAlpha(Mathf.Min(1.0f, Modulate.A + (float)delta));
                if (Modulate.A >= 1.0f)
                {
                    _state = STATE.START_TIMER;
                }
                break;

            case STATE.FADE_OUT:
                setAlpha(Mathf.Max(0.0f, Modulate.A - (float)delta));
                if (Modulate.A <= 0.0f)
                {
                    QueueFree();
                }
                break;

            case STATE.START_TIMER:
                (FindChild("Timer") as Timer).Start();
                _state = STATE.WAIT;
                break;

            case STATE.WAIT:
                break;
        }

        if (ActionText == null || ActionText.Length == 0)
        {
            return;
        }

        if (!_initialized)
        {
            _initialized = true;
            if (IconName == null || IconName.Length == 0)
            {
                (FindChild("Icon") as TextureRect).Visible = false;
            }
            else
            {
                (FindChild("Icon") as TextureRect).Texture = EditorInterface
                    .Singleton.GetEditorTheme()
                    .GetIcon(IconName, "EditorIcons");
            }
            (FindChild("Label") as Label).Text = ActionText;
        }
    }

    /// <summary>
    /// Sets the alpha of the action.
    /// </summary>
    /// <param name="alpha"></param>
    private void setAlpha(float alpha)
    {
        Color currentColor = Modulate;
        currentColor.A = alpha;
        Modulate = currentColor;
    }
}
#endif