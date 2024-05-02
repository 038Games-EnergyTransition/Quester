using Godot;
using Godot.Collections;

[Tool]
public partial class EndNode : QuestGraphNode
{

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    protected override QuestNode _getModel()
    {
        return new QuestEnd();
    }

    protected override void _setModelProperties(QuestNode node)
    {
    }

    protected override void _getModelProperties(QuestNode node)
    {
    }
}