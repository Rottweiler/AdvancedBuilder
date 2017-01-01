using System;
using System.Collections.Generic;
using System.Text;

namespace TestStub
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Install: " + Settings.Install);
            Console.WriteLine("Install path: " + Settings.Install_path);
            Console.WriteLine("Secret: " + Settings.Secret);
            Console.WriteLine("Secret: " + Settings.Something_secret );

            Console.ReadLine();
        }
    }
}
