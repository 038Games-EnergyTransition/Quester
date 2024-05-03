using Godot;
using Godot.Collections;

[Tool]
public partial class VariantInput : HBoxContainer
{

    [Signal]
    public delegate void ValueChangedEventHandler(Variant value);

    [Export]
    protected OptionButton typeSelect;
    [Export]
    protected Control inputsContainer;

    [ExportCategory("Variants")]
    [Export]
    public CheckBox boolInput;
    [Export]
    public LineEdit stringInput;
    [Export]
    public Vector2Input vector2Input;
    [Export]
    public Vector3Input vector3Input;
    [Export]
    public SpinBox intInput;
    [Export]
    public SpinBox floatInput;

    public override void _Ready()
    {
        typeSelect.SetItemIcon(0, GetThemeIcon("bool", "EditorIcons"));
        typeSelect.SetItemIcon(1, GetThemeIcon("String", "EditorIcons"));
        typeSelect.SetItemIcon(2, GetThemeIcon("Vector2", "EditorIcons"));
        typeSelect.SetItemIcon(3, GetThemeIcon("Vector3", "EditorIcons"));
        typeSelect.SetItemIcon(4, GetThemeIcon("int", "EditorIcons"));
        typeSelect.SetItemIcon(5, GetThemeIcon("float", "EditorIcons"));

        typeSelect.ItemSelected += _onTypeSelected;
        boolInput.Toggled += _onValueChanged;
        stringInput.TextChanged += _onValueChanged;
        vector2Input.ValueChanged += _onValueChanged;
        vector3Input.ValueChanged += _onValueChanged;
        intInput.ValueChanged += _onValueChanged;
        floatInput.ValueChanged += _onValueChanged;
    }

    public void SelectType(int index)
    {
        foreach (Control child in inputsContainer.GetChildren())
        {
            child.Hide();
        }
        (inputsContainer.GetChild(index) as Control).Show();
    }

    public void SetValue(Variant value)
    {
        int index = 0;
        switch (value.VariantType)
        {
            case Variant.Type.Bool:
                index = 0;
                break;
            case Variant.Type.String:
                index = 1;
                break;
            case Variant.Type.Vector2:
                index = 2;
                break;
            case Variant.Type.Vector3:
                index = 3;
                break;
            case Variant.Type.Int:
                index = 4;
                break;
            case Variant.Type.Float:
                index = (float)value % 1 == 0 ? 4 : 5;
                break;
        }

        typeSelect.Selected = index;
        SelectType(index);

        Control input = inputsContainer.GetChild(index) as Control;
        if (input is CheckBox)
        {
            (input as CheckBox).ButtonPressed = (bool)value;
        }
        else if (input is LineEdit)
        {
            (input as LineEdit).Text = value.ToString();
        }
        else if (input is Vector2Input)
        {
            (input as Vector2Input).SetValue((Vector2)value);
        }
        else if (input is Vector3Input)
        {
            (input as Vector3Input).SetValue((Vector3)value);
        }
        else if (input is SpinBox)
        {
            (input as SpinBox).Value = ((double)value) % 1 == 0 ? (int)value : (float)value;
        }
    }

    public Variant GetValue()
    {
        Control input = inputsContainer.GetChild(typeSelect.Selected) as Control;
        if (input is CheckBox)
        {
            return (input as CheckBox).ButtonPressed;
        }
        else if (input is LineEdit)
        {
            return (input as LineEdit).Text;
        }
        else if (input is Vector2Input)
        {
            return (input as Vector2Input).GetValue();
        }
        else if (input is Vector3Input)
        {
            return (input as Vector3Input).GetValue();
        }
        else if (input is SpinBox)
        {
            return (float)(input as SpinBox).Value;
        }
        
        return new Variant();
    }

    private void _onTypeSelected(long index)
    {
        SelectType((int)index);

        Variant value = GetValue();
        if (value.VariantType == Variant.Type.Float && (float)value % 1 == 0)
        {
            value = (int)value;
        }
        _emitValueChanged(value);
    }

    private void _onValueChanged(bool value)
    {
        _emitValueChanged(value);
    }

    private void _onValueChanged(string value)
    {
        _emitValueChanged(value);
    }

    private void _onValueChanged(Vector2 value)
    {
        _emitValueChanged(value);
    }

    private void _onValueChanged(Vector3 value)
    {
        _emitValueChanged(value);
    }

    private void _onValueChanged(double value)
    {
        if (value % 1 == 0)
        {
            _emitValueChanged((int)value);
        }
        else
        {
            _emitValueChanged(value);
        }
    }

    private void _emitValueChanged(Variant value)
    {
        EmitSignal(SignalName.ValueChanged, value);
    }
}