using System;
using System.Collections.Generic;
using System.IO;
using Godot;

///<summary>
/// A class responsible for managing active quests, sending events and checking if quests are completed.
///</summary>
[Tool]
public partial class QuestManager : Node
{
    [Signal]
    public delegate void ConditionQueryRequestedEventHandler(
        string type,
        string key,
        Variant value,
        QuestCondition requester
    );

    [Signal]
    public delegate void ActionQueryRequestedEventHandler(
        string type,
        string key,
        Variant value,
        QuestAction requester
    );

    [Signal]
    public delegate void QuestStartedEventHandler(QuestResource quest);

    [Signal]
    public delegate void QuestObjectiveAddedEventHandler(
        QuestResource quest,
        QuestObjective objective
    );

    [Signal]
    public delegate void QuestObjectiveCompletedEventHandler(
        QuestResource quest,
        QuestObjective objective
    );

    [Signal]
    public delegate void QuestCompletedEventHandler(QuestResource quest);

    private List<QuestResource> _quests = new List<QuestResource>();
    private Timer _questUpdateTimer;

    private static QuestManager _instance;
    public static QuestManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new QuestManager();
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    public override void _Ready()
    {
        if (QuesterSettings.get_polling_enabled())
        {
            _setupTimer();
            _questUpdateTimer.Connect("timeout", new Callable(this, MethodName.UpdateQuests));
        }
    }

    /// <summary>
    /// Sets up the timer thats used to pol quest progression.
    /// </summary>
    private void _setupTimer()
    {
        _questUpdateTimer = new Timer
        {
            WaitTime = QuesterSettings.get_polling_interval(),
            OneShot = false
        };
    }

    /// <summary>
    /// Starts a quest and adds it to the list of active quests.
    /// </summary>
    public void StartQuest(QuestResource quest)
    {
        _quests.Add(quest);
        quest.Start();
    }

    ///<summary>
    /// Goes over every quest and updates them by calling their Update method.
    /// Invoked by <see cref="_questUpdateTimer"/>
    ///</summary>
    public void UpdateQuests()
    {
        for (int i = 0; i < _quests.Count; i++)
        {
            _quests[i].Update();
        }
    }

    ///<summary>
    /// Removes a specific quest from the list of active quests.
    ///<param name="quest">The quest to remove from the list of active quests.
    ///</summary>
    public void RemoveQuest(QuestResource quest)
    {
        _quests.Remove(quest);
    }

    ///<summary>
    /// Sets all active states in the nodes of this quest to false.This however doesnt reset the values it checks for.
    ///<param name="quest">The quest to reset
    ///</summary>

    public void ResetQuest(QuestResource quest)
    {
        quest.Reset();
    }

    ///<summary>
    /// Clears the list of active quests.
    ///</summary>
    public void Clear()
    {
        _quests.Clear();
    }

    ///<summary>
    /// returns a list of all quests.
    ///</summary>
    public List<QuestResource> GetQuests()
    {
        return _quests;
    }

    ///<summary>
    /// Returns a list of all completed quests.
    ///</summary>
    public List<QuestResource> GetCompletedQuests()
    {
        return _quests.FindAll(quest => quest.Completed);
    }

    ///<summary>
    /// Returns a list serialized quests.
    ///</summary>
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

    ///<summary>
    /// Takes a list of serialized quests, deserializes them and ads them to the active quests.
    ///</summary>
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

    ///<summary>
    /// Toggles the update polling on or off.
    ///</summary>
    public void ToggleUpdatePolling(bool value)
    {
        if (QuesterSettings.get_polling_enabled())
        {
            _questUpdateTimer.Paused = !value;
        }
    }
}
