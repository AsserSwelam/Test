using ESRI.ArcGIS.Client;
using Geo.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.Devices.Geolocation;


namespace Geo.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        GeoMap _geoMap;
        
        bool _graphicsOnMap = false;
        double _nextBtnOpacity = 0.3;
        double _prevBtnOpacity = 0.3;
        double _extentBtnOpacity = 1.0;
        string _storyBoardDirection = string.Empty;

        public double ExtentBtnOpacity
        {
            get { return _extentBtnOpacity; }
            set
            {
                _extentBtnOpacity = value;
                NotifyPropertyChanged("ExtentBtnOpacity");
            }
        }

        public bool GraphicsOnMap
        {
            get { return _graphicsOnMap; }
            set
            {
                _graphicsOnMap = value;
                NotifyPropertyChanged("GraphicsOnMap");
            }
        }

        public double NextBtnOpacity
        {
            get { return _nextBtnOpacity; }
            set
            {
                _nextBtnOpacity = value;
                NotifyPropertyChanged("NextBtnOpacity");
            }
        }
        
        public double PrevBtnOpacity
        {
            get { return _prevBtnOpacity; }
            set
            {
                _prevBtnOpacity = value;
                NotifyPropertyChanged("PrevBtnOpacity");
            }
        }

        public string StoryBoardDirection
        {
            get { return _storyBoardDirection; }
            set
            {
                _storyBoardDirection = value;
                NotifyPropertyChanged("StoryBoardDirection");
            }
        }

        public GeoMap GeoMap
        {
            get { return _geoMap; }
            set { _geoMap = value; }
        }

        public ViewModel(Map esriMap)
        { 
            GeoMap = new GeoMap(esriMap);

            GeoMap.PanToChangedEvent += GeoMap_PanToChangedEvent;
            _geoMap.NavigateExtentDoneEvent += _geoMap_NavigateExtentDoneEvent;
        }

        void GeoMap_PanToChangedEvent(object sender, string direction)
        {
            StoryBoardDirection = direction;
        }

        void _geoMap_NavigateExtentDoneEvent(object sender, NavigateExtentDoneEventArgs e)
        {
            if (e.navigateExtentOn == NavigateExtentFocus.Both)
            {
                if(e.nextEnabled)
                    NextBtnOpacity = 1;
                else
                    NextBtnOpacity = 0.3;

                if(e.previousEnabled)
                    PrevBtnOpacity = 1;
                else
                    PrevBtnOpacity = 0.3;

            }
            else if (e.navigateExtentOn == NavigateExtentFocus.Next)
            {
                if (e.nextEnabled)
                    NextBtnOpacity = 1;
                else
                    NextBtnOpacity = 0.3;
            }
            else
            {
                if (e.previousEnabled)
                    PrevBtnOpacity = 1;
                else
                    PrevBtnOpacity = 0.3;
            }
        }

        public void MapFullExtent()
        {
            if (GeoMap != null)
                GeoMap.FullExtent();
        }

        public async void SetMyLocation()
        {
            double xLoc;
            double yLoc;

            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
            {
                // The user has opted out of Location.
                return;
            }

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync();

                xLoc = geoposition.Coordinate.Longitude;
                yLoc = geoposition.Coordinate.Latitude;

                _geoMap.ZoomToPoint(xLoc, yLoc);

                GraphicsOnMap = true;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageBox.Show("location  is disabled in phone settings.",
                   "Location",
                   MessageBoxButton.OK);
                }
                //else
                {
                    // something else happened acquring the location
                }
            }

        }

        public void GoToPreviousExtent()
        {
            _geoMap.PrivousExtent();
        }

        public void GoToNextExtent()
        {
            _geoMap.NextExtent();
        }

        public void ClearMapGraphics()
        {
            _geoMap.ClearGraphicsLayer();
            GraphicsOnMap = false;
        }
       
        public void EnableAutoNav()
        {
            _geoMap.EnableAutoNav();
        }

        public void SetAutoNavBase()
        {
            _geoMap.SetAutoNavBase();
        }

        public void StopAutoNav()
        {
            _geoMap.DisableAutoNav();
        }

        public void SetDrawMode(string mode)
        {
            if (string.IsNullOrEmpty(mode))
                return;

            GraphicsOnMap = true;
            switch (mode)
            { 
                case "point":
                    GeoMap.SetDrawMode(GeoDrawMode.Point);
                    break;
                case "line":
                    GeoMap.SetDrawMode(GeoDrawMode.Line);
                    break;
                case "polygon":
                    GeoMap.SetDrawMode(GeoDrawMode.Polygon);
                    break;
                case "text":
                    GeoMap.SetDrawMode(GeoDrawMode.Text);
                    break;
                case "none":
                    GeoMap.SetDrawMode(GeoDrawMode.None);
                    break;
            }
        }

        public void StopDrawControl()
        {
            GeoMap.StopDrawControl();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

    }

   


}
