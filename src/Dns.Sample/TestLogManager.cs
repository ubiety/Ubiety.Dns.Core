using System;
using Ubiety.Logging.Core;

namespace Dns.Sample
{
    class TestLogManager : IUbietyLogManager
    {
        public IUbietyLogger GetLogger(string name)
        {
            return new TestLogger(name);
        }

        private class TestLogger : IUbietyLogger
        {
            private readonly string _name;

            public TestLogger(string name)
            {
                _name = name;
            }

            public void Log(LogLevel level, object message)
            {
                Log(level, $"{message}");
            }

            public void Log(LogLevel level, object message, Exception exception)
            {
                Log(level, $"{message}{Environment.NewLine}{exception}");
            }

            private void Log(LogLevel level, string message)
            {
                Console.WriteLine($"[{_name}::{level}] {message}");
            }
        }
    }
}
