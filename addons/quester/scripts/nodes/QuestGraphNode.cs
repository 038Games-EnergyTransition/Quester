using Godot;
using Godot.Collections;

[Tool]
public abstract partial class QuestGraphNode : GraphNode
{

    public string Id;
    public bool HasLoadedPosition;

    public QuestNode GetModel() {
        QuestNode node = _getModel();
        node.Id = Id;
        node.GraphEditorPosition = PositionOffset;
        _setModelProperties(node);
        return node;
    }

    public void LoadModel(QuestNode node) {
        Id = node.Id;
        PositionOffset = node.GraphEditorPosition;
        HasLoadedPosition = true;
        _getModelProperties(node);
    }

    protected abstract QuestNode _getModel();
    protected abstract void _setModelProperties(QuestNode node);
    protected abstract void _getModelProperties(QuestNode node);

}