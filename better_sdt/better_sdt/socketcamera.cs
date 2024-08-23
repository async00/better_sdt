using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

class socketcamera
{
    internal static void start()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8000);
        listener.Start();

        Console.WriteLine("Server listening on port 8000...");

        while (true)
        {
            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Client connected...");

                // Bellek akışı
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    // Veriyi oku ve bellekte sakla
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);
                    }

                    // Veriyi başa sar ve işleme al
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // JPEG verisini Emgu CV ile işleme
                    byte[] imageBytes = memoryStream.ToArray();
                    Mat mat = ByteArrayToMat(imageBytes);

                    if (mat == null || mat.IsEmpty)
                    {
                        Console.WriteLine("Error: Invalid or empty image.");
                        continue;
                    }

                    // Gelen görüntüyle işlem yapın
                    // Örneğin: QR kodu okuyabilir ya da görüntüyü kaydedebilirsiniz.
                    CvInvoke.Imwrite("received_image.jpg", mat);

                    // Mat nesnesini işleme (QR kod okuma, vb.)
                    // Örneğin: qrs.initalize(mat);
                }
            }

            Console.WriteLine("Client disconnected.");
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
}
