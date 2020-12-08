/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
