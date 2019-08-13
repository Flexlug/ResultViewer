﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ResultViewerWPF.Viewer;

namespace ResultViewerWPF.Program
{
    /// <summary>
    /// Предоставляет доступ к настройкам программы
    /// </summary>
    public static class Settings
    {
        #region Совместимость

        /// <summary>
        /// Настройки для старого режима показа
        /// </summary>
        public static OldSettingsProvider OldSettings = new OldSettingsProvider();

        #endregion

        #region Панель участника
        /// <summary>
        /// Ширина панели участника
        /// </summary>
        public static double MemberPanelWidth = 405;

        /// <summary>
        /// Высота панели участника
        /// </summary>
        public static double MemberPanelHeight = 45;

        /// <summary>
        /// Размер шрифта для панели участника
        /// </summary>
        public static double MemberNameFontSize = 20;

        /// <summary>
        /// Итоговая прозрачность панели участника после проявления
        /// </summary>
        public static double MemberPanelOpacity = 1;

        /// <summary>
        /// Ширина контура панели участников
        /// </summary>
        public static double MemberPanelStrokeWidth = 1;
        
        /// <summary>
        /// Цвет панели участника
        /// </summary>
        public static Color MemberPanelColor = new Color()
        {
            A = 100,
            R = 200,
            G = 200,
            B = 200
        };

        /// <summary>
        /// Цвет панели выделенного участника
        /// </summary>
        public static Color MemberPanelChosenColor = new Color()
        {
            A = 255,
            R = 255,
            G = 100,
            B = 100
        };

        /// <summary>
        /// Цвет панели после выделения
        /// </summary>
        public static Color MemberPanelChosenColor2 = new Color()
        {
            A = 255,
            R = 100,
            G = 255,
            B = 100
        };

        /// <summary>
        /// Цвет контура панели участника
        /// </summary>
        public static Color MemberPanelStrokeColor = new Color()
        {
            A = 255,
            R = 255,
            G = 255,
            B = 255
        };

        /// <summary>
        /// Включает или отключает выделение участников отдельными цветами в финальном показе
        /// </summary>
        public static bool MemberPanelHighlightLeaders = true;


        /// <summary>
        /// Цвет панели участника, находящейся на первом месте
        /// </summary>
        public static Color MemberPanelFirstPlace = new Color()
        {
            A = 200,
            R = 255,
            G = 218,
            B = 6
        };

        /// <summary>
        /// Цвет панели участника, находящегося на втором месте
        /// </summary>
        public static Color MemberPanelSecondPlace = new Color()
        {
            A = 200,
            R = 198,
            G = 198,
            B = 198
        };

        /// <summary>
        /// Цвет панели участника, находящегося на третьем месте
        /// </summary>
        public static Color MemberPanelThirdPlace = new Color()
        {
            A = 200,
            R = 170,
            G = 98,
            B = 0
        };

        /// <summary>
        /// Цвет панели участника, находящегося не в топ-3
        /// </summary>
        public static Color MemberPanelOtherPlaces = new Color()
        {
            A = 100,
            R = 50,
            G = 50,
            B = 50
        };

        /// <summary>
        /// Показывает, необходимо ли использовать еще один цвет для отдельного выделения участнков, которым уже был выдан балл
        /// </summary>
        public static bool MemberPanelUseSecondChooseColor = true;

        /// <summary>
        /// Стиль шрифта для имени участника
        /// </summary>
        public static FontWeight MemberNameFontWeight = FontWeights.Thin;

        /// <summary>
        /// Цвет имени участника
        /// </summary>
        public static SolidColorBrush MemberNameFontColor = Brushes.Black;

        /// <summary>
        /// Стиль шрифта для баллов участника
        /// </summary>
        public static FontWeight MemberPointsFontWeight = FontWeights.Thin;

        /// <summary>
        /// Размер шрифта для баллов участника
        /// </summary>
        public static double MemberPointsFontSize = 22;

