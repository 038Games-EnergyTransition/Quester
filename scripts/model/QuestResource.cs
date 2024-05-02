using Godot;
using Godot.Collections;

[Tool]
public partial class QuestResource : Resource
{

    [Export]
    public Array<QuestNode> Nodes { get; set; } = new Array<QuestNode>();
    [Export]
    public Array<QuestEdge> Edges { get; set; } = new Array<QuestEdge>();

    private bool _wasInitialized = false;

    private QuestStart _intStartNode;
    private QuestStart _startNode {
        get { _initialize(); return _intStartNode; }
        set { _intStartNode = value; }
    }

    public string Name { get { return _startNode.Name; } }
    public string Description { get { return _startNode.Description; } }
    public bool Started { get { return _startNode.Active; } }
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
            QuestManager.GetInstance().EmitSignal(QuestManager.SignalName.QuestStarted, this);
            //EmitSignal(QuestManager.SignalName.QuestStarted, this);
            NotifyActiveObjectives();
        }
    }

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

    public Array<QuestObjective> GetActiveObjectives()
    {
        Array<QuestObjective> objectives = new Array<QuestObjective>();
        foreach (QuestNode node in Nodes)
        {
            if (node is QuestObjective && node.Active)
            {
                objectives.Add(node as QuestObjective);
            }
        }
        return objectives;
    }

    public Array<QuestNode> GetPreviousNodes(QuestNode node, QuestEdge.EdgeType edgeType = QuestEdge.EdgeType.NORMAL)
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

    public Array<QuestNode> GetNextNodes(QuestNode node, QuestEdge.EdgeType edgeType = QuestEdge.EdgeType.NORMAL)
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

    public string GetResourcePath()
    {
        return (string)GetMeta("resource_path");
    }

    public void RequestQuery(string type, string key, Variant value, QuestCondition requester)
    {
        // DONE: Emit signal to request a query.
        QuestManager.GetInstance().EmitSignal(QuestManager.SignalName.ConditionQueryRequested, type, key, value, requester);
    }

    public void CompleteObjective(QuestObjective objective)
    {
        // DONE: Emit signal to notify that an objective has been completed.
        QuestManager.GetInstance().EmitSignal(QuestManager.SignalName.QuestObjectiveCompleted, this, objective);
        NotifyActiveObjectives();
    }

    public void CompleteQuest()
    {
        Completed = true;
        // DONE: Emit signal to notify that the quest has been completed.
        QuestManager.GetInstance().EmitSignal(QuestManager.SignalName.QuestCompleted, this);
    }

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

    public void NotifyActiveObjectives()
    {
        foreach (QuestObjective objective in GetActiveObjectives())
        {
            // DONE: Emit signal to notify that a new objective has been added.
            QuestManager.GetInstance().EmitSignal(QuestManager.SignalName.QuestObjectiveAdded, this, objective);
        }
    }
}