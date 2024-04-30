using Godot;
using Godot.Collections;

[Tool]
public partial class ObjectiveNode : QuestGraphNode
{

    [Export]
    protected CheckBox OptionalCheckBox;
    [Export]
    protected TextEdit descriptionTextEdit;
    [Export]
    protected MetadataEditor metadataEditor;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    protected override QuestNode _getModel()
    {
        return new QuestObjective();
    }

    protected override void _setModelProperties(QuestNode node)
    {
        throw new System.NotImplementedException();
    }

    protected override void _getModelProperties(QuestNode node)
    {
        throw new System.NotImplementedException();
    }
}