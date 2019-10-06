// Copyright 2019 Flexlug

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

// See the License for the specific language governing permissions and
// limitations under the License.

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
