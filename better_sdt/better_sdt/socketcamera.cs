using System;
using System.Net.Sockets;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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
                    using (var image = Image.Load<Rgb24>(memStream))
                    {
                        // Emgu CV formatına çevir
                        Mat frame = ImageToMat(image);

                        // QR kod işlemlerini yap
                        ProcessFrame(frame, qrDetector);
                    }
                }
            }
        }

        private static Mat ImageToMat(Image<Rgb24> image)
        {
            // Convert ImageSharp image to byte array
            byte[] imageBytes = ImageToByteArray(image);

            // Create a Mat from the byte array
            using (var buffer = new VectorOfByte(imageBytes))
            {
                // Decode the byte array into a Mat
                Mat mat = new Mat();
                CvInvoke.Imdecode(buffer, ImreadModes.Color, mat);
                return mat;
            }
        }

        private static byte[] ImageToByteArray(Image<Rgb24> image)
        {
            using (var ms = new MemoryStream())
            {
                image.SaveAsBmp(ms); // Save the image as BMP
                return ms.ToArray();
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
