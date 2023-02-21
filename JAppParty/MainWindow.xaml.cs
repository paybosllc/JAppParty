using JAppParty.Helpers;
using Windows.UI;

namespace JAppParty;

public sealed partial class MainWindow : WindowEx
{
    private Color m_blueish = Color.FromArgb(0xFF, 0x40, 0x40, 0xFF);
    private Color m_yellowish = Color.FromArgb(0xFF, 0xFC, 0xAE, 0x1E);
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        //AppWindow.TitleBar.ForegroundColor = m_yellowish;
        //AppWindow.TitleBar.BackgroundColor = m_blueish;
        //AppWindow.TitleBar.ButtonForegroundColor = m_yellowish;
        //AppWindow.TitleBar.ButtonBackgroundColor = m_blueish;

    }
}
