using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace better_sdt
{
    class TestCameraIndex
    {
        internal static void begin()
        {
            for (int i = 0; i < 25; i++)
            {
                using var capture = new VideoCapture(i);
                if (capture.IsOpened)
                {
                    Console.WriteLine($"Kamera {i} mevcut ve kullanılabilir.");
                }
                else
                {
                    Console.WriteLine($"Kamera {i} bulunamadı.");
                }
            }
        }
    }
}
