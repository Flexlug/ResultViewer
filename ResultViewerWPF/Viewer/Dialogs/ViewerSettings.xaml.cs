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

using ResultViewerWPF.Viewer.Primitives;

namespace ResultViewerWPF.Viewer.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для 
    /// .xaml
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
            MemberPanelWidthTB.Text = Program.Settings.MemberPanelWidth.ToString();
            MemberPanelHeightTB.Text = Program.Settings.MemberPanelHeight.ToString();
            MemberNameFontSizeTB.Text = Program.Settings.MemberNameFontSize.ToString();
            MemberPanelStrokeWidthTB.Text = Program.Settings.MemberPanelStrokeWidth.ToString();

            ShowMemberPanelColor.Background = new SolidColorBrush(Program.Settings.MemberPanelColor);
            ShowMemberPanelChosenColor.Background = new SolidColorBrush(Program.Settings.MemberPanelChosenColor);
            ShowMemberPanelStrokeColor.Background = new SolidColorBrush(Program.Settings.MemberPanelStrokeColor);
            ChangeMemberPanelUseSecondShooseColor.IsChecked = Program.Settings.MemberPanelUseSecondChooseColor;
            ShowMemberPanelChosenColor2.Background = new SolidColorBrush(Program.Settings.MemberPanelChosenColor2);

            ChangeMemberPanelHighlightLeaders.IsChecked = Program.Settings.MemberPanelHighlightLeaders;

            ShowMemberPanelFirstPlace.Background = new SolidColorBrush(Program.Settings.MemberPanelFirstPlace);
            ShowMemberPanelSecondPlace.Background = new SolidColorBrush(Program.Settings.MemberPanelSecondPlace);
            ShowMemberPanelThirdPlace.Background = new SolidColorBrush(Program.Settings.MemberPanelThirdPlace);
            ShowMemberPanelOtherPlaces.Background = new SolidColorBrush(Program.Settings.MemberPanelOtherPlaces);

            ChooseMemberNameFontWeight.SelectedIndex = ConvertFontWeightToIndex(Program.Settings.MemberNameFontWeight);
            ShowMemberNameFontColor.Background = Program.Settings.MemberNameFontColor;

            MemberPointsFontSizeTB.Text = Program.Settings.MemberPointsFontSize.ToString();
            MemberPointsPanelHeightTB.Text = Program.Settings.MemberPointsPanelHeight.ToString();
            MemberPointsPanelWidthTB.Text = Program.Settings.MemberPointsPanelWidth.ToString();

            MemberPointsFontSizeTB.Text = Program.Settings.MemberPointsFontSize.ToString();
            MemberPointsPanelHeightTB.Text = Program.Settings.MemberPointsPanelHeight.ToString();
            MemberPointsPanelWidthTB.Text = Program.Settings.MemberPointsPanelWidth.ToString();
            MemberPointsStrokeWidthTB.Text = Program.Settings.MemberPointsStrokeWidth.ToString();
            ShowMemberPointsPanelColor.Background = new SolidColorBrush(Program.Settings.MemberPointsPanelColor);
            ShowMemberPointsStrokeColor.Background = new SolidColorBrush(Program.Settings.MemberPointsStrokeColor);
            ShowMemberPointsFontColor.Background = Program.Settings.MemberPointsFontColor;

            MemberPlaceFontSizeTB.Text = Program.Settings.MemberPlaceFontSize.ToString();
            MemberPlacePanelOffsetTB.Text = Program.Settings.MemberPlacePanelOffset.ToString();
            ShowMemberPlaceFontColor.Background = new SolidColorBrush(Program.Settings.MemberPlaceFontColor);
            ChooseMemberPlaceFontWeight.SelectedIndex = ConvertFontWeightToIndex(Program.Settings.MemberPlaceFontWeight);
            ChooseMemberPlaceShowMode.SelectedIndex = (int)Program.Settings.MemberPlaceShowMode;
            MemberPlaceStrokeWidthTB.Text = Program.Settings.MemberPlaceStrokeWidth.ToString();
            ShowMemberPlacePanelColor.Background = new SolidColorBrush(Program.Settings.MemberPlacePanelColor);
            ShowMemberPlaceStrokeColor.Background = new SolidColorBrush(Program.Settings.MemberPlaceStrokeColor);

            ChangeShowMemberResultMode.SelectedIndex = (int)Program.Settings.ShowMemberResultMode;
            MemberResultPanelWidthTB.Text = Program.Settings.MemberResultPanelWidth.ToString();
            MemberResultPanelHeightTB.Text = Program.Settings.MemberResultPanelHeight.ToString();
            MemberResultFontSizeTB.Text = Program.Settings.MemberResultFontSize.ToString();
            MemberResultPanelOffsetTB.Text = Program.Settings.MemberResultPanelOffset.ToString();
            MemberResultStrokeWidthTB.Text = Program.Settings.MemberResultStrokeWidth.ToString();
            ChooseMemberResultFontWeight.SelectedIndex = ConvertFontWeightToIndex(Program.Settings.MemberResultFontWeight);
            ShowMemberResultFontColor.Background = new SolidColorBrush(Program.Settings.MemberResultFontColor);
            ShowMemberResultPanelColor.Background = new SolidColorBrush(Program.Settings.MemberResultPanelColor);
            ShowMemberResultStrokeColor.Background = new SolidColorBrush(Program.Settings.MemberResultStrokeColor);

            JuryPanelWidthTB.Text = Program.Settings.JuryPanelWidth.ToString();
            JuryPanelHeightTB.Text = Program.Settings.JuryPanelHeight.ToString();
            JuryFontSizeTB.Text = Program.Settings.JuryFontSize.ToString();
            ShowJuryPanelColor.Background = new SolidColorBrush(Program.Settings.JuryPanelColor);

            PointBarFontSizeTB.Text = Program.Settings.PointBarFontSize.ToString();

            LowerPhraseOffsetTB.Text = Program.Settings.LowerPhraseOffset.ToString();
            LowerPhraseFontHeightTB.Text = Program.Settings.LowerPhraseFontSize.ToString();
            ChooseLowerPhraseFontWeight.SelectedIndex = ConvertFontWeightToIndex(Program.Settings.LowerPhraseFontWeight);
            ShowLowerPanelFontColor.Background = new SolidColorBrush(Program.Settings.LowerPhraseFontColor);
            LowerPhraseTB.Document.Blocks.Clear();
            ChooseLowerPhraseShowMode.SelectedIndex = (int)Program.Settings.LowerPhraseShowMode;
            foreach (string str in Program.Settings.LowerPhrase.Split('\n'))
            {
                LowerPhraseTB.AppendText(str);
                LowerPhraseTB.AppendText(Environment.NewLine);
            }

            TopJuryIntervalTB.Text = Program.Settings.TopJuryInterval.ToString();
            JuryMemberOffsetTB.Text = Program.Settings.JuryMemberOffset.ToString();
            MemberIntervalTB.Text = Program.Settings.MemberInterval.ToString();
            MemberColumnIntervalTB.Text = Program.Settings.MemberColumnInterval.ToString();
            ChooseMemberPointsMode.SelectedIndex = (int)Program.Settings.MemberPointsMode;
            ChooseMemberSortingMode.SelectedIndex = (int)Program.Settings.MemberSortingMode;
            ChangeTrueTopRating.IsChecked = Program.Settings.TrueTopRating;
            StartJuryTB.Text = Program.Settings.StartJury.ToString();
            ChangeTwoColumns.IsChecked = Program.Settings.TwoColumns;
            MaxMembersInColumnTB.Text = Program.Settings.MaxMembersInColumn.ToString();
            FinalPhraseTB.Text = Program.Settings.FinalPhrase;

            ChangeAnimatedBackground.IsChecked = Program.Settings.AnimatedBackground;
            ChangeVideoBackground.IsChecked = Program.Settings.VideoBackground;
            VideoPathTB.Text = Program.Settings.VideoPath;
            AnimMoveTimeTB.Text = Program.Settings.AnimMoveTime.TotalMilliseconds.ToString();
            AnimAppearTimeTB.Text = Program.Settings.AnimAppearTime.TotalMilliseconds.ToString();
            AnimPauseTB.Text = Program.Settings.AnimPause.TotalMilliseconds.ToString();
            AnimPointBarPauseTB.Text = Program.Settings.AnimPointBarPause.TotalMilliseconds.ToString();
            ShowBackgroundColor1.Background = new SolidColorBrush(Program.Settings.BackgroundColor1);
            ShowBackgroundColor2.Background = new SolidColorBrush(Program.Settings.BackgroundColor2);
            BackgroundAnimPeriodTB.Text = Program.Settings.BackgroundAnimPeriod.TotalMilliseconds.ToString();
            BackgroundAppearTimeTB.Text = Program.Settings.BackgroundAppearTime.TotalMilliseconds.ToString();

            LowerPhraseTB.Document.LineHeight = 0.1;
            #endregion

            StartViewer();
        }

        private void LoadCoordinates()
        {
            coordinates = new CoordinatesProvider((int)Width, (int)Height, pointPanel, Program.Settings.MaxMembersInColumn);
        }

        private void SetDefaultSettings(object sender, RoutedEventArgs e)
        {
            Program.Settings.MemberPanelWidth = 405;
            Program.Settings.MemberPanelHeight = 45;
            Program.Settings.MemberNameFontSize = 20;
            Program.Settings.MemberPanelOpacity = 1;
            Program.Settings.MemberPanelStrokeWidth = 1;
            Program.Settings.MemberPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.MemberPanelChosenColor = new Color()
            {
                A = 255,
                R = 255,
                G = 100,
                B = 100
            };

            Program.Settings.MemberPanelStrokeColor = new Color()
            {
                A = 255,
                R = 255,
                G = 255,
                B = 255
            };

            Program.Settings.MemberPanelChosenColor2 = new Color()
            {
                A = 255,
                R = 100,
                G = 255,
                B = 100
            };


            Program.Settings.MemberPanelHighlightLeaders = true;
            Program.Settings.MemberPanelFirstPlace = new Color()
            {
                A = 200,
                R = 255,
                G = 218,
                B = 6
            };

            Program.Settings.MemberPanelSecondPlace = new Color()
            {
                A = 200,
                R = 198,
                G = 198,
                B = 198
            };

            Program.Settings.MemberPanelThirdPlace = new Color()
            {
                A = 200,
                R = 170,
                G = 98,
                B = 0
            };

            Program.Settings.MemberPanelOtherPlaces = new Color()
            {
                A = 100,
                R = 50,
                G = 50,
                B = 50
            };

            Program.Settings.MemberPanelUseSecondChooseColor = true;
            Program.Settings.MemberNameFontWeight = FontWeights.Thin;
            Program.Settings.MemberNameFontColor = Brushes.Black;
            Program.Settings.MemberPointsFontWeight = FontWeights.Thin;
            Program.Settings.MemberPointsFontSize = 22;
            Program.Settings.MemberPointsPanelHeight = 45;
            Program.Settings.MemberPointsPanelWidth = 45;

            Program.Settings.MemberPointsPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.MemberPointsStrokeColor = new Color()
            {
                A = 200,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.MemberPointsFontColor = Brushes.Black;
            Program.Settings.MemberPointsStrokeWidth = 2;
            Program.Settings.MemberPlaceFontColor = Colors.Black;
            Program.Settings.MemberPlaceFontSize = 22;
            Program.Settings.MemberPlaceFontWeight = FontWeights.Black;
            Program.Settings.MemberPlacePanelOffset = 0;
            Program.Settings.MemberPlaceShowMode = Program.Settings.PlaceShowMode.AlwaysVisible;
            Program.Settings.MemberPlaceStrokeWidth = 2;

            Program.Settings.MemberPlacePanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.MemberPlaceStrokeColor = new Color()
            {
                A = 200,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.ShowMemberResultMode = Program.Settings.ResultShowMode.Visible;
            Program.Settings.MemberResultPanelWidth = 50;
            Program.Settings.MemberResultPanelHeight = 45;
            Program.Settings.MemberResultFontSize = 22;
            Program.Settings.MemberResultPanelOffset = 0;
            Program.Settings.MemberResultStrokeWidth = 2;
            Program.Settings.MemberResultFontWeight = FontWeights.Thin;
            Program.Settings.MemberResultFontColor = Colors.Black;

            Program.Settings.MemberResultPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.MemberResultStrokeColor = new Color()
            {
                A = 200,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.JuryPanelWidth = 405;
            Program.Settings.JuryPanelHeight = 35;
            Program.Settings.JuryPanelOpacity = 1;
            Program.Settings.JuryFontSize = 20;

            Program.Settings.JuryPanelColor = new Color()
            {
                A = 100,
                R = 200,
                G = 200,
                B = 200
            };

            Program.Settings.PointBarFontSize = 120;
            Program.Settings.PointBarPanelOpacity = 1;
            Program.Settings.TopJuryInterval = 100;
            Program.Settings.JuryMemberOffset = 100;
            Program.Settings.MemberInterval = 5;
            Program.Settings.MemberColumnInterval = 100;
            Program.Settings.MemberPointsMode = Program.Settings.PointsMode.Descending;
            Program.Settings.MemberSortingMode = Program.Settings.SortingMode.Descending;
            Program.Settings.TrueTopRating = true;
            Program.Settings.StartJury = 0;
            Program.Settings.TwoColumns = true;
            Program.Settings.MaxMembersInColumn = 10;
            Program.Settings.FinalPhrase = "Поздравляем победителей!";


            Program.Settings.LowerPhraseOffset = 100;
            Program.Settings.LowerPhraseFontSize = 10;
            Program.Settings.LowerPhraseFontWeight = FontWeights.Normal;
            Program.Settings.LowerPhraseFontColor = Colors.Black;
            Program.Settings.LowerPhraseShowMode = Program.Settings.ShowMode.AlwaysVisible;
            Program.Settings.LowerPhrase = "Н - не явившиеся на конкурс \nX - не приславшие конкурсную работу";

            Program.Settings.MainBackground = new ImageBrush(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.mainBackground2.GetHbitmap(),
                                                                              IntPtr.Zero,
                                                                              Int32Rect.Empty,
                                                                              BitmapSizeOptions.FromEmptyOptions()));
            Program.Settings.ShowPointAnim = true;
            Program.Settings.AnimatedBackground = false;
            Program.Settings.VideoBackground = false;
            Program.Settings.VideoPath = "background.mp4";
            Program.Settings.AnimMoveTime = TimeSpan.FromMilliseconds(500);
            Program.Settings.AnimAppearTime = TimeSpan.FromMilliseconds(500);
            Program.Settings.AnimPause = TimeSpan.FromMilliseconds(1100);
            Program.Settings.AnimPointBarPause = TimeSpan.FromMilliseconds(500);
            Program.Settings.BackgroundColor1 = Colors.DarkBlue;
            Program.Settings.BackgroundColor2 = Colors.DeepSkyBlue;
            Program.Settings.BackgroundAnimPeriod = TimeSpan.FromSeconds(3);
            Program.Settings.BackgroundAppearTime = TimeSpan.FromSeconds(2);

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

            if (loadingLogic.GetMemberNames().Length > Program.Settings.MaxMembersInColumn)
            {
                if (Program.Settings.TwoColumns)
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

            this.Background = Program.Settings.MainBackground;

            // Запускаем анимацию проявления
            GraphicsEngine.BcgAppear(MainCanvas.Background);

            // Если анимированный фон включён
            if (Program.Settings.AnimatedBackground)
            {
                // Если в качестве анимации выступает видео
                if (Program.Settings.VideoBackground)
                {
                    // Проверим наличие файла
                    if (File.Exists(Program.Settings.VideoPath))
                    {
                        // На случай, если файл битый
                        try
                        {
                            // Загрузим видео на фон....

                            MediaElement video = new MediaElement()
                            {
                                Source = new Uri(Program.Settings.VideoPath),
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
                        catch(Exception)
                        {
                            MessageBox.Show("Ошибка при загрузке видео", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            Background = Program.Settings.MainBackground;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Не получилось загрузить видео на фон. Проверьте наличие файла {Program.Settings.VideoPath}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Background = Program.Settings.MainBackground;
                    }
                }
                else
                {
                    // Присваеваем фону новый цвет
                    Background = new SolidColorBrush(Program.Settings.BackgroundColor1);

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
            if (Program.Settings.ShowMemberResultMode == Program.Settings.ResultShowMode.Visible ||
                Program.Settings.ShowMemberResultMode == Program.Settings.ResultShowMode.AlwaysVisible)
            {
                // Загрузим массив с дополнитльеными результатами
                double[][] memberValues = loadingLogic.GetValues();

                // Проставляем участникам их дополнительные результаты
                for (int i = 0; i < memberPanels.Count; i++)
                    memberPanels[i].Value = memberValues[0][i];
            }

            // Проверим, с какого жюри нам надо начать показ
            if (Program.Settings.StartJury != 0)
            {
                // Просчитаем все баллы, которые необходимо добавить
                int[] calculatedPoints = new int[memberNames.Length];

                // Проставим просчитанные баллы участникам
                for (int jur = 0; jur < Program.Settings.StartJury; jur++)
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

            lowerPhrase = new TextBar(MainCanvas, Program.Settings.LowerPhrase);

            if (Program.Settings.MemberPanelUseSecondChooseColor)
                GraphicsEngine.ChangeMemberColor(memberPanels[0], Program.Settings.MemberPanelChosenColor2);
            GraphicsEngine.ChangeMemberColor(memberPanels[1], Program.Settings.MemberPanelChosenColor, (e3, ev3) =>
            {
                GraphicsEngine.Wait(Program.Settings.AnimPause, (object e, EventArgs ev) =>
                {
                    pointPanel.NumOfPoints = 10;
                    memberPanels[1].IsChosen = true;
                    memberPanels[1].IsColored = true;
                    GraphicsEngine.MoveToInstant(pointPanel, coordinates.PointBar(-1));
                    GraphicsEngine.Appear(pointPanel);
                    GraphicsEngine.Resize(pointPanel, 1.5, (object e1, EventArgs ev1) =>
                    {
                        GraphicsEngine.Wait(Program.Settings.AnimPointBarPause, (object e2, EventArgs ev2) =>
                        {
                            GraphicsEngine.MoveTo(pointPanel, coordinates.PointBar(1));
                            GraphicsEngine.Resize(pointPanel, 0.1);
                            memberPanels[1].Points += pointPanel.NumOfPoints;
                            GraphicsEngine.Disappear(pointPanel, new EventHandler((object e4, EventArgs ev4) =>
                            {

                                GraphicsEngine.MoveToInstant(lowerPhrase, coordinates.LowerFrase(lowerPhrase.GetPanelWidth(), lowerPhrase.GetPanelHeight()));
                                GraphicsEngine.Appear(lowerPhrase);
                                SortMembers();
                                UpdatePlaces();

                                // Обновляем позиции участников
                                for (int i = 0; i < memberPanels.Count; i++)
                                {
                                    GraphicsEngine.MoveTo(memberPanels[i], coordinates.Member(i), new EventHandler((object objc, EventArgs evc) =>
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
            if (Program.Settings.MemberSortingMode == Program.Settings.SortingMode.Ascending)
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
            if (Program.Settings.TrueTopRating)
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
            if (Program.Settings.MemberPanelUseSecondChooseColor)
                GraphicsEngine.ChangeMemberColor(memberPanels[0], Program.Settings.MemberPanelChosenColor);
            GraphicsEngine.ChangeMemberColor(memberPanels[1], Program.Settings.MemberPanelChosenColor2);
            for (int i = 2; i < memberPanels.Count; i++)
                GraphicsEngine.ChangeMemberColor(memberPanels[i], Program.Settings.MemberPanelColor);
        }

        private void ShowFinalscreenColors(object sender, RoutedEventArgs e)
        {
            GraphicsEngine.ChangeMemberColor(memberPanels[0], Program.Settings.MemberPanelFirstPlace);
            GraphicsEngine.ChangeMemberColor(memberPanels[1], Program.Settings.MemberPanelSecondPlace);
            GraphicsEngine.ChangeMemberColor(memberPanels[2], Program.Settings.MemberPanelThirdPlace);
            for (int i = 3; i < memberPanels.Count; i++)
                GraphicsEngine.ChangeMemberColor(memberPanels[i], Program.Settings.MemberPanelOtherPlaces);
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

        private void ChangeMemberPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelColor, ShowMemberPanelColor);
        private void ChangeMemberPanelChosenColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelChosenColor, ShowMemberPanelChosenColor);
        private void ChangeMemberPanelChosenColor2_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelChosenColor2, ShowMemberPanelChosenColor2);
        private void ChangeMemberPanelStrokeColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelStrokeColor, ShowMemberPanelStrokeColor);
        private void ChangeMemberNameFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberNameFontColor, ShowMemberNameFontColor);
        private void ChangeMemberPanelFirstPlace_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelFirstPlace, ShowMemberPanelFirstPlace);
        private void ChangeMemberPanelSecondPlace_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelSecondPlace, ShowMemberPanelSecondPlace);
        private void ChangeMemberPanelThirdPlace_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelThirdPlace, ShowMemberPanelThirdPlace);
        private void ChangeMemberPanelOtherPlaces_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPanelOtherPlaces, ShowMemberPanelOtherPlaces);
        private void ChangeMemberPointsPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPointsPanelColor, ShowMemberPointsPanelColor);
        private void ChangeMemberPointsStrokeColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPointsStrokeColor, ShowMemberPointsStrokeColor);
        private void ChangeMemberPointsFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPointsFontColor, ShowMemberPointsFontColor);
        private void ChangeMemberPlaceFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPlaceFontColor, ShowMemberPlaceFontColor);
        private void ChangeMemberResultFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberResultFontColor, ShowMemberResultFontColor);
        private void ChangeMemberResultPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberResultPanelColor, ShowMemberResultPanelColor);
        private void ChangeMemberResultStrokeColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberResultStrokeColor, ShowMemberResultStrokeColor);
        private void ChangeJuryPanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.JuryPanelColor, ShowJuryPanelColor);
        private void ChangeBackgroundColor1_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.BackgroundColor1, ShowBackgroundColor1);
        private void ChangeBackgroundColor2_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.BackgroundColor2, ShowBackgroundColor2);
        private void ChangeMemberPlacePanelColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPlacePanelColor, ShowMemberPlacePanelColor);
        private void ChangeMemberPlaceStrokeColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.MemberPlaceStrokeColor, ShowMemberPlaceStrokeColor);
        private void ChangeLowerPanelFontColor_Click(object sender, RoutedEventArgs e) => UpdateColor(ref Program.Settings.LowerPhraseFontColor, ShowLowerPanelFontColor);

        private void ChangeMemberPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPanelWidth);
        private void ChangeMemberPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPanelHeight);
        private void ChangeMemberNameFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberNameFontSize);
        private void ChangeMemberPanelStrokeWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPanelStrokeWidth);
        private void ChangeMemberPointsPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPointsPanelHeight);
        private void ChangeMemberPointsPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPointsPanelWidth);
        private void ChangeMemberPointsFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPointsFontSize);
        private void ChangeMemberPointsStrokeWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPointsStrokeWidth);
        private void ChangeMemberPlaceFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPlaceFontSize);
        private void ChangeMemberPlacePanelOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(sender as TextBox, ref Program.Settings.MemberPlacePanelOffset);
        private void ChangeMemberResultPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultPanelWidthTB, ref Program.Settings.MemberResultPanelWidth);
        private void ChangeMemberResultPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultPanelHeightTB, ref Program.Settings.MemberResultPanelHeight);
        private void ChangeMemberResultFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultFontSizeTB, ref Program.Settings.MemberResultFontSize);
        private void ChangeMemberResultPanelOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultPanelOffsetTB, ref Program.Settings.MemberResultPanelOffset);
        private void ChangeJuryPanelWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryPanelWidthTB, ref Program.Settings.JuryPanelWidth);
        private void ChangeJuryPanelHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryPanelHeightTB, ref Program.Settings.JuryPanelHeight);
        private void ChangeJuryFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryFontSizeTB, ref Program.Settings.JuryFontSize);
        private void ChangePointBarFontSize(object sender, RoutedEventArgs e) => UpdateValueFromTB(PointBarFontSizeTB, ref Program.Settings.PointBarFontSize);
        private void ChangeTopJuryInterval(object sender, RoutedEventArgs e) => UpdateValueFromTB(TopJuryIntervalTB, ref Program.Settings.TopJuryInterval);
        private void ChangeJuryMemberOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(JuryMemberOffsetTB, ref Program.Settings.JuryMemberOffset);
        private void ChangeMemberInterval(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberIntervalTB, ref Program.Settings.MemberInterval);
        private void ChangeMemberColumnInterval(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberColumnIntervalTB, ref Program.Settings.MemberColumnInterval);
        private void ChangeMaxMembersInColumn(object sender, RoutedEventArgs e) => UpdateValueFromTB(MaxMembersInColumnTB, ref Program.Settings.MaxMembersInColumn);
        private void ChangeStartJury(object sender, RoutedEventArgs e) => UpdateValueFromTB(StartJuryTB, ref Program.Settings.StartJury);
        private void ChangeMemberResultStrokeWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberResultStrokeWidthTB, ref Program.Settings.MemberResultStrokeWidth);
        private void ChangeAnimMoveTime(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimMoveTimeTB, ref Program.Settings.AnimMoveTime);
        private void ChangeAnimAppearTime(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimAppearTimeTB, ref Program.Settings.AnimAppearTime);
        private void ChangeAnimPause(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimPauseTB, ref Program.Settings.AnimPause);
        private void ChangeAnimPointBarPause(object sender, RoutedEventArgs e) => UpdateValueFromTB(AnimPointBarPauseTB, ref Program.Settings.AnimPointBarPause);
        private void ChangeBackgroundAnimPeriod(object sender, RoutedEventArgs e) => UpdateValueFromTB(BackgroundAnimPeriodTB, ref Program.Settings.BackgroundAnimPeriod);
        private void ChangeBackgroundAppearTime(object sender, RoutedEventArgs e) => UpdateValueFromTB(BackgroundAppearTimeTB, ref Program.Settings.BackgroundAppearTime);
        private void ChangeMemberPlaceStrokeWidth(object sender, RoutedEventArgs e) => UpdateValueFromTB(MemberPlaceStrokeWidthTB, ref Program.Settings.MemberPlaceStrokeWidth);
        private void ChangeLowerPhraseOffset(object sender, RoutedEventArgs e) => UpdateValueFromTB(LowerPhraseOffsetTB, ref Program.Settings.LowerPhraseOffset);
        private void ChangeLowerPhraseFontHeight(object sender, RoutedEventArgs e) => UpdateValueFromTB(LowerPhraseFontHeightTB, ref Program.Settings.LowerPhraseFontSize);

        private void ChangeMemberPanelUseSecondShooseColor_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref Program.Settings.MemberPanelUseSecondChooseColor);
        private void ChangeMemberPanelHighlightLeaders_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref Program.Settings.MemberPanelHighlightLeaders);
        private void ChangeTrueTopRating_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref Program.Settings.TrueTopRating);
        private void ChangeChangeTwoColumns_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref Program.Settings.TwoColumns);        
        private void ChangeAnimatedBackground_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref Program.Settings.AnimatedBackground);
        private void ChangeVideoBackground_Click(object sender, RoutedEventArgs e) => UpdateValueFromCheckBox(sender as CheckBox, ref Program.Settings.VideoBackground);

        private void ChangeFinalPhrase(object sender, RoutedEventArgs e) => Program.Settings.FinalPhrase = (sender as TextBox).Text ?? Program.Settings.FinalPhrase;
        private void ChangeLowerPhrase(object sender, RoutedEventArgs e) => Program.Settings.LowerPhrase = new TextRange((sender as RichTextBox).Document.ContentStart, (sender as RichTextBox).Document.ContentEnd).Text ?? Program.Settings.LowerPhrase;

        private void ChooseMemberNameFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => Program.Settings.MemberNameFontWeight = ConvertIndexToFontWeight(ChooseMemberNameFontWeight.SelectedIndex);
        private void ChooseMemberPlaceFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => Program.Settings.MemberPlaceFontWeight = ConvertIndexToFontWeight(ChooseMemberPlaceFontWeight.SelectedIndex);
        private void ChooseMemberResultFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => Program.Settings.MemberResultFontWeight = ConvertIndexToFontWeight((sender as ComboBox).SelectedIndex);
        private void ChooseLowerPhraseFontWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) => Program.Settings.LowerPhraseFontWeight = ConvertIndexToFontWeight((sender as ComboBox).SelectedIndex);

        private void ChooseMemberPlaceShowMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Program.Settings.MemberPlaceShowMode = Program.Settings.PlaceShowMode.AlwaysVisible;
                    break;
                case 1:
                    Program.Settings.MemberPlaceShowMode = Program.Settings.PlaceShowMode.VisibleOnFS;
                    break;
                case 2:
                    Program.Settings.MemberPlaceShowMode = Program.Settings.PlaceShowMode.Hidden;
                    break;
            }
        }

        private void ChooseMemberSortingMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Program.Settings.MemberSortingMode = Program.Settings.SortingMode.Ascending;
                    break;
                case 1:
                    Program.Settings.MemberSortingMode = Program.Settings.SortingMode.Descending;
                    break;
            }
        }

        private void ChangeShowMemberResultMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Program.Settings.ShowMemberResultMode = Program.Settings.ResultShowMode.AlwaysVisible;
                    break;
                case 1:
                    Program.Settings.ShowMemberResultMode = Program.Settings.ResultShowMode.Visible;
                    break;
                case 2:
                    Program.Settings.ShowMemberResultMode = Program.Settings.ResultShowMode.Hidden;
                    break;
            }
        }

        private void ChangeMemberPointsMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Program.Settings.MemberPointsMode = Program.Settings.PointsMode.Standard;
                    break;
                case 1:
                    Program.Settings.MemberPointsMode = Program.Settings.PointsMode.Ascending;
                    break;
                case 2:
                    Program.Settings.MemberPointsMode = Program.Settings.PointsMode.Descending;
                    break;
            }
        }
        
        private void ChooseShowMemberResultModee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Program.Settings.ShowMemberResultMode = Program.Settings.ResultShowMode.AlwaysVisible;
                    break;
                case 1:
                    Program.Settings.ShowMemberResultMode = Program.Settings.ResultShowMode.Visible;
                    break;
                case 2:
                    Program.Settings.ShowMemberResultMode = Program.Settings.ResultShowMode.Hidden;
                    break;
            }
        }

        private void ChooseLowerPhraseShowMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Program.Settings.LowerPhraseShowMode = Program.Settings.ShowMode.AlwaysVisible;
                    break;
                case 1:
                    Program.Settings.LowerPhraseShowMode = Program.Settings.ShowMode.OnlyXN;
                    break;
                case 2:
                    Program.Settings.LowerPhraseShowMode = Program.Settings.ShowMode.Never;
                    break;
            }
        }

        private void ChangeVideoChange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Program.Settings.VideoPath = dialog.FileName;
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
                    Program.Settings.MainBackground = new ImageBrush(newImage);

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

        private void NominationWizard_Click(object sender, RoutedEventArgs e)
        {
            NominationsWizard wizard = new NominationsWizard();
            wizard.ShowDialog();
        }
    }
}