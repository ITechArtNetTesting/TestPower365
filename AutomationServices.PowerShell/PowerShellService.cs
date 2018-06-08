using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AutomationServices.PowerShell
{
    public class PowerShellService
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        public PowerShellService()
        {

        }

        public Process RunPowershellScript(string scriptPath, string parameters, bool is32Bit = false)
        {
            var ptr = IntPtr.Zero;
            var isWow64FsRedirectionDisabled = Wow64DisableWow64FsRedirection(ref ptr);

            var processPath = string.Format(@"C:\Windows\{0}\WindowsPowerShell\v1.0\powershell.exe", is32Bit ? "SysWOW64" : "System32");
            var processArgs = string.Format(@"-NoProfile -ExecutionPolicy unrestricted -File {0} {1}", scriptPath, parameters);

            ProcessStartInfo processInfo = new ProcessStartInfo(processPath, processArgs);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;

            return Process.Start(processInfo);
        }

        public RemotePowerShell GetRemotePowerShellInstance(string host, string username, string password, bool useWSMan = true)
        {
            return new RemotePowerShell(host, username, password, useWSMan);
        }
    }
}
