using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ResultViewerWPF.Viewer;

namespace ResultViewerWPF.Settings
{
    /// <summary>
    /// Логика взаимодействия для ViewerSettings.xaml
    /// </summary>
    public partial class ViewerSettings : Window
    {
        #region Графика

        /// <summary>
        /// Коллекция с панелями участников, которые сейчас прогружены в окне
        /// </summary>
        List<MemberBar> memberPanels = new List<MemberBar>();

        /// <summary>
        /// Коллекция с панелями жюри, которые сейчас прогружены в окне
        /// </summary>
        List<JuryBar> juryPanels = new List<JuryBar>();

        /// <summary>
        /// Координатная система
        /// </summary>
        CoordinatesProvider coordinates;

        /// <summary>
        /// Панель с баллами жюри
        /// </summary>
        PointBar pointPanel = null;
        
        /// <summary>
        /// Текст в нижней части экрана
        /// </summary>
        TextBar lowerPhrase = null;

        /// <summary>
        /// Флаг, блокирующий ввод пользователя при какой-нибудь анимации
        /// </summary>
        bool isBusy = false;

        #endregion

        #region Костыли

        /// <summary>
        /// Для работы int.TryParse
        /// </summary>
        int tempIntVar = 0;

        /// <summary>
        /// Для работы double.TryParse
        /// </summary>
        double tempDoubleVar = 0;

        #endregion

        /// <summary>
        /// Показывает, открыто ли сейчас меню настроек
        /// По Умолчанию оно закрыто
        /// </summary>
        private bool menuOpened = false;

        enum LogicType
        {
            User,
            Test
        }

        LogicType CurrentLogicType = LogicType.Test;

        /// <summary>
        /// Ссылка на логику с пользовательскими данными
        /// </summary>
        private Logic userLogic = null;

        public ViewerSettings(Logic innerLogic)
        {
            InitializeComponent();

            // Проверим, есть ли что-нибудь в пользовательской логике
            if (innerLogic != null && !innerLogic.IsEmpty())
            {
                userLogic = innerLogic;
                CurrentLogicType = LogicType.User;
            }
            else
                CurrentLogicType = LogicType.Test;
        }

        // Когда окно прогрузит, можно включать все анимации
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pointPanel = new PointBar(MainCanvas);
            LoadCoordinates();

            #region Load settings
            MemberPanelWidthTB.Text = ProgramSettings.MemberPanelWidth.ToString();
            MemberPanelHeightTB.Text = ProgramSettings.MemberPanelHeight.ToString();
            MemberNameFontSizeTB.Text = ProgramSettings.MemberNameFontSize.ToString();

            ShowMemberPanelColor.Background = new SolidColorBrush(ProgramSettings.MemberPanelColor);
            ShowMemberPanelChosenColor.Background = new SolidColorBrush(ProgramSettings.MemberPanelChosenColor);
            ChangeMemberPanelUseSecondShooseColor.IsChecked = ProgramSettings.MemberPanelUseSecondChooseColor;
            ShowMemberPanelChosenColor2.Background = new SolidColorBrush(ProgramSettings.MemberPanelChosenColor2);

            ChangeMemberPanelHighlightLeaders.IsChecked = ProgramSettings.MemberPanelHighlightLeaders;

            ShowMemberPanelFirstPlace.Background = new SolidColorBrush(ProgramSettings.MemberPanelFirstPlace);
            ShowMemberPanelSecondPlace.Background = new SolidColorBrush(ProgramSettings.MemberPanelSecondPlace);
            ShowMemberPanelThirdPlace.Background = new SolidColorBrush(ProgramSettings.MemberPanelThirdPlace);
            ShowMemberPanelOtherPlaces.Background = new SolidColorBrush(ProgramSettings.MemberPanelOtherPlaces);

            ChooseMemberNameFontWeight.SelectedIndex = ConvertFontWeightToIndex(ProgramSettings.MemberNameFontWeight);
            ShowMemberNameFontColor.Background = ProgramSettings.MemberNameFontColor;

            MemberPointsFontSizeTB.Text = ProgramSettings.MemberPointsFontSize.ToString();
            MemberPointsPanelHeightTB.Text = ProgramSettings.MemberPointsPanelHeight.ToString();
            MemberPointsPanelWidthTB.Text = ProgramSettings.MemberPointsPanelWidth.ToString();

            MemberPointsFontSizeTB.Text = ProgramSettings.MemberPointsFontSize.ToString();
            MemberPointsPanelHeightTB.Text = ProgramSettings.MemberPointsPanelHeight.ToString();
            MemberPointsPanelWidthTB.Text = ProgramSettings.MemberPointsPanelWidth.ToString();
            MemberPointsStrokeWidthTB.Text = ProgramSettings.MemberPointsStrokeWidth.ToString();
            ShowMemberPointsPanelColor.Background = new SolidColorBrush(ProgramSettings.MemberPointsPanelColor);
            ShowMemberPointsStrokeColor.Background = new SolidColorBrush(ProgramSettings.MemberPointsStrokeColor);
            ShowMemberPointsFontColor.Background = ProgramSettings.MemberPointsFontColor;

            MemberPlaceFontSizeTB.Text = ProgramSettings.MemberPlaceFontSize.ToString();
            MemberPlacePanelOffsetTB.Text = ProgramSettings.MemberPlacePanelOffset.ToString();
            ShowMemberPlaceFontColor.Background = new SolidColorBrush(ProgramSettings.MemberPlaceFontColor);
            ChooseMemberPlaceFontWeight.SelectedIndex = ConvertFontWeightToIndex(ProgramSettings.MemberPlaceFontWeight);
            ChooseMemberPlaceShowMode.SelectedIndex = (int)ProgramSettings.MemberPlaceShowMode;
            MemberPlaceStrokeWidthTB.Text = ProgramSettings.MemberPlaceStrokeWidth.ToString();
            ShowMemberPlacePanelColor.Background = new SolidColorBrush(ProgramSettings.MemberPlacePanelColor);
            ShowMemberPlaceStrokeColor.Background = new SolidColorBrush(ProgramSettings.MemberPlaceStrokeColor);

