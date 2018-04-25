param(
	[Parameter(Mandatory=$true)]
	[string]$sourceLocalLogin, 
	[Parameter(Mandatory=$true)]
	[string]$sourceLocalPassword, 
	[Parameter(Mandatory=$true)]
	[string]$sourceLocalExchangePowerShellUri, 

	[Parameter(Mandatory=$true)]
	[string]$sourceAzureAdSyncLogin,
	[Parameter(Mandatory=$true)]
	[string]$sourceAzureAdSyncPassword, 
	[Parameter(Mandatory=$true)]
	[string]$sourceAzureAdSyncServer,

	[Parameter(Mandatory=$true)]
	[string]$sourceCloudLogin, 
	[Parameter(Mandatory=$true)]
	[string]$sourceCloudPassword, 

	[Parameter(Mandatory=$true)]
	[string]$targetLocalLogin, 
	[Parameter(Mandatory=$true)]
	[string]$targetLocalPassword, 
	[Parameter(Mandatory=$true)]
	[string]$targetLocalExchangePowerShellUri, 

	[Parameter(Mandatory=$true)]
	[string]$targetAzureAdSyncLogin,
	[Parameter(Mandatory=$true)]
	[string]$targetAzureAdSyncPassword, 
	[Parameter(Mandatory=$true)]
	[string]$targetAzureAdSyncServer,

	[Parameter(Mandatory=$true)]
	[string]$targetCloudLogin,
	[Parameter(Mandatory=$true)]
	[string]$targetCloudPassword,

	[Parameter(Mandatory=$true)]
	[string]$testMailboxNamePrefix, 
	[Parameter(Mandatory=$true)]
	[string]$testMailboxOU, 
	[Parameter(Mandatory=$true)]
	[string]$testMailboxSourceUPNSuffix, 
	[Parameter(Mandatory=$true)]
	[string]$testMailboxTargetUPNSuffix, 
	[Parameter(Mandatory=$true)]
	[string]$testMailboxPassword, 
	[Parameter(Mandatory=$true)]
	[string]$p365DiscoveryGroup, 

	[string]$msolUri = "https://ps.outlook.com/powershell", 
	[string]$msolConnectParams = "",

	[switch]$simulationMode
)

#.\NewProbeUser.ps1 -sourceLocalLogin "administrator@corp29.cmtsandbox.com" -sourceLocalPassword "Password29" -sourceLocalExchangePowerShellUri "https://10.1.141.20/powershell" -sourceAzureAdSyncLogin "administrator@corp29.cmtsandbox.com" -sourceAzureAdSyncPassword "Password29" -sourceAzureAdSyncServer "W12-C29-EX13" -sourceCloudLogin "QAProbeAdmin@btcorp29.onmicrosoft.com" -sourceCloudPassword "Password29" -targetLocalLogin "administrator@corp30.cmtsandbox.com" -targetLocalPassword "Password30" -targetLocalExchangePowerShellUri "https://10.1.141.40/powershell" -targetAzureAdSyncLogin "administrator@corp30.cmtsandbox.com" -targetAzureAdSyncPassword "Password30" -targetAzureAdSyncServer "W12-C30-EX13"  -targetCloudLogin "QAProbeAdmin@corp30.cmtsandbox.com" -targetCloudPassword "Password30" -testMailboxNamePrefix "qamailprobe" -testMailboxOU "P365QAProbe" -testMailboxSourceUPNSuffix "@corp29.cmtsandbox.com" -testMailboxTargetUPNSuffix "@corp30.cmtsandbox.com" -testMailboxPassword "Password1" -p365DiscoveryGroup "BTQAMonitor-1" -msolUri "https://ps.outlook.com/powershell" -simulationMode

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
	Write-Host ("sourceLocalLogin: $($sourceLocalLogin)")
	Write-Host ("sourceLocalPassword: $($sourceLocalPassword)")
	Write-Host ("sourceLocalExchangePowerShellUri: $($sourceLocalExchangePowerShellUri)")
    Write-Host ("")
    Write-Host ("sourceAzureAdSyncLogin: $($sourceAzureAdSyncLogin)")
	Write-Host ("sourceAzureAdSyncPassword: $($sourceAzureAdSyncPassword)")
	Write-Host ("sourceAzureAdSyncServer: $($sourceAzureAdSyncServer)")
    Write-Host ("")
	Write-Host ("sourceCloudLogin: $($sourceCloudLogin)")
	Write-Host ("sourceCloudPassword: $($sourceCloudPassword)")
    Write-Host ("")
	Write-Host ("targetLocalLogin: $($targetLocalLogin)")
	Write-Host ("targetLocalPassword: $($targetLocalPassword)")
	Write-Host ("targetLocalExchangePowerShellUri: $($targetLocalExchangePowerShellUri)")
    Write-Host ("")
    Write-Host ("targetAzureAdSyncLogin: $($targetAzureAdSyncLogin)")
	Write-Host ("targetAzureAdSyncPassword: $($targetAzureAdSyncPassword)")
	Write-Host ("targetAzureAdSyncServer: $($targetAzureAdSyncServer)")
    Write-Host ("")
	Write-Host ("targetCloudLogin: $($targetCloudLogin)")
	Write-Host ("targetCloudPassword: $($targetCloudPassword)")
	Write-Host ("")
	Write-Host ("testMailboxPassword: $($testMailboxPassword)")
	Write-Host ("testMailboxSourceUPNSuffix: $($testMailboxSourceUPNSuffix)")
	Write-Host ("testMailboxTargetUPNSuffix: $($testMailboxTargetUPNSuffix)")
	Write-Host ("testMailboxOU: $($testMailboxOU)")
	Write-Host ("testMailboxNamePrefix: $($testMailboxNamePrefix)")
    Write-Host ("")
	Write-Host ("p365DiscoveryGroup: $($p365DiscoveryGroup)")
    Write-Host ("")
	Write-Host ("msolUri: $($msolUri)")
	Write-Host ("msolConnectParams: $($msolConnectParams)")
	Write-Host ("")
	Write-Host ("simulationMode: $($simulationMode)")
    Write-Host ("----------------------------------------------")
    Write-Host ("")
    Write-Host ("")
}

