using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using PS = System.Management.Automation.PowerShell;

namespace AutomationServices.PowerShell
{

    public abstract class PowerShellBase : IDisposable
    {
        public PS Session { get; protected set; }

        protected Runspace Runspace;
        protected InitialSessionState InitialSessionState;

        public PowerShellBase()
        {
            Runspace = RunspaceFactory.CreateRunspace();
            Runspace.Open();
        }

        public Collection<T> Invoke<T>(PSCommand command)
        {
            Session.Streams.ClearStreams();
            Session.Commands = command;
            var result = Session.Invoke<T>();
            ThrowIfErrors(Session);
            return result;
        }

        protected void ThrowIfErrors(PS session)
        {
            if (session.Streams.Error.Count > 0)
                throw new Exception("Errors in result");
        }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);
            if (Runspace != null)
                Runspace.Dispose();
        }
    }
}