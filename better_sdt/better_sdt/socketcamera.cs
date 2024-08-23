using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using bettersdt;
using better_sdt;

class socketcamera
{
    internal static void start()
    {
        TcpListener listener = new TcpListener(8000);
        listener.Start();

        Console.WriteLine("Server listening on port 8000...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[4096];
            int bytesRead;

            // Veriyi oku ve bellekte sakla
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                LogSys.InfoLog("veri geldi ");
                memoryStream.Write(buffer, 0, bytesRead);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            // JPEG verisini Emgu CV ile işleme
            using (var imgStream = new MemoryStream(memoryStream.ToArray()))
            {
                // Emgu CV Image nesnesini oluştur
                byte[] imageBytes = memoryStream.ToArray();
                Mat mat = ByteArrayToMat(imageBytes);



                // Şimdi img nesnesini kullanabilirsiniz
                // Örneğin, resmi ekrana gösterebilirsiniz
                if(mat == null || mat.IsEmpty)
                {
                    LogSys.ErrorLog("mat error");
                    return;
                }
                qrs.initalize(mat);
                CvInvoke.WaitKey(0);
            }

            // Bağlantıyı kapat
            client.Close();
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

/*
 *  private static Mat ByteArrayToMat(byte[] imageBytes)
        {
            Mat mat = new Mat();
            using (VectorOfByte vec = new VectorOfByte(imageBytes))
            {
                CvInvoke.Imdecode(vec, ImreadModes.Color, mat);
            }
            return mat;
        }

 * 
 */
