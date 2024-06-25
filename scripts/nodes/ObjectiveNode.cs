#if TOOLS
using Godot;
using Godot.Collections;

/// <summary>
/// A node that represents an objective in the quest graph.
/// </summary>
[Tool]
public partial class ObjectiveNode : QuestGraphNode
{
    public string QuestDescription;
    public bool Optional;

    [Export]
    protected CheckBox OptionalCheckBox;

    [Export]
    protected TextEdit descriptionTextEdit;

    [Export]
    protected MetadataEditor metadataEditor;

    public override void _Ready()
    {
        base._Ready();

        descriptionTextEdit.TextChanged += _onDescriptionTextChanged;
        OptionalCheckBox.Toggled += _onOptionalCheckBoxToggled;
    }

    /// <summary>
    /// Returns a new instance of the model.
    /// </summary>
    /// <returns></returns>
    protected override QuestNode _getModel()
    {
        return new QuestObjective();
    }

    /// <summary>
    /// Sets the properties of the model and transfers the metadata.
    /// </summary>
    /// <param name="node"></param>
    protected override void _setModelProperties(QuestNode node)
    {
        QuestObjective objective = (QuestObjective)node;
        objective.Description = QuestDescription;
        objective.Optional = Optional;
        foreach (string key in GetMetaList())
        {
            objective.SetMeta(key, GetMeta(key));
        }
    }

    /// <summary>
    /// Gets the properties of the model and transfers the metadata.
    /// </summary>
    /// <param name="node"></param>
    protected override void _getModelProperties(QuestNode node)
    {
        QuestObjective objective = (QuestObjective)node;
        QuestDescription = objective.Description;
        Optional = objective.Optional;
        descriptionTextEdit.Text = QuestDescription;
        OptionalCheckBox.ButtonPressed = Optional;
        foreach (string key in objective.GetMetaList())
        {
            SetMeta(key, objective.GetMeta(key));
        }
        metadataEditor.Update();
    }

    /// <summary>
    /// Updates the description of the objective.
    /// </summary>
    private void _onDescriptionTextChanged()
    {
        QuestDescription = descriptionTextEdit.Text;
    }

    /// <summary>
    /// Updates the optional state of the objective.
    /// </summary>
    /// <param name="toggled"></param>
    private void _onOptionalCheckBoxToggled(bool toggled)
    {
        Optional = toggled;
    }
}
#endif