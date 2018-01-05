using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MasterCode.MControls;
using Visifire.Charts;

namespace MasterCode.MControls
{
    /// <summary>
    /// DatasDisplayPage.xaml 的交互逻辑
    /// </summary>
    public partial class DatasDisplayPage : UserControl
    {
        private ImageTextButton NowSelectDeviceButton = null;
        private int NowSelectDeviceID = 0;

        public DatasDisplayPage()
        {
            InitializeComponent();

            for (int counter = 1; counter <= 20; counter++)
            {
                ImageTextButton deviceButton = new ImageTextButton();
                deviceButton.Style = (Style)this.Resources["MComSelectTButtonStyle"];
                deviceButton.SetIDAndClick(DeviceSelectButtonClick, counter.ToString());
                deviceButton.MButtonText = counter.ToString();
                deviceButton.ButtonText.TextAlignment = TextAlignment.Center;
                DeviceIDSelectSP.Children.Add(deviceButton);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /*
        private void CreateChart()
        {
            
            DataSeries dataSeries = new DataSeries();

            DTotalChart.Series.Add(dataSeries);

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

                dataPoint.XValue = loopIndex;
                // Set the YValue using random number
                dataPoint.YValue = random.Next(1, 100);

                // Add DataPoint to DataSeries
                dataSeries.DataPoints.Add(dataPoint);
            }
        }*/

        private void DeviceSelectButtonClick(ImageTextButton sender , String id)
        {
            if(NowSelectDeviceButton != null && NowSelectDeviceButton != sender)
            {
                NowSelectDeviceButton.DisActive();
            }

            NowSelectDeviceButton = sender;
            NowSelectDeviceID = int.Parse(id);
        }
    }
}