function Start-AzureADSyncAndWait($session)
{
    Invoke-Command -Session $session -ScriptBlock { 

        $inProgress = $null
        do
        {
            $inProgress = (Get-ADSyncScheduler | Select SyncCycleInProgress).SyncCycleInProgress
            Write-Host ("Azure AD Sync In Progress: $($inProgress)")
            if($inProgress -eq $false)
            {
                break;
            }
            Start-Sleep -Seconds 10
        }
        while($inProgress)

        Write-Host("Invoking Azure AD Delta Sync")

        Start-ADSyncSyncCycle -PolicyType Delta

        Write-Host("Waiting for Delta Sync Start")

        $inProgress = $null
        do
        {
            $inProgress = (Get-ADSyncScheduler | Select SyncCycleInProgress).SyncCycleInProgress
            Write-Host ("Azure AD Sync In Progress: $($inProgress)")
            if($inProgress -eq $true)
            {
                Write-Host("Delta Sync Started")
                break;
            }
            Start-Sleep -Seconds 10
        }
        while(!$inProgress)

        Write-Host("Waiting for Delta Sync Finish")

        $inProgress = $null
        do
        {
            $inProgress = (Get-ADSyncScheduler | Select SyncCycleInProgress).SyncCycleInProgress
            Write-Host ("Azure AD Sync In Progress: $($inProgress)")
            if($inProgress -eq $false)
            {
                Write-Host("Delta Sync Finished")
                break;
            }
            Start-Sleep -Seconds 10
        }
        while($inProgress)
    }
}

[System.Int32] $retryDelay = 30
[System.Int32] $retryLimit = 10

$date = Get-Date -Format "MMddHHmm"
$month = (get-date).Month
$mailboxName = [string]::Format("{0}_{1}_{2}",$month, $testMailboxNamePrefix, $date)
$sourceLocalCreds = New-CredentialFromClear $sourceLocalLogin $sourceLocalPassword
$sourceCloudCreds = New-CredentialFromClear $sourceCloudLogin $sourceCloudPassword
$sourceAzureAdSyncCredentials = New-CredentialFromClear $sourceAzureAdSyncLogin $sourceAzureAdSyncPassword

$targetLocalCreds = New-CredentialFromClear $targetLocalLogin $targetLocalPassword
$targetCloudCreds = New-CredentialFromClear $targetCloudLogin $targetCloudPassword
$targetAzureAdSyncCredentials = New-CredentialFromClear $targetAzureAdSyncLogin $targetAzureAdSyncPassword

$testMailboxPasswordSecureString  = ConvertTo-SecureString $testMailboxPassword -AsPlainText -Force

$sessionOptions = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck

$sourceLocalSession = $null
$sourceCloudSession = $null
$sourceAzureAdSyncSession = $null

$targetLocalSession = $null
$targetCloudSession = $null
$targetAzureAdSyncSession = $null

