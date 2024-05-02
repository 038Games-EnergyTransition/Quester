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
        string name = "-";
        name = this is QuestStart ? "Start" : name;
        name = this is QuestEnd ? "End" : name;
        name = this is QuestCondition ? "Condition" : name;
        name = this is QuestObjective ? "Objective" : name;

        if (Completed)
        {
            foreach (QuestNode nextNode in NextNodes)
            {

                Update(nextNode);
            }
        } else
        {
            foreach (QuestNode node in Graph.Nodes)
            {
                if (node.Id == Id)
                {
                    GD.Print($"[{name}:{node.Active}]: Update()");
                    break;
                }
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