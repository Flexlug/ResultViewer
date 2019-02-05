using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using ResultViewerWPF.Viewer;
using System.Windows;

namespace ResultViewerWPF
{
    public class DataIO
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public static string FilePath = "data.xml";

        /// <summary>
        /// Сохраняет Dictionary и List
        /// </summary>
        /// <param name="juryChoise">Коллекция выбора жюри</param>
        /// <param name="memberList">Коллекция имён участников</param>
        public static void SaveData(Logic appLogic)
        {
            Dictionary<string, Tuple<List<double>, List<double>>> juryChoise = appLogic.GetJuryChoice();
            List<string> memberList = appLogic.GetMembersList();

            XDocument xdoc = new XDocument();               //Создание xml файла
            XElement data = new XElement("Data");           //Создание основного элемента xml
            XElement jurys = new XElement("Jurys_List", new XAttribute("list", "jury"), from keyValue in juryChoise select new XElement("Jury", new XAttribute("name", keyValue.Key), keyValue.Value.Item1.Select(x => new XElement("Ball", x)), keyValue.Value.Item2.Select(x => new XElement("RV", Convert.ToString(x)))));     //Сохранение Dictionary<string, Tuple<List<int>, List<fouble>>>
            XElement members = new XElement("Members_List", new XAttribute("list", "members"), memberList.Select(x => new XElement("Member", new XAttribute("name", x))));      //Сохранение List<string>

            #region OldSettings

            XElement oldSettings = new XElement("Old_Settings");

            oldSettings.Add(new XElement("ContBarColor", new XAttribute("R", ProgramSettings.OldSettings.ContBarColor.R),
                                                         new XAttribute("G", ProgramSettings.OldSettings.ContBarColor.G),
                                                         new XAttribute("B", ProgramSettings.OldSettings.ContBarColor.B),
                                                         new XAttribute("A", ProgramSettings.OldSettings.ContBarColor.A)));

            oldSettings.Add(new XElement("sndContBarColor", new XAttribute("R", ProgramSettings.OldSettings.sndContBarColor.R),
                                                            new XAttribute("G", ProgramSettings.OldSettings.sndContBarColor.G),
                                                            new XAttribute("B", ProgramSettings.OldSettings.sndContBarColor.B),
                                                            new XAttribute("A", ProgramSettings.OldSettings.sndContBarColor.A)));

            oldSettings.Add(new XElement("ContBarFontSize", new XAttribute("Value", ProgramSettings.OldSettings.ContBarFontSize)));
            oldSettings.Add(new XElement("ContBarWidth", new XAttribute("Value", ProgramSettings.OldSettings.ContBarWidth)));
            oldSettings.Add(new XElement("ContBarHeight", new XAttribute("Value", ProgramSettings.OldSettings.ContBarHeight)));

            oldSettings.Add(new XElement("XNum", new XAttribute("Value", ProgramSettings.OldSettings.XNum)));
            oldSettings.Add(new XElement("YNum", new XAttribute("Value", ProgramSettings.OldSettings.YNum)));
            oldSettings.Add(new XElement("FrameRate", new XAttribute("Value", ProgramSettings.OldSettings.FrameRate)));
            oldSettings.Add(new XElement("FrameInterval", new XAttribute("Value", ProgramSettings.OldSettings.FrameInterval)));
            oldSettings.Add(new XElement("QuitFrase", new XAttribute("Value", ProgramSettings.OldSettings.QuitFrase)));

            oldSettings.Add(new XElement("PointBarColor", new XAttribute("R", ProgramSettings.OldSettings.PointBarColor.R),
                                                            new XAttribute("G", ProgramSettings.OldSettings.PointBarColor.G),
                                                            new XAttribute("B", ProgramSettings.OldSettings.PointBarColor.B),
                                                            new XAttribute("A", ProgramSettings.OldSettings.PointBarColor.A)));

            oldSettings.Add(new XElement("sndPointBarColor", new XAttribute("R", ProgramSettings.OldSettings.sndPointBarColor.R),
                                                            new XAttribute("G", ProgramSettings.OldSettings.sndPointBarColor.G),
                                                            new XAttribute("B", ProgramSettings.OldSettings.sndPointBarColor.B),
                                                            new XAttribute("A", ProgramSettings.OldSettings.sndPointBarColor.A)));

            oldSettings.Add(new XElement("pointBarWidth", new XAttribute("Value", ProgramSettings.OldSettings.pointBarWidth)));
            oldSettings.Add(new XElement("pointBarHeight", new XAttribute("Value", ProgramSettings.OldSettings.pointBarHeight)));
            oldSettings.Add(new XElement("pointBarInterval", new XAttribute("Value", ProgramSettings.OldSettings.pointBarInterval)));
            oldSettings.Add(new XElement("pointBarFontSize", new XAttribute("Value", ProgramSettings.OldSettings.pointBarFontSize)));


            oldSettings.Add(new XElement("JuryBarColor", new XAttribute("R", ProgramSettings.OldSettings.JuryBarColor.R),
                                                            new XAttribute("G", ProgramSettings.OldSettings.JuryBarColor.G),
                                                            new XAttribute("B", ProgramSettings.OldSettings.JuryBarColor.B),
                                                            new XAttribute("A", ProgramSettings.OldSettings.JuryBarColor.A)));

            oldSettings.Add(new XElement("sndJuryBarColor", new XAttribute("R", ProgramSettings.OldSettings.sndJuryBarColor.R),
                                                            new XAttribute("G", ProgramSettings.OldSettings.sndJuryBarColor.G),
                                                            new XAttribute("B", ProgramSettings.OldSettings.sndJuryBarColor.B),
                                                            new XAttribute("A", ProgramSettings.OldSettings.sndJuryBarColor.A)));

            oldSettings.Add(new XElement("JuryBarWidth", new XAttribute("Value", ProgramSettings.OldSettings.JuryBarWidth)));
            oldSettings.Add(new XElement("JuryBarHeight", new XAttribute("Value", ProgramSettings.OldSettings.JuryBarHeight)));
            oldSettings.Add(new XElement("JuryBarFontSize", new XAttribute("Value", ProgramSettings.OldSettings.JuryBarFontSize)));

            #endregion

            #region newSettings

            XElement settings = new XElement("Settings");

            settings.Add(new XElement("MemberPanelWidth", new XAttribute("Value", ProgramSettings.MemberPanelWidth)));
            settings.Add(new XElement("MemberPanelHeight", new XAttribute("Value", ProgramSettings.MemberPanelHeight)));
            settings.Add(new XElement("MemberNameFontSize", new XAttribute("Value", ProgramSettings.MemberNameFontSize)));
            settings.Add(new XElement("MemberPanelOpacity", new XAttribute("Value", ProgramSettings.MemberPanelOpacity)));

            settings.Add(new XElement("MemberPanelColor", new XAttribute("R", ProgramSettings.MemberPanelColor.R),
                                                             new XAttribute("G", ProgramSettings.MemberPanelColor.G),
                                                             new XAttribute("B", ProgramSettings.MemberPanelColor.B),
                                                             new XAttribute("A", ProgramSettings.MemberPanelColor.A)));


            settings.Add(new XElement("MemberPanelChosenColor", new XAttribute("R", ProgramSettings.MemberPanelChosenColor.R),
                                                                   new XAttribute("G", ProgramSettings.MemberPanelChosenColor.G),
                                                                   new XAttribute("B", ProgramSettings.MemberPanelChosenColor.B),
                                                                   new XAttribute("A", ProgramSettings.MemberPanelChosenColor.A)));


            settings.Add(new XElement("MemberPanelChosenColor2", new XAttribute("R", ProgramSettings.MemberPanelChosenColor2.R),
                                                                    new XAttribute("G", ProgramSettings.MemberPanelChosenColor2.G),
                                                                    new XAttribute("B", ProgramSettings.MemberPanelChosenColor2.B),
                                                                    new XAttribute("A", ProgramSettings.MemberPanelChosenColor2.A)));

            settings.Add(new XElement("MemberPanelHighlightLeaders", new XAttribute("Value", ProgramSettings.MemberPanelHighlightLeaders)));
            
            settings.Add(new XElement("MemberPanelFirstPlace", new XAttribute("R", ProgramSettings.MemberPanelFirstPlace.R),
                                                                    new XAttribute("G", ProgramSettings.MemberPanelFirstPlace.G),
                                                                    new XAttribute("B", ProgramSettings.MemberPanelFirstPlace.B),
                                                                    new XAttribute("A", ProgramSettings.MemberPanelFirstPlace.A)));

            settings.Add(new XElement("MemberPanelSecondPlace", new XAttribute("R", ProgramSettings.MemberPanelSecondPlace.R),
                                                                    new XAttribute("G", ProgramSettings.MemberPanelSecondPlace.G),
                                                                    new XAttribute("B", ProgramSettings.MemberPanelSecondPlace.B),
                                                                    new XAttribute("A", ProgramSettings.MemberPanelSecondPlace.A)));

            settings.Add(new XElement("MemberPanelThirdPlace", new XAttribute("R", ProgramSettings.MemberPanelThirdPlace.R),
                                                                    new XAttribute("G", ProgramSettings.MemberPanelThirdPlace.G),
                                                                    new XAttribute("B", ProgramSettings.MemberPanelThirdPlace.B),
                                                                    new XAttribute("A", ProgramSettings.MemberPanelThirdPlace.A)));

            settings.Add(new XElement("MemberPanelOtherPlaces", new XAttribute("R", ProgramSettings.MemberPanelOtherPlaces.R),
                                                                    new XAttribute("G", ProgramSettings.MemberPanelOtherPlaces.G),
                                                                    new XAttribute("B", ProgramSettings.MemberPanelOtherPlaces.B),
                                                                    new XAttribute("A", ProgramSettings.MemberPanelOtherPlaces.A)));

            settings.Add(new XElement("MemberPanelUseSecondChooseColor", new XAttribute("Value", ProgramSettings.MemberPanelUseSecondChooseColor)));

            settings.Add(new XElement("MemberNameFontWeight", new XAttribute("Value", Settings.ViewerSettings.ConvertFontWeightToIndex(ProgramSettings.MemberNameFontWeight))));

            settings.Add(new XElement("MemberNameFontColor", new XAttribute("R", ProgramSettings.MemberNameFontColor.Color.R),
                                                                    new XAttribute("G", ProgramSettings.MemberNameFontColor.Color.G),
                                                                    new XAttribute("B", ProgramSettings.MemberNameFontColor.Color.B),
                                                                    new XAttribute("A", ProgramSettings.MemberNameFontColor.Color.A)));

            settings.Add(new XElement("MemberPointsFontWeight", new XAttribute("Value", Settings.ViewerSettings.ConvertFontWeightToIndex(ProgramSettings.MemberPointsFontWeight))));

            settings.Add(new XElement("MemberPointsFontSize", new XAttribute("Value", ProgramSettings.MemberPointsFontSize)));
            settings.Add(new XElement("MemberPointsPanelHeight", new XAttribute("Value", ProgramSettings.MemberPointsPanelHeight)));
            settings.Add(new XElement("MemberPointsPanelWidth", new XAttribute("Value", ProgramSettings.MemberPointsPanelWidth)));

            settings.Add(new XElement("MemberPointsPanelColor", new XAttribute("R", ProgramSettings.MemberPointsPanelColor.R),
                                                        new XAttribute("G", ProgramSettings.MemberPointsPanelColor.G),
                                                        new XAttribute("B", ProgramSettings.MemberPointsPanelColor.B),
                                                        new XAttribute("A", ProgramSettings.MemberPointsPanelColor.A)));

            settings.Add(new XElement("MemberPointsStrokeColor", new XAttribute("R", ProgramSettings.MemberPointsStrokeColor.R),
                                                        new XAttribute("G", ProgramSettings.MemberPointsStrokeColor.G),
                                                        new XAttribute("B", ProgramSettings.MemberPointsStrokeColor.B),
                                                        new XAttribute("A", ProgramSettings.MemberPointsStrokeColor.A)));

            settings.Add(new XElement("MemberPointsFontColor", new XAttribute("R", ProgramSettings.MemberPointsFontColor.Color.R),
                                                        new XAttribute("G", ProgramSettings.MemberPointsFontColor.Color.G),
                                                        new XAttribute("B", ProgramSettings.MemberPointsFontColor.Color.B),
                                                        new XAttribute("A", ProgramSettings.MemberPointsFontColor.Color.A)));

            settings.Add(new XElement("MemberPointsStrokeWidth", new XAttribute("Value", ProgramSettings.MemberPointsStrokeWidth)));

            settings.Add(new XElement("MemberPlaceFontColor", new XAttribute("R", ProgramSettings.MemberPlaceFontColor.R),
                                                        new XAttribute("G", ProgramSettings.MemberPlaceFontColor.G),
                                                        new XAttribute("B", ProgramSettings.MemberPlaceFontColor.B),
                                                        new XAttribute("A", ProgramSettings.MemberPlaceFontColor.A)));

            settings.Add(new XElement("MemberPlaceFontSize", new XAttribute("Value", ProgramSettings.MemberPlaceFontSize)));

            settings.Add(new XElement("MemberPlaceFontWeight", new XAttribute("Value", Settings.ViewerSettings.ConvertFontWeightToIndex(ProgramSettings.MemberPlaceFontWeight))));

            settings.Add(new XElement("MemberPlacePanelOffset", new XAttribute("Value", ProgramSettings.MemberPlacePanelOffset)));

            settings.Add(new XElement("MemberPlaceShowMode", new XAttribute("Value", (int)ProgramSettings.MemberPlaceShowMode)));

            settings.Add(new XElement("MemberPlaceStrokeWidth", new XAttribute("Value", ProgramSettings.MemberPlaceStrokeWidth)));

            settings.Add(new XElement("MemberPlacePanelColor", new XAttribute("R", ProgramSettings.MemberPlacePanelColor.R),
                                                        new XAttribute("G", ProgramSettings.MemberPlacePanelColor.G),
                                                        new XAttribute("B", ProgramSettings.MemberPlacePanelColor.B),
                                                        new XAttribute("A", ProgramSettings.MemberPlacePanelColor.A)));

            settings.Add(new XElement("MemberPlaceStrokeColor", new XAttribute("R", ProgramSettings.MemberPlaceStrokeColor.R),
                                                        new XAttribute("G", ProgramSettings.MemberPlaceStrokeColor.G),
                                                        new XAttribute("B", ProgramSettings.MemberPlaceStrokeColor.B),
                                                        new XAttribute("A", ProgramSettings.MemberPlaceStrokeColor.A)));

            settings.Add(new XElement("ShowMemberResultMode", new XAttribute("Value", (int)ProgramSettings.ShowMemberResultMode)));

            settings.Add(new XElement("MemberResultPanelWidth", new XAttribute("Value", ProgramSettings.MemberResultPanelWidth)));
            settings.Add(new XElement("MemberResultPanelHeight", new XAttribute("Value", ProgramSettings.MemberResultPanelHeight)));
            settings.Add(new XElement("MemberResultFontSize", new XAttribute("Value", ProgramSettings.MemberResultFontSize)));
            settings.Add(new XElement("MemberResultPanelOffset", new XAttribute("Value", ProgramSettings.MemberResultPanelOffset)));
            settings.Add(new XElement("MemberResultStrokeWidth", new XAttribute("Value", ProgramSettings.MemberResultStrokeWidth)));

            settings.Add(new XElement("MemberResultFontWeight", new XAttribute("Value", Settings.ViewerSettings.ConvertFontWeightToIndex(ProgramSettings.MemberResultFontWeight))));

            settings.Add(new XElement("MemberResultFontColor", new XAttribute("R", ProgramSettings.MemberResultFontColor.R),
                                            new XAttribute("G", ProgramSettings.MemberResultFontColor.G),
                                            new XAttribute("B", ProgramSettings.MemberResultFontColor.B),
                                            new XAttribute("A", ProgramSettings.MemberResultFontColor.A)));

            settings.Add(new XElement("MemberResultPanelColor", new XAttribute("R", ProgramSettings.MemberResultPanelColor.R),
                                new XAttribute("G", ProgramSettings.MemberResultPanelColor.G),
                                new XAttribute("B", ProgramSettings.MemberResultPanelColor.B),
                                new XAttribute("A", ProgramSettings.MemberResultPanelColor.A)));

            settings.Add(new XElement("MemberResultStrokeColor", new XAttribute("R", ProgramSettings.MemberResultStrokeColor.R),
                    new XAttribute("G", ProgramSettings.MemberResultStrokeColor.G),
                    new XAttribute("B", ProgramSettings.MemberResultStrokeColor.B),
                    new XAttribute("A", ProgramSettings.MemberResultStrokeColor.A)));

            settings.Add(new XElement("JuryPanelWidth", new XAttribute("Value", ProgramSettings.JuryPanelWidth)));
            settings.Add(new XElement("JuryPanelHeight", new XAttribute("Value", ProgramSettings.JuryPanelHeight)));
            settings.Add(new XElement("JuryPanelOpacity", new XAttribute("Value", ProgramSettings.JuryPanelOpacity)));
            settings.Add(new XElement("JuryFontSize", new XAttribute("Value", ProgramSettings.JuryFontSize)));

            settings.Add(new XElement("JuryPanelColor", new XAttribute("R", ProgramSettings.JuryPanelColor.R),
                                                        new XAttribute("G", ProgramSettings.JuryPanelColor.G),
                                                        new XAttribute("B", ProgramSettings.JuryPanelColor.B),
                                                        new XAttribute("A", ProgramSettings.JuryPanelColor.A)));

            settings.Add(new XElement("PointBarFontSize", new XAttribute("Value", ProgramSettings.PointBarFontSize)));
            settings.Add(new XElement("PointBarPanelOpacity", new XAttribute("Value", ProgramSettings.PointBarPanelOpacity)));

            settings.Add(new XElement("TopJuryInterval", new XAttribute("Value", ProgramSettings.TopJuryInterval)));
            settings.Add(new XElement("JuryMemberOffset", new XAttribute("Value", ProgramSettings.JuryMemberOffset)));
            settings.Add(new XElement("MemberInterval", new XAttribute("Value", ProgramSettings.MemberInterval)));
            settings.Add(new XElement("MemberColumnInterval", new XAttribute("Value", ProgramSettings.MemberColumnInterval)));

            settings.Add(new XElement("MemberPointsMode", new XAttribute("Value", (int)ProgramSettings.MemberPointsMode)));
            settings.Add(new XElement("MemberSortingMode", new XAttribute("Value", (int)ProgramSettings.MemberSortingMode)));
            settings.Add(new XElement("TrueTopRating", new XAttribute("Value", ProgramSettings.TrueTopRating)));
            settings.Add(new XElement("StartJury", new XAttribute("Value", ProgramSettings.StartJury)));
            settings.Add(new XElement("TwoColumns", new XAttribute("Value", ProgramSettings.TwoColumns)));
            settings.Add(new XElement("MaxMembersInColumn", new XAttribute("Value", ProgramSettings.MaxMembersInColumn)));
            settings.Add(new XElement("FinalPhrase", new XAttribute("Value", ProgramSettings.FinalPhrase)));

            settings.Add(new XElement("ShowPointAnim", new XAttribute("Value", ProgramSettings.ShowPointAnim)));
            settings.Add(new XElement("AnimatedBackground", new XAttribute("Value", ProgramSettings.AnimatedBackground)));
            settings.Add(new XElement("VideoBackground", new XAttribute("Value", ProgramSettings.VideoBackground)));
            settings.Add(new XElement("VideoPath", new XAttribute("Value", ProgramSettings.VideoPath)));

            settings.Add(new XElement("AnimMoveTime", new XAttribute("Value", ProgramSettings.AnimMoveTime.TotalMilliseconds)));
            settings.Add(new XElement("AnimAppearTime", new XAttribute("Value", ProgramSettings.AnimAppearTime.TotalMilliseconds)));
            settings.Add(new XElement("AnimPause", new XAttribute("Value", ProgramSettings.AnimPause.TotalMilliseconds)));
            settings.Add(new XElement("AnimPointBarPause", new XAttribute("Value", ProgramSettings.AnimPointBarPause.TotalMilliseconds)));

            settings.Add(new XElement("BackgroundColor1", new XAttribute("R", ProgramSettings.BackgroundColor1.R),
                                                        new XAttribute("G", ProgramSettings.BackgroundColor1.G),
                                                        new XAttribute("B", ProgramSettings.BackgroundColor1.B),
                                                        new XAttribute("A", ProgramSettings.BackgroundColor1.A)));

            settings.Add(new XElement("BackgroundColor2", new XAttribute("R", ProgramSettings.BackgroundColor2.R),
                                                        new XAttribute("G", ProgramSettings.BackgroundColor2.G),
                                                        new XAttribute("B", ProgramSettings.BackgroundColor2.B),
                                                        new XAttribute("A", ProgramSettings.BackgroundColor2.A)));

            settings.Add(new XElement("BackgroundAnimPeriod", new XAttribute("Value", ProgramSettings.BackgroundAnimPeriod.TotalMilliseconds)));
            settings.Add(new XElement("BackgroundAppearTime", new XAttribute("Value", ProgramSettings.BackgroundAppearTime.TotalMilliseconds)));
            
            #endregion

            data.Add(jurys);        //<
            data.Add(members);      //<
            data.Add(oldSettings);  // Собираем файл xml
            data.Add(settings);     //<
            xdoc.Add(data);         //<
            xdoc.Save(new StreamWriter(FilePath));  //Сохранение файла
        }

