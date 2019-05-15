using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ResultViewerWPF.Compitability.Primitives
{
    class PointBar
    {
        //from Point1 to point 10 color is 91 218 211
        //Point12 color is 203 12 179

        //PointBarWidth 43
        //PointBarHeight 27
        //PointBarInterval 10

        #region Fields

        private int xCord;
        private int yCord;
        private int NumOfPoint;
        private Color PointBarColor;        
        private Color FontColor;
        private int PointBarWidth = 43;
        private int PointBarHeight = 27;
        private int PointBarFontSize = 20;

        #endregion

        #region Contructor

        public PointBar(Point Location, int NumOfPoint, int FontSize)
        {
            if ((NumOfPoint < 1) ||(NumOfPoint > 10))
                throw new ArgumentException("Number is out of range");

            this.PointBarFontSize = FontSize;
            if (NumOfPoint < 9)
            {
                this.NumOfPoint = NumOfPoint;
                PointBarColor = Color.FromArgb(255, 91, 218, 211);
            }
            else
            if (NumOfPoint == 9)
            {
                this.NumOfPoint = NumOfPoint + 1;
                PointBarColor = Color.FromArgb(255, 91, 218, 211);
            }
            else
            if (NumOfPoint == 10)
            {
                this.NumOfPoint = NumOfPoint + 2;
                PointBarColor = Color.FromArgb(255, 203, 12, 179);
            }
            FontColor = Color.FromArgb(255, 255, 255, 255);
        }

        public PointBar(Point location, int NumOfPoint, int FontSize, int BarWidth, int BarHeight, Color BarColor, Color sndBarColor)
        {
            xCord = location.X;
            yCord = location.Y;

            if ((NumOfPoint < 1) || (NumOfPoint > 10))
                throw new ArgumentException("Number is out of range");

            this.PointBarFontSize = FontSize;
            if (NumOfPoint < 9)
            {
                this.NumOfPoint = NumOfPoint;
                PointBarColor = BarColor;
            }
            else
            if (NumOfPoint == 9)
            {
                this.NumOfPoint = NumOfPoint + 1;
                PointBarColor = BarColor;
            }
            else
            if (NumOfPoint == 10)
            {
                this.NumOfPoint = NumOfPoint + 2;
                PointBarColor = sndBarColor;
            }
            PointBarWidth = BarWidth;
            PointBarHeight = BarHeight;
            FontColor = Color.FromArgb(255, 255, 255, 255);
        }

        #endregion

        #region Procedures

        public void MoveAlpha(int A)
        {
            PointBarColor = Color.FromArgb((PointBarColor.A + A > 255) || (PointBarColor.A + A < 0) ? PointBarColor.A : PointBarColor.A + A,
                                           PointBarColor.R,
                                           PointBarColor.G, 
                                           PointBarColor.B);
            FontColor = Color.FromArgb((FontColor.A + A > 255) || (FontColor.A + A < 0) ? FontColor.A : FontColor.A + A,
                                       255,
                                       255,
                                       255);
        }

        public void SetAlpha(int A)
        {
            PointBarColor = Color.FromArgb(A, PointBarColor.R, PointBarColor.G, PointBarColor.B);
            FontColor = Color.FromArgb(A, FontColor.R, FontColor.G, FontColor.B);
        }

        public void SetCord(Point location)
        {
            xCord = location.X;
            yCord = location.Y;
        }

        public void SetColor(Color color)
        {
            PointBarColor = Color.FromArgb(255, color.R, color.G, color.B);
        }


        public void MovePoint(int xStep, int yStep)
        {
            xCord += xStep;
            yCord += yStep;
        }

        public void MoveColor(int aStep, int rStep, int gStep, int bStep)
        {
            PointBarColor = Color.FromArgb((PointBarColor.A + aStep > 255) || (PointBarColor.A + aStep < 0) ? PointBarColor.A : PointBarColor.A + aStep,
                                           (PointBarColor.R + rStep > 255) || (PointBarColor.R + rStep < 0) ? PointBarColor.R : PointBarColor.R + rStep,
                                           (PointBarColor.G + gStep > 255) || (PointBarColor.G + gStep < 0) ? PointBarColor.G : PointBarColor.G + gStep,
                                           (PointBarColor.B + bStep > 255) || (PointBarColor.B + bStep < 0) ? PointBarColor.B : PointBarColor.B + bStep);
            FontColor = Color.FromArgb((FontColor.A + aStep > 255) || (FontColor.A + aStep < 0) ? FontColor.A : FontColor.A + aStep,
                                        FontColor.R,
                                        FontColor.G,
                                        FontColor.B);
        }

        public int GetNumOfPoints()
        {
            return NumOfPoint;
        }

        public Point GetCords()
        {
            return new Point(xCord, yCord);
        }

        #endregion

        public void Draw(Graphics g)
        {
            string pointBarText = $"{NumOfPoint}";

            Rectangle pointBarBorder = new Rectangle(xCord,
                                                     yCord,
                                                     PointBarWidth,
                                                     PointBarHeight);

            Font pointBarFont = new Font("Arial",
                                          PointBarFontSize,
                                          FontStyle.Bold,
                                          GraphicsUnit.Pixel);

            SolidBrush pointBarSolidBrush = new SolidBrush(PointBarColor);
            SolidBrush fontSolidBrush = new SolidBrush(FontColor);

            StringFormat pointBarStringFormat = new StringFormat();
            pointBarStringFormat.Alignment = StringAlignment.Center;
            pointBarStringFormat.LineAlignment = StringAlignment.Center;

            g.FillRectangle(pointBarSolidBrush, pointBarBorder);

            g.DrawString(pointBarText, 
                         pointBarFont, 
                         fontSolidBrush, 
                         pointBarBorder, 
                         pointBarStringFormat);


        }



    }
}
