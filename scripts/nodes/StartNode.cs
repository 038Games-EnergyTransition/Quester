#if TOOLS
using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents the start in the quest graph.
/// </summary>
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

        nameTextEdit.TextChanged += _onNameTextChanged;
        descriptionTextEdit.TextChanged += _onDescriptionTextChanged;
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

    private void _onNameTextChanged(string newText)
    {
        QuestName = newText;
    }

    private void _onDescriptionTextChanged()
    {
        QuestDescription = descriptionTextEdit.Text;
    }
}
#endif