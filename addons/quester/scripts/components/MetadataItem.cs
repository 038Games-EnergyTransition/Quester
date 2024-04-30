using Godot;

[Tool]
public partial class MetadataItem : VBoxContainer
{

    [Export]
    public LineEdit keyValue;
    [Export]
    public VariantInput metadataInput;
    [Export]
    public Button removeButton;

    private MetadataEditor _metaEditor;
    private string _currentKey;
    private Variant _currentValue;
    private Debouncer _debouncer;

    public override void _Ready()
    {
        removeButton.Icon = EditorInterface.Singleton.GetEditorTheme().GetIcon("Remove", "EditorIcons");
        _debouncer = new Debouncer(((EditorPlugin) EngineCS.GetMeta("QuesterPlugin")).GetTree());

        removeButton.Pressed += _OnRemoveButtonPressed;
        keyValue.TextChanged += _OnKeyValueTextChanged;
        metadataInput.ValueChanged += _OnMetadataInputValueChanged;
    }

    public void SetKeyValue(string key, Variant value)
    {
        _currentKey = key;
        keyValue.Text = key;
        _currentValue = value;
        metadataInput.SetValue(value);
    }

    public string GetKey()
    {
        return _currentKey;
    }

    public Variant GetValue()
    {
        return _currentValue;
    }

    public void SetEditor(MetadataEditor metaEditor)
    {
        _metaEditor = metaEditor;
    }

    public void _OnRemoveButtonPressed()
    {
        _metaEditor.EraseValue(this);
        QueueFree();
    }

    public void _OnMetadataInputValueChanged(Variant newValue)
    {
        _currentValue = newValue;
        if (_currentKey != "")
        {
            _metaEditor.SetValue(this);
        }
    }

    public void _OnKeyValueTextChanged(string newText)
    {
        Callable callable = new Callable(this, MethodName._debounceCallback);
        _debouncer.Debounce(callable);
    }

    private void _debounceCallback()
    {
        if (_currentKey != "")
        {
            _metaEditor.ClearValue(this);
        }
        _currentKey = keyValue.Text;
        _metaEditor.SetValue(this);
    }
}