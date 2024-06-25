using System;
using Godot;
using Godot.Collections;

/// <summary>
/// A template node that represents a node in a quest graph.
/// </summary>
[Tool]
public partial class QuestNode : Resource
{
    [Export]
    public string Id;

    [Export]
    public bool Optional;

    [Export]
    public Vector2 GraphEditorPosition { get; set; }

    public bool Active;
    public bool Completed;

    public QuestResource Graph;

    public Array<QuestNode> PreviousNodes
    {
        get { return Graph.GetPreviousNodes(this); }
    }
    public Array<QuestNode> NextNodes
    {
        get { return Graph.GetNextNodes(this); }
    }

    /// <summary>
    /// Returns true if all previous nodes are completed.
    /// </summary>
    public bool AllPreviousNodesCompleted()
    {
        foreach (QuestNode node in PreviousNodes)
        {
            if (!node.Completed && !node.Optional)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns true if any previous nodes are completed.
    /// </summary>
    public bool AnyPreviousNodesCompleted()
    {
        foreach (QuestNode node in PreviousNodes)
        {
            if (node.Completed || node.Optional)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns true if any children are active.
    /// </summary>
    public bool AnyChildrenActive()
    {
        foreach (QuestNode node in NextNodes)
        {
            if (node.Active)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Updates the node.
    /// </summary>
    public void Update()
    {
        if (Completed)
        {
            foreach (QuestNode nextNode in NextNodes)
            {
                Update(nextNode);
            }
        }
    }

    /// <summary>
    /// Updates the given node
    /// Fires from <see cref="Update"/> usualy the next node in line.
    /// </summary>

    public void Update(QuestNode node)
    {
        if (node is QuestStart)
        {
            (node as QuestStart).Update();
        }

        if (node is QuestObjective)
        {
            (node as QuestObjective).Update();
        }

        if (node is QuestEnd)
        {
            (node as QuestEnd).Update();
        }

        if (node is QuestCondition)
        {
            (node as QuestCondition).Update();
        }

        if (node is QuestAction)
        {
            (node as QuestAction).Update();
        }
    }

    /// <summary>
    /// Resets the node.
    /// </summary>
    public void Reset()
    {
        Active = false;
        Completed = false;
    }

    /// <summary>
    /// Resets the given node.
    /// </summary>
    public static void Reset(QuestNode node)
    {
        if (node is QuestStart)
        {
            (node as QuestStart).Reset();
        }

        if (node is QuestObjective)
        {
            (node as QuestObjective).Reset();
        }

        if (node is QuestEnd)
        {
            (node as QuestEnd).Reset();
        }

        if (node is QuestCondition)
        {
            (node as QuestCondition).Reset();
        }

        if (node is QuestAction)
        {
            (node as QuestAction).Reset();
        }
    }

    /// <summary>
    /// Sets the graph that this node belongs to.
    /// </summary>
    public void SetGraph(QuestResource _graph)
    {
        Graph = _graph;
    }

    /// <summary>
    /// Serializes the node to a dictionary.
    /// </summary>
    public Dictionary Serialize()
    {
        Dictionary data = new Dictionary { { "id", Id }, { "Completed", Completed } };

        return data;
    }

    /// <summary>
    /// Deserializes the node from a dictionary.
    /// </summary>
    public void Deserialize(Dictionary data)
    {
        Id = (string)data["Id"];

        Completed = (bool)data["Completed"];
    }
}
