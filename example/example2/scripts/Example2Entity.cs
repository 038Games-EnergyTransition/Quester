using Godot;
using System;

public partial class Example2Entity : StaticBody2D
{

	[Export]
	public QuestResource Quest;
	[Export]
    public DataManager _dataManager;
	[Export]
	public string BaseKey;

	[Export]
	public Area2D QuestTrigger;
	[Export]
	public Label QuestLabel;

	private QuestResource _quest;

	private QuestManager _questManager = QuestManager.Instance;

	public override void _Ready()
	{
		_quest = Quest.Instantiate();
		QuestLabel.Text = "";

		QuestTrigger.BodyEntered += (body) =>
		{
			if (body is Example2Player player)
			{
				GD.Print("Player entered trigger");
				_questManager.ResetQuest(_quest);
				_questManager.StartQuest(_quest);
			}
		};

		QuestTrigger.BodyExited += (body) =>
		{
			if (body is Example2Player player)
			{
				GD.Print("Player exited trigger");
				_questManager.ResetQuest(_quest);
			}
		};

		_questManager.QuestStarted += (QuestResource quest) =>
		{
			if (quest == _quest)
			{
				GD.Print($"Quest '{_quest.Name}' started");
			}
		};

		_questManager.QuestObjectiveAdded += (QuestResource quest, QuestObjective objective) =>
		{
			if (quest == _quest)
			{
				GD.Print($"Objective '{objective.Description}' added to quest {_quest.Name}");
			}
		};

		_questManager.QuestObjectiveCompleted += (QuestResource quest, QuestObjective objective) =>
		{
			if (quest == _quest)
			{
				GD.Print($"Objective '{objective.Description}' completed in quest {_quest.Name}");
			}
		};

		_questManager.QuestCompleted += (QuestResource quest) =>
		{
			if (quest == _quest)
			{
				_questManager.ResetQuest(_quest);
				GD.Print($"Quest '{_quest.Name}' completed");
			}
		};

		_dataManager.DataChanged += (string key, Variant value) =>
		{
			if (key == BaseKey + "_speak")
			{
				QuestLabel.Text = (string)value;
			}
		};
	}

	public override void _Process(double delta)
	{
	}
}
