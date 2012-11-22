using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
namespace Fluent.Infrastructure.Log
{
    public class DefaultLoggerFactory : ILoggerFactory
    {
        static DefaultLoggerFactory()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            if (!File.Exists(configPath))
                throw new FileNotFoundException(configPath);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(configPath));
        }

        public ILog GetLogger()
        {
            return LogManager.GetLogger(this.GetType());
        }

        public ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }
    }
}
