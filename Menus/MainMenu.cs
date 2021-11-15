using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2.Menus
{
    class MainMenu
    {
        public void Menu()
        {
            PrintLine();
            Console.Write("1. Park\t2. Retrive\t3. Move\t4. Search\t0. Quit\n");
            PrintLine();
        }
        void PrintLine()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }
        }
    }
}
