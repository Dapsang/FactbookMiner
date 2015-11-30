using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FactbookMiner;

namespace FactbookMinerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string president = MainMiner.GetPresident();
            Console.WriteLine(president);
            Console.Read();
        }
    }
}
