using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bettersdt
{
    internal static class LogSys
    {
        //herhangibi info uyarı veya erroru buradaki methodlar ile belirt
        //buraya belirtelen mesajlar sonradan bir .txtye dönüşecek debug için lazım
        //sonrasında tcp ile kendi sunucularımıza sorunları ileteceğiz.
        //olabilcek herşeyi try catch ve sürekli bir method kullandığında bunu loglamayı unutma kullanmayı unutma
        #region LOG METHODS !! 
        private static string History=string.Empty;
        private static string currentTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

        
        internal static void SuccesLog(string message)
        {
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SCS]" + message);
            Console.ResetColor();
            History += "[SCS]" + message;
        }
        internal static void InfoLog(string message) 
        { 
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[INFO]"+message);
            Console.ResetColor();
            History += "[INFO]" + message;
        }
        internal static void WarnLog(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARN]" + message);
            Console.ResetColor();
            History += "[WARN]"+message;
        }
        internal static void ErrorLog(string message,ConsoleColor specialcolor = ConsoleColor.Red)
        {
            Console.ForegroundColor = specialcolor;
            Console.WriteLine("[ERR]" + message);
            Console.ResetColor();
            History += "[ERR]"+message;
        }
        internal static void GrayLog(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ResetColor();
            History +=message;
        }
        internal static string PrepareHistory()
        {
            File.WriteAllText(currentTime+".txt",History);
            return History;//istiyosan al
        }
        #endregion
    }
}
