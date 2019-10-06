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
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ResultViewerWPF.Viewer.Primitives.ColumnTextBar
{
    /// <summary>
    /// Панель, куда можно загрузить многострочный текст
    /// </summary>
    public class PointColumnTextBar : Bar
    {
        private string[] _textLines;

        /// <summary>
        /// Разделённые строки по строкам
        /// </summary>
        public string[] textLines
        {
            get
            {
                return _textLines;
            }
            set
            {
                _textLines = value;
                textControl.Inlines.Clear();

                foreach (string s in _textLines)
                {
                    textControl.Inlines.Add(new Run()
                    {
                        Text = s
                    });
                    textControl.Inlines.Add(new LineBreak());
                }
            }
        }

        /// <summary>
        /// Текст надписи
        /// </summary>
        public string Text
        {
            get
            {
                StringBuilder res = new StringBuilder();
                foreach (string s in textLines)
                    res.Append(s + '\n');

                return res.ToString();
            }
            set
            {
                textLines = value.Split('\n').Select(x => x.Replace("\r", "")).ToArray();
            }
        }

        /// <summary>
        /// Оторажает, в каком состоянии сейчас находится объект
        /// </summary>
        public bool IsVisible = false;

        /// <summary>
        /// Возвращает реальную ширину textBlock-а, котороая , фактически, является шириной панели
        /// </summary>
        /// <returns></returns>
        public double GetPanelWidth()
        {
            return textControl.ActualWidth;
        }

        /// <summary>
        /// Возвращает реальную высоту textBlock-а, котороая , фактически, является высотой панели
        /// </summary>
        /// <returns></returns>
        public double GetPanelHeight()
        {
            return textControl.ActualHeight;
        }

        /// <summary>
        /// Элемент, в котором рамещаются все строки
        /// </summary>
        TextBlock textControl;

        /// <summary>
        /// Стандартный публичный конструктор
        /// </summary>
        public PointColumnTextBar(Canvas parentCanvas)
        {
            // Инициализируем главную панель
            mainPanel = new StackPanel()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical
            };

            // Инициализируем TranslateGroup
            BarTG = new TransformGroup();
            BarTG.Children.Add(new TranslateTransform());
            BarTG.Children.Add(new ScaleTransform());
            mainPanel.RenderTransform = BarTG;

            // Инициализируем TextBlock
            textControl = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = Program.Settings.PointsColumnPhraseFontSize,
                Foreground = new SolidColorBrush(Program.Settings.PointsColumnPhraseFontColor),
                TextAlignment = TextAlignment.Center,
                FontWeight = Program.Settings.PointsColumnPhraseFontWeight,
                TextDecorations = Program.Settings.PointsColumnPhraseIsUnderlined ? TextDecorations.Underline : null,
                Text = ""
            };

            // Проверяем наличие ссылки на родительскую панель
            if (parentCanvas == null)
                throw new NullReferenceException("Не указана панель, куда должен загружаться элемент");

            // Проверка прошла успешно. Грузим элемент на панель
            mainPanel.Children.Add(textControl);
            parentCanvas.Children.Add(mainPanel);

            //Делаем объект невидимым по умолчанию
            mainPanel.OpacityMask = new SolidColorBrush(Colors.White);
            mainPanel.Opacity = 0;

            // Проверим в настройках, включена ли надпись
            if (Program.Settings.PointsColumnPhraseShowMode != Program.Settings.PhraseShowMode.Never)
                // Если надпись включена - грузим текст. Иначе текст вообще загружен не будет
                Text = Program.Settings.PointsColumnPhrase;
        }
    }
}