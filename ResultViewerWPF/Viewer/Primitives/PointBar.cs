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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ResultViewerWPF.Viewer.Primitives
{
    public class PointBar : Bar
    {
        /// <summary>
        /// Холст, на котором расположен данный элемент
        /// </summary>
        public Canvas ParentCanvas = null;

        private double _numOfPoints = 0;
        /// <summary>
        /// Количество баллов, присвоенных данному PointBar-у
        /// </summary>
        public double NumOfPoints
        {
            get
            {
                return _numOfPoints;
            }
            set
            {
                _numOfPoints = value;
                barPoints.Content = $"{_numOfPoints}";
            }
        }

        #region NumOfPointsProperty

        //public static readonly DependencyProperty NumOfPointsProperty = 
        //    DependencyProperty.Register("NumOfPoints",
        //                                typeof(int),
        //                                typeof(PointBar),
        //                                new UIPropertyMetadata(0, OnNumOfPointsChanged));

        //private static void OnNumOfPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}

        #endregion

        // Визуальная составляющая баллов
        Label barPoints = null;

        /// <summary>
        /// Инициализирует панель и сразу же размешает её на Canvas, который задан через GraphicsReferenceProvider, в левом нижнем углу
        /// </summary>
        /// <param name="_numOfpoints">Количество баллов</param>
        public PointBar(Canvas parentCanvas)
        {
            // Инициализируем графическую составляющую
            mainPanel = new Grid();

            // Рисуем шестиугольник
            Polygon pointHex = new Polygon()
            {
                // Рисуем шестиугольник
                Points = new PointCollection()
                {
                    new Point(0, 125),
                    new Point(75, 250),
                    new Point(225, 250),
                    new Point(300, 125),
                    new Point(225, 0),
                    new Point(75, 0)
                },
                // Заполним белым цветом
                Fill = Brushes.White,
                // Контур
                Stroke = Brushes.LightGray,
                // Толщина контура
                StrokeThickness = 4
            };

            // Количество баллов
            barPoints = new Label()
            {
                // Размер шрифта
                FontSize = Program.Settings.PointBarFontSize,
                // Стиль шрифта
                FontWeight = FontWeights.Thin,
                // Центрирование контрола
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                // Выравнивание контента
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };

            // Расставляем элементы
            mainPanel.Children.Add(pointHex);
            mainPanel.Children.Add(barPoints);

            // Инициализируем TranslateGroup
            BarTG = new TransformGroup();
            BarTG.Children.Add(new TranslateTransform());
            BarTG.Children.Add(new ScaleTransform());

            mainPanel.RenderTransform = BarTG;

            // Проверяем наличие ссылки на панель
            if (parentCanvas == null)
                throw new NullReferenceException("Не указана панель, куда должны загружаться все элементы");

            // Проверка прошла успешно. Грузим элемент в панель
            ParentCanvas = parentCanvas;
            ParentCanvas.Children.Add(mainPanel);

            // Выставляем координаты. Левый нижний угол. Отныне работаем с центром элемента
            Canvas.SetTop(mainPanel, -(mainPanel.ActualHeight));
            Canvas.SetLeft(mainPanel, -(mainPanel.ActualWidth));
            Canvas.SetZIndex(mainPanel, 100);

            // Делаем объект невидимым по умолчанию
            mainPanel.OpacityMask = new SolidColorBrush(Colors.White);
            mainPanel.Opacity = 0;

            // Обнуляем количество баллов
            NumOfPoints = 0;
        }
    }
}
