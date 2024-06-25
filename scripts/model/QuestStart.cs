using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents the start of a quest.
/// </summary>
[Tool]
public partial class QuestStart : QuestNode
{
    [Export]
    public string Name;

    [Export]
    public string Description;
}
