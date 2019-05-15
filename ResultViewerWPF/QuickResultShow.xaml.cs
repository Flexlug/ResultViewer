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
    /// Класс, который предоставляет возможность добавить строчку в DataGrid
    /// </summary>
    public class TableRow
    {
        /// <summary>
        /// Место участника в общем топе
        /// </summary>
        public int Place { get; set; }

        /// <summary>
        /// ФИО участника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество баллов у участника
        /// </summary>
        public double Points { get; set; }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="name">Имя участника</param>
        public TableRow(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Логика взаимодействия для QuickResultShow.xaml
    /// </summary>
    public partial class QuickResultShow : Window
    {
        /// <summary>
        /// Список с просчитанными баллами
        /// </summary>
        List<TableRow> table = null;

        /// <summary>
        /// Ссылка на объект логики
        /// </summary>
        Logic appLogic;

        public QuickResultShow(Logic appLogic)
        {
            InitializeComponent();
            Update(appLogic);
        }

        private void QuckResultViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public bool IsClosed = false;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }

        public void Update(Logic appLogic)
        {
            // Обновим ссылку
            this.appLogic = appLogic;

            // Очистим данные
            QuickResultViewer.Items.Clear();

            // Таблица, которая потом станет источником данных для нашего DataGrid-а
            table = new List<TableRow>();

            // Получим количество участников и жюри для того, чтобы знать количество итераций для цикла
            int memberCount = appLogic.GetMemberNames().Length;
            int juryCount = appLogic.GetJuryNames().Length;

            // Заполним заранее коллекцию именами участников
            foreach (string name in appLogic.GetMemberNames())
                table.Add(new TableRow(name));

            // Заполним баллы
            double point = 0;
            for (int jury = 0; jury < juryCount; jury++)
                for (int member = 0; member < memberCount; member++)
                    if ((point = appLogic.GetPoint(jury, member)) > 0)
                    table[member].Points += point;

            // Сортировка участников
            if (Program.Settings.MemberSortingMode == Program.Settings.SortingMode.Ascending)
                table = table.OrderByDescending(x => x.Points).ToList();
            else
                table = table.OrderBy(x => x.Points).ToList();

            // Расстановка мест
            if (Program.Settings.TrueTopRating)
            {
                for (int i = 0; i < table.Count; i++)
                    if (i == 0)
                    {
                        table[i].Place = i + 1;
                    }
                    else
                    {
                        if (table[i - 1].Points == table[i].Points)
                            table[i].Place = table[i - 1].Place;
                        else
                            table[i].Place = i + 1;
                    }
            }
            else
            {
                for (int i = 0; i < table.Count; i++)
                    table[i].Place = i + 1;
            }

            // Выведем результаты в таблицу
            foreach (TableRow row in table)
                QuickResultViewer.Items.Add(row);

            QuickResultViewer.CanUserAddRows = false;
            QuickResultViewer.CanUserDeleteRows = false;

        }

        /// <summary>
        /// Передвигает окно
        /// </summary>
        /// <param name="_left">Расстояние окна от левого края экрана</param>
        /// <param name="_top">Расстояние окна от правого края экрана</param>
        public void MoveWindow(double _left, double _top)
        {
            Left = _left;
            Top = _top - Height;
        }

        private void OpenFullSize_Click(object sender, RoutedEventArgs e)
        {
            (new Viewer.FastViewer(appLogic)).Show();
        }
    }
}
