using System;
using System.Threading;
using OpenCvSharp;

public class QRCodeReader
{
    private readonly string _devicePath;
    private readonly Thread _workerThread;
    private bool _isRunning;

    public QRCodeReader(string devicePath)
    {
        _devicePath = devicePath;
        _workerThread = new Thread(Run);
    }

    public void Start()
    {
        _isRunning = true;
        _workerThread.Start();
    }

    public void Stop()
    {
        _isRunning = false;
        _workerThread.Join();
    }

    private void Run()
    {
        using var capture = new VideoCapture(_devicePath);
        using var qrDecoder = new QRCodeDetector();

        if (!capture.IsOpened())
        {
            Console.WriteLine("Kamera açılmadı.");
            return;
        }

        while (_isRunning)
        {
            using var frame = new Mat();
            capture.Read(frame);

            if (frame.Empty())
            {
                Console.WriteLine("Boş kare.");
                continue;
            }

            var qrCodeResult = qrDecoder.DetectAndDecode(frame, out Point2f[] points);

            if (!string.IsNullOrEmpty(qrCodeResult))
            {
                Console.Clear();
                Console.WriteLine($"QR Kod Okundu: {qrCodeResult}");
            }

            Thread.Sleep(100); // Kısa bir süre bekle
        }
    }
}
