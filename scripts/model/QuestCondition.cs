using Godot;
using Godot.Collections;

[Tool]
public partial class QuestCondition : QuestNode
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
        get
        {
            return GetMeta("Value");
        }
        set
        {
            SetMeta("Value", value);
        }
    }

    public new void Update()
    {
        if (!Completed)
        {

            // DONE: Emit condition query requested signal
            QuestManager.GetInstance().EmitSignal(QuestManager.SignalName.ConditionQueryRequested, Type, Key, Value, this);
        }
    }
}