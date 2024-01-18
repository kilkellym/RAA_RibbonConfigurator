using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAA_RibbonConfigurator
{
    public class TabData
    {
        public string TabName { get; set; }
        public bool IsVisible { get; set; }
        public List<RibbonPanel> Panels { get; set; }
        public TabData(string tabName)
        {
            TabName = tabName;
            IsVisible = true;
        }
    }
}