        /// <summary>
        /// Высота панели с баллами участника
        /// </summary>
        public static double MemberPointsPanelHeight = 45;

        /// <summary>
        /// Ширина панели с баллами участника
        /// </summary>
        public static double MemberPointsPanelWidth = 45;

        /// <summary>
        /// Цвет панели с баллами
        /// </summary>
        public static Color MemberPointsPanelColor = new Color()
        {
            A = 100,
            R = 200,
            G = 200,
            B = 200
        };

        /// <summary>
        /// Цвет контура вокруг баллов
        /// </summary>
        public static Color MemberPointsStrokeColor = new Color()
        {
            A = 200,
            R = 200,
            G = 200,
            B = 200
        };

        /// <summary>
        /// Цвет количества баллов участника
        /// </summary>
        public static SolidColorBrush MemberPointsFontColor = Brushes.Black;

        /// <summary>
        /// Ширина контура вокруг баллов
        /// </summary>
        public static double MemberPointsStrokeWidth = 2;

        /// <summary>
        /// Номинации, которые предусмотрены в данном мероприятии. По умолчанию три места с цветами золота, серебра и бронзы соответственно
        /// </summary>
        public static LinkedList<ColorRange> ColorRangeList = new LinkedList<ColorRange>(new ColorRange[] {
            new ColorRange("Первое место", 1, MemberPanelFirstPlace),
            new ColorRange("Второе место", 1, MemberPanelSecondPlace),
            new ColorRange("Третье место", 1, MemberPanelThirdPlace)
        });

        /// <summary>
        /// Включает или отключает выделение участников настраиваемыми цветами
        /// </summary>
        public static bool UseColorRanges = true;

        #endregion

        #region Панель предварительного места

        /// <summary>
        /// Цвет предварительного места участника
        /// </summary>
        public static Color MemberPlaceFontColor = Colors.Black;
        
        /// <summary>
        /// Размер шрифта для панели с предварительным местом в топе
        /// </summary>
        public static int MemberPlaceFontSize = 22;

        /// <summary>
        /// Стиль шрифта для панели с предварительным местом участника
        /// </summary>
        public static FontWeight MemberPlaceFontWeight = FontWeights.Black;

        /// <summary>
        /// Отступ между панелью с предварительным местом и главной панелью участнику
        /// </summary>
        public static double MemberPlacePanelOffset = 0;

        /// <summary>
        /// Режим показа мест
        /// </summary>
        public static PlaceShowMode MemberPlaceShowMode = PlaceShowMode.AlwaysVisible;
        /// <summary>
        /// Режим показа мест
        /// </summary>
        public enum PlaceShowMode
        {
            /// <summary>
            /// Всегда отображаются
            /// </summary>
            AlwaysVisible,

            /// <summary>
            /// Отображаются только на финальном экране
            /// </summary>
            VisibleOnFS,

            /// <summary>
            /// Спрятаны
            /// </summary>
            Hidden
        }

        /// <summary>
        /// Ширина контура вокруг предварительного места
        /// </summary>
        public static double MemberPlaceStrokeWidth = 2;

        /// <summary>
        /// Цвет панели предварительного места участника
        /// </summary>
        public static Color MemberPlacePanelColor = new Color()
        {
            A = 100,
            R = 200,
            G = 200,
            B = 200
        };

        /// <summary>
        /// Цвет контура вокруг предварительного места
        /// </summary>
        public static Color MemberPlaceStrokeColor = new Color()
        {
            A = 200,
            R = 200,
            G = 200,
            B = 200
        };

        #endregion

        #region Панель дополнительных данных

        /// <summary>
        /// Режим отображения результата участника
        /// </summary>
        public static ResultShowMode ShowMemberResultMode = ResultShowMode.Hidden;
        /// <summary>
        /// Режим отображения результата участника
        /// </summary>
        public enum ResultShowMode
        {
            /// <summary>
            /// Всегда отображать результаты участника
            /// </summary>
            AlwaysVisible,

