using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Projection;
using ESRI.ArcGIS.Client.Symbols;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace Geo.Model
{
    public class GeoMap
    {
        Map _map;
        WebMercator _mercator;
        GraphicsLayer _myLocationLayer;
        GraphicsLayer _redliningGraphicsLayer;

         List<Envelope> _extentHistory = new List<Envelope>();
        int _currentExtentIndex = 0;
        bool _newExtent = true;
        AutoNavigation _autoNavigation;
        DrawControl _drawControl;
        GeoDrawMode _drawMode;

        public SimpleMarkerSymbol PointMarkerSymbol;
        public SimpleFillSymbol PolygonFillSymbol;
        public SimpleLineSymbol LineSymbol;
        public TextSymbol TextDrawsymbol;

        public event MapPanToDirection PanToChangedEvent;

        public event NavigateExtentDone NavigateExtentDoneEvent;

        public GeoMap(Map esriMap)
        {
            _map = esriMap;

            ArcGISTiledMapServiceLayer layer = new ArcGISTiledMapServiceLayer();
            layer.Url = "http://server.arcgisonline.com/arcgis/rest/services/ESRI_StreetMap_World_2D/MapServer";

            _myLocationLayer = new GraphicsLayer();
            _redliningGraphicsLayer = new GraphicsLayer();

            _map.Layers.Add(layer);
            _map.Layers.Add(_redliningGraphicsLayer);
            _map.Layers.Add(_myLocationLayer);

            _map.ExtentChanged += _map_ExtentChanged;

            ////////Init DrawControl//////////
            _drawControl = new DrawControl(_map);
            _drawMode = GeoDrawMode.None;
            _drawControl.SetDrawMode(DrawMode.None);
            _drawControl.DrawCompletedEvent += _drawControl_DrawCompletedEvent;

            ///////Init default Draw Symbols////////
            PointMarkerSymbol = new SimpleMarkerSymbol();

            PointMarkerSymbol.Color = new SolidColorBrush(Colors.Red);
            PointMarkerSymbol.Size = 15;
            PointMarkerSymbol.Style = SimpleMarkerSymbol.SimpleMarkerStyle.Circle;

            PolygonFillSymbol = new SimpleFillSymbol();
            PolygonFillSymbol.BorderBrush = new SolidColorBrush(Colors.Red);
            PolygonFillSymbol.BorderThickness = 3;
            PolygonFillSymbol.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 0, 0));

            LineSymbol = new SimpleLineSymbol();
            LineSymbol.Color = new SolidColorBrush(Colors.Red);
            LineSymbol.Width = 5;
            LineSymbol.Style = SimpleLineSymbol.LineStyle.Solid;

            TextDrawsymbol = new TextSymbol();
            TextDrawsymbol.Text = "Text";
            TextDrawsymbol.FontSize = 15;


        }

        public void SetDrawMode(GeoDrawMode mode)
        {
            _drawMode = mode;

            switch (mode)
            { 
                case GeoDrawMode.None:
                    _drawControl.SetDrawMode(DrawMode.None);
                    break;
                case GeoDrawMode.Point:
                case GeoDrawMode.Text:
                    _drawControl.SetDrawMode(DrawMode.Point);
                    break;
                case GeoDrawMode.Line:
                    _drawControl.SetDrawMode(DrawMode.Polyline);
                    break;
                case GeoDrawMode.Polygon:
                    _drawControl.SetDrawMode(DrawMode.Polygon);
                    break;
            }
        }

        public void StopDrawControl()
        {
            _drawControl.DisableDrawControl();
        }

        public void PrivousExtent()
        {
            if (_currentExtentIndex != 0)
            {
                _currentExtentIndex--;

                if (_currentExtentIndex == 0)
                {
                    if (NavigateExtentDoneEvent != null)
                    {
                        NavigateExtentDoneEventArgs args = new NavigateExtentDoneEventArgs();
                        args.navigateExtentOn = NavigateExtentFocus.Previous;
                        args.previousEnabled = false;

                        NavigateExtentDoneEvent(this, args);
                    }
                }

                _newExtent = false;

                _map.ZoomTo(_extentHistory[_currentExtentIndex]);

                if (NavigateExtentDoneEvent != null)
                {
                    NavigateExtentDoneEventArgs args = new NavigateExtentDoneEventArgs();
                    args.navigateExtentOn = NavigateExtentFocus.Next;
                    args.nextEnabled = true;

                    NavigateExtentDoneEvent(this, args);
                }
               
            }

        }

        public void NextExtent()
        {
            if (_currentExtentIndex < _extentHistory.Count - 1)
            {
                _currentExtentIndex++;

                if (_currentExtentIndex == (_extentHistory.Count - 1))
                {

                    if (NavigateExtentDoneEvent != null)
                    {
                        NavigateExtentDoneEventArgs args = new NavigateExtentDoneEventArgs();
                        args.navigateExtentOn = NavigateExtentFocus.Next;
                        args.nextEnabled = false;

                        NavigateExtentDoneEvent(this, args);
                    }
                }

                _newExtent = false;

              
                _map.ZoomTo(_extentHistory[_currentExtentIndex]);

                if (NavigateExtentDoneEvent != null)
                {
                    NavigateExtentDoneEventArgs args = new NavigateExtentDoneEventArgs();
                    args.navigateExtentOn = NavigateExtentFocus.Previous;
                    args.previousEnabled = true;

                    NavigateExtentDoneEvent(this, args);
                }
            }


        }

        public void FullExtent()
        {
            _map.Extent = _map.Layers[0].FullExtent;
        }

        public void ZoomToPoint(double x, double y)
        {
            _mercator = new WebMercator();
            Graphic pointGraphic = new Graphic();

            pointGraphic.Geometry = new MapPoint(x, y);

            SimpleMarkerSymbol symbol = new SimpleMarkerSymbol();
            symbol.Color = new SolidColorBrush(Colors.Red);
            symbol.Style = SimpleMarkerSymbol.SimpleMarkerStyle.Diamond;
            symbol.Size = 20;

            pointGraphic.Symbol = symbol;

            _myLocationLayer.Graphics.Clear();
            _myLocationLayer.Graphics.Add(pointGraphic);


            _map.ZoomTo(GetCenterExtent(pointGraphic.Geometry as MapPoint));
        }

        public void ClearGraphicsLayer()
        {
            _myLocationLayer.Graphics.Clear();
            _redliningGraphicsLayer.Graphics.Clear();
        }

        public void EnableAutoNav()
        {
            _autoNavigation = new AutoNavigation(_map);

            _autoNavigation.PanToEvent += _autoNavigation_PanToEvent;
            _autoNavigation.Start();
        }

        public void SetAutoNavBase()
        {
            if (_autoNavigation != null)
                _autoNavigation.Start();
        }

        public void DisableAutoNav()
        {
            if (_autoNavigation != null)
            {
                _autoNavigation.Stop();

                _autoNavigation.PanToEvent -= _autoNavigation_PanToEvent;
                _autoNavigation = null;
            }
        }



        /////////////////////////////////Private Methods/////////////////////////////
        void _drawControl_DrawCompletedEvent(object sender, DrawEventArgs e)
        {
            Graphic graphic = new Graphic();

            if (e.Geometry != null)
                graphic.Geometry = e.Geometry;

            if (_drawMode == GeoDrawMode.Point && e.Geometry is MapPoint)
                graphic.Symbol = PointMarkerSymbol;
            else if (_drawMode == GeoDrawMode.Text && e.Geometry is MapPoint)
                graphic.Symbol = TextDrawsymbol;
            else if (_drawMode == GeoDrawMode.Line && e.Geometry is Polyline)
                graphic.Symbol = LineSymbol;
            else if (_drawMode == GeoDrawMode.Polygon && e.Geometry is Polygon)
                graphic.Symbol = PolygonFillSymbol;

            _redliningGraphicsLayer.Graphics.Add(graphic);
        }

        void _map_ExtentChanged(object sender, ExtentEventArgs e)
        {

            ////////////////Next and previous extents handling///////////////////
            if (e.OldExtent == null)
            {
                _extentHistory.Add(e.NewExtent.Clone());
                return;
            }

            if (_newExtent)
            {
                _currentExtentIndex++;

                if (_extentHistory.Count - _currentExtentIndex > 0)
                    _extentHistory.RemoveRange(_currentExtentIndex, (_extentHistory.Count - _currentExtentIndex));

                _extentHistory.Add(e.NewExtent.Clone());

                if (NavigateExtentDoneEvent != null)
                {
                    NavigateExtentDoneEventArgs args = new NavigateExtentDoneEventArgs();
                    args.navigateExtentOn = NavigateExtentFocus.Both;
                    args.nextEnabled = false;
                    args.previousEnabled = true;

                    NavigateExtentDoneEvent(this, args);
                }
            }
            else
            {
                _newExtent = true;
            }

            ////////////////////////////////////////////////////////////////////////

        }

        void _autoNavigation_PanToEvent(object sender, string direction)
        {
            if (PanToChangedEvent != null)
                PanToChangedEvent(this, direction);
        }

        Envelope GetCenterExtent(MapPoint point)
        {
            Envelope extent = new Envelope();

            if (_map.Layers[0] == null)
                return extent;

            double XRatio = Math.Abs((_map.Layers[0].FullExtent as Envelope).XMax / 100000);
            double YRatio = Math.Abs((_map.Layers[0].FullExtent as Envelope).YMax / 100000);
            double xMinNew = point.X - XRatio;
            double xMaxNew = point.X + XRatio;
            double yMinNew = point.Y - YRatio;
            double yMaxNew = point.Y + YRatio;

            extent = new Envelope(xMinNew, yMinNew, xMaxNew, yMaxNew);

            return extent;
        }
    }

    public class StopZone
    {

        public Vector3 From;
        public Vector3 To;

    }

    public class StopZoneDouble
    {
        public double From;
        public double TO;
    }

   


}