            ChangeShowMemberResultMode.SelectedIndex = (int)ProgramSettings.ShowMemberResultMode;
            MemberResultPanelWidthTB.Text = ProgramSettings.MemberResultPanelWidth.ToString();
            MemberResultPanelHeightTB.Text = ProgramSettings.MemberResultPanelHeight.ToString();
            MemberResultFontSizeTB.Text = ProgramSettings.MemberResultFontSize.ToString();
            MemberResultPanelOffsetTB.Text = ProgramSettings.MemberResultPanelOffset.ToString();
            MemberResultStrokeWidthTB.Text = ProgramSettings.MemberResultStrokeWidth.ToString();
            ChooseMemberResultFontWeight.SelectedIndex = ConvertFontWeightToIndex(ProgramSettings.MemberResultFontWeight);
            ShowMemberResultFontColor.Background = new SolidColorBrush(ProgramSettings.MemberResultFontColor);
            ShowMemberResultPanelColor.Background = new SolidColorBrush(ProgramSettings.MemberResultPanelColor);
            ShowMemberResultStrokeColor.Background = new SolidColorBrush(ProgramSettings.MemberResultStrokeColor);

            JuryPanelWidthTB.Text = ProgramSettings.JuryPanelWidth.ToString();
            JuryPanelHeightTB.Text = ProgramSettings.JuryPanelHeight.ToString();
            JuryFontSizeTB.Text = ProgramSettings.JuryFontSize.ToString();
            ShowJuryPanelColor.Background = new SolidColorBrush(ProgramSettings.JuryPanelColor);

            PointBarFontSizeTB.Text = ProgramSettings.PointBarFontSize.ToString();

            LowerPhraseOffsetTB.Text = ProgramSettings.LowerPhraseOffset.ToString();
            LowerPhraseFontHeightTB.Text = ProgramSettings.LowerPhraseFontSize.ToString();
            ChooseLowerPhraseFontWeight.SelectedIndex = ConvertFontWeightToIndex(ProgramSettings.LowerPhraseFontWeight);
            ShowLowerPanelFontColor.Background = new SolidColorBrush(ProgramSettings.LowerPhraseFontColor);
            LowerPhraseTB.Document.Blocks.Clear();
            ChooseLowerPhraseShowMode.SelectedIndex = (int)ProgramSettings.LowerPhraseShowMode;
            foreach (string str in ProgramSettings.LowerPhrase.Split('\n'))
            {
                LowerPhraseTB.AppendText(str);
                LowerPhraseTB.AppendText(Environment.NewLine);
            }

            TopJuryIntervalTB.Text = ProgramSettings.TopJuryInterval.ToString();
            JuryMemberOffsetTB.Text = ProgramSettings.JuryMemberOffset.ToString();
            MemberIntervalTB.Text = ProgramSettings.MemberInterval.ToString();
            MemberColumnIntervalTB.Text = ProgramSettings.MemberColumnInterval.ToString();
            ChooseMemberPointsMode.SelectedIndex = (int)ProgramSettings.MemberPointsMode;
            ChooseMemberSortingMode.SelectedIndex = (int)ProgramSettings.MemberSortingMode;
            ChangeTrueTopRating.IsChecked = ProgramSettings.TrueTopRating;
            StartJuryTB.Text = ProgramSettings.StartJury.ToString();
            ChangeTwoColumns.IsChecked = ProgramSettings.TwoColumns;
            MaxMembersInColumnTB.Text = ProgramSettings.MaxMembersInColumn.ToString();
            FinalPhraseTB.Text = ProgramSettings.FinalPhrase;

            ChangeAnimatedBackground.IsChecked = ProgramSettings.AnimatedBackground;
            ChangeVideoBackground.IsChecked = ProgramSettings.VideoBackground;
            VideoPathTB.Text = ProgramSettings.VideoPath;
            AnimMoveTimeTB.Text = ProgramSettings.AnimMoveTime.TotalMilliseconds.ToString();
            AnimAppearTimeTB.Text = ProgramSettings.AnimAppearTime.TotalMilliseconds.ToString();
            AnimPauseTB.Text = ProgramSettings.AnimPause.TotalMilliseconds.ToString();
            AnimPointBarPauseTB.Text = ProgramSettings.AnimPointBarPause.TotalMilliseconds.ToString();
            ShowBackgroundColor1.Background = new SolidColorBrush(ProgramSettings.BackgroundColor1);
            ShowBackgroundColor2.Background = new SolidColorBrush(ProgramSettings.BackgroundColor2);
            BackgroundAnimPeriodTB.Text = ProgramSettings.BackgroundAnimPeriod.TotalMilliseconds.ToString();
            BackgroundAppearTimeTB.Text = ProgramSettings.BackgroundAppearTime.TotalMilliseconds.ToString();

            LowerPhraseTB.Document.LineHeight = 0.1;
            #endregion

            StartViewer();
        }

        private void LoadCoordinates()
        {
            coordinates = new CoordinatesProvider((int)Width, (int)Height, pointPanel, ProgramSettings.MaxMembersInColumn);
        }

