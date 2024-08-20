using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace better_sdt
{
    class qrs
    {
        private static int qrcounbt = 0;
        internal static void start()
        {
            // Kamerayı aç
            VideoCapture capture = new VideoCapture(0); // 0, varsayılan kamerayı temsil eder
            capture.Set(CapProp.FrameWidth, 1280);  // Çözünürlük ayarla
            capture.Set(CapProp.FrameHeight, 720); // Çözünürlük ayarla
            capture.Set(CapProp.Fps, 60);
            // QR kod dedektörünü oluştur
            QRCodeDetector qrDetector = new QRCodeDetector();

            // Kare tamponu oluştur
            Queue<Mat> frameBuffer = new Queue<Mat>();

            while (true)
            {
                // Yeni bir kare al ve tampona ekle
                Mat frame = new Mat();
                capture.Read(frame);
                frameBuffer.Enqueue(frame);

                // Tampondaki kareyi işle
                if (frameBuffer.Count > 0)
                {
                    Mat bufferedFrame = frameBuffer.Dequeue();

                    using Mat grayFrame = new Mat();
                    CvInvoke.CvtColor(bufferedFrame, grayFrame, ColorConversion.Rgb2Gray);

                    CvInvoke.GaussianBlur(grayFrame, grayFrame, new Size(3, 3), 0);

                    // Kontrast artırma
                    CvInvoke.EqualizeHist(grayFrame, grayFrame);
                    CvInvoke.MedianBlur(frame, frame, 5);

                    // Eşikleme
                      CvInvoke.Threshold(bufferedFrame, bufferedFrame, 25, 255, ThresholdType.Binary);
                       CvInvoke.MorphologyEx(bufferedFrame, bufferedFrame, MorphOp.Close, CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0));


                    // QR kodları ara
                    Mat points = new Mat();
                    if (qrDetector.Detect(grayFrame, points))
                    {
                        string decodedText = qrDetector.Decode(grayFrame, points).Trim();
                        if (!string.IsNullOrEmpty(decodedText))
                        {
                            qrcounbt++;
                            Console.WriteLine($"{qrcounbt}Decoded QR code: {decodedText}");
                        }
                    }

                    // İşlenmiş kareyi ekranda göster
                 //   CvInvoke.Imshow("Optimized Frame", grayFrame);
                    CvInvoke.WaitKey(1);

                    // Kareyi serbest bırak
                    bufferedFrame.Dispose();
                }
            }
        }
    }
}


