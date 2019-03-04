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
using System.Reflection;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Interop;
using System.IO;

namespace ResultViewerWPF.Viewer

{
    /// <summary>
    /// Логика взаимодействия для Viewer.xaml
    /// </summary>
    public partial class ContentViewer : Window
    {
        /// <summary>
        /// Возможные стадии показа
        /// </summary>
        enum ShowState
        {
            ShowField,
            ShowingPoints,
            FinalScreen
        }

        /// <summary>
        /// Последний участник, на котором был остановлен выбор
        /// </summary>
        int memberIterator = 0;

        /// <summary>
        /// Жюри, выбор которого сейчас демонстрируется
        /// </summary>
        int currentJury = 0;

        /// <summary>
        /// Показывает, идет ли сейчас какая-нибудь анимация
        /// </summary>
        bool isBusy = false;

        /// <summary>
        /// Промежуточный массив, который необходим для вычисления следующего балла при режиме Ascending или Descending у PointsMode
        /// </summary>
        double[][] memberPoints = null;

        /// <summary>
        /// Массив с дополнительными результатами участников
        /// </summary>
        double[][] memberValues = null;

        /// <summary>
        /// Показывает, в какой стадии сейчас находится показ
        /// </summary>
        ShowState currentState;

        /// <summary>
        /// Логика приложения со всеми данными
        /// </summary>
        Logic appLogic;

        /// <summary>
        /// Расчитывает координаты для всех элементов
        /// </summary>
        CoordinatesProvider coordinates;

        /// <summary>
        /// Коллекция, содержащая в себе визуальную составлающую участников
        /// </summary>
        List<MemberBar> memberPanels = new List<MemberBar>();

        /// <summary>
        /// Коллекция, содержащая в себе визуальную составлающую жюри
        /// </summary>
        List<JuryBar> juryPanels = new List<JuryBar>();

        /// <summary>
        /// Панель с баллом участника
        /// </summary>
        PointBar pointBar;

