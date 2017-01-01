using dnlib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            TestStub.Settings.Install_path = "Hello world, this was injected";
            TestStub.Settings.Install = true;
            TestStub.Settings.Secret = 33;
            TestStub.Settings.Something_secret = "This is also injected";

            var builder = new NormalBuilder()
            {
                Stub = "TestStub.exe",
                FullName = "TestStub.Settings"
            };

            builder.Build("build.exe");
        }
    }
}
