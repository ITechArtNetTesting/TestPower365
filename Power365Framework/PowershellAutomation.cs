using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework
{
    public class PowershellAutomation
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

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
    }
}
