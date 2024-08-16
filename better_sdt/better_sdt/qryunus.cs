using System;
using System.Drawing;
using OpenCvSharp;
using ZXing;
using ZXing.QrCode;

class qryunus
{
    internal static void start()
    {
        // VideoCapture ile kamerayı başlat
        using var capture = new VideoCapture(0);

        if (!capture.IsOpened())
        {
            Console.WriteLine("Kamera açılamadı.");
            return;
        }

        var barcodeReader = new BarcodeReader();

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
            var result = ReadQRCode(frame, barcodeReader);

            if (result != null)
            {
                Console.WriteLine("QR Kodu: " + result.Text);
            }

            // Çıkmak için 'q' tuşuna basın
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                break;
            }
        }
    }

    private static Result ReadQRCode(Mat frame, BarcodeReader barcodeReader)
    {
        // OpenCV'den görüntüyü GRAY scale'e dönüştür
        using var grayFrame = new Mat();
        Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);

        // OpenCV'den Mat'ı byte array'e dönüştür
        var bytes = grayFrame.ToBytes(".png");

        // ZXing ile QR kodunu oku
        using var bitmapStream = new MemoryStream(bytes);
        using var bitmap = new System.Drawing.Bitmap(bitmapStream);
        var result = barcodeReader.Decode(bitmap);

        return result;
    }
}
