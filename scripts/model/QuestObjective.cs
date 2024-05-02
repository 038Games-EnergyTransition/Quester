using Godot;
using Godot.Collections;

[Tool]
public partial class QuestObjective : QuestNode
{

    [Export]
    public string Description;

    public Array<QuestNode> Conditions {
        get {
            return Graph.GetPreviousNodes(this, QuestEdge.EdgeType.CONDITIONAL);
        }
    }

    public bool IsExclusive {
        get {
            // foreach (QuestNode node in Graph.GetNextNodes(this)) {
            //     if (node is QuestExclusiveBranchConnector || node is QuestAnyPrevious) {
            //         return true;
            //     }
            // }
            return false;
        }
    }

    public new bool Active {
        get {
            if (Completed) {
                return false;
            }
            return AllPreviousNodesCompleted() && !AnyChildrenActive();
        }
    }

    public new void Update()
    {
        bool justCompleted = false;
        if (Active) {
            foreach (QuestNode condition in Conditions) {
                Update(condition);
                if (!condition.Completed) {
                    return;
                }
            }
            justCompleted = true;
            Completed = true;
        }

        base.Update();

        if (justCompleted) {
            Graph.CompleteObjective(this);
        }
    }
}