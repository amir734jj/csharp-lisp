using System;
using System.IO;
using Core;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadAllText("test.lisp");

            var simulator = new LipsSimulator();

            Console.WriteLine(simulator.Simmulate<double>(code)());
        }
    }
}