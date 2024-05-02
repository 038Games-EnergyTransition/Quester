#if TOOLS
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{

	QuestEditor MainPanelWindow;
	PackedScene MainPanel = ResourceLoader.Load<PackedScene>("res://addons/quester/scenes/QuestEditorView.tscn");

	public override void _EnterTree()
	{
		AddAutoloadSingleton("QuestManager", "res://addons/quester/QuestManager.cs");

        MainPanelWindow = (QuestEditor)MainPanel.Instantiate();
		EditorInterface.Singleton.GetEditorMainScreen().AddChild(MainPanelWindow);

		EngineCS.SetMeta("QuesterPlugin", this);

		_MakeVisible(false);
	}

	public override void _ExitTree()
	{
		EditorInterface.Singleton.GetEditorMainScreen().RemoveChild(MainPanelWindow);

		EngineCS.RemoveMeta("QuesterPlugin");

		RemoveAutoloadSingleton("QuestManager");

        MainPanelWindow.Free();
	}
	public override bool _HasMainScreen()
	{
		return true;
	}

	public override void _MakeVisible(bool visible)
	{
		if (MainPanelWindow != null)
		{
			MainPanelWindow.Visible = visible;
		}
	}

	public override string _GetPluginName()
	{
		return "Quester";
	}

	public override Texture2D _GetPluginIcon()
	{
		return EditorInterface.Singleton.GetEditorTheme().GetIcon("Zoom", "EditorIcons");
	}
}
#endif
