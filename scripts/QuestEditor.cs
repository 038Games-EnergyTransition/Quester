using Godot;
using System;
using System.IO;

[Tool]
public partial class QuestEditor : Control
{
    public PackedScene EditorActionScene = GD.Load<PackedScene>("res://addons/quester/scenes/EditorAction.tscn");

    private Button _newButton;
	private Button _openButton;
	private Button _saveButton;
	private Button _addNodeButton;

	private FileDialog _saveFileDialog;
	private FileDialog _loadFileDialog;
	private PopupMenu _addNodeMenu;

	private QuestEditorGraph _graph;
	private VBoxContainer _actionlogContainer;

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
        _actionlogContainer = GetNode<VBoxContainer>("ActionLog");

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
			GD.PrintErr($"Error({error}) saving file: " + path);
		}
		else
		{
			_saveFilePath = path;
			EditorInterface.Singleton.GetResourceFilesystem().Scan();

			_logAction($"Quest '{path}' saved!", "Save");
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

        _logAction($"Quest '{path}' loaded!", "Load");
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

        _logAction($"New quest created!", "New");
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
			
			case 4:
				nodeScene = _graph.ActionNodeScene;
				break;
		}

		if (nodeScene == null) {
			return;
		}

		_graph.AddNode(nodeScene, GetGlobalMousePosition());
    }

    /// <summary>
    /// Create a log entry on top of the graph to notify the user of an action
    /// </summary>
    /// <param name="text"></param>
    /// <param name="icon"></param>
    private void _logAction(string text, string icon)
    {
        EditorAction editorAction = (EditorAction)EditorActionScene.Instantiate();
        editorAction.IconName = icon;
        editorAction.ActionText = text;
        _actionlogContainer.AddChild(editorAction);
    }
}
