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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Diagnostics;

using ResultViewerWPF.Viewer.Primitives;

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Логика взаимодействия для FastViewer.xaml
    /// </summary>
    public partial class FastViewer : Window
    {
        Logic appLogic;
        List<MemberBar> memberPanels;

        JuryBar finalPhrase;

        CoordinatesProvider coordinates;

        double[] finalPoints = null;

        public FastViewer(Logic _appLogic)
        {
            InitializeComponent();

            this.appLogic = _appLogic;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Инициализируем координатную систему
            coordinates = new CoordinatesProvider((int)Width, (int)Height, null, Program.Settings.MaxMembersInColumn);

            // Выставляем холст
            GraphicsEngine.CurrentCanvas = mainCanvas;

            // Поставим фон
            Background = Program.Settings.MainBackground;

            // Инициализируем и заполним коллекцию из участников
            memberPanels = new List<MemberBar>();
            string[] memberNames = appLogic.GetMemberNames();
            for (int i = 0; i < memberNames.Length; i++)
                memberPanels.Add(new MemberBar(mainCanvas, memberNames[i], i));

            
            SortMembers();
            UpdatePlaces();

            // Инициализируем панель с финальной фразой
            finalPhrase = new JuryBar(mainCanvas, Program.Settings.FinalPhrase);

            // Расставляем участников
            for (int i = 0; i < memberPanels.Count; i++)
            {
                GraphicsEngine.MoveTo(memberPanels[i], coordinates.Member(i));
                GraphicsEngine.Appear(memberPanels[i]);
            }

            GraphicsEngine.MoveTo(finalPhrase, coordinates.Jury());
            GraphicsEngine.Appear(finalPhrase);

            // Проверяем, включено ли использоване двух колонок по умолчанию
            if (Program.Settings.TwoColumns)
            {
                UseTwoCloumns.IsChecked = true;
            }
            else
            {
                UseTwoCloumns.IsChecked = false;
            }

            // Проверяем, надо ли выделять первые три места отдельным цветом
            if (Program.Settings.MemberPanelHighlightLeaders)
            {
                UseTopColors.IsChecked = true;
            }
            else
            {
                UseTopColors.IsChecked = false;
            }

            // Изменим цвет участников, если это необходимо
            if (Program.Settings.MemberPanelHighlightLeaders)
            {
                EnableTopColors(null, null);
            }
            else
            {
                UseTopColors.IsChecked = false;
            }

            // Инициализация просмотра баллов

            // Заполнение ComboBox-а
            foreach (string jury in appLogic.GetJuryNames())
                ChooseJury.Items.Add(jury);

            // При финальной фразе высвечивает сумму всех баллов
            ChooseJury.Items.Add(Program.Settings.FinalPhrase);

            // Выберем последний вариант (финальная фраза)
            ChooseJury.SelectedIndex = ChooseJury.Items.Count - 1;

            // Просчитаем сумму всех баллов
            finalPoints = new double[memberPanels.Count];

            double point = 0;

            for (int jury = 0; jury < appLogic.GetJuryNames().Length; jury++)
                for (int member = 0; member < memberPanels.Count; member++)
                    if ((point = appLogic.GetPoint(jury, member)) > 0)
                        finalPoints[member] += point;

            ShowValues.IsChecked = false;

            // Включим показ финала
            SetJury(ChooseJury.SelectedIndex);

            ChooseJury.SelectionChanged += ChooseJury_SelectionChanged;

            // Проявим фон
            mainCanvas.Background = new SolidColorBrush(Colors.Black);
            GraphicsEngine.BcgAppear(mainCanvas.Background);
        }

        /// <summary>
        /// Обновляет положение участников в топе
        /// </summary>
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
        /// Сортирует участников в соответствии с заданными настройками
        /// </summary>
        private void SortMembers()
        {
            List<MemberBar> membersWithPoints = null;

            // Сортируем участников порядке возрастания/убывания в соответствии с настройками программы
            if (Program.Settings.MemberSortingMode == Program.Settings.SortingMode.Ascending)
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

        /// <summary>
        /// Задаёт жюри, выбор которого надо показать ((кол-во жюри + 1 ) - id финального показа)
        /// </summary>
        /// <param name="jury"></param>
        private void SetJury(int jury)
        {
            if (jury >= ChooseJury.Items.Count)
                throw new IndexOutOfRangeException("Jury id is too big");

            if (jury == ChooseJury.Items.Count - 1)
            {
                // Final phrase

                // Присвоим просчитанные баллы участникам
                for (int member = 0; member < memberPanels.Count; member++)
                {
                    memberPanels.Find(panel => panel.ID == member).Points = finalPoints[member];
                    memberPanels[member].ShowMask = false;
                }

                // Выключим отображение результатов
                if (ShowValues.IsChecked ?? false)
                {
                    ShowValues.IsChecked = false;
                }
            }
            else
            {
                // Some jury

                MemberBar memberBar;

                // Присвоим соответствующие баллы участникам и проставим соответстующие результаты
                for (int member = 0; member < memberPanels.Count; member++)
                {
                    memberBar = memberPanels.Find(panel => panel.ID == member);
                    memberBar.Points = appLogic.GetPoint(jury, member);
                    memberBar.Value = appLogic.GetValue(jury, member);

                    if (memberBar.Points == Constants.MEMBER_NO_POINTS)
                    {
                        memberBar.MemberMaskType = MemberBar.MaskType.NoPoints;
                        memberBar.ShowMask = true;
                    }
                    else
                    {
                        if (memberBar.Points == Constants.MEMBER_ABSENT)
                        {
                            memberBar.MemberMaskType = MemberBar.MaskType.Absent;
                            memberBar.ShowMask = true;
                        }
                        else
                        {
                            memberBar.ShowMask = false;
                        }
                    }
                }

            }

            finalPhrase.Name = ChooseJury.Items[ChooseJury.SelectedIndex].ToString();

            SortMembers();
            UpdateMembersLayout();
            UpdatePlaces();

            // Обновим цвет первых трёх мест
            if (UseTopColors.IsChecked ?? false)
                EnableTopColors(null, null);

            // Если флажок на отображение результатов включён, но они еще невидимы, то включаем их
            if ((ShowValues.IsChecked ?? false) && !memberPanels[0].ValueVisible)
                foreach (MemberBar mem in memberPanels)
                    mem.ToggleValue();

            // Если флажок на отображение результатов отключен, но результаты отображаются, то выключим их
            if ((!ShowValues.IsChecked ?? false) && memberPanels[0].ValueVisible)
                foreach (MemberBar mem in memberPanels)
                    mem.ToggleValue();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Уберём лишнее
            UseTopColors.Visibility = Visibility.Hidden;
            UseTwoCloumns.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            ShowValues.Visibility = Visibility.Hidden;
            ChooseJury.Visibility = Visibility.Hidden;

            // Создадим скриншот (warning, magic)
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(this);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            // Создадим файл для хранения скриншота
            FileStream file = new FileStream($"Screenshot_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.png", FileMode.CreateNew);

            // Запишем только что полученный скрин в файл
            encoder.Save(file);

            // Откроем файл скриншота
            try
            {
                Process.Start(file.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при открытии скриншота. Проверьте папку с программой. Возможно картинка сохранена успешно, но возникли неполадки с программой для просмотра изображений\n\nПодробности об ошибке: {ex.Message}\n{ex.Source}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Вернём всё на свои места
            UseTopColors.Visibility = Visibility.Visible;
            UseTwoCloumns.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Visible;
            ShowValues.Visibility = Visibility.Visible;
            ChooseJury.Visibility = Visibility.Visible;
        }

        private void UseTwoCloumns_Checked(object sender, RoutedEventArgs e)
        {
            coordinates.TwoColumns = true;
            UpdateMembersLayout();
        }
        private void UseTwoCloumns_Unchecked(object sender, RoutedEventArgs e)
        {
            coordinates.TwoColumns = false;
            UpdateMembersLayout();
        }
        private void UpdateMembersLayout()
        {
            // Заново расставим панели
            for (int i = 0; i < memberPanels.Count; i++)
                GraphicsEngine.MoveTo(memberPanels[i], coordinates.Member(i));
        }

        private void EnableTopColors(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < memberPanels.Count; i++)
            {
                if (memberPanels[i].Place == 1)
                {
                    GraphicsEngine.ChangeMemberColor(memberPanels[i], Program.Settings.MemberPanelFirstPlace);
                }
                else
                {
                    if (memberPanels[i].Place == 2)
                    {
                        GraphicsEngine.ChangeMemberColor(memberPanels[i], Program.Settings.MemberPanelSecondPlace);
                    }
                    else
                    {
                        if (memberPanels[i].Place == 3)
                        {
                            GraphicsEngine.ChangeMemberColor(memberPanels[i], Program.Settings.MemberPanelThirdPlace);
                        }
                        else
                        {
                            GraphicsEngine.ChangeMemberColor(memberPanels[i], Program.Settings.MemberPanelOtherPlaces);
                        }
                    }
                }
            };
        }
        
        private void DisableTopColors(object sender, RoutedEventArgs e)
        {
            foreach (MemberBar mem in memberPanels)
                GraphicsEngine.ChangeMemberColor(mem, Program.Settings.MemberPanelColor);
        }

        /// <summary>
        /// Включает отображение дополнительных результатов
        /// </summary>
        private void ShowValues_Checked(object sender, RoutedEventArgs e)
        {
            if (ChooseJury.SelectedIndex == ChooseJury.Items.Count - 1)
            {
                e.Handled = true;
            }
            else
            {
                foreach (MemberBar member in memberPanels)
                    member.ToggleValue();
            }
        }

        /// <summary>
        /// Отключает отображение дополнительных результатов
        /// </summary>
        private void ShowValues_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowValues_Checked(sender, e);
        }

        /// <summary>
        /// Позволяет выбрать проставленные баллы от какого-либо жюри
        /// </summary>
        private void ChooseJury_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetJury((sender as ComboBox).SelectedIndex);
        }
    }
}
