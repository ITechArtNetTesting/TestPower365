using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using PS = System.Management.Automation.PowerShell;

namespace AutomationServices.PowerShell
{

    public class RemotePowerShell : LocalPowerShell
    {
        protected readonly PSCredential Credential;
        protected readonly string RemoteHostUri;

        private PSObject _psSession;

        public RemotePowerShell(string remoteHostUri, string username, string password, bool useWSMan = true)
        {
            RemoteHostUri = remoteHostUri;
            Credential = new PSCredential(username, (new NetworkCredential(username, password).SecurePassword));

            if (useWSMan)
            {
                AddTrustedHost(RemoteHostUri);
                AllowUnencrypted();

                Uri remoteComputerUri = new Uri(string.Format("http://{0}:5985/wsman", remoteHostUri));
                WSManConnectionInfo connection = new WSManConnectionInfo(remoteComputerUri,
                    "http://schemas.microsoft.com/powershell/Microsoft.PowerShell",
                    Credential);

                connection.AuthenticationMechanism = AuthenticationMechanism.Basic;
                connection.NoEncryption = true;
                connection.SkipCACheck = true;
                connection.SkipCNCheck = true;
                connection.SkipRevocationCheck = true;

                //string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
                //WSManConnectionInfo connectionInfo = new WSManConnectionInfo(false, RemoteHostUri, 5985, "/wsman", shellUri, Credential);

                Runspace = RunspaceFactory.CreateRunspace(connection);
                Runspace.Open();

                Session = PS.Create();
                Session.Runspace = Runspace;
            }
        }

        protected void AddTrustedHost(string trustedHost)
        {
            var addTrustedHostsFormat = "$trusthosts = (Get-Item -Path WSMan:\\localhost\\Client\\TrustedHosts).Value" + Environment.NewLine +
            "If($trusthosts -like \"*{0}*\") {{ return; }} Else {{" + Environment.NewLine +
                "If($trusthosts -eq \"\") {{" + Environment.NewLine +
                    "Set-Item -Path WSMan:\\localhost\\Client\\TrustedHosts \"{0}\" -Force" + Environment.NewLine +
                "}} Else {{" + Environment.NewLine +
                    "Set-Item -Path WSMan:\\localhost\\Client\\TrustedHosts \"$trusthosts, {0}\" -Force" + Environment.NewLine +
                "}}" + Environment.NewLine +
            "}}";

            var addTrustedHost = string.Format(addTrustedHostsFormat, trustedHost);
            
            using (var localPowerShell = PS.Create())
            {
                localPowerShell.AddScript(addTrustedHost);
                localPowerShell.Invoke();
            }
        }

        protected void AllowUnencrypted()
        {
            var allowUnencrypted = "Set-Item -Path WSMan:\\localhost\\Client\\AllowUnencrypted $true -Force";
            
            using (var localPowerShell = PS.Create())
            {
                localPowerShell.AddScript(allowUnencrypted);
                localPowerShell.Invoke();
            }
        }
    }
}