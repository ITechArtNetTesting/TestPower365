using AutomationServices.PowerShell;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test
{
    public abstract class PowerShellTestBase : TestBase
    {
        protected PowerShellTestBase()
            : base() { }

        protected void RunScript(string scriptPath, string parameters, bool is32Bit = false, int exitTimeoutSec = 60)
        {
            var powerShellService = Automation.GetService<PowerShellService>();

            using (var powershellProcess = powerShellService.RunPowershellScript(scriptPath, parameters, is32Bit))
            {
                Task stdout = Task.Run(() =>
                {
                    while (!powershellProcess.StandardOutput.EndOfStream)
                    {
                        var line = powershellProcess.StandardOutput.ReadLine();
                        Logger.Info(line);
                        ScriptOutputHandler(line);
                    }
                });

                Task stderr = Task.Run(() =>
                {
                    while (!powershellProcess.StandardError.EndOfStream)
                    {
                        var line = powershellProcess.StandardError.ReadLine();
                        Logger.Error(line);
                        ScriptOutputHandler(line, true);
                    }
                });

                Task.WaitAll(stderr, stdout);
                powershellProcess.WaitForExit(exitTimeoutSec * 1000);

                stdout.Dispose();
                stderr.Dispose();
            }
        }

        protected abstract void ScriptOutputHandler(string line, bool isError = false);
    }
}
