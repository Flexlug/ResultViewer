using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResultViewerWPF.OldGraphics;

namespace ResultViewerWPF
{
    public partial class MVSettings : Form
    {
        OldSettingsProvider oldSettings;
        MainWindow mainWindow;

        Graphics ContPreview;
        Graphics PointPreview;
        Graphics JuryPreview;

        Color ContBarColor;
        Color sndContBarColor;

        Color PointBarColor;
        Color sndPointBarColor;

        Color JuryBarColor;
        Color sndJuryBarColor;

        List<JuryBar> JuryBarList;
        List<ContestBar> ContestBarList;
        List<PointBar> PointBarList;

        public MVSettings(OldSettingsProvider _oldSettings)
        {
            InitializeComponent();
            oldSettings = _oldSettings;

            if (oldSettings == null)
                throw new NullReferenceException("При открытии настроек не инициализирован OldSettingsProvider");

            XNumUpDown.Value = oldSettings.XNum;
            XNumUpDown.Refresh();
            XNumUpDown.ValueChanged += new System.EventHandler(this.XNumUpDown_ValueChanged);

            YNumUpDown.Value = oldSettings.YNum;
            YNumUpDown.Refresh();
            YNumUpDown.ValueChanged += new System.EventHandler(this.YNumUpDown_ValueChanged);

            ContBarFontNumupdown.Value = oldSettings.ContBarFontSize;
            ContBarFontNumupdown.Refresh();
            ContBarFontNumupdown.ValueChanged += new System.EventHandler(this.ContBarFontNumupdown_ValueChanged);

            ContBarWidthNumupdown.Value = oldSettings.ContBarWidth;
            ContBarWidthNumupdown.Refresh();
            ContBarWidthNumupdown.ValueChanged += new System.EventHandler(this.ContBarWidthNumupdown_ValueChanged);

            ContBarHeightNumupdown.Value = oldSettings.ContBarHeight;
            ContBarHeightNumupdown.Refresh();
            ContBarHeightNumupdown.ValueChanged += new System.EventHandler(this.ContBarHeightNumupdown_ValueChanged);

            PointBarWidthNumupdown.Value = oldSettings.pointBarWidth;
            PointBarWidthNumupdown.Refresh();
            PointBarWidthNumupdown.ValueChanged += new System.EventHandler(this.PointBarWidthNumupdown_ValueChanged);

            PointBarHeightNumupdown.Value = oldSettings.pointBarHeight;
            PointBarHeightNumupdown.Refresh();
            PointBarHeightNumupdown.ValueChanged += new System.EventHandler(this.PointBarHeightNumupdown_ValueChanged);

            PointBarFontSizeNumupdown.Value = oldSettings.pointBarFontSize;
            PointBarFontSizeNumupdown.Refresh();
            PointBarFontSizeNumupdown.ValueChanged += new System.EventHandler(this.PointBarFontSizeNumupdown_ValueChanged);

            PointBarIntervalNumupdown.Value = oldSettings.pointBarInterval;
            PointBarIntervalNumupdown.Refresh();
            PointBarIntervalNumupdown.ValueChanged += new System.EventHandler(this.PointBarIntervalNumupdown_ValueChanged);

            JuryBarWidthNumupdown.Value = oldSettings.JuryBarWidth;
            JuryBarWidthNumupdown.Refresh();
            JuryBarWidthNumupdown.ValueChanged += new System.EventHandler(this.JuryBarWidthNumupdown_ValueChanged);

            JuryBarHeightNumupdown.Value = oldSettings.JuryBarHeight;
            JuryBarHeightNumupdown.Refresh();
            JuryBarHeightNumupdown.ValueChanged += new System.EventHandler(this.JuryBarHeightNumupdown_ValueChanged);

            JuryBarFontSizeNumupdown.Value = oldSettings.JuryBarFontSize;
            JuryBarFontSizeNumupdown.Refresh();
            JuryBarFontSizeNumupdown.ValueChanged += new System.EventHandler(this.JuryBarFontSizeNumupdown_ValueChanged);

            QuitFraseTextBox.Text = oldSettings.QuitFrase;
            QuitFraseTextBox.Refresh();
            QuitFraseTextBox.TextChanged += new System.EventHandler(this.QuitFraseTextBox_TextChanged);

            FrameRateNumupdown.Value = oldSettings.FrameRate;
            FrameRateNumupdown.Refresh();

            FrameIntervalNumupdown.Value = oldSettings.FrameInterval;
            FrameIntervalNumupdown.Refresh();

            ContBarColor = oldSettings.ContBarColor;
            sndContBarColor = oldSettings.sndContBarColor;
            PointBarColor = oldSettings.PointBarColor;
            sndPointBarColor = oldSettings.sndPointBarColor;
            JuryBarColor = oldSettings.JuryBarColor;
            sndJuryBarColor = oldSettings.sndContBarColor;


            ContestBarPreviewPictureBox.Image = new Bitmap(Properties.Resources.mainViewerBcg);
            ContPreview = Graphics.FromImage(ContestBarPreviewPictureBox.Image);
            PointBarPreviewPictureBox.Image = new Bitmap(Properties.Resources.mainViewerBcg);
            PointPreview = Graphics.FromImage(PointBarPreviewPictureBox.Image);
            JuryBarPreviewPictureBox.Image = new Bitmap(Properties.Resources.mainViewerBcg);
            JuryPreview = Graphics.FromImage(JuryBarPreviewPictureBox.Image);

            PointBarList = new List<PointBar>();
            for (int i = 0; i < 10; i++)
            {
                PointBarList.Add(new PointBar(GetStartPointCord(i + 1),
                                              i + 1,
                                              (int)PointBarFontSizeNumupdown.Value,
                                              (int)PointBarWidthNumupdown.Value,
                                              (int)PointBarHeightNumupdown.Value,
                                              PointBarColor,
                                              sndPointBarColor));
            }

            ContestBarList = new List<ContestBar>();
            ContestBarList.Add(new ContestBar(GetStartContestBarCord(0),
                                              $"Участник №{1}",
                                              (int)ContBarFontNumupdown.Value,
                                              (int)ContBarWidthNumupdown.Value,
                                              (int)ContBarHeightNumupdown.Value,
                                              ContBarColor));
            ContestBarList.Add(new ContestBar(GetStartContestBarCord(2),
                                              $"Участник №{2}",
                                              (int)ContBarFontNumupdown.Value,
                                              (int)ContBarWidthNumupdown.Value,
                                              (int)ContBarHeightNumupdown.Value,
                                              sndContBarColor));
            JuryBarList = new List<JuryBar>();
            JuryBarList.Add(new JuryBar(GetStartContestBarCord(0),
                                        $"Жюри №{1}",
                                        (int)JuryBarFontSizeNumupdown.Value,
                                        (int)JuryBarWidthNumupdown.Value,
                                        (int)JuryBarHeightNumupdown.Value,
                                        JuryBarColor));
            JuryBarList.Add(new JuryBar(GetStartContestBarCord(2),
                                         QuitFraseTextBox.Text,
                                         (int)JuryBarFontSizeNumupdown.Value,
                                         (int)JuryBarWidthNumupdown.Value,
                                         (int)JuryBarHeightNumupdown.Value,
                                         sndJuryBarColor));
            DrawContestBars();
            DrawPointBars();
            DrawJuryBars();

        }

