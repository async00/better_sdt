using MMALSharp.Components;
using MMALSharp.Common;
using MMALSharp.Handlers;
using MMALSharp.Native;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;
using MMALSharp.Common.Utility;
using MMALSharp;

namespace better_sdt
{
    class qrs
    {
        private static int qrcounbt = 0;
        private static int fps = 0;
        private static int framecount = 0;
        private static Stopwatch stopwatch = new Stopwatch();
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

        internal static async void start()
        {
            // MMALSharp'ı başlatın
            MMALCamera camera = MMALCamera.Instance;
            var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg");

            // Kamera ayarlarını yapın
            camera.ConfigureCameraSettings();
            MMALCameraConfig.StillResolution = new Resolution(1280, 720);
            MMALCameraConfig.StillFramerate = new MMAL_RATIONAL_T(60, 1);

            // QRCodeDetector'ı başlatın
            QRCodeDetector qrDetector = new QRCodeDetector();

            stopwatch.Start();

            while (true)
            {
                // Fotoğrafı çek ve dosyayı kaydet
                await camera.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);

                // En son kaydedilen dosyayı al
                string imagePath = imgCaptureHandler.GetFilepath();
                if (File.Exists(imagePath))
                {
                    // Dosyayı yükle
                    Mat frame = CvInvoke.Imread(imagePath, ImreadModes.Color);
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

                    // Dosyayı işlemeden sonra silin (isteğe bağlı)
                    File.Delete(imagePath);
                }
            }
        }
    }
}
