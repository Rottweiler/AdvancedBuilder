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
            TestStub.Settings settings = new TestStub.Settings();
            settings.Install_path = "Hello world, this was injected";
            settings.Install = true;
            settings.Secret = 33;
            settings.Something_secret = "This is also injected";

            var builder = new DynamicBuilder<TestStub.Settings>("TestStub.exe");
            builder.Build(settings, "build.exe");
        }
    }
}
