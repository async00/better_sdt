using System;
using System.Net.Sockets;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Jpeg;
using bettersdt;

namespace better_sdt
{
    class socketcamera
    {
        internal static void start()
        {
            TcpClient client = new TcpClient("127.0.0.1", 8000);
            while (!client.Connected)
            {

                try
                {
                   
                    NetworkStream stream = client.GetStream();
                    byte[] dataBuffer = new byte[4];

                }
                catch (Exception e) {

                    LogSys.InfoLog("conection waited");
                }
               

            }
            

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
                    using (var image = Image.Load<Rgb24>(memStream, new JpegDecoder()))
                    {
                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, new JpegEncoder());
                            byte[] imageBytes = ms.ToArray();

                            // Emgu CV formatına çevir
                            Mat frame = ByteArrayToMat(imageBytes);

                            // QR kod işlemlerini yap
                            ProcessFrame(frame, qrDetector);
                        }
                    }
                }
            }
        }

        private static Mat ByteArrayToMat(byte[] imageBytes)
        {
            Mat mat = new Mat();
            using (VectorOfByte vec = new VectorOfByte(imageBytes))
            {
                CvInvoke.Imdecode(vec, ImreadModes.Color, mat);
            }
            return mat;
        }

        private static void ProcessFrame(Mat frame, Emgu.CV.QRCodeDetector qrDetector)
        {
            // Frame üzerinde QR kod tespit işlemleri yapılabilir
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
