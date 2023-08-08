using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging.SeriLog
{
    public class FileLogConfiguration
    {
        public string Path { get; set; }

        public FileLogConfiguration()
        {
            Path = String.Empty;
        }
    }
}
