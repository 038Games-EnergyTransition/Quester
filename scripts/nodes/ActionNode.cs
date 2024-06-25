#if TOOLS
using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents an action in the quest graph.
/// </summary>
[Tool]
public partial class ActionNode : QuestGraphNode
{
    public string Type;
    public string Key;
    public Variant Value;

    [Export]
    protected LineEdit typeInput;

    [Export]
    protected LineEdit keyInput;

    [Export]
    protected VariantInput metaInput;

    public override void _Ready()
    {
        base._Ready();

        typeInput.TextChanged += _onTypeTextChanged;
        keyInput.TextChanged += _onKeyTextChanged;
        metaInput.ValueChanged += _onMetadataInputValueChanged;
    }

    /// <summary>
    /// Returns a new instance of the model.
    /// </summary>
    /// <returns></returns>
    protected override QuestNode _getModel()
    {
        return new QuestAction();
    }

    /// <summary>
    /// Sets the properties of the model.
    /// </summary>
    /// <param name="node"></param>
    protected override void _setModelProperties(QuestNode node)
    {
        QuestAction condition = (QuestAction)node;
        condition.Type = Type;
        condition.Key = Key;
        condition.SetMeta("Value", Value);
    }

    /// <summary>
    /// Gets the properties of the model.
    /// </summary>
    /// <param name="node"></param>
    protected override void _getModelProperties(QuestNode node)
    {
        QuestAction condition = (QuestAction)node;
        Type = condition.Type;
        Key = condition.Key;
        Value = condition.GetMeta("Value", false);

        typeInput.Text = Type;
        keyInput.Text = Key;
        metaInput.SetValue(Value);
    }

    /// <summary>
    /// Updates the type of the action.
    /// </summary>
    /// <param name="type"></param>
    private void _onTypeTextChanged(string type)
    {
        Type = type;
    }

    /// <summary>
    /// Updates the key of the action.
    /// </summary>
    /// <param name="key"></param>
    private void _onKeyTextChanged(string key)
    {
        Key = key;
    }

    /// <summary>
    /// Updates the metadata of the action.
    /// </summary>
    /// <param name="value"></param>
    private void _onMetadataInputValueChanged(Variant value)
    {
        Value = value;
    }
}
#endif