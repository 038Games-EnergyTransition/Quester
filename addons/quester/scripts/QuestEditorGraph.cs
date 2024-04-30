using Godot;
using Godot.Collections;

[Tool]
public partial class QuestEditorGraph : GraphEdit
{

	public PackedScene StartNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/StartNode.tscn");
	public PackedScene EndNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/EndNode.tscn");
	public PackedScene ObjectiveNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/ObjectiveNode.tscn");
	public PackedScene ConditionNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/ConditionNode.tscn");

	private Array<QuestGraphNode> _selectedNodes = new Array<QuestGraphNode>();
	private Array<QuestGraphNode> _copiedNodes = new Array<QuestGraphNode>();

	public override void _Ready()
	{
		ConnectionRequest += _onConnectionRequest;
		DisconnectionRequest += _onDisconnectionRequest;
		NodeSelected += _onNodeSelected;
		NodeDeselected += _onNodeDeselected;
		DeleteNodesRequest += _onDeleteNodesRequest;
		CopyNodesRequest += _onNodeCopyRequest;
		PasteNodesRequest += _onNodePasteRequest;
	}

	/// <summary>
	/// Clear all connections and nodes in the graph
	/// </summary>
	public void Clear()
	{
		ClearConnections();
		foreach (Node node in GetChildren())
		{
			RemoveChild(node);
			node.QueueFree();
		}
	}

	public Error Save(string path)
	{
		QuestResource resource = _serializeResource();
		if (resource != null)
		{
			return ResourceSaver.Save(resource, path);
		}
		return Error.CantCreate;
	}

	public QuestResource Load(string path)
	{
		QuestResource resource = ResourceLoader.Load(path, "", ResourceLoader.CacheMode.Ignore) as QuestResource;
		LoadResource(resource);
		return resource;
	}

	public void LoadResource(QuestResource resource)
	{
		_deserializeResource(resource);
	}

	/// <summary>
	/// Serialize the graph into a resource
	/// </summary>
	/// <returns></returns>
	private QuestResource _serializeResource()
	{
		Array<Node> startNodes = new Array<Node>();
		Array<Node> endNodes = new Array<Node>();

		foreach (Node node in GetChildren())
		{
			if (node is StartNode)
			{
				startNodes.Add(node);
			}
			else if (node is EndNode)
			{
				endNodes.Add(node);
			}
		}

		if (startNodes.Count != 1)
		{
			GD.PrintErr("QuestEditorGraph: Quest must have exactly one start node.");
			return null;
		}

		if (endNodes.Count != 1)
		{
			GD.PrintErr("QuestEditorGraph: Quest must have exactly one end node.");
			return null;
		}

		Array<Dictionary> connections = GetConnectionList();
		QuestResource resource = new QuestResource();
		Array<QuestEdge> edges = new Array<QuestEdge>();
		resource.Nodes = _getNodes(connections, ref edges);
		resource.Edges = edges;

		return resource;
	}

	/// <summary>
	/// Deserialize a resource into the graph
	/// </summary>
	/// <param name="resource"></param>
	private void _deserializeResource(QuestResource resource)
	{
		Clear();
		Dictionary modelToGraphNodeMap = new Dictionary();
		foreach (QuestNode node in resource.Nodes)
		{
			QuestGraphNode graphNode = null;
			switch (node)
			{
				case QuestStart startNode:
					graphNode = StartNodeScene.Instantiate() as StartNode;
					break;
				case QuestEnd endNode:
					graphNode = EndNodeScene.Instantiate() as EndNode;
					break;
				case QuestObjective objectiveNode:
					graphNode = ObjectiveNodeScene.Instantiate() as ObjectiveNode;
					break;
				case QuestCondition conditionNode:
					graphNode = ConditionNodeScene.Instantiate() as ConditionNode;
					break;
			}

			if (graphNode == null)
			{
				continue;
			}

			graphNode.LoadModel(node);
			AddNode(graphNode, node.GraphEditorPosition);
			modelToGraphNodeMap[node.Id] = graphNode;
		}

		GD.Print(resource.Nodes);
		GD.Print(resource.Edges);

		foreach (QuestEdge edge in resource.Edges)
		{
			ConnectNode(((GraphNode)modelToGraphNodeMap[edge.From]).Name, 0, ((GraphNode)modelToGraphNodeMap[edge.To]).Name, edge.edgeType == QuestEdge.EdgeType.NORMAL ? 0 : 1);
		}
	}

