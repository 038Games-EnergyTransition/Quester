using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents the end of a quest.
/// </summary>
[Tool]
public partial class QuestEnd : QuestNode
{

    public new bool Active {
        get {
            return AllPreviousNodesCompleted();
        }
    }

    public new bool Completed {
        get {
            return Active;
        }
    }

    public new void Update()
    {
        if (Completed) {
            Graph.CompleteQuest();
        }
    }
}