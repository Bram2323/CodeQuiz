using Godot;

public partial class Choice : Node
{
    [Export]
    Button ToggleButton, TextButton;

    public delegate void ChoiceToggled(Choice choice, bool enabled);
    public event ChoiceToggled Toggled;

    bool round = false;


    public override void _Ready()
    {
        ToggleButton.Toggled += OnToggled;
    }


    public void SetText(string text)
    {
        TextButton.Text = text;
    }

    public void SetRound(bool enabled)
    {
        round = enabled;
        UpdateTheme();
    }

    public bool IsPressed()
    {
        return ToggleButton.ButtonPressed;
    }

    public void SetPressed(bool pressed)
    {
        ToggleButton.ButtonPressed = pressed;
        UpdateTheme();
    }

    public void SetDisabled(bool disabled)
    {
        ToggleButton.Disabled = disabled;
    }

    public void SetTextActive(bool active)
    {
        TextButton.Disabled = active;
    }

    void OnToggled(bool toggledOn)
    {
        //TextButton.ButtonPressed = toggledOn;
        Toggled?.Invoke(this, toggledOn);
        UpdateTheme();
    }

    void UpdateTheme()
    {
        string theme;

        if (IsPressed()) theme = "ToggleButtonActive";
        else theme = "ToggleButton";

        if (round) theme += "Round";

        ToggleButton.ThemeTypeVariation = theme;
    }
}
