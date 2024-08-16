using System;
using System.Drawing;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ZXing;

class qrs
{
    internal static void start()
    {
        using var capture = new VideoCapture(0);
        if (!capture.IsOpened())
        {
            Console.WriteLine("Kamera açılamadı.");
            return;
        }

        var barcodeReader = new BarcodeReader();
        int frameCount = 0;

        while (true)
        {
            using var frame = new Mat();
            capture.Read(frame);

            if (frame.Empty())
            {
                Console.WriteLine("Görüntü alınamadı.");
                break;
            }

            frameCount++;
            if (frameCount % 5 == 0) // Her 5 karede bir işlem yap
            {
                var result = ReadQRCode(frame, barcodeReader);
                if (result != null)
                {
                    Console.WriteLine("QR Kodu: " + result.Text);
                }
            }

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                break;
            }
        }
    }


    private static Result ReadQRCode(Mat frame, BarcodeReader barcodeReader)
    {
        using var grayFrame = new Mat();
        Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);

        // Boyutlandırma (isteğe bağlı, performansı artırabilir)
        var resizedFrame = new Mat();
        Cv2.Resize(grayFrame, resizedFrame, new OpenCvSharp.Size(640, 480));

        var bytes = resizedFrame.ToBytes(".png");
        using var bitmapStream = new MemoryStream(bytes);
        using var bitmap = new System.Drawing.Bitmap(bitmapStream);
        var result = barcodeReader.Decode(bitmap);

        return result;
    }

}
