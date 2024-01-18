using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AdWin = Autodesk.Windows; 

namespace RAA_RibbonConfigurator
{
    internal static class Utils
    {
        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel currentPanel = GetRibbonPanelByName(app, tabName, panelName);

            if (currentPanel == null)
                currentPanel = app.CreateRibbonPanel(tabName, panelName);

            return currentPanel;
        }

        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

        internal static List<TabData> GetTabData()
        {
            List<TabData> returnList = new List<TabData>();
            List<string> tabNameList = Utils.GetTabNames();

            foreach (string tabName in tabNameList)
            {
                TabData curTab = new TabData(tabName);

                if (GetConfigValue(tabName) == "Show" || (GetConfigValue(tabName) == null))
                    curTab.IsVisible = true;
                else
                    curTab.IsVisible = false;

                returnList.Add(curTab);

            }

            return returnList;
        }

        private static List<string> GetTabNames()
        {
            return new List<string>()
            {
                "RAA Arch",
                "RAA Mechanical",
                "RAA Electrical",
                "RAA Plumbing",
                "RAA Structural"
            };
        }

        internal static string GetConfigPath()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configPath = Path.Combine(path, "ribbon.config");
            return configPath;
        }
        private static void CreateConfigFile(string path)
        {
            // create file
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (string tabName in GetTabNames())
                {
                    writer.WriteLine(tabName + ",Show");
                }
            }
        }

        internal static string GetConfigValue(string value)
        {
            string configPath = GetConfigPath();

            if(!File.Exists(configPath))
                CreateConfigFile(configPath);

            string returnValue = SearchConfig(configPath, value);
            return returnValue;
        }

        private static string SearchConfig(string configPath, string value)
        {
            string result = null;

            using (StreamReader reader = new StreamReader(configPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    if (values.Length >= 2 && values[0] ==  value)
                    {
                        result = values[1];
                        break;
                    }
                }
            }

            return result;
        }

        internal static void UpdateRibbon()
        {
            App.tabData = GetTabData();
            foreach(TabData tab in App.tabData)
            {
                AdWin.RibbonTab ribbonTab = App.ribbon.Tabs.FirstOrDefault(x => x.Id == tab.TabName);

                if(tab.IsVisible == false)
                    ribbonTab.IsVisible = false;
                else
                    ribbonTab.IsVisible = true;
            }
        }

        internal static void SetConfigValue(string tabName, string value)
        {
            // read config file
            var lines = File.ReadAllLines(GetConfigPath());

            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Split(',');

                if (line[0].Trim() == tabName)
                {
                    if (line.Length > 1)
                        line[1] = value;

                    lines[i] = string.Join(",", line);
                    break;
                }

            }  
            
            File.WriteAllLines(GetConfigPath(), lines);
        }
    }
}
