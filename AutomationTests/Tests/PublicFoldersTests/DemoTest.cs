﻿using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using log4net.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.PublicFoldersTests
{
	[TestClass]
	public class DemoTest : LoginAndConfigureTest
	{

		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

	    [TestMethod]
	    public void StoreDOM()
	    {
	        try
	        {
	            string login = configurator.GetValueByXpath("//metaname[text()='client1']/..//user");
	            string password = configurator.GetValueByXpath("//metaname[text()='client1']/..//password");
	            string client = configurator.GetValueByXpath("//metaname[text()='client1']/../name");
	            string projectName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
	            string sourceMailbox = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry2']/..//source");

	            LoginAndSelectRole(login, password, client);
            }
	        catch (Exception e)
	        {
                Log.Info(Driver.GetDriver(driver.GetDriverKey()).PageSource);
	           throw e;
	        }
	    }

        [TestMethod]
		public void PSwaits()
		{
			LoginAndSelectRole(configurator.GetValueByXpath("//metaname[text()='client1']/..//user"), configurator.GetValueByXpath("//metaname[text()='client1']/..//password"), configurator.GetValueByXpath("//metaname[text()='client1']/../name"));
            var launcher = new PsLauncher();
			var testProc = launcher.TestLaunchPowerShellInstance("TestScript.ps1", String.Empty, "x64");
			while (!testProc.StandardOutput.EndOfStream)
			{
				var line = testProc.StandardOutput.ReadLine();
				Log.Info(line);
				if (line.Contains("Press"))
				{
                    launcher.SuspendProcess();
				    Log.Info("SUCCESS");
					Log.Info("SUCCESS");
					Log.Info("SUCCESS");
					Thread.Sleep(5000);
				    //KeySender.SendKeyToProcess(testProc);
                    launcher.ResumeProcess();
				}
			}
			testProc.WaitForExit(5000);
			SelectProject(configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name"));
		}
	}

   

    public static class KeySender
	{
		[DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
		public static void SendKeyToProcess(Process p)
		{
			SetForegroundWindow(p.MainWindowHandle);
			SendKeys.SendWait("{ENTER}");
		}
	}
}
