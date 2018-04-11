powershell -command "& {Set-ExecutionPolicy -Scope LocalMachine Unrestricted -Force}"
powershell ".\resources\MailUserProbe-Cleanup.ps1"