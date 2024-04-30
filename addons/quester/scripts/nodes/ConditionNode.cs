using Godot;
using Godot.Collections;

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

    protected override QuestNode _getModel()
    {
        return new QuestCondition();
    }

    protected override void _setModelProperties(QuestNode node)
    {
        QuestCondition condition = (QuestCondition)node;
        condition.Type = Type;
        condition.Key = Key;
        condition.SetMeta("Value", Value);
    }

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

    private void _onTypeTextChanged(string type) {
        Type = type;
    }


    private void _onKeyTextChanged(string key) {
        Key = key;
    }


    private void _onMetadataInputValueChanged(Variant value) {
        GD.Print("Value changed: " + value);
        Value = value;
    }
}