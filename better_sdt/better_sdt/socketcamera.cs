using System;
using System.Net.Sockets;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace better_sdt
{
    class socketcamera
    {
        internal static void start()
        {
            TcpClient client = new TcpClient("127.0.0.1", 8000);
            NetworkStream stream = client.GetStream();
            byte[] dataBuffer = new byte[4];

            Emgu.CV.QRCodeDetector qrDetector = new Emgu.CV.QRCodeDetector();

            while (true)
            {
                // Paket başlığını oku (veri uzunluğu)
                stream.Read(dataBuffer, 0, dataBuffer.Length);
                int dataLength = BitConverter.ToInt32(dataBuffer, 0);
                byte[] data = new byte[dataLength];

                int bytesRead = 0;
                while (bytesRead < dataLength)
                {
                    bytesRead += stream.Read(data, bytesRead, dataLength - bytesRead);
                }

                // Veriyi aç
                using (var memStream = new MemoryStream(data))
                {
                    Bitmap bitmapFrame = new Bitmap(memStream);

                    // Emgu CV formatına çevir
                    Mat frame = BitmapToMat(bitmapFrame);

                    // QR kod işlemlerini yap
                    ProcessFrame(frame, qrDetector);
                }
            }
        }

        private static Mat BitmapToMat(Bitmap bitmap)
        {
            // Bitmap'i byte dizisine çevir
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] imageBytes = ms.ToArray();
                Mat mat = new Mat();
                using (VectorOfByte buf = new VectorOfByte(imageBytes))
                {
                    CvInvoke.Imdecode(buf, ImreadModes.Color, mat);
                }
                return mat;
            }
        }

        private static void ProcessFrame(Mat frame, Emgu.CV.QRCodeDetector qrDetector)
        {
            // Frame üzerinde QR kod tespit işlemleri yapılabilir
            // Örneğin:
            Mat grayFrame = new Mat();
            CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);

            Mat points = new Mat();
            if (qrDetector.Detect(grayFrame, points))
            {
                string decodedText = qrDetector.Decode(grayFrame, points).Trim();
                if (!string.IsNullOrEmpty(decodedText))
                {
                    Console.WriteLine($"Decoded QR code: {decodedText}");
                }
            }
        }
    }
}
