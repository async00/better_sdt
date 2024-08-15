using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace better_sdt
{
    internal static class UPins
    {
        #region pins 
        /*
1  3v3 Power       |  2  5v Power
3  GPIO 2          |  4  5v Power
5  GPIO 3          |  6  Ground
7  GPIO 4          |  8  GPIO 14 
9  Ground          |  10 GPIO 15 
11 GPIO 17         |  12 GPIO 18 
13 GPIO 27         |  14 Ground
15 GPIO 22         |  16 GPIO 23
17 3v3 Power       |  18 GPIO 24
19 GPIO 10         |  20 Ground
21 GPIO 9          |  22 GPIO 25
23 GPIO 11         |  24 GPIO 8 
25 Ground          |  26 GPIO 7 
27 GPIO 0          |  28 GPIO 1 
29 GPIO 5          |  30 Ground
31 GPIO 6          |  32 GPIO 12 
33 GPIO 13         |  34 Ground
35 GPIO 19         |  36 GPIO 16
37 GPIO 26         |  38 GPIO 20 
39 Ground          |  40 GPIO 21 
         */
        #endregion

        //line read cizgi sensoru pnileri 
        internal static int LR_PIN1 = 2;
        internal static int LR_PIN2 = 3;
        internal static int LR_PIN3 = 4;
        internal static int LR_PIN4 = 17;
        internal static int LR_PIN5 = 27;
        internal static int LR_PIN6 = 22;
        internal static int LR_PIN7 = 10;
        internal static int LR_PIN8 = 9;



    }
}