        private void DrawContestBars()
        {
            ContPreview.Clear(Color.Transparent);
            foreach (ContestBar cb in ContestBarList)
            {
                cb.Draw(ContPreview);
            }
            ContestBarPreviewPictureBox.Invalidate();
        }

        private void DrawJuryBars()
        {
            JuryPreview.Clear(Color.Transparent);
            foreach (JuryBar jb in JuryBarList)
            {
                jb.Draw(JuryPreview);
            }
            JuryBarPreviewPictureBox.Invalidate();
        }

        private void DrawPointBars()
        {
            PointPreview.Clear(Color.Transparent);
            foreach (PointBar pb in PointBarList)
            {
                pb.Draw(PointPreview);
            }
            PointBarPreviewPictureBox.Invalidate();
        }

        private void RefreshContestBars()
        {
            ContestBarList.Clear();
            ContestBarList.Add(new ContestBar(GetStartContestBarCord(0),
                                              $"Участник №{1}",
                                              (int)ContBarFontNumupdown.Value,
                                              (int)ContBarWidthNumupdown.Value,
                                              (int)ContBarHeightNumupdown.Value,
                                              ContBarColor));
            ContestBarList.Add(new ContestBar(GetStartContestBarCord(2),
                                              $"Участник №{2}",
                                              (int)ContBarFontNumupdown.Value,
                                              (int)ContBarWidthNumupdown.Value,
                                              (int)ContBarHeightNumupdown.Value,
                                              sndContBarColor));
            DrawContestBars();
        }

