using System.ComponentModel;
using System.Device.Gpio;
using System.Reflection.Emit;
using Swan.Logging;
using Unosquare.RaspberryIO.Abstractions;

namespace bettersdt
{
    public class PwmEngine
    {

                /// SAG MOTOR
                static VirtualPwm Xvpwm14=new VirtualPwm(14,100);
                static VirtualPwm Xvpwm15=new VirtualPwm(15,100);

                //SOL MOTOR
                static VirtualPwm Xvpwm12 = new  VirtualPwm(12,100);
                static VirtualPwm Xvpwm18 = new VirtualPwm(13,100);

                //linear
                static VirtualPwm Xvpm20 = new VirtualPwm(20,100);
                static VirtualPwm Xvpm21 = new VirtualPwm(21,100);
        internal static void PWM_BEGİN()
        {
            //KORNA
            cpv.PreparePin(22, PinMode.Output);

            //MOTOR DIGITAL 1 
            cpv.PreparePin(26, PinMode.Output);
            cpv.PreparePin(3, PinMode.Output);

            //MOTOR DIGITAL 2
            cpv.PreparePin(17, PinMode.Output);
            cpv.PreparePin(27, PinMode.Output);

            //LINEAR DIGITAL 1
            cpv.PreparePin(23, PinMode.Output);
            cpv.PreparePin(24, PinMode.Output);

            //high
            cpv.Write(26, PinValue.High);
            cpv.Write(3, PinValue.High);
            cpv.Write(17, PinValue.High);
            cpv.Write(27, PinValue.High);
            cpv.Write(23, PinValue.High);
            cpv.Write(24, PinValue.High);


        }

        internal static void PWM_FORWARD(){
            PWM_STOP();

            Xvpwm14.SetPercent(50);
            Xvpwm15.SetPercent(0);

            Xvpwm12.SetPercent(43 );
            Xvpwm18.SetPercent(0);

            LogSys.InfoLog("PWMFORWARD");


        }
        internal static void PWM_LEFT(int sagmotor = 37, int solmotor = 37)
        {
            PWM_STOP();

            Xvpwm14.SetPercent(37);
            Xvpwm15.SetPercent(0);

            Xvpwm12.SetPercent(0);
            Xvpwm18.SetPercent(37);

            LogSys.InfoLog("PWM LEFT  ");


        }
        internal static void PWM_BACKWARD()
        {

            Xvpwm14.Stop();
            Xvpwm15.Stop();
            Xvpwm12.Stop();
            Xvpwm18.Stop();



            Xvpwm14.SetPercent(0);
            Xvpwm15.SetPercent(50);

            Xvpwm12.SetPercent(0);
            Xvpwm18.SetPercent(43);

            LogSys.InfoLog("PWM BACK");


        }
        internal static void PWM_RİGHT()
        {

            Xvpwm14.Stop();
            Xvpwm15.Stop();
            Xvpwm12.Stop();
            Xvpwm18.Stop();



            Xvpwm14.SetPercent(0);
            Xvpwm15.SetPercent(37);
            Xvpwm12.SetPercent(37);
            Xvpwm18.SetPercent(0);
            LogSys.InfoLog("PWM RİGHT");


        }

        internal static void PWM_STOP()
        {

            Xvpwm14.Stop();
            Xvpwm15.Stop();
            Xvpwm12.Stop();
            Xvpwm18.Stop();

        }
        //linear

        internal static void PWM_LINEAR_PUSH()
        {
            Xvpm20.Stop();
            Xvpm21.Stop();

            Xvpm20.SetPercent(100);
            Xvpm21.SetPercent(0);
            LogSys.ErrorLog("PWM_LINER_PUSH");
        }
        internal static void PWM_LINEAR_STOP()
        {
            Xvpm20.Stop();
            Xvpm21.Stop();

            Xvpm20.SetPercent(0);
            Xvpm21.SetPercent(0);
            LogSys.ErrorLog("PWM_LINER_BACK");
        }
        internal static void PWM_LINEAR_BACK()
        {
            Xvpm20.Stop();
            Xvpm21.Stop();

            Xvpm20.SetPercent(0);
            Xvpm21.SetPercent(100);
            LogSys.ErrorLog("PWM_LINER_BACK");
        }
        internal static void WASD_MODE()
        {
            bool hornstatus = false;
            string previouskey = string.Empty;
            while (true)
            {

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.W && !(previouskey == "w"))
                    {
                        PWM_FORWARD();
                        previouskey = "w";
                    }
                    if (key.Key == ConsoleKey.A && !(previouskey == "a"))
                    {

                        PWM_LEFT();
                        previouskey = "a";

                    }
                    if (key.Key == ConsoleKey.S && !(previouskey == "s"))
                    {
                        PWM_BACKWARD();
                        LogSys.InfoLog("pWWwm12 255 yazdir !! ILERI IIII  ");
                        previouskey = "s";
                    }
                    if (key.Key == ConsoleKey.D && !(previouskey == "d"))
                    {
                        PWM_RİGHT();
                        previouskey = "d";

                    }
                    if (key.Key == ConsoleKey.B && !(previouskey == "b"))
                    {
                        // korna çal
                        if (!hornstatus)
                        {
                            cpv.Write(22, PinValue.High);
                            hornstatus = true;
                        }
                        else
                        {
                            hornstatus = false;
                            cpv.Write(22, PinValue.Low);
                        }
                        LogSys.SuccesLog("Horny beytullah");
                        previouskey = "b";

                    }
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        PWM_STOP();
                        previouskey = "";
                    }
                    if (key.Key == ConsoleKey.X)
                    {
                        Console.WriteLine("x pressed exiting ");
                        break;
                    }
                    if (key.Key == ConsoleKey.P && !(previouskey == "p"))
                    {
                        PWM_LINEAR_PUSH();
                        previouskey = "p";
                    }
                    if (key.Key == ConsoleKey.O && !(previouskey == "o"))
                    {
                        PWM_LINEAR_STOP();
                        previouskey = "o";
                    }
                    if (key.Key == ConsoleKey.L && !(previouskey == "l"))
                    {
                        PWM_LINEAR_BACK();
                        previouskey = "l";
                    }

                }
            }
        }
        
    }
}