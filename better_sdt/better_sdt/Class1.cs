using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace better_sdt
{
    class qrs
    {
        private static int qrcounbt = 0;

        internal static void start()
        {
            // Open the camera
            VideoCapture capture = new VideoCapture(0); // 0 represents the default camera
            capture.Set(CapProp.FrameWidth, 1280);  // Set resolution
            capture.Set(CapProp.FrameHeight, 720);  // Set resolution
            capture.Set(CapProp.Fps, 60);

            // Check if the camera is opened successfully
            if (!capture.IsOpened)
            {
                Console.WriteLine("Failed to open the camera.");
                return;
            }

            // Create the QR code detector
            QRCodeDetector qrDetector = new QRCodeDetector();

            // Create a frame buffer
            Queue<Mat> frameBuffer = new Queue<Mat>();

            while (true)
            {
                Mat frame = new Mat();
                capture.Read(frame);

                // Check if the frame is empty
                if (frame.IsEmpty)
                {
                    Console.WriteLine("Empty frame, skipping...");
                    continue;
                }

                frameBuffer.Enqueue(frame);

                if (frameBuffer.Count > 0)
                {
                    Mat bufferedFrame = frameBuffer.Dequeue();

                    using Mat grayFrame = new Mat();
                    CvInvoke.CvtColor(bufferedFrame, grayFrame, ColorConversion.Rgb2Gray);

                    CvInvoke.GaussianBlur(grayFrame, grayFrame, new Size(3, 3), 0);
                    CvInvoke.EqualizeHist(grayFrame, grayFrame);
                    CvInvoke.MedianBlur(grayFrame, grayFrame, 5);

                    CvInvoke.Threshold(grayFrame, grayFrame, 25, 255, ThresholdType.Binary);
                    CvInvoke.MorphologyEx(grayFrame, grayFrame, MorphOp.Close, CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0));

                    Mat points = new Mat();
                    if (qrDetector.Detect(grayFrame, points))
                    {
                        string decodedText = qrDetector.Decode(grayFrame, points).Trim();
                        if (!string.IsNullOrEmpty(decodedText))
                        {
                            qrcounbt++;
                            Console.WriteLine($"{qrcounbt} Decoded QR code: {decodedText}");
                        }
                    }

                    CvInvoke.WaitKey(1);

                    // Release the frame
                    bufferedFrame.Dispose();
                }
            }
        }
    }
}
