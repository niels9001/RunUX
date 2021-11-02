using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace RunUX
{
    public sealed partial class SearchControl : UserControl
    {
        ObservableCollection<SearchResult> allSearchResults;
        ObservableCollection<SearchResult> filteredResults;
        ObservableCollection<SearchResult> recentResults;
        ObservableCollection<Plugin> plugins;
        int ResultCount = 0;
        public SearchControl()
        {
            this.InitializeComponent();
            allSearchResults = new ObservableCollection<SearchResult>();
            filteredResults = new ObservableCollection<SearchResult>();
            recentResults = new ObservableCollection<SearchResult>();
            plugins = new ObservableCollection<Plugin>();

            PopulateSearchResults();
            GetSearchResultsGroupedAsync();
        }

        private void PopulateSearchResults()
        {
            allSearchResults.Add(new SearchResult() { Name = "Calendar", Icon = "ms-appx:///Assets/Icons/Calendar.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Camera", Icon = "ms-appx:///Assets/Icons/Camera.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Excel", Icon = "ms-appx:///Assets/Icons/Excel.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Feedback", Icon = "ms-appx:///Assets/Icons/Feedback.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "File Explorer", Icon = "ms-appx:///Assets/Icons/FileExplorer.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Groove", Icon = "ms-appx:///Assets/Icons/Groove.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Mail", Icon = "ms-appx:///Assets/Icons/Mail.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Mobile", Icon = "ms-appx:///Assets/Icons/Mobile.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Movies", Icon = "ms-appx:///Assets/Icons/Movies.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "OneDrive", Icon = "ms-appx:///Assets/Icons/OneDrive.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "OneNote", Icon = "ms-appx:///Assets/Icons/OneNote.png", Type = SearchResultType.Apps });

            allSearchResults.Add(new SearchResult() { Name = "Photos", Icon = "ms-appx:///Assets/Icons/Photos.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "PowerPoint", Icon = "ms-appx:///Assets/Icons/PowerPoint.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "SharePoint", Icon = "ms-appx:///Assets/Icons/SharePoint.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Skype", Icon = "ms-appx:///Assets/Icons/Skype.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Solitaire", Icon = "ms-appx:///Assets/Icons/Solitaire.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Teams", Icon = "ms-appx:///Assets/Icons/Teams.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Weather", Icon = "ms-appx:///Assets/Icons/Weather.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Whiteboard", Icon = "ms-appx:///Assets/Icons/Whiteboard.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Word", Icon = "ms-appx:///Assets/Icons/Word.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Yammer", Icon = "ms-appx:///Assets/Icons/Yammer.png", Type = SearchResultType.Apps });
            allSearchResults.Add(new SearchResult() { Name = "Wallpaper1.jpg", Icon = "ms-appx:///Assets/Icons/Wallpaper.jpg", Type = SearchResultType.Files });
            allSearchResults.Add(new SearchResult() { Name = "Wallpaper2.jpg", Icon = "ms-appx:///Assets/Icons/Wallpaper2.jpg", Type = SearchResultType.Files });
            allSearchResults.Add(new SearchResult() { Name = "Wallpaper3.jpg", Icon = "ms-appx:///Assets/Icons/Wallpaper3.jpg", Type = SearchResultType.Files });
            allSearchResults.Add(new SearchResult() { Name = "Wallpaper4.jpg", Icon = "ms-appx:///Assets/Icons/Wallpaper4.jpg", Type = SearchResultType.Files });
            allSearchResults.Add(new SearchResult() { Name = "5 * 5", Icon = "ms-appx:///Assets/Icons/Wallpaper5.jpg", Type = SearchResultType.Calculator });

            recentResults.Add(new SearchResult() { Name = "10 m in ft", Icon = "\uE1D0" });
            recentResults.Add(new SearchResult() { Name = "*.pptx", Icon = "\uE721" });
            recentResults.Add(new SearchResult() { Name = "visual studio", Icon = "\uE721" });
            recentResults.Add(new SearchResult() { Name = "windows 11", Icon = "\uE12B" });

            plugins.Add(new Plugin() { Name = "Calculator", Description = "= 2+2", ActivationKey = "=" });
            plugins.Add(new Plugin() { Name = "URLs" , Description = "//microsoft.com", ActivationKey = "//" });
            plugins.Add(new Plugin() { Name = "Shell command", Description = "> ping localhost", ActivationKey = ">" });
            plugins.Add(new Plugin() { Name = "Unit converter", Description = "%% 10 ft in m", ActivationKey = "%" });
            plugins.Add(new Plugin() { Name = "Registry keys", Description = " :hkcu", ActivationKey = ":" });
            plugins.Add(new Plugin() { Name = "Windows services", Description = "$ Add/Remove Programs", ActivationKey = "!" });
        }

        public void GetSearchResultsGroupedAsync()
        {
            var query = from item in filteredResults
                        group item by item.Type into g
                        orderby g.Key
                        select new SearchResultList(g) { Key = g.Key };
         
            var resultsList = new ObservableCollection<SearchResultList>(query);
            ResultCount = resultsList.Count;
            SearchResultsCVS.Source = resultsList;
    
        }


        private bool Filter(SearchResult result, string character)
        {
            return result.Name.Contains(character, StringComparison.InvariantCultureIgnoreCase);
        }


        private void Remove_NonMatching(IEnumerable<SearchResult> filteredData)
        {
            for (int i = filteredResults.Count - 1; i >= 0; i--)
            {
                var item = filteredResults[i];
                // If contact is not in the filtered argument list, remove it from the ListView's source.
                if (!filteredData.Contains(item))
                {
                    filteredResults.Remove(item);
                }
            }
        }

        private void AddBack_Contacts(IEnumerable<SearchResult> filteredData)
        {
            foreach (var item in filteredData)
            {
                // If item in filtered list is not currently in ListView's source collection, add it back in
                if (!filteredResults.Contains(item))
                {
                    filteredResults.Add(item);
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text.Length > 0)
            {
                RecentPane.Visibility = Visibility.Collapsed;
                ResultsList.Visibility = Visibility.Visible;
                string characater = SearchBox.Text.Substring(0);
                var filtered = allSearchResults.Where(contact => Filter(contact, characater));
                Remove_NonMatching(filtered);
                AddBack_Contacts(filtered);
                GetSearchResultsGroupedAsync();

                if (ResultCount > 0)
                {
                    DetailsPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    DetailsPanel.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                RecentPane.Visibility = Visibility.Visible;
                ResultsList.Visibility = Visibility.Collapsed;
                DetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = ResultsPresenter.ContentTemplateSelector;
            ResultsPresenter.ContentTemplateSelector = null;
            ResultsPresenter.ContentTemplateSelector = x;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BackdropShadow.Receivers.Add(ShadowCastingGrid);
      
        }

        private void PluginListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Plugin selectedPlugin = e.ClickedItem as Plugin;
            SearchBox.Text = selectedPlugin.ActivationKey;
        }

        private void RecentListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SearchResult selectedRecentSearchResult = e.ClickedItem as SearchResult;
            SearchBox.Text = selectedRecentSearchResult.Name;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }

    public class SearchResultList : List<object>
    {
        public SearchResultList(IEnumerable<object> items) : base(items)
        {
        }
        public object Key { get; set; }
    }


    public class SearchResult
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public SearchResultType Type { get; set; }
    }

    public class Plugin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActivationKey { get; set; }
    }

    public enum SearchResultType
    {
        Running,
        Apps,
        Files,
        Calculator
    }
}
