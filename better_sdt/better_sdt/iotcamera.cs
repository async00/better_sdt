using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iot.Device.Media;
using System.Threading.Tasks;

namespace better_sdt
{
    internal class iotcamera
    {
        internal static VideoConnectionSettings settings = new VideoConnectionSettings(busId: 0, captureSize: (1280, 720), pixelFormat: PixelFormat.JPEG);



        internal static void saveimage() {
           
            using VideoDevice device = VideoDevice.Create(settings);

            Console.WriteLine("Smile, you are on camera");
            device.Capture("ferhaterdogan.jpeg");
        }
    }
     }
