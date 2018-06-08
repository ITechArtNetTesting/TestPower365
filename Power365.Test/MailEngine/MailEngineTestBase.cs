using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.IO;
using System.Net;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework;

namespace BinaryTree.Power365.Test.MailEngine
{
    public abstract class MailEngineTestBase: PowerShellTestBase
    {
        protected class MailEngineTestResult
        {
            public string TestCase { get; private set; }
            public string Description { get; private set; }

            public string TestPrepareResult { get; private set; }
            public DateTime TestPrepareLastRun { get; private set; }
            public string ValidationResult { get; private set; }
            public DateTime VaildationLastRun { get; private set; }

            public string OverallResult { get; private set; }

            public MailEngineTestResult(PSObject psObject)
            {
                TestCase = psObject.Properties["TestCase"].Value?.ToString();
                Description = psObject.Properties["Description"].Value?.ToString();
                TestPrepareResult = psObject.Properties["TestResult"].Value?.ToString();
                TestPrepareLastRun = psObject.Properties["TestLastRun"] != null ? (DateTime)((PSObject)psObject.Properties["TestLastRun"].Value).BaseObject : DateTime.MinValue;
                ValidationResult = psObject.Properties["ValidationResult"].Value?.ToString();
                VaildationLastRun = psObject.Properties["VaildationLastRun"] != null ? (DateTime)((PSObject)psObject.Properties["VaildationLastRun"].Value).BaseObject : DateTime.MinValue;
                OverallResult = psObject.Properties["OverAllResult"].Value?.ToString();
            }

            public override string ToString()
            {
                return string.Format("TestCase: {0}, Description: {1}, TestPrepareResult: {2}, ValidationResult: {3}, OverallResult: {4}", TestCase, Description, TestPrepareResult, ValidationResult, OverallResult);
            }
        }

        protected InitialSessionState InitialState;

        public MailEngineTestBase() 
            : base() { }
        
        protected void InitialStateSetup()
        {
            var assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(MailEngineTestBase)).Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            string modulePath = Path.Combine(assemblyDirectory, "Resources", "MailEngine\\Power365-Test.psd1");
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { modulePath });
            initial.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
            InitialState = initial;
        }

        protected PowerShell GetPowerShellSession()
        {
            var runspace = RunspaceFactory.CreateRunspace(InitialState);
            runspace.Open();
            var ps = PowerShell.Create();
            ps.Runspace = runspace;
            return ps;
        }
        
        protected void ConnectToMailboxes(PowerShell powerShell, string sourceAdminUser, string sourceAdminPassword, string targetAdminUser, string targetAdminPassword, string sourceMailbox, string targetMailbox, string sourceUrl, string targetUrl)
        {
            Logger.DebugFormat("ConnectToMailboxes: sourceAdminUser: {0}, targetAdminUser: {1}, sourceMailbox: {2}, targetMailbox: {3}", sourceAdminUser, targetAdminUser, sourceMailbox, targetMailbox);

            var sourceAdminPasswordSecure = new NetworkCredential("", sourceAdminPassword).SecurePassword;
            PSCredential sourceCredential = new PSCredential(sourceAdminUser, sourceAdminPasswordSecure);

            var targetAdminPasswordSecure = new NetworkCredential("", targetAdminPassword).SecurePassword;
            PSCredential targetCredential = new PSCredential(targetAdminUser, targetAdminPasswordSecure);

            PSCommand command = new PSCommand();
            command.AddCommand("Connect-P365Mailboxes");
            command.AddParameter("SourceMailbox", sourceMailbox);
            command.AddParameter("TargetMailbox", targetMailbox);
            command.AddParameter("SourceCredential", sourceCredential);
            command.AddParameter("TargetCredential", targetCredential);
            command.AddParameter("SourceURL", sourceUrl);
            command.AddParameter("TargetURL", targetUrl);

            powerShell.Commands = command;

            powerShell.Invoke();

            if (powerShell.HadErrors)
                throw new Exception("Failed to execute Connect-P365Mailboxes");
        }

        protected T Invoke<T>(PowerShell powerShell, string cmd, Func<Collection<PSObject>, T> convertReturn, params KeyValuePair<string, Object>[] parameters)
        {
            Logger.DebugFormat("Invoke<T>: {0}", cmd);

            powerShell.Streams.ClearStreams();

            PSCommand testCommand = new PSCommand();
            testCommand.AddCommand(cmd);

            foreach (var p in parameters)
            {
                testCommand.AddParameter(p.Key, p.Value);
            }

            powerShell.Commands = testCommand;

            T result = convertReturn(powerShell.Invoke());

            return result;
        }

        protected T Invoke<T, PT>(PowerShell powerShell, string cmd, Func<Collection<PT>, T> convertReturn, params KeyValuePair<string, object>[] parameters)
        {
            Logger.DebugFormat("Invoke<T, PT>: {0}", cmd);

            powerShell.Streams.ClearStreams();

            PSCommand command = new PSCommand();
            command.AddCommand(cmd);

            foreach (var p in parameters)
            {
                command.AddParameter(p.Key, p.Value);
            }

            powerShell.Commands = command;

            T result = convertReturn(powerShell.Invoke<PT>());

            return result;
        }

        protected void Invoke(PowerShell powerShell, string cmd, params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                Logger.DebugFormat("Invoke: {0}", cmd);

                powerShell.Streams.ClearStreams();

                PSCommand command = new PSCommand();
                command.AddCommand(cmd);

                foreach (var p in parameters)
                {
                    command.AddParameter(p.Key, p.Value);
                }

                powerShell.Commands = command;
                powerShell.Invoke();
            }
            catch (Exception ex)
            {
                if (powerShell.Streams.Error == null)
                    throw;

                var exceptions = powerShell.Streams.Error.Select(e => e.Exception).ToList();
                exceptions.Add(ex);
                throw new AggregateException(string.Format("Failed to run {0}", cmd), exceptions);
            }
        }
        
        protected MailEngineTestResult GetTestResults(PowerShell powerShell, string testId)
        {
            return Invoke<MailEngineTestResult, Dictionary<string, PSObject>>(powerShell, "Get-P365TestResults", (res) =>
            {
                var firstDictionary = res.FirstOrDefault();
                var tr = firstDictionary[string.Format("Test{0}", testId)];
                return new MailEngineTestResult(tr);
            });
        }

        protected void AssertTestPasses(PowerShell powerShell, string testId, string inputId, Action<string> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            Invoke(powerShell, string.Format("Invoke-Test{0}", testId), parameters);

            var testPrepareResults = GetTestResults(powerShell, testId);

            Logger.Debug(testPrepareResults);

            Assert.AreEqual("Succeeded", testPrepareResults.TestPrepareResult);

            userInterfaceActions(inputId);

            MailEngineTestResult validateResults = null;

            try
            {
                Invoke(powerShell, string.Format("Invoke-Validate{0}", testId));
            }
            finally
            {
                validateResults = GetTestResults(powerShell, testId);
                Logger.Debug(validateResults);
                Assert.AreEqual("Succeeded", validateResults.ValidationResult);
                Assert.AreEqual("Succeeded", validateResults.OverallResult);
            }
        }

        protected override void ScriptOutputHandler(string line, bool isError = false) { }
    }
}
