#if TOOLS
using System.Linq;
using Godot;
using Godot.Collections;

[Tool]
public partial class MetadataEditor : VBoxContainer
{
    PackedScene MetadataItemScene = (PackedScene)
        GD.Load("res://addons/quester/scenes/MetadataItem.tscn");

    [Export]
    public Node container;

    [Export]
    public VBoxContainer itemsContainer;

    [Export]
    public Button addMetadataButton;

    private Array<MetadataItem> _currentMeta = new Array<MetadataItem>();

    public override void _Ready()
    {
        // addMetadataButton.Icon = EditorInterface.Singleton.GetEditorTheme().GetIcon("Add", "EditorIcons");
        addMetadataButton.Pressed += OnAddMetadataButtonPressed;
    }

    public void Update()
    {
        foreach (var key in container.GetMetaList())
        {
            _AddItem(key, container.GetMeta(key));
        }
    }

    /// <summary>
    /// Sets a metadata key-value pair.
    /// </summary>
    /// <param name="caller"></param>
    public void SetValue(MetadataItem caller)
    {
        var key = caller.GetKey();
        var alreadyHasKey = _currentMeta.Any(metaItem =>
            metaItem != caller && metaItem.GetKey() == key
        );
        if (alreadyHasKey)
        {
            GD.PrintErr(
                "Metadata key '{0}' already added to the node. Changes will not be saved.",
                key
            );
            return;
        }
        container.SetMeta(key, caller.GetValue());
    }

    /// <summary>
    /// Clears a metadata key-value pair.
    /// </summary>
    /// <param name="caller"></param>
    public void ClearValue(MetadataItem caller)
    {
        var key = caller.GetKey();
        if (container.HasMeta(key))
        {
            container.RemoveMeta(key);
        }
    }

    /// <summary>
    /// Erases a metadata key-value pair.
    /// </summary>
    /// <param name="caller"></param>
    public void EraseValue(MetadataItem caller)
    {
        ClearValue(caller);
        if (_currentMeta.Contains(caller))
        {
            _currentMeta.Remove(caller);
        }
    }

    /// <summary>
    /// Adds a metadata item to the container.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private MetadataItem _AddItem(string key, Variant value)
    {
        var instance = (MetadataItem)MetadataItemScene.Instantiate();
        instance.SetEditor(this);
        itemsContainer.AddChild(instance);
        instance.SetKeyValue(key, value);
        _currentMeta.Add(instance);
        return instance;
    }

    /// <summary>
    /// Called when the "Add Metadata" button is pressed.
    /// </summary>
    public void OnAddMetadataButtonPressed()
    {
        _AddItem("", false);
    }
}
#endif