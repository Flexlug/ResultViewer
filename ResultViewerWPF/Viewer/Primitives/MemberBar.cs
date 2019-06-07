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

namespace ResultViewerWPF.Viewer.Primitives
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

        private bool _showMask = false;
        /// <summary>
        /// Отображение маски поверх баллов
        /// </summary>
        public bool ShowMask
        {
            get
            {
                return _showMask;
            }
            set
            {
                _showMask = value;

                // Обновим счётчик баллов
                Points = _numOfPoints;
            }
        }

        /// <summary>
        /// Тип маски, в зависимости от ситуации
        /// </summary>
        public enum MaskType
        {
            /// <summary>
            /// У участника нет баллов в данном туре
            /// </summary>
            NoPoints,

            /// <summary>
            /// Участник отсутствовал в данном туре
            /// </summary>
            Absent
        }

        /// <summary>
        /// Выбранный тип маски для данного участника
        /// </summary>
        public MaskType MemberMaskType = MaskType.NoPoints;

        /// <summary>
        /// Показывает, выделена ли панель участника первым цветом
        /// </summary>
        public bool IsChosen = false;

        /// <summary>
        /// Показывает, отображает ли на данный момент панель с дополнительными результатами
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

        Label memberPlace = null;
        private double _numOfPoints = 0;
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
                
                if (_showMask)
                {
                    // Если включено отображение маски, то в зависимости от выбранной маски отображаем соответствующий символ
                    switch (MemberMaskType)
                    {
                        case MaskType.Absent:
                            memberPoints.Content = "Н";
                            break;

                        case MaskType.NoPoints:
                            memberPoints.Content = "X";
                            break;
                    }
                }
                else
                {
                    if (memberPoints != null)
                        memberPoints.Content = $"{_numOfPoints}";
                }
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
                Width = Program.Settings.MemberPanelWidth,
                // Высота
                Height = Program.Settings.MemberPanelHeight,
                // Цвет фона панели
                Fill = new SolidColorBrush(Program.Settings.MemberPanelColor),
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center,
                // Цвет контура
                Stroke = new SolidColorBrush(Program.Settings.MemberPanelStrokeColor),
                // Ширина контура
                StrokeThickness = Program.Settings.MemberPanelStrokeWidth,
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
                Width = Program.Settings.MemberPointsPanelWidth,
                // Высота
                Height = Program.Settings.MemberPointsPanelHeight,
                // Цвет контура
                Stroke = new SolidColorBrush(Program.Settings.MemberPointsStrokeColor),
                // Толщина
                StrokeThickness = Program.Settings.MemberPointsStrokeWidth,
                // Цвет фона
                Fill = new SolidColorBrush(Program.Settings.MemberPointsPanelColor),
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
                Foreground = Program.Settings.MemberNameFontColor,
                // Стиль текста
                FontWeight = Program.Settings.MemberNameFontWeight,
                // Размер шрифта
                FontSize = Program.Settings.MemberNameFontSize,
                // Вертикальное центрирование
                VerticalAlignment = VerticalAlignment.Center
            };

            // Количество баллов
            memberPoints = new Label()
            {
                // Размер шрифта
                FontSize = Program.Settings.MemberPointsFontSize,
                // Стиль текста
                FontWeight = Program.Settings.MemberPointsFontWeight,
                // Цвет текста
                Foreground = Program.Settings.MemberPointsFontColor,
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
            Canvas.SetTop(memberName, Program.Settings.MemberPanelHeight / 2 - Program.Settings.MemberNameFontSize / 2);
            Canvas.SetLeft(mainRectangle, Program.Settings.MemberPointsPanelWidth);                                                // Отодвигаем главную панель с фоном
            Canvas.SetLeft(memberName, Program.Settings.MemberPointsPanelWidth + 10);                                                // Отодвигаем имя
            pointsPanel.Children.Add(pointsStroke);                                                                             // Контур баллов
            pointsPanel.Children.Add(memberPoints);                                                                             // Количество баллов
            mainPanel.Children.Add(pointsPanel);                                                                           // Панель с баллами

            #region Панель с предварительным местом

            // Проверяем, надо ли отображать текущее место участника в топе
            if (Program.Settings.MemberPlaceShowMode == Program.Settings.PlaceShowMode.VisibleOnFS ||
                Program.Settings.MemberPlaceShowMode == Program.Settings.PlaceShowMode.AlwaysVisible)
            {
                // Предварительное место участника в топе
                memberPlace = new Label()
                {
                    // Размер шрифта
                    FontSize = Program.Settings.MemberPlaceFontSize,
                    // Стиль текста
                    FontWeight = Program.Settings.MemberPlaceFontWeight,
                    // Цвет текста
                    Foreground = new SolidColorBrush(Program.Settings.MemberPlaceFontColor),
                    // Вертикальное центрирование текста
                    VerticalAlignment = VerticalAlignment.Center,
                    // Горизонтальное выравнивание текста
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Контур вокруг предварительного места
                Rectangle placeBorder = new Rectangle()
                {
                    // Ширина
                    Width = Program.Settings.MemberPanelHeight,
                    // Высота
                    Height = Program.Settings.MemberPanelHeight,
                    // Цвет
                    Stroke = new SolidColorBrush(Program.Settings.MemberPlaceStrokeColor),
                    // Толщина
                    StrokeThickness = Program.Settings.MemberPlaceStrokeWidth,
                    // Вертикальное центрирование
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Цвет фона для предварительного места
                Rectangle placeBackground = new Rectangle()
                {
                    // Ширина
                    Width = Program.Settings.MemberPanelHeight,
                    // Высота
                    Height = Program.Settings.MemberPanelHeight,
                    // Цвет фона панели
                    Fill = new SolidColorBrush(Program.Settings.MemberPlacePanelColor),
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

                if (Program.Settings.MemberPlaceShowMode == Program.Settings.PlaceShowMode.VisibleOnFS)
                {
                    placePanel.Opacity = 0;
                }
                else
                {
                    placePanel.Opacity = 1;
                }

                mainPanel.Children.Add(placePanel);
                Canvas.SetRight(placePanel, Program.Settings.MemberPlacePanelOffset);

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

            if (Program.Settings.ShowMemberResultMode == Program.Settings.ResultShowMode.Visible ||
                Program.Settings.ShowMemberResultMode == Program.Settings.ResultShowMode.AlwaysVisible)
            {
                // Предварительное место участника в топе
                memberResult = new Label()
                {
                    // Размер шрифта
                    FontSize = Program.Settings.MemberResultFontSize,
                    // Стиль текста
                    FontWeight = Program.Settings.MemberResultFontWeight,
                    // Цвет текста
                    Foreground = new SolidColorBrush(Program.Settings.MemberResultFontColor),
                    // Вертикальное центрирование текста
                    VerticalAlignment = VerticalAlignment.Center,
                    // Горизонтальное выравнивание текста
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Контур вокруг предварительного места
                Rectangle valueBorder = new Rectangle()
                {
                    // Ширина
                    Width = Program.Settings.MemberResultPanelWidth,
                    // Высота
                    Height = Program.Settings.MemberResultPanelHeight,
                    // Цвет
                    Stroke = new SolidColorBrush(Program.Settings.MemberResultStrokeColor),
                    // Толщина
                    StrokeThickness = Program.Settings.MemberResultStrokeWidth,
                    // Вертикальное центрирование
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Цвет фона для предварительного места
                Rectangle valueBackground = new Rectangle()
                {
                    // Ширина
                    Width = Program.Settings.MemberResultPanelWidth,
                    // Высота
                    Height = Program.Settings.MemberResultPanelHeight,
                    // Цвет фона панели
                    Fill = new SolidColorBrush(Program.Settings.MemberResultPanelColor),
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
                    Canvas.SetRight(placePanel, Program.Settings.MemberPlacePanelOffset + Program.Settings.MemberResultPanelWidth);
                Canvas.SetRight(valuePanel, Program.Settings.MemberResultPanelOffset);

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
                valueClosedCord = new Point(Program.Settings.MemberResultPanelWidth, 0);
                valueOpenedCord = new Point(0, 0);

                // Проверяем, надо ли постоянно показывать 
                if (Program.Settings.ShowMemberResultMode == Program.Settings.ResultShowMode.AlwaysVisible)
                {
                    // Оставляем панель видимой. Ничего не двигаем
                    ValueVisible = true;
                    valuePanel.Opacity = 1;
                    GraphicsEngine.MoveToInstant(placeBar, valueOpenedCord);
                }
                else
                {
                    if (Program.Settings.ShowMemberResultMode == Program.Settings.ResultShowMode.Visible)
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
            Canvas.SetTop(mainPanel, -(Program.Settings.MemberPanelHeight / 2));
            Canvas.SetLeft(mainPanel, -(Program.Settings.MemberPanelWidth / 2));
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

            // Обновляем результат
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
                    GraphicsEngine.barAppear(valueBar, Program.Settings.MemberPanelOpacity);
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