            /// <summary>
            /// Отображать результаты участника после его выделения
            /// </summary>
            Visible,

            /// <summary>
            /// Не отображать результаты участника
            /// </summary>
            Hidden
        }

        /// <summary>
        /// Ширина панели дополнительных данных
        /// </summary>
        public static double MemberResultPanelWidth = 50;

        /// <summary>
        /// Высота панели дополнительных данных
        /// </summary>
        public static double MemberResultPanelHeight = 45;

        /// <summary>
        /// Размер шрифта для панели с дополнительными данными
        /// </summary>
        public static int MemberResultFontSize = 22;

        /// <summary>
        /// Отступ между панелью с дополнительными данными и главной панелью
        /// </summary>
        public static int MemberResultPanelOffset = 0;

        /// <summary>
        /// Ширина контура для панели с дополнительными данными
        /// </summary>
        public static double MemberResultStrokeWidth = 2;

        /// <summary>
        /// Стиль шрифта для дополнительных данных
        /// </summary>
        public static FontWeight MemberResultFontWeight = FontWeights.Thin;

        /// <summary>
        /// Цвет шрифта для панели дополнительных данных
        /// </summary>
        public static Color MemberResultFontColor = Colors.Black;

        /// <summary>
        /// Цвет фона на пенли с дополнительными данными
        /// </summary>
        public static Color MemberResultPanelColor = new Color()
        {
            A = 100,
            R = 200,
            G = 200,
            B = 200
        };

        /// <summary>
        /// Цвет контура вокруг дополнительных данных
        /// </summary>
        public static Color MemberResultStrokeColor = new Color()
        {
            A = 200,
            R = 200,
            G = 200,
            B = 200
        };

        #endregion

        #region Панель жюри
        /// <summary>
        /// Ширина панели жюри
        /// </summary>
        public static double JuryPanelWidth = 405;

        /// <summary>
        /// Высота панели жюри
        /// </summary>
        public static double JuryPanelHeight = 35;

        /// <summary>
        /// Итоговая прозрачность панели жюри после проявления
        /// </summary>
        public static double JuryPanelOpacity = 1;

        /// <summary>
        /// Цвет панели жюри
        /// </summary>
        public static Color JuryPanelColor = new Color()
        {
            A = 100,
            R = 200,
            G = 200,
            B = 200
        };

        /// <summary>
        /// Тип шрифта панели жюри
        /// </summary>
        public static FontWeight JuryFontWeight = FontWeights.Thin;

        /// <summary>
        /// Ширина контура для панели жюри
        /// </summary>
        public static double JuryPanelStrokeWidth = 1;

        /// <summary>
        /// Цвет контура панели жюри
        /// </summary>
        public static Color JuryPanelStrokeColor = new Color()
        {
            A = 100,
            R = 200,
            G = 200,
            B = 200
        };

        /// <summary>
        /// Размер шрифта для панели жюри
        /// </summary>
        public static double JuryFontSize = 20;

        #endregion

        #region Панель баллов

        /// <summary>
        /// Размер шрифта для панели с баллами
        /// </summary>
        public static double PointBarFontSize = 120;

        /// <summary>
        /// Итоговая прозрачность панели с баллами после проявления
        /// </summary>
        public static double PointBarPanelOpacity = 1;

        #endregion

        #region Расстановка объектов
        /// <summary>
        /// Интервал между верхней частью окна и панелью жюри
        /// </summary>
        public static int TopJuryInterval = 100;

        /// <summary>
        /// Интервал между панелью жюри и началом таблицы с участниками
        /// </summary>
        public static int JuryMemberOffset = 100;

        /// <summary>
        /// Расстояние между панелями участников
        /// </summary>
        public static int MemberInterval = 5;

        /// <summary>
        /// Расстояние между колонками в случае отображения участников в две колонки
        /// </summary>
        public static int MemberColumnInterval = 100;

