using System;
using System.Management.Automation;

namespace AutomationServices.PowerShell
{


    public class Office365PowerShell : RemotePowerShell
    {
        public Office365PowerShell(string remoteHostUri, string username, string password)
            : base(remoteHostUri, username, password, false) { }

        public void Connect()
        {
            PSCommand newSession = new PSCommand();
            newSession.AddCommand("New-PSSession");
            newSession.AddParameter("ConfigurationName", "Microsoft.Exchange");
            newSession.AddParameter("ConnectionUri", new Uri(RemoteHostUri));
            newSession.AddParameter("Credential", Credential);
            newSession.AddParameter("Authentication", "Basic");

            var result = Invoke<PSObject>(newSession);

            PSCommand importSession = new PSCommand();
            importSession.AddCommand("Import-PSSession");
            importSession.AddParameter("Session", result[0]);

            Invoke<PSObject>(importSession);
        }
    }
}