using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace MasterCode.Code
{
    public class ComControler
    {
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
            DisposePort.NewLine = "\r\n";
        }

        public bool IsPortOpen()
        {
            return IsPortOpened_In;
        }
    }
}
