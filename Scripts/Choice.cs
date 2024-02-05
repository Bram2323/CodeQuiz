using Godot;

public partial class Choice : Node
{
	[Export]
	Button ToggleButton, TextButton;

    public delegate void ChoiceToggled(Choice choice, bool enabled);
    public event ChoiceToggled Toggled;


    public override void _Ready()
    {
        ToggleButton.Toggled += OnToggled;
    }


    public void SetText(string text)
	{
		TextButton.Text = text;
	}

    public bool IsPressed()
    {
        return ToggleButton.ButtonPressed;
    }

    public void SetPressed(bool pressed)
    {
        //ToggleButton.ButtonPressed = pressed;
        ToggleButton.ButtonPressed = pressed;
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
        if (toggledOn) ToggleButton.ThemeTypeVariation = "ToggleButtonActive";
        else ToggleButton.ThemeTypeVariation = "ToggleButton";
    }
}
