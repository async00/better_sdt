using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Threading.Tasks;
using bettersdt;

namespace better_sdt
{
    class socketcamera
    {
        private const string JPEG_BOUNDARY = "--frame--";
        internal static async Task StartAsync() // Asenkron metot
        {
            // Sunucu oluşturma
            TcpListener listener = new TcpListener(IPAddress.Any, 8000);
            listener.Start();
            Console.WriteLine("Server listening on port 8000...");

            // Bağlantı bekleme
            using (TcpClient client = await listener.AcceptTcpClientAsync())
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Client connected.");

                // Verileri okumak için bir tampon
                byte[] buffer = new byte[1024 * 1024]; // 1 MB tampon
                int bytesRead;
                MemoryStream ms = new MemoryStream();

                while (true)
                {
                    try
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            ms.Write(buffer, 0, bytesRead);

                            // JPEG sınırlarını kontrol et
                            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes(JPEG_BOUNDARY);
                            int boundaryIndex = FindBoundary(ms.ToArray(), boundaryBytes);

                            if (boundaryIndex >= 0)
                            {
                                // JPEG verisini ayır
                                byte[] imageBytes = ms.ToArray();
                                Array.Resize(ref imageBytes, boundaryIndex);

                                // Görüntüyü yükle
                                //Mat frame = CvInvoke.Imdecode(imageBytes, ImreadModes.Color);

                               
                                    ProcessFrame(imageBytes);
                                
                              

                                // Kalan verileri sakla
                                ms.SetLength(0);
                                ms.Write(buffer, boundaryIndex + boundaryBytes.Length, bytesRead - boundaryIndex - boundaryBytes.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        break; // Bağlantı kesildiğinde döngüden çık
                    }
                }
            }
        }

        private static int FindBoundary(byte[] data, byte[] boundary)
        {
            for (int i = 0; i < data.Length - boundary.Length + 1; i++)
            {
                bool match = true;
                for (int j = 0; j < boundary.Length; j++)
                {
                    if (data[i + j] != boundary[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return i;
                }
            }
            return -1;
        }

        private static void ProcessFrame(byte[] imageBytes)
        {
            try
            {
                using (var vec = new VectorOfByte(imageBytes))
                {
                    var frame = new Mat();
                    CvInvoke.Imdecode(vec, ImreadModes.Color, frame);

                    if (frame.IsEmpty)
                    {
                        Console.WriteLine("Empty frame");
                        return;
                    }

                    // İşlem yapılacaksa buraya yazabilirsiniz
                    // Örneğin:
                    // CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);
                    // qrs.initalize(frame);
                }
            }
            catch(Exception ex)
            {
                LogSys.ErrorLog(ex.Message);
            }
            
        }
    }
}