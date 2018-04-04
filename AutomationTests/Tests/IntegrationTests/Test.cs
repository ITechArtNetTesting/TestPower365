using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.IntegrationTests
{
    [TestClass]
    public class Test: LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [TestMethod]
        public void DoTest()
        {
            string targetEntry = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//target");
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string sourceMailbox1 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//source");
            string sourceMailbox2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry2']/..//source");
            string sourceMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry3']/..//source");
            string sourceMailbox6 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry6']/..//source");
            string sourceMailbox8 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry8']/..//source");
            string sourceMailbox9 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry9']/..//source");
            string sourceMailbox11 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry11']/..//source");
            string sourceMailbox11Upn = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry11']/..//upn");
            string sourceMailbox12 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry12']/..//source");
            string sourceMailbox13 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//source");
            string sourceMailbox14 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry14']/..//source");
            string sourceMailbox15 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry15']/..//source");
            string targetLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "user");
            string targetPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "password");
            string sourceMailbox1Smtp = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//smtp");
            string targetOnPremLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "aduser");
            string targetOnPremPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "adpassword");

            string slogin = targetLogin;
            string spassword = targetPassword;
            string mailbox = targetEntry.Substring(0, targetEntry.LastIndexOf("@", StringComparison.Ordinal));
            string smtp = sourceMailbox1Smtp;
            string remoteshare = sourceMailbox3.Substring(0, sourceMailbox3.LastIndexOf("@", StringComparison.Ordinal));
            string remoteroom = sourceMailbox2.Substring(0, sourceMailbox2.LastIndexOf("@", StringComparison.Ordinal));
            string mailboxremote2 = sourceMailbox6.Substring(0, sourceMailbox6.LastIndexOf("@", StringComparison.Ordinal));
            string mailboxremote2contact = sourceMailbox6;
            string mailboxremote4 = sourceMailbox8.Substring(0, sourceMailbox8.LastIndexOf("@", StringComparison.Ordinal));
            string remoteequip1 = sourceMailbox9.Substring(0, sourceMailbox9.LastIndexOf("@", StringComparison.Ordinal));
            string sam1 = sourceMailbox11.Substring(0, sourceMailbox11.LastIndexOf("@", StringComparison.Ordinal));
            string sam1upn = sourceMailbox11Upn;
            string sam2 = sourceMailbox12.Substring(0, sourceMailbox12.LastIndexOf("@", StringComparison.Ordinal));
            string adlogin = targetOnPremLogin;
            string adpassword = targetOnPremPassword;
            bool tc32195Group = false;
            bool tc32188 = false;
            bool tc32180Group = false;
            bool tc32181Group = false;
            bool tc32196 = false;
            bool tc28066Group = false;
            bool tc32390 = false;
            bool tc39398 = false;
            bool tc32179 = false;
            bool tc28078 = false;
            bool tc32623 = false;
            bool tc32624 = false;
            using (
                    var process = new PsLauncher().LaunchPowerShellInstance("IntegrationUsers-Prepare.ps1",
                        $" -slogin {slogin}" +
                        $" -spassword {spassword}" +
                        $" -mailbox {mailbox}" +
                        $" -smtp {smtp}" +
                        $" -remoteshare {remoteshare}" +
                        $" -remoteroom {remoteroom}" +
                        $" -mailboxremote2 {mailboxremote2}" +
                        $" -mailboxremote2contact {mailboxremote2contact}" +
                        $" -mailboxremote4 {mailboxremote4}" +
                        $" -remoteequip1 {remoteequip1}" +
                        $" -sam1 {sam1}" +
                        $" -sam1upn {sam1upn}" +
                        $" -sam2 {sam2}" +
                        $" -adlogin {adlogin}" +
                        $" -adpassword {adpassword}",
                        "x64"))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    Log.Info(line);
                    if (line.Contains("TC32195, 32189, 32395 Passed"))
                    {
                        tc32195Group = true;
                    }
                    if (line.Contains("TC32188 Passed"))
                    {
                        tc32188 = true;
                    }
                    if (line.Contains("TC32180, 32396 Passed"))
                    {
                        tc32180Group = true;
                    }
                    if (line.Contains("TC32181, 32396 Passed"))
                    {
                        tc32181Group = true;
                    }
                    if (line.Contains("TC32196 Passed"))
                    {
                        tc32196 = true;
                    }
                    if (line.Contains("TC28066, 39398 Passed"))
                    {
                        tc28066Group = true;
                    }
                    if (line.Contains("TC32390 Passed"))
                    {
                        tc32390 = true;
                    }
                    if (line.Contains("TC39398 Passed"))
                    {
                        tc39398 = true;
                    }
                    if (line.Contains("TC32179 Passed"))
                    {
                        tc32179 = true;
                    }
                    if (line.Contains("TC28078 Passed"))
                    {
                        tc28078 = true;
                    }
                    if (line.Contains("TC32623 Passed"))
                    {
                        tc32623 = true;
                    }
                    if (line.Contains("TC32624 Passed"))
                    {
                        tc32624 = true;
                    }
                }
                process.WaitForExit(600000);
            }
        }
    }
}
