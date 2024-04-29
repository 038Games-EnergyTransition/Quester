using Godot;

[Tool]
public partial class QuestEditor : Control
{

	private Button _newButton;
	private Button _openButton;
	private Button _saveButton;
	private Button _addNodeButton;

	private FileDialog _saveFileDialog;
	private FileDialog _loadFileDialog;
	private PopupMenu _addNodeMenu;

	private QuestEditorGraph _graph;

	public override void _Ready()
	{
		_newButton = GetNode<Button>("Content/MarginContainer/Toolbar/NewButton");
		_openButton = GetNode<Button>("Content/MarginContainer/Toolbar/OpenButton");
		_saveButton = GetNode<Button>("Content/MarginContainer/Toolbar/SaveButton");
		_addNodeButton = GetNode<Button>("Content/MarginContainer/Toolbar/AddNodeButton");

		_newButton.Pressed += () => _on_NewButton_pressed();
		_openButton.Pressed += () => _on_OpenButton_pressed();
		_saveButton.Pressed += () => _on_SaveButton_pressed();
		_addNodeButton.Pressed += () => _on_AddNodeButton_pressed();

		_newButton.Icon = EditorInterface.Singleton.GetEditorTheme().GetIcon("New", "EditorIcons");
		_openButton.Icon = EditorInterface.Singleton.GetEditorTheme().GetIcon("Load", "EditorIcons");
		_saveButton.Icon = EditorInterface.Singleton.GetEditorTheme().GetIcon("Save", "EditorIcons");
		_addNodeButton.Icon = EditorInterface.Singleton.GetEditorTheme().GetIcon("Add", "EditorIcons");

		_saveFileDialog = GetNode<FileDialog>("SaveFileDialog");
		_loadFileDialog = GetNode<FileDialog>("LoadFileDialog");
		_addNodeMenu = GetNode<PopupMenu>("AddNodePopupMenu");

		_addNodeMenu.IndexPressed += (long idx) => _addNewNode((int)idx);

		_graph = GetNode<QuestEditorGraph>("Content/QuestGraphEditor");
	}

	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Clear the graph
	/// </summary>
	private void _on_NewButton_pressed()
	{
		_graph.Clear();
	}

	/// <summary>
	/// Open a file dialog to load a quest resource
	/// </summary>
	private void _on_OpenButton_pressed()
	{
		_loadFileDialog.Popup();
	}

	/// <summary>
	/// Open a file dialog to save a quest resource
	/// </summary>
	private void _on_SaveButton_pressed()
	{
		_saveFileDialog.Popup();
	}

	/// <summary>
	/// Open the add node menu at the add node button position
	/// </summary>
	private void _on_AddNodeButton_pressed()
	{
		// ShowAddNodeMenu(GetGlobalMousePosition());
		ShowAddNodeMenu(_addNodeButton.GlobalPosition);
	}

	/// <summary>
	/// Show the add node menu at the given position
	/// </summary>
	/// <param name="position"></param>
	public void ShowAddNodeMenu(Vector2 position)
	{
		_addNodeMenu.PopupOnParent(new Rect2I((Vector2I)position, Vector2I.Zero));
	}

	private void _addNewNode(int idx)
	{
		PackedScene nodeScene = null;
		switch (idx)
		{
			case 0:
				nodeScene = _graph.StartNodeScene;
				break;

			case 1:
				nodeScene = _graph.EndNodeScene;
				break;

			case 2:
				nodeScene = _graph.ObjectiveNodeScene;
				break;
		}

		if (nodeScene == null) {
			return;
		}

		_graph.AddNode(nodeScene, GetGlobalMousePosition());
	}
}
