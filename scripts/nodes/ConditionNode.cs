#if TOOLS
using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents a condition in the quest graph.
/// </summary>
[Tool]
public partial class ConditionNode : QuestGraphNode
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
        return new QuestCondition();
    }

    /// <summary>
    /// Sets the properties of the model.
    /// </summary>
    /// <param name="node"></param>
    protected override void _setModelProperties(QuestNode node)
    {
        QuestCondition condition = (QuestCondition)node;
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
        QuestCondition condition = (QuestCondition)node;
        Type = condition.Type;
        Key = condition.Key;
        Value = condition.GetMeta("Value", false);

        typeInput.Text = Type;
        keyInput.Text = Key;
        metaInput.SetValue(Value);
    }

    /// <summary>
    /// Sets the type of the condition.
    /// </summary>
    /// <param name="type"></param>
    private void _onTypeTextChanged(string type)
    {
        Type = type;
    }

    /// <summary>
    /// Sets the key of the condition.
    /// </summary>
    /// <param name="key"></param>
    private void _onKeyTextChanged(string key)
    {
        Key = key;
    }

    /// <summary>
    /// Sets the metadata of the condition.
    /// </summary>
    /// <param name="value"></param>
    private void _onMetadataInputValueChanged(Variant value)
    {
        Value = value;
    }
}
#endif