#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Windows.Markup;
using AdWin = Autodesk.Windows;

#endregion

namespace RAA_RibbonConfigurator
{
    internal class App : IExternalApplication
    {
        public static List<TabData> tabData = new List<TabData>();
        public static AdWin.RibbonControl ribbon;

        public Result OnStartup(UIControlledApplication app)
        {
            tabData = Utils.GetTabData();
            ribbon = AdWin.ComponentManager.Ribbon;

            // create ribbon tabs
            foreach (TabData tab in tabData)
            {
                try
                {
                    app.CreateRibbonTab(tab.TabName);
                }
                catch (Exception)
                {
                    Debug.Print("Tab already exists");
                }

                tab.Panels = new List<RibbonPanel>();
                tab.Panels.Add(Utils.CreateRibbonPanel(app, tab.TabName, "Config"));
                tab.Panels.Add(Utils.CreateRibbonPanel(app, tab.TabName, "Revit Tools"));
            }

            // create buttons
            PushButtonData btnConfig = CmdConfig.GetButtonData();
            PushButtonData btnData1 = Command1.GetButtonData();
            PushButtonData btnData2 = Command2.GetButtonData();

            // add buttons to panels
            foreach (TabData tab in tabData)
            {
                foreach (RibbonPanel panel in tab.Panels)
                {
                    if (panel.Name == "Config")
                        panel.AddItem(btnConfig);
                    else
                    {
                        panel.AddItem(btnData1);
                        panel.AddItem(btnData2);
                    }
                }
            }

            // turn on/off tabs
            Utils.UpdateRibbon();

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }


    }
}
