using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultViewerWPF.Compitability
{
    public static class LogicConverter
    {
        /// <summary>
        /// Проверяет экземпляр Logic на возможность конвертации в старый формат
        /// </summary>
        /// <param name="logic">Ссылка на экземпляр Logic</param>
        /// <returns></returns>
        public static bool IsCompatible(Logic logic)
        {
            // Проверка на наличие данных
            if (logic.IsEmpty())
                return false;

            // Проверка на наличие дробных баллов
            // По мере проверки получим уже преобразованные баллы в целочисленном виде
            double[][] doubleJuryChoice = logic.GetPoints();

            int[][] juryChoice = new int[logic.GetJuryCount()][];
            for (int i = 0; i < juryChoice.Length; i++)
                juryChoice[i] = new int[logic.GetMemberCount()];

            for (int jur = 0; jur < doubleJuryChoice.Length; jur++)
                for (int comp = 0; comp < doubleJuryChoice[0].Length; comp++)
                    if (Math.Truncate(doubleJuryChoice[jur][comp]) == 0)
                    {
                        juryChoice[jur][comp] = (int)doubleJuryChoice[jur][comp];
                    }
                    else
                    {
                        return false;
                    }
                    

            // Проверка на дубликаты
            for (int jur = 0; jur < juryChoice.Length; jur++)
                for (int comp1 = 0; comp1 < juryChoice[0].Length; comp1++)
                    for (int comp2 = 0; comp2 < juryChoice[0].Length; comp2++)
                    {
                        // Проходимся по всем баллам и убеждаемся, что дубликаты отсутствуют
                        if (juryChoice[jur][comp1] == juryChoice[jur][comp2] && comp1 != comp2 && juryChoice[jur][comp1] != 0)
                            return false;
                    }

            // Проверяем на наличие недопустимых баллов
            for (int jur = 0; jur < juryChoice.Length; jur++)
                for (int comp = 0; comp < juryChoice[0].Length; comp++)
                    if (juryChoice[jur][comp] == 9 ||
                        juryChoice[jur][comp] == 11 ||
                        juryChoice[jur][comp] > 13 ||
                        juryChoice[jur][comp] < 0)
                    {
                        return false;
                    }

            // Если логика прошла все эти пытки успешно, можно считать, что она достойна стать заменой своему предку =)
            return true;
        }

        /// <summary>
        /// Конвертирует экземпляр Logic в старый формат. Перед конвератцией обязательно убетитесь в возможности конвертации через IsCompitable воизбежание ошибок
        /// </summary>
        /// <returns>Logic в старом формате</returns>
        public static Tuple<string[], string[], int[][]> DeprecateLogic(Logic logic)
        {
            // Копируем список участников
            List<string> members = new List<string>(logic.GetMembersList());

            // Получаем кллекцию, где представлен выбор жюри
            Dictionary<string, Tuple<List<double>, List<double>>> juryСhoice = logic.GetJuryChoice();

            // Проводим конвертацию
            int[][] juryChoiceConverted = new int[juryСhoice.Count][];

            for (int i = 0; i < juryСhoice.Count; i++)
            {
                juryChoiceConverted[i] = new int[10];
                for (int i2 = 0; i2 < members.Count; i2++)
                {
                    if (juryСhoice.ElementAt(i).Value.Item1[i2] == 12)
                    {
                        juryChoiceConverted[i][9] = i2 + 1;
                    }
                    else if (juryСhoice.ElementAt(i).Value.Item1[i2] == 10)
                    {
                        juryChoiceConverted[i][8] = i2 + 1;
                    }
                    else if (juryСhoice.ElementAt(i).Value.Item1[i2] != 0)
                    {
                        juryChoiceConverted[i][(int)juryСhoice.ElementAt(i).Value.Item1[i2] - 1] = i2 + 1;
                    }
                }
            }

            // Нулевой участник добавляется здесь 
            members.Insert(0, "...");

            return Tuple.Create(juryСhoice.Keys.ToArray(),
                                members.ToArray(),
                                juryChoiceConverted);
        }
    }
}
