using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sunucu oluşturma
            TcpListener listener = new TcpListener(IPAddress.Any, 8000);
            listener.Start();
            Console.WriteLine("Server listening on port 8000...");

            // Bağlantı bekleme
            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Client connected.");

                while (true)
                {
                    try
                    {
                        // Paket başlığını oku (veri uzunluğu)
                        byte[] lengthBuffer = new byte[4];
                        int bytesRead = stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                        if (bytesRead == 0)
                            break; // Bağlantı kapalı

                        int dataLength = BitConverter.ToInt32(lengthBuffer, 0);
                        byte[] data = new byte[dataLength];

                        int totalBytesRead = 0;
                        while (totalBytesRead < dataLength)
                        {
                            bytesRead = stream.Read(data, totalBytesRead, dataLength - totalBytesRead);
                            totalBytesRead += bytesRead;
                        }

                        // Veriyi işleme
                        using (var memStream = new MemoryStream(data))
                        {
                            using (var image = Image.Load<Rgb24>(memStream, new JpegDecoder()))
                            {
                                // Emgu CV formatına çevir
                                Mat frame = ImageToMat(image);

                                // QR kod işlemleri yapılabilir
                                ProcessFrame(frame);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }

        private static Mat ImageToMat(Image<Rgb24> image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, new JpegEncoder());
                byte[] imageBytes = ms.ToArray();
                Mat mat = new Mat();
                using (VectorOfByte vec = new VectorOfByte(imageBytes))
                {
                    CvInvoke.Imdecode(vec, ImreadModes.Color, mat);
                }
                return mat;
            }
        }

        private static void ProcessFrame(Mat frame)
        {
            // Frame üzerinde işlem yapılabilir
            Mat grayFrame = new Mat();
            CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);
            // İşlem kodları buraya eklenebilir
        }
    }
}
