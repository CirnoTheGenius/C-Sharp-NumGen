using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RandomChart
{
    public partial class frmMain : Form
    {
        //Default bar graph sucks. Majorly.
        const long MAXNUMBERPOINTS = 1000;
        long[] aSummation;

        public frmMain()
        {
            InitializeComponent();
        }

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

            int seed = ((int)DateTime.Now.Ticks) & 0xFFFFFFF;

            this.seedLabel.Text = seed.ToString();
            Random r = new Random(seed);
            long summation = 0;

            for (long i = 0; i < numPoints.Value; i++)
            {
                summation = summation + (r.Next(0, 2) == 1 ? 1 : -1);
                aSummation[i] = summation;
            }

            btnPlot.Enabled = true;
            btnRandomize.Enabled = false;
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            if (numPoints.Value > MAXNUMBERPOINTS)
            {
                MessageBox.Show("The number " + numPoints.Value + " is too large. Please specify a value less than " + MAXNUMBERPOINTS + ".");
                return;
            }
            
            this.chartPlot.Series.Clear();

            for (long i = 0; i < aSummation.Length; i++)
            {
                this.chartPlot.Series.Add("RandomPoints" + i);
                this.chartPlot.Series["RandomPoints" + i].Points.AddY(aSummation[i]);
                chartPlot.Update();
            }

            chartPlot.ResetAutoValues();

            btnPlot.Enabled = false;
            btnRandomize.Enabled = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            btnPlot.Enabled = false;
            btnRandomize.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnRandomize_Click(sender, e);
            btnPlot_Click(sender, e);
        }
    }
}
