using Godot;
using Godot.Collections;

[Tool]
public abstract partial class QuestGraphNode : GraphNode
{

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public QuestNode GetModel() {
        QuestNode node = _getModel();
        node.GraphEditorPosition = PositionOffset;
        _setModelProperties(node);
        return node;
    }

    protected abstract QuestNode _getModel();
    protected abstract void _setModelProperties(QuestNode node);
    protected abstract void _getModelProperties(QuestNode node);

}