using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ResultViewerWPF
{
    /// <summary>
    /// Логика взаимодействия для EditPoints.xaml
    /// </summary>
    public partial class EditPoints : Window
    {
        Logic appLogic;

        string[] memberNames = null;
        double[][] selectedPoints = null;
        double[][] selectedValues = null;

        bool pointInputFocused = true;

        ToolTip helpToolTip = new ToolTip()
        {
            Content = new TextBlock()
            {
                Text = "Управление через клавиатуру\n\n - Стрелка вниз: выбрать следующего участника\n - Стрелка вверх: выбрать предыдущего участника\n - Стрелка вправо/влево: сменить поле ввода\n - Enter: ввести данные\n - Z: Выбрать следующего жюри\n - X: Выбрать предыдущего жюри"
            }
        };

        public EditPoints(Logic logic)
        {
            InitializeComponent();

            // Предоставляем доступ к редактируемым данным
            appLogic = logic;

            // Проверим, надо ли вводить дополнительные данные
            if (Viewer.ProgramSettings.ShowMemberResultMode == Viewer.ProgramSettings.ResultShowMode.Visible ||
                Viewer.ProgramSettings.ShowMemberResultMode == Viewer.ProgramSettings.ResultShowMode.AlwaysVisible)
            {
                // Надо, значит оставляем все элементы включенными. Также подгрузим дополнительную информацию
                selectedValues = appLogic.GetValues();
            }
            else
            {
                // Не надо, значит выключаем дополнительные элементы
                ValueInfo.Visibility = Visibility.Hidden;
                ValuesInput.Visibility = Visibility.Hidden;
                AddValue.Visibility = Visibility.Hidden;
            }

            // Получаем данные
            memberNames = appLogic.GetMemberNames();
            selectedPoints = appLogic.GetPoints();

            // Загружаем список жюри в ComboBox
            LoadJuryData();

            // Выбираем самого первого жюри в списке
            SelectedJury.SelectedIndex = 0;

            // Грузим данные по выбранному жюри
            ResetMemberData();

            // Выберем самого первого участника
            MemberList.SelectedIndex = 0;
        }

        private void LoadJuryData()
        {
            // Получаем список жюри
            string[] juryList = appLogic.GetJuryNames();

            // Загружаем имена в SelectedJury
            foreach (string s in juryList)
                SelectedJury.Items.Add(s);
            
            // Выберем самого первого участника
            MemberList.SelectedIndex = 0;
        }

        /// <summary>
        /// Обработать ввод пользователя
        /// </summary>
        /// <returns></returns>
        private bool HandleInput()
        {
            // Если были введены какие-то значения, то вернём true;
            bool handleResult = false;

            // Если отсутствует выделение, то не обрабатываем ввод
            if (MemberList.SelectedIndex < 0)
                return false;

            double fakeDVar = 0;

            PointsInput.Text = PointsInput.Text.Replace('.', ',');
            if (PointsInput.Text != string.Empty && double.TryParse(PointsInput.Text, out fakeDVar))
            {
                HandlePointsInput();
                handleResult = true;
            }
            else
            {
                PointsInput.Text = string.Empty;
            }

            ValuesInput.Text = ValuesInput.Text.Replace('.', ',');
            if (ValuesInput.Text != string.Empty && double.TryParse(ValuesInput.Text, out fakeDVar))
            {
                HandleValueInput();
                handleResult = true;
            }
            else
            {
                ValuesInput.Text = string.Empty;
            } 

            return handleResult;
        }

        private void HandleValueInput()
        {
            // Получим введённый результат
            double enteredValue = Convert.ToDouble(ValuesInput.Text);

            // Присвоим выбранному конкурсанту результат
            appLogic.SetValue(SelectedJury.SelectedIndex, MemberList.SelectedIndex, enteredValue);
            selectedValues[SelectedJury.SelectedIndex][MemberList.SelectedIndex] = enteredValue;

            // Очистим поле для ввода результата
            ValuesInput.Text = string.Empty;
        }

        private void HandlePointsInput()
        {
            // Получаем введённые баллы, конвертируем их из string в int
            double enteredPoints = Convert.ToDouble(PointsInput.Text);

            // Присваиваем выбранному конкурсанту данный балл
            appLogic.SetPoint(SelectedJury.SelectedIndex, MemberList.SelectedIndex, enteredPoints);
            selectedPoints[SelectedJury.SelectedIndex][MemberList.SelectedIndex] = enteredPoints;

            // Очищаем поле для ввода баллов
            PointsInput.Text = String.Empty;
        }

        private string checkNumber(double pointsNum)
        {
            if (pointsNum == 0)
            {
                return "X";
            }
            else
            {
                if (pointsNum == -1)
                {
                    return "H";
                }
                else
                    return pointsNum.ToString();
            }
        }

        private void ResetMemberData()
        {
            // Сохраняем индекс выбранного участника
            int selectedIndex = MemberList.SelectedIndex;

            // Очищаем список конкурсантов
            MemberList.Items.Clear();

            if (Viewer.ProgramSettings.ShowMemberResultMode == Viewer.ProgramSettings.ResultShowMode.Hidden)
            {
                // Получаем баллы по выбранному жюри
                double[] points = selectedPoints[SelectedJury.SelectedIndex];

                // Выводим данные в MemberList
                for (int i = 0; i < memberNames.Length; i++)
                    MemberList.Items.Add($"{memberNames[i]} -> баллов: {checkNumber(points[i])}");
            }
            else
            {
                // Получим все баллы и дополнительные данные по выбранному жюри
                double[] points = selectedPoints[SelectedJury.SelectedIndex];
                double[] values = selectedValues[SelectedJury.SelectedIndex];

                // Выводим данные в MemberList
                for (int i = 0; i < memberNames.Length; i++)
                {
                    MemberList.Items.Add(new ListBoxItem()
                    {
                        Content = new TextBlock()
                        {
                            Text = $"{memberNames[i]}\n-> Баллы: {checkNumber(points[i])}\n-> Результат: {values[i]}"
                        }
                    });
                }
            }

            // Вернём назад сохранённый индекс
            MemberList.SelectedIndex = selectedIndex;
        }

        private void PointsInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Если точка, то...
            if (e.Text == "," || e.Text == ".")
            {
                // Если точку/запятую не вводят в пустое поле или еще такие в поле не были введены, то всё норм
                if ((!PointsInput.Text.Contains(".") && PointsInput.Text.Length != 0) || (!PointsInput.Text.Contains(",") && PointsInput.Text.Length != 0))
                    return;
            }
            else
            {
                // Если пустая строка (char.isDigit не дружит с пустыми строками)
                if (e.Text == string.Empty)
                {
                    return;
                }
                else
                {
                    // Если это цифра, то всё норм
                    if (char.IsDigit(e.Text, 0))
                        return;
                }
            }

            e.Handled = true;
        }

        private void SelectedJury_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Прогружаем выбор только что выбранного жюри
            ResetMemberData();
        }

        private void AddValue_Click(object sender, RoutedEventArgs e)
        {
            // Если какие-то данные были обработаны
            if (HandleInput())
                // Сместим выделение на следующзего участника
                MemberList.SelectedIndex++;

            // Перезагрузим список участников, чтобы прогрузить новые данные
            ResetMemberData();
        }

        private void SubmitData_Click(object sender, RoutedEventArgs e)
        {
            // Передаём финальную весию AppLogic главному окну
            (Owner as MainWindow).AppLogic = appLogic;

            // Закрываем окно для редактирования баллов
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Убеждаемся в том, что пользователь уведомлен о том, что при закрытии формы все данные будут утеряны
            if (MessageBox.Show("При закрытии данной формы все введённые данные будут утеряны. Вы уверены, что хотите закрыть это окно?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Close();
        }

        private void MemberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Если был выбран какой-то участник
            if (MemberList.SelectedIndex != -1)
            {
                // Выводим его имя в SelectedMember
                ChosenMember.Content = memberNames[MemberList.SelectedIndex];

                // Смещаем фокус на PointsInput
                PointsInput.Focus();
                pointInputFocused = true;
            }
            else
                ChosenMember.Content = "Выберете участника";
        }

        /// <summary>
        /// Событие, которое обрабатывает ввод дополнитлеьных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValuesInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Если точка, то...
            if (e.Text == "," || e.Text == ".")
            {
                // Если точку/запятую не вводят в пустое поле или еще такие в поле не были введены, то всё норм
                if ((!ValuesInput.Text.Contains(".") && ValuesInput.Text.Length != 0) || (!ValuesInput.Text.Contains(",") && ValuesInput.Text.Length != 0))
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
        /// Остлеживает нажатия клавиш со стрелками для простого перемещения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка клавиши Enter
            // Ввод информации
            if (e.Key == Key.Enter && MemberList.SelectedIndex != -1)
            {
                // Если какие-то данные были обработаны
                if (HandleInput())
                    // Сместим выделение на следующзего участника
                    MemberList.SelectedIndex++;
                
                // Перезагрузим список участников, чтобы прогрузить новые данные
                ResetMemberData();
            }
            else
            {
                // Обработка клавиш со стрелками влево и вправо
                // Смена выделения полей ввода
                if (e.Key == Key.Left || e.Key == Key.Right)
                {
                    // Если выделена панель с баллами
                    if (pointInputFocused)
                    {
                        // Выделяем панель с результатом
                        ValuesInput.Focus();
                        pointInputFocused = false;
                    }
                    else
                    {
                        // Иначе выделяем панель с баллом
                        PointsInput.Focus();
                        pointInputFocused = true;
                    }
                }
                else
                {
                    // Обработка клавиши со стрелкой вверх
                    // Выбрать участника выше по списку
                    if (e.Key == Key.Up)
                    {
                        // Если выделен самый верхний участник
                        if (MemberList.SelectedIndex == 0)
                        {
                            // Выделяем самого нижнего
                            MemberList.SelectedIndex = MemberList.Items.Count - 1;
                        }
                        else
                        {
                            // Иначе просто выделяем участника выше по списку
                            MemberList.SelectedIndex--;
                        }
                    }
                    else
                    {
                        // Обработка нажатия клавиши со стрелкой вниз
                        // Выбрать участника ниже по списку
                        if (e.Key == Key.Down)
                        {
                            // Если выбран самый нижний участник
                            if (MemberList.SelectedIndex == MemberList.Items.Count - 1)
                            {
                                // Выделяем самого верхнего
                                MemberList.SelectedIndex = 0;
                            }
                            else
                            {
                                // Иначе просто выделяем участника ниже по списку
                                MemberList.SelectedIndex++;
                            }
                        }
                        else
                        {
                            // Обработка нажатия клавиши Z
                            // Выбрать следующего жюри
                            if (e.Key == Key.Z)
                            {
                                // Если выбран самый первый жюри
                                if (SelectedJury.SelectedIndex == 0)
                                {
                                    // Выберем самого последнего жюри
                                    SelectedJury.SelectedIndex = SelectedJury.Items.Count - 1;
                                    // Выберем самого верхнего участника по списку
                                    MemberList.SelectedIndex = 0;
                                }
                                else
                                {
                                    // Иначе выберем жюри выше по списку
                                    SelectedJury.SelectedIndex--;
                                    // Выберем самого верхнего участника по списку
                                    MemberList.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                // Обработка нажатия клавиши X
                                // Выбрать предыдущего жюри
                                if (e.Key == Key.X)
                                {
                                    // Если выбран последний жюри
                                    if (SelectedJury.SelectedIndex == SelectedJury.Items.Count - 1)
                                    {
                                        // Выберем самого первого жюри
                                        SelectedJury.SelectedIndex = 0;
                                        // Выберем самого верхнего участника по списку
                                        MemberList.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        // Иначе выберем жюри ниже по списку
                                        SelectedJury.SelectedIndex++;
                                        // Выберем самого верхнего участника по списку
                                        MemberList.SelectedIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Обработка события, которое возникает при наведении курсора мышки на панель со знаком вопроса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            // Покажем пользователю подсказку по управлению
            helpToolTip.IsOpen = true;
        }

        /// <summary>
        /// Обработка события, которое возникает при отводе курсора мыши от панели со знаком вопроса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            // Уберём подсказку
            helpToolTip.IsOpen = false;
        }

        private void SetZeroPointsButton_Click(object sender, RoutedEventArgs e)
        {
            // Участник получил 0 баллов
            PointsInput.Text = Constants.MEMBER_NO_POINTS.ToString();

            // Если какие-то данные были обработаны
            if (HandleInput())
                // Сместим выделение на следующзего участника
                MemberList.SelectedIndex++;

            // Перезагрузим список участников, чтобы прогрузить новые данные
            ResetMemberData();
        }

        private void SetAbsentButton_Click(object sender, RoutedEventArgs e)
        {
            // Участник отсутствовал
            PointsInput.Text = Constants.MEMBER_ABSENT.ToString();

            // Если какие-то данные были обработаны
            if (HandleInput())
                // Сместим выделение на следующзего участника
                MemberList.SelectedIndex++;

            // Перезагрузим список участников, чтобы прогрузить новые данные
            ResetMemberData();
        }

        //private void ValuesInput_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    TextBox currentTB = sender as TextBox;

        //    // Проверяет соответствие введённых данных численному значению с плавающей запятой
        //    double fakeVar = 0;
        //    if (!double.TryParse(currentTB.Text, out fakeVar) && currentTB.Text != string.Empty)
        //    {
        //        MessageBox.Show("Введены некорректные данные");
        //        currentTB.Focus();
        //        currentTB.Text = string.Empty;
        //    }
        //}
    }
}