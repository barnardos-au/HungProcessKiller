# HungProcessKiller
A Windows background service which monitors hung processes and kills them after a fixed time period

## Configuration
Edit `app.settings` and define your processes you wish to monitor.
Example:
````
CheckInterval PT30S
Definitions [{ProcessName:chromedriver,MaxRunTime:PT30M},{ProcessName:notepad,MaxRunTime:PT8H4M15S}]
````
`CheckInterval` is the interval time for checking for hung processes. The value is a `TimeSpan`. In this example it is set to `30 seconds`.

`Definitions` is an array of items to monitor. Each item consists of a `ProcessName` and a `MaxRunTime`. In this example we wish to monitor for hung `chromedriver` and `notepad` instances. The former we're allowing to run up to `30 minutes` before the process is killed, and the latter we're allowing up to `8 hours`, `4 minutes` and `15 seconds` before the process is killed.

## Install
````powershell
HungProcessKiller.exe install
HungProcessKiller.exe start
````
