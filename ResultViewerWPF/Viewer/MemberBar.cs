using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;
using System.Windows.Media.Animation;

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Панель, визуализирующая участника конкурса
    /// </summary>
    public class MemberBar : Bar
    {
        /// <summary>
        /// Имя участника
        /// </summary>
        public string Name = "";
        
        /// <summary>
        /// Уникальный 
        /// </summary>
        public int ID = 0;

        /// <summary>
        /// Ссылка на пенль, куда была загружена панель участника
        /// </summary>
        public Canvas ParentCanvas;

        /// <summary>
        /// Координата PlaceBar-а, когда доп. значение показано
        /// </summary>
        Point valueOpenedCord;

        /// <summary>
        /// Координата PlaceBar-а, когда доп. значение скрыто
        /// </summary>
        Point valueClosedCord;

        public bool ShowValueAllowed
        {
            get; private set;
        }

        public Bar placeBar = null;
        public Panel placePanel = null;
        private int place = 0;
        /// <summary>
        /// Задаёт или возвращает предварительное место участника
        /// </summary>
        public int Place
        {
            get
            {
                return place;
            }
            set
            {
                place = value;
                if (memberPlace != null)
                    memberPlace.Content = $"{place}";
            }
        }       

        /// <summary>
        /// Показывает, выделена ли панель участника первым цветом
        /// </summary>
        public bool IsChosen = false;

        /// <summary>
        /// Показывает, отображает ли на данный момент 
        /// </summary>
        public bool ValueVisible;

        /// <summary>
        /// Показывает, выделена ли панель участника вторым цветом
        /// </summary>
        public bool IsColored = false;

        /// <summary>
        /// Задная заливка
        /// </summary>
        public Rectangle mainRectangle;

        private double _numOfPoints = 0;
        Label memberPlace = null;
        /// <summary>
        /// Количество баллов, которое получил участник
        /// </summary>
        public double Points
        {
            get
            {
                return _numOfPoints;
            }
            set
            {
                _numOfPoints = value;
                if (memberPoints != null)
                    memberPoints.Content = $"{_numOfPoints}";
            }
        }

        public Bar valueBar = null;
        public Panel valuePanel = null;
        private double _value = 0;
        Label memberResult = null;
        /// <summary>
        /// Дополнительные данные участника
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (memberResult != null)
                    memberResult.Content = $"{_value}";
            }
        }

        // Визуальная составлющая баллов
        Label memberPoints = null;

        /// <summary>
        /// Инициализирует панель и сразу размещает её на Canvas, который задан через GraphicsReferenceProvider, в левом нижнем углу
        /// </summary>
        /// <param name="parentCanvas">Ссылка на холст, куда будет загружена панель</param>
        /// <param name="name">Имя участника</param>
        /// <param name="_id">Уникальный id участника, по которому его можно найти в </param>
        public MemberBar(Canvas parentCanvas, string name, int _id = 0, double _value = 0)
        {
            // Ставим нашему участнику имя2 мин
            Name = name;

            // Присваиваем уникальный ID участнику, чтобы его можно было найти вдальнейшем
            ID = _id;

            // Инициализируем графическую состовляющую
            mainPanel = new Canvas();

            // Фон панели
            mainRectangle = new Rectangle()
            {
                // Ширина
                Width = ProgramSettings.MemberPanelWidth,
                // Высота
                Height = ProgramSettings.MemberPanelHeight,
                // Цвет фона панели
                Fill = new SolidColorBrush(ProgramSettings.MemberPanelColor),
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center
            };

            // Панель баллов
            Grid pointsPanel = new Grid()
            {
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center
            };

            // Контур вокруг баллов
            Rectangle pointsStroke = new Rectangle()
            {
                // Ширина
                Width = ProgramSettings.MemberPointsPanelWidth,
                // Высота
                Height = ProgramSettings.MemberPointsPanelHeight,
                // Цвет контура
                Stroke = new SolidColorBrush(ProgramSettings.MemberPointsStrokeColor),
                // Толщина
                StrokeThickness = ProgramSettings.MemberPointsStrokeWidth,
                // Цвет фона
                Fill = new SolidColorBrush(ProgramSettings.MemberPointsPanelColor),
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center
            };

            // Имя участника
            TextBlock memberName = new TextBlock()
            {
                // Текст
                Text = name,
                // Тип щрифта
                FontFamily = new FontFamily("Calibri"),
                // Цвет шрифта
                Foreground = ProgramSettings.MemberNameFontColor,
                // Стиль текста
                FontWeight = ProgramSettings.MemberNameFontWeight,
                // Размер шрифта
                FontSize = ProgramSettings.MemberNameFontSize,
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center
            };

            // Количество баллов
            memberPoints = new Label()
            {
                // Размер шрифта
                FontSize = ProgramSettings.MemberPointsFontSize,
                // Стиль текста
                FontWeight = ProgramSettings.MemberPointsFontWeight,
                // Цвет текста
                Foreground = ProgramSettings.MemberPointsFontColor,
                // Вертикальное центрирование текста
                VerticalContentAlignment = VerticalAlignment.Center,
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center,
                // Горизонтальное центрирование
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Расставляем элементы
            mainPanel.Children.Add(mainRectangle);                                                                         // Фон
            mainPanel.Children.Add(memberName);                                                                            // Имя участника
            Canvas.SetTop(memberName, ProgramSettings.MemberPanelHeight / 2 - ProgramSettings.MemberNameFontSize / 2);
            Canvas.SetLeft(memberName, ProgramSettings.MemberPanelHeight + 10);                                                // Отодвигаем имя участника
            pointsPanel.Children.Add(pointsStroke);                                                                             // Контур баллов
            pointsPanel.Children.Add(memberPoints);                                                                             // Количество баллов
            mainPanel.Children.Add(pointsPanel);                                                                           // Панель с баллами

            #region Панель с предварительным местом

            // Проверяем, надо ли отображать текущее место участника в топе
            if (ProgramSettings.MemberPlaceShowMode == ProgramSettings.PlaceShowMode.VisibleOnFS ||
                ProgramSettings.MemberPlaceShowMode == ProgramSettings.PlaceShowMode.AlwaysVisible)
            {
                // Предварительное место участника в топе
                memberPlace = new Label()
                {
                    // Размер шрифта
                    FontSize = ProgramSettings.MemberPlaceFontSize,
                    // Стиль текста
                    FontWeight = ProgramSettings.MemberPlaceFontWeight,
                    // Цвет текста
                    Foreground = new SolidColorBrush(ProgramSettings.MemberPlaceFontColor),
                    // Вертикальное центрирование текста
                    VerticalAlignment = VerticalAlignment.Center,
                    // Горизонтальное выравнивание текста
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Контур вокруг предварительного места
                Rectangle placeBorder = new Rectangle()
                {
                    // Ширина
                    Width = ProgramSettings.MemberPanelHeight,
                    // Высота
                    Height = ProgramSettings.MemberPanelHeight,
                    // Цвет
                    Stroke = new SolidColorBrush(ProgramSettings.MemberPlaceStrokeColor),
                    // Толщина
                    StrokeThickness = ProgramSettings.MemberPlaceStrokeWidth,
                    // Вертикальное центрирование
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Цвет фона для предварительного места
                Rectangle placeBackground = new Rectangle()
                {
                    // Ширина
                    Width = ProgramSettings.MemberPanelHeight,
                    // Высота
                    Height = ProgramSettings.MemberPanelHeight,
                    // Цвет фона панели
                    Fill = new SolidColorBrush(ProgramSettings.MemberPlacePanelColor),
                    // Вертикальное центрирование
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Панель баллов
                placePanel = new Grid();

                // Выставляем компоненты на панели
                placePanel.Children.Add(placeBackground);
                placePanel.Children.Add(placeBorder);
                placePanel.Children.Add(memberPlace);

                // Добавляем возможность перемещать панель с предв. местом с помощью graphics
                placeBar = new Bar()
                {
                    mainPanel = placePanel,
                    BarTG = new TransformGroup()
                    {
                        Children =
                        {
                            new TranslateTransform(),
                            new ScaleTransform()
                        }
                    }
                };
                placeBar.mainPanel.RenderTransform = placeBar.BarTG;

                // Настраиваем прозрачность в соответствии с настройками
                placePanel.OpacityMask = Brushes.Black;

                if (ProgramSettings.MemberPlaceShowMode == ProgramSettings.PlaceShowMode.VisibleOnFS)
                {
                    placePanel.Opacity = 0;
                }
                else
                {
                    placePanel.Opacity = 1;
                }

                mainPanel.Children.Add(placePanel);
                Canvas.SetRight(placePanel, ProgramSettings.MemberPlacePanelOffset);

            }
            else
            {
                placeBar = new Bar()
                {
                    mainPanel = new Grid()
                    {
                        Opacity = 0
                    },
                    BarTG = new TransformGroup()
                    {
                        Children =
                        {
                            new TranslateTransform(),
                            new ScaleTransform()
                        }
                    }
                };
            }

            #endregion

            #region Панель дополнительных данных

            if (ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.Visible ||
                ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.AlwaysVisible)
            {
                // Предварительное место участника в топе
                memberResult = new Label()
                {
                    // Размер шрифта
                    FontSize = ProgramSettings.MemberResultFontSize,
                    // Стиль текста
                    FontWeight = ProgramSettings.MemberResultFontWeight,
                    // Цвет текста
                    Foreground = new SolidColorBrush(ProgramSettings.MemberResultFontColor),
                    // Вертикальное центрирование текста
                    VerticalAlignment = VerticalAlignment.Center,
                    // Горизонтальное выравнивание текста
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Контур вокруг предварительного места
                Rectangle valueBorder = new Rectangle()
                {
                    // Ширина
                    Width = ProgramSettings.MemberResultPanelWidth,
                    // Высота
                    Height = ProgramSettings.MemberResultPanelHeight,
                    // Цвет
                    Stroke = new SolidColorBrush(ProgramSettings.MemberResultStrokeColor),
                    // Толщина
                    StrokeThickness = ProgramSettings.MemberResultStrokeWidth,
                    // Вертикальное центрирование
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Цвет фона для предварительного места
                Rectangle valueBackground = new Rectangle()
                {
                    // Ширина
                    Width = ProgramSettings.MemberResultPanelWidth,
                    // Высота
                    Height = ProgramSettings.MemberResultPanelHeight,
                    // Цвет фона панели
                    Fill = new SolidColorBrush(ProgramSettings.MemberResultPanelColor),
                    // Вертикальное центрирование
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Инициализируем панели, расставляем внутренние элементы
                valuePanel = new Grid();
                valuePanel.Children.Add(valueBackground);
                valuePanel.Children.Add(valueBorder);
                valuePanel.Children.Add(memberResult);

                // Ставим собранную панель на главную панель, отодвигаем панель с предварительным местом
                mainPanel.Children.Add(valuePanel);
                if (placePanel != null)
                    Canvas.SetRight(placePanel, ProgramSettings.MemberPlacePanelOffset + ProgramSettings.MemberResultPanelWidth);
                Canvas.SetRight(valuePanel, ProgramSettings.MemberResultPanelOffset);

                valuePanel.OpacityMask = Brushes.Black;

                // Добавляем возможность изменять положение панели с доп. данными через класс graphics
                valueBar = new Bar()
                {
                    mainPanel = valuePanel,
                    BarTG = new TransformGroup()
                    {
                        Children =
                        {
                            new TranslateTransform(),
                            new ScaleTransform()
                        }
                    }
                };
                valueBar.mainPanel.RenderTransform = valueBar.BarTG;

                // Вычислим координаты, необходимые для ToggleBar
                valueClosedCord = new Point(ProgramSettings.MemberResultPanelWidth, 0);
                valueOpenedCord = new Point(0, 0);

                // Проверяем, надо ли постоянно показывать 
                if (ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.AlwaysVisible)
                {
                    // Оставляем понель видимой. Ничего не двигаем
                    ValueVisible = true;
                    valuePanel.Opacity = 1;
                    GraphicsEngine.MoveToInstant(placeBar, valueOpenedCord);
                }
                else
                {
                    if (ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.Visible)
                    {
                        // Делаем панель невидимой, смещаем её вправо
                        ValueVisible = false;
                        GraphicsEngine.MoveToInstant(placeBar, valueClosedCord);
                        valuePanel.Opacity = 0;
                    }
                }

                // Инициализируем необходимые переменные, которые необходимы для анимации ShowValue

                // Мы можем использовать анимацию ShowValue
                ShowValueAllowed = true;
            }
            else
                ShowValueAllowed = false;

            #endregion

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
            Canvas.SetTop(mainPanel, -(ProgramSettings.MemberPanelHeight / 2));
            Canvas.SetLeft(mainPanel, -(ProgramSettings.MemberPanelWidth / 2));
            mainPanel.RenderTransformOrigin = new Point(0.5, 0.5);

            //Canvas.SetBottom(MemberBarPanel, 500);
            //Canvas.SetLeft(MemberBarPanel, 500);

            //Делаем объект невидимым по умолчанию
            mainPanel.OpacityMask = new SolidColorBrush(Colors.White);
            mainPanel.Opacity = 0;

            // Обнуляем количество баллов
            Points = 0;

            // Обнуляем место в топе
            Place = 0;

            // ОБновляем результат
            Value = _value;
        }

        /// <summary>
        /// Показывает или скрывает результат участника
        /// </summary>
        /// <param name="ev"></param>
        public void ToggleValue(EventHandler ev = null)
        {
            // Проверяем доступность анимации
            if (ShowValueAllowed)
            {
                // Смотрим, показана ли панель или скрыта
                if (ValueVisible)
                {
                    // Скроем панель. Переместим панель с предвариетльным местом
                    GraphicsEngine.Disappear(valueBar, (eсv, args) =>
                    {
                        if (placeBar != null)
                            GraphicsEngine.MoveTo(placeBar, valueClosedCord);
                    });
                    ValueVisible = false;
                }
                else
                {
                    // Покажем панель
                    GraphicsEngine.barAppear(valueBar, ProgramSettings.MemberPanelOpacity);
                    GraphicsEngine.MoveTo(placeBar, valueOpenedCord);
                    ValueVisible = true;
                }
            }
            else
            {
                // Если не можем включить анимацию, то переключимся на следующее действие, если таковое есть
                if (ev != null)
                {
                    ev.Invoke(null, null);
                }
            }
        }
    }
}