using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimatedBackgroundWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Откроем диалог для того, чтобы выбрать видео на фон
            OpenFileDialog opf = new OpenFileDialog();

            // Получим путь к этому видео
            opf.ShowDialog();

            MediaElement video = new MediaElement()
            {
                Source = new Uri(opf.FileName),
                Volume = 0,
            };

            video.MediaEnded += (obj, e) =>
            {
                video.Stop();
                video.Play();
            };

            Background = new VisualBrush()
            {
                Visual = video
            };
        }
    }
}
