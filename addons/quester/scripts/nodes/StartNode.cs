using Godot;
using Godot.Collections;

[Tool]
public partial class StartNode : QuestGraphNode
{

    public string QuestName;
    public string QuestDescription;

    [Export]
    protected LineEdit nameTextEdit;
    [Export]
    protected TextEdit descriptionTextEdit;

    public override void _Ready()
    {
        base._Ready();
    }

    protected override QuestNode _getModel()
    {
        return new QuestStart();
    }

    protected override void _setModelProperties(QuestNode node)
    {
        (node as QuestStart).Name = QuestName;
        (node as QuestStart).Description = QuestDescription;
    }

    protected override void _getModelProperties(QuestNode node)
    {
        QuestStart startNode = node as QuestStart;
        QuestName = startNode.Name;
        QuestDescription = startNode.Description;
        
        nameTextEdit.Text = QuestName;
        descriptionTextEdit.Text = QuestDescription;
    }
}