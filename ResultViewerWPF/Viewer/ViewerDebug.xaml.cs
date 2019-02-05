using System;
using System.Collections.Generic;
using System.IO;
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

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Логика взаимодействия для ViewerDebug.xaml
    /// </summary>
    public partial class ViewerDebug : Window
    {
        public ViewerDebug()
        {
            InitializeComponent();
        }

        public void Add(string info) => DebugInfo.AppendText(info + '\n');

        private void DebugInfo_TextChanged(object sender, TextChangedEventArgs e) => DebugInfo.ScrollToEnd();

        public void Save() => Button_Click(null, null);

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter($"debug_{DateTime.Now.ToFileTime()}_{DateTime.Now.DayOfWeek}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt");

             foreach (string s in DebugInfo.Text.Split('\n')) 
                await sw.WriteLineAsync(s);

            sw.Close();

            MessageBox.Show($"Debug trace saved\nFile name: debug_{DateTime.Now.ToFileTime()}_{DateTime.Now.DayOfWeek}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt", "Debug", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
