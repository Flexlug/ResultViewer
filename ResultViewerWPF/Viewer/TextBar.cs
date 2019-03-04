using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Панель, куда можно загрузить многострочный текст
    /// </summary>
    class TextBar : Bar
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] TextLines;

        /// <summary>
        /// Стандартный публичный конструктор
        /// </summary>
        public TextBar()
        {
            // Инициализируем главную панель
            mainPanel = new StackPanel()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical
            };

            // Инициализируем TranslateTransform
            BarTG = new TransformGroup();
            BarTG.Children.Add(new TranslateTransform());
            BarTG.Children.Add(new ScaleTransform());
            mainPanel.RenderTransform = BarTG;
        }
    }
}
