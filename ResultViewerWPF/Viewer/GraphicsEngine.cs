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
    /// Устанавливает панель, в которую будут загружаться все графические элементы
    /// </summary>
    public class GraphicsEngine
    {
        /// <summary>
        /// Панель, куда будут загружаться все элементы
        /// </summary>
        public static Canvas CurrentCanvas = null;

        public static ViewerDebug Debug;
        static Random rnd = new Random();
        static SolidColorBrush kostil = new SolidColorBrush();

        #region Анимация перетекания баллов [not working]
        ///// <summary>
        ///// Анимация добавления баллов
        ///// </summary>
        //public Int32Animation pointsAnimation = new Int32Animation()
        //{
        //    To = 0,
        //    DecelerationRatio = 0.4,
        //    Duration = GraphicsSettings.AnimMoveTime
        //};

        ///// <summary>
        ///// Начать анимацию передачи баллов от PointBar к MemberBar-у
        ///// </summary>
        ///// <param name="pointBar">Ссылка на PointBar</param>
        //public void StartReducingAnimation(PointBar pointBar)
        //{
        //    // Настраиваем анимацию
        //    pointsAnimation.From = pointBar.NumOfPoints;

        //    // Запускаем анимацию
        //    pointsAnimation.BeginAnimation()
        //}
        #endregion

        /// <summary>
        /// Запускает бесконечную анимацию фона
        /// </summary>
        /// <param name="bcgBrush">Ссылка на анимируемый фон</param>
        public static void StartInfiniteBcgAnim(Brush bcgBrush)
        {
            // Создаём анимацию
            ColorAnimation colorAnimation = new ColorAnimation();

            // Настраиваем анимацию в соответствии с найтроками графики
            colorAnimation.From = ProgramSettings.BackgroundColor1;
            colorAnimation.To = ProgramSettings.BackgroundColor2;
            colorAnimation.Duration = ProgramSettings.BackgroundAnimPeriod;
            colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
            colorAnimation.AutoReverse = true;

            // Запускаем анимацию
            bcgBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | InfiniteBcgAnim");
            }
        }

        /// <summary>
        /// Запускет анимацю проявления фона
        /// </summary>
        /// <param name="bcgBrush">Ссылка на анимируемый фон</param>
        public static void BcgAppear(Brush bcgBrush)
        {
            // Создаём анимацию
            ColorAnimation colorAnimation = new ColorAnimation();

            // Настраиваем анимацию в соответствии с настройками графики
            colorAnimation.From = Colors.Black;
            colorAnimation.To = Colors.Transparent;
            colorAnimation.Duration = ProgramSettings.BackgroundAppearTime;
            colorAnimation.AutoReverse = false;
            colorAnimation.RepeatBehavior = new RepeatBehavior(1);

            // Запускаем анимацию
            bcgBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | BcgAppear");
            }
        }

        /// <summary>
        /// Меняет размеры объекта
        /// </summary>
        /// <param name="resizingBar">Панель, размеры которой необходимо изменить</param>
        /// <param name="destScale">Размеры, которых она должна достичь (defualt: 1)</param>
        /// <param name="afterCompleted">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void Resize(Bar resizingBar, double destScale, EventHandler afterCompleted = null)
        {
            ScaleTransform currentST = resizingBar.GetST();
            TranslateTransform currentTT = resizingBar.GetTT();

            currentST.CenterX = currentTT.X;
            currentST.CenterY = currentTT.Y;

            #region Position compensation

            DoubleAnimation scalingAnimationCenterX = new DoubleAnimation()
            {
                Duration = ProgramSettings.AnimAppearTime,
                DecelerationRatio = 0.4,
                FillBehavior = FillBehavior.Stop
            };

            scalingAnimationCenterX.From = currentST.CenterX;
            scalingAnimationCenterX.To = resizingBar.movingTo.X;

            scalingAnimationCenterX.Completed += (obj, ev) =>
            {
                currentST.CenterX = (double)scalingAnimationCenterX.To;
            };

            DoubleAnimation scalingAnimationCenterY = new DoubleAnimation()
            {
                Duration = ProgramSettings.AnimAppearTime,
                DecelerationRatio = 0.4,
                FillBehavior = FillBehavior.Stop
            };

            scalingAnimationCenterY.From = currentST.CenterY;
            scalingAnimationCenterY.To = resizingBar.movingTo.Y;

            scalingAnimationCenterY.Completed += (obj, ev) =>
            {
                currentST.CenterY = (double)scalingAnimationCenterY.To;
            };

            #endregion

            #region Scaling animation

            
            // Создаём анимации
            DoubleAnimation scalingAnimationAxisX = new DoubleAnimation()
            {
                Duration = ProgramSettings.AnimAppearTime,
                DecelerationRatio = 0.4,
                FillBehavior = FillBehavior.Stop
            };
            DoubleAnimation scalingAnimationAxisY = new DoubleAnimation()
            {
                Duration = ProgramSettings.AnimAppearTime,
                DecelerationRatio = 0.4,
                FillBehavior = FillBehavior.Stop
            };


            // Задаём для анимации текущие размеры объекта
            scalingAnimationAxisX.From = currentST.ScaleX;
            scalingAnimationAxisY.From = currentST.ScaleY;

            // Задаём конечные размеры объекта
            scalingAnimationAxisX.To = destScale;
            scalingAnimationAxisY.To = destScale;

            // Если есть что-то, что надо запустить только по завершении анимации, то вешаем его на Event завершения анимации
            if (afterCompleted != null)
                scalingAnimationAxisX.Completed += afterCompleted;

            // Применим изменения по завершению анимации
            scalingAnimationAxisX.Completed += (obj, ev) =>
            {
                currentST.ScaleX = destScale;
                currentST.ScaleY = destScale;
            };


            #endregion

            // Запускаем анимацию
            currentST.BeginAnimation(ScaleTransform.ScaleXProperty, scalingAnimationAxisX);
            currentST.BeginAnimation(ScaleTransform.ScaleYProperty, scalingAnimationAxisY);
            currentST.BeginAnimation(ScaleTransform.CenterXProperty, scalingAnimationCenterX);
            currentST.BeginAnimation(ScaleTransform.CenterYProperty, scalingAnimationCenterY);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | Resise | destScale: {destScale} | afterCompleted? {afterCompleted != null}");

                Rectangle testObject1 = new Rectangle()
                {
                    Fill = Brushes.Green,
                    Width = 5,
                    Height = 5
                };
                CurrentCanvas.Children.Add(testObject1);
                Canvas.SetLeft(testObject1, currentST.CenterX);
                Canvas.SetTop(testObject1, currentST.CenterY);

                Rectangle testObject2 = new Rectangle()
                {
                    Fill = Brushes.Green,
                    Width = 5,
                    Height = 5
                };
                CurrentCanvas.Children.Add(testObject2);
                Canvas.SetLeft(testObject2, (double)scalingAnimationCenterX.To);
                Canvas.SetTop(testObject2, (double)scalingAnimationCenterY.To);

                scalingAnimationAxisX.Completed += (obj, ev) =>
                {
                    Debug.Add($"Animation | {id} | Complete | Resize | destScale: {destScale} | afterCompleted? {afterCompleted != null}");
                    CurrentCanvas.Children.Remove(testObject1);
                    CurrentCanvas.Children.Remove(testObject2);
                };
            };
        }

        /// <summary>
        /// Меняет цвет заданному участнику
        /// </summary>
        /// <param name="coloringBar">Участник, цвет которого надо инвертировать</param>
        /// <param name="afterCompleted">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void ChangeMemberColor(MemberBar coloringBar, Color _destColor, EventHandler afterCompleted = null)
        {
            // Смотрим, какой цвет нам нужен
            Color destColor = _destColor, 
                  currentColor = (coloringBar.mainRectangle.Fill as SolidColorBrush).Color;
            
            // Создаём соответствующую анимацию
            ColorAnimation colorAnimation = new ColorAnimation()
            {
                // Длительность анимации
                Duration = ProgramSettings.AnimAppearTime,
                // Для корректной работы события Completed
                FillBehavior = FillBehavior.Stop,
                // Начальный цвет
                From = currentColor,
                // Конечный цвет
                To = destColor
            };

            // По завершении анимации применим все изменения
            colorAnimation.Completed += (obj, ev) =>
            {
                coloringBar.mainRectangle.Fill = new SolidColorBrush(destColor);
            };

            // Если есть что-то, что необходимо сделать по завершении анимации:
            if (afterCompleted != null)
                colorAnimation.Completed += afterCompleted;

            // Запускаем анимацию
            coloringBar.mainRectangle.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | ChangeMemberColor | MemberName: {coloringBar.Name} | AfterCompleted? {afterCompleted != null}");

                colorAnimation.Completed += (pbj, ev) =>
                {
                    Debug.Add($"Animation | {id} | Complete | ChangeMemberColor | MemberName: {coloringBar.Name} | AfterCompleted? {afterCompleted != null}");
                };
            }
        }

        /// <summary>
        /// Перемещает панель в заданную точку
        /// </summary>
        /// <param name="movingBar">Панель, которую необходимо переместить</param>
        /// <param name="destinationPoint">Точка назначения</param>
        /// <param name="nextAnim">Следующее действие, которое будет выполнено немедленно после начала работы анимации</param>
        /// <param name="afterCompleted">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void MoveTo(Bar movingBar, Point destinationPoint, EventHandler afterCompleted = null)
        {

            // Создаём анимации
            DoubleAnimation movingAnimationAxisX = new DoubleAnimation()
            {
                Duration = ProgramSettings.AnimAppearTime,
                DecelerationRatio = 0.4,
                FillBehavior = FillBehavior.Stop
            };
            DoubleAnimation movingAnimationAxisY = new DoubleAnimation()
            {                
                Duration = ProgramSettings.AnimAppearTime,
                DecelerationRatio = 0.4,                
                FillBehavior = FillBehavior.Stop
            };

            // Работаем с TranslateTransform, который находится в коллекции TranformGroup под индексом 0
            TranslateTransform currentTT = movingBar.GetTT();

            // Задаём для анимации начальные координаты
            movingAnimationAxisX.From = currentTT.X;
            movingAnimationAxisY.From = currentTT.Y;

            // Задаём конечные координаты
            movingAnimationAxisX.To = destinationPoint.X;
            movingAnimationAxisY.To = destinationPoint.Y;
            movingBar.movingTo = destinationPoint;
            
            // По завершении анимации применим все изменения
            movingAnimationAxisX.Completed += (obj, ev) =>
            {
                currentTT.X = destinationPoint.X;
                currentTT.Y = destinationPoint.Y;
            };

            // Если есть что-то, что надо запустить только по завершении анимации, то вешаем его на Event завершения анимации
            if (afterCompleted != null)
                movingAnimationAxisX.Completed += afterCompleted;

            // Запускаем анимацию
            currentTT.BeginAnimation(TranslateTransform.XProperty, movingAnimationAxisX);
            currentTT.BeginAnimation(TranslateTransform.YProperty, movingAnimationAxisY);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | MoveTo | Destpoint: X{destinationPoint.X} Y{destinationPoint.Y} | AfterCompleted? {afterCompleted != null}");

                Rectangle testObject = new Rectangle()
                {
                    Name = "testObj",
                    Fill = Brushes.Red,
                    Width = 10,
                    Height = 10
                };
                CurrentCanvas.Children.Add(testObject);
                Canvas.SetLeft(testObject, destinationPoint.X);
                Canvas.SetTop(testObject, destinationPoint.Y);

                movingAnimationAxisX.Completed += (obj, ev) =>
                {
                    Debug.Add($"Animation | {id} | Complete | MoveTo | Destpoint: X{destinationPoint.X} Y{destinationPoint.Y} | AfterCompleted? {afterCompleted != null}");
                    CurrentCanvas.Children.Remove(testObject);
                };
            }
        }

        /// <summary>
        /// Пауза
        /// </summary>
        /// <param name="time">Время для паузы</param>
        /// <param name="afterCompleted">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void Wait(TimeSpan time, EventHandler afterCompleted = null)
        {
            // Создаём факовую анимацию
            ColorAnimation fakeAnim = new ColorAnimation()
            {
                // Длительнроть фейковой анимации
                Duration = time,
                // Для корректного завершения анимации
                FillBehavior = FillBehavior.Stop,
                From = Colors.AliceBlue,
                To = Colors.AntiqueWhite
            };

            // Если есть что-то, что надо запустить только по завершении анимации, то вешаем его на Event завершения анимации
            if (afterCompleted != null)
                fakeAnim.Completed += afterCompleted;


            // Запускаем фейковую анимацию
            kostil.BeginAnimation(SolidColorBrush.ColorProperty, fakeAnim);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | Wait | Time: {time.TotalMilliseconds}ms | AfterCompleted? {afterCompleted != null}");

                fakeAnim.Completed += (obj, ev) =>
                {
                    Debug.Add($"Animation | {id} | Complete | Wait | Time: {time.TotalMilliseconds}ms | AfterCompleted? {afterCompleted != null}");
                };
            }

        }

        /// <summary>
        /// Моментально перемещает панель в заданную точку
        /// </summary>
        /// <param name="movingBar">Панель, которую необходимо переместить</param>
        /// <param name="destinationPoint">Точка назначения</param>
        public static void MoveToInstant(Bar movingBar, Point destinationPoint)
        {
            // Работаем с TranslateTransform, который находится в коллекции TranformGroup под индексом 0
            TranslateTransform currentTT = movingBar.GetTT();

            // Перемещаем панель
            currentTT.X = destinationPoint.X;
            currentTT.Y = destinationPoint.Y;

            // Обновляем координаты объекта
            movingBar.movingTo = destinationPoint;

            if (Debug != null)
            {
                Debug.Add($"Animation | Start | MoveToInstant | Destpoint: X{destinationPoint.X} Y{destinationPoint.Y}");
            }
        }


        /// <summary>
        /// Делает панель MemberBar видимой
        /// </summary>
        /// <param name="memberBar">Панель типа MemberBar, которую необходимо проявить</param>
        /// <param name="afterComplited">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void Appear(MemberBar memberBar, EventHandler afterCompleted = null) => barAppear(memberBar, ProgramSettings.MemberPanelOpacity, afterCompleted);

        /// <summary>
        /// Делает панель JuryBar видимой
        /// </summary>
        /// <param name="juryBar">Панель типа JuryBar, которую необходимо проявить</param>
        /// <param name="afterComplited">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void Appear(JuryBar juryBar, EventHandler afterCompleted = null) => barAppear(juryBar, ProgramSettings.JuryPanelOpacity, afterCompleted);

        /// <summary>
        /// Делает панель TextBar видимой
        /// </summary>
        /// <param name="textBar"></param>
        /// <param name="textBar">Панель типа TextBar, которую необходимо проявить</param>
        /// <param name="afterComplited">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void Appear(TextBar textBar, EventHandler afterCompleted = null)
        {
            barAppear(textBar, 1, afterCompleted);
            textBar.IsVisible = true;
        } 

        /// <summary>
        /// Делает панель видимой
        /// </summary>
        /// <param name="pointBar">Панель типа PointBar, которую необходимо проявить</param>
        /// <param name="afterComplited">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void Appear(PointBar pointBar, EventHandler afterCompleted = null) => barAppear(pointBar, ProgramSettings.PointBarPanelOpacity, afterCompleted);
        
        public static void barAppear(Bar appearingPanel, double finalOpacity, EventHandler afterCompleted = null)
        {
            // Создаём анимацию
            DoubleAnimation appearAnimation = new DoubleAnimation()
            {
                // Из прозрачного
                From = 0,
                // В любой другой
                To = finalOpacity,
                // Время проявления
                Duration = ProgramSettings.AnimAppearTime,
                // Для корректного завершения анимации
                FillBehavior = FillBehavior.Stop
            };

            // Применим изменения по окончанию анимации
            appearAnimation.Completed += (obj, ev) =>
            {
                appearingPanel.mainPanel.Opacity = finalOpacity;
            };

            // Если есть что-то, что надо запустить только по завершении анимации, то вешаем его на Event завершения анимации
            if (afterCompleted != null)
            {
                appearAnimation.Completed += afterCompleted;
            }

            // Проявляем панель
            appearingPanel.mainPanel.BeginAnimation(Grid.OpacityProperty, appearAnimation);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | Appear | afterCompleted? {afterCompleted != null}");

                appearAnimation.Completed += (obj, ev) =>
                {
                    Debug.Add($"Animation | {id} | Complete | Appear | afterCompleted? {afterCompleted != null}");
                };
            }
        }

        /// <summary>
        /// Делает панель невидимой
        /// </summary>
        /// <param name="disapperaingPanel">Панель, которую необходимо затемнить</param>
        /// <param name="afterComplited">Следующее действие, которое будет выполнено только по завершении работы данной анимамции</param>
        public static void Disappear(Bar disapperaingPanel, EventHandler afterCompleted = null)
        {
            // Создаём анимацию
            DoubleAnimation disappearAnimation = new DoubleAnimation()
            {
                // Из непрозрачного
                From = ProgramSettings.JuryPanelOpacity,
                // В любой другой
                To = 0,
                // Время проявления
                Duration = ProgramSettings.AnimAppearTime,
                // Для корректного завершения анимации
                FillBehavior = FillBehavior.Stop
            };

            // По завершении анимации примерним все изменения
            disappearAnimation.Completed += (obj, ev) =>
            {
                disapperaingPanel.mainPanel.Opacity = 0;
            };

            // Если есть что-то, что надо запустить только по завершении анимации, то вешаем его на Event завершения анимации
            if (afterCompleted != null)
                disappearAnimation.Completed += afterCompleted;

            // Спрячем панель
            disapperaingPanel.mainPanel.BeginAnimation(Panel.OpacityProperty, disappearAnimation);

            if (Debug != null)
            {
                int id = rnd.Next(9999);

                Debug.Add($"Animation | {id} | Start | Disappear | afterCompleted? {afterCompleted != null}");

                disappearAnimation.Completed += (obj, ev) =>
                {
                    Debug.Add($"Animation | {id} | Stop | Disappear | afterCompleted? {afterCompleted != null}");
                };
            }

        }
    }
}
