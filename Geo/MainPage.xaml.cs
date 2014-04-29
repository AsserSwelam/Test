using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Geo.Resources;
using Geo.ViewModel;
using System.IO.IsolatedStorage;
using System.Windows.Media.Animation;

namespace Geo
{
    public partial class MainPage : PhoneApplicationPage
    {

        private bool isMapFullScrn = false;
        ViewModel.ViewModel _viewModel;

        Storyboard _beamImgDirectionStoryboard;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _viewModel = new ViewModel.ViewModel(myMap);

            this.DataContext = _viewModel;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        //Handle view model property changed instead of binding the values
        void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GraphicsOnMap")
                (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).IsEnabled = _viewModel.GraphicsOnMap;
            else if (e.PropertyName == "StoryBoardDirection")
            {
                RunStoryBoard(_viewModel.StoryBoardDirection);
            }
        }


        //Ask user to access location info
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                // User has opted in or out of Location
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("This app accesses your phone's location. Is that ok?",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }

        }


        //TODO create style triger instead
        private void btnMapFullScrn_Click_1(object sender, RoutedEventArgs e)
        {
            if (!isMapFullScrn)
            {
                mapContent.SetValue(Grid.RowProperty, 0);
                mapContent.SetValue(Grid.RowSpanProperty, 2);

                btnMapFullScrn.Content = "v";

                functionsPivot.Visibility = System.Windows.Visibility.Collapsed;

                isMapFullScrn = true;
            }
            else
            {
                mapContent.SetValue(Grid.RowProperty, 1);
                mapContent.SetValue(Grid.RowSpanProperty, 1);

                btnMapFullScrn.Content = "^";

                functionsPivot.Visibility = System.Windows.Visibility.Visible;

                isMapFullScrn = false;
            }
        }

        private void btnRedSettings_Click_1(object sender, RoutedEventArgs e)
        {
             NavigationService.Navigate(new Uri("/View/RedSetPivotPage.xaml", UriKind.Relative));
             

        }

        private void btnMeasurSet_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MeasurSettPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_OrientationChanged_1(object sender, OrientationChangedEventArgs e)
        {
           
        }

        private void btnFullExtent_MapNav_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.MapFullExtent();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            _viewModel.SetMyLocation();
        }

        private void btnPrevExtent_MapNav_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.GoToPreviousExtent();
        }

        private void btnNextExtent_MapNav_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.GoToNextExtent();
        }

        private void btnClearMapGraphics_Click_1(object sender, EventArgs e)
        {
            _viewModel.ClearMapGraphics();
        }

        private void btnAutoNavBase_MapNav_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.SetAutoNavBase();
        }

        private void btnAutoNav_MapNav_Click_1(object sender, RoutedEventArgs e)
        {
            if (btnAutoNav_MapNav.Opacity != 1)
                return;

            ShowHideAutoNavigationArrows(true);

            _viewModel.EnableAutoNav();

            btnAutoNav_MapNav.Opacity = 0.3;
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            //////disable autp nav
            _viewModel.StopAutoNav();
            btnAutoNav_MapNav.Opacity = 1;
            ShowHideAutoNavigationArrows(false);

            /////Disable Drawing/////
            ResetDrawButtons(null);
            _viewModel.StopDrawControl();
        }

        private void ShowHideAutoNavigationArrows(bool isVisible)
        {
            if (isVisible)
            {
                imgE.Visibility = System.Windows.Visibility.Visible;
                imgN.Visibility = System.Windows.Visibility.Visible;
                imgNE.Visibility = System.Windows.Visibility.Visible;
                imgNW.Visibility = System.Windows.Visibility.Visible;
                imgS.Visibility = System.Windows.Visibility.Visible;
                imgSE.Visibility = System.Windows.Visibility.Visible;
                imgSW.Visibility = System.Windows.Visibility.Visible;
                imgW.Visibility = System.Windows.Visibility.Visible;
                btnAutoNavBase_MapNav.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                imgE.Visibility = System.Windows.Visibility.Collapsed;
                imgN.Visibility = System.Windows.Visibility.Collapsed;
                imgNE.Visibility = System.Windows.Visibility.Collapsed;
                imgNW.Visibility = System.Windows.Visibility.Collapsed;
                imgS.Visibility = System.Windows.Visibility.Collapsed;
                imgSE.Visibility = System.Windows.Visibility.Collapsed;
                imgSW.Visibility = System.Windows.Visibility.Collapsed;
                imgW.Visibility = System.Windows.Visibility.Collapsed;
                btnAutoNavBase_MapNav.Visibility = System.Windows.Visibility.Collapsed;
            }
            
        }

        private void RunStoryBoard(string direction)
        {
            if (_beamImgDirectionStoryboard == null)
                _beamImgDirectionStoryboard = new Storyboard();

            _beamImgDirectionStoryboard.Stop();

            switch (direction)
            {
                case "W":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgW_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "E":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgE_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "N":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgN_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "S":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgS_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "NE":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgNE_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "SE":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgSE_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "NW":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgNW_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "SW":
                    _beamImgDirectionStoryboard = (Storyboard)this.Resources["beamImgSW_SB"];
                    _beamImgDirectionStoryboard.Begin();
                    break;
                case "none":
                    _beamImgDirectionStoryboard.Stop();
                    break;
                default: break;
            }
        }

        private void btnRedDrawPoint_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.SetDrawMode("point");

            ResetDrawButtons(btnRedDrawPoint);
        }

        private void btnRedDrawLine_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.SetDrawMode("line");

            ResetDrawButtons(btnRedDrawLine);
        }

        private void btnRedDrawPolygon_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.SetDrawMode("polygon");

            ResetDrawButtons(btnRedDrawPolygon);
        }

        private void btnRedDrawText_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.SetDrawMode("text");

            ResetDrawButtons(btnRedDrawText);
        }

        private void ResetDrawButtons(Image img)
        {
            btnRedDrawPoint.Opacity = 1;
            btnRedDrawLine.Opacity = 1;
            btnRedDrawPolygon.Opacity = 1;
            btnRedDrawText.Opacity = 1;

            if (img != null)
                (img as Image).Opacity = 0.3;
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}