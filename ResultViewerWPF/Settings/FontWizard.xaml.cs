using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ResultViewerWPF.Settings
{
    /// <summary>
    /// Логика взаимодействия для FontWizard.xaml
    /// </summary>
    public partial class FontWizard : Window
    {
        /// <summary>
        /// Цвет шрифта
        /// </summary>
        public System.Drawing.Color currentColor;

        /// <summary>
        /// Тип шрифта
        /// </summary>
        public FontWeight currentFontWeight;

        /// <summary>
        /// Размер шрифта
        /// </summary>
        public int currentFontSize;
        
        public FontWizard(System.Windows.Media.Brush _fontColor, FontWeight _fontWeight, int _fontSize)
        {
            // Прогрузим цвет
            System.Windows.Media.Color tempColor = (_fontColor as SolidColorBrush).Color;
            currentColor = System.Drawing.Color.FromArgb(tempColor.A, tempColor.R, tempColor.G, tempColor.B);

            // Прогрузим FontWeight и FontSize
            currentFontWeight = _fontWeight;
            currentFontSize = _fontSize;
        }

        private void OnlyNumbersInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
                e.Handled = true;
        }

        private void ChangeFontColor(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
