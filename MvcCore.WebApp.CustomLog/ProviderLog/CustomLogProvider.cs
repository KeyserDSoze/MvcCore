using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.CustomLog.ProviderLog
{
    public class CustomLogProvider : ILoggerProvider
    {
        private readonly Configuration configuration;
        private readonly ConcurrentDictionary<string, CustomLog> _loggers = new ConcurrentDictionary<string, CustomLog>();

        public CustomLogProvider(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new CustomLog(name, this.configuration));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
