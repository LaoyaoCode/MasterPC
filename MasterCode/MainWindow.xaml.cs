using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire.Charts;
using System.Threading;
using System.Windows.Threading;

namespace MasterCode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // Create new DataSeries
        DataSeries dataSeries = new DataSeries();

        public MainWindow()
        {
            InitializeComponent();

            // Add DataSeries to Chart
            TestChar.Series.Add(dataSeries);

            CreateChart();
            CreateChart();
            CreateChart();
            CreateChart();
            CreateChart();
            CreateChart();
            CreateChart();
            CreateChart();
            CreateChart();




            //CreateChart2();

            //CreateChart2();
        }


        private void CreateChart()
        {
            dataSeries.RenderAs = RenderAs.Spline;

            // Number of DataPoints to be generated
            int numberOfDataPoints = 10;

            // To set the YValues of DataPoint
            Random random = new Random();

            // Loop and add a few DataPoints
            for (int loopIndex = 0; loopIndex < numberOfDataPoints; loopIndex++)
            {
                // Create a DataPoint
                DataPoint dataPoint = new DataPoint();

                // Set the YValue using random number
                dataPoint.YValue = random.Next(1, 100);

                // Add DataPoint to DataSeries
                dataSeries.DataPoints.Add(dataPoint);
            }
        }

        private void CreateChart2()
        {
            DataSeries dataSeries1 = new DataSeries();

            dataSeries1.RenderAs = RenderAs.Spline;

            // Number of DataPoints to be generated
            int numberOfDataPoints = 15;

            // To set the YValues of DataPoint
            Random random = new Random();

            // Loop and add a few DataPoints
            for (int loopIndex = 0; loopIndex < numberOfDataPoints; loopIndex++)
            {
                // Create a DataPoint
                DataPoint dataPoint = new DataPoint();

                // Set the YValue using random number
                dataPoint.YValue = random.Next(1, 100);

                // Add DataPoint to DataSeries
                dataSeries1.DataPoints.Add(dataPoint);
            }

            // Add DataSeries to Chart
            TestChar.Series.Add(dataSeries1);
        }
    }
}
