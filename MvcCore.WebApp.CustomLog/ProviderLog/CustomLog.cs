using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.CustomLog.ProviderLog
{
    public class CustomLog : ILogger
    {
        private readonly string _name;
        private readonly Configuration configuration;

        public CustomLog(string name, Configuration configuration)
        {
            _name = name;
            this.configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == configuration.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (configuration.EventId == 0 || configuration.EventId == eventId.Id)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = configuration.Color;
                Console.WriteLine($"{logLevel.ToString()} - {eventId.Id} - {_name} - {formatter(state, exception)}");
                Console.ForegroundColor = color;
            }
        }
    }
}
