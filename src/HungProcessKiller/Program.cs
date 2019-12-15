using System;
using System.Collections.Generic;
using System.IO;
using ServiceStack.Configuration;
using Topshelf;

namespace HungProcessKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettingsFile = Path.Combine(AppContext.BaseDirectory, "app.settings");
            var appSettings = new TextFileSettings(appSettingsFile);
            var checkInterval = appSettings.Get<TimeSpan>("CheckInterval");
            var definitions = appSettings.Get<List<ProcessDefinition>>("Definitions");

            HostFactory.Run(windowsService =>
            {
                windowsService.Service<HungProcessMonitor>(s =>
                {
                    s.ConstructUsing(service => new HungProcessMonitor(checkInterval, definitions));
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });

                windowsService.RunAsLocalSystem();
                windowsService.StartAutomatically();

                windowsService.SetServiceName("HungProcessKiller");
                windowsService.SetDisplayName("HungProcessKiller");
                windowsService.SetDescription("A Windows background service which monitors hung processes and kills them after a fixed time period");
            });
        }
    }
}
