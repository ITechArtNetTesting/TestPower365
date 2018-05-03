using log4net;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test
{
    public abstract class PowershellTestBase : TestBase
    {
    //    [DllImport("kernel32.dll", SetLastError = true)]
    //    public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        protected PowershellTestBase(ILog logger)
            : base(logger) { }

        protected void RunScript(string scriptPath, string parameters, bool is32Bit = false, int exitTimeoutSec = 60)
        {
            using (var powershellProcess = Automation.Powershell.RunPowershellScript(scriptPath, parameters, is32Bit))
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
