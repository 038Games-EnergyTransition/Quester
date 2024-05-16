using Godot;
using Godot.Collections;
using System;

public partial class DataManager : Node
{
    [Signal]
    public delegate void DataChangedEventHandler(string key, Variant value);

    private Dictionary<string, Variant> _data = new Dictionary<string, Variant>();

    private static DataManager _instance;

    public override void _Ready()
    {
        QuestManager.Instance.ConditionQueryRequested += (string type, string key, Variant value, QuestCondition requester) =>
        {
            if (type == "variable")
            {
                if (CompareValue(key, value))
                {
                    requester.Completed = true;
                }
            }
        };

        QuestManager.Instance.ActionQueryRequested += (string type, string key, Variant value, QuestAction requester) =>
        {
            if (type == "variable")
            {
                SetValue(key, value);
            }
        };
    }

    private bool CompareValue(string key, Variant value)
    {
        if (value.VariantType == Variant.Type.Float)
        {
            return (float)value % 1 == 0 ? GetValue(key).Equals((int)value) : GetValue(key).Equals(value);
        }

        return GetValue(key).Equals(value);
    }

    public void SetValue(string key, Variant value)
    {
        if (_data.ContainsKey(key))
        {
            _data[key] = value;
        }
        else
        {
            _data.Add(key, value);
        }

        EmitSignal(SignalName.DataChanged, key, value);
    }

    public Variant GetValue(string key)
    {
        if (_data.ContainsKey(key))
        {
            return _data[key];
        }
        return new Variant();
    }

    public static DataManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DataManager();
        }

        return _instance;
    }
}
