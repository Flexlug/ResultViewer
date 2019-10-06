// Copyright 2019 Flexlug

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

// See the License for the specific language governing permissions and
// limitations under the License.

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
