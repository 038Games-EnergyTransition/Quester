using Godot;
using Godot.Collections;

[Tool]
public partial class QuestEditorGraph : GraphEdit
{

	public PackedScene StartNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/StartNode.tscn");
	public PackedScene EndNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/EndNode.tscn");
	public PackedScene ObjectiveNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/ObjectiveNode.tscn");
	public PackedScene ConditionNodeScene = GD.Load<PackedScene>("res://addons/quester/nodes/ConditionNode.tscn");

	private Array<GraphNode> _selectedNodes = new Array<GraphNode>();
	private Array<GraphNode> _copiedNodes = new Array<GraphNode>();

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

	public override void _Process(double delta)
	{
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

	/// <summary>
	/// Add a start node to the graph
	/// </summary>
	/// <param name="scene"></param>
	/// <param name="position"></param>
	public void AddNode(PackedScene scene, Vector2 position)
	{
		GraphNode node = scene.Instantiate() as GraphNode;
		node.PositionOffset += position;
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
		if (node is GraphNode)
		{
			(node as GraphNode).Selected = true;
			if (!_selectedNodes.Contains(node as GraphNode))
			{
				_selectedNodes.Add(node as GraphNode);
			}
		}
	}

	/// <summary>
	/// Deselect a node
	/// </summary>
	/// <param name="node"></param>
	private void _onNodeDeselected(Node node)
	{
		if (node is GraphNode)
		{
			(node as GraphNode).Selected = false;
			if (_selectedNodes.Contains(node as GraphNode))
			{
				_selectedNodes.Remove(node as GraphNode);
			}
		}
	}

	/// <summary>
	/// Copy all selected nodes to a buffer
	/// </summary>
	private void _onNodeCopyRequest()
	{
		_copiedNodes.Clear();
		foreach (GraphNode node in _selectedNodes)
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
		foreach (GraphNode node in _copiedNodes)
		{
			GraphNode newNode = node.Duplicate() as GraphNode;
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
			foreach (GraphNode node in _selectedNodes)
			{
				if ((StringName)connection["from_node"] == node.Name || (StringName)connection["to_node"] == node.Name)
				{
					DisconnectNode((StringName)connection["from_node"], (int)connection["from_port"], (StringName)connection["to_node"], (int)connection["to_port"]);
				}
			}
		}

		foreach (GraphNode node in _selectedNodes)
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