        /// <summary>
        /// Режим выдачи баллов участникам
        /// </summary>
        public static PointsMode MemberPointsMode = PointsMode.Descending;
        /// <summary>
        /// Режим выдачи баллов участникам
        /// </summary>
        public enum PointsMode
        {
            /// <summary>
            /// Баллы выдаются участникам в том порядке, в котором они расположены по списку
            /// </summary>
            Standard,

            /// <summary>
            /// Баллы выдаются с наименьшего
            /// </summary>
            Ascending,

            /// <summary>
            /// Баллы выдаются с наибольшего
            /// </summary>
            Descending
        }
        
        /// <summary>
        /// Режим сортировки участника
        /// </summary>
        public static SortingMode MemberSortingMode = SortingMode.Descending;
        /// <summary>
        /// Режим сортировки участника
        /// </summary>
        public enum SortingMode
        {
            /// <summary>
            /// По возрастанию
            /// </summary>
            Ascending,

            /// <summary>
            /// По убыванию
            /// </summary>
            Descending
        }
        
        /// <summary>
        /// Режим расстановки участников в топе согласно стандартным требованиям спорта
        /// </summary>
        public static bool TrueTopRating = true;

        /// <summary>
        /// Жюри, с которого начинается показ
        /// </summary>
        public static int StartJury = 0;

        /// <summary>
        /// Включает или отключает отображение результатов в две колонки
        /// </summary>
        public static bool TwoColumns = true;

        /// <summary>
        /// Количество участников, которое должна уместить одна колонка в режиме отображения в две колонки
        /// </summary>
        public static int MaxMembersInColumn = 10;

        /// <summary>
        /// Финальная фраза, отображаемая в коцне показа
        /// </summary>
        public static string FinalPhrase = "Поздравляем победителей!";

        /// <summary>
        /// Режим отображения фраз
        /// </summary>
        public enum PhraseShowMode
        {
            /// <summary>
            /// Не показывать фразу
            /// </summary>
            Never,

            /// <summary>
            /// Показывать только на финальном экране
            /// </summary>
            OnlyOnFinalScreen,

            /// <summary>
            /// Показывать всегда
            /// </summary>
            Always
        }

        /// <summary>
        /// Режим отображения названия колонки с баллами
        /// </summary>
        public static PhraseShowMode PointsColumnPhraseShowMode = PhraseShowMode.Always;    // TODO    VIS SET    PhraseShowMode PointsColumnPhraseShowMode = PhraseShowMode.Always;

        /// <summary>
        /// Включает или отключает подчёркивание названия колонки с баллами
        /// </summary>
        public static bool PointsColumnPhraseIsUnderlined = true;    // TODO    VIS SET    bool PointsColumnPhraseIsUnderLined = true;

        /// <summary>
        /// Стиль шрифта для названия колонки с баллами
        /// </summary>
        public static FontWeight PointsColumnPhraseFontWeight = FontWeights.Normal;         // TODO    VIS SET    FontWeight PointsColumnPhraseFontWeight = FontWeights.Normal;

        /// <summary>
        /// Цвет шрифта для названия колонки с баллами
        /// </summary>
        public static Color PointsColumnPhraseFontColor = Colors.Red;                     // TODO    IO VIS SET    Color PointsColumnPhraseFontColor = Colors.Black;

        /// <summary>
        /// Размер шрифта для названия колонки с баллами
        /// </summary>
        public static double PointsColumnPhraseFontSize = 14;                               // TODO    IO VIS SET    double PointsColumnPhraseFontSize = 12;

        /// <summary>
        /// Название колонки с баллами
        /// </summary>
        public static string PointsColumnPhrase = "Баллы";                                  // TODO    VIS SET    string PointsColumnPhrase = "Баллы"

        /// <summary>
        /// Смещение фразы по горизонтали
        /// </summary>
        public static double PointsColumnPhraseXOffset = -2;                                 // TODO    VIS SET    double PointsColumnPhraseXOffset = 0;

