using System;
using System.Collections.Generic;
using System.Text;

namespace Beater
{
    class Logger
    {
        private static List<string> _log = new List<string>();

        public static void Log(string message)
        {
            _log.Add(message);
        }
    }
}
