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
using System.Drawing;

namespace ResultViewerWPF.Compitability.Primitives
{
    class JuryBar
    {
        #region Fields

        private int xCord;  //xy coordinates on screen
        private int yCord;

        private Color BarColor;
        private Color FontColor;
        
        private int barWidth = 720;
        private int barHeight = 75;
        private int fontSize = 20;

        string JuryName; //name of the contest

        #endregion
                
        #region Overloaded Contructors

        public JuryBar(Point location, string JuryName, int FontSize, int BarWidth, int BarHeight, Color BarColor)
        {
            this.xCord = location.X;
            this.yCord = location.Y;
            this.fontSize = FontSize;
            this.barWidth = BarWidth;
            this.barHeight = BarHeight;
            this.JuryName = JuryName;
            this.BarColor = BarColor;
            this.FontColor = Color.FromArgb(255, 255, 255, 255);
        }
        

        public JuryBar(string JuryName)
        {
            this.xCord = 1;
            this.yCord = 1;
            this.JuryName = JuryName;
        }

        public JuryBar(int xCord, int yCord, string JuryName)
        {
            this.xCord = xCord;
            this.yCord = yCord;
            this.JuryName = JuryName;
        }

        #endregion 

        public void SetCord(Point location)
        {
            xCord = location.X;
            yCord = location.Y;
        }

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

        public void SetColor(Color color)
        {
            BarColor = Color.FromArgb(255, color.R, color.G, color.B);
        }
        
        public void SetName(string s)
        {
            JuryName = s;
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

        public void SetAlpha(int A)
        {
            BarColor = Color.FromArgb(A, BarColor.R, BarColor.G, BarColor.B);
            FontColor = Color.FromArgb(A, 255, 255, 255);
        }

        public void Draw(Graphics g)
        {
            string juryBarText = $"{ JuryName }";


            Rectangle juryBarBorder = new Rectangle(xCord,
                                                    yCord,
                                                    barWidth,
                                                    barHeight);

            Font juryNameFont = new Font("Calibri",
                                            fontSize,
                                            FontStyle.Regular,
                                            GraphicsUnit.Pixel);

            SolidBrush fontSolidBrush = new SolidBrush(FontColor);
            SolidBrush juryBarSolidBrush = new SolidBrush(BarColor);

            StringFormat contestNameFormat = new StringFormat();
            contestNameFormat.Alignment = StringAlignment.Center;
            contestNameFormat.LineAlignment = StringAlignment.Center;

            g.FillRectangle(juryBarSolidBrush, juryBarBorder);

            g.DrawString(juryBarText,
                         juryNameFont,
                         fontSolidBrush,
                         juryBarBorder,
                         contestNameFormat);


        }
    }
}
