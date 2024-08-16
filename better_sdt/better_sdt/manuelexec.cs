using bettersdt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace better_sdt
{
    internal class manuelexec
    {
        internal static string[] commandlisrt = ["help", "list","clear","exit","lrstart","lrlog"];
        internal static bool consolestatus = false; 
        internal static void GetConsole()
        {
            // im better than fish and bash 
            consolestatus = true;
            string input = string.Empty;
            while (consolestatus)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("m_exec : ");
                Console.ForegroundColor = ConsoleColor.White;
                input = Console.ReadLine();
                if (!(string.IsNullOrEmpty(input)))// null veya empty ise sg 
                {
                    
                    doit(input);
                   
                }
                else
                {
                    LogSys.ErrorLog("bash  ? null or empty ! ! ");
                }


            }
        }
        private static void doit(string command )
        {
            if (command.Contains("wr"))
            {
                string text = command.Replace("wr ", "");
                BitCommunication.SendMessage(text);
                return;
            }

            switch (command)
            {
                case "help":
                    LogSys.InfoLog("yardima ihtiyacin varsa kodlari oku");
                    break;
                case "list":
                    int i = 0;
                    foreach (string  cmd  in  commandlisrt)
                    {
                        i++;
                        Console.WriteLine($"{i} {cmd}");
                    }
                    i = 0;
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "bitstart":
                    BitCommunication.StartListener();
                    break;
                case "exit":
                    LogSys.InfoLog("bye ! exit");
                    manuelexec.Dispose();
                    break;
                case "open":
                    BitCommunication.OpenPort();
                    break;
                case "qr":
                    qrs.Start();
                    break;
                default:
                    LogSys.ErrorLog("Mexec ? command not found ");
                    break;

            }

        }
        internal static void Dispose()
        {
            consolestatus = false;
        }


    }
}
