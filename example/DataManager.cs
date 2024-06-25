using System;
using Godot;
using Godot.Collections;

/// <summary>
/// an example implimentation of a data manager used for quests.
/// </summary>
public partial class DataManager : Node
{
    [Signal]
    public delegate void DataChangedEventHandler(string key, Variant value);

    private Dictionary<string, Variant> _data = new Dictionary<string, Variant>();

    private static DataManager _instance;

    public override void _Ready()
    {
        QuestManager.Instance.ConditionQueryRequested += (
            string type,
            string key,
            Variant value,
            QuestCondition requester
        ) =>
        {
            if (type == "variable")
            {
                if (CompareValue(key, value))
                {
                    requester.Completed = true;
                }
            }
        };

        QuestManager.Instance.ActionQueryRequested += (
            string type,
            string key,
            Variant value,
            QuestAction requester
        ) =>
        {
            if (type == "variable")
            {
                SetValue(key, value);
            }
        };
    }

    /// <summary>
    /// Compares the value of a key to a given value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool CompareValue(string key, Variant value)
    {
        if (value.VariantType == Variant.Type.Float)
        {
            return (float)value % 1 == 0
                ? GetValue(key).Equals((int)value)
                : GetValue(key).Equals(value);
        }

        return GetValue(key).Equals(value);
    }

    /// <summary>
    /// Sets a value in the data manager.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
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

    /// <summary>
    /// Gets a value from the data manager.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Variant GetValue(string key)
    {
        if (_data.ContainsKey(key))
        {
            return _data[key];
        }
        return new Variant();
    }

    /// <summary>
    ///gets the current instance of the DataManager
    /// </summary>
    public static DataManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DataManager();
        }

        return _instance;
    }
}
