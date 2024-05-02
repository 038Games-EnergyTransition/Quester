using Godot;
public partial class IncrementButton : Button
{

    [Export]
    public string TargetQuestKey;
    [Export]
    public Label labelValue;
    [Export]
    public DataManager _dataManager;


    private QuestManager _questManager = QuestManager.GetInstance();

    public override void _Ready()
    {
        _updateValue(0);
        Pressed += () =>
        {
            _updateValue((int)_dataManager.GetValue(TargetQuestKey) + 1);
        };
    }

    private void _updateValue(int value)
    {
        _dataManager.SetValue(TargetQuestKey, value);
        labelValue.Text = value.ToString();
    }
}

