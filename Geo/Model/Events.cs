using ESRI.ArcGIS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Model
{
    public delegate void NavigateExtentDone(object sender, NavigateExtentDoneEventArgs e);

    public delegate void MapPanToDirection(object sender, string direction);

    public delegate void DrawControlDrawCompleted(object sender, DrawEventArgs e);
}
