using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents an edge between two nodes in a quest graph.
/// </summary>
[Tool]
public partial class QuestEdge : Resource
{

    public enum EdgeType
    {
        NORMAL,
        CONDITIONAL
    }

    [Export]
    public QuestNode From;
    [Export]
    public QuestNode To;
    [Export]
    public EdgeType edgeType;

    public Dictionary Serialize()
    {
        Dictionary data = new Dictionary
        {
            { "From", From },
            { "To", To },
            { "EdgeType", edgeType == EdgeType.NORMAL ? 0 : 1 }
        };

        return data;
    }


    public void Deserialize(Dictionary data)
    {
        From = (QuestNode)data["From"];
        To = (QuestNode)data["To"];
        edgeType = (int)data["EdgeType"] == 0 ? EdgeType.NORMAL : EdgeType.CONDITIONAL;
    }
}