	/// <summary>
	/// Get all nodes and edges from the graph
	/// </summary>
	/// <param name="connections"></param>
	/// <param name="edges"></param>
	/// <returns></returns>
	private Array<QuestNode> _getNodes(Array<Dictionary> connections, ref Array<QuestEdge> edges)
	{
		Dictionary createdNodes = new Dictionary();
		foreach (QuestGraphNode node in GetChildren())
		{
			createdNodes[node.Name] = node.GetModel();
		}

		foreach (Dictionary connection in connections)
		{
			string fromNode = (string)connection["from_node"];
			string toNode = (string)connection["to_node"];
			int toPort = (int)connection["to_port"];

            QuestEdge edge = new QuestEdge
            {
                From = (QuestNode)createdNodes[fromNode],
                To = (QuestNode)createdNodes[toNode],
                edgeType = toPort == 0 ? QuestEdge.EdgeType.NORMAL : QuestEdge.EdgeType.CONDITIONAL
            };
            edges.Add(edge);
		}

		Array<QuestNode> result = new Array<QuestNode>();
		foreach (QuestNode node in createdNodes.Values)
		{
			result.Add(node);
		}
		return result;
	}

	/// <summary>
	/// Add a start node to the graph
	/// </summary>
	/// <param name="scene"></param>
	/// <param name="position"></param>
	public void AddNode(PackedScene scene, Vector2 position)
	{
		AddNode(scene.Instantiate() as QuestGraphNode, position);
	}
	public void AddNode(QuestGraphNode node, Vector2 position)
	{
		if (node.Id == null)
		{
			node.Id = NanoidGenerator.Generate(10);
		}
		node.PositionOffset = position;
		if (!node.HasLoadedPosition)
		{
			node.PositionOffset += ScrollOffset;
			node.PositionOffset /= Zoom;
		}
		AddChild(node);
	}

	/// <summary>
	/// Connect two nodes
	/// </summary>
	/// <param name="from"></param>
	/// <param name="fromSlot"></param>
	/// <param name="to"></param>
	/// <param name="toSlot"></param>
	private void _onConnectionRequest(StringName from, long fromSlot, StringName to, long toSlot)
	{
		ConnectNode(from, (int)fromSlot, to, (int)toSlot);
	}

	/// <summary>
	/// Disconnect two nodes
	/// </summary>
	/// <param name="from"></param>
	/// <param name="fromSlot"></param>
	/// <param name="to"></param>
	/// <param name="toSlot"></param>
	private void _onDisconnectionRequest(StringName from, long fromSlot, StringName to, long toSlot)
	{
		DisconnectNode(from, (int)fromSlot, to, (int)toSlot);
	}

	/// <summary>
	/// Select a node
	/// </summary>
	/// <param name="node"></param>
	private void _onNodeSelected(Node node)
	{
		if (node is QuestGraphNode)
		{
			(node as QuestGraphNode).Selected = true;
			if (!_selectedNodes.Contains(node as QuestGraphNode))
			{
				_selectedNodes.Add(node as QuestGraphNode);
			}
		}
	}

	/// <summary>
	/// Deselect a node
	/// </summary>
	/// <param name="node"></param>
	private void _onNodeDeselected(Node node)
	{
		if (node is QuestGraphNode)
		{
			(node as QuestGraphNode).Selected = false;
			if (_selectedNodes.Contains(node as QuestGraphNode))
			{
				_selectedNodes.Remove(node as QuestGraphNode);
			}
		}
	}

	/// <summary>
	/// Copy all selected nodes to a buffer
	/// </summary>
	private void _onNodeCopyRequest()
	{
		_copiedNodes.Clear();
		foreach (QuestGraphNode node in _selectedNodes)
		{
			_copiedNodes.Add(node);
		}
		_selectedNodes.Clear();
	}

	/// <summary>
	/// Paste all copied nodes to the graph with a slight offset
	/// </summary>
	private void _onNodePasteRequest()
	{
		foreach (QuestGraphNode node in _copiedNodes)
		{
			QuestGraphNode newNode = node.Duplicate() as QuestGraphNode;
			newNode.PositionOffset += new Vector2(50, 50);
			AddChild(newNode);
		}
	}

	/// <summary>
	/// Delete all selected nodes and their connections
	/// </summary>
	/// <param name="nodes"></param>
	private void _onDeleteNodesRequest(Array nodes)
	{
		foreach (Dictionary connection in GetConnectionList())
		{
			foreach (QuestGraphNode node in _selectedNodes)
			{
				if ((StringName)connection["from_node"] == node.Name || (StringName)connection["to_node"] == node.Name)
				{
					DisconnectNode((StringName)connection["from_node"], (int)connection["from_port"], (StringName)connection["to_node"], (int)connection["to_port"]);
				}
			}
		}

		foreach (QuestGraphNode node in _selectedNodes)
		{
			if (node == null)
			{
				continue;
			}

			RemoveChild(node);
			node.QueueFree();
		}
		_selectedNodes.Clear();
	}
}
