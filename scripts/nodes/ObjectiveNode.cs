#if TOOLS
using Godot;
using Godot.Collections;

[Tool]
public partial class ObjectiveNode : QuestGraphNode
{

    public string QuestDescription;
    public bool Optional;

    [Export]
    protected CheckBox OptionalCheckBox;
    [Export]
    protected TextEdit descriptionTextEdit;
    [Export]
    protected MetadataEditor metadataEditor;

    public override void _Ready()
    {
        base._Ready();

        descriptionTextEdit.TextChanged += _onDescriptionTextChanged;
        OptionalCheckBox.Toggled += _onOptionalCheckBoxToggled;
    }

    protected override QuestNode _getModel()
    {
        return new QuestObjective();
    }

    protected override void _setModelProperties(QuestNode node)
    {
        QuestObjective objective = (QuestObjective)node;
        objective.Description = QuestDescription;
        objective.Optional = Optional;
        foreach (string key in GetMetaList())
        {
            objective.SetMeta(key, GetMeta(key));
        }
    }

    protected override void _getModelProperties(QuestNode node)
    {
        QuestObjective objective = (QuestObjective)node;
        QuestDescription = objective.Description;
        Optional = objective.Optional;
        descriptionTextEdit.Text = QuestDescription;
        OptionalCheckBox.ButtonPressed = Optional;
        foreach (string key in objective.GetMetaList())
        {
            SetMeta(key, objective.GetMeta(key));
        }
        metadataEditor.Update();
    }

    private void _onDescriptionTextChanged()
    {
        QuestDescription = descriptionTextEdit.Text;
    }

    private void _onOptionalCheckBoxToggled(bool toggled)
    {
        Optional = toggled;
    }
}
#endif