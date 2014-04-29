using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using Geo.Model;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Geo
{
    class AutoNavigation
    {

        const double _freeZoneFactor = 0.2;
        StopZone _portrateStopZone;
        Vector3 _basePosition;
        Vector3 _currentPosition;
        Quarter _baseQuarter;

        StopZoneDouble _portrateStopZonePoint;
        StopZoneDouble _portrateFreeZonePoint;

        double _currentPositionPoint;
        bool _isFirstTime;

        Map _map;
        Accelerometer _accelerometer;
        System.Windows.Threading.DispatcherTimer _myDispatcherTimer;
        Directions _panDirection = new Directions();

        public event MapPanToDirection PanToEvent;

         // Constructor
        public AutoNavigation(Map map)
        {

            _isFirstTime = true;

            _map = map;
            _map.ExtentChanged += myMap_ExtentChanged;

            /////Define Portarte Default Stop Zone/////
            _portrateStopZone = new StopZone();
            _portrateStopZone.From = new Vector3(0.0f, 0.7f, -0.3f);
            _portrateStopZone.To = new Vector3(0.0f, 0.7f, 0.3f);

            _panDirection = Directions.None;

            if (_myDispatcherTimer == null)
            {
                _myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                _myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 3, 0); // 100 Milliseconds 
                _myDispatcherTimer.Tick += myDispatcherTimer_Tick;
            }
           
        }

        void myMap_ExtentChanged(object sender, ExtentEventArgs e)
        {
            
            if(_myDispatcherTimer != null)
                _myDispatcherTimer.Start();
     
        }

        void myDispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_accelerometer != null)
                _accelerometer.Start();
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => ValuesHandler(e.SensorReading));

        }

        private void ValuesHandler(AccelerometerReading reading)
        {
            //Set current position
            if (reading.Acceleration != null)
                _currentPosition = reading.Acceleration;

            if (_isFirstTime)
            {
                if (_currentPosition != null)
                    _basePosition = _currentPosition;

                _baseQuarter = GetQuarter(_basePosition);

                _portrateFreeZonePoint = new StopZoneDouble();
                _portrateFreeZonePoint.From = ConvertPositionToDouble(_basePosition) + 2;
                _portrateFreeZonePoint.TO = ConvertPositionToDouble(_basePosition) - 2;

                _portrateStopZonePoint = new StopZoneDouble();
                _portrateStopZonePoint.From = ConvertPositionToDouble(_portrateStopZone.From);
                _portrateStopZonePoint.TO = ConvertPositionToDouble(_portrateStopZone.To);

                _isFirstTime = false;
            }
         
            _panDirection = Directions.None;

            _currentPositionPoint = ConvertPositionToDouble(_currentPosition);

            NavigateRightLeftHandler(_currentPosition);
            NavigateUpDownHandler(_currentPositionPoint);


            PanTo(_panDirection);

            RunStoryBoard(_panDirection);
        }

        private void NavigateRightLeftHandler(Vector3 position)
        {
          

            if (position.X < -0.2)
            {
                _panDirection = Directions.West;

            }
            else if (position.X > 0.2)
            {
                _panDirection = Directions.East;


            }

        }

        private void NavigateUpDownHandler(double position)
        {

            if (_portrateFreeZonePoint == null || _portrateStopZonePoint == null)
                return;

            //Navigate Down
           
            if (_portrateFreeZonePoint.TO < 20 && position > 20)
            {
                double freeZonePointToNew = _portrateFreeZonePoint.TO + 40;

                if (position < freeZonePointToNew && position > _portrateStopZonePoint.TO)
                {
                    if (_panDirection == Directions.East)
                        _panDirection = Directions.SouthEast;
                    else if (_panDirection == Directions.West)
                        _panDirection = Directions.SouthWest;
                    else
                        _panDirection = Directions.South;


                }
            }
            else if (_portrateFreeZonePoint.TO < 20 && position < 20 && position < _portrateFreeZonePoint.From)
            {
                if (position < _portrateFreeZonePoint.TO && position < _portrateStopZonePoint.TO)
                {
                      if (_panDirection == Directions.East)
                        _panDirection = Directions.SouthEast;
                    else if (_panDirection == Directions.West)
                        _panDirection = Directions.SouthWest;
                    else
                        _panDirection = Directions.South;


                }
            }
            else if (_portrateFreeZonePoint.TO > 20 && position > 20)
            {
                if (position < _portrateFreeZonePoint.TO && position > _portrateStopZonePoint.TO)
                {
                    if (_panDirection == Directions.East)
                        _panDirection = Directions.SouthEast;
                    else if (_panDirection == Directions.West)
                        _panDirection = Directions.SouthWest;
                    else
                        _panDirection = Directions.South;


                }
            }

            //Navigate Up
            else if (_portrateFreeZonePoint.From > 20 && position < 20)
            {
                //double freeZonePointFromNew = _portrateFreeZonePoint.From - 20;

                if (position > 0 && position < _portrateStopZonePoint.From)
                {
                    if (_panDirection == Directions.East)
                        _panDirection = Directions.NorthEast;
                    else if (_panDirection == Directions.West)
                        _panDirection = Directions.NorthWest;
                    else
                        _panDirection = Directions.North;


                }
            }
            else if (_portrateFreeZonePoint.From > 20 && position > 20)
            {
                if (position > _portrateFreeZonePoint.From && position > _portrateStopZonePoint.From)
                {
                    if (_panDirection == Directions.East)
                        _panDirection = Directions.NorthEast;
                    else if (_panDirection == Directions.West)
                        _panDirection = Directions.NorthWest;
                    else
                        _panDirection = Directions.North;


                }
            }
            else
            {
                if (position > _portrateFreeZonePoint.From && position < _portrateStopZonePoint.From)
                {
                   
                    if (_panDirection == Directions.East)
                        _panDirection = Directions.NorthEast;
                    else if (_panDirection == Directions.West)
                        _panDirection = Directions.NorthWest;
                    else
                        _panDirection = Directions.North;


                }
            }

        }

        public void Start()
        {

            _isFirstTime = true;

             _accelerometer = new Accelerometer();
            _accelerometer.CurrentValueChanged +=  new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);

            _accelerometer.Start();
            _myDispatcherTimer.Stop();
            FirePanToEvent("none");


        }

        private Quarter GetQuarter(Vector3 position)
        {
            Quarter quarter = new Quarter();

            if (position.Z < 1 && position.Z > 0 && position.Y < 0 && position.Y > -1)
                quarter = Quarter.TopRight;
            else if (position.Z < 0 && position.Z > -1 && position.Y < 0 && position.Y > -1)
                quarter = Quarter.TopLeft;
            else if (position.Z < 0 && position.Z > -1 && position.Y > 0 && position.Y < 1)
                quarter = Quarter.DownLeft;
            else if (position.Z < 1 && position.Z > 0 && position.Y > 0 && position.Y < 1)
                quarter = Quarter.DownRight;


            return quarter;
        }

        private StopZoneDouble ConvertZoneTodouble(StopZone zone)
        {
            StopZoneDouble stDouble = new StopZoneDouble();

            if (zone == null)
                return stDouble;

            if (GetQuarter(zone.From) == Quarter.TopRight)
            {
                stDouble.From = 40 - (zone.From.Z * 10);
            }
            else if (GetQuarter(zone.From) == Quarter.TopLeft)
            {
                stDouble.From = -(zone.From.Z * 10);
            }
            else if (GetQuarter(zone.From) == Quarter.DownLeft)
            {
                stDouble.From = 20 - (-(zone.From.Z * 10));
            }
            else if (GetQuarter(zone.From) == Quarter.DownRight)
            {
                stDouble.From = 20 + (zone.From.Z * 10);
            }

            if (GetQuarter(zone.To) == Quarter.TopRight)
            {
                stDouble.TO = 40 - (zone.To.Z * 10);
            }
            else if (GetQuarter(zone.To) == Quarter.TopLeft)
            {
                stDouble.TO = -(zone.To.Z * 10);
            }
            else if (GetQuarter(zone.To) == Quarter.DownLeft)
            {
                stDouble.TO = 20 - (-(zone.To.Z * 10));
            }
            else if (GetQuarter(zone.To) == Quarter.DownRight)
            {
                stDouble.TO = 20 + (zone.To.Z * 10);
            }


            return stDouble;
        }

        private double ConvertPositionToDouble(Vector3 position)
        {
            double pDouble = 0.0;

            if (position == null)
                return pDouble;

            if (GetQuarter(position) == Quarter.TopRight)
            {
                pDouble = 40 - (position.Z * 10);
            }
            else if (GetQuarter(position) == Quarter.TopLeft)
            {
                pDouble = -(position.Z * 10);
            }
            else if (GetQuarter(position) == Quarter.DownLeft)
            {
                pDouble = 20 - (-(position.Z * 10));
            }
            else if (GetQuarter(position) == Quarter.DownRight)
            {
                pDouble = 20 + (position.Z * 10);
            }


            return pDouble;
        }

        private void PanTo(Directions direction)
        {
            if (_accelerometer != null && direction != Directions.None)
                _accelerometer.Stop();

            if (_myDispatcherTimer != null && direction != Directions.None)
                _myDispatcherTimer.Stop();

            Envelope extent = _map.Extent;
            
            if (extent == null) 
                return;
            
            MapPoint center = extent.GetCenter();

            switch (direction)
            {
                case Directions.West:
                    _map.PanTo(new MapPoint(extent.XMin, center.Y)); break;
                case Directions.East:
                    _map.PanTo(new MapPoint(extent.XMax, center.Y)); break;
                case Directions.North:
                    _map.PanTo(new MapPoint(center.X, extent.YMax)); break;
                case Directions.South:
                    _map.PanTo(new MapPoint(center.X, extent.YMin)); break;
                case Directions.NorthEast:
                    _map.PanTo(new MapPoint(extent.XMax, extent.YMax)); break;
                case Directions.SouthEast:
                    _map.PanTo(new MapPoint(extent.XMax, extent.YMin)); break;
                case Directions.NorthWest:
                    _map.PanTo(new MapPoint(extent.XMin, extent.YMax)); break;
                case Directions.SouthWest:
                    _map.PanTo(new MapPoint(extent.XMin, extent.YMin)); break;
                default: break;
            }
        }

        public void Stop()
        {
            if (_accelerometer != null)
            {
                _accelerometer.Stop();
                _accelerometer = null;
            }

            if (_myDispatcherTimer != null)
            {
                _myDispatcherTimer.Stop();
                _myDispatcherTimer = null;
            }

            FirePanToEvent("none");
            
        }

        private void RunStoryBoard(Directions direction)
        {

            FirePanToEvent("none");

            switch (direction)
            {
                case Directions.West:
                    FirePanToEvent("W");
                    break;
                case Directions.East:
                    FirePanToEvent("E");
                    break;
                case Directions.North:
                    FirePanToEvent("N");
                    break;
                case Directions.South:
                    FirePanToEvent("S");
                    break;
                case Directions.NorthEast:
                    FirePanToEvent("NE");
                    break;
                case Directions.SouthEast:
                    FirePanToEvent("SE");
                    break;
                case Directions.NorthWest:
                    FirePanToEvent("NW");
                    break;
                case Directions.SouthWest:
                    FirePanToEvent("SW");
                    break;
                default: break;
            }
        }

        private void FirePanToEvent(string direction)
        {
            if (PanToEvent != null)
                PanToEvent(this, direction);
        }

    }
}
