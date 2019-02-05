using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Абстрактное представление панели, которая поддаётся анимации
    /// </summary>
    public class Bar
    {
        /// <summary>
        /// Коллекция из различных Transform-ов, которые можно анимировать
        /// </summary>
        public TransformGroup BarTG;

        /// <summary>
        /// Главная панель, на которой расположен весь декор
        /// </summary>
        public Panel mainPanel;

        /// <summary>
        /// Точка, куда движется объект на данный момент, или где он находится на данный момент
        /// </summary>
        public Point movingTo;

        public TranslateTransform GetTT()
        {
            return BarTG.Children[0] as TranslateTransform;
        }

        public ScaleTransform GetST()
        {
            return BarTG.Children[1] as ScaleTransform;
        }
    }
}