        /// <summary>
        /// Смещение фразы по вертикали
        /// </summary>
        public static double PointsColumnPhraseYOffset = -8;                                 // TODO    VIS SET    double PointsColumnPhraseYOffset = 0;

        /// <summary>
        /// Режим отображения названия колонки с результатами
        /// </summary>
        public static PhraseShowMode ResultColumnPhraseShowMode = PhraseShowMode.Never;    // TODO    VIS SET    PhraseShowMode ResultColumnPhraseShowMode = PhraseShowMode.Always;

        /// <summary>
        /// Включает или отключает подчёркивание названия колонки с результатами
        /// </summary>
        public static bool ResultColumnPhraseIsUnderlined = true;    // TODO    VIS SET    bool ResultColumnPhraseIsUnderlined = true;

        /// <summary>
        /// Стиль шрифта для названия колонки с результатами
        /// </summary>
        public static FontWeight ResultColumnPhraseFontWeight = FontWeights.Normal;         // TODO    IO VIS SET    FontWeight ResultColumnPhraseFontWeight = FontWeights.Normal;

        /// <summary>
        /// Цвет шрифта для названия колонки с результатами
        /// </summary>
        public static Color ResultColumnPhraseFontColor = Colors.Red;                       // TODO    IO VIS SET    Color ResultColumnPhraseFontColor = Colors.Red;

        /// <summary>
        /// Размер шрифта для названия колонки с результатами
        /// </summary>
        public static double ResultColumnPhraseFontSize = 14;                               // TODO    IO VIS SET    double ResultColumnPhraseFontSize = 12;

        /// <summary>
        /// Название колонки с результатами
        /// </summary>
        public static string ResultColumnPhrase = "Результаты";                             // TODO    VIS SET    string ResultColumnPhrase = "Результаты"

        /// <summary>
        /// Смещение фразы по горизонтали
        /// </summary>
        public static double ResultColumnPhraseXOffset = -4;                                 // TODO    VIS SET    double ResultColumnPhraseXOffset = 0;

        /// <summary>
        /// Смещение фразы по вертикали
        /// </summary>
        public static double ResultColumnPhraseYOffset = -7;                                 // TODO    VIS SET    double ResultColumnPhraseYOffset = 0;

        /// <summary>
        /// Режим отображения названия колонки с местом в топе
        /// </summary>
        public static PhraseShowMode PlaceColumnPhraseShowMode = PhraseShowMode.Always;     // TODO    VIS SET    PhraseShowMode PlaceColumnPhraseShowMode = PhraseShowMode.Always;

        /// <summary>
        /// Доволнительные параметры отображения названия колонки с местами в топе
        /// </summary>
        public static bool PlaceColumnPhraseIsUnderlined = true;     // TODO    VIS SET    bool PlaceColumnPhraseIsUnderlined = true;

        /// <summary>
        /// Стиль шрифта для названия колонки с местами в топе
        /// </summary>
        public static FontWeight PlaceColumnPhraseFontWeight = FontWeights.Normal;         // TODO    IO VIS SET    FontWeight PlaceColumnPhraseFontWeight = FontWeights.Normal;

        /// <summary>
        /// Цвет шрифта для названия колонки с местами в топе
        /// </summary>
        public static Color PlaceColumnPhraseFontColor = Colors.Red;                       // TODO    IO VIS SET    Color PlaceColumnPhraseFontColor = Colors.Red;

        /// <summary>
        /// Размер шрифта для названия колонки с местами в топе
        /// </summary>
        public static double PlaceColumnPhraseFontSize = 14;                               // TODO    IO VIS SET    double PlaceColumnPhraseFontSize = 12;

        /// <summary>
        /// Название колонки с местом в топе
        /// </summary>
        public static string PlaceColumnPhrase = "Место";                                   // TODO    VIS SET    string PlaceColumnPhrase = "Место"

