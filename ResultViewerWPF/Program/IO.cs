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
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Windows;
using System.Text;

using ResultViewerWPF.Viewer.Dialogs;


namespace ResultViewerWPF.Program
{
    public class IO
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

            oldSettings.Add(new XElement("ContBarColor", new XAttribute("R", Program.Settings.OldSettings.ContBarColor.R),
                                                         new XAttribute("G", Program.Settings.OldSettings.ContBarColor.G),
                                                         new XAttribute("B", Program.Settings.OldSettings.ContBarColor.B),
                                                         new XAttribute("A", Program.Settings.OldSettings.ContBarColor.A)));

            oldSettings.Add(new XElement("sndContBarColor", new XAttribute("R", Program.Settings.OldSettings.sndContBarColor.R),
                                                            new XAttribute("G", Program.Settings.OldSettings.sndContBarColor.G),
                                                            new XAttribute("B", Program.Settings.OldSettings.sndContBarColor.B),
                                                            new XAttribute("A", Program.Settings.OldSettings.sndContBarColor.A)));

            oldSettings.Add(new XElement("ContBarFontSize", new XAttribute("Value", Program.Settings.OldSettings.ContBarFontSize)));
            oldSettings.Add(new XElement("ContBarWidth", new XAttribute("Value", Program.Settings.OldSettings.ContBarWidth)));
            oldSettings.Add(new XElement("ContBarHeight", new XAttribute("Value", Program.Settings.OldSettings.ContBarHeight)));

            oldSettings.Add(new XElement("XNum", new XAttribute("Value", Program.Settings.OldSettings.XNum)));
            oldSettings.Add(new XElement("YNum", new XAttribute("Value", Program.Settings.OldSettings.YNum)));
            oldSettings.Add(new XElement("FrameRate", new XAttribute("Value", Program.Settings.OldSettings.FrameRate)));
            oldSettings.Add(new XElement("FrameInterval", new XAttribute("Value", Program.Settings.OldSettings.FrameInterval)));
            oldSettings.Add(new XElement("QuitFrase", new XAttribute("Value", Program.Settings.OldSettings.QuitFrase)));

            oldSettings.Add(new XElement("PointBarColor", new XAttribute("R", Program.Settings.OldSettings.PointBarColor.R),
                                                            new XAttribute("G", Program.Settings.OldSettings.PointBarColor.G),
                                                            new XAttribute("B", Program.Settings.OldSettings.PointBarColor.B),
                                                            new XAttribute("A", Program.Settings.OldSettings.PointBarColor.A)));

            oldSettings.Add(new XElement("sndPointBarColor", new XAttribute("R", Program.Settings.OldSettings.sndPointBarColor.R),
                                                            new XAttribute("G", Program.Settings.OldSettings.sndPointBarColor.G),
                                                            new XAttribute("B", Program.Settings.OldSettings.sndPointBarColor.B),
                                                            new XAttribute("A", Program.Settings.OldSettings.sndPointBarColor.A)));

            oldSettings.Add(new XElement("pointBarWidth", new XAttribute("Value", Program.Settings.OldSettings.pointBarWidth)));
            oldSettings.Add(new XElement("pointBarHeight", new XAttribute("Value", Program.Settings.OldSettings.pointBarHeight)));
            oldSettings.Add(new XElement("pointBarInterval", new XAttribute("Value", Program.Settings.OldSettings.pointBarInterval)));
            oldSettings.Add(new XElement("pointBarFontSize", new XAttribute("Value", Program.Settings.OldSettings.pointBarFontSize)));


            oldSettings.Add(new XElement("JuryBarColor", new XAttribute("R", Program.Settings.OldSettings.JuryBarColor.R),
                                                            new XAttribute("G", Program.Settings.OldSettings.JuryBarColor.G),
                                                            new XAttribute("B", Program.Settings.OldSettings.JuryBarColor.B),
                                                            new XAttribute("A", Program.Settings.OldSettings.JuryBarColor.A)));

            oldSettings.Add(new XElement("sndJuryBarColor", new XAttribute("R", Program.Settings.OldSettings.sndJuryBarColor.R),
                                                            new XAttribute("G", Program.Settings.OldSettings.sndJuryBarColor.G),
                                                            new XAttribute("B", Program.Settings.OldSettings.sndJuryBarColor.B),
                                                            new XAttribute("A", Program.Settings.OldSettings.sndJuryBarColor.A)));

            oldSettings.Add(new XElement("JuryBarWidth", new XAttribute("Value", Program.Settings.OldSettings.JuryBarWidth)));
            oldSettings.Add(new XElement("JuryBarHeight", new XAttribute("Value", Program.Settings.OldSettings.JuryBarHeight)));
            oldSettings.Add(new XElement("JuryBarFontSize", new XAttribute("Value", Program.Settings.OldSettings.JuryBarFontSize)));

            #endregion

            #region newSettings

            XElement settings = new XElement("Settings");

            settings.Add(new XElement("MemberPanelWidth", new XAttribute("Value", Program.Settings.MemberPanelWidth)));
            settings.Add(new XElement("MemberPanelHeight", new XAttribute("Value", Program.Settings.MemberPanelHeight)));
            settings.Add(new XElement("MemberNameFontSize", new XAttribute("Value", Program.Settings.MemberNameFontSize)));
            settings.Add(new XElement("MemberPanelOpacity", new XAttribute("Value", Program.Settings.MemberPanelOpacity)));
            settings.Add(new XElement("MemberPanelStrokeWidth", new XAttribute("Value", Program.Settings.MemberPanelStrokeWidth)));

            settings.Add(new XElement("MemberPanelColor", new XAttribute("R", Program.Settings.MemberPanelColor.R),
                                                             new XAttribute("G", Program.Settings.MemberPanelColor.G),
                                                             new XAttribute("B", Program.Settings.MemberPanelColor.B),
                                                             new XAttribute("A", Program.Settings.MemberPanelColor.A)));