        /// <summary>
        /// Метод загружает список жюри и их баллы из файла старого формата
        /// </summary>
        /// <param name="appLogic">Ссылка на </param>
        public static void LoadOldData(ref Logic appLogic)
        {
            // Выделяем память
            string[] juryNames;
            string[] memberNames;
            int[][] juryChoice;

            // Временный буфер
            string inpStr;
            int inpInt;

            // Инициализируем считыватель файла
            using (StreamReader reader = new StreamReader(FilePath))
            {
                // Локальная функция, через которую мы будем получать стртоку из файла
                Func<string> getNextStr = () =>
                {
                    // Получаем строку из файла
                    inpStr = reader.ReadLine();

                    // Если она не нулевая, значит можно возвращать, иначе exception
                    if (inpStr != null)
                        return inpStr;
                    else
                        throw new Exception("Ошибка во время чтения файла. Возможно файл повреждён");
                };

                // Локальная функция для получения целыых чисел
                Func<int> getNextInt = () =>
                {
                    // Получаем строку из файла
                    inpStr = getNextStr();

                    // Если ковентировать получается, тогда возвращаем значение
                    if (int.TryParse(inpStr, out inpInt))
                        return inpInt;
                    else
                        throw new Exception("Ошибка во время чтения файла. Возможно файл повреждён");
                };

                // /\ /\ /\ /\ /\
                // || || || || ||
                // Это в любом случае пришлось бы писать, просто я не хотел засорять класс лишними методами
                // Зачем? Я хочу кастомный Exception =)

                // Получаем количество жюри
                int juryCount = getNextInt();

                // Инициализируем и заполняем массив с именами жюри
                juryNames = new string[juryCount];
                for (int jur = 0; jur < juryCount; jur++)
                    juryNames[jur] = getNextStr();

                // Получаем количество конкурсантов (включая нулевого)
                int membersCount = getNextInt();

                // Инициализируем и заполняем массив с именами участников
                memberNames = new string[membersCount];
                for (int mem = 0; mem < membersCount; mem++)
                    memberNames[mem] = getNextStr();

                // Инициализируем массив с баллами и считываем его
                juryChoice = new int[juryCount][];
                for (int jur = 0; jur < juryCount; jur++)
                {
                    // 10 - количество возможных баллов
                    juryChoice[jur] = new int[10];

                    // Заполняем баллы
                    for (int jurCh = 0; jurCh < 10; jurCh++)
                        juryChoice[jur][jurCh] = getNextInt();
                }

                #region Читаем настройки графики

                // Для сокращения кода
                OldSettingsProvider osp = ProgramSettings.OldSettings;

                osp.ContBarColor = Color.FromArgb(255, getNextInt(),
                                                       getNextInt(),
                                                       getNextInt());

                osp.sndContBarColor = Color.FromArgb(255, getNextInt(),
                                                          getNextInt(),
                                                          getNextInt());


                osp.ContBarFontSize = getNextInt();
                osp.ContBarWidth = getNextInt();
                osp.ContBarHeight = getNextInt();
                osp.XNum = getNextInt();
                osp.YNum = getNextInt();

                osp.PointBarColor = Color.FromArgb(255, getNextInt(),
                                                        getNextInt(),
                                                        getNextInt());

                osp.sndPointBarColor = Color.FromArgb(255, getNextInt(),
                                                           getNextInt(),
                                                           getNextInt());

                osp.pointBarWidth = getNextInt();
                osp.pointBarHeight = getNextInt();
                osp.pointBarInterval = getNextInt();
                osp.pointBarFontSize = getNextInt();

                osp.JuryBarColor = Color.FromArgb(255, getNextInt(),
                                                       getNextInt(),
                                                       getNextInt());

                osp.sndJuryBarColor = Color.FromArgb(255, getNextInt(),
                                                          getNextInt(),
                                                          getNextInt());

                osp.JuryBarWidth = getNextInt();
                osp.JuryBarHeight = getNextInt();
                osp.JuryBarFontSize = getNextInt();

                osp.QuitFrase = getNextStr();
                osp.FrameRate = getNextInt();
                osp.FrameInterval = getNextInt();

                #endregion

                reader.Close();
            }

            appLogic = new Logic(juryNames, memberNames, juryChoice);
        }

