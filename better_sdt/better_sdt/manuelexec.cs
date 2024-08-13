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
        internal static string[] commandlisrt = ["help", "list","clear","exit"];
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
                    if (commandlisrt.Contains(input)){
                        doit(input);
                    }
                    else
                    {
                        LogSys.ErrorLog("bash ? command not found ");
                    }
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
                default:
                    LogSys.ErrorLog("update your command list lil nigga");
                    break;

            }
        }
        internal static void Dispose()
        {
            consolestatus = false;
        }


    }
}
