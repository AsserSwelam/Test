using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Model
{
    public class NavigateExtentDoneEventArgs : EventArgs
    {
        public NavigateExtentFocus navigateExtentOn; 
        public bool previousEnabled;
        public bool nextEnabled;
        
    }
}
