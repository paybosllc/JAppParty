using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using JAppParty.Contracts.Services;
using JAppParty.Helpers;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace JAppParty.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService? _themeSelectorService;
    private ElementTheme _elementTheme;
    private string? _versionDescription;
    private List<string> _titles = new() { "Title 1", "Title 2", "Title 3", "Title 4", "Title 5" };
    private const int DEFAULT_QUESTIONS = 6;

    public List<string> Titles
    {
        get => _titles;
        set => SetProperty(ref _titles, value);
    }

    public int NumberOfQuestions
    {
        get;
        set;
    }

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => string.IsNullOrEmpty(_versionDescription) ? string.Empty : _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public ICommand? SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public void AddCategory()
    {
        _titles.Add("Title " + (Titles.Count + 1).ToString());
    }
    public void RemoveCategory()
    {
        _titles.RemoveAt(_titles.Count - 1);
    }

    public void SaveNumberOfQuestions()
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        localSettings.Values.Remove("number_of_questions");
        _ = localSettings.Values.TryAdd("number_of_questions", NumberOfQuestions.ToString());
    }
    public void OnSave(string savedTitles)
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        localSettings.Values.Remove("game_titles");
        _ = localSettings.Values.TryAdd("game_titles", savedTitles);

        SaveNumberOfQuestions();
    }

    public void Initialize()
    {
        NumberOfQuestions = DEFAULT_QUESTIONS;
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        object? value;
        var succeeded = localSettings.Values.TryGetValue("game_titles", out value);
        if (succeeded)
        {
            var s = value as string;
            if (!string.IsNullOrEmpty(s))
            {
                _titles = s.Split(',').ToList();
            }
        }

        succeeded = localSettings.Values.TryGetValue("number_of_questions", out value);
        if (succeeded)
        {
            int result;
            var success = int.TryParse(value as string, out result);
            NumberOfQuestions =  success ? result : DEFAULT_QUESTIONS;
        }
    }

    public SettingsViewModel()
    {
        Initialize();
    }

}
