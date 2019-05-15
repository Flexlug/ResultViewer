using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ResultViewerWPF.Viewer.Primitives;

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Позволяет работать с координатами обектов
    /// </summary>
    public class CoordinatesProvider
    {
        /// <summary>
        /// Ширина окна
        /// </summary>
        int width;

        /// <summary>
        /// Высота окна
        /// </summary>
        int height;

        /// <summary>
        /// Максимальное количество участников в одной колонке
        /// </summary>
        int maxColumnCount;

        /// <summary>
        /// Использовать две колонки
        /// </summary>
        public bool TwoColumns;

        /// <summary>
        /// Ссылка на панель балла. Необходимо для точно расчёта центра для неё на экране
        /// </summary>
        PointBar pb;

        /// <summary>
        /// Инициализирует обработчик координат для панелей JuryBar и MemberBar
        /// </summary>
        /// <param name="_width">Ширина окна</param>
        /// <param name="_height">Высота окна</param>
        /// <param name="_maxColumnCount">Максимально еколичество участников в одной колонке</param>
        public CoordinatesProvider(int _width, int _height, PointBar _pb, int _maxColumnCount = 10)
        {
            width = _width;
            height = _height;
            maxColumnCount = _maxColumnCount;
            pb = _pb;

            TwoColumns = Program.Settings.TwoColumns;
        }

        /// <summary>
        /// Получить координаты для панели жюри
        /// </summary>
        /// <returns>Структуру Point с необходимыми координатами</returns>
        public Point Jury()
        {
            return new Point(width / 2,
                             ((int)(Program.Settings.TopJuryInterval + (Program.Settings.JuryPanelHeight / 2))));
        }

        /// <summary>
        /// Получить координаты для панели участника
        /// </summary>
        /// <param name="MemberIndex">Индекс участника в коллекции (или место участника - 1)</param>
        /// <returns>Структуру Point необходимыми координатами</returns>
        public Point Member(int MemberIndex)
        {
            if (TwoColumns)
            {
                // Проверяем, в какой колонке у нас будет расположен участник
                if (MemberIndex < maxColumnCount)
                    // Левая колонка
                    return new Point((width / 2) - (Program.Settings.MemberPanelWidth / 2) - Program.Settings.MemberColumnInterval,
                                     ((int)(Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset + (Program.Settings.MemberPanelHeight + Program.Settings.MemberInterval) * (MemberIndex))));
                else
                    // Правая колонка
                    return new Point((width / 2) + (Program.Settings.MemberPanelWidth / 2) + Program.Settings.MemberColumnInterval,
                                     ((int)(Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset + (Program.Settings.MemberPanelHeight + Program.Settings.MemberInterval) * (MemberIndex - maxColumnCount))));
            }
            else
            {
                return new Point(width / 2,
                                 (int)(Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset + (Program.Settings.MemberPanelHeight + Program.Settings.MemberInterval) * MemberIndex));
            }
        }

        /// <summary>
        /// Получить координаты для панели с баллами
        /// </summary>
        /// <param name="PlaceInd">Индекс участника. Если -1, то перемещает в центр экрана</param>
        /// <returns></returns>
        public Point PointBar(int PlaceInd)
        {
            // Запрашивает координаты центра экрана
            if (PlaceInd == -1)
                // Центр экрана
                return new Point(width / 2 - pb.mainPanel.ActualWidth / 2, height / 2 - pb.mainPanel.ActualHeight / 2);
            else
            if (TwoColumns)
            {
                // Запрашивает координаты какого-то участника
                if (PlaceInd < maxColumnCount)
                    // Левая колонка
                    return new Point((width / 2) - Program.Settings.MemberPanelWidth - Program.Settings.MemberColumnInterval/* + (ProgramSettings.MemberPanelWidth / 2)*/,
                                 (int)(Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - Program.Settings.MemberPanelHeight / 2 + Program.Settings.MemberInterval + (Program.Settings.MemberPanelHeight + (Program.Settings.MemberInterval + 1.4)) * PlaceInd));
                else
                    // Правая колонка
                    return new Point((width / 2) + Program.Settings.MemberColumnInterval + (Program.Settings.MemberPointsPanelWidth / 4),
                                 (int)(Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - Program.Settings.MemberPanelHeight / 2 + Program.Settings.MemberInterval + (Program.Settings.MemberPanelHeight + (Program.Settings.MemberInterval * 1.4)) * (PlaceInd - maxColumnCount)));
            }
            else
                return new Point((width / 2) - Program.Settings.MemberPanelWidth / 2,
                                 (int)(Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - Program.Settings.MemberPanelHeight / 2 + Program.Settings.MemberInterval + (Program.Settings.MemberPanelHeight + (Program.Settings.MemberInterval + 1.4)) * PlaceInd));
        }

        public Point LowerFrase(double actualWidth, double actualHeight)
        {
            return new Point(width / 2 - actualWidth / 2, height / 2 + actualHeight / 2 + Program.Settings.LowerPhraseOffset);
        }
    }
}
