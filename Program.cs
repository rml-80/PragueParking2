using PragueParking2.Menus;
using System;
using System.Collections.Generic;

namespace PragueParking2
{
    class Program
    {
        static void Main(string[] args)
        {
            FileContext FC = new();
            Menu menu = new();
            menu.MainMenu(FC);
        }

    }
}
