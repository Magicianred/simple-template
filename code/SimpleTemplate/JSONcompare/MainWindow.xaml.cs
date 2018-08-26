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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JSONcompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtResult.Text = compare(txtSource.Text, txtDest.Text);
            }
            catch (Exception ex)
            {
                StringBuilder bld = new StringBuilder();
                bld.AppendLine("Error");
                bld.AppendLine(ex.Message);
                txtResult.Text = bld.ToString();
            }
            
        }

        private string compare(string source, string destination)
        {
            JsonDiffPatchDotNet.JsonDiffPatch jdp = new JsonDiffPatchDotNet.JsonDiffPatch();

            Newtonsoft.Json.Linq.JToken patch = jdp.Diff(source, destination);

            return patch.ToString();
        }
    }
}
