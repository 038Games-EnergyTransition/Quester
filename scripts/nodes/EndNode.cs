#if TOOLS
using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents the end in the quest graph.
/// </summary>
[Tool]
public partial class EndNode : QuestGraphNode
{
    public override void _Ready() { }

    public override void _Process(double delta) { }

    /// <summary>
    /// Returns a new instance of the model.
    /// </summary>
    /// <returns></returns>
    protected override QuestNode _getModel()
    {
        return new QuestEnd();
    }

    /// <summary>
    /// does nothing
    /// </summary>
    /// <param name="node"></param>
    protected override void _setModelProperties(QuestNode node) { }

    /// <summary>
    /// does nothing
    /// </summary>
    /// <param name="node"></param>
    protected override void _getModelProperties(QuestNode node) { }
}
#endif