            settings.Add(new XElement("MemberPanelChosenColor", new XAttribute("R", Program.Settings.MemberPanelChosenColor.R),
                                                                   new XAttribute("G", Program.Settings.MemberPanelChosenColor.G),
                                                                   new XAttribute("B", Program.Settings.MemberPanelChosenColor.B),
                                                                   new XAttribute("A", Program.Settings.MemberPanelChosenColor.A)));


            settings.Add(new XElement("MemberPanelChosenColor2", new XAttribute("R", Program.Settings.MemberPanelChosenColor2.R),
                                                                    new XAttribute("G", Program.Settings.MemberPanelChosenColor2.G),
                                                                    new XAttribute("B", Program.Settings.MemberPanelChosenColor2.B),
                                                                    new XAttribute("A", Program.Settings.MemberPanelChosenColor2.A)));

            settings.Add(new XElement("MemberPanelStrokeColor", new XAttribute("R", Program.Settings.MemberPanelStrokeColor.R),
                                                                    new XAttribute("G", Program.Settings.MemberPanelStrokeColor.G),
                                                                    new XAttribute("B", Program.Settings.MemberPanelStrokeColor.B),
                                                                    new XAttribute("A", Program.Settings.MemberPanelStrokeColor.A)));

            settings.Add(new XElement("MemberPanelHighlightLeaders", new XAttribute("Value", Program.Settings.MemberPanelHighlightLeaders)));
            
            settings.Add(new XElement("MemberPanelFirstPlace", new XAttribute("R", Program.Settings.MemberPanelFirstPlace.R),
                                                                    new XAttribute("G", Program.Settings.MemberPanelFirstPlace.G),
                                                                    new XAttribute("B", Program.Settings.MemberPanelFirstPlace.B),
                                                                    new XAttribute("A", Program.Settings.MemberPanelFirstPlace.A)));

            settings.Add(new XElement("MemberPanelSecondPlace", new XAttribute("R", Program.Settings.MemberPanelSecondPlace.R),
                                                                    new XAttribute("G", Program.Settings.MemberPanelSecondPlace.G),
                                                                    new XAttribute("B", Program.Settings.MemberPanelSecondPlace.B),
                                                                    new XAttribute("A", Program.Settings.MemberPanelSecondPlace.A)));

            settings.Add(new XElement("MemberPanelThirdPlace", new XAttribute("R", Program.Settings.MemberPanelThirdPlace.R),
                                                                    new XAttribute("G", Program.Settings.MemberPanelThirdPlace.G),
                                                                    new XAttribute("B", Program.Settings.MemberPanelThirdPlace.B),
                                                                    new XAttribute("A", Program.Settings.MemberPanelThirdPlace.A)));

            settings.Add(new XElement("MemberPanelOtherPlaces", new XAttribute("R", Program.Settings.MemberPanelOtherPlaces.R),
                                                                    new XAttribute("G", Program.Settings.MemberPanelOtherPlaces.G),
                                                                    new XAttribute("B", Program.Settings.MemberPanelOtherPlaces.B),
                                                                    new XAttribute("A", Program.Settings.MemberPanelOtherPlaces.A)));

            settings.Add(new XElement("MemberPanelUseSecondChooseColor", new XAttribute("Value", Program.Settings.MemberPanelUseSecondChooseColor)));

