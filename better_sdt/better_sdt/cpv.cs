using System;
using System.Device.Gpio;
using Unosquare.RaspberryIO.Abstractions;


using Unosquare.RaspberryIO;
using bettersdt;
namespace bettersdt{
public class cpv
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
            return;
        }
            LogSys.ErrorLog($"Pin {pinNumber} is not open.");


        
    }

    // pini oku 1 veya 0 döner
    public static bool ReadPin(int pinNumber)
    {
        if (_gpioController.IsPinOpen(pinNumber))
        {
            PinValue value = _gpioController.Read(pinNumber);
            if (value==PinValue.High)
                return true;    
            return false;
            
        }
        else
        {
            LogSys.ErrorLog($"Pin {pinNumber} is not open.");
            return false;
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