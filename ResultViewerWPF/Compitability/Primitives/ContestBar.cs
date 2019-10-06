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
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultViewerWPF.Compitability.Primitives
{
    class ContestBar
    {

        //ContBarColor 255 26 43 63
        //sndContBarColor 255 203 12 179



        #region Fields

        private int xCord;  //xy coordinates on screen
                private int yCord;

                Color BarColor;
                Color FontColor;

                private int barWidth = 720;
                private int barHeight = 75;
                private int fontSize = 20;
                private int numOfPoints = 0; //Number of gained points
                string contestName; //name of the contest

        #endregion

        #region Overloaded Contructors

        public ContestBar(Point location, string ContestName, int FontSize, int BarWidth, int BarHeight, Color BarColor)
        {
            this.xCord = location.X;
            this.yCord = location.Y;
            this.fontSize = FontSize;
            this.barWidth = BarWidth;
            this.barHeight = BarHeight;
            this.contestName = ContestName;
            this.BarColor = BarColor;
            this.FontColor = Color.FromArgb(255, 255, 255, 255);
        }
        public ContestBar(int xCord, int yCord, string ContestName)
        {
            this.xCord = xCord;
            this.yCord = yCord;
            this.contestName = ContestName;
            this.FontColor = Color.FromArgb(255, 255, 255, 255);

        }
        public ContestBar(string ContestName)
        {
            this.xCord = 1;
            this.yCord = 1;
            this.contestName = ContestName;
            this.FontColor = Color.FromArgb(255, 255, 255, 255);

        }
        public ContestBar(Point location, string ContestName)
        {
            this.xCord = location.X;
            this.yCord = location.Y;
            this.contestName = ContestName;
            this.FontColor = Color.FromArgb(255, 255, 255, 255);
        }
        #endregion

        #region Procedures

        public void MoveAlpha(int A)
        {
            BarColor = Color.FromArgb((BarColor.A + A > 255) || (BarColor.A + A < 0) ? BarColor.A : BarColor.A + A,
                                      BarColor.R,
                                      BarColor.G, 
                                      BarColor.B);
            FontColor = Color.FromArgb((FontColor.A + A > 255) || (FontColor.A + A < 0) ? FontColor.A : FontColor.A + A,
                                       255,
                                       255,
                                       255);
        }

        public void SetCord(Point location)
        {
            xCord = location.X;
            yCord = location.Y;
        }

        public string GetContestName()
        {
            return contestName;
        }

        public int GetNumOfPoints()
        {
            return numOfPoints;
        }

        public Point GetCord()
        {
            return new Point(xCord, yCord);
        }

        public void SetAlpha(int A)
        {
            BarColor = Color.FromArgb(A, BarColor.R, BarColor.G, BarColor.B);
        }
        
        public void MoveColor(int aStep, int rStep, int gStep, int bStep)
        {
            BarColor = Color.FromArgb((BarColor.A + aStep > 255) || (BarColor.A + aStep < 0) ? BarColor.A : BarColor.A + aStep,
                                      (BarColor.R + rStep > 255) || (BarColor.R + rStep < 0) ? BarColor.R : BarColor.R + rStep,
                                      (BarColor.G + gStep > 255) || (BarColor.G + gStep < 0) ? BarColor.G : BarColor.G + gStep,
                                      (BarColor.B + bStep > 255) || (BarColor.B + bStep < 0) ? BarColor.B : BarColor.B + bStep);
            FontColor = Color.FromArgb((FontColor.A + aStep > 255) || (FontColor.A + aStep < 0) ? FontColor.A : FontColor.A + aStep,
                                        FontColor.R,
                                        FontColor.G,
                                        FontColor.B);
            
        }

        public void SetColor(int A, int R, int G, int B)
        {
            BarColor = Color.FromArgb(A, R, G, B);
            FontColor = Color.FromArgb(A,
                                       FontColor.R,
                                       FontColor.G,
                                       FontColor.B);
        }

        public void SetColor(Color cl)
        {
            BarColor = cl;
        }

        public string NumOfPoints
        {
            get
            {
                if (numOfPoints > 9)
                {
                    return numOfPoints.ToString();
                }
                else
                {
                    return $"0{ numOfPoints.ToString() }";
                }
            }
            set { }

        }

        public void AddPoints(int i)
        {
            numOfPoints += i;
        }

        #endregion

        public void Draw(Graphics g)
        {
            string contestBarText = $"{ NumOfPoints } | { contestName }";           


            Rectangle contestBarBorder = new Rectangle( xCord, 
                                                        yCord,
                                                        barWidth, 
                                                        barHeight);

            Font contestNameFont = new Font("Calibri", 
                                            fontSize, 
                                            FontStyle.Regular, 
                                            GraphicsUnit.Pixel);

            SolidBrush contestBarSolidBrush = new SolidBrush(BarColor);
            SolidBrush fontSolidBrush = new SolidBrush(FontColor);

            StringFormat contestNameFormat = new StringFormat();
            contestNameFormat.Alignment = StringAlignment.Near;
            contestNameFormat.LineAlignment = StringAlignment.Center;

            g.FillRectangle(contestBarSolidBrush, contestBarBorder);

            g.DrawString(contestBarText, 
                         contestNameFont, 
                         fontSolidBrush,
                         contestBarBorder, 
                         contestNameFormat);


        }

        //public Bitmap GetBitmap()
        //{
        //    Bitmap contestBarBitmap = new Bitmap(Properties.Resources.contestBarTexture);
        //    Graphics g = Graphics.FromImage(new Bitmap(contestBarBitmap.Width, contestBarBitmap.Height));

        //    Rectangle contestBarBorder = new Rectangle( 1,
        //                                                1,
        //                                                contestBarBitmap.Width,
        //                                                contestBarBitmap.Height);

        //    Font contestNameFont = new Font("Calibri",
        //                                    fontSize,
        //                                    FontStyle.Regular,
        //                                    GraphicsUnit.Pixel);

        //    TextureBrush contestBarTextureBrush = new TextureBrush(new Bitmap(Properties.Resources.contestBarTexture));

        //    StringFormat contestNameFormat = new StringFormat();
        //    contestNameFormat.Alignment = StringAlignment.Near;
        //    contestNameFormat.LineAlignment = StringAlignment.Center;

        //    g.FillRectangle(contestBarTextureBrush, contestBarBorder);
            
        //    string contestBarText = $"{ NumOfPoints } | { contestName }";

        //    g.DrawString(contestBarText,
        //                 contestNameFont,
        //                 Brushes.White,
        //                 contestBarBorder,
        //                 contestNameFormat);

            //g.DrawString(contestBarText,
            //             contestNameFont,
            //             Brushes.White,
            //             new Point(1, 1), 
            //             contestNameFormat);
        
        //    return contestBarBitmap;


        //}
    }
}