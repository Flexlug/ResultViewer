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

namespace ResultViewerWPF.Viewer.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для NominationsWizard.xaml
    /// </summary>
    public partial class NominationsWizard : Window
    {
        public NominationsWizard()
        {
            InitializeComponent();
        }

        private void ColorRangeView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRangeView();
        }

        /// <summary>
        /// Заново загрузить данные для RangeView
        /// </summary>
        private void UpdateRangeView()
        {
            // Предварительно очистим поля
            ColorRangeView.ItemsSource = null;
            ColorRangeView.Items.Clear();

            // Создадим промежуточную коллекцию, благодаря которой обеспечивается поддержка предварительного отображения цвета и поочерёдной нумерации
            List<Tuple<int, string, int, SolidColorBrush>> transformedColorRange = new List<Tuple<int, string, int, SolidColorBrush>>();

            for (int i = 0; i < Program.Settings.ColorRangeList.Count; i++)
            {
                ColorRange currentRange = Program.Settings.ColorRangeList.ElementAt(i);
                transformedColorRange.Add(Tuple.Create(i + 1,
                                                       currentRange.Name,
                                                       currentRange.Count,
                                                       new SolidColorBrush(currentRange.CurrentColor)));
            }

            ColorRangeView.ItemsSource = transformedColorRange;
        }

        private void AddColorRange_Click(object sender, RoutedEventArgs e)
        {
            // Запустим диалог
            EditColorRange rangeData = new EditColorRange(Program.Settings.ColorRangeList.Count);

            // Если диалог успешно завершён и есть данные, которые позволяют создать новый экземпляр ColorRange...
            if (rangeData.ShowDialog() == true)
            {
                //... то создаём новый ColorRange
                ColorRange newColorRange = new ColorRange(rangeData.RangeName, rangeData.RangeCount, rangeData.RangeColor);

                // Добавляем его в конец
                Program.Settings.ColorRangeList.AddLast(newColorRange);

                // Обновим список
                UpdateRangeView();
            }
        }

        private void EditColorRange_Click(object sender, RoutedEventArgs e)
        {
            int currentItem = 0;

            if (ColorRangeView.SelectedValue != null)
            {
                // Получим индекс объекта, который сейчас будет редактироваться (из номера объекта)
                // Вычтем 1, чтобы получить индекс элемента
                currentItem = (ColorRangeView.SelectedValue as Tuple<int, string, int, SolidColorBrush>).Item1 - 1;
            }
            else
            {
                MessageBox.Show("Сначала выберете объект для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Запустим диалог
            EditColorRange rangeData = new EditColorRange(Program.Settings.ColorRangeList.ElementAt(currentItem));

            // Если диалог успешно завершён и есть данные, которые позволяют редактировать экземпляр ColorRange...
            if (rangeData.ShowDialog() == true)
            {
                // Найдём элемент, который нужно отредактировать
                ColorRange editingRange = Program.Settings.ColorRangeList.ElementAt(currentItem);

                // Обновляем его поля
                editingRange.Name = rangeData.RangeName;
                editingRange.Count = rangeData.RangeCount;
                editingRange.CurrentColor = rangeData.RangeColor;

                // Обновим список
                UpdateRangeView();
            }
        }

        private void RemoveColorRange_Click(object sender, RoutedEventArgs e)
        {
            int currentItem = 0;

            if (ColorRangeView.SelectedValue != null)
            {
                // Получим индекс объекта, который сейчас будет редактироваться
                currentItem = (ColorRangeView.SelectedValue as Tuple<int, string, int, SolidColorBrush>).Item1 - 1;

                Program.Settings.ColorRangeList.Remove(Program.Settings.ColorRangeList.ElementAt(currentItem));

                // Обновим список
                UpdateRangeView();
            }
            else
            {
                MessageBox.Show("Сначала выберете объект для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SetDefault_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OKButton_Click(object sedner, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
