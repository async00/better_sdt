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
                case "exit":
                    LogSys.InfoLog("bye ! exit");
                    Dispose();
                    break;
                case "lrstart":
                    LogSys.InfoLog("Line read started with calibration ");
                    PwmEngine.PWM_BEGİN();
                    LRinital.Start();
                    break;
                case "lrlog":
                    LogSys.InfoLog("Line read started with calibration ");
                    LRinital.Start();
                    LRinital.LogPins();
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
