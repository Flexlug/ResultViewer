using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ResultViewerWPF;
using ResultViewerWPF.Viewer.Dialogs;
using ResultViewerWPF.Compitability;
using System.Diagnostics;

namespace ResultViewerWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Главная логика приложения
        /// </summary>
        public Logic AppLogic;

        /// <summary>
        /// Окно для быстрого показа результатов
        /// </summary>
        public QuickResultShow quickShow = null;

        /// <summary>
        /// Окно с настройками программы
        /// </summary>
        public ViewerSettings settings = null;

        /// <summary>
        /// Режим совсестимости со старым режимом
        /// </summary>
        public bool CompitabilityMode = false;

        public MainWindow()
        {
            InitializeComponent();

            AppLogic = new Logic();

            // Проверим наличие сохранённого фона
            if (File.Exists("program_background.bmp"))
            {
                try
                {
                    Uri myUri = new Uri("program_background.bmp", UriKind.RelativeOrAbsolute);
                    BmpBitmapDecoder loadingImage = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    BitmapSource image = loadingImage.Frames[0];
                    Program.Settings.MainBackground = new ImageBrush(image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Не удалось загрузить фоновое изображение из файла \"program_background.bmp\"\n{exc.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadOrEditData_Click(object sender, RoutedEventArgs e)
        {
            EditInvolved ei;
            Logic oldLogic = AppLogic;

            // Уведомляем пользователя о начале редактирования данных
            UpdateStatus("Режим редактирования");

            if (AppLogic.IsEmpty())
            {
                // Инициализируем форму для ввода имён жюри и участников
                ei = new EditInvolved();
            }
            else
            {
                ei = new EditInvolved(AppLogic);
            }
            // Присваиваем полю owner ссылку на данное окно, чтобы вдальнейшеи можно было связаться с ним
            ei.Owner = this;

            // Начинаем диалог
            ei.ShowDialog();

            // Проверяем, изменилась ли ссылка после редактирования. Если изменилась, то данные были изменены, иначе редактирование было отменено
            if (ReferenceEquals(AppLogic, oldLogic))
                UpdateStatus("Отменено");
            else
                UpdateStatus("Данные успешно изменены");

            UpdateDataStatus();
        }

        private void StartViewer_Click(object sender, RoutedEventArgs e)
        {
            #region Выводим данные в MessageBox [закомментировано]

            //if (!AppLogic.IsEmpty())
            //{
            //    StringBuilder output = new StringBuilder();
            //    output.Append("Имена участников:\n");
            //    foreach (string s in AppLogic.GetMemberNames())
            //        output.Append($"    {s}\n");
            //    output.Append("Имена жюри:\n");
            //    foreach (string s in AppLogic.GetJuryNames())
            //        output.Append($"    {s}\n");
            //    output.Append("Введённые баллы:\n");

            //    string[] memberNames = AppLogic.GetMemberNames();

            //    foreach (List<int> l in AppLogic.GetJuryChoice().Values)
            //        for (int i = 0; i < memberNames.Length; i++)
            //            output.Append($"    {memberNames[i]}: {l[i]}\n");

            //    MessageBox.Show(output.ToString());
            //}
            //else
            //    MessageBox.Show("App logic is empty");

            #endregion

            if (!CompitabilityMode)
            {
                if (AppLogic.IsEmpty())
                {
                    MessageBox.Show("Отсутствуют данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    // Проверим, можно ли начать показ с данного жюри
                    if (Program.Settings.StartJury < AppLogic.GetJuryChoice().Count)
                    {
                        (new Viewer.ContentViewer(AppLogic)).ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Невозможно начать показ с данного жюри. Проверьте параметр, отвечающий за начального жюри, с которого начнётся расстановка баллов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

            else
            {
                // Проверяем наличие данных
                if (!AppLogic.IsEmpty())
                {
                    UpdateStatus("Начинается показ в режиме совместимости...");
                    // Проверяем совместимость
                    if (LogicConverter.IsCompatible(AppLogic))
                    {
                        // Конвертируем логику
                        Tuple<string[], string[], int[][]> convertedLogic = LogicConverter.DeprecateLogic(AppLogic);

                        // Инициализируем визуализатор
                        (new MainViewer(convertedLogic, Program.Settings.OldSettings)).ShowDialog();
                        UpdateStatus("Показ завершён");
                    }
                    else
                    {
                        MessageBox.Show("Формат данных не совместим со старой версией ResultViewer", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        UpdateStatus("Ошибка во время проверки на совместимость");
                    }
                }
                else
                {
                    MessageBox.Show("Отсутствуют данные", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    UpdateStatus("Отсутствуют данные");
                }

            }
        }

        /// <summary>
        /// Проверяет наличие введённых данных в программы. При их наличии текст кнопки LoadOrEditData будет соответственно изменен
        /// </summary>
        /// <returns>Есть ли введённые данные на данный момент</returns>
        private bool UpdateDataStatus()
        {
            if (AppLogic.IsEmpty())
            {
                LoadOrEditData.Content = "Новые данные";

                return false;
            }
            else
            {
                LoadOrEditData.Content = "Изменить данные";

                // Покажем кнопку быстрого показа
                QuickShow.Visibility = Visibility.Visible;

                if (quickShow != null)
                    quickShow.Update(AppLogic);

                return true;
            }
        }



        /// <summary>
        /// Обновляет значение StatusLabel
        /// </summary>
        /// <param name="s">Новое сообщение</param>
        private void UpdateStatus(string s)
        {
            StatusBar.Content = s;
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем наличие данных
            if (AppLogic.IsEmpty())
            {
                // Уточняем это в статусе
                UpdateStatus("Сохранение невозможно. Отсутствуют данные");

                // Говорим, что нечего сохранять
                MessageBox.Show("Данные отсутсвуют. Сохранять нечего", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            }
            else
            {
                try
                {
                    // Сохраняем данные
                    Program.IO.SaveData(AppLogic);

                    // Обновляем статус
                    UpdateStatus("Сохранение данных завершено");
                }
                catch (Exception ex)
                {
                    // Во время сохранения что-то пошло наперекосяк...
                    UpdateStatus("Critical error!");

                    MessageBox.Show($"Неожиданное исключение: {ex.Message}\nSource:{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем наличие уже введённых данных
            if (!AppLogic.IsEmpty())
            {
                // Данные есть
                MessageBoxResult res = MessageBox.Show("Обнаружены уже введённые данные. При загрузке новых все введённые данные будут утеряны. Вы уверены, что хотите продолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                // Пользователю не плевать на данные
                if (res != MessageBoxResult.Yes)
                {
                    // Обновляем статус
                    UpdateStatus("Отменено");

                    // Выходим из функции
                    return;
                }
                // Иначе удаялем все данные
                else
                    AppLogic.Clear();
            }

            // Пробуем загрузить данные из папки, где запущена программа (.xml формат)
            try
            {
                // Грузим данные
                Program.IO.LoadData(AppLogic);

                // Уведомляем пользователя об успешной загрузке данных
                UpdateStatus("Загрузка данных завершена");

                // Обновляем состояние данных
                UpdateDataStatus();

                // Возвращаем дефолтный FilePath
                Program.IO.RestoreDefaultPath();
            }
            catch (System.IO.FileNotFoundException)
            {
                // Спрашиваем пользователя, не хочет ли он сам выбрать файл
                MessageBoxResult result = MessageBox.Show("В указанном расположении файл не был найден. Желаете выбрать путь самостоятельно?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                try
                {

                    if (result == MessageBoxResult.No)
                    {
                        // Окей, не хочет. Ну и бог с ним
                    }
                    // Пользователь хочет сам выбрать файл
                    else
                    {
                        // Настраиваем диалог выбора файла
                        System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();

                        // Ставим фильтр либо на новый формат (data.xml) либо на старый формат (data.txt)
                        fileDialog.Filter = "Data file(*.xml)|*.xml|Old data file(*.txt)|*.txt";

                        // Не-не-не, не надо нам много файлов
                        fileDialog.Multiselect = false;

                        // Вызываем диалог. Получаем результат диалога
                        System.Windows.Forms.DialogResult res = fileDialog.ShowDialog();

                        // Если пользователь выбрал что-то
                        if (res == System.Windows.Forms.DialogResult.OK)
                        {
                            // Смотрим на расширение файла
                            switch (fileDialog.FileName.Split('.').Last())
                            {
                                // Старый формат
                                case "txt":
                                    // Задаём путь к файлу
                                    Program.IO.FilePath = fileDialog.FileName;

                                    // Грузим данные
                                    Program.IO.LoadOldData(ref AppLogic);
                                    break;

                                // Новый формат
                                case "xml":

                                    // Задаём путь к файлу
                                    Program.IO.FilePath = fileDialog.FileName;

                                    // Грузим данные в отдельное место
                                    Logic newLogic = new Logic();
                                    Program.IO.LoadData(newLogic);

                                    // Если данные удалось загрузить, то чистим старые и заменяем их
                                    if (!newLogic.IsEmpty())
                                    {
                                        AppLogic.Clear();
                                        AppLogic = newLogic;
                                    }
                                    // Если данные не удалось загрузить, то выведем сообщение об ошибке. Старые данные остались нетронутыми
                                    else
                                    {
                                        MessageBox.Show("Не удалось загрузить файл. Возможно файл повреждён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }

                                    break;

                                // Ух ты! Такого не должно быть. FileDialog должен сам фильтровать расширения
                                default:
                                    throw new Exception($"Нераспознанный формат файла: {fileDialog.FileName}");
                            }

                            // Обновляем статус программы
                            UpdateStatus("Загрузка данных завершена");

                            // Обновляем кнопку Создать/Изменить
                            UpdateDataStatus();

                            
                            //// Если присутствуют коллизии, то предупреждаем об этом пользователя
                            //if (AppLogic.PointsCollisionsExists())
                            //    Program.Warnings.ShowLogicCollisionWarning();

                            // Возвращаем дефолтный FilePath
                            Program.IO.RestoreDefaultPath();
                        }
                        else
                        {
                            UpdateStatus("Отменено");
                        }
                    }
                }
                catch (Exception exc)
                {
                    // Выводим сообщение об ошибке
                    UpdateStatus("Critical error!");
                    MessageBox.Show($"Неожиданное исключение:\nMessage: {exc.Message}\nSource: {exc.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    // Возвращаем дефолтный FilePath
                    Program.IO.RestoreDefaultPath();
                }

            }
            catch (Exception ex)
            {
                UpdateStatus("Critical error!");
                MessageBox.Show($"Неожиданное исключение:\nMessage: {ex.Message}\nSource: {ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DevInfo_Click(object sender, RoutedEventArgs e)
        {
            (new DevInfo()).Show();
        }

        private void OpenCustomDir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Настраиваем диалог выбора файла
                System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();

                // Ставим фильтр либо на новый формат (data.xml) либо на старый формат (data.txt)
                fileDialog.Filter = "Data file(*.xml)|*.xml|Old data file(*.txt)|*.txt";

                // Не-не-не, не надо нам много файлов
                fileDialog.Multiselect = false;

                // Вызываем диалог. Получаем результат диалога
                System.Windows.Forms.DialogResult res = fileDialog.ShowDialog();

                // Если пользователь выбрал что-то
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    // Смотрим на расширение файла
                    switch (fileDialog.FileName.Split('.').Last())
                    {
                        // Старый формат
                        case "txt":
                            // Задаём путь к файлу
                            Program.IO.FilePath = fileDialog.FileName;

                            // Грузим данные
                            Program.IO.LoadOldData(ref AppLogic);
                            break;

                        // Новый формат
                        case "xml":

                            // Задаём путь к файлу
                            Program.IO.FilePath = fileDialog.FileName;

                            // Грузим данные в отдельное место
                            Logic newLogic = new Logic();
                            Program.IO.LoadData(newLogic);

                            // Если данные удалось загрузить, то чистим старые и заменяем их
                            if (!newLogic.IsEmpty())
                            {
                                AppLogic.Clear();
                                AppLogic = newLogic;
                            }
                            // Если данные не удалось загрузить, то выведем сообщение об ошибке. Старые данные остались нетронутыми
                            else
                            {
                                MessageBox.Show("Не удалось загрузить файл. Возможно файл повреждён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                            break;

                        // Ух ты! Такого не должно быть. FileDialog должен сам фильтровать расширения
                        default:
                            throw new Exception($"Нераспознанный формат файла: {fileDialog.FileName}");
                    }

                    // Обновляем статус программы
                    UpdateStatus($"Данные загружены из файла {fileDialog.FileName.Split('\\').Last()}");

                    // Обновим состояние данных
                    UpdateDataStatus();

                    // Возвращаем дефолтный FilePath
                    Program.IO.RestoreDefaultPath();
                }
                else
                {
                    UpdateStatus("Отменено");
                    MessageBox.Show("Отменено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Critical error!");
                MessageBox.Show($"Неожиданное исключение:\nMessage: {ex.Message}\nSource: {ex.Source}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик события, которое вызывается по вызову быстрого показа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickShow_Click(object sender, RoutedEventArgs e)
        {
            if (quickShow == null || quickShow.IsClosed)
            {
                quickShow = new QuickResultShow(AppLogic);
                quickShow.MoveWindow(Left + Width, Top + Height);
                QuickShow.Content = "<<";
            }
            if (quickShow.IsVisible)
            {
                quickShow.Hide();
                QuickShow.Content = ">>";
            }
            else
            {
                quickShow.Show();
                QuickShow.Content = "<<";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (quickShow != null)
                quickShow.Close();


            // Завершение всех потоков
            // может появиться исключение, если FastViewer закрыли а не все анимации успели завершиться
            var threads = Process.GetCurrentProcess().Threads;
            foreach (ProcessThread th in threads)
                th.Dispose();

            Application.Current.Shutdown(0);
        }

        /// <summary>
        /// При изменении размеров окна переместим окно quickShow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (quickShow != null)
                quickShow.MoveWindow(Left + Width, Top + Height);
        }

        /// <summary>
        /// При перемещении главного окна также перемести окно quickShow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (quickShow != null)
                quickShow.MoveWindow(Left + Width, Top + Height);
        }

        /// <summary>
        /// Загрузить тестовую логику
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Запустим тестовую логику
            AppLogic = Logic.GetTestLogic();
            UpdateStatus("Загружены тестовые данные");
            UpdateDataStatus();
        }

        private void RemoveAllData_Click(object sender, RoutedEventArgs e)
        {
            // Убиваем логику и все данные, вырубаем быстрый показ
            AppLogic.Clear();
            QuickShow.Visibility = Visibility.Hidden;
            QuickShow.Content = ">>";
            if (quickShow != null)
            {
                quickShow.Close();
                quickShow = null;
            }

            // Обновим состояние данных
            UpdateDataStatus();

            // Сообщим пользователю об успешном удалении данных
            UpdateStatus("Все данные удалены");
        }

        private void SaveCustomDir_Click(object sender, RoutedEventArgs e)
        {
            // Проверим, есть ли данные, которые можно сохарнить
            if (!AppLogic.IsEmpty())
            {
                // Настроим SaveFileDialog
                using (System.Windows.Forms.SaveFileDialog saveDailog = new System.Windows.Forms.SaveFileDialog())
                {
                    // Возможность сохранить только в формате .xml
                    saveDailog.Filter = "Data file(*.xml)|*.xml";

                    // Изначальная директория - местонахождение .exe файла
                    saveDailog.InitialDirectory = Environment.CurrentDirectory;

                    // Если диалог был завершён с результатом OK
                    if (saveDailog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        try
                        {
                            // Сохраним файл
                            Program.IO.FilePath = saveDailog.FileName;
                            Program.IO.SaveData(AppLogic);

                            // Восстановис стандартное имя файла
                            Program.IO.RestoreDefaultPath();

                            // Уведомим пользователя об успешном сохранении данных
                            UpdateStatus($"Данные успешно сохранены в файле: {saveDailog.FileName.Split('\\').Last().ToString()}");
                        }
                        catch (Exception ex)
                        {
                            // Такого вообще не должно быть... Выведем сообщение об ошибке. Вернём всё так, как было
                            MessageBox.Show($"Неожиданное исключение во время сохранения файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            Program.IO.RestoreDefaultPath();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Сохранение прервано пользователем.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
            }
            else
            {
                MessageBox.Show("Сохранение невозможно. Отсутствуют данные.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CompitabilityMode)
            {
                (new MVSettings(Program.Settings.OldSettings)).ShowDialog();
            }
            else
            {
                // Откроем окно с настройками, покажем диалог
                settings = new ViewerSettings(AppLogic);
                settings.ShowDialog();

                // Обнулим ссылку
                settings = null;
            }
        }

        private void SwitchCompitabilityMode_Click(object sender, RoutedEventArgs e)
        {
            CompitabilityMode = !CompitabilityMode;
            if (CompitabilityMode)
                MessageBox.Show("Программа переключена в режим совместимости (классический)", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            else
                MessageBox.Show("Программа переключена в стандартный режим", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ResetPoints_Click(object sender, RoutedEventArgs e)
        {
            if (!AppLogic.IsEmpty())
            {
                Dictionary<string, Tuple<List<double>, List<double>>> members_data = AppLogic.GetJuryChoice();

                // Получим количество участников
                string[] memberNames = AppLogic.GetMemberNames();
                string[] juryNames = AppLogic.GetJuryNames();
                int membersCount = members_data[juryNames[0]].Item1.Count;

                // Пересоздадим списки с баллами, тем самым обнулив их
                members_data.Clear();

                for (int i = 0; i < juryNames.Length; i++)
                    members_data.Add(juryNames[i], new Tuple<List<double>, List<double>>(new List<double>(new double[membersCount]), new List<double>(new double[membersCount])));

                // Обновим состояние данных
                UpdateDataStatus();

                // Обновим баллы
                if (quickShow != null)
                    quickShow.Update(AppLogic);

                // Сообщним пользователю об успешном сбросе баллов
                UpdateStatus("Все баллы и результаты сброшены");
            }
            else
            {
                UpdateStatus("Данные отсутствуют. Нечего удалять");
            }
        }
    }
}