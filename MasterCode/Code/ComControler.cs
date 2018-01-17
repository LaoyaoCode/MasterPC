using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using MasterCode.MControls;
using System.Threading.Tasks;
using System.Threading;
using MasterCode.Code.SQL;

namespace MasterCode.Code
{
    public class ComControler
    {
        public delegate void DatasCollectFinishedDel(Dictionary<int, RecordModel> datas);
        /// <summary>
        /// 数据采集完成之后得代理事件
        /// </summary>
        public static event DatasCollectFinishedDel DatasCollectFinishedEvent;

        public delegate void SendCommandDel(bool isSuccess) ;
        public static event SendCommandDel SendCommandEvent;

        public delegate void DatasCollectProgressDel(float percent);
        /// <summary>
        /// 采集数据进度
        /// </summary>
        public static event  DatasCollectProgressDel DatasCollectProgressEvent;
        /// <summary>
        /// 现在采集的数据数目
        /// </summary>
        private int CounterForDatas = 0;
        /// <summary>
        /// 最大数据数目
        /// </summary>
        private const float MaxCountForDatas = 600.0f;

        /// <summary>
        /// 命令字枚举体
        /// </summary>
        public enum CommandEnum
        {
            /// <summary>
            /// 开始传输数据
            /// </summary>
            BeginTrans = 1 ,
            /// <summary>
            /// 结束传输数据
            /// </summary>
            EndTrans = 2 ,
            /// <summary>
            /// 校验成功，可以开始下一组数据传输
            /// </summary>
            CheckSuccess = 3 ,
            /// <summary>
            /// 校验失败，需要再次发送上一组数据
            /// </summary>
            CheckFailed = 4
        }

        /// <summary>
        /// 命令字字节长度
        /// </summary>
        private const int CommandByteNumber = 9;
        /// <summary>
        /// 一次接受片段需要的字节长度
        /// </summary>
        private const int RecieveOnePieceByteNumber = 30;
        //开始字节
        private const byte StartBytes = 0xFF;
        //结束字节
        private const byte EndBytes = 0x00;
        /// <summary>
        /// 一次数据的缓存
        /// </summary>
        private byte[] OnePieceDataTemp = new byte[RecieveOnePieceByteNumber];
        /// <summary>
        /// 一次采集的数据模型
        /// </summary>
        private ExcelDatasModel ExcelDatas = null;
        //刚刚收集的数据的数据库信息
        private Dictionary<int, RecordModel> RightNowCollectDR = new Dictionary<int, RecordModel>();

        private System.Windows.Threading.DispatcherTimer InsideTimer = new System.Windows.Threading.DispatcherTimer();//初始化时钟

        public class PortParaSetStruct
        {
            public string ComName;
            public Parity ChooseParity;
            public Handshake ChooseHandShake;
            public int BaudRate;
            public StopBits ChooseStopBits;
        }

        private SerialPort DisposePort = new SerialPort();
        private bool IsPortOpened_In = false;
        public static ComControler UnityIns = null;


        public ComControler()
        {
            UnityIns = this;

            DisposePort.ReadBufferSize = 1 * 1024;

            InsideTimer.Interval = TimeSpan.FromMilliseconds(100);//每100MS触发一次
            InsideTimer.IsEnabled = true;//开启定时器
            InsideTimer.Tick += InsideTimer_Tick;
            InsideTimer.Start();
        }

        private void InsideTimer_Tick(object sender, EventArgs e)
        {
            if(DisposePort.IsOpen)
            {
                byte[] datas = new byte[DisposePort.BytesToRead];

                DisposePort.Read(datas, 0, DisposePort.BytesToRead);

                for (int counter = 0; counter < datas.Length; counter++)
                {
                    AddDataToTemp(datas[counter]);
                }
            }
        }

