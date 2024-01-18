using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autodesk.Revit.UI;

namespace RAA_RibbonConfigurator
{
    /// <summary>
    /// Interaction logic for WinConfig.xaml
    /// </summary>
    public partial class WinConfig : Window
    {
        public List<TabData> Ribbons { get; set; }
        public WinConfig()
        {
            InitializeComponent();
            
            Ribbons = Utils.GetTabData();

            this.DataContext = this;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in Ribbons)
            {
                if(item.IsVisible == true)
                    Utils.SetConfigValue(item.TabName, "Show");
                else
                    Utils.SetConfigValue(item.TabName, "Hide");
            }
            this.Close();
            Utils.UpdateRibbon();
            TaskDialog.Show("Complete", "The ribbon has been updated");

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
