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
        QuestManager.GetInstance().ConditionQueryRequested += (string type, string key, Variant value, QuestCondition requester) =>
        {
            if (type == "variable")
            {
                if (GetValue(key).Equals(value))
                {
                    requester.Completed = true;
                }
            }
        };
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
