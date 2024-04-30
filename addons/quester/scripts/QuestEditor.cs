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

	private string _saveFilePath;

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

		_saveFileDialog.FileSelected += _onSaveFileDialogFileSelected;
		_loadFileDialog.FileSelected += _onLoadFileDialogFileSelected;
		_addNodeMenu.IndexPressed += (long idx) => _addNewNode((int)idx);

		_graph = GetNode<QuestEditorGraph>("Content/QuestGraphEditor");
	}

	/// <summary>
	/// Save the file to the given path
	/// </summary>
	/// <param name="path"></param>
	private void saveFile(string path)
	{
		Error error = _graph.Save(path);
		if (error != Error.Ok)
		{
			GD.PrintErr("Error saving file: " + path);
		}
		else
		{
			_saveFilePath = path;
			EditorInterface.Singleton.GetResourceFilesystem().Scan();
		}
	}

	/// <summary>
	/// Load the file from the given path
	/// </summary>
	/// <param name="path"></param>
	private void loadFile(string path)
	{
		_graph.Load(path);
		_saveFilePath = path;
	}

	/// <summary>
	/// Apply changes to the graph
	/// </summary>
	private void applyChanges()
	{
		if (_graph.GetChildCount() > 0)
		{
			saveChanges();
		}
	}

	/// <summary>
	/// Show a save file dialog if the file path is not set, otherwise save the file
	/// </summary>
	private void saveChanges()
	{
		if (_saveFilePath == null || _saveFilePath == "")
		{
			_saveFileDialog.Popup();
		}
		else
		{
			saveFile(_saveFilePath);
		}
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
		saveChanges();
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
	/// Save the file to the given path
	/// </summary>
	/// <param name="path"></param>
	private void _onSaveFileDialogFileSelected(string path)
	{
		saveFile(path);
	}

	/// <summary>
	/// Load the file from the given path
	/// </summary>
	/// <param name="path"></param>
	private void _onLoadFileDialogFileSelected(string path)
	{
		loadFile(path);
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
			
			case 3:
				nodeScene = _graph.ConditionNodeScene;
				break;
		}

		if (nodeScene == null) {
			return;
		}

		_graph.AddNode(nodeScene, GetGlobalMousePosition());
	}
}
