using System;

namespace HungProcessKiller
{
    public class ProcessDefinition
    {
        public string ProcessName { get; set; }
        public TimeSpan MaxRunTime { get; set; }
    }
}
