using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Панель, визуализирующая жюри
    /// </summary>
    public class JuryBar : Bar
    {
        /// <summary>
        /// Иия жюри
        /// </summary>
        private string name = "";
        public string Name
        {
            get => name;
            set
            {
                name = value;
                if (juryName != null)
                    juryName.Text = name;
            }
        }

        TextBlock juryName = null;

        /// <summary>
        /// Ссылка на холст, куда будет загружена панель жюри
        /// </summary>
        public Canvas ParentCanvas = null;

        /// <summary>
        /// Инициализирует панель и сразу расзмещает её на Canvas, который задан через GraphicsReferenceProvider, в левом нижнем углу
        /// </summary>
        /// <param name="name"></param>
        public JuryBar(Canvas parentCanvas, string name)
        {
            // Инициализируем графическую составляющую
            mainPanel = new Grid()
            {
                // Ширина
                MinWidth = ProgramSettings.JuryPanelWidth,
                // Высотаэ
                MinHeight = ProgramSettings.JuryPanelHeight,
                // Цвет фона панели
                Background = new SolidColorBrush(ProgramSettings.JuryPanelColor),
                // Горизонтальное центрирование
                HorizontalAlignment = HorizontalAlignment.Center,
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center,
                
            };

            // Ставим имя
            Name = name;

            // Имя жюри
            juryName = new TextBlock()
            {
                // Текст
                Text = Name,
                // Положение текста
                TextAlignment = TextAlignment.Center,
                // Размер шрифта
                FontSize = ProgramSettings.JuryFontSize,
                // Стиль текста
                FontWeight = FontWeights.Thin,
                // Максимально допустимая ширина TextBox-a
                MaxWidth = ProgramSettings.JuryPanelWidth,
                // Максмально допустимая высота TextBox-а
                MaxHeight = ProgramSettings.JuryPanelHeight,
                // Горизонтальное центрирование
                HorizontalAlignment = HorizontalAlignment.Center,
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center,
                // Для построчного переноса текста
                TextWrapping = TextWrapping.Wrap
            };

            // Расставляем элементы
            mainPanel.Children.Add(juryName);

            // Инициализируем TranslateGroup
            BarTG = new TransformGroup();
            BarTG.Children.Add(new TranslateTransform());
            BarTG.Children.Add(new ScaleTransform());
            mainPanel.RenderTransform = BarTG;

            // Проверяем наличие ссылки на панель
            if (parentCanvas == null)
                throw new NullReferenceException("Не указана панель, куда должны загружаться все элементы");

            // Проверка прошла успешно. Грузим элемент в родительскую панель
            ParentCanvas = parentCanvas;
            ParentCanvas.Children.Add(mainPanel);

            // Выставляем уоординаты. Левый нижний угол. Отныне работаем с центром элемента
            Canvas.SetLeft(mainPanel, -(ProgramSettings.JuryPanelWidth / 2));
            Canvas.SetTop(mainPanel, -(ProgramSettings.JuryPanelHeight / 2));
            mainPanel.RenderTransformOrigin = new Point(0.5, 0.5);

            // Делаем объект невидимым
            mainPanel.OpacityMask = new SolidColorBrush(Colors.White);
            mainPanel.Opacity = 0;
        }
    }
}
