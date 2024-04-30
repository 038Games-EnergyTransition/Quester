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

    public override void _Ready()
    {
        typeSelect.SetItemIcon(0, GetThemeIcon("bool", "EditorIcons"));
        typeSelect.SetItemIcon(1, GetThemeIcon("String", "EditorIcons"));
        typeSelect.SetItemIcon(2, GetThemeIcon("Vector2", "EditorIcons"));
        typeSelect.SetItemIcon(3, GetThemeIcon("Vector3", "EditorIcons"));        
        typeSelect.SetItemIcon(4, GetThemeIcon("int", "EditorIcons"));
        typeSelect.SetItemIcon(5, GetThemeIcon("float", "EditorIcons"));

        typeSelect.ItemSelected += _onTypeSelected;
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
                index = 5;
                break;
        }

        typeSelect.Selected = index;
        SelectType(index);

        Control input = inputsContainer.GetChild(index) as Control;
        if (input is LineEdit)
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
            (input as SpinBox).Value = (float)value;
        }
    }

    private void _onTypeSelected(long index) {
        SelectType((int)index);
    }
        
        
    private void _onValueChanged(Variant value) {
        EmitSignal(SignalName.ValueChanged, value);
    }
}