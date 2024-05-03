using Godot;
using Godot.Collections;

[Tool]
public partial class Vector3Input : HBoxContainer
{
    [Signal]
    public delegate void ValueChangedEventHandler(Vector3 value);

    Vector3 _currentValue = Vector3.Zero;

    [Export]
    public SpinBox xValue;
    [Export]
    public SpinBox yValue;
    [Export]
    public SpinBox zValue;

    public override void _Ready()
    {
        xValue.ValueChanged += (value) =>
        {
            var newValue = new Vector3((float)value, (float)yValue.Value, (float)zValue.Value);
            EmitSignal(SignalName.ValueChanged, newValue);
        };
        
        yValue.ValueChanged += (value) =>
        {
            var newValue = new Vector3((float)xValue.Value, (float)value, (float)zValue.Value);
            EmitSignal(SignalName.ValueChanged, newValue);
        };

        zValue.ValueChanged += (value) =>
        {
            var newValue = new Vector3((float)xValue.Value, (float)yValue.Value, (float)value);
            EmitSignal(SignalName.ValueChanged, newValue);
        };
    }

    public void SetValue(Vector3 value)
    {
        xValue.Value = value.X;
        yValue.Value = value.Y;
        zValue.Value = value.Z;
    }

    public Vector3 GetValue()
    {
        return new Vector3((float)xValue.Value, (float)yValue.Value, (float)zValue.Value);
    }
}
