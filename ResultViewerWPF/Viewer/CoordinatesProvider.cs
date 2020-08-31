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
        /// Флаг, указывающий, отображается ли сейчас колонка с результатами конкурсантов или нет
        /// </summary>
        public bool ResultColumnVisible;

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
            ResultColumnVisible = Program.Settings.ResultColumnPhraseShowMode == Program.Settings.PhraseShowMode.Always;
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

        /// <summary>
        /// Возвращает координаты для названия колонки с баллами
        /// </summary>
        /// <param name="actualPhraseWidth">Ширина надписи</param>
        /// <param name="actualPhraseHeight">Высота надписи</param>
        /// <returns></returns>
        public Point PointsColumnPhrase(double actualPhraseWidth, double actualPhraseHeight)        
        {
            if (TwoColumns)
            {
                return new Point((width / 2) - Program.Settings.MemberColumnInterval / 2 - Program.Settings.MemberPointsPanelWidth - Program.Settings.MemberPanelWidth + Program.Settings.PointsColumnPhraseXOffset,
                                 Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - actualPhraseHeight + Program.Settings.PointsColumnPhraseYOffset);
            }
            else
            {
                return new Point((width / 2) - (Program.Settings.MemberPanelWidth + Program.Settings.MemberPointsPanelWidth) / 2 + actualPhraseWidth / 2 + Program.Settings.PointsColumnPhraseXOffset,
                                 Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - actualPhraseHeight + Program.Settings.PointsColumnPhraseYOffset);
            }

            throw new ArgumentException($"Invalid arguments. Can't return a value. actualWidth: {actualPhraseWidth}, actualHeight: {actualPhraseHeight}");
        }

        /// <summary>
        /// Возвращает координаты для названия колонки с результатами
        /// </summary>
        /// <param name="actualPhraseWidth">Ширина надписи</param>
        /// <param name="actualPhraseHeight">Высота надписи</param>
        /// <returns></returns>
        public Point ResultColumnPhrase(double actualPhraseWidth, double actualPhraseHeight)
        {
            if (TwoColumns)
            {
                return new Point((width / 2) - Program.Settings.MemberColumnInterval / 2 - Program.Settings.MemberPointsPanelWidth - Program.Settings.MemberPanelWidth - Program.Settings.MemberResultPanelWidth + Program.Settings.ResultColumnPhraseXOffset,
                                 Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - actualPhraseHeight + Program.Settings.ResultColumnPhraseYOffset);
            }
            else
            {
                return new Point((width / 2) - (Program.Settings.MemberPanelWidth + Program.Settings.MemberPointsPanelWidth - Program.Settings.MemberResultPanelWidth) / 2 + actualPhraseWidth / 2 + Program.Settings.ResultColumnPhraseXOffset,
                                 Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - actualPhraseHeight + Program.Settings.ResultColumnPhraseYOffset);
            }

            throw new ArgumentException($"Invalid arguments. Can't return a value. actualWidth: {actualPhraseWidth}, actualHeight: {actualPhraseHeight}");
        }

        /// <summary> 
        /// Возвращает координаты для названия колонки с местом в топе
        /// </summary>
        /// <param name="actualPhraseWidth">Ширина надписи</param>
        /// <param name="actualPhraseHeight">Высота надписи</param>
        /// <param name="ResultIsVisible">Отображаются ли на данный момент панели с результатами участников</param>
        /// <returns></returns>
        public Point PlaceColumnPhrase(double actualPhraseWidth, double actualPhraseHeight, bool ResultIsVisible)
        {
            if (TwoColumns)
            {
                return new Point((width / 2) - Program.Settings.MemberColumnInterval / 2 - Program.Settings.MemberPointsPanelWidth - Program.Settings.MemberPanelWidth - Program.Settings.MemberResultPanelWidth - 
                    Program.Settings.MemberPlaceStrokeWidth + Program.Settings.PlaceColumnPhraseXOffset - (ResultIsVisible ? Program.Settings.MemberResultPanelWidth : 0),
                                 Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - actualPhraseHeight + Program.Settings.PlaceColumnPhraseYOffset);
            }
            else
            {
                return new Point((width / 2) - (Program.Settings.MemberPanelWidth + Program.Settings.MemberPointsPanelWidth + Program.Settings.MemberResultPanelWidth + Program.Settings.MemberPlaceStrokeWidth) / 2 + actualPhraseWidth / 2 + Program.Settings.PlaceColumnPhraseXOffset - (ResultIsVisible ? Program.Settings.MemberResultPanelWidth : 0),
                                 Program.Settings.TopJuryInterval + Program.Settings.JuryPanelHeight + Program.Settings.JuryMemberOffset - actualPhraseHeight + Program.Settings.PlaceColumnPhraseYOffset);
            }

            throw new ArgumentException($"Invalid arguments. Can't return a value. actualWidth: {actualPhraseWidth}, actualHeight: {actualPhraseHeight}");
        }

        /// <summary>
        /// Возвращает координаты для названия колонки с местом в топе
        /// </summary>
        /// <param name="actualPhraseWidth">Ширина надписи</param>
        /// <param name="actualPhraseHeight">Высота надписи</param>
        /// <returns></returns>
        public Point PlaceColumnPhrase(double actualPhraseWidth, double actualPhraseHeight)
        {
            return PlaceColumnPhrase(actualPhraseWidth, actualPhraseHeight, ResultColumnVisible);
        }

        /// <summary>
        /// Возвращает координаты для нижней фразы
        /// </summary>
        /// <param name="actualPhraseWidth">Ширина нижней фразы (необходима для центрирования)</param>
        /// <param name="actualPhraseHeight">Высота нижней фразы (необходима для центрирования)</param>
        /// <returns></returns>
        public Point LowerFrase(double actualPhraseWidth, double actualPhraseHeight)
        {
            return new Point(width / 2 - actualPhraseWidth / 2, height / 2 + actualPhraseHeight / 2 + Program.Settings.LowerPhraseOffset);
        }
    }
}