        private void SetDefaultSettings(object sender, RoutedEventArgs e)
        {
            ProgramSettings.MemberPanelWidth = 405;
            ProgramSettings.MemberPanelHeight = 45;
            ProgramSettings.MemberNameFontSize = 20;
            ProgramSettings.MemberPanelOpacity = 1;
            ProgramSettings.MemberPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.MemberPanelChosenColor = new Color()
            {
                A = 255,
                R = 255,
                G = 100,
                B = 100
            };

            ProgramSettings.MemberPanelChosenColor2 = new Color()
            {
                A = 255,
                R = 100,
                G = 255,
                B = 100
            };


            ProgramSettings.MemberPanelHighlightLeaders = true;
            ProgramSettings.MemberPanelFirstPlace = new Color()
            {
                A = 200,
                R = 255,
                G = 218,
                B = 6
            };

            ProgramSettings.MemberPanelSecondPlace = new Color()
            {
                A = 200,
                R = 198,
                G = 198,
                B = 198
            };

            ProgramSettings.MemberPanelThirdPlace = new Color()
            {
                A = 200,
                R = 170,
                G = 98,
                B = 0
            };

            ProgramSettings.MemberPanelOtherPlaces = new Color()
            {
                A = 100,
                R = 50,
                G = 50,
                B = 50
            };

            ProgramSettings.MemberPanelUseSecondChooseColor = true;
            ProgramSettings.MemberNameFontWeight = FontWeights.Thin;
            ProgramSettings.MemberNameFontColor = Brushes.Black;
            ProgramSettings.MemberPointsFontWeight = FontWeights.Thin;
            ProgramSettings.MemberPointsFontSize = 22;
            ProgramSettings.MemberPointsPanelHeight = 45;
            ProgramSettings.MemberPointsPanelWidth = 45;

            ProgramSettings.MemberPointsPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.MemberPointsStrokeColor = new Color()
            {
                A = 200,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.MemberPointsFontColor = Brushes.Black;
            ProgramSettings.MemberPointsStrokeWidth = 2;
            ProgramSettings.MemberPlaceFontColor = Colors.Black;
            ProgramSettings.MemberPlaceFontSize = 22;
            ProgramSettings.MemberPlaceFontWeight = FontWeights.Black;
            ProgramSettings.MemberPlacePanelOffset = 0;
            ProgramSettings.MemberPlaceShowMode = ProgramSettings.PlaceShowMode.AlwaysVisible;
            ProgramSettings.MemberPlaceStrokeWidth = 2;

            ProgramSettings.MemberPlacePanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.MemberPlaceStrokeColor = new Color()
            {
                A = 200,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.ShowMemberResultMode = ProgramSettings.ResultShowMode.Visible;
            ProgramSettings.MemberResultPanelWidth = 50;
            ProgramSettings.MemberResultPanelHeight = 45;
            ProgramSettings.MemberResultFontSize = 22;
            ProgramSettings.MemberResultPanelOffset = 0;
            ProgramSettings.MemberResultStrokeWidth = 2;
            ProgramSettings.MemberResultFontWeight = FontWeights.Thin;
            ProgramSettings.MemberResultFontColor = Colors.Black;

            ProgramSettings.MemberResultPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.MemberResultStrokeColor = new Color()
            {
                A = 200,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.JuryPanelWidth = 405;
            ProgramSettings.JuryPanelHeight = 35;
            ProgramSettings.JuryPanelOpacity = 1;
            ProgramSettings.JuryFontSize = 20;

            ProgramSettings.JuryPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            ProgramSettings.PointBarFontSize = 120;
            ProgramSettings.PointBarPanelOpacity = 1;
            ProgramSettings.TopJuryInterval = 100;
            ProgramSettings.JuryMemberOffset = 100;
            ProgramSettings.MemberInterval = 5;
            ProgramSettings.MemberColumnInterval = 100;
            ProgramSettings.MemberPointsMode = ProgramSettings.PointsMode.Descending;
            ProgramSettings.MemberSortingMode = ProgramSettings.SortingMode.Descending;
            ProgramSettings.TrueTopRating = true;
            ProgramSettings.StartJury = 0;
            ProgramSettings.TwoColumns = true;
            ProgramSettings.MaxMembersInColumn = 10;
            ProgramSettings.FinalPhrase = "Поздравляем победителей!";


            ProgramSettings.LowerPhraseOffset = 100;
            ProgramSettings.LowerPhraseFontSize = 10;
            ProgramSettings.LowerPhraseFontWeight = FontWeights.Normal;
            ProgramSettings.LowerPhraseFontColor = Colors.Black;
            ProgramSettings.LowerPhraseShowMode = ProgramSettings.ShowMode.AlwaysVisible;
            ProgramSettings.LowerPhrase = "Н - не явившиеся на конкурс \nX - не приславшие конкурсную работу";

            ProgramSettings.MainBackground = new ImageBrush(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.mainBackground2.GetHbitmap(),
                                                                              IntPtr.Zero,
                                                                              Int32Rect.Empty,
                                                                              BitmapSizeOptions.FromEmptyOptions()));
            ProgramSettings.ShowPointAnim = true;
            ProgramSettings.AnimatedBackground = false;
            ProgramSettings.VideoBackground = false;
            ProgramSettings.VideoPath = "background.mp4";
            ProgramSettings.AnimMoveTime = TimeSpan.FromMilliseconds(500);
            ProgramSettings.AnimAppearTime = TimeSpan.FromMilliseconds(500);
            ProgramSettings.AnimPause = TimeSpan.FromMilliseconds(1100);
            ProgramSettings.AnimPointBarPause = TimeSpan.FromMilliseconds(500);
            ProgramSettings.BackgroundColor1 = Colors.DarkBlue;
            ProgramSettings.BackgroundColor2 = Colors.DeepSkyBlue;
            ProgramSettings.BackgroundAnimPeriod = TimeSpan.FromSeconds(3);
            ProgramSettings.BackgroundAppearTime = TimeSpan.FromSeconds(2);

            MessageBox.Show("Установлены настройки по-умолчанию", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void StartViewer()
        {
            Logic loadingLogic = null;

            switch (CurrentLogicType)
            {
                case LogicType.User:
                    loadingLogic = userLogic;
                    break;

                case LogicType.Test:
                    loadingLogic = Logic.GetTestLogic();
                    break;
            }

            LoadCoordinates();

            if (loadingLogic.GetMemberNames().Length > ProgramSettings.MaxMembersInColumn)
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

            // Присваиваем фону чёрный цвет
            SolidColorBrush backgroundColor = new SolidColorBrush(Colors.Black);
            MainCanvas.Background = backgroundColor;

            this.Background = ProgramSettings.MainBackground;

            // Запускаем анимацию проявления
            GraphicsEngine.BcgAppear(MainCanvas.Background);

            // Если анимированный фон включён
            if (ProgramSettings.AnimatedBackground)
            {
                // Если в качестве анимации выступает видео
                if (ProgramSettings.VideoBackground)
                {
                    // Проверим наличие файла
                    if (File.Exists(ProgramSettings.VideoPath))
                    {
                        // На случай, если файл битый
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
                            MessageBox.Show("Ошибка при загрузке видео", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    // Присваеваем фону новый цвет
                    Background = new SolidColorBrush(ProgramSettings.BackgroundColor1);

                    // Запускаем анимацию
                    GraphicsEngine.StartInfiniteBcgAnim(Background);
                }
            };

            // Прогрузим промежуточный массив с баллами участника
            double[][] memberPoints = loadingLogic.GetPoints();

            // Загрузим жюри
            string[] juryNames = loadingLogic.GetJuryNames();
            foreach (string jur in juryNames)
                juryPanels.Add(new JuryBar(MainCanvas, jur));

            // Загрузим участников
            string[] memberNames = loadingLogic.GetMemberNames();
            for (int i = 0; i < memberNames.Length; i++)
                memberPanels.Add(new MemberBar(MainCanvas, memberNames[i], i));

            pointPanel = new PointBar(MainCanvas);

            // Грузим дополнительные данные участников, если показ таковых включен
            if (ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.Visible ||
                ProgramSettings.ShowMemberResultMode == ProgramSettings.ResultShowMode.AlwaysVisible)
            {
                // Загрузим массив с дополнитльеными результатами
                double[][] memberValues = loadingLogic.GetValues();

                // Проставляем участникам их дополнительные результаты
                for (int i = 0; i < memberPanels.Count; i++)
                    memberPanels[i].Value = memberValues[0][i];
            }

            // Проверим, с какого жюри нам надо начать показ
            if (ProgramSettings.StartJury != 0)
            {
                // Просчитаем все баллы, которые необходимо добавить
                int[] calculatedPoints = new int[memberNames.Length];

                // Проставим просчитанные баллы участникам
                for (int jur = 0; jur < ProgramSettings.StartJury; jur++)
                    for (int mem = 0; mem < memberNames.Length; mem++)
                        memberPanels[mem].Points += userLogic.GetPoint(jur, mem);

                // Отсортируем список
                SortMembers();
            }

            // Покажем жюри
            GraphicsEngine.Appear(juryPanels[0], (obj, ev) => {
                isBusy = false;
            });

            // Переместим его с центр экрана
            GraphicsEngine.MoveTo(juryPanels[0], coordinates.Jury());

            // Отсортируем участников по алфавиту
            SortMembers();

            // Расставим участников конкурса по экрану в соответствии с координатной системой
            for (int i = 0; i < memberPanels.Count; i++)
            {
                memberPanels[i].Place = 1;
                GraphicsEngine.Appear(memberPanels[i]);
                GraphicsEngine.MoveTo(memberPanels[i], coordinates.Member(i));
            }

            lowerPhrase = new TextBar(MainCanvas, ProgramSettings.LowerPhrase);

            if (ProgramSettings.MemberPanelUseSecondChooseColor)
                GraphicsEngine.ChangeMemberColor(memberPanels[0], ProgramSettings.MemberPanelChosenColor2);
            GraphicsEngine.ChangeMemberColor(memberPanels[1], ProgramSettings.MemberPanelChosenColor, (e3, ev3) =>
            {
                GraphicsEngine.Wait(ProgramSettings.AnimPause, (e, ev) =>
                {
                    pointPanel.NumOfPoints = 10;
                    memberPanels[1].IsChosen = true;
                    memberPanels[1].IsColored = true;
                    GraphicsEngine.MoveToInstant(pointPanel, coordinates.PointBar(-1));
                    GraphicsEngine.Appear(pointPanel);
                    GraphicsEngine.Resize(pointPanel, 1.5, (e1, ev1) =>
                    {
                        GraphicsEngine.Wait(ProgramSettings.AnimPointBarPause, (e2, ev2) =>
                        {
                            GraphicsEngine.MoveTo(pointPanel, coordinates.PointBar(1));
                            GraphicsEngine.Resize(pointPanel, 0.1);
                            memberPanels[1].Points += pointPanel.NumOfPoints;
                            GraphicsEngine.Disappear(pointPanel, new EventHandler((e4, ev4) =>
                            {

                                GraphicsEngine.MoveToInstant(lowerPhrase, coordinates.LowerFrase(lowerPhrase.GetPanelWidth(), lowerPhrase.GetPanelHeight()));
                                GraphicsEngine.Appear(lowerPhrase);
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
                            }));
                        });
                    });
                });
            });
        }

        void FilterNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            TextBox currentTB = sender as TextBox;

            // Если точка, то...
            if (e.Text == "," || e.Text == ".")
            {
                // Смотрим, не вводят ли её в пустое поле или нет ли еще таких же уже введённых точек, то всё норм
                if ((!currentTB.Text.Contains(",") && currentTB.Text.Length != 0) || (!currentTB.Text.Contains(".") && currentTB.Text.Length != 0))
                    return;
            }
            else
            {
                // Если это цифра, то всё норм
                if (char.IsDigit(e.Text, 0))
                    return;
            }

            e.Handled = true;
        }        

        /// <summary>
        /// Показать или спрятать меню с настройками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleSettingsMenu(object sender, RoutedEventArgs e)
        {
            // Анимация, которая будет двигать меню с настройками и кнопку
            DoubleAnimation settingsPanelAnimation;
            DoubleAnimation toggleButtonAnimation;
            DoubleAnimation applyButtonAnimation;

            // Необходимо для привязки анимации к свойству элемента Canvas.Left
            Storyboard animationStoryboard;

            // Проверяем, надо ли открыть или закрыть меню
            if (menuOpened)
            {
                // Иницилазируем анимации
                settingsPanelAnimation = new DoubleAnimation()
                {
                    From = 0,
                    To = -230,
                    Duration = TimeSpan.FromSeconds(0.3),
                    AccelerationRatio = 0.9
                };

                toggleButtonAnimation = new DoubleAnimation()
                {
                    From = 235,
                    To = 5,
                    Duration = TimeSpan.FromSeconds(0.3),
                    AccelerationRatio = 0.9
                };

                applyButtonAnimation = toggleButtonAnimation.Clone();

                // Привяжем анимации к соответствующим элементам
                Storyboard.SetTargetProperty(settingsPanelAnimation, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTargetProperty(toggleButtonAnimation, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTargetProperty(applyButtonAnimation, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTarget(settingsPanelAnimation, this.SettingsPanel);
                Storyboard.SetTarget(toggleButtonAnimation, this.ToggleSettingsButton);
                Storyboard.SetTarget(applyButtonAnimation, this.ApplySettingsButton);

                // Запустим анимации
                animationStoryboard = new Storyboard();
                animationStoryboard.Children.Add(settingsPanelAnimation);
                animationStoryboard.Children.Add(toggleButtonAnimation);
                animationStoryboard.Children.Add(applyButtonAnimation);
                animationStoryboard.Begin(MainCanvas);

                // Обновим состояние переменных флагов, изменим текст кнопки
                menuOpened = false;
                ToggleSettingsButton.Content = "Закрыть";
            }
            else
            {
                // Иницилазируем анимации
                settingsPanelAnimation = new DoubleAnimation()
                {
                    From = -230,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.3),
                    AccelerationRatio = 0.9
                };

                toggleButtonAnimation = new DoubleAnimation()
                {
                    From = 5,
                    To = 235,
                    Duration = TimeSpan.FromSeconds(0.3),
                    AccelerationRatio = 0.9
                };

                applyButtonAnimation = toggleButtonAnimation.Clone();

                // Привяжем анимации к соответствующим элементам
                Storyboard.SetTargetProperty(settingsPanelAnimation, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTargetProperty(toggleButtonAnimation, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTargetProperty(applyButtonAnimation, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTarget(settingsPanelAnimation, this.SettingsPanel);
                Storyboard.SetTarget(toggleButtonAnimation, this.ToggleSettingsButton);
                Storyboard.SetTarget(applyButtonAnimation, this.ApplySettingsButton);

                // Запустим анимации
                animationStoryboard = new Storyboard();
                animationStoryboard.Children.Add(settingsPanelAnimation);
                animationStoryboard.Children.Add(toggleButtonAnimation);
                animationStoryboard.Children.Add(applyButtonAnimation);
                animationStoryboard.Begin(MainCanvas);

                // Обновим состояние переменных флагов, изменим текст кнопки
                menuOpened = true;
                ToggleSettingsButton.Content = "Открыть";
            }
        }

        /// <summary>
        /// Отсортировать участников
        /// </summary>
        private void SortMembers()
        {
            List<MemberBar> membersWithPoints = null;

            // Сортируем участников порядке возрастания/убывания в соответствии с настройками программы
            if (ProgramSettings.MemberSortingMode == ProgramSettings.SortingMode.Ascending)
            {
                // Отсортируем тех, у кого уже есть баллы
                membersWithPoints = memberPanels.Select(x => x)
                                           .Where(x => x.Points != 0)
                                           .OrderByDescending(x => x.Points)
                                           .ThenBy(x => x.Name)
                                           .ToList();
            }
            else
            {
                // Отсортируем тех, у кого уже есть баллы
                membersWithPoints = memberPanels.Select(x => x)
                                                .Where(x => x.Points != 0)
                                                .OrderBy(x => x.Points)
                                                .ThenBy(x => x.Name)
                                                .ToList();
            }



            // Отсортируем тех, у кого нет баллов
            List<MemberBar> membersWithoutPoints = memberPanels.Select(x => x)
                                       .Where(x => x.Points == 0)
                                       .OrderBy(x => x.Name)
                                       .ToList();

            // Соединим два этих списка
            membersWithPoints.InsertRange(membersWithPoints.Count, membersWithoutPoints);
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
                // Без всяких махинаций по порядку присваиваем баллы
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

        private void ApplySettingsButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (MemberBar bar in memberPanels)
                MainCanvas.Children.Remove(bar.mainPanel);
            memberPanels.Clear();

            foreach (JuryBar bar in juryPanels)
                MainCanvas.Children.Remove(bar.mainPanel);
            juryPanels.Clear();

            MainCanvas.Children.Remove(pointPanel.mainPanel);
            MainCanvas.Children.Remove(lowerPhrase.mainPanel);

            MainCanvas.Background = Brushes.Black;
            StartViewer();
        }

        private void ShowMainScreenColors(object sender, RoutedEventArgs e)
        {
            if (ProgramSettings.MemberPanelUseSecondChooseColor)
                GraphicsEngine.ChangeMemberColor(memberPanels[0], ProgramSettings.MemberPanelChosenColor);
            GraphicsEngine.ChangeMemberColor(memberPanels[1], ProgramSettings.MemberPanelChosenColor2);
            for (int i = 2; i < memberPanels.Count; i++)
                GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelColor);
        }

        private void ShowFinalscreenColors(object sender, RoutedEventArgs e)
        {
            GraphicsEngine.ChangeMemberColor(memberPanels[0], ProgramSettings.MemberPanelFirstPlace);
            GraphicsEngine.ChangeMemberColor(memberPanels[1], ProgramSettings.MemberPanelSecondPlace);
            GraphicsEngine.ChangeMemberColor(memberPanels[2], ProgramSettings.MemberPanelThirdPlace);
            for (int i = 3; i < memberPanels.Count; i++)
                GraphicsEngine.ChangeMemberColor(memberPanels[i], ProgramSettings.MemberPanelOtherPlaces);
        }

        /// <summary>
        /// Вызывается при закрытии окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void CloseWindow(object sender, EventArgs ev)
        {
            Close();
        }

        static public int ConvertFontWeightToIndex(FontWeight weight)
        {
            if (weight == FontWeights.Black)
                return 0;

            if (weight == FontWeights.Bold)
                return 1;

            if (weight == FontWeights.DemiBold)
                return 2;

            if (weight == FontWeights.ExtraBlack)
                return 3;

            if (weight == FontWeights.ExtraBold)
                return 4;

            if (weight == FontWeights.ExtraLight)
                return 5;

            if (weight == FontWeights.Heavy)
                return 6;

            if (weight == FontWeights.Light)
                return 7;

            if (weight == FontWeights.Medium)
                return 8;

            if (weight == FontWeights.Normal)
                return 9;

            if (weight == FontWeights.Regular)
                return 10;

            if (weight == FontWeights.SemiBold)
                return 11;

            if (weight == FontWeights.Thin)
                return 12;

            if (weight == FontWeights.UltraBlack)
                return 13;

            if (weight == FontWeights.UltraBold)
                return 14;

            if (weight == FontWeights.UltraLight)
                return 15;

            throw new Exception("WTF");
        }

        static public FontWeight ConvertIndexToFontWeight(int index)
        {
            switch(index)
            {
                case 0:
                    return FontWeights.Black;
                case 1:
                    return FontWeights.Bold;
                case 2:
                    return FontWeights.DemiBold;
                case 3:
                    return FontWeights.ExtraBlack;
                case 4:
                    return FontWeights.ExtraBold;
                case 5:
                    return FontWeights.ExtraLight;
                case 6:
                    return FontWeights.Heavy;
                case 7:
                    return FontWeights.Light;
                case 8:
                    return FontWeights.Medium;
                case 9:
                    return FontWeights.Normal;
                case 10:
                    return FontWeights.Regular;
                case 11:
                    return FontWeights.SemiBold;
                case 12:
                    return FontWeights.Thin;
                case 13:
                    return FontWeights.UltraBlack;
                case 14:
                    return FontWeights.UltraBold;
                case 15:
                    return FontWeights.UltraLight;
            }

            throw new Exception("WTF");
        }

        /// <summary>
        /// Обновляет значение параметра из настроек, получая новое значение из TexboBox и загружая его в необходимый параметр в настройках
        /// </summary>
        /// <param name="textBox">Ссылка на TextBox, откуда нужно получить значение</param>
        /// <param name="value">Ссылка на параметр в настройках, который нужно обновить</param>
        private void UpdateValueFromTB(TextBox textBox, ref int value)
        {
            if (int.TryParse(textBox.Text, out tempIntVar))
            {
                value = tempIntVar;
            }
            else
            {
                textBox.Text = tempIntVar.ToString();
                MessageBox.Show("Введены некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Обновляет значение параметра из настроек, получая новое значение из TexboBox и загружая его в необходимый параметр в настройках
        /// </summary>
        /// <param name="textBox">Ссылка на TextBox, откуда нужно получить значение</param>
        /// <param name="value">Ссылка на параметр в настройках, который нужно обновить</param>
        private void UpdateValueFromTB(TextBox textBox, ref double value)
        {
            if (double.TryParse(textBox.Text, out tempDoubleVar))
            {
                value = tempDoubleVar;
            }
            else
            {
                textBox.Text = tempDoubleVar.ToString();
                MessageBox.Show("Введены некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Обновляет значение параметра из настроек, получая новое значение из TexboBox и загружая его в необходимый параметр в настройках
        /// </summary>
        /// <param name="textBox">Ссылка на TextBox, откуда нужно получить значение</param>
        /// <param name="value">Ссылка на параметр в настройках, который нужно обновить</param>
        private void UpdateValueFromTB(TextBox textBox, ref TimeSpan value)
        {
            if (double.TryParse(textBox.Text, out tempDoubleVar))
            {
                value = TimeSpan.FromMilliseconds(tempDoubleVar);
            }
            else
            {
                textBox.Text = tempDoubleVar.ToString();
                MessageBox.Show("Введены некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        /// <summary>
        /// Обновляет значение параметра из настроек, получая новое значение из CheckBox и загружая его в необходимый параметр в настройках
        /// </summary>
        /// <param name="checkBox">Ссылка на CheckBox, откуда нужно получить значение</param>
        /// <param name="value">Ссылка на параметр в настройках, который нужно обновить</param>
        private void UpdateValueFromCheckBox(CheckBox checkBox, ref bool value)
        {
            value = checkBox.IsChecked ?? value;
        }

        /// <summary>
        /// /// Обновляет значение параметра из настроек, получая новое значение через диалог с пользователем и загружая его в необходимый параметр в настройках
        /// </summary>
        /// <param name="changingColor">Параметр в настройках, который нужно обновить</param>
        /// <param name="previewColor">Панель, в которой нужно отобразить только что полученный цвет</param>
        private void UpdateColor(ref Color changingColor, Canvas previewColor)
        {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            cd.ShowDialog();
            System.Drawing.Color dialogRes = cd.Color;

            changingColor = Color.FromArgb(dialogRes.A, dialogRes.R, dialogRes.G, dialogRes.B);

            UpdatePanelColor(previewColor, dialogRes);
        }

        /// <summary>
        /// /// Обновляет значение параметра из настроек, получая новое значение через диалог с пользователем и загружая его в необходимый параметр в настройках
        /// </summary>
        /// <param name="changingColor">Параметр в настройках, который нужно обновить</param>
        /// <param name="previewColor">Панель, в которой нужно отобразить только что полученный цвет</param>
        private void UpdateColor(ref SolidColorBrush changingBrush, Canvas previedColor)
        {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            cd.ShowDialog();
            System.Drawing.Color dialogRes = cd.Color;

            changingBrush = new SolidColorBrush(Color.FromArgb(dialogRes.A, dialogRes.R, dialogRes.G, dialogRes.B));

            UpdatePanelColor(previedColor, dialogRes);
        }

        /// <summary>
        /// Задаёт отображение тестовых данных по запросу пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTestLogic(object sender, RoutedEventArgs e)
        {
            CurrentLogicType = LogicType.Test;
            MessageBox.Show("При следующем обновлении будут загружены тестовые данные", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// Задаёт отображение пользовательских данных от пользователя, если это возможно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetUserLogic(object sender, RoutedEventArgs e)
        {
            if (userLogic != null)
            {
                CurrentLogicType = LogicType.User;
                MessageBox.Show("При следующем обновлении будут загружены пользовательские данные", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
                MessageBox.Show("Невозможно отобразить пользователские данные из-за отсутствия таковых", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Обновляет цвет панели, заданной в параметрах
        /// </summary>
        /// <param name="panel"></param>
        private void UpdatePanelColor(Canvas panel, System.Drawing.Color color)
        {
            panel.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        private void ChangeMemberPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPanelColor, ShowMemberPanelColor);
        private void ChangeMemberPanelChosenColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPanelChosenColor, ShowMemberPanelChosenColor);
        private void ChangeMemberPanelChosenColor2_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPanelChosenColor2, ShowMemberPanelChosenColor2);
        private void ChangeMemberNameFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberNameFontColor, ShowMemberNameFontColor);
        private void ChangeMemberPanelFirstPlace_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPanelFirstPlace, ShowMemberPanelFirstPlace);
        private void ChangeMemberPanelSecondPlace_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPanelSecondPlace, ShowMemberPanelSecondPlace);
        private void ChangeMemberPanelThirdPlace_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPanelThirdPlace, ShowMemberPanelThirdPlace);
        private void ChangeMemberPanelOtherPlaces_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPanelOtherPlaces, ShowMemberPanelOtherPlaces);
        private void ChangeMemberPointsPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPointsPanelColor, ShowMemberPointsPanelColor);
        private void ChangeMemberPointsStrokeColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPointsStrokeColor, ShowMemberPointsStrokeColor);
        private void ChangeMemberPointsFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPointsFontColor, ShowMemberPointsFontColor);
        private void ChangeMemberPlaceFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPlaceFontColor, ShowMemberPlaceFontColor);
        private void ChangeMemberResultFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberResultFontColor, ShowMemberResultFontColor);
        private void ChangeMemberResultPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberResultPanelColor, ShowMemberResultPanelColor);
        private void ChangeMemberResultStrokeColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberResultStrokeColor, ShowMemberResultStrokeColor);
        private void ChangeJuryPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.JuryPanelColor, ShowJuryPanelColor);
        private void ChangeBackgroundColor1_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.BackgroundColor1, ShowBackgroundColor1);
        private void ChangeBackgroundColor2_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.BackgroundColor2, ShowBackgroundColor2);
        private void ChangeMemberPlacePanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPlacePanelColor, ShowMemberPlacePanelColor);
        private void ChangeMemberPlaceStrokeColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.MemberPlaceStrokeColor, ShowMemberPlaceStrokeColor);
        private void ChangeLowerPanelFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref ProgramSettings.LowerPhraseFontColor, ShowLowerPanelFontColor);

        private void ChangeMemberPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPanelWidth);
        private void ChangeMemberPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPanelHeight);
        private void ChangeMemberNameFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberNameFontSize);
        private void ChangeMemberPointsPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPointsPanelHeight);
        private void ChangeMemberPointsPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPointsPanelWidth);
        private void ChangeMemberPointsFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPointsFontSize);
        private void ChangeMemberPointsStrokeWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPointsStrokeWidth);
        private void ChangeMemberPlaceFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPlaceFontSize);
        private void ChangeMemberPlacePanelOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref ProgramSettings.MemberPlacePanelOffset);
        private void ChangeMemberResultPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultPanelWidthTB, ref ProgramSettings.MemberResultPanelWidth);
        private void ChangeMemberResultPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultPanelHeightTB, ref ProgramSettings.MemberResultPanelHeight);
        private void ChangeMemberResultFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultFontSizeTB, ref ProgramSettings.MemberResultFontSize);
        private void ChangeMemberResultPanelOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultPanelOffsetTB, ref ProgramSettings.MemberResultPanelOffset);
        private void ChangeJuryPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryPanelWidthTB, ref ProgramSettings.JuryPanelWidth);
        private void ChangeJuryPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryPanelHeightTB, ref ProgramSettings.JuryPanelHeight);
        private void ChangeJuryFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryFontSizeTB, ref ProgramSettings.JuryFontSize);
        private void ChangePointBarFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(PointBarFontSizeTB, ref ProgramSettings.PointBarFontSize);
        private void ChangeTopJuryInterval(object sender, RoutedEventArgs e) => UpdateValueFromTB(TopJuryIntervalTB, ref ProgramSettings.TopJuryInterval);
        private void ChangeJuryMemberOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryMemberOffsetTB, ref ProgramSettings.JuryMemberOffset);
        private void ChangeMemberInterval(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberIntervalTB, ref ProgramSettings.MemberInterval);
        private void ChangeMemberColumnInterval(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberColumnIntervalTB, ref ProgramSettings.MemberColumnInterval);
        private void ChangeMaxMembersInColumn(object sender, RoutedEventArgs e) => UpdateValueFromTB(MaxMembersInColumnTB, ref ProgramSettings.MaxMembersInColumn);
        private void ChangeStartJury(object sender, RoutedEventArgs e) => UpdateValueFromTB(StartJuryTB, ref ProgramSettings.StartJury);
        private void ChangeMemberResultStrokeWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultStrokeWidthTB, ref ProgramSettings.MemberResultStrokeWidth);
        private void ChangeAnimMoveTime(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimMoveTimeTB, ref ProgramSettings.AnimMoveTime);
        private void ChangeAnimAppearTime(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimAppearTimeTB, ref ProgramSettings.AnimAppearTime);
        private void ChangeAnimPause(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimPauseTB, ref ProgramSettings.AnimPause);
        private void ChangeAnimPointBarPause(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimPointBarPauseTB, ref ProgramSettings.AnimPointBarPause);
        private void ChangeBackgroundAnimPeriod(object sender, RoutedEventArgs e) => UpdateValueFromTB(BackgroundAnimPeriodTB, ref ProgramSettings.BackgroundAnimPeriod);
        private void ChangeBackgroundAppearTime(object sender, RoutedEventArgs e) => UpdateValueFromTB(BackgroundAppearTimeTB, ref ProgramSettings.BackgroundAppearTime);
        private void ChangeMemberPlaceStrokeWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberPlaceStrokeWidthTB, ref ProgramSettings.MemberPlaceStrokeWidth);
        private void ChangeLowerPhraseOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(LowerPhraseOffsetTB, ref ProgramSettings.LowerPhraseOffset);
        private void ChangeLowerPhraseFontHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(LowerPhraseFontHeightTB, ref ProgramSettings.LowerPhraseFontSize);

        private void ChangeMemberPanelUseSecondShooseColor_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref ProgramSettings.MemberPanelUseSecondChooseColor);
        private void ChangeMemberPanelHighlightLeaders_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref ProgramSettings.MemberPanelHighlightLeaders);
        private void ChangeTrueTopRating_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref ProgramSettings.TrueTopRating);
        private void ChangeChangeTwoColumns_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref ProgramSettings.TwoColumns);        
        private void ChangeAnimatedBackground_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref ProgramSettings.AnimatedBackground);
        private void ChangeVideoBackground_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref ProgramSettings.VideoBackground);

        private void ChangeFinalPhrase(object sender, RoutedEventArgs e) => ProgramSettings.FinalPhrase = (sender as TextBox).Text ?? ProgramSettings.FinalPhrase;
        private void ChangeLowerPhrase(object sender, RoutedEventArgs e) => ProgramSettings.LowerPhrase = new TextRange((sender as RichTextBox).Document.ContentStart, (sender as RichTextBox).Document.ContentEnd).Text ?? ProgramSettings.LowerPhrase;

        private void ChooseMemberNameFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => ProgramSettings.MemberNameFontWeight = ConvertIndexToFontWeight(ChooseMemberNameFontWeight.SelectedIndex);
        private void ChooseMemberPlaceFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => ProgramSettings.MemberPlaceFontWeight = ConvertIndexToFontWeight(ChooseMemberPlaceFontWeight.SelectedIndex);
        private void ChooseMemberResultFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => ProgramSettings.MemberResultFontWeight = ConvertIndexToFontWeight((sender as ComboBox).SelectedIndex);
        private void ChooseLowerPhraseFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => ProgramSettings.LowerPhraseFontWeight = ConvertIndexToFontWeight((sender as ComboBox).SelectedIndex);

        private void ChooseMemberPlaceShowMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    ProgramSettings.MemberPlaceShowMode = ProgramSettings.PlaceShowMode.AlwaysVisible;
                    break;
                case 1:
                    ProgramSettings.MemberPlaceShowMode = ProgramSettings.PlaceShowMode.VisibleOnFS;
                    break;
                case 2:
                    ProgramSettings.MemberPlaceShowMode = ProgramSettings.PlaceShowMode.Hidden;
                    break;
            }
        }

        private void ChooseMemberSortingMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    ProgramSettings.MemberSortingMode = ProgramSettings.SortingMode.Ascending;
                    break;
                case 1:
                    ProgramSettings.MemberSortingMode = ProgramSettings.SortingMode.Descending;
                    break;
            }
        }

        private void ChangeShowMemberResultMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    ProgramSettings.ShowMemberResultMode = ProgramSettings.ResultShowMode.AlwaysVisible;
                    break;
                case 1:
                    ProgramSettings.ShowMemberResultMode = ProgramSettings.ResultShowMode.Visible;
                    break;
                case 2:
                    ProgramSettings.ShowMemberResultMode = ProgramSettings.ResultShowMode.Hidden;
                    break;
            }
        }

        private void ChangeMemberPointsMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    ProgramSettings.MemberPointsMode = ProgramSettings.PointsMode.Standard;
                    break;
                case 1:
                    ProgramSettings.MemberPointsMode = ProgramSettings.PointsMode.Ascending;
                    break;
                case 2:
                    ProgramSettings.MemberPointsMode = ProgramSettings.PointsMode.Descending;
                    break;
            }
        }
        
        private void ChooseShowMemberResultModee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    ProgramSettings.ShowMemberResultMode = ProgramSettings.ResultShowMode.AlwaysVisible;
                    break;
                case 1:
                    ProgramSettings.ShowMemberResultMode = ProgramSettings.ResultShowMode.Visible;
                    break;
                case 2:
                    ProgramSettings.ShowMemberResultMode = ProgramSettings.ResultShowMode.Hidden;
                    break;
            }
        }

        private void ChooseLowerPhraseShowMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    ProgramSettings.LowerPhraseShowMode = ProgramSettings.ShowMode.AlwaysVisible;
                    break;
                case 1:
                    ProgramSettings.LowerPhraseShowMode = ProgramSettings.ShowMode.OnlyXN;
                    break;
                case 2:
                    ProgramSettings.LowerPhraseShowMode = ProgramSettings.ShowMode.Never;
                    break;
            }
        }

        private void ChangeVideoChange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProgramSettings.VideoPath = dialog.FileName;
                VideoPathTB.Text = dialog.FileName;
            }
        }

        private void ChangeMainBackground_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    BitmapImage newImage = new BitmapImage(new Uri(dialog.FileName));
                    ProgramSettings.MainBackground = new ImageBrush(newImage);

                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(newImage));

                    if (File.Exists("program_background.bmp"))
                        File.Delete("program_background.bmp");

                    using (FileStream fs = new FileStream("program_background.bmp", FileMode.Create))
                    {
                        encoder.Save(fs);
                    }
                }
                catch(Exception exc)
                {
                    MessageBox.Show($"Не удалось загрузить картинку\n{exc.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Необходимо для осуществления обновления сцены по нажатию F5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ApplySettingsButton.Focus();
                ApplySettingsButton_Click(ApplySettingsButton, null);
            }
        }
    }
}