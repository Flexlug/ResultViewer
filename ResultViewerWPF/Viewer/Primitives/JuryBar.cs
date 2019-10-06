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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ResultViewerWPF.Viewer.Primitives
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
            //// Инициализируем графическую составляющую
            //mainPanel = new Grid()
            //{
            //    // Ширина
            //    MinWidth = Program.Settings.JuryPanelWidth,
            //    // Высотаэ
            //    MinHeight = Program.Settings.JuryPanelHeight,
            //    // Цвет фона панели
            //    Background = new SolidColorBrush(Program.Settings.JuryPanelColor),
            //    // Цвет контура панели

            //    // Горизонтальное центрирование
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    // Вертикальное центрирование
            //    VerticalAlignment = VerticalAlignment.Center,

            //};

            //// Ставим имя
            //Name = name;

            //// Имя жюри
            //juryName = new TextBlock()
            //{
            //    // Текст
            //    Text = Name,
            //    // Положение текста
            //    TextAlignment = TextAlignment.Center,
            //    // Размер шрифта
            //    FontSize = Program.Settings.JuryFontSize,
            //    // Стиль текста
            //    FontWeight = Program.Settings.JuryFontWeight,
            //    // Максимально допустимая ширина TextBox-a
            //    MaxWidth = Program.Settings.JuryPanelWidth,
            //    // Максмально допустимая высота TextBox-а
            //    MaxHeight = Program.Settings.JuryPanelHeight,
            //    // Горизонтальное центрирование
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    // Вертикальное центрирование
            //    VerticalAlignment = VerticalAlignment.Center,
            //    // Для построчного переноса текста
            //    TextWrapping = TextWrapping.Wrap
            //};

            // Инициализируем главную родительскую панель
            mainPanel = new Grid();

            // Фон панели
            Rectangle mainRectangle = new Rectangle()
            {
                // Ширина панели
                Width = Program.Settings.JuryPanelWidth,
                // Высота панели
                Height = Program.Settings.JuryPanelHeight,
                // Цвет фона панели
                Fill = new SolidColorBrush(Program.Settings.JuryPanelColor),
                // Вертикальное центрирование содержимого
                VerticalAlignment = VerticalAlignment.Center,
                // Цвет контура панели
                Stroke = new SolidColorBrush(Program.Settings.JuryPanelStrokeColor),
                // Ширина контура панели
                StrokeThickness = Program.Settings.JuryPanelStrokeWidth
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
                FontSize = Program.Settings.JuryFontSize,
                // Стиль текста
                FontWeight = Program.Settings.JuryFontWeight,
                // Горизонтальное центрирование
                HorizontalAlignment = HorizontalAlignment.Center,
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center,
                // Для построчного переноса текста
                TextWrapping = TextWrapping.Wrap
            };

            //// Расставляем элементы
            //mainPanel.Children.Add(juryName);

            mainPanel.Children.Add(mainRectangle);
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

            // Выставляем координаты. Левый нижний угол. Отныне работаем с центром элемента
            Canvas.SetLeft(mainPanel, -(Program.Settings.JuryPanelWidth / 2));
            Canvas.SetTop(mainPanel, -(Program.Settings.JuryPanelHeight / 2));
            mainPanel.RenderTransformOrigin = new Point(0.5, 0.5);

            // Делаем объект невидимым
            mainPanel.OpacityMask = new SolidColorBrush(Colors.White);
            mainPanel.Opacity = 0;
        }
    }
}
