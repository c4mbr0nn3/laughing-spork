using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PlotCharts
{
    public partial class ResultPlot : Form
    {
        public ResultPlot()
        {
            InitializeComponent();

            List<double> tempCIR = new List<double>();
            List<double> tempHIS = new List<double>();

            StreamReader filereader = new StreamReader("cir_calibration_result.txt");
            string line = null;
            line = filereader.ReadLine();
            while ((line = filereader.ReadLine()) != null)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    string[] record = line.Split('\t');
                    tempCIR.Add(Double.Parse(record[1]));
                    tempHIS.Add(Double.Parse(record[2]));
                }
            }
            filereader.Close();

            int j = 1;
            int i = 1;
            foreach (double C in tempCIR)
            {
                chart1.Series["Estimated"].Points.Add(new DataPoint(j, C));
                j++;
            }
            foreach (double H in tempHIS)
            {
                chart1.Series["Actual"].Points.Add(new DataPoint(i, H));
                i++;
            }

            chart1.Series["Estimated"].ChartType = SeriesChartType.FastLine;

            chart1.Series["Actual"].ChartType = SeriesChartType.FastLine;


            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Gainsboro;


        }
    }
}