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
            for (int i = 0; i < 36; i++)
            {
                using var capture = new VideoCapture("$/dev/video{i}");
                Emgu.CV.Mat frame = new Emgu.CV.Mat();
                capture.Read(frame);
                if (!frame.IsEmpty)
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
