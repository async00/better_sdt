using System;
using IronBarCode;
using OpenCvSharp;
using OpenCvSharp.Extensions;

class qrs
{
    internal static void Start()
    {
        // VideoCapture ile kamerayı başlat
        using var capture = new VideoCapture(0);

        if (!capture.IsOpened())
        {
            Console.WriteLine("Kamera açılamadı.");
            return;
        }

        while (true)
        {
            using var frame = new Mat();
            capture.Read(frame);

            if (frame.Empty())
            {
                Console.WriteLine("Görüntü alınamadı.");
                break;
            }

            // QR kodunu oku
            var result = ReadQRCode(frame);

            if (result != null)
            {
                Console.WriteLine("QR Kodu: " + result);
            }

            // Çıkmak için 'q' tuşuna basın
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                break;
            }
        }
    }

    private static string ReadQRCode(Mat frame)
    {
        using var bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
        var qrCode = BarcodeReader.QuicklyReadOneBarcode(bitmap);
        return qrCode?.Text;
    }
}
