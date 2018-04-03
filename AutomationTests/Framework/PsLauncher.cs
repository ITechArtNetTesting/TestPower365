using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework
{
	public class PsLauncher : BaseEntity
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
	    [Flags]
	    public enum ThreadAccess : int
	    {
	        TERMINATE = (0x0001),
	        SUSPEND_RESUME = (0x0002),
	        GET_CONTEXT = (0x0008),
	        SET_CONTEXT = (0x0010),
	        SET_INFORMATION = (0x0020),
	        QUERY_INFORMATION = (0x0040),
	        SET_THREAD_TOKEN = (0x0080),
	        IMPERSONATE = (0x0100),
	        DIRECT_IMPERSONATION = (0x0200)
	    }

	    [DllImport("kernel32.dll")]
	    static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
	    [DllImport("kernel32.dll")]
	    static extern uint SuspendThread(IntPtr hThread);
	    [DllImport("kernel32.dll")]
	    static extern int ResumeThread(IntPtr hThread);
	    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
	    static extern bool CloseHandle(IntPtr handle);

	    private Process mainProcess;

        public Process LaunchPowerShellInstance(string scriptName, string parameters)
		{
			ProcessStartInfo processInfo;
			Process process;
			//Disabling architechture redirecting
			var ptr = new IntPtr();
			var isWow64FsRedirectionDisabled = Wow64DisableWow64FsRedirection(ref ptr);
			Debug.WriteLine("Is Wow64Fs Redirection disabled:\t" + isWow64FsRedirectionDisabled);
			var path = Path.GetFullPath(RunConfigurator.ResourcesPath + scriptName);
			processInfo = RunConfigurator.GetValue("arc") == "x86"
				? new ProcessStartInfo(@"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
					@"-NoProfile -ExecutionPolicy unrestricted -File """ + path + @"""" +parameters)
				: new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
					@"-ExecutionPolicy RemoteSigned -File """ + path + @"""" +parameters);
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.RedirectStandardOutput = true;
		    mainProcess = Process.Start(processInfo);
		    return mainProcess;
        }

		public Process LaunchPowerShellInstance(string scriptName, string parameters, string arcType)
		{
			ProcessStartInfo processInfo;
			Process process;
			//Disabling architechture redirecting
			var ptr = new IntPtr();
			var isWow64FsRedirectionDisabled = Wow64DisableWow64FsRedirection(ref ptr);
			Debug.WriteLine("Is Wow64Fs Redirection disabled:\t" + isWow64FsRedirectionDisabled);
			var path = Path.GetFullPath(RunConfigurator.ResourcesPath + scriptName);
			processInfo = arcType == "x86"
				? new ProcessStartInfo(@"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
					@"-NoProfile -ExecutionPolicy unrestricted -File """ + path + @"""" + parameters)
				: new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
					@"-ExecutionPolicy RemoteSigned -File """ + path + @"""" + parameters);
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            mainProcess = Process.Start(processInfo);
		    return mainProcess;
        }

		public Process TestLaunchPowerShellInstance(string scriptName, string parameters, string arcType)
		{
			ProcessStartInfo processInfo;
			//Disabling architechture redirecting
			var ptr = new IntPtr();
			var isWow64FsRedirectionDisabled = Wow64DisableWow64FsRedirection(ref ptr);
			Debug.WriteLine("Is Wow64Fs Redirection disabled:\t" + isWow64FsRedirectionDisabled);
			var path = Path.GetFullPath(RunConfigurator.ResourcesPath + scriptName);
			processInfo = arcType == "x86"
				? new ProcessStartInfo(@"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
					@"-NoProfile -ExecutionPolicy unrestricted -File """ + path + @"""" + parameters)
				: new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
					@"-ExecutionPolicy RemoteSigned -File """ + path + @"""" + parameters);
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.RedirectStandardOutput = true;
			//processInfo.RedirectStandardInput = true;
			//processInfo.RedirectStandardError = true;
            mainProcess= Process.Start(processInfo);
			return mainProcess;
		}

	    public void SuspendProcess()
	    {
	        foreach (ProcessThread pT in mainProcess.Threads)
	        {
	            IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);
	            if (pOpenThread == IntPtr.Zero)
	            {
	                continue;
	            }
	            SuspendThread(pOpenThread);
	            CloseHandle(pOpenThread);
	        }
	    }

	    public void ResumeProcess()
	    {
	        foreach (ProcessThread pT in mainProcess.Threads)
	        {
	            IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);
	            if (pOpenThread == IntPtr.Zero)
	            {
	                continue;
	            }
	            var suspendCount = 0;
	            do
	            {
	                suspendCount = ResumeThread(pOpenThread);
	            } while (suspendCount > 0);
	            CloseHandle(pOpenThread);
	        }
	    }

    }
}