        public static void RestoreDefaultPath()
        {
            FilePath = "data.xml";
        }

        /// <summary>
        /// Метод загружает список жюри и их балы (Dictionary juryChoice) и список участников (List memberList) 
        /// </summary>
        /// <param name="juryChoice">Список жюри и их баллов</param>
        /// <param name="memberList">Список участников</param>
        public static void LoadData(Logic appLogic)
        {
            if (File.Exists(FilePath))                    //Проверяем на существование файла Data.xml
            {
                Dictionary<string, Tuple<List<double>, List<double>>> juryChoice = appLogic.GetJuryChoice();
                List<string> memberList = appLogic.GetMembersList();

                try         //Попытка загрузить данные
                {
                    XmlDocument xdoc = new XmlDocument();      //Загружаем файл xml
                    xdoc.Load(FilePath);                      //<
                    XmlElement xRoot = xdoc.DocumentElement;   //Берем верхний элемент
                    foreach (XmlNode xnode in xRoot)           //Перебераем все дочерние элементы Data
                    {
                        if (xnode.Attributes.Count > 0)        //Проверяем наличие наименования дочернего элемента Data
                        {
                            XmlNode list_info = xnode.Attributes.GetNamedItem("list");      //Запоминаем наименование дочернего элемента Data
                            switch (list_info.Value)                                        //После того, как узнали имя доч. элемента Data выполняется определенный код
                            {
                                case ("jury"):                          //Если дочерний элемент Jury_List
                                    foreach (XmlNode xnode2 in xnode)   //Перебираем все дочерние элементы Jury_List
                                    {
                                        juryChoice.Add(xnode2.Attributes.GetNamedItem("name").Value, Tuple.Create(new List<double>(), new List<double>()));                      //Загружаем имя жюри т.е. ключ Dictionary
                                        foreach (XmlNode ball in xnode2.ChildNodes)                                                         //Перебираем выставленные баллы и RV
                                        {
                                            switch (ball.Name)
                                            {
                                                case ("Ball"):                                                                                            //Записывает балл в List<int>
                                                    juryChoice[xnode2.Attributes.GetNamedItem("name").Value].Item1.Add(Convert.ToDouble(ball.InnerText.Replace('.', ',')));
                                                    break;
                                                case ("RV"):                                                                                              //Записывает RV в List<double>
                                                    juryChoice[xnode2.Attributes.GetNamedItem("name").Value].Item2.Add(Convert.ToDouble(ball.InnerText.Replace('.', ',')));
                                                    break;
                                            }
                                        }
                                    }
                                    break;
                                case ("members"):                                                       //Если дочерний элемент Members_List
                                    foreach (XmlNode xnode2 in xnode)                                   //Перебираем дочерние элементы Member_List
                                    {
                                        memberList.Add(xnode2.Attributes.GetNamedItem("name").Value);   //Загружаем имя участника в List
                                    }
                                    break;
                            }
                        }
                    }

                    #region Get OldSettings

                    XmlNode oldSettings = xRoot.SelectSingleNode("Old_Settings");
                    XmlNode tempElement = null;
                    int tempIntVar;
                    double tempDoubleVar;
                    byte tempByteVar;
                    bool tempBoolVar;

                    Func<string, int> getInt = (name) =>
                    {
                        if (int.TryParse(tempElement.Attributes.GetNamedItem(name).Value, out tempIntVar))
                            return tempIntVar;
                        else
                            throw new Exception("Не получилось сконвертировать данный элемент в тип int");
                    };

                    Func<string, string> getStr = (name) =>
                    {
                        return tempElement.Attributes.GetNamedItem(name).Value;
                    };

                    Func<string, double> getDouble = (name) =>
                    {
                        if (double.TryParse(tempElement.Attributes.GetNamedItem(name).Value, out tempDoubleVar))
                            return tempDoubleVar;
                        else
                            throw new Exception("Не получилось сконвертировать данный элемент в тип double");
                    };

                    Func<string, byte> getByte = (name) =>
                    {
                        if (byte.TryParse(tempElement.Attributes.GetNamedItem(name).Value, out tempByteVar))
                            return tempByteVar;
                        else
                            throw new Exception("Не получилось сконвертировать данный элемент в тип byte");
                    };

                    Func<string, bool> getBool = (name) =>
                    {
                        if (bool.TryParse(tempElement.Attributes.GetNamedItem(name).Value, out tempBoolVar))
                            return tempBoolVar;
                        else
                            throw new Exception("Не получилось сконвертировать данный элемент в тип bool");
                    };

                    tempElement = oldSettings.SelectSingleNode("ContBarColor");
                    ProgramSettings.OldSettings.ContBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    tempElement = oldSettings.SelectSingleNode("sndContBarColor");
                    ProgramSettings.OldSettings.sndContBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    tempElement = oldSettings.SelectSingleNode("ContBarFontSize");
                    ProgramSettings.OldSettings.ContBarFontSize = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("ContBarWidth");
                    ProgramSettings.OldSettings.ContBarWidth = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("ContBarHeight");
                    ProgramSettings.OldSettings.ContBarHeight = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("XNum");
                    ProgramSettings.OldSettings.XNum = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("YNum");
                    ProgramSettings.OldSettings.YNum = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("FrameRate");
                    ProgramSettings.OldSettings.FrameRate = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("FrameInterval");
                    ProgramSettings.OldSettings.FrameInterval = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("QuitFrase");
                    ProgramSettings.OldSettings.QuitFrase = getStr("Value");

                    tempElement = oldSettings.SelectSingleNode("PointBarColor");
                    ProgramSettings.OldSettings.PointBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    tempElement = oldSettings.SelectSingleNode("sndPointBarColor");
                    ProgramSettings.OldSettings.sndPointBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    tempElement = oldSettings.SelectSingleNode("pointBarWidth");
                    ProgramSettings.OldSettings.pointBarWidth = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("pointBarHeight");
                    ProgramSettings.OldSettings.pointBarHeight = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("pointBarInterval");
                    ProgramSettings.OldSettings.pointBarInterval = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("pointBarFontSize");
                    ProgramSettings.OldSettings.pointBarFontSize = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("JuryBarColor");
                    ProgramSettings.OldSettings.JuryBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    tempElement = oldSettings.SelectSingleNode("sndJuryBarColor");
                    ProgramSettings.OldSettings.sndJuryBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    tempElement = oldSettings.SelectSingleNode("JuryBarWidth");
                    ProgramSettings.OldSettings.JuryBarWidth = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("JuryBarHeight");
                    ProgramSettings.OldSettings.JuryBarHeight = getInt("Value");

                    tempElement = oldSettings.SelectSingleNode("JuryBarFontSize");
                    ProgramSettings.OldSettings.JuryBarFontSize = getInt("Value");

                    #endregion

                    #region Settings

                    XmlNode settings = xRoot.SelectSingleNode("Settings");

                    tempElement = settings.SelectSingleNode("MemberPanelWidth");
                    ProgramSettings.MemberPanelWidth = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPanelHeight");
                    ProgramSettings.MemberPanelHeight = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberNameFontSize");
                    ProgramSettings.MemberNameFontSize = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPanelOpacity");
                    ProgramSettings.MemberPanelOpacity = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPanelColor");
                    ProgramSettings.MemberPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPanelChosenColor");
                    ProgramSettings.MemberPanelChosenColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPanelChosenColor2");
                    ProgramSettings.MemberPanelChosenColor2 = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPanelHighlightLeaders");
                    ProgramSettings.MemberPanelHighlightLeaders = getBool("Value");

                    tempElement = settings.SelectSingleNode("MemberPanelFirstPlace");
                    ProgramSettings.MemberPanelFirstPlace = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPanelSecondPlace");
                    ProgramSettings.MemberPanelSecondPlace = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPanelSecondPlace");
                    ProgramSettings.MemberPanelSecondPlace = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPanelOtherPlaces");
                    ProgramSettings.MemberPanelOtherPlaces = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPanelUseSecondChooseColor");
                    ProgramSettings.MemberPanelUseSecondChooseColor = getBool("Value");

                    tempElement = settings.SelectSingleNode("MemberNameFontWeight");
                    ProgramSettings.MemberNameFontWeight = Settings.ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));
                    
                    tempElement = settings.SelectSingleNode("MemberNameFontColor");
                    ProgramSettings.MemberNameFontColor = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"))));

                    tempElement = settings.SelectSingleNode("MemberPointsFontWeight");
                    ProgramSettings.MemberPointsFontWeight = Settings.ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    tempElement = settings.SelectSingleNode("MemberPointsFontSize");
                    ProgramSettings.MemberPointsFontSize = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPointsPanelHeight");
                    ProgramSettings.MemberPointsPanelHeight = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPointsPanelWidth");
                    ProgramSettings.MemberPointsPanelWidth = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPointsPanelColor");
                    ProgramSettings.MemberPointsPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPointsStrokeColor");
                    ProgramSettings.MemberPointsStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPointsFontColor");
                    ProgramSettings.MemberPointsFontColor = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"))));

                    tempElement = settings.SelectSingleNode("MemberPointsStrokeWidth");
                    ProgramSettings.MemberPointsStrokeWidth = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPlaceFontColor");
                    ProgramSettings.MemberPlaceFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPlaceFontSize");
                    ProgramSettings.MemberPlaceFontSize = getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberPlaceFontWeight");
                    ProgramSettings.MemberPlaceFontWeight = Settings.ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    tempElement = settings.SelectSingleNode("MemberPlaceShowMode");
                    ProgramSettings.MemberPlaceShowMode = (ProgramSettings.PlaceShowMode)getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberPlaceStrokeWidth");
                    ProgramSettings.MemberPlaceStrokeWidth = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberPlacePanelColor");
                    ProgramSettings.MemberPlacePanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberPlaceStrokeColor");
                    ProgramSettings.MemberPlaceStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("ShowMemberResultMode");
                    ProgramSettings.ShowMemberResultMode = (ProgramSettings.ResultShowMode)getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberResultPanelWidth");
                    ProgramSettings.MemberResultPanelWidth = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberResultPanelHeight");
                    ProgramSettings.MemberResultPanelHeight = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberResultFontSize");
                    ProgramSettings.MemberResultFontSize = getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberResultPanelOffset");
                    ProgramSettings.MemberResultPanelOffset = getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberResultStrokeWidth");
                    ProgramSettings.MemberResultStrokeWidth = getDouble("Value");

                    tempElement = settings.SelectSingleNode("MemberResultFontWeight");
                    ProgramSettings.MemberResultFontWeight = Settings.ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    tempElement = settings.SelectSingleNode("MemberResultFontColor");
                    ProgramSettings.MemberResultFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberResultPanelColor");
                    ProgramSettings.MemberResultPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("MemberResultStrokeColor");
                    ProgramSettings.MemberResultStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("JuryPanelWidth");
                    ProgramSettings.JuryPanelWidth = getDouble("Value");

                    tempElement = settings.SelectSingleNode("JuryPanelHeight");
                    ProgramSettings.JuryPanelHeight = getDouble("Value");

                    tempElement = settings.SelectSingleNode("JuryPanelOpacity");
                    ProgramSettings.JuryPanelOpacity = getDouble("Value");

                    tempElement = settings.SelectSingleNode("JuryFontSize");
                    ProgramSettings.JuryFontSize = getDouble("Value");
                    
                    tempElement = settings.SelectSingleNode("JuryPanelColor");
                    ProgramSettings.JuryPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("PointBarFontSize");
                    ProgramSettings.PointBarFontSize = getDouble("Value");

                    tempElement = settings.SelectSingleNode("PointBarPanelOpacity");
                    ProgramSettings.PointBarPanelOpacity = getDouble("Value");

                    tempElement = settings.SelectSingleNode("TopJuryInterval");
                    ProgramSettings.TopJuryInterval = getInt("Value");

                    tempElement = settings.SelectSingleNode("JuryMemberOffset");
                    ProgramSettings.JuryMemberOffset = getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberInterval");
                    ProgramSettings.MemberInterval = getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberColumnInterval");
                    ProgramSettings.MemberColumnInterval = getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberPointsMode");
                    ProgramSettings.MemberPointsMode = (ProgramSettings.PointsMode)getInt("Value");

                    tempElement = settings.SelectSingleNode("MemberSortingMode");
                    ProgramSettings.MemberSortingMode = (ProgramSettings.SortingMode)getInt("Value");

                    tempElement = settings.SelectSingleNode("TrueTopRating");
                    ProgramSettings.TrueTopRating = getBool("Value");

                    tempElement = settings.SelectSingleNode("StartJury");
                    ProgramSettings.StartJury = getInt("Value");

                    tempElement = settings.SelectSingleNode("TwoColumns");
                    ProgramSettings.TwoColumns = getBool("Value");

                    tempElement = settings.SelectSingleNode("MaxMembersInColumn");
                    ProgramSettings.MaxMembersInColumn = getInt("Value");

                    tempElement = settings.SelectSingleNode("FinalPhrase");
                    ProgramSettings.FinalPhrase = getStr("Value");
                    
                    tempElement = settings.SelectSingleNode("ShowPointAnim");
                    ProgramSettings.ShowPointAnim = getBool("Value");

                    tempElement = settings.SelectSingleNode("AnimatedBackground");
                    ProgramSettings.AnimatedBackground = getBool("Value");

                    tempElement = settings.SelectSingleNode("VideoBackground");
                    ProgramSettings.VideoBackground = getBool("Value");

                    tempElement = settings.SelectSingleNode("VideoPath");
                    ProgramSettings.VideoPath = getStr("Value");

                    tempElement = settings.SelectSingleNode("AnimMoveTime");
                    ProgramSettings.AnimMoveTime = TimeSpan.FromMilliseconds(getDouble("Value"));

                    tempElement = settings.SelectSingleNode("AnimAppearTime");
                    ProgramSettings.AnimAppearTime = TimeSpan.FromMilliseconds(getDouble("Value"));

                    tempElement = settings.SelectSingleNode("AnimPause");
                    ProgramSettings.AnimPause = TimeSpan.FromMilliseconds(getDouble("Value"));

                    tempElement = settings.SelectSingleNode("AnimPointBarPause");
                    ProgramSettings.AnimPointBarPause = TimeSpan.FromMilliseconds(getDouble("Value"));

                    tempElement = settings.SelectSingleNode("BackgroundColor1");
                    ProgramSettings.BackgroundColor1 = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("BackgroundColor2");
                    ProgramSettings.BackgroundColor2 = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    tempElement = settings.SelectSingleNode("BackgroundAnimPeriod");
                    ProgramSettings.BackgroundAnimPeriod = TimeSpan.FromMilliseconds(getDouble("Value"));

                    tempElement = settings.SelectSingleNode("BackgroundAppearTime");
                    ProgramSettings.BackgroundAppearTime = TimeSpan.FromMilliseconds(getDouble("Value"));
                    #endregion
                }
                catch (Exception x)
                {
                    throw new Exception("Ошибка во время чтения файла");
                }
            }
        }
    }
}