using Godot;

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
}