try
{
	Write-Settings

	#region Source Local Mailbox Creation
	Write-Host ("Connecting to Source Local Exchange")
	$sourceLocalSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $sourceLocalExchangePowerShellUri -Credential $sourceLocalCreds -Authentication Basic -SessionOption $sessionOptions
	Import-PSSession $sourceLocalSession

	$remoteMailboxes = Get-RemoteMailbox “*$testMailboxNamePrefix*”
	if(!$simulationMode)
	{
        Write-Host ("Removing existing probe remote mailboxes")
		$remoteMailboxes | Remove-RemoteMailbox -Confirm:$false
	}
    else
    {
        Write-Host ("SIMULATION: Removing existing probe remote mailboxes")
    }

	if(!$simulationMode)
	{
        Write-Host ("Creating Mailbox: $($mailboxName)")
		$newRemoteMailbox = New-RemoteMailbox -Alias $mailboxName -Name $mailboxName -UserPrincipalName $mailboxName@$testMailboxSourceUPNSuffix -LastName Test -FirstName $mailboxName -OnPremisesOrganizationalUnit $testMailboxOU -Password $testMailboxPasswordSecureString  
	}
    else
    {
        Write-Host ("SIMULATION: Creating Mailbox: $($mailboxName)")
    }
    
    $sourceMailboxUpn = $newRemoteMailbox.UserPrincipalName

	if(!$simulationMode)
	{
        Write-Host ("Adding $($sourceMailboxUpn) to group $($p365DiscoveryGroup).")
		Add-DistributionGroupMember -Member $sourceMailboxUpn -Identity $p365DiscoveryGroup
		$members = Get-DistributionGroupMember -Identity $p365DiscoveryGroup
        Write-Host ("Member count=" + $members.Count)
	}
    else
    {
        Write-Host ("SIMULATION: Adding $($mailboxName) to group $($p365DiscoveryGroup).")
    }

	Write-Host("Removing PSSession sourceLocalSession")
    Remove-PSSession $sourceLocalSession
    $sourceLocalSession = $null
	#endregion

	#region Target Local Mailbox Creation
	Write-Host ("Connecting to Target Local Exchange")
	$targetLocalSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $targetLocalExchangePowerShellUri -Credential $targetLocalCreds -Authentication Basic -SessionOption $sessionOptions
	Import-PSSession $targetLocalSession

	$remoteMailboxes = Get-RemoteMailbox “*$testMailboxNamePrefix*”
	if(!$simulationMode)
	{
        Write-Host ("Removing existing probe remote mailboxes")
		$remoteMailboxes | Remove-RemoteMailbox -Confirm:$false
	}
    else
    {
        Write-Host ("SIMULATION: Removing existing probe remote mailboxes")
    }

	if(!$simulationMode)
	{
        Write-Host ("Creating Mailbox: $($mailboxName)")
		$newRemoteTargetMailbox = New-RemoteMailbox -Alias $mailboxName -Name $mailboxName -UserPrincipalName $mailboxName@$testMailboxTargetUPNSuffix -LastName Test -FirstName $mailboxName -OnPremisesOrganizationalUnit $testMailboxOU -Password $testMailboxPasswordSecureString  
	}
    else
    {
        Write-Host ("SIMULATION: Creating Mailbox: $($mailboxName)")
    }

	Write-Host("Removing PSSession targetLocalSession")
    Remove-PSSession $targetLocalSession
    $targetLocalSession = $null

	#endregion

	#region Source Azure AD Sync
	Write-Host("Setting Trusted Hosts")
    $winRMSettings = [string]::Format('@{{TrustedHosts="{0}"}}', $sourceAzureAdSyncServer)
    winrm set winrm/config/client $winRMSettings

	Write-Host("Connecting to Source Azure AD Sync")
    $sourceAzureAdSyncSession = New-PSSession -ErrorAction Stop -ComputerName $sourceAzureAdSyncServer -Credential $sourceAzureAdSyncCredentials -SessionOption $sessionOptions

    if(!$simulationMode)
    {
        Start-AzureADSyncAndWait $sourceAzureAdSyncSession
    }
    else
    {
        Write-Host("SIMULATION: Invoking Azure AD Delta Sync")
    }
    Write-Host ("Removing PSSession sourceAzureAdSyncSession")
    Remove-PSSession $sourceAzureAdSyncSession
    $sourceAzureAdSyncSession = $null
	#endregion

	#region Target Azure AD Sync
	Write-Host("Setting Trusted Hosts")
    $winRMSettings = [string]::Format('@{{TrustedHosts="{0}"}}', $targetAzureAdSyncServer)
    winrm set winrm/config/client $winRMSettings

	Write-Host("Connecting to Target Azure AD Sync")
    $targetAzureAdSyncSession = New-PSSession -ErrorAction Stop -ComputerName $targetAzureAdSyncServer -Credential $targetAzureAdSyncCredentials -SessionOption $sessionOptions

    if(!$simulationMode)
    {
        Start-AzureADSyncAndWait $targetAzureAdSyncSession
    }
    else
    {
        Write-Host("SIMULATION: Invoking Azure AD Delta Sync")
    }
    Write-Host ("Removing PSSession targetAzureAdSyncSession")
    Remove-PSSession $targetAzureAdSyncSession
    $targetAzureAdSyncSession = $null
	#endregion

	#region Source Cloud Mailbox Verify
	Write-Host ("Connecting to Source O365 Tenant")
    $sourceCloudSession= New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $msolUri -Credential $sourceCloudCreds -Authentication Basic -AllowRedirection
    Import-PSSession $sourceCloudSession
    Import-Module msonline
	$connectMsolCommand = 'Connect-MsolService -ErrorAction Stop -Credential $sourceCloudCreds ' + $msolConnectParams
    Invoke-Expression $connectMsolCommand

    $sourceMbx = $null
    $count = 0

    do
    {
        Write-Host ("Trying to locate $($mailboxName) Mailbox in Source, attempt $($count + 1)...")
        $sourceMbx = Get-Mailbox -ErrorAction SilentlyContinue $mailboxName
        if($sourceMbx)
        {
            Write-Host ("$($sourceMbx) mailbox found!")
            $sourceMbx | fl
            break;
        }
        $count++
        Start-Sleep -Seconds $retryDelay
    }
    while(!$sourceMbx -and ($count -lt $retryLimit))


    if(!$sourceMbx -and !$simulationMode)
    {
        throw "$($mailboxName) mailbox could not be found!"
    }

    Write-Host("Removing PSSession sourceCloudSession")
    Remove-PSSession $sourceCloudSession
    $sourceCloudSession = $null
	#endregion
	
	#region Target Cloud Mailbox Verify
	Write-Host ("Connecting to Target O365 Tenant")
    $targetCloudSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $msolUri -Credential $targetCloudCreds -Authentication Basic -AllowRedirection
    Import-PSSession $targetCloudSession
    Import-Module msonline
	$connectMsolCommand = 'Connect-MsolService -ErrorAction Stop -Credential $targetCloudCreds ' + $msolConnectParams
    Invoke-Expression $connectMsolCommand

    $targetMbx = $null
    $count = 0

    do
    {
        Write-Host ("Trying to locate $($mailboxName) Mailbox in Target, attempt $($count + 1)...")
        $targetMbx = Get-Mailbox -ErrorAction SilentlyContinue $mailboxName
        if($targetMbx)
        {
            Write-Host ("$($targetMbx) mailbox found!")
            $targetMbx | fl
            break;
        }
        $count++
        Start-Sleep -Seconds $retryDelay
    }
    while(!$targetMbx -and ($count -lt $retryLimit))

    if(!$targetMbx -and !$simulationMode)
    {
        throw "$($mailboxName) mailbox could not be found!"
    }

    Write-Host("Removing PSSession targetCloudSession")
    Remove-PSSession $targetCloudSession
    $targetCloudSession = $null
	#endregion
}
catch
{
	Write-Error ("An error occured. $($_.Exception)")
    return -1;
}
finally
{
    if($sourceLocalSession)
    {
        Write-Host("Removing PSSession sourceLocalSession")
        Remove-PSSession $sourceLocalSession
    }

    if($targetLocalSession)
    {
        Write-Host("Removing PSSession targetLocalSession")
        Remove-PSSession $targetLocalSession
    }

    if($sourceAzureAdSyncSession)
    {
        Write-Host("Removing PSSession sourceAzureAdSyncSession")
        Remove-PSSession $sourceAzureAdSyncSession
    }

	if($targetAzureAdSyncSession)
    {
        Write-Host("Removing PSSession targetAzureAdSyncSession")
        Remove-PSSession $targetAzureAdSyncSession
    }

    if($sourceCloudSession)
    {
        Write-Host("Removing PSSession sourceCloudSession")
        Remove-PSSession $sourceCloudSession
    }

	if($targetCloudSession)
    {
        Write-Host("Removing PSSession targetCloudSession")
        Remove-PSSession $targetCloudSession
    }
}