using Godot;
using Godot.Collections;

[Tool]
public partial class Vector2Input : HBoxContainer
{
    [Signal]
    public delegate void ValueChangedEventHandler(Vector2 value);

    Vector2 _currentValue = Vector2.Zero;

    [Export]
    public SpinBox xValue;
    [Export]
    public SpinBox yValue;

    public override void _Ready()
    {
        xValue.ValueChanged += (value) =>
        {
            var newValue = new Vector2((float)value, (float)yValue.Value);
            EmitSignal(SignalName.ValueChanged, newValue);
        };
        yValue.ValueChanged += (value) =>
        {
            var newValue = new Vector2((float)xValue.Value, (float)value);
            EmitSignal(SignalName.ValueChanged, newValue);
        };
    }

    public void SetValue(Vector2 value)
    {
        xValue.Value = value.X;
        yValue.Value = value.Y;
    }

    public Vector2 GetValue()
    {
        return new Vector2((float)xValue.Value, (float)yValue.Value);
    }
}
