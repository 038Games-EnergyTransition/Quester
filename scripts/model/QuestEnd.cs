using Godot;
using Godot.Collections;

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