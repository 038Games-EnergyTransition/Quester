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

    public Variant Value {
        get {
            return GetMeta("Value");
        }
        set {
            SetMeta("Value", value);
        }
    }

    public new void Update()
    {
        if (!Completed) {
            // TODO: Emit condition query requested signal
            // Questify.condition_query_requested.emit(type, key, value, self)
        }
    }
}