        private void RefreshPointBars()
        {
            PointBarList.Clear();
            for (int i = 0; i < 10; i++)
            {
                PointBarList.Add(new PointBar(GetStartPointCord(i + 1),
                                              i + 1,
                                              (int)PointBarFontSizeNumupdown.Value,
                                              (int)PointBarWidthNumupdown.Value,
                                              (int)PointBarHeightNumupdown.Value,
                                              PointBarColor,
                                              sndPointBarColor));
            }
            DrawPointBars();
        }

        private void RefreshJuryBars()
        {
            JuryBarList.Clear();
            JuryBarList.Add(new JuryBar(GetStartJuryBarCord(0),
                                        $"Жюри №{1}",
                                        (int)JuryBarFontSizeNumupdown.Value,
                                        (int)JuryBarWidthNumupdown.Value,
                                        (int)JuryBarHeightNumupdown.Value,
                                        JuryBarColor));
            JuryBarList.Add(new JuryBar(GetStartJuryBarCord(2),
                                         QuitFraseTextBox.Text,
                                         (int)JuryBarFontSizeNumupdown.Value,
                                         (int)JuryBarWidthNumupdown.Value,
                                         (int)JuryBarHeightNumupdown.Value,
                                         sndJuryBarColor));

            DrawJuryBars();
        }

        private Point GetStartJuryBarCord(int i)
        {
            return new Point((JuryBarPreviewPictureBox.Width / 2) - ((int)JuryBarWidthNumupdown.Value / 2),
                             (JuryBarPreviewPictureBox.Height / 4 * (i + 1)) - ((int)JuryBarHeightNumupdown.Value / 2));
        }

        private Point GetStartPointCord(int i)
        {
            int bi = i;
            i = i <= 5 ? i -= 5 : i -= 6;
            return new Point((PointBarPreviewPictureBox.Width / 2) + ((int)PointBarWidthNumupdown.Value * i) + ((int)PointBarIntervalNumupdown.Value * i) + (((int)PointBarIntervalNumupdown.Value / 2) * (bi > 5 ? 1 : -1)) + (((int)PointBarWidthNumupdown.Value / 2) * (bi > 5 ? 1 : -1)) - ((int)PointBarWidthNumupdown.Value / 2),
                              (PointBarPreviewPictureBox.Height / 2) - ((int)PointBarHeightNumupdown.Value / 2) );
        }

        private Point GetStartContestBarCord(int i)
        {
            return new Point((ContestBarPreviewPictureBox.Width / 2) - ((int)ContBarWidthNumupdown.Value / 2),
                             (ContestBarPreviewPictureBox.Height / 4 * (i + 1)) - ((int)ContBarHeightNumupdown.Value / 2));
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            oldSettings.ContBarColor = ContBarColor;
            oldSettings.sndContBarColor = sndContBarColor;
            oldSettings.ContBarFontSize = (int)ContBarFontNumupdown.Value;
            oldSettings.ContBarWidth = (int)ContBarWidthNumupdown.Value;
            oldSettings.ContBarHeight = (int)ContBarHeightNumupdown.Value;

            oldSettings.XNum = (int)XNumUpDown.Value;
            oldSettings.YNum = (int)YNumUpDown.Value;

            oldSettings.PointBarColor = PointBarColor;
            oldSettings.sndPointBarColor = sndPointBarColor;
            oldSettings.pointBarWidth = (int)PointBarWidthNumupdown.Value;
            oldSettings.pointBarHeight = (int)PointBarHeightNumupdown.Value;
            oldSettings.pointBarInterval = (int)PointBarIntervalNumupdown.Value;
            oldSettings.pointBarFontSize = (int)PointBarFontSizeNumupdown.Value;

            oldSettings.JuryBarColor = JuryBarColor;
            oldSettings.sndJuryBarColor = sndJuryBarColor;
            oldSettings.JuryBarWidth = (int)JuryBarWidthNumupdown.Value;
            oldSettings.JuryBarHeight = (int)JuryBarHeightNumupdown.Value;
            oldSettings.JuryBarFontSize = (int)JuryBarFontSizeNumupdown.Value;

            oldSettings.QuitFrase = QuitFraseTextBox.Text;
            oldSettings.FrameRate = (int)FrameRateNumupdown.Value;
            oldSettings.FrameInterval = (int)FrameIntervalNumupdown.Value;

            Viewer.ProgramSettings.OldSettings = oldSettings;
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            ContBarColor = Color.FromArgb(255, 26, 43, 63);
            sndContBarColor = Color.FromArgb(255, 203, 12, 179);
            ContBarFontNumupdown.Value = 20;
            ContBarWidthNumupdown.Value = 405;
            ContBarHeightNumupdown.Value = 35;

            XNumUpDown.Value = 2;
            YNumUpDown.Value = 15;

            PointBarColor = Color.FromArgb(255, 91, 218, 211);
            sndPointBarColor = Color.FromArgb(255, 203, 12, 179);
            PointBarWidthNumupdown.Value = 43;
            PointBarHeightNumupdown.Value = 27;
            PointBarIntervalNumupdown.Value = 10;
            PointBarFontSizeNumupdown.Value = 20;

            JuryBarColor = Color.FromArgb(255, 26, 43, 63);
            sndJuryBarColor = Color.FromArgb(255, 203, 12, 179);
            JuryBarWidthNumupdown.Value = 405;
            JuryBarHeightNumupdown.Value = 35;
            JuryBarFontSizeNumupdown.Value = 20;

            QuitFraseTextBox.Text = "Показ завершён";
            FrameRateNumupdown.Value = 20;
            FrameIntervalNumupdown.Value = 10;
        }

