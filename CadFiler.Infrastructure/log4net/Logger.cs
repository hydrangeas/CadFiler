using CadFile.Domain.Repositories;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CadFiler.Infrastructure.log4net
{
    public class Logger : ILoggerRepository
    {
        public static ILog _logger = null;
        public ILog GetLogger()
        {
            if (_logger == null)
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
                _logger = LogManager.GetLogger(typeof(Logger));

            }
            return _logger;
        }
    }
}
