using JAppParty.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace JAppParty.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    readonly StackPanel _titlesStackPanel = new();
    readonly List<TextBox> _textBoxes = new();

    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
        ViewModel.Initialize();
        DisplayCategories();
    }

    public void ConfigureTitles()
    {

        var titles = ViewModel.Titles;
        _titlesStackPanel.Children.Clear();
        _textBoxes.Clear();

        foreach (var title in titles)
        {
            var index = 0;
            TextBox textBox = new()
            {
                Name = "textBox" + index.ToString(),
                Text = title,
                Margin = new Thickness(0, 10, 40, 0),
            };
            _textBoxes.Add(textBox);
            _titlesStackPanel.Children.Add(textBox);
        }
    }

    public void DisplayCategories()
    {
        TextBlock questionsTextBlock = new()
        {
            Text = "Number of Questions",
            FontSize = 36,
        };

        ComboBox comboBox = new ComboBox();
        comboBox.Items.Add("4");
        comboBox.Items.Add("5");
        comboBox.Items.Add("6");
        comboBox.SelectedValue = string.IsNullOrEmpty(ViewModel.NumberOfQuestions.ToString()) ? "6" : ViewModel.NumberOfQuestions.ToString();

        comboBox.SelectionChanged += (sender, args) =>
        {
            var item = comboBox.SelectedItem as string;
            ViewModel.NumberOfQuestions = Convert.ToInt32(item);
            ViewModel.SaveNumberOfQuestions();
        };

        TextBlock titleTextBlock = new()
        {
            Text = "Game Categories",
            FontSize = 36,
        };

        CategoryPanel.Children.Add(questionsTextBlock);
        CategoryPanel.Children.Add(comboBox);
        CategoryPanel.Children.Add(titleTextBlock);

        ConfigureTitles();
        CategoryPanel.Children.Add(_titlesStackPanel);

        StackPanel buttonStackPanel = new()
        {
            Name = "ButtonStackPanel",
            Orientation = Orientation.Horizontal,
        };

        Button addButton = new() { Content = "+", Width = 40, Margin = new Thickness(10) };
        Button subButton = new() { Content = "-", Width = 40, Margin = new Thickness(10) };
        Button saveButton = new() { Content = "Save", Margin = new Thickness(10) };

        addButton.Click += (s, e) =>
        {
            ViewModel.AddCategory();
            ConfigureTitles();
        };

        subButton.Click += (s, e) =>
        {
            ViewModel.RemoveCategory();
            ConfigureTitles();
        };

        saveButton.Click += (s, e) =>
        {
            var savedTitles = string.Empty;
            foreach (var textbox in _textBoxes)
            {
                savedTitles += string.IsNullOrEmpty(savedTitles) ? textbox.Text : "," + textbox.Text;
            }
            ViewModel.OnSave(savedTitles);
            ConfigureTitles();
        };

        buttonStackPanel.Children.Add(addButton);
        buttonStackPanel.Children.Add(subButton);
        buttonStackPanel.Children.Add(saveButton);

        CategoryPanel.Children.Add(buttonStackPanel);

    }
}
