using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace better_sdt
{
    class qrs
    {
        private static int qrcounbt = 0;
        private static int fps = 0;
        private static int framecount = 0;
        private static Stopwatch stopwatch = new Stopwatch();
        private static VideoCapture capture = new VideoCapture(0);
        private static Queue<Mat> frameBuffer = new Queue<Mat>();

        private static Mat SharpenImage(Mat inputImage)
        {
            Mat outputImage = new Mat();
            CvInvoke.Laplacian(inputImage, outputImage, DepthType.Cv8U, 3);
            CvInvoke.AddWeighted(inputImage, 1.5, outputImage, -0.5, 0, outputImage);
            return outputImage;
        }

        internal static void ApplyOp(Mat grayFrame, Mat resultFrame)
        {
            grayFrame.CopyTo(resultFrame);
            CvInvoke.Add(resultFrame, new ScalarArray(new MCvScalar(0, 255, 0)), resultFrame);
            resultFrame = SharpenImage(resultFrame);
            CvInvoke.MedianBlur(resultFrame, resultFrame, 5);
        }


        internal static void start()
        {
            capture.Set(CapProp.FrameWidth, 1280);
            capture.Set(CapProp.FrameHeight, 720);
            capture.Set(CapProp.Fps, 60);

            QRCodeDetector qrDetector = new QRCodeDetector();

            stopwatch.Start();

            while (true)
            {
                Mat frame = new Mat();
                capture.Read(frame);
                frameBuffer.Enqueue(frame);


                if (frame.IsEmpty)
                {
                    Console.WriteLine("Kamera görüntüsü alınamadı veya boş.");
                    continue;
                }

                if (frameBuffer.Count > 0)
                {
                    framecount++;
                    Mat bufferedFrame = frameBuffer.Dequeue();
                    Mat grayFrame = new Mat();
                    Mat resultFrame = new Mat();

                    CvInvoke.CvtColor(bufferedFrame, grayFrame, ColorConversion.Bgr2Gray);


                    Mat points = new Mat();
                    if (qrDetector.Detect(grayFrame, points))
                    {


                        ApplyOp(grayFrame, resultFrame);
                        string decodedText = qrDetector.Decode(resultFrame, points).Trim();
                        if (!string.IsNullOrEmpty(decodedText))
                        {
                            qrcounbt++;
                            Console.WriteLine($"{qrcounbt} Decoded QR code: {decodedText}");
                        }
                    }

                    if (stopwatch.ElapsedMilliseconds >= 1000)
                    {
                        fps = (int)(1000.0 / stopwatch.ElapsedMilliseconds * framecount);
                        stopwatch.Restart();
                        framecount = 0;
                    }

                    CvInvoke.PutText(bufferedFrame, $"FPS: {fps}", new Point(10, 30), FontFace.HersheySimplex, 1.0, new MCvScalar(0, 255, 0), 2);
                    CvInvoke.Imshow("QR Code Detection", bufferedFrame);
                    CvInvoke.WaitKey(1);

                    bufferedFrame.Dispose();
                }
            }
        }
    }
}