            settings.Add(new XElement("MemberNameFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.MemberNameFontWeight))));

            settings.Add(new XElement("MemberNameFontColor", new XAttribute("R", Program.Settings.MemberNameFontColor.Color.R),
                                                                    new XAttribute("G", Program.Settings.MemberNameFontColor.Color.G),
                                                                    new XAttribute("B", Program.Settings.MemberNameFontColor.Color.B),
                                                                    new XAttribute("A", Program.Settings.MemberNameFontColor.Color.A)));

            settings.Add(new XElement("MemberPointsFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.MemberPointsFontWeight))));

            settings.Add(new XElement("MemberPointsFontSize", new XAttribute("Value", Program.Settings.MemberPointsFontSize)));
            settings.Add(new XElement("MemberPointsPanelHeight", new XAttribute("Value", Program.Settings.MemberPointsPanelHeight)));
            settings.Add(new XElement("MemberPointsPanelWidth", new XAttribute("Value", Program.Settings.MemberPointsPanelWidth)));

            settings.Add(new XElement("MemberPointsPanelColor", new XAttribute("R", Program.Settings.MemberPointsPanelColor.R),
                                                        new XAttribute("G", Program.Settings.MemberPointsPanelColor.G),
                                                        new XAttribute("B", Program.Settings.MemberPointsPanelColor.B),
                                                        new XAttribute("A", Program.Settings.MemberPointsPanelColor.A)));

            settings.Add(new XElement("MemberPointsStrokeColor", new XAttribute("R", Program.Settings.MemberPointsStrokeColor.R),
                                                        new XAttribute("G", Program.Settings.MemberPointsStrokeColor.G),
                                                        new XAttribute("B", Program.Settings.MemberPointsStrokeColor.B),
                                                        new XAttribute("A", Program.Settings.MemberPointsStrokeColor.A)));

            settings.Add(new XElement("MemberPointsFontColor", new XAttribute("R", Program.Settings.MemberPointsFontColor.Color.R),
                                                        new XAttribute("G", Program.Settings.MemberPointsFontColor.Color.G),
                                                        new XAttribute("B", Program.Settings.MemberPointsFontColor.Color.B),
                                                        new XAttribute("A", Program.Settings.MemberPointsFontColor.Color.A)));

            settings.Add(new XElement("MemberPointsStrokeWidth", new XAttribute("Value", Program.Settings.MemberPointsStrokeWidth)));

            settings.Add(new XElement("MemberPlaceFontColor", new XAttribute("R", Program.Settings.MemberPlaceFontColor.R),
                                                        new XAttribute("G", Program.Settings.MemberPlaceFontColor.G),
                                                        new XAttribute("B", Program.Settings.MemberPlaceFontColor.B),
                                                        new XAttribute("A", Program.Settings.MemberPlaceFontColor.A)));

            settings.Add(new XElement("MemberPlaceFontSize", new XAttribute("Value", Program.Settings.MemberPlaceFontSize)));

            settings.Add(new XElement("MemberPlaceFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.MemberPlaceFontWeight))));

            settings.Add(new XElement("MemberPlacePanelOffset", new XAttribute("Value", Program.Settings.MemberPlacePanelOffset)));

            settings.Add(new XElement("MemberPlaceShowMode", new XAttribute("Value", (int)Program.Settings.MemberPlaceShowMode)));

            settings.Add(new XElement("MemberPlaceStrokeWidth", new XAttribute("Value", Program.Settings.MemberPlaceStrokeWidth)));

            settings.Add(new XElement("MemberPlacePanelColor", new XAttribute("R", Program.Settings.MemberPlacePanelColor.R),
                                                        new XAttribute("G", Program.Settings.MemberPlacePanelColor.G),
                                                        new XAttribute("B", Program.Settings.MemberPlacePanelColor.B),
                                                        new XAttribute("A", Program.Settings.MemberPlacePanelColor.A)));

            settings.Add(new XElement("MemberPlaceStrokeColor", new XAttribute("R", Program.Settings.MemberPlaceStrokeColor.R),
                                                        new XAttribute("G", Program.Settings.MemberPlaceStrokeColor.G),
                                                        new XAttribute("B", Program.Settings.MemberPlaceStrokeColor.B),
                                                        new XAttribute("A", Program.Settings.MemberPlaceStrokeColor.A)));

            settings.Add(new XElement("ShowMemberResultMode", new XAttribute("Value", (int)Program.Settings.ShowMemberResultMode)));

            settings.Add(new XElement("MemberResultPanelWidth", new XAttribute("Value", Program.Settings.MemberResultPanelWidth)));
            settings.Add(new XElement("MemberResultPanelHeight", new XAttribute("Value", Program.Settings.MemberResultPanelHeight)));
            settings.Add(new XElement("MemberResultFontSize", new XAttribute("Value", Program.Settings.MemberResultFontSize)));
            settings.Add(new XElement("MemberResultPanelOffset", new XAttribute("Value", Program.Settings.MemberResultPanelOffset)));
            settings.Add(new XElement("MemberResultStrokeWidth", new XAttribute("Value", Program.Settings.MemberResultStrokeWidth)));

            settings.Add(new XElement("MemberResultFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.MemberResultFontWeight))));

            settings.Add(new XElement("MemberResultFontColor", new XAttribute("R", Program.Settings.MemberResultFontColor.R),
                                            new XAttribute("G", Program.Settings.MemberResultFontColor.G),
                                            new XAttribute("B", Program.Settings.MemberResultFontColor.B),
                                            new XAttribute("A", Program.Settings.MemberResultFontColor.A)));

            settings.Add(new XElement("MemberResultPanelColor", new XAttribute("R", Program.Settings.MemberResultPanelColor.R),
                                new XAttribute("G", Program.Settings.MemberResultPanelColor.G),
                                new XAttribute("B", Program.Settings.MemberResultPanelColor.B),
                                new XAttribute("A", Program.Settings.MemberResultPanelColor.A)));

            settings.Add(new XElement("MemberResultStrokeColor", new XAttribute("R", Program.Settings.MemberResultStrokeColor.R),
                    new XAttribute("G", Program.Settings.MemberResultStrokeColor.G),
                    new XAttribute("B", Program.Settings.MemberResultStrokeColor.B),
                    new XAttribute("A", Program.Settings.MemberResultStrokeColor.A)));

            settings.Add(new XElement("JuryPanelWidth", new XAttribute("Value", Program.Settings.JuryPanelWidth)));
            settings.Add(new XElement("JuryPanelHeight", new XAttribute("Value", Program.Settings.JuryPanelHeight)));
            settings.Add(new XElement("JuryPanelOpacity", new XAttribute("Value", Program.Settings.JuryPanelOpacity)));
            settings.Add(new XElement("JuryFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.JuryFontWeight))));
            settings.Add(new XElement("JuryPanelStrokeWidth", new XAttribute("Value", Program.Settings.JuryPanelStrokeWidth)));

            settings.Add(new XElement("JuryFontSize", new XAttribute("Value", Program.Settings.JuryFontSize)));

            settings.Add(new XElement("JuryPanelColor", new XAttribute("R", Program.Settings.JuryPanelColor.R),
                                                        new XAttribute("G", Program.Settings.JuryPanelColor.G),
                                                        new XAttribute("B", Program.Settings.JuryPanelColor.B),
                                                        new XAttribute("A", Program.Settings.JuryPanelColor.A)));

            settings.Add(new XElement("JuryPanelStrokeColor", new XAttribute("R", Program.Settings.JuryPanelStrokeColor.R),
                                                        new XAttribute("G", Program.Settings.JuryPanelStrokeColor.G),
                                                        new XAttribute("B", Program.Settings.JuryPanelStrokeColor.B),
                                                        new XAttribute("A", Program.Settings.JuryPanelStrokeColor.A)));

            settings.Add(new XElement("PointBarFontSize", new XAttribute("Value", Program.Settings.PointBarFontSize)));
            settings.Add(new XElement("PointBarPanelOpacity", new XAttribute("Value", Program.Settings.PointBarPanelOpacity)));

            settings.Add(new XElement("TopJuryInterval", new XAttribute("Value", Program.Settings.TopJuryInterval)));
            settings.Add(new XElement("JuryMemberOffset", new XAttribute("Value", Program.Settings.JuryMemberOffset)));
            settings.Add(new XElement("MemberInterval", new XAttribute("Value", Program.Settings.MemberInterval)));
            settings.Add(new XElement("MemberColumnInterval", new XAttribute("Value", Program.Settings.MemberColumnInterval)));

            settings.Add(new XElement("MemberPointsMode", new XAttribute("Value", (int)Program.Settings.MemberPointsMode)));
            settings.Add(new XElement("MemberSortingMode", new XAttribute("Value", (int)Program.Settings.MemberSortingMode)));
            settings.Add(new XElement("TrueTopRating", new XAttribute("Value", Program.Settings.TrueTopRating)));
            settings.Add(new XElement("StartJury", new XAttribute("Value", Program.Settings.StartJury)));
            settings.Add(new XElement("TwoColumns", new XAttribute("Value", Program.Settings.TwoColumns)));
            settings.Add(new XElement("MaxMembersInColumn", new XAttribute("Value", Program.Settings.MaxMembersInColumn)));
            settings.Add(new XElement("FinalPhrase", new XAttribute("Value", Program.Settings.FinalPhrase)));

            settings.Add(new XElement("ShowPointAnim", new XAttribute("Value", Program.Settings.ShowPointAnim)));
            settings.Add(new XElement("AnimatedBackground", new XAttribute("Value", Program.Settings.AnimatedBackground)));
            settings.Add(new XElement("VideoBackground", new XAttribute("Value", Program.Settings.VideoBackground)));
            settings.Add(new XElement("VideoPath", new XAttribute("Value", Program.Settings.VideoPath)));

            settings.Add(new XElement("AnimMoveTime", new XAttribute("Value", Program.Settings.AnimMoveTime.TotalMilliseconds)));
            settings.Add(new XElement("AnimAppearTime", new XAttribute("Value", Program.Settings.AnimAppearTime.TotalMilliseconds)));
            settings.Add(new XElement("AnimPause", new XAttribute("Value", Program.Settings.AnimPause.TotalMilliseconds)));
            settings.Add(new XElement("AnimPointBarPause", new XAttribute("Value", Program.Settings.AnimPointBarPause.TotalMilliseconds)));

            settings.Add(new XElement("BackgroundColor1", new XAttribute("R", Program.Settings.BackgroundColor1.R),
                                                        new XAttribute("G", Program.Settings.BackgroundColor1.G),
                                                        new XAttribute("B", Program.Settings.BackgroundColor1.B),
                                                        new XAttribute("A", Program.Settings.BackgroundColor1.A)));

            settings.Add(new XElement("BackgroundColor2", new XAttribute("R", Program.Settings.BackgroundColor2.R),
                                                        new XAttribute("G", Program.Settings.BackgroundColor2.G),
                                                        new XAttribute("B", Program.Settings.BackgroundColor2.B),
                                                        new XAttribute("A", Program.Settings.BackgroundColor2.A)));

            settings.Add(new XElement("BackgroundAnimPeriod", new XAttribute("Value", Program.Settings.BackgroundAnimPeriod.TotalMilliseconds)));
            settings.Add(new XElement("BackgroundAppearTime", new XAttribute("Value", Program.Settings.BackgroundAppearTime.TotalMilliseconds)));

            settings.Add(new XElement("LowerPhraseFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.LowerPhraseFontWeight))));
            settings.Add(new XElement("LowerPhraseFontColor", new XAttribute("A", Program.Settings.LowerPhraseFontColor.A),
                                                               new XAttribute("R", Program.Settings.LowerPhraseFontColor.R),
                                                               new XAttribute("G", Program.Settings.LowerPhraseFontColor.G),
                                                               new XAttribute("B", Program.Settings.LowerPhraseFontColor.B)));
            settings.Add(new XElement("LowerPhraseOffset", new XAttribute("Value", Program.Settings.LowerPhraseOffset)));
            settings.Add(new XElement("LowerPhraseFontSize", new XAttribute("Value", Program.Settings.LowerPhraseFontSize)));
            settings.Add(new XElement("LowerPhraseShowMode", new XAttribute("Value", (int)Program.Settings.LowerPhraseShowMode)));
            settings.Add(new XElement("LowerPhrase", new XAttribute("Value", Program.Settings.LowerPhrase)));

            settings.Add(new XElement("UseColorRanges", new XAttribute("Value", Program.Settings.UseColorRanges)));

            settings.Add(new XElement("ColorConfiguration",
                new XAttribute("Count", Program.Settings.ColorRangeList.Count),
                    Program.Settings.ColorRangeList
                        .Select(x => 
                            new XElement("ColorRange",
                                new XAttribute("Name", ((Viewer.ColorRange)x).Name),
                                new XAttribute("A", ((Viewer.ColorRange)x).CurrentColor.A),
                                new XAttribute("R", ((Viewer.ColorRange)x).CurrentColor.R),
                                new XAttribute("G", ((Viewer.ColorRange)x).CurrentColor.G),
                                new XAttribute("B", ((Viewer.ColorRange)x).CurrentColor.B),
                                new XAttribute("Count", ((Viewer.ColorRange)x).Count)
                                ))));

            settings.Add(new XElement("PointsColumnPhraseShowMode", new XAttribute("Value", (int)Program.Settings.PointsColumnPhraseShowMode)));
            settings.Add(new XElement("PointsColumnPhraseIsUnderlined", new XAttribute("Value", Program.Settings.PointsColumnPhraseIsUnderlined)));
            settings.Add(new XElement("PointsColumnPhraseFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.PointsColumnPhraseFontWeight))));
            settings.Add(new XElement("PointsColumnPhraseFontColor", new XAttribute("R", Program.Settings.PointsColumnPhraseFontColor.R),
                                                                     new XAttribute("G", Program.Settings.PointsColumnPhraseFontColor.G),
                                                                     new XAttribute("B", Program.Settings.PointsColumnPhraseFontColor.B),
                                                                     new XAttribute("A", Program.Settings.PointsColumnPhraseFontColor.A)));
            settings.Add(new XElement("PointsColumnPhraseFontSize", new XAttribute("Value", Program.Settings.PointsColumnPhraseFontSize)));
            settings.Add(new XElement("PointsColumnPhrase", new XAttribute("Value", Program.Settings.PointsColumnPhrase)));
            settings.Add(new XElement("PointsColumnPhraseXOffset", new XAttribute("Value", Program.Settings.PointsColumnPhraseXOffset)));
            settings.Add(new XElement("PointsColumnPhraseYOffset", new XAttribute("Value", Program.Settings.PointsColumnPhraseYOffset)));



            settings.Add(new XElement("ResultColumnPhraseShowMode", new XAttribute("Value", (int)Program.Settings.ResultColumnPhraseShowMode)));
            settings.Add(new XElement("ResultColumnPhraseIsUnderlined", new XAttribute("Value", Program.Settings.ResultColumnPhraseIsUnderlined)));
            settings.Add(new XElement("ResultColumnPhraseFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.ResultColumnPhraseFontWeight))));
            settings.Add(new XElement("ResultColumnPhraseFontColor", new XAttribute("R", Program.Settings.ResultColumnPhraseFontColor.R),
                                                                     new XAttribute("G", Program.Settings.ResultColumnPhraseFontColor.G),
                                                                     new XAttribute("B", Program.Settings.ResultColumnPhraseFontColor.B),
                                                                     new XAttribute("A", Program.Settings.ResultColumnPhraseFontColor.A)));
            settings.Add(new XElement("ResultColumnPhraseFontSize", new XAttribute("Value", Program.Settings.ResultColumnPhraseFontSize)));
            settings.Add(new XElement("ResultColumnPhrase", new XAttribute("Value", Program.Settings.ResultColumnPhrase)));
            settings.Add(new XElement("ResultColumnPhraseXOffset", new XAttribute("Value", Program.Settings.ResultColumnPhraseXOffset)));
            settings.Add(new XElement("ResultColumnPhraseYOffset", new XAttribute("Value", Program.Settings.ResultColumnPhraseYOffset)));


            
            settings.Add(new XElement("PlaceColumnPhraseShowMode", new XAttribute("Value", (int)Program.Settings.PlaceColumnPhraseShowMode)));
            settings.Add(new XElement("PlaceColumnPhraseIsUnderlined", new XAttribute("Value", Program.Settings.PlaceColumnPhraseIsUnderlined)));
            settings.Add(new XElement("PlaceColumnPhraseFontWeight", new XAttribute("Value", ViewerSettings.ConvertFontWeightToIndex(Program.Settings.PlaceColumnPhraseFontWeight))));
            settings.Add(new XElement("PlaceColumnPhraseFontColor", new XAttribute("R", Program.Settings.PlaceColumnPhraseFontColor.R),
                                                                     new XAttribute("G", Program.Settings.PlaceColumnPhraseFontColor.G),
                                                                     new XAttribute("B", Program.Settings.PlaceColumnPhraseFontColor.B),
                                                                     new XAttribute("A", Program.Settings.PlaceColumnPhraseFontColor.A)));
            settings.Add(new XElement("PlaceColumnPhraseFontSize", new XAttribute("Value", Program.Settings.PlaceColumnPhraseFontSize)));
            settings.Add(new XElement("PlaceColumnPhrase", new XAttribute("Value", Program.Settings.PlaceColumnPhrase)));
            settings.Add(new XElement("PlaceColumnPhraseXOffset", new XAttribute("Value", Program.Settings.PlaceColumnPhraseXOffset)));
            settings.Add(new XElement("PlaceColumnPhraseYOffset", new XAttribute("Value", Program.Settings.PlaceColumnPhraseYOffset)));
            settings.Add(new XElement("ShowAverageResults", new XAttribute("Value", Program.Settings.ShowAverageResults)));

            #endregion

            data.Add(jurys);        //<
            data.Add(members);      //<
            data.Add(oldSettings);  // Собираем файл xml
            data.Add(settings);     //<
            xdoc.Add(data);         //<
            StreamWriter writingFile = new StreamWriter(FilePath);
            xdoc.Save(writingFile);  //Сохранение файла
            writingFile.Dispose();
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
                        throw new FormatException("Ошибка во время чтения файла. Возможно файл повреждён");
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
                        throw new FormatException("Ошибка во время чтения файла. Возможно файл повреждён");
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
                OldSettingsProvider osp = Program.Settings.OldSettings;

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

                // Заранее объявим элемент, в котором будет временно храниться считываемый нод, чтобы можно было вычислить, где произошла ошибка
                XmlNode readingElement = null;

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

                    #region IO functions

                    XmlNode oldSettings = xRoot.SelectSingleNode("Old_Settings");

                    int tempIntVar;
                    double tempDoubleVar;
                    byte tempByteVar;
                    bool tempBoolVar;

                    Func<string, int> getInt = (name) =>
                    {
                        if (int.TryParse(readingElement.Attributes.GetNamedItem(name).Value, out tempIntVar))
                            return tempIntVar;
                        else
                            throw new FormatException("Не получилось сконвертировать данный элемент в тип int");
                    };

                    Func<string, string> getStr = (name) =>
                    {
                        return readingElement.Attributes.GetNamedItem(name).Value;
                    };

                    Func<string, double> getDouble = (name) =>
                    {
                        if (double.TryParse(readingElement.Attributes.GetNamedItem(name).Value, out tempDoubleVar))
                            return tempDoubleVar;
                        else
                            throw new FormatException("Не получилось сконвертировать данный элемент в тип double");
                    };

                    Func<string, byte> getByte = (name) =>
                    {
                        if (byte.TryParse(readingElement.Attributes.GetNamedItem(name).Value, out tempByteVar))
                            return tempByteVar;
                        else
                            throw new FormatException("Не получилось сконвертировать данный элемент в тип byte");
                    };

                    Func<string, bool> getBool = (name) =>
                    {
                        if (bool.TryParse(readingElement.Attributes.GetNamedItem(name).Value, out tempBoolVar))
                            return tempBoolVar;
                        else
                            throw new FormatException("Не получилось сконвертировать данный элемент в тип bool");
                    };

                    StringBuilder errorList = new StringBuilder();

                    Func<XmlNode, string, bool> tryGetSingleNode = (parentNode, nodeName) =>
                    {
                        try
                        {
                            readingElement = parentNode.SelectSingleNode(nodeName) ?? throw new NullReferenceException();
                            return true;
                        }
                        catch (Exception)
                        {
                            errorList.Append($"{nodeName}\n");
                            return false;
                        }
                    };

                    #endregion

                    #region Get OldSettings

                    if (tryGetSingleNode(oldSettings, "ContBarColor"))
                        Program.Settings.OldSettings.ContBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    if (tryGetSingleNode(oldSettings, "sndContBarColor"))
                        Program.Settings.OldSettings.sndContBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    if (tryGetSingleNode(oldSettings, "ContBarFontSize"))
                        Program.Settings.OldSettings.ContBarFontSize = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "ContBarWidth"))
                        Program.Settings.OldSettings.ContBarWidth = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "ContBarHeight"))
                        Program.Settings.OldSettings.ContBarHeight = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "XNum"))
                        Program.Settings.OldSettings.XNum = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "YNum"))
                        Program.Settings.OldSettings.YNum = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "FrameRate"))
                        Program.Settings.OldSettings.FrameRate = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "FrameInterval"))
                        Program.Settings.OldSettings.FrameInterval = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "QuitFrase"))
                        Program.Settings.OldSettings.QuitFrase = getStr("Value");

                    if (tryGetSingleNode(oldSettings, "PointBarColor"))
                        Program.Settings.OldSettings.PointBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    if (tryGetSingleNode(oldSettings, "sndPointBarColor"))
                        Program.Settings.OldSettings.sndPointBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    if (tryGetSingleNode(oldSettings, "pointBarWidth"))
                        Program.Settings.OldSettings.pointBarWidth = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "pointBarHeight"))
                        Program.Settings.OldSettings.pointBarHeight = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "pointBarInterval"))
                        Program.Settings.OldSettings.pointBarInterval = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "pointBarFontSize"))
                        Program.Settings.OldSettings.pointBarFontSize = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "JuryBarColor"))
                        Program.Settings.OldSettings.JuryBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    if (tryGetSingleNode(oldSettings, "sndJuryBarColor"))
                        Program.Settings.OldSettings.sndJuryBarColor = Color.FromArgb(getInt("R"), getInt("G"), getInt("B"), getInt("A"));

                    if (tryGetSingleNode(oldSettings, "JuryBarWidth"))
                        Program.Settings.OldSettings.JuryBarWidth = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "JuryBarHeight"))
                        Program.Settings.OldSettings.JuryBarHeight = getInt("Value");

                    if (tryGetSingleNode(oldSettings, "JuryBarFontSize"))
                        Program.Settings.OldSettings.JuryBarFontSize = getInt("Value");

                    #endregion

                    #region Settings

                    XmlNode settings = xRoot.SelectSingleNode("Settings");

                    if (tryGetSingleNode(settings, "MemberPanelWidth"))
                         Program.Settings.MemberPanelWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPanelHeight"))
                        Program.Settings.MemberPanelHeight = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberNameFontSize"))
                        Program.Settings.MemberNameFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPanelStrokeWidth"))
                        Program.Settings.MemberPanelStrokeWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPanelOpacity"))
                        Program.Settings.MemberPanelOpacity = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPanelColor"))
                        Program.Settings.MemberPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelChosenColor"))
                        Program.Settings.MemberPanelChosenColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelChosenColor2"))
                        Program.Settings.MemberPanelChosenColor2 = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelStrokeColor"))
                        Program.Settings.MemberPanelStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelHighlightLeaders"))
                        Program.Settings.MemberPanelHighlightLeaders = getBool("Value");

                    if (tryGetSingleNode(settings, "MemberPanelFirstPlace"))
                        Program.Settings.MemberPanelFirstPlace = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelSecondPlace"))
                        Program.Settings.MemberPanelSecondPlace = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelSecondPlace"))
                        Program.Settings.MemberPanelSecondPlace = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelOtherPlaces"))
                        Program.Settings.MemberPanelOtherPlaces = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPanelUseSecondChooseColor"))
                        Program.Settings.MemberPanelUseSecondChooseColor = getBool("Value");

                    if (tryGetSingleNode(settings, "MemberNameFontWeight"))
                        Program.Settings.MemberNameFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "MemberNameFontColor"))
                        Program.Settings.MemberNameFontColor = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"))));

                    if (tryGetSingleNode(settings, "MemberPointsFontWeight"))
                        Program.Settings.MemberPointsFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "MemberPointsFontSize"))
                        Program.Settings.MemberPointsFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPointsPanelHeight"))
                        Program.Settings.MemberPointsPanelHeight = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPointsPanelWidth"))
                        Program.Settings.MemberPointsPanelWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPointsPanelColor"))
                        Program.Settings.MemberPointsPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPointsStrokeColor"))
                        Program.Settings.MemberPointsStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPointsFontColor"))
                        Program.Settings.MemberPointsFontColor = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"))));

                    if (tryGetSingleNode(settings, "MemberPointsStrokeWidth"))
                        Program.Settings.MemberPointsStrokeWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPlaceFontColor"))
                        Program.Settings.MemberPlaceFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPlaceFontSize"))
                        Program.Settings.MemberPlaceFontSize = getInt("Value");

                    if (tryGetSingleNode(settings, "MemberPlaceFontWeight"))
                        Program.Settings.MemberPlaceFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "MemberPlaceShowMode"))
                        Program.Settings.MemberPlaceShowMode = (Program.Settings.PlaceShowMode)getInt("Value");

                    if (tryGetSingleNode(settings, "MemberPlaceStrokeWidth"))
                        Program.Settings.MemberPlaceStrokeWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberPlacePanelColor"))
                        Program.Settings.MemberPlacePanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberPlaceStrokeColor"))
                        Program.Settings.MemberPlaceStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "ShowMemberResultMode"))
                        Program.Settings.ShowMemberResultMode = (Program.Settings.ResultShowMode)getInt("Value");

                    if (tryGetSingleNode(settings, "MemberResultPanelWidth"))
                        Program.Settings.MemberResultPanelWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberResultPanelHeight"))
                        Program.Settings.MemberResultPanelHeight = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberResultFontSize"))
                        Program.Settings.MemberResultFontSize = getInt("Value");

                    if (tryGetSingleNode(settings, "MemberResultPanelOffset"))
                        Program.Settings.MemberResultPanelOffset = getInt("Value");

                    if (tryGetSingleNode(settings, "MemberResultStrokeWidth"))
                        Program.Settings.MemberResultStrokeWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "MemberResultFontWeight"))
                        Program.Settings.MemberResultFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "MemberResultFontColor"))
                        Program.Settings.MemberResultFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberResultPanelColor"))
                        Program.Settings.MemberResultPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "MemberResultStrokeColor"))
                        Program.Settings.MemberResultStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "JuryPanelWidth"))
                        Program.Settings.JuryPanelWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "JuryPanelHeight"))
                        Program.Settings.JuryPanelHeight = getDouble("Value");

                    if (tryGetSingleNode(settings, "JuryPanelOpacity"))
                        Program.Settings.JuryPanelOpacity = getDouble("Value");
                    
                    if (tryGetSingleNode(settings, "JuryFontWeight"))
                        Program.Settings.JuryFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "JuryPanelStrokeWidth"))
                        Program.Settings.JuryPanelStrokeWidth = getDouble("Value");

                    if (tryGetSingleNode(settings, "JuryFontSize"))
                        Program.Settings.JuryFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "JuryPanelColor"))
                        Program.Settings.JuryPanelColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "JuryPanelStrokeColor"))
                        Program.Settings.JuryPanelStrokeColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "PointBarFontSize"))
                        Program.Settings.PointBarFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "PointBarPanelOpacity"))
                        Program.Settings.PointBarPanelOpacity = getDouble("Value");

                    if (tryGetSingleNode(settings, "TopJuryInterval"))
                        Program.Settings.TopJuryInterval = getInt("Value");

                    if (tryGetSingleNode(settings, "JuryMemberOffset"))
                        Program.Settings.JuryMemberOffset = getInt("Value");

                    if (tryGetSingleNode(settings, "MemberInterval"))
                        Program.Settings.MemberInterval = getInt("Value");

                    if (tryGetSingleNode(settings, "MemberColumnInterval"))
                        Program.Settings.MemberColumnInterval = getInt("Value");

                    if (tryGetSingleNode(settings, "MemberPointsMode"))
                        Program.Settings.MemberPointsMode = (Program.Settings.PointsMode)getInt("Value");

                    if (tryGetSingleNode(settings, "MemberSortingMode"))
                        Program.Settings.MemberSortingMode = (Program.Settings.SortingMode)getInt("Value");

                    if (tryGetSingleNode(settings, "TrueTopRating"))
                        Program.Settings.TrueTopRating = getBool("Value");

                    if (tryGetSingleNode(settings, "StartJury"))
                        Program.Settings.StartJury = getInt("Value");

                    if (tryGetSingleNode(settings, "TwoColumns"))
                        Program.Settings.TwoColumns = getBool("Value");

                    if (tryGetSingleNode(settings, "MaxMembersInColumn"))
                        Program.Settings.MaxMembersInColumn = getInt("Value");

                    if (tryGetSingleNode(settings, "FinalPhrase"))
                        Program.Settings.FinalPhrase = getStr("Value");

                    if (tryGetSingleNode(settings, "ShowPointAnim"))
                        Program.Settings.ShowPointAnim = getBool("Value");

                    if (tryGetSingleNode(settings, "AnimatedBackground"))
                        Program.Settings.AnimatedBackground = getBool("Value");

                    if (tryGetSingleNode(settings, "VideoBackground"))
                        Program.Settings.VideoBackground = getBool("Value");

                    if (tryGetSingleNode(settings, "VideoPath"))
                        Program.Settings.VideoPath = getStr("Value");

                    if (tryGetSingleNode(settings, "AnimMoveTime"))
                        Program.Settings.AnimMoveTime = TimeSpan.FromMilliseconds(getDouble("Value"));

                    if (tryGetSingleNode(settings, "AnimAppearTime"))
                        Program.Settings.AnimAppearTime = TimeSpan.FromMilliseconds(getDouble("Value"));

                    if (tryGetSingleNode(settings, "AnimPause"))
                        Program.Settings.AnimPause = TimeSpan.FromMilliseconds(getDouble("Value"));

                    if (tryGetSingleNode(settings, "AnimPointBarPause"))
                        Program.Settings.AnimPointBarPause = TimeSpan.FromMilliseconds(getDouble("Value"));

                    if (tryGetSingleNode(settings, "BackgroundColor1"))
                        Program.Settings.BackgroundColor1 = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "BackgroundColor2"))
                        Program.Settings.BackgroundColor2 = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "BackgroundAnimPeriod"))
                        Program.Settings.BackgroundAnimPeriod = TimeSpan.FromMilliseconds(getDouble("Value"));

                    if (tryGetSingleNode(settings, "BackgroundAppearTime"))
                        Program.Settings.BackgroundAppearTime = TimeSpan.FromMilliseconds(getDouble("Value"));

                    if (tryGetSingleNode(settings, "LowerPhraseFontWeight"))
                        Program.Settings.LowerPhraseFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "LowerPhraseFontColor"))
                        Program.Settings.LowerPhraseFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "LowerPhraseOffset"))
                        Program.Settings.LowerPhraseOffset = getDouble("Value");

                    if (tryGetSingleNode(settings, "LowerPhraseFontSize"))
                        Program.Settings.LowerPhraseFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "LowerPhraseShowMode"))
                        Program.Settings.LowerPhraseShowMode = (Program.Settings.ShowMode)getInt("Value");

                    if (tryGetSingleNode(settings, "LowerPhrase"))
                        Program.Settings.LowerPhrase = getStr("Value");

                    if (tryGetSingleNode(settings, "UseColorRanges"))
                        Program.Settings.UseColorRanges = getBool("Value");

                    if (tryGetSingleNode(settings, "ColorConfiguration"))
                    {
                        Program.Settings.ColorRangeList = new LinkedList<Viewer.ColorRange>();

                        // Сохраняем ссылку на общий список всех настроек цветов
                        XmlNode loadingRangeList = readingElement;
                        foreach (XmlNode rangeColor in loadingRangeList)
                        {
                            readingElement = rangeColor;
                            Viewer.ColorRange loadingRange = new Viewer.ColorRange(getStr("Name"), 
                                                                                   getInt("Count"),
                                                                                   System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B")));

                            Program.Settings.ColorRangeList.AddLast(loadingRange);
                        }
                    }

                    if (tryGetSingleNode(settings, "PointsColumnPhraseShowMode"))
                        Program.Settings.PointsColumnPhraseShowMode = (Program.Settings.PhraseShowMode)getInt("Value");

                    if (tryGetSingleNode(settings, "PointsColumnPhraseIsUnderlined"))
                        Program.Settings.PointsColumnPhraseIsUnderlined = getBool("Value");

                    if (tryGetSingleNode(settings, "PointsColumnPhraseFontWeight"))
                        Program.Settings.PointsColumnPhraseFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "PointsColumnPhraseFontColor"))
                        Program.Settings.PointsColumnPhraseFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "PointsColumnPhraseFontSize"))
                        Program.Settings.PointsColumnPhraseFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "PointsColumnPhrase"))
                        Program.Settings.PointsColumnPhrase = getStr("Value");

                    if (tryGetSingleNode(settings, "PointsColumnPhraseXOffset"))
                        Program.Settings.PointsColumnPhraseXOffset = getDouble("Value");

                    if (tryGetSingleNode(settings, "PointsColumnPhraseYOffset"))
                        Program.Settings.PointsColumnPhraseYOffset = getDouble("Value");

                    if (tryGetSingleNode(settings, "ResultColumnPhraseShowMode"))
                        Program.Settings.ResultColumnPhraseShowMode = (Program.Settings.PhraseShowMode)getInt("Value");

                    if (tryGetSingleNode(settings, "ResultColumnPhraseIsUnderlined"))
                        Program.Settings.ResultColumnPhraseIsUnderlined = getBool("Value");

                    if (tryGetSingleNode(settings, "ResultColumnPhraseFontWeight"))
                        Program.Settings.ResultColumnPhraseFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "ResultColumnPhraseFontColor"))
                        Program.Settings.ResultColumnPhraseFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "ResultColumnPhraseFontSize"))
                        Program.Settings.ResultColumnPhraseFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "ResultColumnPhrase"))
                        Program.Settings.ResultColumnPhrase = getStr("Value");

                    if (tryGetSingleNode(settings, "ResultColumnPhraseXOffset"))
                        Program.Settings.ResultColumnPhraseXOffset = getDouble("Value");

                    if (tryGetSingleNode(settings, "ResultColumnPhraseYOffset"))
                        Program.Settings.ResultColumnPhraseYOffset = getDouble("Value");

                    if (tryGetSingleNode(settings, "PlaceColumnPhraseShowMode"))
                        Program.Settings.PlaceColumnPhraseShowMode = (Program.Settings.PhraseShowMode)getInt("Value");

                    if (tryGetSingleNode(settings, "PlaceColumnPhraseIsUnderlined"))
                        Program.Settings.PlaceColumnPhraseIsUnderlined = getBool("Value");

                    if (tryGetSingleNode(settings, "PlaceColumnPhraseFontWeight"))
                        Program.Settings.PlaceColumnPhraseFontWeight = ViewerSettings.ConvertIndexToFontWeight(getInt("Value"));

                    if (tryGetSingleNode(settings, "PlaceColumnPhraseFontColor"))
                        Program.Settings.PlaceColumnPhraseFontColor = System.Windows.Media.Color.FromArgb(getByte("A"), getByte("R"), getByte("G"), getByte("B"));

                    if (tryGetSingleNode(settings, "PlaceColumnPhraseFontSize"))
                        Program.Settings.PlaceColumnPhraseFontSize = getDouble("Value");

                    if (tryGetSingleNode(settings, "PlaceColumnPhrase"))
                        Program.Settings.PlaceColumnPhrase = getStr("Value");

                    if (tryGetSingleNode(settings, "PlaceColumnPhraseXOffset"))
                        Program.Settings.PlaceColumnPhraseXOffset = getDouble("Value");

                    if (tryGetSingleNode(settings, "PlaceColumnPhraseYOffset"))
                        Program.Settings.PlaceColumnPhraseYOffset = getDouble("Value");

                    if (tryGetSingleNode(settings, "ShowAverageResults"))
                        Program.Settings.ShowAverageResults = getBool("Value");

                    #endregion

                    if (errorList.Length != 0)
                        MessageBox.Show($"Возникли некоторые ошибки во время чтения файла. Данные участников и жюри успешно считаны, но не получилось получить следующие настройки: {errorList.ToString()}", "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (FormatException fx)
                {
                    throw new FormatException($"Ошибка во время преобразования типов в элементе: {readingElement.InnerText}\n\nException data: {fx.Data}\nStackTrace: {fx.StackTrace}");
                }
                catch (Exception x)
                {
                    throw new Exception($"Неожиданная ошибка во время чтения файла\n\nСчитываемый элемент: {readingElement.InnerText}\nException data: {x.Data}\nStackTrace: {x.StackTrace}");
                }
            }
        }
    }
}