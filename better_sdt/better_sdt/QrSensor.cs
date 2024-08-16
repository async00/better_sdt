using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using bettersdt;
using System.Reflection.Metadata.Ecma335;



namespace better_sdt
{
    internal class QrSensor
    {

        internal static string lastmessage = string.Empty;
        private static bool threadstatus = true;
        private static string receivedtext = string.Empty;

        private static SerialPort serialPort = new SerialPort
        {
            PortName = "/dev/hidraw0", // Arduino'nun bağlı olduğu portu belirleyin, bu genellikle "/dev/ttyACM0" veya "/dev/ttyUSB0" olur.
            BaudRate = 9600,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None,
            Encoding = Encoding.UTF8,
            ReadTimeout = 500,
            WriteTimeout = 500
        };

        internal static void StartListener()
        {
            threadstatus = true;
            Thread rxtc = new Thread(RXTXCListener_t);
            rxtc.Start();
            serialPort.Open();
        }

        internal static void Stop() { threadstatus = false; }

        private static void RXTXCListener_t()
        {
            while (threadstatus)
            {
                if (serialPort.IsOpen)
                {
                    receivedtext = serialPort.ReadLine();
                    LogSys.InfoLog("ARDUINO : " + receivedtext);
                    if (receivedtext != lastmessage && !(string.IsNullOrEmpty(receivedtext)))
                    {
                        lastmessage = receivedtext;
                        BitCommunication.SendMessage(lastmessage);
                    }
                    Thread.Sleep(200);
                }
            }

            LogSys.ErrorLog("BitCommunication thread died");
        }



    }
}
