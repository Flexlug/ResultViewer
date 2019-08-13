using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ResultViewerWPF.Program
{
    static class Warnings
    {
        /// <summary>
        /// Выводит предупреждающее сообщение, в котором написано предупреждение, уведомляющее о наличии коллизий в загруженных данных, т.е. когда несколько участников разделяют одно место из-за одинакового количества баллов
        /// </summary>
        public static void ShowLogicCollisionWarning()
        {
            MessageBox.Show("Невозможно использовать заданную конфигурацию выделения участников, т.к. присутствуют коллизии (несколько участников на одном месте в топе). Будут подсвечиваться только первые три места", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
