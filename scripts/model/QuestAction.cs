using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents an action in a quest.
/// </summary>
[Tool]
public partial class QuestAction : QuestNode
{
    public enum ValueType
    {
        BOOLEAN,
        STRING,
        INTEGER,
    }

    [Export]
    public string Type;

    [Export]
    public string Key;

    public Variant Value
    {
        get { return GetMeta("Value", false); }
        set { SetMeta("Value", value); }
    }

    public new void Update()
    {
        if (!Completed)
        {
            // DONE: Emit condition query requested signal
            QuestManager.Instance.EmitSignal(
                QuestManager.SignalName.ActionQueryRequested,
                Type,
                Key,
                Value,
                this
            );
            Completed = true;
        }
    }
}
