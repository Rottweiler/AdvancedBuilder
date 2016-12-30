using System;
using System.Collections.Generic;
using System.Text;

namespace TestStub
{
    public class Settings
    {
        public string Install_path { get; set; }
        public bool Install { get; set; }
        public int Secret { get; set; }

        public string Something_secret { get; set; }

        public Settings()
        {
            Install_path = "";
            Install = false;
            Secret = 0;
            Something_secret = "";
        }
    }
}