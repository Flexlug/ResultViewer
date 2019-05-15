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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovingObjectWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DoubleAnimation XCordAnim;
        DoubleAnimation YCordAnim;

        TranslateTransform movingObjTrans;

        public MainWindow()
        {
            // Инициализация окна и его содержимого
            InitializeComponent();


            // MAGIC
            movingObjTrans = new TranslateTransform();
            MovingObject.RenderTransform = movingObjTrans;


            MediaPlayer md = new MediaPlayer();

            DoubleAnimation volumeAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1
            };
            volumeAnimation.Completed += (ev, arg) =>
            {
                md.Volume = 1;
            };

            volumeAnimation.BeginAnimation(md.Volume, TimeSpan.FromSeconds(3));




            // Начальная инициализация анимаций
            XCordAnim = new DoubleAnimation()
            {
                From = 50,
                To = 50,
                Duration = TimeSpan.FromSeconds(0.5),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };

            YCordAnim = new DoubleAnimation()
            {
                From = 50,
                To = 50,
                Duration = TimeSpan.FromSeconds(0.5),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем координаты миши
            double x = e.GetPosition(null).X;
            double y = e.GetPosition(null).Y;

            // Задаём конечную точку анимации, обновляем начальную
            XCordAnim.From = movingObjTrans.X;
            YCordAnim.From = movingObjTrans.Y;

            XCordAnim.To = x;
            YCordAnim.To = y;


            // Запускаем анимацию
            movingObjTrans.BeginAnimation(TranslateTransform.XProperty, XCordAnim);
            movingObjTrans.BeginAnimation(TranslateTransform.YProperty, YCordAnim);
        }
    }
}
