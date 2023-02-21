using JAppParty.Behaviors;
using JAppParty.ViewModels;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI;

namespace JAppParty.Views;

public sealed partial class MainPage : Page
{

    private Color m_blueish = Color.FromArgb(0xFF, 0x40, 0x40, 0xFF);
    private Color m_yellowish = Color.FromArgb(0xFF, 0xFC, 0xAE, 0x1E);
    private Color m_oldlace = Color.FromArgb(0xFF, 0xFD, 0xF5, 0xE6);

    public MainViewModel ViewModel
    {
        get;
    }
    public SettingsViewModel SViewModel
    {
        get;
    }


    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        SViewModel = App.GetService<SettingsViewModel>();

        InitializeComponent();
        SViewModel.Initialize();

        NavigationViewHeaderBehavior.SetHeaderMode(this, NavigationViewHeaderMode.Never);

        Application.Current.Resources["ToggleButtonBackgroundChecked"] = new SolidColorBrush(m_yellowish);
        //Application.Current.Resources["ToggleButtonForeground"] = new SolidColorBrush(m_oldlace);
        //Application.Current.Resources["ToggleButtonForegroundPointerOver"] = new SolidColorBrush(m_oldlace);
        Application.Current.Resources["ToggleButtonBackgroundCheckedPointerOver"] = new SolidColorBrush(m_yellowish);
        Application.Current.Resources["ToggleButtonBackgroundCheckedPressed"] = new SolidColorBrush(m_yellowish);
        Application.Current.Resources["ButtonForegroundPointerOver"] = new SolidColorBrush(m_oldlace);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        Grid jPartyGrid = GenerateGrid();
        ContentArea.Children.Add(jPartyGrid);
    }

    public Grid GenerateGrid() // int rows, int cols, int[,] map)
    {

        var rows = SViewModel.NumberOfQuestions + 1;
        var titles = SViewModel.Titles;
        var cols = titles.Count;

        Grid grid = new();
        grid.Background = new SolidColorBrush(m_blueish);

        // 1.Prepare RowDefinitions
        for (int i = 0; i < rows + 1; i++)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(row);
        }

        // 2.Prepare ColumnDefinitions
        for (int j = 0; j < cols; j++)
        {
            var column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(column);
        }

        // set the titles
        for (int j = 0; j < cols; j++)
        {
            TextBlock textBlock = new()
            {
                Text = titles[j],
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(m_oldlace),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(textBlock);
            Grid.SetColumn(textBlock, j);
            Grid.SetRow(textBlock, 0); // Set row too!
        }

        // 3.Add each item and set row and column.
        for (int i = 1; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Button button = new()
                {
                    Content = (i * 200).ToString(),
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(m_yellowish),
                    Background = new SolidColorBrush(m_blueish),
                };

                grid.Children.Add(button);
                Grid.SetColumn(button, j);
                Grid.SetRow(button, i); // Set row too!

                StackPanel stackPanel = new()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Orientation = Orientation.Horizontal,
                    Visibility = Visibility.Collapsed
                };

                ToggleButton tbA = new()
                {
                    Content = "A",
                    FontSize = 40,
                    FontWeight = FontWeights.Bold,
                    //Foreground = new SolidColorBrush(m_yellowish),
                    Tag = i,
                    Width = 80,
                    Margin = new Thickness(4)
                };

                ToggleButton tbB = new()
                {
                    Content = "B",
                    FontSize = 40,
                    FontWeight = FontWeights.Bold,
                    //Foreground = new SolidColorBrush(m_yellowish),
                    Tag = i,
                    Width = 80,
                    Margin = new Thickness(4),
                };

                stackPanel.Children.Add(tbA);
                stackPanel.Children.Add(tbB);

                grid.Children.Add(stackPanel);
                Grid.SetColumn(stackPanel, j);
                Grid.SetRow(stackPanel, i); // Set row too!

                button.Click += (e, args) =>
                {
                    button.Visibility = Visibility.Collapsed;
                    stackPanel.Visibility = Visibility.Visible;
                };

                tbA.Click += (e, args) =>
                {
                    var toggle = e as ToggleButton;
                    int value;
                    var succeded = int.TryParse(tbA.Tag.ToString(), out value);
                    if (succeded)
                    {
                        var score = value * 200;
                        ViewModel.ScoreTeamA += (toggle?.IsChecked == true) ? score : -score;
                    }

                    tbB.IsEnabled = !(toggle?.IsChecked == true);
                };

                tbB.Click += (e, args) =>
                {
                    var toggle = e as ToggleButton;
                    int value;
                    var succeded = int.TryParse(tbA.Tag.ToString(), out value);
                    if (succeded)
                    {
                        var score = value * 200;
                        ViewModel.ScoreTeamB += (toggle?.IsChecked == true) ? score : -score;
                    }

                    tbA.IsEnabled = !(toggle?.IsChecked == true);
                };
            }
        }

        TextBlock textBlockATitle = new()
        {
            Text = "Score A",
            FontSize = 60,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Foreground = new SolidColorBrush(m_oldlace),
        };

        Grid.SetColumn(textBlockATitle, 0);
        Grid.SetRow(textBlockATitle, rows); // Set row too!

        TextBlock textBlockA = new()
        {
            Text = ViewModel.ScoreTeamA.ToString(),
            FontSize = 60,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Foreground = new SolidColorBrush(m_oldlace),
            Name = "TextBlockScoreA"
        };

        Grid.SetColumn(textBlockA, 1);
        Grid.SetRow(textBlockA, rows); // Set row too!

        Binding bindingA = new()
        {
            Path = new PropertyPath("ScoreTeamA"),
            Source = ViewModel,
        };

        textBlockA.SetBinding(TextBlock.TextProperty, bindingA);

        TextBlock textBlockBTitle = new()
        {
            Text = "Score B",
            FontSize = 60,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Foreground = new SolidColorBrush(m_oldlace),
        };

        Grid.SetColumn(textBlockBTitle, cols - 2);
        Grid.SetRow(textBlockBTitle, rows); // Set row too!


        TextBlock textBlockB = new()
        {
            Text = ViewModel.ScoreTeamB.ToString(),
            FontSize = 60,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Foreground = new SolidColorBrush(m_oldlace),
            Name = "TextBlockScoreB"
        };

        Binding bindingB = new()
        {
            Path = new PropertyPath("ScoreTeamB"),
            Source = ViewModel,
        };

        textBlockB.SetBinding(TextBlock.TextProperty, bindingB);

        Grid.SetColumn(textBlockB, cols - 1);
        Grid.SetRow(textBlockB, rows); // Set row too!

        grid.Children.Add(textBlockATitle);
        grid.Children.Add(textBlockBTitle);

        grid.Children.Add(textBlockA);
        grid.Children.Add(textBlockB);

        return grid;
    }
}



