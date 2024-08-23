using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System;
using OpenCvSharp;
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
 //       private static Emgu.CV.VideoCapture capture = new Emgu.CV.VideoCapture(0);
        private static Queue<Emgu.CV.Mat> frameBuffer = new Queue<Emgu.CV.Mat>();
        private static Emgu.CV.Mat SharpenImage(Emgu.CV.Mat inputImage)
        {
            Emgu.CV.Mat outputImage = new Emgu.CV.Mat();
            CvInvoke.Laplacian(inputImage, outputImage, DepthType.Cv8U, 3);
            CvInvoke.AddWeighted(inputImage, 1.5, outputImage, -0.5, 0, outputImage);
            return outputImage;
        }

        internal static void ApplyOp(Emgu.CV.Mat grayFrame, Emgu.CV.Mat resultFrame)
        {
            grayFrame.CopyTo(resultFrame);
            CvInvoke.Add(resultFrame, new ScalarArray(new MCvScalar(0, 255, 0)  ), resultFrame);
            resultFrame = SharpenImage(resultFrame);
            CvInvoke.MedianBlur(resultFrame, resultFrame, 5);
        }


        internal static void initalize(Emgu.CV.Mat frame)
        {

            Emgu.CV.QRCodeDetector qrDetector = new Emgu.CV.QRCodeDetector();

            stopwatch.Start();

           
                frame = new Emgu.CV.Mat();

                frameBuffer.Enqueue(frame);
                if (frame.IsEmpty)
                {
                    Console.WriteLine("Kamera görüntüsü alınamadı veya boş.");
                return;
                }

                if (frameBuffer.Count > 0)
                {
                    framecount++;
                    Emgu.CV.Mat bufferedFrame = frameBuffer.Dequeue();
                    Emgu.CV.Mat grayFrame = new Emgu.CV.Mat();
                    Emgu.CV.Mat resultFrame = new Emgu.CV.Mat();

                    CvInvoke.CvtColor(bufferedFrame, grayFrame, ColorConversion.Bgr2Gray);


                    Emgu.CV.Mat points = new Emgu.CV.Mat();
                 //   ApplyOp(grayFrame, resultFrame);
                
                    if (qrDetector.Detect(grayFrame, points))
                    {


                       
                        string decodedText = qrDetector.Decode(resultFrame, points).Trim();
                        if (!string.IsNullOrEmpty(decodedText))
                        {
                            qrcounbt++;
                            Console.WriteLine($"{qrcounbt} Decoded QR code: {decodedText}");
                        }
                    }
                    

                //    CvInvoke.PutText(resultFrame, $"FPS: {fps}", new System.Drawing.Point(10, 30), FontFace.HersheySimplex, 1.0, new MCvScalar(0, 255, 0), 2);
               //     CvInvoke.Imshow("QR Code Detection", resultFrame);
                 //   CvInvoke.WaitKey(1);

                    bufferedFrame.Dispose();
                
            }
        }
    }
}
