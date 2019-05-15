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
    /// Логика взаимодействия для EditColorRange.xaml
    /// </summary>
    public partial class EditColorRange : Window
    {
        /// <summary>
        /// Название цветового промежутка
        /// </summary>
        public string RangeName
        {
            get
            {
                return ColorRangeName.Text;
            }
            set
            {
                ColorRangeName.Text = value;
            }
        }

        /// <summary>
        /// Количество мест, которое может быть в цветовом промежутке
        /// </summary>
        public int RangeCount
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RangeCountEdit.Text))
                {
                    if (int.TryParse(RangeCountEdit.Text, out int result))
                        return result;
                }

                throw new InvalidCastException("Попытка преобразования пустого поля в TextBox-е RangeCout");
            }
            set
            {
                RangeCountEdit.Text = value.ToString();
            }
        }

        /// <summary>
        /// Цвет цветового промежутка
        /// </summary>
        public Color RangeColor
        {
            get
            {
                return (RangeColorPreview.Background as SolidColorBrush).Color;
            }
            set
            {
                RangeColorPreview.Background = new SolidColorBrush(value);
            }
        }

        /// <summary>
        /// Инициализация с использованием уже существующего экземпляра ColorRange. Подходит для редактирования уже существующего экземпляра.
        /// </summary>
        /// <param name="range">Существующий экземпляр ColorRange</param>
        public EditColorRange(ColorRange range)
        {
            InitializeComponent();

            RangeName = range.Name;
            RangeCount = range.Count;
            RangeColor = range.CurrentColor;
        }

        /// <summary>
        /// Конструктор для создания нового экземпляра ColorRange
        /// </summary>
        /// <param name="count">Количество существующих RangeColor (для того, чтобы избежать коллизий при выдаче новых имён)</param>
        public EditColorRange(int count)
        {
            InitializeComponent();

            RangeName = $"Конфигурация{count + 1}";
            RangeCount = 1;
            RangeColor = Colors.Aqua;
        }

        private void ChangeRangeColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();

            // Из-за разницы классов System.Drawing.Color и System.Windows.Media.Color приходится заниматься вот такой вот фигнёй... а именно такими странными преобразованиями
            Color currentColor = RangeColor;
            cd.Color = System.Drawing.Color.FromArgb(currentColor.A, currentColor.R, currentColor.G, currentColor.B);
            
            // Если нажали ок, то меняем цвет
            // Если не ок, то оставляем всё, как есть
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RangeColor = Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверим, нет ли пустых полей
            if (!string.IsNullOrWhiteSpace(ColorRangeName.Text) && !string.IsNullOrWhiteSpace(RangeCountEdit.Text))
            {
                // Если нет, то указываем, что с этой формы можно брать значения
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Заполните пустые поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
