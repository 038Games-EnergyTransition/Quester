using Godot;
using Godot.Collections;

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
    public Label labelQuestObjective;
    [Export]
    public Button buttonStartQuest;

    private QuestResource _quest;
    private QuestManager _questManager = QuestManager.Instance;
    private string _questStatusText;
    private Array<QuestObjective> _activeObjectives = new Array<QuestObjective>();
    private bool _questStartClicked = false;

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

        if (missing)
        {
            return;
        }

        _quest = (QuestResource)Quest.Instantiate();

        _questStatusText = "Quest not started";

        buttonStartQuest.Pressed += () =>
        {
            _questManager.StartQuest(_quest);
            _questStartClicked = true;
        };

        _questManager.QuestStarted += (QuestResource qr) =>
        {
            GD.Print("Quest started");
            _questStatusText = $"{qr.Name} - {qr.Description}";
        };

        _questManager.QuestObjectiveAdded += (QuestResource qr, QuestObjective qo) =>
        {
            GD.Print("Quest objective added");
            if (!_activeObjectives.Contains(qo))
                _activeObjectives.Add(qo);
        };

        _questManager.QuestObjectiveCompleted += (QuestResource qr, QuestObjective qo) =>
        {
            GD.Print("Quest objective completed");
            if (_activeObjectives.Contains(qo))
                _activeObjectives.Remove(qo);
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

        if (labelQuestStatus == null || labelQuestObjective == null)
        {
            return;
        }

        if (buttonStartQuest == null)
        {
            return;
        }

        labelQuestStatus.Text = _questStatusText;

        buttonStartQuest.Disabled = _quest.Started || _questStartClicked;
        buttonStartQuest.Text = _quest.Started ? "Quest in progress..." : (_questStartClicked ? "Quest done" : "Start Quest");

        if (_activeObjectives.Count > 0)
        {
            string desc = "";
            foreach (QuestObjective qo in _activeObjectives)
            {
                desc += qo.Description + " | ";
            }
            desc = desc.Substring(0, desc.Length - 3);
            labelQuestObjective.Text = $"Objective: {desc}";
        } else
        {
            labelQuestObjective.Text = "Objective: None";
        }

        _quest.Update();
    }
}
