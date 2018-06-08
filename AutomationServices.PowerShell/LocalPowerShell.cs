using PS = System.Management.Automation.PowerShell;

namespace AutomationServices.PowerShell
{

    public class LocalPowerShell : PowerShellBase
    {
        public LocalPowerShell()
        {
            Session = PS.Create();
            Session.Runspace = Runspace;
        }

        protected override void Dispose(bool disposing) { }
    }
}