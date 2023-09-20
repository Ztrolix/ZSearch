using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Diagnostics;
using System;
using System.Security;
using System.ComponentModel;
using System.Collections.Generic;
using Ionic.Zip;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Security.Policy;

namespace ZSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SearchHistoryFile = "data/search_history.txt";

        public MainWindow()
        {
            InitializeComponent();

            Website.Visibility = Visibility.Visible;
            Search.Visibility = Visibility.Visible;
            SearchBox.Visibility = Visibility.Visible;

            SettingsGrid.Visibility = Visibility.Collapsed;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the URL from the TextBox
            string url = SearchBox.Text;

            SaveToSearchHistory(url, DateTime.Now);

            if (url == "zsearch://chromium")
            {
                Process.Start("ZSearchW.exe");

                return;
            }

            if (url == "zsearch://settings")
            {
                Website.Visibility = Visibility.Collapsed;

                SettingsGrid.Visibility = Visibility.Visible;

                return;
            }

            if (url == "zsearch://experiments")
            {
                MessageBox.Show("This site is currently being made, sorry.", "zsearch://experiments");

                return;
            }

            if (url == "zsearch://")
            {
                MessageBox.Show("This site is currently being made, sorry.", "zsearch://");

                return;
            }

            Website.Visibility = Visibility.Visible;

            try
            {
                // Set the Source property of the WebBrowser to the entered URL
                Website.Source = new Uri(url);

                
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Invalid URL. Please enter a valid URL.");
            }
        }

        private void SaveToSearchHistory(string url, DateTime dateTime)
        {
            try
            {
                // Format the entry as "URL - Date Time"
                string entry = $"{url} - {dateTime}";

                // Append the entry to the search history file
                File.AppendAllText(SearchHistoryFile, entry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to search history: {ex.Message}");
            }
        }

        private void ExitSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsGrid.Visibility = Visibility.Collapsed;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Website.Visibility = Visibility.Collapsed;
            Search.Visibility = Visibility.Collapsed;
            SearchBox.Visibility = Visibility.Collapsed;

            SettingsGrid.Visibility = Visibility.Visible;
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSearchHistory();
        }

        private void DeleteHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteLoadSearchHistory();
        }

        private void DeleteLoadSearchHistory()
        {
            // Load and display the search history in a MessageBox
            try
            {
                string searchHistory = File.ReadAllText(SearchHistoryFile);
                MessageBox.Show("Are you sure you want to delete:\n\n" + searchHistory, "Delete History");
                MessageBox.Show("Removing all traces...", "Delete History");
                File.Delete(SearchHistoryFile);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Search history file not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading search history: {ex.Message}");
            }
        }

        private void LoadSearchHistory()
        {
            // Load and display the search history in a MessageBox
            try
            {
                string searchHistory = File.ReadAllText(SearchHistoryFile);
                MessageBox.Show("Search History:\n\n" + searchHistory, "Search History");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Search history file not found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading search history: {ex.Message}");
            }
        }

        private void HighlightColorChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string hexColorCode = HighlightColorChangeBox.Text;

            try
            {
                // Check if the entered text is a valid hex color code
                if (IsHexColorCode(hexColorCode))
                {
                    // Convert the hex color code to a SolidColorBrush
                    Color color = (Color)ColorConverter.ConvertFromString(hexColorCode);
                    SolidColorBrush brush = new SolidColorBrush(color);

                    // Set the background color of the MainContent grid
                    ExitSettings.BorderBrush = brush;
                    DeleteHistory.BorderBrush = brush;
                    SearchHistory.BorderBrush = brush;
                    HighlightColorChangeBox.BorderBrush = brush;
                    BackgroundColorChangeBox.BorderBrush = brush;
                    HighlightColorChangeButton.BorderBrush = brush;
                    BackgroundColorChangeButton.BorderBrush = brush;
                    Search.BorderBrush = brush;
                    SearchBox.BorderBrush = brush;
                }
                else
                {
                    MessageBox.Show("Invalid hex color code. Please enter a valid hex color code.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid hex color code format. Please enter a valid hex color code.");
            }
        }

        private void BackgroundColorChangeButton_Click(object sender, RoutedEventArgs e)
        {
            string hexColorCode = BackgroundColorChangeBox.Text;

            try
            {
                // Check if the entered text is a valid hex color code
                if (IsHexColorCode(hexColorCode))
                {
                    // Convert the hex color code to a SolidColorBrush
                    Color color = (Color)ColorConverter.ConvertFromString(hexColorCode);
                    SolidColorBrush brush = new SolidColorBrush(color);

                    // Set the background color of the MainContent grid
                    ApplicationWindow.Background = brush;
                }
                else
                {
                    MessageBox.Show("Invalid hex color code. Please enter a valid hex color code.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid hex color code format. Please enter a valid hex color code.");
            }
        }

        private bool IsHexColorCode(string input)
        {
            // Check if the input is a valid hex color code (e.g., "#RRGGBB" or "#AARRGGBB")
            return !string.IsNullOrEmpty(input) && input.Length >= 7 && input[0] == '#' &&
                   System.Text.RegularExpressions.Regex.IsMatch(input.Substring(1), @"^[0-9A-Fa-f]+$");
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Minimize the window
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }
    }
}