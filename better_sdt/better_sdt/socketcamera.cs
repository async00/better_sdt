﻿using System;
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
                            Console.WriteLine("Veri alındı. Toplam byte: " + totalBytesRead);
                            bytesRead = stream.Read(data, totalBytesRead, dataLength - totalBytesRead);
                            totalBytesRead += bytesRead;
                        }

                        // Veriyi işleme
                        using (var memStream = new MemoryStream(data))
                        {
                            // JPEG verisini doğrudan Emgu CV ile yükle
                            Mat frame = ByteArrayToMat(data);

                            if (frame.IsEmpty)
                            {
                                Console.WriteLine("Kamera görüntüsü alınamadı veya boş.");
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

        private static Mat ByteArrayToMat(byte[] imageBytes)
        {
            Mat mat = new Mat();
            try
            {
                using (VectorOfByte vec = new VectorOfByte(imageBytes))
                {
                    CvInvoke.Imdecode(vec, ImreadModes.Color, mat);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Imdecode error: {ex.Message}");
            }
            return mat;
        }

        private static void ProcessFrame(Mat frame)
        {
            // Frame üzerinde işlem yapılabilir
            Mat grayFrame = new Mat();
            try
            {
                CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);

                // QR kod işlemleri yapılabilir
                // Example: Detect QR codes or other processing
                qrs.initalize(grayFrame); // QR kodu işleme için
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessFrame error: {ex.Message}");
            }
        }
    }
}
