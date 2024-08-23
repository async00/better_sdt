using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace better_sdt
{
    class socketcamera
    {
        internal static void start()
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
                        // Veriyi oku
                        byte[] data = ReadStream(stream);

                        if (data.Length > 0)
                        {
                            // JPEG verisini doğrudan Emgu CV ile yükle
                            SaveJpegFrame(data);
                            Mat frame = ByteArrayToMat(data);

                            if (frame.IsEmpty)
                            {
                                Console.WriteLine("Empty frame");
                                continue; // Boş frame geldiğinde bir sonraki veriyi bekle
                            }

                            // QR kod işlemleri yapılabilir
                            ProcessFrame(frame);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }

        private static byte[] ReadStream(NetworkStream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytesRead);
                }
                return ms.ToArray();
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
        private static void SaveJpegFrame(byte[] imageBytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (var fs = new FileStream("received_frame.jpg", FileMode.Create, FileAccess.Write))
                    {
                        ms.WriteTo(fs);
                    }
                }
                Console.WriteLine("Frame saved as received_frame.jpg");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving JPEG frame: {ex.Message}");
            }
        }
        private static void ProcessFrame(Mat frame)
        {
            // Frame üzerinde işlem yapılabilir
            Mat grayFrame = new Mat();
            try
            {
                //CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);

                // QR kod işlemleri yapılabilir
                // Example: Detect QR codes or other processing
                qrs.initalize(frame); // QR kodu işleme için

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessFrame error: {ex.Message}");
            }
        }
    }
}
