using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Model
{
    public enum NavigateExtentFocus
    { 
        Both,
        Previous,
        Next
    }

    public enum Quarter
    {
        TopLeft,
        TopRight,
        DownLeft,
        DownRight
    }

    public enum Directions
    {
        North,
        South,
        West,
        East,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest,
        None
    }

    public enum GeoDrawMode
    { 
        Point,
        Line,
        Polygon,
        Text,
        None
    }
}
