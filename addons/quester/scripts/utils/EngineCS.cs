using System;
using Godot;
using Godot.Collections;

public static partial class EngineCS
{
    public static Dictionary Metadata { get; private set; } = new Dictionary();

    public static void SetMeta(string key, Variant value)
    {
        Metadata[key] = value;
    }

    public static Variant GetMeta(string key)
    {
        if (Metadata.ContainsKey(key))
        {
            return Metadata[key];
        }
        return -1;
    }

    public static void RemoveMeta(string key)
    {
        if (Metadata.ContainsKey(key))
        {
            Metadata.Remove(key);
        }
    }
}