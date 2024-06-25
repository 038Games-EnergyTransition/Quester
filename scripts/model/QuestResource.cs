using Godot;
using Godot.Collections;

/// <summary>
/// A resource where all information about a quest is stored.
/// </summary>
[Tool]
public partial class QuestResource : Resource
{
    [Export]
    public Array<QuestNode> Nodes { get; set; } = new Array<QuestNode>();

    [Export]
    public Array<QuestEdge> Edges { get; set; } = new Array<QuestEdge>();

    private bool _wasInitialized = false;

    private QuestStart _intStartNode;
    private QuestStart _startNode
    {
        get
        {
            _initialize();
            return _intStartNode;
        }
        set { _intStartNode = value; }
    }

    public string Name
    {
        get { return _startNode.Name; }
    }
    public string Description
    {
        get { return _startNode.Description; }
    }
    public bool Started
    {
        get { return _startNode.Active; }
    }
    public bool Completed;
    public bool IsInstance;

    /// <summary>
    /// Instantiates a copy of the resource.
    /// </summary>
    /// <returns></returns>
    public QuestResource Instantiate()
    {
        if (IsInstance)
        {
            return this;
        }
        QuestResource instance = Duplicate(true) as QuestResource;
        instance.IsInstance = true;
        instance.SetMeta("resource_path", ResourcePath);
        return instance;
    }

    /// <summary>
    /// Starts the quest and does the necesery check to see if it isnt already complete.
    /// </summary>
    public void Start()
    {
        if (!IsInstance)
        {
            GD.PrintErr("QuestResource: Start() called on non-instance resource.");
            return;
        }

        if (!Completed && !Started)
        {
            _startNode.Active = true;
            _startNode.Completed = true;
            // DONE: Emit signal to notify that the quest has started.
            QuestManager.Instance.EmitSignal(QuestManager.SignalName.QuestStarted, this);
            //EmitSignal(QuestManager.SignalName.QuestStarted, this);
            NotifyActiveObjectives();
        }
    }

    /// <summary>
    /// Updates the quest by calling the update method of the start node.
    /// </summary>
    public void Update()
    {
        if (!Started)
        {
            return;
        }

        if (!IsInstance)
        {
            GD.PrintErr("QuestResource: Update() called on non-instance resource.");
            return;
        }

        if (!Completed)
        {
            _startNode.Update();
        }
    }

    /// <summary>
    /// Returns a list of all active nodes in the current quest
    /// </summary>
    public Array<QuestObjective> GetActiveObjectives()
    {
        Array<QuestObjective> objectives = new Array<QuestObjective>();
        foreach (QuestNode node in Nodes)
        {
            if (node is QuestObjective && (node as QuestObjective).Active)
            {
                objectives.Add(node as QuestObjective);
            }
        }
        return objectives;
    }

    /// <summary>
    /// Returns a list of all nodes before the given node.
    /// </summary>
    public Array<QuestNode> GetPreviousNodes(
        QuestNode node,
        QuestEdge.EdgeType edgeType = QuestEdge.EdgeType.NORMAL
    )
    {
        Array<QuestNode> previousNodes = new Array<QuestNode>();
        foreach (QuestEdge edge in Edges)
        {
            if (edge.To == node && edge.edgeType == edgeType)
            {
                previousNodes.Add(edge.From);
            }
        }
        return previousNodes;
    }

    /// <summary>
    /// Returns a list of all nodes after the given node.
    /// </summary>
    public Array<QuestNode> GetNextNodes(
        QuestNode node,
        QuestEdge.EdgeType edgeType = QuestEdge.EdgeType.NORMAL
    )
    {
        Array<QuestNode> nextNodes = new Array<QuestNode>();
        foreach (QuestEdge edge in Edges)
        {
            if (edge.From == node && edge.edgeType == edgeType)
            {
                nextNodes.Add(edge.To);
            }
        }
        return nextNodes;
    }

    /// <summary>
    /// Returns the data path of the current resource.
    /// </summary>
    public string GetResourcePath()
    {
        return (string)GetMeta("resource_path");
    }

    /// <summary>
    /// Fires off an event used to query a completeness of a condition.
    /// </summary>
    public void RequestQuery(string type, string key, Variant value, QuestCondition requester)
    {
        // DONE: Emit signal to request a query.
        QuestManager.Instance.EmitSignal(
            QuestManager.SignalName.ConditionQueryRequested,
            type,
            key,
            value,
            requester
        );
    }

    /// <summary>
    /// Fires off signalign an objective has been completed.
    /// </summary>
    public void CompleteObjective(QuestObjective objective)
    {
        // DONE: Emit signal to notify that an objective has been completed.
        QuestManager.Instance.EmitSignal(
            QuestManager.SignalName.QuestObjectiveCompleted,
            this,
            objective
        );
        NotifyActiveObjectives();
    }

    /// <summary>
    /// Fires off an event signaling this quest is finished and itself up.
    /// </summary>
    public void CompleteQuest()
    {
        Completed = true;
        // _startNode.Active = false;
        // DONE: Emit signal to notify that the quest has been completed.
        QuestManager.Instance.EmitSignal(QuestManager.SignalName.QuestCompleted, this);
        QuestManager.Instance.RemoveQuest(this);
    }

    /// <summary>
    /// Serializes the quest resource to a dictionary.
    /// </summary>
    public Dictionary Serialize()
    {
        Dictionary data = new Dictionary();
        data["Completed"] = Completed;
        data["Nodes"] = new Array();
        foreach (QuestNode node in Nodes)
        {
            ((Array)data["Nodes"]).Add(node.Serialize());
        }
        return data;
    }

    /// <summary>
    /// Deserializes the quest resource from a dictionary.
    /// </summary>
    public void Deserialize(Dictionary data)
    {
        if (!IsInstance)
        {
            GD.PrintErr("QuestResource: Deserialize() called on non-instance resource.");
            return;
        }

        Completed = (bool)data["Completed"];

        Dictionary node_map = new Dictionary();
        foreach (QuestNode node in Nodes)
        {
            node_map[node.Id] = node;
        }
        foreach (Dictionary node in (Array)data["Nodes"])
        {
            if (node_map.ContainsKey(node["Id"]))
            {
                ((QuestNode)node_map[node["Id"]]).Deserialize(node);
            }
        }
    }

    /// <summary>
    /// Initialized all nodes in the graph.
    /// </summary>
    public void _initialize()
    {
        if (_wasInitialized)
        {
            return;
        }

        foreach (QuestNode node in Nodes)
        {
            node.SetGraph(this);
            if (node is QuestStart)
            {
                _startNode = node as QuestStart;
            }
        }

        _wasInitialized = true;
    }

    /// <summary>
    /// Fires an event that signals a new objective has been added to the quest.
    /// </summary>
    public void NotifyActiveObjectives()
    {
        foreach (QuestObjective objective in GetActiveObjectives())
        {
            // DONE: Emit signal to notify that a new objective has been added.
            QuestManager.Instance.EmitSignal(
                QuestManager.SignalName.QuestObjectiveAdded,
                this,
                objective
            );
        }
    }

    /// <summary>
    /// Resets the quest to its initial state.
    /// </summary>
    internal void Reset()
    {
        Completed = false;
        foreach (QuestNode node in Nodes)
        {
            QuestNode.Reset(node);
        }
    }
}
