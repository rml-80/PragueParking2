using PragueParking2.Menus;
using System;
using System.Collections.Generic;

namespace PragueParking2
{
    class Program
    {
        static void Main(string[] args)
        {
            FileContext FC = new();     //must run before Menu class, because it checks files amd creates if not exsist
            Menu menu = new();
            menu.MainMenu(FC);
        }

    }
}
