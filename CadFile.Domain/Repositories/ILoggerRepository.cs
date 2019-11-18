using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadFile.Domain.Repositories
{
    public interface ILoggerRepository
    {
        ILog GetLogger();
    }
}