        /// <summary>
        /// Инициализирует визуализатор результатов конкурса
        /// </summary>
        /// <param name="logic">Логика приложения со всеми необходимыми данными</param>
        public ContentViewer(Logic logic)
        {
            InitializeComponent();

            // Проверим, достаточно ли у нас жюри, чтобы мы имели возможность кого-то пропустить
            if (ProgramSettings.StartJury >= logic.GetJuryChoice().Count)
            {
                MessageBox.Show("Невозможно начать показ с заданного жюри. Проверьте параметр, отвечающий за то, с какого жюри начинается показ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получим ссылку на логику
            appLogic = logic;
            
            // Стадия показа: загрузить начальные данные
            currentState = ShowState.ShowField;
            
            // Указываем рабочую поверхность
            GraphicsEngine.CurrentCanvas = mainCanvas;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isBusy)
            {
                // Блокируем ввода пользователя во время выполнения анимации
                isBusy = true;
                
                switch (currentState)
                {
                    case ShowState.ShowField:
                        // Проявление фона

                        // Присваиваем фону чёрный цвет
                        SolidColorBrush backgroundColor = new SolidColorBrush(Colors.Black);
                        mainCanvas.Background = backgroundColor;

                        // Запускаем анимацию проявления
                        GraphicsEngine.BcgAppear(mainCanvas.Background);

                        // Анимированный фон

                        // Если анимированный фон включён
                        if (ProgramSettings.AnimatedBackground)
                        {
                            // Если в качестве анимации выступает видео
                            if (ProgramSettings.VideoBackground)
                            {
                                // Проверим наличие файла
                                if (File.Exists(ProgramSettings.VideoPath))
                                {
                                    // На случай, если попадётся битый файл
                                    try
                                    {
                                        // Загрузим видео на фон....

                                        MediaElement video = new MediaElement()
                                        {
                                            Source = new Uri(ProgramSettings.VideoPath),
                                            LoadedBehavior = MediaState.Manual,
                                            UnloadedBehavior = MediaState.Manual,
                                            Volume = 0,
                                        };

                                        video.MediaEnded += (obj, ev) =>
                                        {
                                            video.Stop();
                                            video.Play();
                                        };

                                        Background = new VisualBrush()
                                        {
                                            Visual = video
                                        };

                                        video.Play();
                                    }
                                    catch(Exception exc)
                                    {
                                        MessageBox.Show("Ошибка при загрузке видео" + ProgramSettings.VideoPath, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                        Background = ProgramSettings.MainBackground;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"Не получилось загрузить видео на фон. Проверьте наличие файла {ProgramSettings.VideoPath}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    Background = ProgramSettings.MainBackground;
                                }
                            }
                            else
                            {
                                // Присваиваем фону новый цвет
                                Background = new SolidColorBrush(ProgramSettings.BackgroundColor1);

                                // Запускаем анимацию
                                GraphicsEngine.StartInfiniteBcgAnim(Background);
                            }
                        };


                        #region Расстановка и анимация объектов

                        // Прогрузим промежуточный массив с баллами участника
                        memberPoints = appLogic.GetPoints();
                        
                        // Загрузим жюри
                        string[] juryNames = appLogic.GetJuryNames();
                        foreach (string jur in juryNames)
                            juryPanels.Add(new JuryBar(mainCanvas, jur));
                        
                        // Загрузим участников
                        string[] memberNames = appLogic.GetMemberNames();
                        for (int i = 0; i < memberNames.Length; i++)
                            memberPanels.Add(new MemberBar(mainCanvas, memberNames[i], i));

                        // Грузим дополнительные данные участников, если показ таковых включен
                        if (ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.Visible ||
                            ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.AlwaysVisible)
                        {
                            // Загрузим массив с дополнительными результатами
                            memberValues = appLogic.GetValues();

                            // Проставляем участникам их дополнительные результаты
                            for (int i = 0; i < memberPanels.Count; i++)
                                memberPanels[i].Value = memberValues[currentJury][i];
                        }

                        // Проверим, имеются ли участники, которые не получили в данном туре баллов
                        CheckZeroPoints();

                        // Проверим, с какого жюри нам надо начать показ
                        if (ProgramSettings.StartJury != 0)
                        {
                            // Просчитаем все баллы, которые необходимо добавить
                            int[] calculatedPoints = new int[memberNames.Length];

                            // Проставим просчитанные баллы участникам
                            for (int jur = 0; jur < ProgramSettings.StartJury; jur++)
                                for (int mem = 0; mem < memberNames.Length; mem++)
                                    memberPanels[mem].Points += appLogic.GetPoint(jur, mem);

                            // Отсортируем список
                            SortMembers();
                            
                            //Зададим начального жюри
                            currentJury = ProgramSettings.StartJury;
                        }

                        // Покажем жюри
                        GraphicsEngine.Appear(juryPanels[currentJury], (obj, ev) => {
                            isBusy = false;
                        });

                        // Переместим его с центра экрана
                        GraphicsEngine.MoveTo(juryPanels[currentJury], coordinates.Jury());

                        // Отсортируем участников по алфавиту
                        SortMembers();

                        // Расставим участников конкурса по экрану в соответствии с координатной системой
                        for (int i = 0; i < memberPanels.Count; i++)
                        {
                            memberPanels[i].Place = 1;
                            GraphicsEngine.Appear(memberPanels[i]);
                            GraphicsEngine.MoveTo(memberPanels[i], coordinates.Member(i));
                        }

                        #endregion

                        // Стадия показа: показываем анимацию баллов
                        currentState = ShowState.ShowingPoints;
                        break;

                    case ShowState.ShowingPoints:

                        // Проверяем, есть ли еще участники с неприсвоенными баллами
                        if (CheckPoint())
                        {
                            // Меняем цвет участников на стандартный после прошлого жюри
                            for (int i = 0; i < memberPanels.Count; i++)
                            {
                                if (memberPanels[i].IsChosen)
                                {
                                    if (ProgramSettings.MemberPanelUseSecondChooseColor)
                                    {
                                        // Перекрашиваем участника во второй цвет
                                        GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelChosenColor2);
                                        memberPanels[i].IsColored = true;
                                        memberPanels[i].IsChosen = false;
                                    }
                                    else
                                    {
                                        // Возвращаем участнику стандартный цвет
                                        GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelColor);
                                        memberPanels[i].IsChosen = false;
                                    }
                                }
                            }

                            // Получаем балл выбранного участника, присваиваем его PointBar-у
                            pointBar.NumOfPoints = appLogic.GetPoint(currentJury, memberIterator);

                            // Получаем ссылку на выбранного участника
                            MemberBar currentMem = GetCurrentMember();

                            // Начинаем анимацию
                            GraphicsEngine.MoveToInstant(pointBar, coordinates.PointBar(-1));

                            // Показываем результат участника
                            // Только в случае, если показ результатов включен в принципе и результаты не показаны по дефолту
                            if (ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.Visible)
                                currentMem.ToggleValue();
                            
                            GraphicsEngine.ChangeMemberColor(currentMem, ProgramSettings.MemberPanelChosenColor ,(obj, ev) =>
                            {
                                currentMem.IsChosen = true;
                                GraphicsEngine.Wait(ProgramSettings.AnimPause, new EventHandler((obj1, ev1) =>
                                {
                                    GraphicsEngine.Appear(pointBar);
                                    GraphicsEngine.Resize(pointBar, 1.5, new EventHandler((obj2, ev2) =>
                                    {
                                        GraphicsEngine.Wait(ProgramSettings.AnimPointBarPause, new EventHandler((obj8, ev8) =>
                                        {
                                            GraphicsEngine.MoveTo(pointBar, coordinates.PointBar(memberPanels.IndexOf(currentMem)));
                                            GraphicsEngine.Resize(pointBar, 0.1);
                                            currentMem.Points += pointBar.NumOfPoints;
                                            GraphicsEngine.Disappear(pointBar, new EventHandler((obj3, ev3) =>
                                            {
                                                SortMembers();
                                                UpdatePlaces();

                                                // Обновляем позиции участников
                                                for (int i = 0; i < memberPanels.Count; i++)
                                                {
                                                    GraphicsEngine.MoveTo(memberPanels[i], coordinates.Member(i), new EventHandler((objc, evc) =>
                                                    {
                                                        isBusy = false;
                                                    }));
                                                }

                                                if (ProgramSettings.MemberPointsMode == ProgramSettings.PointsMode.Standard)
                                                    memberIterator++;
                                            }));
                                        }));
                                    }));
                                }));
                            });
                        }
                        else
                        {
                            // С этим жюри закончили. Проверяем, не последний ли это жюри
                            if (currentJury + 1 != juryPanels.Count)
                            {
                                // Делаем анимацию для смени жюри
                                GraphicsEngine.Disappear(juryPanels[currentJury]);

                                // Ставим следующего жюри, обнуляем указатель на участника
                                currentJury++;
                                memberIterator = 0;

                                // Меняем все цвета участников на стандартные
                                foreach (MemberBar mem in memberPanels)
                                {
                                    if (mem.IsColored || mem.IsChosen)
                                    {
                                        GraphicsEngine.ChangeMemberColor(mem, ProgramSettings.MemberPanelColor);
                                        mem.IsChosen = false;
                                        mem.IsColored = false;
                                    }
                                    // Скрываем результаты каждого участника
                                    if (mem.ValueVisible)
                                        mem.ToggleValue();

                                    // Обновим баллы участников
                                    if (memberValues != null)
                                        for (int i = 0; i < memberPanels.Count; i++)
                                            memberPanels[i].Value = memberValues[currentJury][i];
                                }


                                // Проверим, имеются ли участники, которые не получили в данном туре баллов
                                CheckZeroPoints();

                                // Обновим позиции участников
                                SortMembers();
                                for (int mem = 0; mem < memberPanels.Count; mem++)
                                    GraphicsEngine.MoveTo(memberPanels[mem], coordinates.Member(mem));

                                // Проявляем нового жюри, перемещаем его в центр
                                GraphicsEngine.Appear(juryPanels[currentJury]);
                                GraphicsEngine.MoveTo(juryPanels[currentJury], coordinates.Jury(), (obj, ev) =>
                                {
                                    isBusy = false;
                                });
                            }
                            else
                            {
                                // Скрываем последние дополнительные результаты участников
                                foreach (MemberBar mem in memberPanels)
                                    if (mem.ValueVisible)
                                        mem.ToggleValue();

                                // Проверяем, надо ли выделить лидеров отдельными цветами
                                if (ProgramSettings.MemberPanelHighlightLeaders)
                                {
                                    for (int i = 0; i < memberPanels.Count; i++)
                                    {
                                        if (memberPanels[i].Place == 1)
                                        {
                                            GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelFirstPlace);
                                        }
                                        else
                                        {
                                            if (memberPanels[i].Place == 2)
                                            {
                                                GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelSecondPlace);
                                            }
                                            else
                                            {
                                                if (memberPanels[i].Place == 3)
                                                {
                                                    GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelThirdPlace);
                                                }
                                                else
                                                {
                                                    GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelOtherPlaces);
                                                }
                                            }
                                        }
                                    };
                                }
                                else
                                {
                                    // Если выделять участников не надо, то меняем цвет на стандартный цвет панели участника
                                    foreach (MemberBar mem in memberPanels)
                                        if (mem.IsChosen || mem.IsColored)
                                            GraphicsEngine.ChangeMemberColor(mem, ProgramSettings.MemberPanelColor);
                                }

                                // Проверяем, надо ли проявить панели предварительных мест по достижении 
                                if (ProgramSettings.MemberPlaceShowMode == ProgramSettings.PlaceShowMode.VisibleOnFS)
                                {
                                    foreach (MemberBar mem in memberPanels)
                                    {
                                        GraphicsEngine.barAppear(new Bar() { mainPanel = mem.placePanel }, ProgramSettings.MemberPanelOpacity);
                                    }
                                }

                                // Показываем финальную фразу
                                JuryBar finalBar = new JuryBar(mainCanvas, ProgramSettings.FinalPhrase);
                                GraphicsEngine.Disappear(juryPanels[currentJury]);
                                GraphicsEngine.MoveTo(finalBar, coordinates.Jury());
                                GraphicsEngine.Appear(finalBar, (obj, ev) =>
                                {
                                    isBusy = false;
                                });

                                // Выключаем все буквы
                                for (int mem = 0; mem < memberPanels.Count; mem++)
                                    memberPanels[mem].ShowMask = false;

                                // Завершаем показ, включаем "заглушку"
                                currentState = ShowState.FinalScreen;
                            }
                        }

