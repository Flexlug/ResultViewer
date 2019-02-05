using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultViewerWPF
{
    /// <summary>
    /// Реализует логику приложения
    /// </summary>
	public class Logic : ICloneable
    {
        /// <summary>
        /// Лист, внутри которого содержатся все имена участников, принимабщих участие в конкурсе
        /// </summary>
		private List<string> members;

        /// <summary>
        /// Коллекция, нутри которой содержатся имена всех жюри и их проставленные баллы
        /// </summary>
		private Dictionary<string, Tuple<List<double>, List<double>>> jury_choice;

        /// <summary>
        /// Реализация интерфейса IClonnable
        /// Данный интерфейс необходим для создания нового объекта по образу и подобию другого с копированием всех данных из него
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Logic()
            {
                members = this.members,
                jury_choice = this.jury_choice
            };
        }

        /// <summary>
        /// Проверяет, есть ли данные внутри данного экземпляра
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            if (members.Count != 0 && jury_choice.Count != 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Сбрасывает все данные в этом экземпляре
        /// </summary>
        public void Clear()
        {
            members = new List<string>();
            jury_choice = new Dictionary<string, Tuple<List<double>, List<double>>>();
        }

        /// <summary>
        /// Добавляет участника конкурса
        /// </summary>
        /// <param name="member_name">Имя участника</param>
		public void AddMember(string member_name)
        {
            members.Add(member_name);
            foreach (Tuple<List<double>, List<double>> list in jury_choice.Values)
            {
                list.Item1.Add(0);
                list.Item2.Add(0.0);
            }
        }

        /// <summary>
        /// Удаляет участника из конкурса
        /// </summary>
        /// <param name="member_index">Порядковый номер участника</param>
		public void RemoveMember(int member_index)
        {
            members.RemoveAt(member_index);
            foreach (Tuple<List<double>, List<double>> list in jury_choice.Values)
            {
                list.Item1.RemoveAt(member_index);
                list.Item2.RemoveAt(member_index);
            }
        }

        /// <summary>
        /// Добавляет жюри
        /// </summary>
        /// <param name="jury_name">Имя жюри</param>
		public void AddJury(string jury_name)
        {
            jury_choice.Add(jury_name, new Tuple<List<double>, List<double>>(new List<double>(new double[members.Count]), new List<double>(new double[members.Count])));
        }

        /// <summary>
        /// Удаляет жюри
        /// </summary>
        /// <param name="jury_index">Порядковый номер жюри</param>
		public void RemoveJury(int jury_index)
        {
            jury_choice.Remove(jury_choice.ElementAt(jury_index).Key);
        }

        /// <summary>
        /// Устанавливает балл, который жюри выдал участнику конкурса
        /// </summary>
        /// <param name="jury_index">Порядковый номер жюри</param>
        /// <param name="member_index">Порядковый номер учатсника</param>
        /// <param name="point">Количество баллов</param>
		public void SetPoint(int jury_index, int member_index, double point)
        {
            jury_choice.ElementAt(jury_index).Value.Item1[member_index] = point;
        }

        /// <summary>
        /// Возвращает балл, присвоенный данным жюри участнику с данным ID
        /// </summary>
        /// <param name="jury_index">ID жюри, выбор которого необходимо узнать</param>
        /// <param name="member_index">ID участника, которому был присвоен балл</param>
        /// <returns></returns>
        public double GetPoint(int jury_index, int member_index)
        {
            return jury_choice.ElementAt(jury_index).Value.Item1[member_index];
        }

        /// <summary>
        /// Устанавливает значение.
        /// </summary>
        /// <param name="jury_index">Порядковый номер жюри</param>
        /// <param name="member_index">Порядковый номер учатсника</param>
        /// <param name="value">Количество баллов</param>
		public void SetValue(int jury_index, int member_index, double value)
        {
            jury_choice.ElementAt(jury_index).Value.Item2[member_index] = value;
        }

        /// <summary>
        /// Возвращает значение.
        /// </summary>
        /// <param name="jury_index">ID жюри, выбор которого необходимо узнать</param>
        /// <param name="member_index">ID участника, которому был присвоен балл</param>
        /// <returns></returns>
        public double GetValue(int jury_index, int member_index)
        {
            return jury_choice.ElementAt(jury_index).Value.Item2[member_index];
        }

        /// <summary>
        /// Возвращет все данные о простановке баллов всеми жюри
        /// </summary>
        /// <returns>Nested array of integers (int[][])</returns>
        public double[][] GetPoints()
        {
            // Инициализируем массив
            double[][] points = new double[jury_choice.Count][];

            // Заполняем массив
            for (int i = 0; i < jury_choice.Count; i++)
                points[i] = jury_choice.ElementAt(i).Value.Item1.ToArray<double>();

            // Возвращаем результат
            return points;
        }

        /// <summary>
        /// Возвращет все значения.
        /// </summary>
        /// <returns>Nested array of doubles (double[][])</returns>
        public double[][] GetValues()
        {
            // Инициализируем массив
            double[][] values = new double[jury_choice.Count][];

            // Заполняем массив
            for (int i = 0; i < jury_choice.Count; i++)
                values[i] = jury_choice.ElementAt(i).Value.Item2.ToArray<double>();

            // Возвращаем результат
            return values;
        }

        /// <summary>
        /// Возвращает List<string>, в котором содержатся все участники конкурса
        /// </summary>
        /// <returns></returns>
        public List<string> GetMembersList()
        {
            return members;
        }

        /// <summary>
        /// Возвращает коллекцию, в которой содержатся имена жюри и баллы, которые они выставили участникам конкурса
        /// </summary>
        /// <returns>Коллекцию Dictionary, в которой KeyValuePair имеют вид: string, List of int></returns>
		public Dictionary<string, Tuple<List<double>, List<double>>> GetJuryChoice()
        {
            return jury_choice;
        }

        /// <summary>
        /// Возвращает имена всех участников конкурса в виде массива из строк
        /// </summary>
        /// <returns>string[]</returns>
        public string[] GetMemberNames()
        {
            return members.ToArray();
        }

        /// <summary>
        /// Возвращает количество участников
        /// </summary>
        /// <returns>int</returns>
        public int GetMemberCount()
        {
            return members.Count;
        }

        /// <summary>
        /// Возвращает имена всех жюри в виде массива из строк
        /// </summary>
        /// <returns>string[]</returns>
		public string[] GetJuryNames()
        {
            return jury_choice.Keys.ToArray();
        }
        
        /// <summary>
        /// Возвращает количество жюри
        /// </summary>
        /// <returns>int</returns>
        public int GetJuryCount()
        {
            return jury_choice.Count;
        }

        public Logic(List<string> _members, Dictionary<string, Tuple<List<double>, List<double>>> _juryChoice)
        {
            members = _members;
            jury_choice = _juryChoice;
        }

        /// <summary>
        /// Инициализирует пустой экземпляр Logic
        /// </summary>
        public Logic()
        {
            members = new List<string>();
            jury_choice = new Dictionary<string, Tuple<List<double>, List<double>>>();
        }

        /// <summary>
        /// Инициализирует Logic из старого формата хранения данных
        /// </summary>
        /// <param name="oldData">Кортеж, в котором содержатся данные в старом формате</param>
        public Logic(Tuple<string[], string[], int[][]> oldData) : this(oldData.Item1, oldData.Item2, oldData.Item3) { }

        /// <summary> 
        /// Инициализирует новый класс Logic из старого формата хранения данных. 
        /// </summary> 
        /// <param name="juryList">массив имён жюри</param> 
        /// <param name="contestList">массив имён участников</param> 
        /// <param name="juryChoice">массивы индексов участников</param> 
        public Logic(string[] juryList, string[] contestList, int[][] juryChoice)
        {
            members = contestList.ToList();
            members.RemoveAt(0);


            jury_choice = new Dictionary<string, Tuple<List<double>, List<double>>>();

            for (int i = 0; i < juryList.Length; i++)
            {
                List<double> choise = new List<double>(new double[members.Count]);
                List<double> values = new List<double>(new double[members.Count]);

                for (int i2 = 0; i2 < 10; i2++)
                {
                    if (juryChoice[i][i2] == 0)
                    {
                        continue;
                    }

                    if (i2 == 9)
                    {
                        choise[juryChoice[i][i2] - 1] = 12;
                    }
                    else if (i2 == 8)
                    {
                        choise[juryChoice[i][i2] - 1] = 10;
                    }
                    else
                    {
                        choise[juryChoice[i][i2] - 1] = i2 + 1;
                    }
                }

                jury_choice.Add(juryList[i], new Tuple<List<double>, List<double>>(choise, values));
            }
        }

        /// <summary>
        /// Получить экземпляр тестовой логики
        /// </summary>
        /// <returns></returns>
        internal static Logic GetTestLogic()
        {
            // Создадим новый экземпляр с тестовыми данными
            Logic testLogic = new Logic();

            // Заполним класс
            Dictionary<string, Tuple<List<double>, List<double>>> testChoice = testLogic.GetJuryChoice();
            List<string> memberTest = testLogic.GetMembersList();

            testChoice.Add("Войнилович Фаддей", Tuple.Create(
                new List<double>()
                {
                    1,
                    2,
                    5,
                    4,
                    0,
                    99,
                    1,
                    22,
                    1,
                    22,
                    1,
                    1,
                },
                new List<double>()
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9,
                    10,
                    1.1,
                    1.2
                }));

            testChoice.Add("Титов Мелхиседек", Tuple.Create(
                new List<double>()
                {
                    3,
                    4,
                    6,
                    6,
                    2,
                    1,
                    0,
                    0,
                    0,
                    1,
                    53,
                    88
                },
                new List<double>()
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9,
                    1,
                    1.1,
                    1.2
                }));

            memberTest.AddRange(
                new List<string>
                {
                "Голохвастов Панфил",
                "Сабуров Эразм",
                "Трусов Флорентий",
                "Тригубович Селиверст",
                "Неверович Корнелий",
                "Чупрасов Капитон",
                "Лиза Тимофеева",
                "Абдул Колесников",
                "Люся Усатова",
                "Екатерина Чернозёмова",
                "Алексей Луговской",
                "Владислав Чернышёв"
                });

            return testLogic;
        }
    }
}