using System;
using System.Collections.Generic;
using System.IO;
using Godot;

[Tool]
public partial class QuestManager : Node
{
    [Signal]
    public delegate void ConditionQueryRequestedEventHandler(string type, string key, Variant value, QuestCondition requester);
    [Signal]
    public delegate void ActionQueryRequestedEventHandler(string type, string key, Variant value, QuestAction requester);
    [Signal]
    public delegate void QuestStartedEventHandler(QuestResource quest);
    [Signal]
    public delegate void QuestObjectiveAddedEventHandler(QuestResource quest, QuestObjective objective);
    [Signal]
    public delegate void QuestObjectiveCompletedEventHandler(QuestResource quest, QuestObjective objective);

    [Signal]
    public delegate void QuestCompletedEventHandler(QuestResource quest);

    private List<QuestResource> _quests = new List<QuestResource>();
    private Timer _questUpdateTimer;

    private static QuestManager _instance;

    public override void _Ready()
    {
        if (QuesterSettings.get_polling_enabled())
        {
            _setupTimer();
            _questUpdateTimer.Connect("timeout", new Callable(this, MethodName.UpdateQuests));
        }
    }

    private void _setupTimer()
    {
        _questUpdateTimer = new Timer
        {
            WaitTime = QuesterSettings.get_polling_interval(),
            OneShot = false

        };
    }

    public void StartQuest(QuestResource quest)
    {
        _quests.Add(quest);
        quest.Start();
    }

    public void UpdateQuests()
    {
        foreach (QuestResource quest in _quests)
        {
            quest.Update();
        }
    }

    public void ResetQuest(QuestResource quest)
    {
        quest.Reset();
    }

    public void Clear()
    {
        _quests.Clear();
    }

    public List<QuestResource> GetQuests()
    {
        return _quests;
    }

    public List<QuestResource> GetCompletedQuests()
    {
        return _quests.FindAll(quest => quest.Completed);
    }

    public List<QuestSerialPart> Serialize()
    {
        List<QuestSerialPart> quests = new List<QuestSerialPart>();
        foreach (QuestResource quest in _quests)
        {
            QuestSerialPart part = new QuestSerialPart
            {
                Path = quest.Name,
                Data = quest.Serialize()
            };
            quests.Add(part);
        }
        return quests;
    }

    public void Deserialize(List<QuestSerialPart> quests)
    {
        Clear();
        foreach (QuestSerialPart part in quests)
        {
            QuestResource quest = ResourceLoader.Load(part.Path) as QuestResource;
            quest.Deserialize(part.Data);
            _quests.Add(quest);
        }
    }
    public void ToggleUpdatePolling(bool value)
    {
        if (QuesterSettings.get_polling_enabled())
        {
            _questUpdateTimer.Paused = !value;
        }
    }

    public static QuestManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new QuestManager();
        }
        return _instance;
    }
}