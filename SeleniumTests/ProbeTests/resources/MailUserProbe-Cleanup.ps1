param([string]$slogin = "Not_set", [string]$spassword = "Not_set", [string]$tlogin = "Not_set", [string]$tpassword = "Not_set", [string]$probeprefix = "Not_set", [string]$sourceuri = "https://10.1.141.20/PowerShell", [string]$targeturi ="https://10.1.141.40/PowerShell")
If ($slogin -eq "Not_set" -or $spassword -eq "Not_set" -or $tlogin -eq "Not_set" -or $tpassword -eq "Not_set" -or $probeprefix -eq "Not_set" -or $sourceuri -eq "Not_set" -or $targeturi -eq "Not_set") 
 {
 [xml]$XmlDocument = Get-Content -Path ($PSScriptRoot + "\probeRun.xml")
 $sourceTenant = $XmlDocument.xml.DiscoveryProbe.tenants.Substring(0,$XmlDocument.xml.DiscoveryProbe.tenants.IndexOf("-"))
 $targetTenant = $XmlDocument.xml.DiscoveryProbe.tenants.Substring($XmlDocument.xml.DiscoveryProbe.tenants.IndexOf(">")+1)
 $sourceNode = $XmlDocument.xml.tenants.tenant | Where-Object {$_.metaname -eq $sourceTenant}
 $targetNode = $XmlDocument.xml.tenants.tenant | Where-Object {$_.metaname -eq $targetTenant}
 $projectNode = $XmlDocument.xml.clients.client.projects.project | Where-Object {$_.metaname -eq "project2"}
 $slogin = $sourceNode.aduser
 $spassword = $sourceNode.adpassword
 $tlogin = $targetNode.aduser
 $tpassword = $targetNode.adpassword
 $sourceuri = $sourceNode.uri
 $targeturi = $targetNode.uri
 $probeprefix = $projectNode.usermigration.entry.probeprefix
 }
function New-CredentialFromClear([string]$username, [string]$password)
{
	$ss = new-object System.Security.SecureString
	$password.ToCharArray() | % { $ss.AppendChar($_) }
	return new-object System.Management.Automation.PSCredential($username, $ss)
}


[System.Int32] $SleepSeconds = 10
[System.Int32] $TimeOutMinutes = 1
$Timeout = new-timespan -Seconds $TimeOutMinutes
$loop = 1
$Month = (Get-Date).AddMonths(-1).Month
$Date = Get-Date -Format "MMddHHmm"
$Mailbox = [string]::Format("{0}_{1}",$Month,$probeprefix)
$Creds = New-CredentialFromClear $slogin $spassword
$Credt = New-CredentialFromClear $tlogin $tpassword
$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck 
$ResultCode = 0
$SleepTimer = 30


#Connecting to Source On-Prem
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Target")
            $SessionOPS = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri $sourceuri -Credential $Creds -Authentication Basic -SessionOption $so 
            Import-PSSession $SessionOPS
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }



    Start-Sleep -Seconds $SleepTimer

	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Removing Old Mailbox")
            Get-RemoteMailbox $Mailbox* | Remove-RemoteMailbox -Confirm:$false
		}
		catch
		{
			$mgs = 'Unable to find previously configured mailbox'
			Write-Error $mgs
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

Write-Host ("Removing Source Session")
Remove-pssession $SessionOPS


#Connecting to Target On-Prem
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Target")
            $SessionOPT = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri $targeturi -Credential $Credt -Authentication Basic -SessionOption $so 
            Import-PSSession $SessionOPT
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }



    Start-Sleep -Seconds $SleepTimer

	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Removing Old Mailbox")
            Get-RemoteMailbox $Mailbox* | Remove-RemoteMailbox -Confirm:$false
		}
		catch
		{
			$mgs = 'Unable to find previously configured mailbox'
			Write-Error $mgs
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

Write-Host ("Removing Source Session")
Remove-pssession $SessionOPT