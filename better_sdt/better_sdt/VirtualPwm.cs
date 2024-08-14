using System;
using System.Device.Gpio;
using System.Threading;

namespace bettersdt
{
    public class VirtualPwm
    {
        private GpioController controller;
        private int pin;
        private int frequency;
        private int dutyCycle;
        private Timer pwmTimer;
        private bool isHigh;

        public VirtualPwm(int pinNumber, int frequency)
        {
            pin = pinNumber;
            this.frequency = frequency;
            controller = new GpioController();
            controller.OpenPin(pin, PinMode.Output);
        }

        public void SetPercent(int dutyCycle)
        {
            if(dutyCycle ==255){
                dutyCycle = 100;
            }


            this.dutyCycle = dutyCycle;
            int period = 1000 / frequency; // Period in milliseconds
            int onTime = period * dutyCycle / 100; // On time in milliseconds
            int offTime = period - onTime; // Off time in milliseconds

            if (pwmTimer != null)
            {
                pwmTimer.Dispose();
            }

            pwmTimer = new Timer(UpdatePWM, null, 0, period);

            void UpdatePWM(object state)
            {
                if (isHigh)
                {
                    controller.Write(pin, PinValue.Low);
                    isHigh = false;
                    pwmTimer.Change(offTime, Timeout.Infinite);
                }
                else
                {
                    try{
                    controller.Write(pin, PinValue.High);
                    isHigh = true;
                    pwmTimer.Change(onTime, Timeout.Infinite);
                    }catch(Exception ex){
                        Stop();
                        LogSys.ErrorLog("55.line hatası :((" + ex.Message);
                    }
                    
                }
            }
        }

        public void Stop()
        {
            try
            {
                this.SetPercent(0);
                if (pwmTimer != null)
                {
                    pwmTimer.Dispose();
                    pwmTimer = null;
                }
                controller.Write(pin, PinValue.Low);
            }
            catch (Exception ex)
            {
                LogSys.ErrorLog("virtual pwm cant stop "+ ex.Message);
            }
          
        }
    }
}
