#if TOOLS
using Godot;
using Godot.Collections;

/// <summary>
/// A template node that represents a node in a quest graph.
/// </summary>
[Tool]
public abstract partial class QuestGraphNode : GraphNode
{
    public string Id;
    public bool HasLoadedPosition;

    /// <summary>
    /// Returns a new instance of the model.
    /// </summary>
    /// <returns></returns>
    public QuestNode GetModel()
    {
        QuestNode node = _getModel();
        node.Id = Id;
        node.GraphEditorPosition = PositionOffset;
        _setModelProperties(node);
        return node;
    }

    /// <summary>
    /// loads a model into a quest graph node.
    /// </summary>
    /// <param name="node"></param>
    public void LoadModel(QuestNode node)
    {
        Id = node.Id;
        PositionOffset = node.GraphEditorPosition;
        HasLoadedPosition = true;
        _getModelProperties(node);
    }

    protected abstract QuestNode _getModel();
    protected abstract void _setModelProperties(QuestNode node);
    protected abstract void _getModelProperties(QuestNode node);
}

}
#endif