        /// <summary>
        /// Смещение фразы по горизонтали
        /// </summary>
        public static double PlaceColumnPhraseXOffset = 2;                                  // TODO    VIS SET    double PlaceColumnPhraseXOffset = 0;

        /// <summary>
        /// Смещение фразы по вертикали
        /// </summary>
        public static double PlaceColumnPhraseYOffset = -8;                                  // TODO    VIS SET    double PlaceColumnPhraseYOffset = 0;

        #endregion

        #region Нижняя фраза

        /// <summary>
        /// Тип шрифта для нижней фразы
        /// </summary>
        public static FontWeight LowerPhraseFontWeight = FontWeights.Normal;

        /// <summary>
        /// Цвет шрифта нижне фразы
        /// </summary>
        public static Color LowerPhraseFontColor = Colors.Black;

        /// <summary>
        /// Расстояние между нижним краем экрана и нижним уведомлением
        /// </summary>
        public static double LowerPhraseOffset = 300;

        /// <summary>
        /// Размер шрифта для нижней фразы
        /// </summary>
        public static double LowerPhraseFontSize = 30;

        /// <summary>
        /// Режим отображения нижней фразы
        /// </summary>
        public enum ShowMode
        {
            /// <summary>
            /// Отображать всегда
            /// </summary>
            AlwaysVisible,

            /// <summary>
            /// Отображать только при наличии участников со значениями баллов X/Н
            /// </summary>
            OnlyXN,

            /// <summary>
            /// ОТображение нижней фразы выключено
            /// </summary>
            Never
        }

        /// <summary>
        /// Режим отображения нижней фразы
        /// </summary>
        public static ShowMode LowerPhraseShowMode = ShowMode.AlwaysVisible;

        /// <summary>
        /// Текст нижнего уведомления
        /// </summary>
        public static string LowerPhrase = "Н - не явившиеся на конкурс \nX - не приславшие конкурсную работу";

        #endregion

        #region Анимация

        /// <summary>
        /// Фон в отображателе
        /// </summary>
        public static Brush MainBackground = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.mainBackground2.GetHbitmap(),
                                                                              IntPtr.Zero,
                                                                              Int32Rect.Empty,
                                                                              BitmapSizeOptions.FromEmptyOptions()));
        //public static Brush MainBackground = Brushes.White;

        /// <summary>
        /// Включает или отключает анимаци баллов
        /// </summary>
        public static bool ShowPointAnim = true;

        /// <summary>
        /// Включает или отключает анимированный фон
        /// </summary>
        public static bool AnimatedBackground = false;

        /// <summary>
        /// Включает или отключает видео на фоне
        /// </summary>
        public static bool VideoBackground = false;

        /// <summary>
        /// Путь к файлу видео
        /// </summary>
        public static string VideoPath = "background.mp4";

        /// <summary>
        /// Время на перемещение объекта
        /// </summary>
        public static TimeSpan AnimMoveTime = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Время на проявление объекта
        /// </summary>
        public static TimeSpan AnimAppearTime = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Задержка между выделением участника и появлением панели PointBar
        /// </summary>
        public static TimeSpan AnimPause = TimeSpan.FromMilliseconds(1100);

        /// <summary>
        /// Задержка перед тем, как PointBar полетит к панели участника
        /// </summary>
        public static TimeSpan AnimPointBarPause = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Первый цвет, которым будет переливаться фон
        /// </summary>
        public static Color BackgroundColor1 = Colors.DarkBlue;

        /// <summary>
        /// Второй цвет, которым будет переливаться фон
        /// </summary>
        public static Color BackgroundColor2 = Colors.DeepSkyBlue;

        /// <summary>
        /// Период, с которым будет переливаться фон
        /// </summary>
        public static TimeSpan BackgroundAnimPeriod = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Время, за которое будет происходить проявление фона
        /// </summary>
        public static TimeSpan BackgroundAppearTime = TimeSpan.FromSeconds(2);

        #endregion
    }
}