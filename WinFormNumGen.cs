using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;

namespace RandomChart
{
    public partial class frmMain : Form
    {
        //Default bar graph sucks. Majorly.
        const long MAXNUMBERPOINTS = 5000;
        long[] aSummation;
        
        public frmMain()
        {
            InitializeComponent();
        }

        //Delegates
        delegate void SetTextCallback(Label l, String text);
        delegate void SetColorCallback(Label l, Color c);
        delegate void AddToChartCallback(Chart c, long i, long y);
        delegate void UpdateElementCallback(Object o);
        delegate void EnableButtonCallback(Button b);
        delegate void RemoveOldDataCallback(Chart c);
        delegate void SuspendUpdatesCallback(Chart c);
        delegate void ResumeUpdatesCallback(Chart c);

        //Delegate methods.
        public void SetText(Label l, String text)
        {
            if (l.InvokeRequired)
            {
                SetTextCallback stc = new SetTextCallback(SetText);
                this.Invoke(stc, new object[] { l, text });
            }
            else
            {
                l.Text = text;
            }
        }

        public void SetColor(Label l, Color c)
        {
            if (l.InvokeRequired)
            {
                SetColorCallback scc = new SetColorCallback(SetColor);
                this.Invoke(scc, new object[] { l, c });
            }
            else
            {
                l.ForeColor = c;
            }
        }

        public void UpdateElement(Object o)
        {
            if (o is Label)
            {
                if (((Label)o).InvokeRequired)
                {
                    UpdateElementCallback ulc = new UpdateElementCallback(UpdateElement);
                    this.Invoke(ulc, new object[] { o });
                }
                else
                {
                    ((Label)o).Update();
                }
            }
            else if (o is Chart)
            {
                Console.WriteLine("tried to pass chart");
            }
        }

        public void EnableButton(Button b)
        {
            if (b.InvokeRequired)
            {
                EnableButtonCallback ebc = new EnableButtonCallback(EnableButton);
                this.Invoke(ebc, new object[] { b });
            }
            else
            {
                b.Enabled = true;
            }
        }

        public void AddToChart(Chart c, long i, long y)
        {
            if (c.InvokeRequired)
            {
                AddToChartCallback atcc = new AddToChartCallback(AddToChart);
                this.Invoke(atcc, new object[] { c, i, y });
            }
            else
            {
                c.Series["elementValue"].Points.AddY(y);
            }
        }

        public void SuspendUpdates(Chart c)
        {
            if (c.InvokeRequired)
            {
                SuspendUpdatesCallback suc = new SuspendUpdatesCallback(SuspendUpdates);
                this.Invoke(suc, new object[] { c });
            }
            else
            {
                c.Series["elementValue"].Points.SuspendUpdates();
            }
        }

        public void ResumeUpdates(Chart c)
        {
            if (c.InvokeRequired)
            {
                ResumeUpdatesCallback suc = new ResumeUpdatesCallback(ResumeUpdates);
                this.Invoke(suc, new object[] { c });
            }
            else
            {
                c.Series["elementValue"].Points.ResumeUpdates();
            }
        }

        public void RemoveOldData(Chart c)
        {
            if (c.InvokeRequired)
            {
                RemoveOldDataCallback rodc = new RemoveOldDataCallback(RemoveOldData);
                this.Invoke(rodc, new object[] { c });
            }
            else
            {
                c.Series["elementValue"].Points.RemoveAt(0);
            }
        }

        //Listeners
        private void btnRandomize_Click(object sender, EventArgs e)
        {
            if (numPoints.Value.ToString().Length <= 0)
            {
                MessageBox.Show("Please enter a valid number.");
                return;
            }

            int result = 0;
            int.TryParse(numPoints.Value.ToString(), out result);
            if (result == 0)
            {
                MessageBox.Show("Please enter a valid number.");
                return;
            }

            if (numPoints.Value > MAXNUMBERPOINTS)
            {
                MessageBox.Show("The number " + numPoints.Value + " is too large. Please specify a value less than " + MAXNUMBERPOINTS + ".");
                return;
            }            

            aSummation = new long[(long)numPoints.Value];

            Random r = new Random((int)DateTime.Now.Ticks);
            long summation = 0;

            for (long i = 0; i < numPoints.Value; i++)
            {
                aSummation[i] = summation += r.Next(-2, 3);
            }

            btnPlot.Enabled = true;
            btnRandomize.Enabled = false;
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            startWork();
            btnPlot.Enabled = false;
            btnRandomize.Enabled = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            btnPlot.Enabled = false;
            btnRandomize.Enabled = true;
        }

        private void btnBoth_Click(object sender, EventArgs e)
        {
            btnRandomize_Click(sender, e);
            btnPlot_Click(sender, e);
        }

        //Workers.
        public void startWork()
        {
            chartPlot.Series.Clear();
            chartPlot.Series.Add("elementValue");
            chartPlot.Series[0].ChartType = SeriesChartType.FastLine;
            chartPlot.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartPlot.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartPlot.ChartAreas[0].AxisX.MinorGrid.Enabled = false;

            btnPlot.Enabled = false;
            Thread updateThread = new Thread(drawElements);
            updateThread.Start();
        }

        private void drawElements()
        {
            int diff;
            
            for (long i = 0; i < aSummation.Length; i++)
            {
                if (i >= 75)
                {
                    RemoveOldData(chartPlot);
                }

                AddToChart(chartPlot, i, aSummation[i]);

                if (i <= 100L)
                {
                    continue;
                }

                SetText(changeBegin, "$" + aSummation[i]);
                UpdateElement(changeBegin);
                
                if (i > 1)
                {
                    
                    diff = (int)(aSummation[i] - aSummation[i - 1]);

                    if (diff > 0)
                    {
                        SetColor(sumLabel, Color.Green);
                        SetText(sumLabel, "+$" + diff + ".00");
                    }
                    else
                    {
                        SetColor(sumLabel, Color.Red);
                        SetText(sumLabel, "-$" + Math.Abs(diff) + ".00");
                    }

                    UpdateElement(sumLabel);
                }

                Thread.Sleep(1000);
            }

            EnableButton(btnRandomize);
        }
    }
}
