using Audio;
using System;
using UI;

public class InGameMenuWindowUI : BaseUIWindow
{
    public event Action closeWindow;

    public override void CloseWindow()
    {
        base.CloseWindow();

        closeWindow?.Invoke();
    }
}