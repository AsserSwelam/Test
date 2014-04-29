using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Geo.Model
{
    public class DrawControl
    {

        Draw _drawControl;

        public event DrawControlDrawCompleted DrawCompletedEvent;
        
        FillSymbol _defaultFillSymbol;
        LineSymbol _defaultLineSymbo;

        DrawControl()
        {
            _drawControl = new Draw();
        }

        public DrawControl(Map map)
        {
            _drawControl = new Draw();
            _drawControl.IsEnabled = false;

            _drawControl.Map = map;

            _defaultFillSymbol = new ESRI.ArcGIS.Client.Symbols.FillSymbol();
            _defaultFillSymbol.BorderThickness = 3;
            _defaultFillSymbol.BorderBrush = new SolidColorBrush(Colors.Red);
            _defaultFillSymbol.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));

            _drawControl.FillSymbol = _defaultFillSymbol;

            _defaultLineSymbo = new ESRI.ArcGIS.Client.Symbols.LineSymbol();
            _defaultLineSymbo.Color = new SolidColorBrush(Colors.Red);
            _defaultLineSymbo.Width = 5;

            _drawControl.LineSymbol = _defaultLineSymbo;

            _drawControl.DrawComplete += _drawControl_DrawComplete;
        }

        public void SetMap(Map map)
        {
            if(_drawControl != null)
                _drawControl.Map= map;
        }

        void _drawControl_DrawComplete(object sender, DrawEventArgs e)
        {
            if (DrawCompletedEvent != null)
                DrawCompletedEvent(this, e);
        }

        public void SetDrawMode(DrawMode mode)
        {
            _drawControl.IsEnabled = true;
            _drawControl.DrawMode = mode;
        }

        public void DisableDrawControl()
        {
            _drawControl.IsEnabled = false;
            _drawControl.DrawMode = DrawMode.None;
        }

    }
}
