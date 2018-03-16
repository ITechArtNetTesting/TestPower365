param(
	[Parameter(Mandatory=$true)]
	[string]$localLogin,
	[Parameter(Mandatory=$true)]
	[string]$localPassword, 
	[Parameter(Mandatory=$true)]
	[string]$localExchangePowerShellUri,

    [Parameter(Mandatory=$true)]
	[string]$azureAdSyncLogin,
	[Parameter(Mandatory=$true)]
	[string]$azureAdSyncPassword, 
	[Parameter(Mandatory=$true)]
	[string]$azureAdSyncServer,

	[Parameter(Mandatory=$true)]
	[string]$cloudLogin, 
	[Parameter(Mandatory=$true)]
	[string]$cloudPassword, 

	[Parameter(Mandatory=$true)]
	[string]$testMailboxPassword,
	[Parameter(Mandatory=$true)] 
	[string]$testMailboxUPNSuffix, 
	[Parameter(Mandatory=$true)]
	[string]$testMailboxOU, 
	[Parameter(Mandatory=$true)] 
	[string]$testMailboxNamePrefix,
		
	[Parameter(Mandatory=$true)]
	[string]$p365DiscoveryGroup,

	[string]$msolUri = "https://ps.outlook.com/powershell",
	[string]$msolConnectParams = "",

	[switch]$simulationMode
)

#QA:  .\Integration_Probe.ps1 -localLogin corp7\Administrator -localPassword BinTree123 -localExchangePowerShellUri https://10.1.137.25/PowerShell -azureAdSyncLogin corp7\Administrator -azureAdSyncPassword BinTree123 -azureAdSyncServer W28DS -cloudLogin c7o365admin@btcorp7.onmicrosoft.com -cloudPassword BinTree123 -testMailboxPassword Password1 -testMailboxUPNSuffix corp7.cmtsandbox.com -testMailboxOU P365Probe -testMailboxNamePrefix P365QAUserProbe -p365DiscoveryGroup P365ProbeGrp -simulationMode

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
	Write-Host ("localLogin: $($localLogin)")
	Write-Host ("localPassword: $($localPassword)")
	Write-Host ("localExchangePowerShellUri: $($localExchangePowerShellUri)")
    Write-Host ("")
    Write-Host ("azureAdSyncLogin: $($azureAdSyncLogin)")
	Write-Host ("azureAdSyncPassword: $($azureAdSyncPassword)")
	Write-Host ("azureAdSyncServer: $($azureAdSyncServer)")
    Write-Host ("")
	Write-Host ("cloudLogin: $($cloudLogin)")
	Write-Host ("cloudPassword: $($cloudPassword)")
    Write-Host ("")
	Write-Host ("testMailboxPassword: $($testMailboxPassword)")
	Write-Host ("testMailboxUPNSuffix: $($testMailboxUPNSuffix)")
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

[System.Int32] $retryDelay = 10
[System.Int32] $retryLimit = 10

$date = Get-Date -Format "MMddHHmm"
$mailboxName = [string]::Format("{0}_{1}", $testMailboxNamePrefix, $date)

$localCredentials = New-CredentialFromClear $localLogin $localPassword
$cloudCredentials = New-CredentialFromClear $cloudLogin $cloudPassword
$azureAdSyncCredentials = New-CredentialFromClear $azureAdSyncLogin $azureAdSyncPassword

$testMailboxPasswordSecureString = ConvertTo-SecureString $testMailboxPassword -AsPlainText -Force

$sessionOptions = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck

$localSession = $null
$cloudSession = $null
$azureAdSyncSession = $null


try
{
    Write-Settings

	Write-Host ("Connecting to Local Exchange")

    $localSession = New-PSSession -ErrorAction Stop -ConfigurationName "Microsoft.Exchange" -ConnectionUri $localExchangePowerShellUri -Credential $localCredentials -Authentication Basic -SessionOption $sessionOptions
    Import-PSSession $localSession


	$remoteMailboxes = Get-RemoteMailbox “$testMailboxNamePrefix*”

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
		$newRemoteMailbox = New-RemoteMailbox -Alias $mailboxName -Name $mailboxName -UserPrincipalName $mailboxName@$testMailboxUPNSuffix -LastName Test -FirstName $mailboxName -OnPremisesOrganizationalUnit $testMailboxOU -Password $testMailboxPasswordSecureString  
	}
    else
    {
        Write-Host ("SIMULATION: Creating Mailbox: $($mailboxName)")
    }


	if(!$simulationMode)
	{
        Write-Host ("Adding $($mailboxName) to group $($p365DiscoveryGroup).")
		Add-DistributionGroupMember -Member $mailboxName -Identity $p365DiscoveryGroup
	}
    else
    {
        Write-Host ("SIMULATION: Adding $($mailboxName) to group $($p365DiscoveryGroup).")
    }
	
    Write-Host("Removing PSSession localSession")
    Remove-PSSession $localSession
    $localSession = $null

    Write-Host("Setting Trusted Hosts")
    $winRMSettings = [string]::Format('@{{TrustedHosts="{0}"}}', $azureAdSyncServer)
    winrm set winrm/config/client $winRMSettings

	Write-Host("Connecting to Azure AD Sync")
    $azureAdSyncSession = New-PSSession -ErrorAction Stop -ComputerName $azureAdSyncServer -Credential $azureAdSyncCredentials -SessionOption $sessionOptions

    if(!$simulationMode)
    {
        Write-Host("Invoking Azure AD Delta Sync")
        Start-AzureADSyncAndWait $azureAdSyncSession
    }
    else
    {
        Write-Host("SIMULATION: Invoking Azure AD Delta Sync")
    }

    Remove-PSSession $azureAdSyncSession
    $azureAdSyncSession = $null

	Write-Host ("Connecting to O365 Tenant")
    $cloudSession= New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $msolUri -Credential $cloudCredentials -Authentication Basic -AllowRedirection
    Import-PSSession $cloudSession
    Import-Module msonline
	$connectMsolCommand = 'Connect-MsolService -ErrorAction Stop -Credential $cloudCredentials ' + $msolConnectParams
    Invoke-Expression $connectMsolCommand

    $msolUser = $null
    $count = 0

    do
    {
        Write-Host ("Trying to locate $($mailboxName) MS Online User, attempt $($count + 1)...")
        $msolUser = Get-MsolUser -SearchString $mailboxName
        $count++
        Start-Sleep -Seconds $retryDelay
    }
    while(!$msolUser -and ($count -lt $retryLimit))

    if($msolUser)
    {
        Write-Host ("$($mailboxName) MS Online User found!")
        $msolUser | fl
    }
    else
    {
        throw "$($mailboxName) MS Online User could not be found!"
    }

    Write-Host("Removing PSSession cloudSession")
    Remove-PSSession $cloudSession
    $cloudSession = $null
}
catch
{
	Write-Error ("An error occured. $($_.Exception)")
    return -1;
}
finally
{
    if($localSession)
    {
        Write-Host("Removing PSSession localSession")
        Remove-PSSession $localSession
    }

    if($cloudSession)
    {
        Write-Host("Removing PSSession cloudSession")
        Remove-PSSession $cloudSession
    }

    if($azureAdSyncSession)
    {
        Write-Host("Removing PSSession azureAdSyncSession")
        Remove-PSSession $azureAdSyncSession
    }
}