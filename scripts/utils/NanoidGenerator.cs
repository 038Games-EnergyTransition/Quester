using Godot;

/// <summary>
/// A class that generates random IDs.
/// </summary>
public static partial class NanoidGenerator
{
    public const string ALPHABET =
        "useandom-26T198340PX75pxJACKVERYMINDBUSHWOLF_GQZbfghjklqvwyzrict";
    public const int DEFAULT_LENGTH = 21;

    /// <summary>
    /// Generates a random ID.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Generate(int length = DEFAULT_LENGTH)
    {
        string id = "";
        for (int i = 0; i < length; i++)
        {
            id += ALPHABET[(int)(GD.Randi() % ALPHABET.Length)];
        }
        return id;
    }
}
