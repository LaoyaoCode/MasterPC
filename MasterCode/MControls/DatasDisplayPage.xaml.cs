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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MasterCode.MControls;
using Visifire.Charts;
using MasterCode.Code.SQL;
using MasterCode.Code;

namespace MasterCode.MControls
{
    /// <summary>
    /// DatasDisplayPage.xaml 的交互逻辑
    /// </summary>
    public partial class DatasDisplayPage : UserControl
    {
        private ImageTextButton NowSelectDeviceButton = null;
        private int NowSelectDeviceID = -1;
        private bool IsDeviceSelectGridMax = false;

        //现在选择的某一次的数据按钮
        private ImageTextButton NowSelectOnceDataButton = null;

        //所有数据的数据系
        private DataSeries TotalLightDS = null;
        private DataSeries TotalVoltageDS = null;
        private DataSeries TotalPowerFactorDS = null;
        private DataSeries TotalSomethings1DS = null;
        private DataSeries TotalSomethings2DS = null;

        //某一次数据的数据系
        private DataSeries OnceLightDS = null;
        private DataSeries OnceVoltageDS = null;
        private DataSeries OncePowerFactorDS = null;
        private DataSeries OnceSomethings1DS = null;
        private DataSeries OnceSomethings2DS = null;



        public SolidColorBrush LightLineBrush
        {
            get { return (SolidColorBrush)GetValue(LightLineBrushProperty); }
            set { SetValue(LightLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LightLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LightLineBrushProperty =
            DependencyProperty.Register("LightLineBrush", typeof(SolidColorBrush), typeof(DatasDisplayPage), new PropertyMetadata(null));




        public SolidColorBrush VoltageLineBrush
        {
            get { return (SolidColorBrush)GetValue(VoltageLineBrushProperty); }
            set { SetValue(VoltageLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VoltageLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VoltageLineBrushProperty =
            DependencyProperty.Register("VoltageLineBrush", typeof(SolidColorBrush), typeof(DatasDisplayPage), new PropertyMetadata(null));



        public SolidColorBrush PowerFactorLineBrush
        {
            get { return (SolidColorBrush)GetValue(PowerFactorLineBrushProperty); }
            set { SetValue(PowerFactorLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PowerFactorLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PowerFactorLineBrushProperty =
            DependencyProperty.Register("PowerFactorLineBrush", typeof(SolidColorBrush), typeof(DatasDisplayPage), new PropertyMetadata(null));



        public SolidColorBrush Something1LineBrush
        {
            get { return (SolidColorBrush)GetValue(Something1LineBrushProperty); }
            set { SetValue(Something1LineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Something1LineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Something1LineBrushProperty =
            DependencyProperty.Register("Something1LineBrush", typeof(SolidColorBrush), typeof(DatasDisplayPage), new PropertyMetadata(null));



        public SolidColorBrush Something2LineBrush
        {
            get { return (SolidColorBrush)GetValue(Something2LineBrushProperty); }
            set { SetValue(Something2LineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Something2LineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Something2LineBrushProperty =
            DependencyProperty.Register("Something2LineBrush", typeof(SolidColorBrush), typeof(DatasDisplayPage), new PropertyMetadata(null));







        public DatasDisplayPage()
        {
            InitializeComponent();

            for (int counter = 0; counter <= 19; counter++)
            {
                ImageTextButton deviceButton = new ImageTextButton();
                deviceButton.Style = (Style)this.Resources["MComSelectTButtonStyle"];
                deviceButton.SetIDAndClick(DeviceSelectButtonClick, counter.ToString());
                deviceButton.MButtonText = counter.ToString();
                deviceButton.ButtonText.TextAlignment = TextAlignment.Center;
                DeviceIDSelectSP.Children.Add(deviceButton);
            }

            ComControler.DatasCollectFinishedEvent += ComControler_DatasCollectFinishedEvent;
        }

        private void ComControler_DatasCollectFinishedEvent(Dictionary<int, RecordModel> model)
        {
            //并没有选择任何器件,则没有任何影响
            if(NowSelectDeviceButton == null)
            {
                return;
            }

            //获取刚刚获得的对应器件的信息
            RecordModel data = model[NowSelectDeviceID];
            

            //添加新的按钮
            ImageTextButton onceButton = new ImageTextButton();
            onceButton.Style = (Style)this.Resources["MComSelectTButtonStyle"];
            onceButton.SetIDAndClick(OnceDataButtonClick, data.FileName);
            onceButton.MButtonText = data.TimeString;
            onceButton.ButtonText.FontSize = 10;
            OnceDataOfDeviceSelectSP.Children.Insert(0,onceButton);

            //获取数据
            OneDeviceDatasModel one = OneDeviceDatasModel.CreateModelFromXMLFile(data.FileName);
            //添加数据到对应的曲线上
            for (int counterForData = 0; counterForData < one.Count(); counterForData++)
            {
                DataPoint lightPoint = new DataPoint();
                lightPoint.YValue = one.LightIntensity[counterForData];
                TotalLightDS.DataPoints.Add(lightPoint);


                DataPoint powerPoint = new DataPoint();
                powerPoint.YValue = one.PowerFactor[counterForData];
                TotalPowerFactorDS.DataPoints.Add(powerPoint);

                DataPoint voltagePoint = new DataPoint();
                voltagePoint.YValue = one.Voltage[counterForData];
                TotalVoltageDS.DataPoints.Add(voltagePoint);

                DataPoint something1Point = new DataPoint();
                something1Point.YValue = one.Somethings1[counterForData];
                TotalSomethings1DS.DataPoints.Add(something1Point);

                DataPoint something2Point = new DataPoint();
                something2Point.YValue = one.Somethings2[counterForData];
                TotalSomethings2DS.DataPoints.Add(something2Point);
            }


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void SelectDeviceMaxOrMinButtonClick()
        {
            if(IsDeviceSelectGridMax)
            {
                IsDeviceSelectGridMax = false;
                SelectMaxOrMinButton.ButtonIcon = MahApps.Metro.IconPacks.PackIconMaterialKind.ArrowLeftBoldCircleOutline;
                Storyboard animation = (Storyboard)this.Resources["SelectDeviceMinAnimation"];
                animation.Begin();
                SelectMaxOrMinButton.ToolTip = "打开器件选择面板";
            }
            else
            {
                IsDeviceSelectGridMax = true;
                SelectMaxOrMinButton.ButtonIcon = MahApps.Metro.IconPacks.PackIconMaterialKind.ArrowRightBoldCircleOutline;
                Storyboard animation = (Storyboard)this.Resources["SelectDeviceMaxAnimation"];
                animation.Begin();
                SelectMaxOrMinButton.ToolTip = "关闭器件选择面板";
            }
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

        //器件选择变换
        private void DeviceSelectButtonClick(ImageTextButton sender , String id)
        {
            if(NowSelectDeviceButton != null && NowSelectDeviceButton != sender)
            {
                NowSelectDeviceButton.DisActive();
            }

            NowSelectDeviceButton = sender;
            NowSelectDeviceID = int.Parse(id);

            //从0 -》 count ， 时间为最接近现在 到 最久远
            //故而显示按钮排列 0 -> COUNT , 而total数据添加，从 count -> 0
            List<RecordModel> records =   RecordDBControler.UnityIns.ReadSpecialDeviceRecord(true, NowSelectDeviceID);

            //如果之前某次数据显示表中有数据，则将其去除
            if(OnceLightDS != null)
            {
                OnceDataChart.Series.Remove(OnceLightDS);
                OnceDataChart.Series.Remove(OncePowerFactorDS);
                OnceDataChart.Series.Remove(OnceVoltageDS);
                OnceDataChart.Series.Remove(OnceSomethings1DS);
                OnceDataChart.Series.Remove(OnceSomethings2DS);
            }

            //如果之前总数据表中有数据，则将其去除
            if(TotalLightDS != null)
            {
                DTotalChart.Series.Remove(TotalLightDS);
                DTotalChart.Series.Remove(TotalPowerFactorDS);
                DTotalChart.Series.Remove(TotalVoltageDS);
                DTotalChart.Series.Remove(TotalSomethings1DS);
                DTotalChart.Series.Remove(TotalSomethings2DS);
            }

            //去除某次数据SP中所有的子元素
            OnceDataOfDeviceSelectSP.Children.Clear();
            NowSelectOnceDataButton = null;

            for (int counter = 0; counter < records.Count; counter++)
            {
                ImageTextButton onceButton = new ImageTextButton();
                onceButton.Style = (Style)this.Resources["MComSelectTButtonStyle"];
                onceButton.SetIDAndClick(OnceDataButtonClick, records[counter].FileName);
                onceButton.MButtonText = records[counter].TimeString;
                onceButton.ButtonText.FontSize = 10;
                OnceDataOfDeviceSelectSP.Children.Add(onceButton);
            }

            //创建新的数据集
            TotalLightDS = new DataSeries();
            TotalLightDS.Color = LightLineBrush;
            TotalLightDS.LegendText = "光照强度";
            TotalLightDS.RenderAs = RenderAs.Spline;

            TotalPowerFactorDS = new DataSeries();
            TotalPowerFactorDS.Color = PowerFactorLineBrush;
            TotalPowerFactorDS.LegendText = "功率因数";
            TotalPowerFactorDS.RenderAs = RenderAs.Spline;

            TotalVoltageDS = new DataSeries();
            TotalVoltageDS.Color = VoltageLineBrush;
            TotalVoltageDS.LegendText = "电压";
            TotalVoltageDS.RenderAs = RenderAs.Spline;

            TotalSomethings1DS = new DataSeries();
            TotalSomethings1DS.Color = Something1LineBrush;
            TotalSomethings1DS.LegendText = "Somethings1";
            TotalSomethings1DS.RenderAs = RenderAs.Spline;

            TotalSomethings2DS = new DataSeries();
            TotalSomethings2DS.Color = Something2LineBrush;
            TotalSomethings2DS.LegendText = "Somethings2";
            TotalSomethings2DS.RenderAs = RenderAs.Spline;

            //添加所有的数据
            for (int counter = records.Count - 1; counter >= 0; counter--)
            {
                //记录的模型则是从0 -> count，是最早采集->最晚采集
                OneDeviceDatasModel model = OneDeviceDatasModel.CreateModelFromXMLFile(records[counter].FileName);
                for(int counterForData = 0; counterForData < model.Count(); counterForData++)
                {
                    DataPoint lightPoint = new DataPoint();
                    lightPoint.YValue = model.LightIntensity[counterForData];
                    TotalLightDS.DataPoints.Add(lightPoint);


                    DataPoint powerPoint = new DataPoint();
                    powerPoint.YValue = model.PowerFactor[counterForData];
                    TotalPowerFactorDS.DataPoints.Add(powerPoint);

                    DataPoint voltagePoint = new DataPoint();
                    voltagePoint.YValue = model.Voltage[counterForData];
                    TotalVoltageDS.DataPoints.Add(voltagePoint);

                    DataPoint something1Point = new DataPoint();
                    something1Point.YValue = model.Somethings1[counterForData];
                    TotalSomethings1DS.DataPoints.Add(something1Point);

                    DataPoint something2Point = new DataPoint();
                    something2Point.YValue = model.Somethings2[counterForData];
                    TotalSomethings2DS.DataPoints.Add(something2Point);
                }

            }

            //添加所有的点集到总集
            DTotalChart.Series.Add(TotalLightDS);
            DTotalChart.Series.Add(TotalVoltageDS);
            DTotalChart.Series.Add(TotalPowerFactorDS);
            DTotalChart.Series.Add(TotalSomethings1DS);
            DTotalChart.Series.Add(TotalSomethings2DS);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="id">对应数据的文件名</param>
        private void OnceDataButtonClick(ImageTextButton sender, String id)
        {
            if(NowSelectOnceDataButton != null && NowSelectOnceDataButton != sender)
            {
                NowSelectOnceDataButton.DisActive();
            }

            NowSelectOnceDataButton = sender;


            //如果之前某次数据显示表中有数据，则将其去除
            if (OnceLightDS != null)
            {
                OnceDataChart.Series.Remove(OnceLightDS);
                OnceDataChart.Series.Remove(OncePowerFactorDS);
                OnceDataChart.Series.Remove(OnceVoltageDS);
                OnceDataChart.Series.Remove(OnceSomethings1DS);
                OnceDataChart.Series.Remove(OnceSomethings2DS);
            }

            //创建新的数据集
            OnceLightDS = new DataSeries();
            OnceLightDS.Color = LightLineBrush;
            OnceLightDS.LegendText = "光照强度";
            OnceLightDS.RenderAs = RenderAs.Spline;

            OncePowerFactorDS = new DataSeries();
            OncePowerFactorDS.Color = PowerFactorLineBrush;
            OncePowerFactorDS.LegendText = "功率因数";
            OncePowerFactorDS.RenderAs = RenderAs.Spline;

            OnceVoltageDS = new DataSeries();
            OnceVoltageDS.Color = VoltageLineBrush;
            OnceVoltageDS.LegendText = "电压";
            OnceVoltageDS.RenderAs = RenderAs.Spline;

            OnceSomethings1DS = new DataSeries();
            OnceSomethings1DS.Color = Something1LineBrush;
            OnceSomethings1DS.LegendText = "Somethings1";
            OnceSomethings1DS.RenderAs = RenderAs.Spline;

            OnceSomethings2DS = new DataSeries();
            OnceSomethings2DS.Color = Something2LineBrush;
            OnceSomethings2DS.LegendText = "Somethings2";
            OnceSomethings2DS.RenderAs = RenderAs.Spline;

            //获得模型数据
            OneDeviceDatasModel model = OneDeviceDatasModel.CreateModelFromXMLFile(id);
            for(int counter = 0; counter < model.Count();counter++)
            {

                DataPoint lightPoint = new DataPoint();
                lightPoint.YValue = model.LightIntensity[counter];
                OnceLightDS.DataPoints.Add(lightPoint);


                DataPoint powerPoint = new DataPoint();
                powerPoint.YValue = model.PowerFactor[counter];
                OncePowerFactorDS.DataPoints.Add(powerPoint);

                DataPoint voltagePoint = new DataPoint();
                voltagePoint.YValue = model.Voltage[counter];
                OnceVoltageDS.DataPoints.Add(voltagePoint);

                DataPoint something1Point = new DataPoint();
                something1Point.YValue = model.Somethings1[counter];
                OnceSomethings1DS.DataPoints.Add(something1Point);

                DataPoint something2Point = new DataPoint();
                something2Point.YValue = model.Somethings2[counter];
                OnceSomethings2DS.DataPoints.Add(something2Point);
            }


            OnceDataChart.Series.Add(OnceLightDS);
            OnceDataChart.Series.Add(OncePowerFactorDS);
            OnceDataChart.Series.Add(OnceVoltageDS);
            OnceDataChart.Series.Add(OnceSomethings1DS);
            OnceDataChart.Series.Add(OnceSomethings2DS);
        }
    }
}
