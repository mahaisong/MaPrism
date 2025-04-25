using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaPrism.TestDryIocAddInDI
{
    public class SerilogUtils
    {

        private static Microsoft.Extensions.Logging.ILogger InjectLogger()
        {
            var logger = GetJsonLogger();
            var ioc = new ServiceCollection();
            ioc.AddLogging(builder => builder.AddSerilog(logger: logger, dispose: true));
            var loggerProvider = ioc.BuildServiceProvider().GetRequiredService<ILoggerProvider>();
            return loggerProvider.CreateLogger("Program");
        }

        private static Serilog.ILogger GetJsonLogger()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                                     .SetBasePath(AppContext.BaseDirectory)
                                     .AddJsonFile(path: "serilog.json", optional: true, reloadOnChange: true)
                                     .Build();
            if (configuration == null)
            {
                throw new ArgumentNullException($"未能找到 serilog.json 日志配置文件");
            }
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            return logger;
        }

        private static Serilog.ILogger GetLogger()
        {
            const string LogTemplate = "{SourceContext} {Scope} {Timestamp:HH:mm} [{Level}] {Message:lj} {Properties:j} {NewLine}{Exception}";
            var logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
#if DEBUG
                .MinimumLevel.Debug()
#else
		                .MinimumLevel.Information()
#endif
                .WriteTo.Console(outputTemplate: LogTemplate)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, outputTemplate: LogTemplate)
                .CreateLogger();
            return logger;
        }

    }
}
