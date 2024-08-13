using System;
using System.Device.Gpio;
using Unosquare.RaspberryIO.Abstractions;


using Unosquare.RaspberryIO;
using bettersdt;
namespace bettersdt{
public static class GC
{
    private static readonly GpioController _gpioController = new GpioController();

    // Pin'i açma ve modunu ayarlama
    
    public static void PreparePin(int pinNumber, PinMode mode)
    {
      
        if (!_gpioController.IsPinOpen(pinNumber))
        {
            _gpioController.OpenPin(pinNumber, mode);
        }
        else
        {
            _gpioController.SetPinMode(pinNumber, mode);
        }
    }


    // pini high ve ya low yap
    public static void Write(int pinNumber, PinValue value)
    {
        if (_gpioController.IsPinOpen(pinNumber))
        {
            _gpioController.Write(pinNumber, value);
        }
        
    }

    // pini oku 1 veya 0 döner
    public static int ReadPin(int pinNumber)
    {
        if (_gpioController.IsPinOpen(pinNumber))
        {
            PinValue value = _gpioController.Read(pinNumber);
            if (value==PinValue.High)
                return 1;    
            return 0;
            
        }
        else
        {
            LogSys.ErrorLog($"Pin {pinNumber} is not open.");
            return -1;
        }
    }

    // PWM PIN CONTROL 
    //ÇIAKRKEN BÜTÜN PİNLERİ KAPAT 
    public static void ClosePin(int pinNumber)
    {
        if (_gpioController.IsPinOpen(pinNumber))
        {
            Write(pinNumber,PinValue.Low);
            _gpioController.ClosePin(pinNumber);
        }
    }

}
}