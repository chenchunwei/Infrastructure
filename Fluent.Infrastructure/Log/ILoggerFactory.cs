using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Fluent.Infrastructure.Log
{
    public interface ILoggerFactory
    {
        ILog GetLogger();
        ILog GetLogger(string name);
    }
}
