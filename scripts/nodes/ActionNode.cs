#if TOOLS
using Godot;
using Godot.Collections;

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

    protected override QuestNode _getModel()
    {
        return new QuestAction();
    }

    protected override void _setModelProperties(QuestNode node)
    {
        QuestAction condition = (QuestAction)node;
        condition.Type = Type;
        condition.Key = Key;
        condition.SetMeta("Value", Value);
    }

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

    private void _onTypeTextChanged(string type) {
        Type = type;
    }


    private void _onKeyTextChanged(string key) {
        Key = key;
    }


    private void _onMetadataInputValueChanged(Variant value) {
        Value = value;
    }
}
#endif