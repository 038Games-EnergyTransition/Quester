using Godot;
using System;

public partial class Example2Game : Node2D
{

    [Export]
    public DataManager _dataManager;
	
	private QuestManager _questManager = QuestManager.GetInstance();

	public override void _Ready()
	{
		_dataManager.DataChanged += (string key, Variant value) =>
		{
			GD.Print("Data changed: " + key + " = " + value);
		};

		_dataManager.SetValue("game_state", 0);
	}

	public override void _Process(double delta)
	{
		_questManager.UpdateQuests();
	}
}
