using System;
using Godot;
using Godot.Collections;

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

    public Array<QuestNode> PreviousNodes { get { return Graph.GetPreviousNodes(this); } }
    public Array<QuestNode> NextNodes { get { return Graph.GetNextNodes(this); } }

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

    public void Reset()
    {
        Active = false;
        Completed = false;
    }

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

    public void SetGraph(QuestResource _graph)
    {
        Graph = _graph;
    }

    public Dictionary Serialize()
    {
        Dictionary data = new Dictionary
        {
            { "id", Id },
            { "Completed", Completed }
        };

        return data;
    }


    public void Deserialize(Dictionary data)
    {
        Id = (string)data["Id"];

        Completed = (bool)data["Completed"];
    }
}