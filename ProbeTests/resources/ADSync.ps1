param(
	[Parameter(Mandatory=$true)]
	[string]$server, 
	[Parameter(Mandatory=$true)]
	[string]$login, 
	[Parameter(Mandatory=$true)]
	[string]$password,
	[switch]$simulationMode
)

function New-CredentialFromClear([string]$username, [string]$password)
{
	$ss = new-object System.Security.SecureString
	$password.ToCharArray() | % { $ss.AppendChar($_) }
	return new-object System.Management.Automation.PSCredential($username, $ss)
}

function Write-Settings()
{
	Write-Host ("SETTINGS: ")
    Write-Host ("----------------------------------------------")
	Write-Host ("server: $($server)")
	Write-Host ("login: $($login)")
	Write-Host ("password: $($password)")
	Write-Host ("")
	Write-Host ("simulationMode: $($simulationMode)")
	Write-Host ("----------------------------------------------")
	Write-Host ("")
	Write-Host ("")
}

$session = $null

try
{
	
	Write-Settings

	if(!$simulationMode)
    {
        Write-Host("Setting Trusted Hosts")
        $winRMSettings = [string]::Format('@{{TrustedHosts="{0}"}}', $server)
        winrm set winrm/config/client $winRMSettings
    }

	$creds = New-CredentialFromClear $login $password

	$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck
	$session = New-PSSession -ComputerName $server -Credential $creds -SessionOption $so

	if(!$simulationMode)
    {
        Write-Host("Invoking Azure AD Delta Sync")
        Invoke-Command -Session $session -ScriptBlock { Start-ADSyncSyncCycle -PolicyType Delta }
    }
    else
    {
        Write-Host("SIMULATION: Invoking Azure AD Delta Sync")
    }
	
	return 0;
}
catch
{
	Write-Error ("An error occured. $($_.Exception)")
    return -1;
}
finally
{
	if($session)
    {
        Write-Host("Removing PSSession")
        Remove-PSSession $session
    }

}