#if TOOLS
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
        removeButton.Icon = EditorInterface
            .Singleton.GetEditorTheme()
            .GetIcon("Remove", "EditorIcons");
        _debouncer = new Debouncer(((EditorPlugin)EngineCS.GetMeta("QuesterPlugin")).GetTree());

        removeButton.Pressed += _OnRemoveButtonPressed;
        keyValue.TextChanged += _OnKeyValueTextChanged;
        metadataInput.ValueChanged += _OnMetadataInputValueChanged;
    }

    /// <summary>
    /// Sets the key-value pair for this metadata item.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetKeyValue(string key, Variant value)
    {
        _currentKey = key;
        keyValue.Text = key;
        _currentValue = value;
        metadataInput.SetValue(value);
    }

    /// <summary>
    /// Gets the key of the metadata item.
    /// </summary>
    /// <returns></returns>
    public string GetKey()
    {
        return _currentKey;
    }

    /// <summary>
    /// Gets the value of the metadata item.
    /// </summary>
    /// <returns></returns>
    public Variant GetValue()
    {
        return _currentValue;
    }

    /// <summary>
    /// Sets the editor for this metadata item.
    /// </summary>
    /// <param name="metaEditor"></param>
    public void SetEditor(MetadataEditor metaEditor)
    {
        _metaEditor = metaEditor;
    }

    /// <summary>
    /// Called when the remove button is pressed.
    /// </summary>
    public void _OnRemoveButtonPressed()
    {
        _metaEditor.EraseValue(this);
        QueueFree();
    }

    /// <summary>
    /// Called when the metadata input value changes.
    /// </summary>
    /// <param name="newValue"></param>
    public void _OnMetadataInputValueChanged(Variant newValue)
    {
        _currentValue = newValue;
        if (_currentKey != "")
        {
            _metaEditor.SetValue(this);
        }
    }

    /// <summary>
    ///  Called when the key value text changes.
    /// </summary>
    /// <param name="newText"></param>
    public void _OnKeyValueTextChanged(string newText)
    {
        Callable callable = new Callable(this, MethodName._debounceCallback);
        _debouncer.Debounce(callable);
    }

    /// <summary>
    /// callback used with the <see cref="Debouncer"/>.
    /// </summary>
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
#endif