        private void ContBarWidthNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshContestBars();
        }

        private void ContBarHeightNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshContestBars();
        }

        private void ContBarFontNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshContestBars();
        }

        private void PointBarWidthNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshPointBars();
        }

        private void PointBarHeightNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshPointBars();
        }

        private void PointBarFontSizeNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshPointBars();
        }

        private void PointBarIntervalNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshPointBars();
        }

        private void JuryBarWidthNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshJuryBars();
        }

        private void JuryBarHeightNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshJuryBars();
        }

        private void JuryBarFontSizeNumupdown_ValueChanged(object sender, EventArgs e)
        {
            RefreshJuryBars();
        }

        private void XNumUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void YNumUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ChangeTopContBarColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.ShowHelp = true;
            cd.Color = ContBarColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                ContBarColor = cd.Color;
                ContestBarList[0].SetColor(cd.Color);
                DrawContestBars();
            }
            else
            {
                DrawContestBars();
            }
            cd.Dispose();
        }

        private void ChangeBotContBarColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.ShowHelp = true;
            cd.Color = sndContBarColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                sndContBarColor = cd.Color;
                ContestBarList[1].SetColor(cd.Color);
                DrawContestBars();
            }
            else
            {
                DrawContestBars();
            }
            cd.Dispose();
        }

        private void ChangeTopPointBarColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.ShowHelp = true;
            cd.Color = PointBarColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                PointBarColor = cd.Color;
                for (int i = 0; i < 9; i++)
                {
                    PointBarList[i].SetColor(cd.Color);
                }
                DrawPointBars();
            }
            else
            {
                DrawPointBars();
            }
            cd.Dispose();
        }

        private void ChangeBotPointBarColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.ShowHelp = true;
            cd.Color = sndPointBarColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                sndPointBarColor = cd.Color;
                PointBarList[9].SetColor(cd.Color);                
                DrawPointBars();
            }
            else
            {
                DrawPointBars();
            }
            cd.Dispose();
        }

        private void ChangeTopJuryBarColorbutton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.ShowHelp = true;
            cd.Color = JuryBarColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                JuryBarColor = cd.Color;
                JuryBarList[0].SetColor(cd.Color);
                DrawJuryBars();
            }
            else
            {
                DrawJuryBars();
            }
            cd.Dispose();
        }

        private void ChangeBotJuryBarColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            cd.ShowHelp = true;
            cd.Color = sndJuryBarColor;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                sndJuryBarColor = cd.Color;
                JuryBarList[1].SetColor(cd.Color);                
                DrawJuryBars();
            }
            else
            {
                DrawJuryBars();
            }
            cd.Dispose();
        }

        private void QuitFraseTextBox_TextChanged(object sender, EventArgs e)
        {
            RefreshJuryBars();
        }

        private void XNumUpDown_ValueChanged_1(object sender, EventArgs e)
        {
        }
    }
}