        private void AddDataToTemp(byte value)
        { 
            bool isGetBegin = true, isGetEnd = true;
            //全部往右移动一个单位
            for(int counter = 0; counter < OnePieceDataTemp.Length - 1; counter++)
            {
                OnePieceDataTemp[counter] = OnePieceDataTemp[counter + 1];
            }

            OnePieceDataTemp[OnePieceDataTemp.Length - 1] = value;

            //检查数据头
            for(int counter = 0; counter < 4; counter++)
            {
                if(OnePieceDataTemp[counter] != StartBytes)
                {
                    isGetBegin = false;
                }
            }

            //检查数据尾
            for (int counter = OnePieceDataTemp.Length - 4; counter < OnePieceDataTemp.Length; counter++)
            {
                if (OnePieceDataTemp[counter] != EndBytes)
                {
                    isGetEnd = false;
                }
            }

            if(isGetBegin && isGetEnd)
            {          
                byte checkByte = 0x00;

                for (int counter = 4; counter < OnePieceDataTemp.Length - 5; counter++)
                {
                    checkByte += OnePieceDataTemp[counter];
                }

                //校验通过，数据可以保存采集
                //可以开始发送下一组
                if (checkByte == OnePieceDataTemp[OnePieceDataTemp.Length - 5])
                {
                    //通知进度
                    CounterForDatas++;
                    if (DatasCollectProgressEvent != null)
                    {
                        DatasCollectProgressEvent.Invoke(CounterForDatas / MaxCountForDatas);
                    }

                    //直接开始记录数据
                    //计算器件ID
                    int device = OnePieceDataTemp[4];
                    //计算光强
                    float light = (OnePieceDataTemp[5] * 256 * 256 * 256 + OnePieceDataTemp[6] * 256 * 256 +
                        OnePieceDataTemp[7] * 256 + OnePieceDataTemp[8]) / 100.0f;
                    float voltage = (OnePieceDataTemp[9] * 256 * 256 * 256 + OnePieceDataTemp[10] * 256 * 256 +
                        OnePieceDataTemp[11] * 256 + OnePieceDataTemp[12]) / 100.0f;
                    float powerFactor = (OnePieceDataTemp[13] * 256 * 256 * 256 + OnePieceDataTemp[14] * 256 * 256 +
                        OnePieceDataTemp[15] * 256 + OnePieceDataTemp[16]) / 100.0f;
                    float something1 = (OnePieceDataTemp[17] * 256 * 256 * 256 + OnePieceDataTemp[18] * 256 * 256 +
                        OnePieceDataTemp[19] * 256 + OnePieceDataTemp[20]) / 100.0f;
                    float something2 = (OnePieceDataTemp[21] * 256 * 256 * 256 + OnePieceDataTemp[22] * 256 * 256 +
                        OnePieceDataTemp[23] * 256 + OnePieceDataTemp[24]) / 100.0f;


                    //添加一次数据
                    ExcelDatas.AllDevicesDatas[device].AddOnceData(light, voltage, powerFactor, something1, something2);

                    //这个设备的数据已经装满了 , 则保存为XML数据，并且将其写入数据库
                    if (ExcelDatas.AllDevicesDatas[device].Count() == OneDeviceDatasModel.MaxCount)
                    {
                        string fileName = ExcelDatas.AllDevicesDatas[device].SaveAsXMLDatas();
                        RecordModel model = new RecordModel(fileName, device);
                       
                        RecordDBControler.UnityIns.AddARecord(model, true);

                        //将信息保存在缓存区中
                        RightNowCollectDR.Add(device, model);
                    }

                    //如果所有数据已经采集完成了，则保存为EXCEL文件，并且发送结束命令
                    if (ExcelDatas.IsFull())
                    {
                        ExcelDatas.SaveAsXLSX();
                        ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "采集完成");

                        //数据采集完成，调用代理
                        if(DatasCollectFinishedEvent != null)
                        {
                            DatasCollectFinishedEvent.Invoke(RightNowCollectDR);
                        }

                        SendCommandToMCU(CommandEnum.EndTrans);
                    }
                    //采集还未结束，则发送下一帧数据命令
                    else
                    {
                        SendCommandToMCU(CommandEnum.CheckSuccess);
                    }

                }
                //校验失败，数据需要再次发送
                else
                {
                    ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Error, "校验失败，重新采集当前帧");
                    SendCommandToMCU(CommandEnum.CheckFailed);
                }


                //清空缓存区域
                for (int counter = 0; counter < OnePieceDataTemp.Length; counter++)
                {
                    OnePieceDataTemp[counter] = 0x00;
                }
            }
        }


        public bool OpenPort()
        {
            if(!IsPortOpened_In)
            {
                try
                {
                    DisposePort.Open();
                    IsPortOpened_In = true;
                    ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "串口打开成功");
                    return true;
                }
                catch (Exception e)
                {
                    ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Error, "串口打开失败");
                    ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Error, "Error Message" + e.Message);
                    IsPortOpened_In = false;
                    return false;
                }
            }
            else
            {
                ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "串口已经打开了");
                return true;
            }
            
        }

        public void PortClosed()
        {
            if(IsPortOpened_In)
            {
                DisposePort.Close();
                ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "串口关闭");
                IsPortOpened_In = false;
            }
            else
            {
                ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "串口已经关闭了");
            }
        }

        /// <summary>
        /// 设置串口参数
        /// </summary>
        /// <param name="para">参数结构体</param>
        public void SetPortPara(PortParaSetStruct para)
        {
            DisposePort.BaudRate = para.BaudRate;
            DisposePort.Handshake = para.ChooseHandShake;
            DisposePort.StopBits = para.ChooseStopBits;
            DisposePort.Parity = para.ChooseParity;
            DisposePort.PortName = para.ComName;
            DisposePort.DataBits = 8;
            DisposePort.NewLine = "\r\n";
        }

        public bool IsPortOpen()
        {
            return IsPortOpened_In;
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="command"></param>
        public void SendCommandToMCU(CommandEnum command)
        {
            //开始传输数据，则初始化excel模型
            if(command == CommandEnum.BeginTrans)
            {
                ExcelDatas = new ExcelDatasModel();
                //清理缓存
                RightNowCollectDR.Clear();
                ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "开始采集");

                CounterForDatas = 0;
            }

            if(!DisposePort.IsOpen)
            {
                ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Error, "串口还未打开");

                //串口还未打开，发送命令失败
                if(SendCommandEvent != null)
                {
                    SendCommandEvent.Invoke(false);
                }
                return;
            }

            byte[] datas = new byte[CommandByteNumber];

            for(int counter = 0; counter < 4; counter++)
            {
                datas[counter] = StartBytes;
            }

            datas[4] = (byte)command;

            for(int counter = 5; counter < CommandByteNumber; counter++)
            {
                datas[counter] = EndBytes;
            }

            ThreadPool.QueueUserWorkItem(SendCommand, datas);
        }

        private void SendCommand(object data)
        {
            byte[] datas = (byte[])(data);

            lock(DisposePort)
            {
                try
                {
                    //写入缓存区准备发送
                    DisposePort.Write(datas, 0, datas.Length);

                    //串口发送命令成功
                    if (SendCommandEvent != null)
                    {
                        SendCommandEvent.Invoke(true);
                    }

                }
                catch(Exception e)
                {
                    //串口数据发送失败
                    if (SendCommandEvent != null)
                    {
                        SendCommandEvent.Invoke(false);
                    }
                }
            }
        }
    }
}
