using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ResultViewerWPF
{
    public class OldSettingsProvider
    {
        public Color ContBarColor = Color.FromArgb(255, 26, 43, 63);
        public Color sndContBarColor = Color.FromArgb(255, 203, 12, 179);
        public int ContBarFontSize = 20;
        public int ContBarWidth = 405;
        public int ContBarHeight = 35;

        public int XNum = 2;
        public int YNum = 20;
        public int FrameRate = 10;
        public int FrameInterval = 20;
        public string QuitFrase = "Показ завершён";

        public Color PointBarColor = Color.FromArgb(255, 91, 218, 211);
        public Color sndPointBarColor = Color.FromArgb(255, 203, 12, 179);
        public int pointBarWidth = 43;
        public int pointBarHeight = 27;
        public int pointBarInterval = 10;
        public int pointBarFontSize = 20;

        public Color JuryBarColor = Color.FromArgb(255, 26, 43, 63);
        public Color sndJuryBarColor = Color.FromArgb(255, 203, 12, 179);
        public int JuryBarWidth = 405;
        public int JuryBarHeight = 35;
        public int JuryBarFontSize = 20;
    }
}
