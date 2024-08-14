using bettersdt;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace better_sdt
{
    internal class LRinital
    {
        internal static bool uptstatus = true;
        private static bool lr1, lr2, lr3, lr4, lr5, lr6,lr7,lr8;
        private static Thread uptlines = new Thread(UpdateLines_t);
        internal static void Start()
        {
            uptstatus = true;
            //prepare pins 
            cpv.PreparePin(UPins.LR_PIN1, PinMode.Input);
            cpv.PreparePin(UPins.LR_PIN2, PinMode.Input);
            cpv.PreparePin(UPins.LR_PIN3, PinMode.Input);

            cpv.PreparePin(UPins.LR_PIN4, PinMode.Input);
            cpv.PreparePin(UPins.LR_PIN5, PinMode.Input);
            cpv.PreparePin(UPins.LR_PIN6, PinMode.Input);

            cpv.PreparePin(UPins.LR_PIN7, PinMode.Input);
            cpv.PreparePin(UPins.LR_PIN8, PinMode.Input);

         
          
            uptlines.Start(); // pni durumlarini surekli guncelle 
        }

       
        static bool OnlyThis2_N(int que1,int que2 )
        {
            // bu 2 si diisnde hepsi false ise true don
            if (!cpv.ReadPin(que1))
                return false;
            if (!cpv.ReadPin(que2)) 
                return false;

            int[] pins = [2,3,4,17,27,22,10,9];
            pins = pins.Where(pin => pin != que1).ToArray();
            pins = pins.Where(pin => pin != que2).ToArray();
            foreach (int pin in pins)
            {
                if (cpv.ReadPin(pin))
                {
                    return false;
                }

            }

            return true;
        }
        static bool OnlyThis3_R(int que1, int que2,int que3)
        {
            //BU 3 UDE YANMIYORSA TRUE DONER
            int[] pins = [que1,que2,que3];

            foreach (int pin in pins)
            {
                if (cpv.ReadPin(pin))
                {
                    return false;
                }

            }
                         
            return true;
        }
        internal static void LogPins()
        {
            while (true)
            {
                int[] pins = { 2, 3, 4, 17, 27, 22, 10, 9 };
                Console.Clear();

                Console.WriteLine("TCRT5000 Sensör Okumaları:");
                Console.WriteLine("--------------------------");
                Console.WriteLine("| Pin | Durum           |");
                Console.WriteLine("--------------------------");

                foreach (var pin in pins)
                {
                    var value = cpv.ReadPin(pin);
                    Console.WriteLine($"{pin}||{value}");
                }
                Console.WriteLine("--------------------------");
                Thread.Sleep(60);
            }
        }
        private static void AttachToEngine()
        {
            while (uptstatus)
            {

                if(lr4 && lr5 && OnlyThis2_N(UPins.LR_PIN4, UPins.LR_PIN5) )
                {
                    //duz cizgi
                    PwmEngine.PWM_FORWARD();
                    
                }
                if (lr1 && lr2 && lr3 && lr4 && lr5 && (OnlyThis3_R(UPins.LR_PIN6, UPins.LR_PIN7,UPins.LR_PIN8))   )
                {
                    //1 den 5 e true gidiyor digerleri tammaen false
                    // sola don aq
                    PwmEngine.PWM_LEFT();

                }
                if (lr4 && lr5 && lr6 && lr7 && lr8 && (OnlyThis3_R(UPins.LR_PIN1, UPins.LR_PIN2, UPins.LR_PIN3)))
                {
                    //4 den 8 e kadar heps ture dighelreir false 
                    //saga don amk 
                    //saga don amk 
                    PwmEngine.PWM_RİGHT();
                }

            }
            LogSys.WarnLog("Attoch to engine thread died ");
        }


        internal static void Dispose()
        {
            //thread.abort iyi birsey degil
            uptstatus = false;
       
        }
        static void UpdateLines_t() 
        {
            //igrenc kod
            while (uptstatus) {
              
                if (cpv.ReadPin(UPins.LR_PIN1))
                    lr1 = true;
                else
                    lr1 = false;

                if (cpv.ReadPin(UPins.LR_PIN2))
                    lr2 = true;
                else
                    lr2 = false;

                if (cpv.ReadPin(UPins.LR_PIN3))
                    lr3 = true;
                else
                    lr3 = false;

                if (cpv.ReadPin(UPins.LR_PIN4))
                    lr4 = true;
                else
                    lr4 = false;

                if (cpv.ReadPin(UPins.LR_PIN5))
                    lr5 = true;
                else
                    lr5 = false;

                if (cpv.ReadPin(UPins.LR_PIN6))
                    lr6 = true;
                else
                    lr6 = false;

                if (cpv.ReadPin(UPins.LR_PIN7))
                    lr7 = true;
                else
                    lr7 = false;

                if (cpv.ReadPin(UPins.LR_PIN8))
                    lr8 = true;
                else
                    lr8 = false;

            }
            LogSys.WarnLog("Lr updatelines thread died");

        }

    }
}
