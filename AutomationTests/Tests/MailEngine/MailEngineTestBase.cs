using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Threading;

namespace Product.Tests.MailEngine
{
    [TestClass]
    public class MailEngineTestBase: LoginAndConfigureTest
    {
        protected static InitialSessionState InitialState;

        protected class TestResults
        {
            public string TestCase { get; private set; }
            public string Description { get; private set; }

            public string TestPrepareResult { get; private set; }
            public DateTime TestPrepareLastRun { get; private set; }
            public string ValidationResult { get; private set; }
            public DateTime VaildationLastRun { get; private set; }

            public string OverallResult { get; private set; }

            public TestResults(PSObject psObject)
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
        
        protected static void InitialStateSetup()
        {
            var assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(MailEngineTestBase)).Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            string modulePath = Path.Combine(assemblyDirectory, "resources", "MailEngine\\Power365-Test.psd1");
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
            Log.DebugFormat("ConnectToMailboxes: sourceAdminUser: {0}, targetAdminUser: {1}, sourceMailbox: {2}, targetMailbox: {3}", sourceAdminUser, targetAdminUser, sourceMailbox, targetMailbox);

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
            Log.DebugFormat("Invoke<T>: {0}", cmd);

            powerShell.Streams.ClearStreams();

            PSCommand testCommand = new PSCommand();
            testCommand.AddCommand(cmd);

            foreach (var p in parameters)
            {
                testCommand.AddParameter(p.Key, p.Value);
            }

            powerShell.Commands = testCommand;

            T result = convertReturn(powerShell.Invoke());

            LogStreams(powerShell);

            return result;
        }

        protected T Invoke<T, PT>(PowerShell powerShell, string cmd, Func<Collection<PT>, T> convertReturn, params KeyValuePair<string, object>[] parameters)
        {
            Log.DebugFormat("Invoke<T, PT>: {0}", cmd);

            powerShell.Streams.ClearStreams();

            PSCommand command = new PSCommand();
            command.AddCommand(cmd);

            foreach (var p in parameters)
            {
                command.AddParameter(p.Key, p.Value);
            }

            powerShell.Commands = command;

            T result = convertReturn(powerShell.Invoke<PT>());

            LogStreams(powerShell);

            return result;
        }

        protected void Invoke(PowerShell powerShell, string cmd, params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                Log.DebugFormat("Invoke: {0}", cmd);

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
            finally
            {
                LogStreams(powerShell);
            }
        }

        protected TestResults GetTestResults(PowerShell powerShell, string testId)
        {
            return Invoke<TestResults, Dictionary<string, PSObject>>(powerShell, "Get-P365TestResults", (res) =>
            {
                var firstDictionary = res.FirstOrDefault();
                var tr = firstDictionary[string.Format("Test{0}", testId)];
                return new TestResults(tr);
            });
        }

        protected void AssertTestPasses(PowerShell powerShell, string testId, string inputId, Action<string> userInterfaceActions, params KeyValuePair<string, object>[] parameters)
        {
            Invoke(powerShell, string.Format("Invoke-Test{0}", testId), parameters);

            var testPrepareResults = GetTestResults(powerShell, testId);
            
            Log.Debug(testPrepareResults);

            Assert.AreEqual("Succeeded", testPrepareResults.TestPrepareResult);

            userInterfaceActions(inputId);

            TestResults validateResults = null;

            try
            {
                Invoke(powerShell, string.Format("Invoke-Validate{0}", testId));
            }
            finally
            {
                validateResults = GetTestResults(powerShell, testId);
                Log.Debug(validateResults);
                Assert.AreEqual("Succeeded", validateResults.ValidationResult);
                Assert.AreEqual("Succeeded", validateResults.OverallResult);
            }
        }

        protected void LogStreams(PowerShell powerShell)
        {
            Log.Info("LogStreams Start");
            if (powerShell.Streams == null)
                return;

            if(powerShell.Streams.Debug != null)
            {
                foreach (var debug in powerShell.Streams.Debug)
                {
                    Log.Debug(debug?.Message);
                }
            }
            
            if(powerShell.Streams.Information != null)
            {
                foreach (var info in powerShell.Streams.Information)
                {
                    Log.Info(info?.MessageData);
                }
            }
            
            if(powerShell.Streams.Warning != null)
            {
                foreach (var warn in powerShell.Streams.Warning)
                {
                    Log.Warn(warn?.Message);
                }
            }
            
            if(powerShell.Streams.Error != null)
            {
                foreach (var error in powerShell.Streams.Error)
                {
                    Log.Error(error?.ErrorDetails?.Message, error?.Exception);
                }
            }
            Log.Info("LogStreams End");
        }
    }
}
