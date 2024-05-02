using Godot;
using System;

public partial class Game : Control
{
    [ExportCategory("Quest Data")]
    [Export]
    public QuestResource Quest;

    [ExportCategory("UI Elements")]
    [Export]
    public Label labelQuestStatus;
    [Export]
    public Label labelQuestPoints;
    [Export]
    public Button buttonStartQuest;
    [Export]
    public Button buttonAddPoint;

    private QuestResource _quest;
    private QuestManager _questManager = QuestManager.GetInstance(); // (QuestManager)Engine.GetSingleton("QuestManager");
    private DataManager _dataManager = DataManager.GetInstance();
    private string _questStatusText;
    private int _numPoints = 0;

    public override void _Ready()
    {
        bool missing = false;
        if (Quest == null)
        {
            GD.PrintErr("No quest assigned!");
            missing = true;
        }
        if (labelQuestStatus == null)
        {
            GD.PrintErr("Missing node Label QuestStatus");
            missing = true;
        }

        if (labelQuestPoints == null)
        {
            GD.PrintErr("Missing node Label QuestPoints");
            missing = true;
        }

        if (buttonStartQuest == null)
        {
            GD.PrintErr("Missing node Button StartQuest");
            missing = true;
        }

        if (buttonAddPoint == null)
        {
            GD.PrintErr("Missing node Button AddPoint");
            missing = true;
        }

        if (missing)
        {
            return;
        }

        _quest = (QuestResource)Quest.Instantiate();

        _questStatusText = "Quest not started";

        buttonStartQuest.Pressed += () =>
        {
            _questManager.StartQuest(_quest);
        };

        buttonAddPoint.Pressed += () =>
        {
            _numPoints++;
            _dataManager.SetValue("target_num_points", _numPoints);
        };


        _questManager.QuestStarted += (QuestResource qr) =>
        {
            GD.Print("Quest started");
            _questStatusText = $"{qr.Name} - {qr.Description}";
        };

        _questManager.QuestObjectiveAdded += (QuestResource qr, QuestObjective qo) =>
        {
            GD.Print("Quest objective added");
        };

        _questManager.QuestObjectiveCompleted += (QuestResource qr, QuestObjective qo) =>
        {
            GD.Print("Quest objective completed");
        };

        _questManager.QuestCompleted += (QuesterSettings qs) =>
        {
            GD.Print("Quest completed");
            _questStatusText = "Quest completed";
        };
    }

    public override void _Process(double delta)
    {
        if (Quest == null)
        {
            return;
        }
        if (labelQuestStatus == null || labelQuestPoints == null)
        {
            return;
        }

        if (buttonStartQuest == null || buttonAddPoint == null)
        {
            return;
        }

        labelQuestStatus.Text = _questStatusText;
        labelQuestPoints.Text = $"{_numPoints} points";

        buttonStartQuest.Disabled = _quest.Started;
        buttonStartQuest.Text = _quest.Started ? "Quest in progress..." : "Start Quest";

        _quest.Update();
    }
}
