using System;
using System.Collections.Generic;
using System.Text;

namespace TestStub
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings sett = new Settings();

            Console.WriteLine("Install: " + sett.Install);
            Console.WriteLine("Install path: " + sett.Install_path);
            Console.WriteLine("Secret: " + sett.Secret);
            Console.WriteLine("Secret: " + sett.Something_secret );

            Console.ReadLine();
        }
    }
}
