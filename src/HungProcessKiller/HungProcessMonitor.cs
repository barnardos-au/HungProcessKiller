using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HungProcessKiller
{
    public class HungProcessMonitor
    {
        private readonly TimeSpan checkInterval;
        private readonly List<ProcessDefinition> definitions;

        public HungProcessMonitor(TimeSpan checkInterval, List<ProcessDefinition> definitions)
        {
            this.checkInterval = checkInterval;
            this.definitions = definitions;
        }
        
        private Task serviceTask;
        private CancellationTokenSource cts;

        public void Start()
        {
            cts = new CancellationTokenSource();
            serviceTask = ExecuteAsync();
        }

        public void Stop()
        {
            cts.Cancel();
            serviceTask.Wait();
        }

        private async Task ExecuteAsync()
        {
            while (!cts.Token.IsCancellationRequested)
            {
                await Check();
            }
        }

        public async Task Check()
        {
            var processes = Process.GetProcesses();

            foreach (var processDefinition in definitions)
            {
                var foundProcesses = processes.Where(p => p.ProcessName == processDefinition.ProcessName).ToList();
                if (!foundProcesses.Any()) continue;

                foundProcesses.ForEach(fp =>
                {
                    var runTime = DateTime.Now - fp.StartTime;
                    if (runTime > processDefinition.MaxRunTime)
                    {
                        fp.Kill(true);
                    }
                });
            }

            await Task.Delay(checkInterval);
        }
    }
}