using Godot;
using System;

public partial class Game : Control
{
    [ExportCategory("Quest Data")]
    [Export]
    public QuestResource Quest;
    [Export]
    public DataManager _dataManager;

    [ExportCategory("UI Elements")]
    [Export]
    public Label labelQuestStatus;
    [Export]
    public Label labelQuestPoints;
    [Export]
    public Label labelQuestObjective;
    [Export]
    public Button buttonStartQuest;
    [Export]
    public Button buttonAddPoint;

    private QuestResource _quest;
    private QuestManager _questManager = QuestManager.GetInstance(); // (QuestManager)Engine.GetSingleton("QuestManager");
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

        if (_dataManager == null)
        {
            GD.PrintErr("No DataManager assigned!");
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

        if (labelQuestObjective == null)
        {
            GD.PrintErr("Missing node Label QuestObjective");
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

        labelQuestObjective.Text = "Objective: None";

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
            _numPoints = 0;
            _questStatusText = $"{qr.Name} - {qr.Description}";
        };

        _questManager.QuestObjectiveAdded += (QuestResource qr, QuestObjective qo) =>
        {
            GD.Print("Quest objective added");
            labelQuestObjective.Text = "Objective: " + qo.Description;
        };

        _questManager.QuestObjectiveCompleted += (QuestResource qr, QuestObjective qo) =>
        {
            GD.Print("Quest objective completed");
            labelQuestObjective.Text = "Objective: None";
        };

        _questManager.QuestCompleted += (QuestResource qs) =>
        {
            GD.Print("Quest completed");
            _questStatusText = "Quest completed";
        };
    }

    public override void _Process(double delta)
    {
        if (Quest == null || _dataManager == null)
        {
            return;
        }

        if (labelQuestStatus == null || labelQuestPoints == null || labelQuestObjective == null)
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
