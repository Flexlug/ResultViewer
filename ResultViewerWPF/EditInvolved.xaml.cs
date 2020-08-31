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
    /// Логика взаимодействия для EditInvolved.xaml
    /// </summary>
    public partial class EditInvolved : Window
    {
        Logic appLogic;

        /// <summary>
        /// Стандартный конструктор. Все данные будут созданы заново
        /// </summary>
        public EditInvolved()
        {
            InitializeComponent();

            // Создаём новый экземпляр, так как вводим новые данные
            appLogic = new Logic();

            // Проверяем поля для ввода
            CheckFields();
        }

        /// <summary>
        /// Расширенный конструктор. Данные будут созданы на основе старых
        /// </summary>
        /// <param name="logic">Ссылка на экземпляр Logic, где располагаются изменяемые данные</param>
        public EditInvolved(Logic logic)
        {            
            InitializeComponent();

            // Копируем данные в эту форму
            appLogic = logic.Clone() as Logic;

            // Заполним все поля в соответствии с уже полученными старыми данными
            LoadJuryData();
            LoadMemberData();

            // Проверяем поля для ввода
            CheckFields();
        }
        
        private void LoadJuryData()
        {
            // Загрузим все уже введённые имена
            string[] juryNames = appLogic.GetJuryNames();

            // Заполним JuryList
            foreach (string jn in juryNames)
            {
                // Вводим в формате: nn: JuryName, пронумеровывая каждого, используя значащие нули
                JuryList.Items.Add($"{String.Format("{0:00}", JuryList.Items.Count + 1)}: {jn}");
            }
        }

        private void LoadMemberData()
        {
            // Загрузим все уже введённые имена
            string[] memberNames = appLogic.GetMemberNames();

            // Заполним MemberList
            foreach (string mn in memberNames)
            {
                // Вводим в формате: nn: MemberName, пронумеровывая каждого, используя значащие нули
                MemberList.Items.Add($"{String.Format("{0:00}", MemberList.Items.Count + 1)}: {mn}");
            }
        }

        /// <summary>
        /// Проверяет, может ли пользователь прямо сейчас добавить жюри или участника конкурса
        /// </summary>
        private void CheckFields()
        {
            // Если поле для нового имени жюри пустое, то кнопку для добавления жюри делаем недоступной
            // Иначе кнопку делаем доступной
            if (NewJuryName.Text == String.Empty)
                AddJury.IsEnabled = false;
            else
                AddJury.IsEnabled = true;

            // Если поле для нового имени участника пустое, то кнопку для добавления участника делаем недоступной
            // Иначе кнопку делаем доступной
            if (NewMemberName.Text == String.Empty)
                AddMember.IsEnabled = false;
            else
                AddMember.IsEnabled = true;

            // Если еще нет добавленных жюри, то кнопку для удаления жюри делаем недоступной
            // Иначе кнопку делаем доступной
            if (JuryList.Items.Count == 0)
                RemoveJury.IsEnabled = false;
            else
                RemoveJury.IsEnabled = true;

            // Если еще нет добавленных участников, то кнопку для удаления участников делаем недоступной
            // Иначе кнопка делаем доступной
            if (MemberList.Items.Count == 0)
                RemoveMember.IsEnabled = false;
            else
                RemoveMember.IsEnabled = true;

            // Выводим количество введённых участнков
            MemberCount.Content = $"Количество учатников: {appLogic.GetMembersList().Count}";

            // Выводим количество введённых жюри
            JuryCount.Content = $"Количество жюри: {appLogic.GetJuryChoice().Count}";
        }

        private void AddJury_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем на коллизии
            if (!appLogic.GetJuryNames().Contains(NewJuryName.Text))
            {
                // Добавляем нового участника. Берём его имя из TextBox
                appLogic.AddJury(NewJuryName.Text);

                // Добавляем его имя в JuryList
                // Выводим его в формате nn: JuryName с добавлением значащих нулей
                JuryList.Items.Add($"{String.Format("{0:00}", JuryList.Items.Count + 1)}: {NewJuryName.Text}");

                // Обнуляем TextBox
                NewJuryName.Text = String.Empty;

                // Перемещаем фокус на TextBox
                NewJuryName.Focus();

                // Проверяем поля
                CheckFields();
            }
            else
            {
                // Уведомляем пользователя о том, что одинаковые имена недопустимы
                MessageBox.Show("Одинаковые имена недопустимы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveJury_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли какой-нибудь элемент в JuryList. Если никто не выбран, ничего не выполнять
            if (JuryList.SelectedItem != null)
            {
                // Удаляем жюри из списка
                appLogic.RemoveJury(JuryList.SelectedIndex);

                // Обновим все индексы в JuryList
                JuryList.Items.Clear();
                LoadJuryData();
            }
        }

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем на коллизии
            if (!appLogic.GetMemberNames().Contains(NewMemberName.Text))
            {
                // Добавляем нового участника. Берём его имя из TextBox
                appLogic.AddMember(NewMemberName.Text);

                // Добавляем его имя в JuryList
                // Выводим его в формате nn: JuryName с добавлением значащих нулей
                MemberList.Items.Add($"{String.Format("{0:00}", MemberList.Items.Count + 1)}: {NewMemberName.Text}");

                // Обнуляем TextBox
                NewMemberName.Text = String.Empty;

                // Перемещаем фокус на TextBox
                NewMemberName.Focus();

                //  Проверяем поля
                CheckFields();
            }
            else
            {
                // Уведомляем пользователя о том, что одинаковые имена недопустимы
                MessageBox.Show("Одинаковые имена недопустимы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveMember_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли какой-нибудь элемент в JuryList. Если никто не выбран, ничего не выполнять
            if (MemberList.SelectedItem != null)
            {
                // Удаляем участника из списка
                appLogic.RemoveMember(MemberList.SelectedIndex);

                // Обновим все индексы в MemberList
                MemberList.Items.Clear();
                LoadMemberData();
            }
        }

        private void NewJuryName_KeyDown(object sender, KeyEventArgs e)
        {
            // Если была нажата клавиша Enter, то переместить фокус на кнопку для добавления 
            if (e.Key == Key.Enter)
                AddJury.Focus();
        }

        private void NewMemberName_KeyDown(object sender, KeyEventArgs e)
        {
            // Если была нажата клавиша Enter, то переместить фокус на кнопку для добавления 
            if (e.Key == Key.Enter)
                AddMember.Focus();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            // Закрыть окно. Забыть про всё, что было введено раннее
            Close();
        }

        private void SetPoints_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем на пустые поля
            if (appLogic.IsEmpty())
            {
                // Если таковые имеются, не даём продолжить редактирование
                MessageBox.Show("Оставлены пустые поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Скрыть окно
                Hide();

                // Начать редактирование баллов
                // Передаём ссылку на главное окно
                (new EditPoints(appLogic)
                {
                    Owner = this.Owner
                }).ShowDialog();

                // Как редактирование баллов завершится, закрыть окно
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Освобождаем списки
            MemberList.Items.Clear();
            JuryList.Items.Clear();
        }

        private void NewJuryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Отслеживаем изменения текста
            CheckFields();
        }

        private void NewMemberName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Отслеживаем изменения текста
            CheckFields();
        }
    }
}
