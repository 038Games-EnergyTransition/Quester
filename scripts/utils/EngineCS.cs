using System;
using Godot;
using Godot.Collections;

/// <summary>
/// A class that contains engine-wide utility functions for working with metadata.
/// </summary>
public static partial class EngineCS
{
    public static Dictionary Metadata { get; private set; } = new Dictionary();

    /// <summary>
    /// Sets a metadata key-value pair.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetMeta(string key, Variant value)
    {
        Metadata[key] = value;
    }

    /// <summary>
    /// Gets a metadata value by key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Variant GetMeta(string key)
    {
        if (Metadata.ContainsKey(key))
        {
            return Metadata[key];
        }
        return -1;
    }

    /// <summary>
    /// Removes a metadata key-value pair.
    /// </summary>
    /// <param name="key"></param>
    public static void RemoveMeta(string key)
    {
        if (Metadata.ContainsKey(key))
        {
            Metadata.Remove(key);
        }
    }
}