                        //// Балл появляется
                        //graphics.MoveToInstant(pointBar, coordinates.PointBar(-1));
                        //graphics.Appear(pointBar, (obj, ev) =>
                        //{
                        //    // Балл прилетает к участнику
                        //    graphics.Disappear(pointBar);
                        //    graphics.MoveTo(pointBar, coordinates.PointBar(i), (obg, es) =>
                        //    {
                        //        // Балл присваивается
                        //        memberPanels[i].NumOfPoints += 55;
                        //        memberPanels = memberPanels.OrderByDescending(x => x.NumOfPoints).ToList();

                        //        // Перестановка участников
                        //        for (int i = 0; i < memberPanels.Count; i++)
                        //            graphics.MoveTo(memberPanels[i], coordinates.Member(i));
                        //    });
                        //});


                        break;

                    case ShowState.FinalScreen:
                        // Финальная сцена. Весь ввод будет игнорирован
                        isBusy = false;
                        currentState = ShowState.FinalScreen;
                        break;
                }
            }
        }

        private void SortMembers()
        {
            List<MemberBar> membersWithPoints = null;

            // Сортируем участников порядке возрастания/убывания в соответствии с настройками программы
            if (ProgramSettings.MemberSortingMode == ProgramSettings.SortingMode.Ascending)
            {
                // Отсортируем тех, у кого уже есть баллы
                membersWithPoints = memberPanels.Select(x => x)
                                           .Where(x => x.Points != 0 && !x.ShowMask)
                                           .OrderByDescending(x => x.Points)
                                           .ThenBy(x => x.Name)
                                           .ToList();
            }
            else
            {
                // Отсортируем тех, у кого уже есть баллы
                membersWithPoints = memberPanels.Select(x => x)
                                                .Where(x => x.Points != 0 && !x.ShowMask)
                                                .OrderBy(x => x.Points)
                                                .ThenBy(x => x.Name)
                                                .ToList();
            }

            // Отсортируем тех, кто отсутствовал или получил нулевой балл
            List<MemberBar> absentMembers = null;
            absentMembers = memberPanels.Select(x => x)
                .Where(x => x.ShowMask == true)
                .OrderBy(x => x.MemberMaskType)
                .ToList();

            // Отсортируем тех, у кого нет баллов
            List<MemberBar> membersWithoutPoints = memberPanels.Select(x => x)
                                       .Where(x => x.Points == 0 && !x.ShowMask)
                                       .OrderBy(x => x.Name)
                                       .ToList();

            // Соединим два этих списка
            membersWithPoints.InsertRange(membersWithPoints.Count, membersWithoutPoints);

            // Если есть те, кто отсутствовал или получил нулевой балл, то пусть тоже отображаются)))
            if (absentMembers != null)
                membersWithPoints.InsertRange(membersWithPoints.Count, absentMembers);

            memberPanels = membersWithPoints;
        }

        // Обновляет положение участников в топе
        private void UpdatePlaces()
        {
            // Расстановка участников по топу
            if (ProgramSettings.TrueTopRating)
            {
                // Привильная рейтинговая система подразумевает следующие критерии:
                // Если есть участники с одинаковым результатом, то их мы помещаем на одно и тоже место, 
                // но предыдущие участники от этого в топе выше не перемещаются

                for (int i = 0; i < memberPanels.Count; i++)
                    if (i == 0)
                    {
                        memberPanels[i].Place = i + 1;
                    }
                    else
                    {
                        if (memberPanels[i].Points == memberPanels[i - 1].Points)
                            memberPanels[i].Place = memberPanels[i - 1].Place;
                        else
                            memberPanels[i].Place = i + 1;
                    }
            }
            else
                // Участники с одинаковыми баллами находятся на одном месте
                for (int i = 0; i < memberPanels.Count; i++)
                    if (i == 0)
                        memberPanels[i].Place = i + 1;
                    else
                    {
                        if (memberPanels[i].Points == memberPanels[i - 1].Points)
                            memberPanels[i].Place = memberPanels[i - 1].Place;
                        else
                            memberPanels[i].Place = memberPanels[i - 1].Place + 1;
                    }
        }

        /// <summary>
        /// Проверить, есть ли участники, которые получили 0 баллов за данный тур или отсутствовали. Если есть, то  они получат соответствующую пометку
        /// </summary>
        private void CheckZeroPoints()
        {
            // Временные переменные для уменьшения количества вызовов функций
            double currentPoint = 0;
            MemberBar memPanel = null;

            for (int mem = 0; mem < memberPanels.Count; mem++)
            {
                memPanel = memberPanels[mem];
                currentPoint = appLogic.GetPoint(currentJury, memPanel.ID);

                // Если участник не получил баллов, то пусть вместо баллов отображается X
                if (currentPoint == Constants.MEMBER_NO_POINTS)
                {
                    memPanel.MemberMaskType = MemberBar.MaskType.NoPoints;
                    memPanel.ShowMask = true;
                }
                else
                {
                    // Если участник отсутствовал в данном туре, то пусть вместо баллов отображается Н
                    if (currentPoint == Constants.MEMBER_ABSENT)
                    {
                        memPanel.MemberMaskType = MemberBar.MaskType.Absent;
                        memPanel.ShowMask = true;
                    }
                    else
                    {
                        memPanel.ShowMask = false;
                    }
                }
            }
        }
        
        /// <summary>
        /// Событие, которое вызывается при нажатии на кнопку "Закрыть" (красная кнопка с крестиком в правом верзнем углу окна Viewer)
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isBusy)
            {
                // Очищаем коллекции
                memberPanels.Clear();
                juryPanels.Clear();

                // Обнуляем ссылки
                memberPanels = null;
                juryPanels = null;
                
                // Закрываем окно
                Close();
            }
        }

        /// <summary>
        /// Возрвращает ссылку на Member-а по его ID
        /// </summary>
        /// <param name="id">ID участника</param>
        /// <returns></returns>
        private MemberBar GetCurrentMember()
        {
            return memberPanels.Find(x => x.ID == memberIterator);
        }

        /// <summary>
        /// Выбирает следующего участника, которому прямо сейчас будет присвоен балл. Если выбор жюри кончился, вернёт false
        /// </summary>
        private bool CheckPoint()
        {
            if (ProgramSettings.MemberPointsMode == ProgramSettings.PointsMode.Standard)
            {
                // Через цикл находим участника, у которого балл больше нуля
                for (int mem = memberIterator; mem < memberPanels.Count; mem++)
                    if (appLogic.GetPoint(currentJury, mem) > 0)
                    {
                        memberIterator = mem;
                        return true;
                    }

                // Таких больше нет. Меняйте жюри!
                return false;
            }
            else
            {
                // Баллы выдаются в порядке возрастания
                if (ProgramSettings.MemberPointsMode == ProgramSettings.PointsMode.Ascending)
                {
                    double minimalPoint = double.MaxValue;

                    // Проверим, есть ли еще баллы
                    for (memberIterator = 0; memberIterator < memberPanels.Count; memberIterator++)
                        if (memberPoints[currentJury][memberIterator] >= 0)
                            // Балл, который не вляется служебной константой (ABSENT, NO_POINTS) нашли, останавливаем цикл
                            break;

                    // Проверим, нашла ли проверка хоть какой-то балл
                    if (memberIterator == memberPanels.Count)
                        // Если итератор достиг предела, значит баллов больше нет и цикл можно останавливать
                        return false;

                    // Что-то есть, ищем самый маленький балл
                    for (int i = 0; i < memberPanels.Count; i++)
                        if (minimalPoint > memberPoints[currentJury][i] && memberPoints[currentJury][i] >= 0)
                        {
                            memberIterator = i;
                            minimalPoint = memberPoints[currentJury][i];
                        }

                    // Минимальный балл нашли, пометим балл использованным
                    memberPoints[currentJury][memberIterator] = Constants.VIEWER_POINT_USED;

                    return true;
                }

                // Баллы выдаются в порядке убывания

                if (ProgramSettings.MemberPointsMode == ProgramSettings.PointsMode.Descending)
                {
                    double maxPoints = double .MinValue;

                    // Проверим, есть ли еще баллы
                    for (memberIterator = 0; memberIterator < memberPanels.Count; memberIterator++)
                        if (memberPoints[currentJury][memberIterator] >= 0)
                            // Балл, который не вляется служебной константой (ABSENT, NO_POINTS) нашли, останавливаем цикл
                            break;

                    // Проверим, нашла ли проверка хоть какой-то балл
                    if (memberIterator == memberPanels.Count)
                        // Если итератор достиг предела, значит баллов больше нет и цикл можно останавливать
                        return false;

                    // Ищем максимальный балл
                    for (int i = 0; i < memberPanels.Count; i++)
                        if (maxPoints < memberPoints[currentJury][i] && memberPoints[currentJury][i] >= 0)
                        {
                            memberIterator = i;
                            maxPoints = memberPoints[currentJury][memberIterator];
                        }

                    // Максимальный балл нашли, пометим балл использованным
                    memberPoints[currentJury][memberIterator] = Constants.VIEWER_POINT_USED;

                    return true;
                }

                // Такого не должно быть
                throw new Exception();
            }
        }



        /// <summary>
        /// Событие, которое взывается при прогрузке формы
        /// Здесь идёт инициализация классов, которые зависят от параметров развернутого окна
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pointBar = new PointBar(mainCanvas);
            coordinates = new CoordinatesProvider((int)Width, (int)Height, pointBar, ProgramSettings.MaxMembersInColumn);

            if (appLogic.GetMemberNames().Length > ProgramSettings.MaxMembersInColumn)
            {
                if (ProgramSettings.TwoColumns)
                {
                    coordinates.TwoColumns = true;
                }
                else
                {
                    coordinates.TwoColumns = false;
                }
            }
            else
                coordinates.TwoColumns = false;
            
            // Если анимированный фон выключен, ставим стандартным статичный фон
            if (!ProgramSettings.AnimatedBackground)
                Background = ProgramSettings.MainBackground;
        }

        /// <summary>
        /// Необходимо для отслеживания нажатия клавиш PageDown и Home
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.PageUp) || (e.Key == Key.Next))
            {
                Window_MouseDown(null, null);
            }
        }
    }
}