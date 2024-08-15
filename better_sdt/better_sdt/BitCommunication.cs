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
    internal class BitCommunication
    {
        //lang
        //rx ve tx ayirt lil nigggaaaaa 
        
        internal static string lastmessage =  string.Empty;
        private static bool threadstatus = true;
        private static SerialPort serialPort = new SerialPort("/dev/serial0", 9600);
        private static string receivedtext = string.Empty;  

        internal static void SendMessage(string message)
        {
            serialPort.WriteLine(message);
        }   
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
                    if (receivedtext != lastmessage  && !(string.IsNullOrEmpty(receivedtext)))
                    {
                        lastmessage = receivedtext;
                    } 
                    Thread.Sleep(200);
                }
            }

            LogSys.ErrorLog("BitCommunication thread died");
        }


    }
}
