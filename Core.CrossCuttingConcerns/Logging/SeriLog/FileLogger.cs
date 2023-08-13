using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;


namespace Core.CrossCuttingConcerns.Logging.SeriLog
{
    public class FileLogger : LoggerServiceBase
    {

        public FileLogger(IOptions<FileLogConfiguration> fileLogConfiguration, HttpContent content)
        {


            FileLogConfiguration _fileLogConfiguration = fileLogConfiguration.Value ??
                                                         throw new Exception(SeriLogMessages.NullOptions);

            string logPath = string.Format("{0},{1}", Directory.GetCurrentDirectory(), _fileLogConfiguration.Path);


            Logger = new LoggerConfiguration()
                    .WriteTo.File(logPath,
                                  rollingInterval: RollingInterval.Day,
                                  fileSizeLimitBytes: 500000,
                                  retainedFileCountLimit: null,
                                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}").CreateLogger();


        }
    }
}
