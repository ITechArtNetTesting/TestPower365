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
	[string]$testGroupName,
	[Parameter(Mandatory=$true)]
	[string]$testGroupOwner,
	[Parameter(Mandatory=$true)]
	[string]$testGroupMember,

	[Parameter(Mandatory=$true)]
	[string]$testDistributionGroupPrefix,
	[Parameter(Mandatory=$true)]
	[string]$testMailboxNamePrefixArray, 
	[System.Int32]$azureAdSyncDelaySec = 7200,

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
	Write-Host ("sourceLocalLogin: $($sourceLocalLogin)")
	Write-Host ("sourceLocalPassword: $($sourceLocalPassword)")
	Write-Host ("sourceLocalExchangePowerShellUri: $($sourceLocalExchangePowerShellUri)")
    Write-Host ("")
    Write-Host ("sourceAzureAdSyncLogin: $($sourceAzureAdSyncLogin)")
	Write-Host ("sourceAzureAdSyncPassword: $($sourceAzureAdSyncPassword)")
	Write-Host ("sourceAzureAdSyncServer: $($sourceAzureAdSyncServer)")
    Write-Host ("")
	Write-Host ("targetLocalLogin: $($targetLocalLogin)")
	Write-Host ("targetLocalPassword: $($targetLocalPassword)")
	Write-Host ("targetLocalExchangePowerShellUri: $($targetLocalExchangePowerShellUri)")
    Write-Host ("")
    Write-Host ("targetAzureAdSyncLogin: $($targetAzureAdSyncLogin)")
	Write-Host ("targetAzureAdSyncPassword: $($targetAzureAdSyncPassword)")
	Write-Host ("targetAzureAdSyncServer: $($targetAzureAdSyncServer)")
	Write-Host ("")
	Write-Host ("testGroupName: $($testGroupName)")
	Write-Host ("testGroupOwner: $($testGroupOwner)")
	Write-Host ("testGroupMember: $($testGroupMember)")
	Write-Host ("testDistributionGroupPrefix: $($testDistributionGroupPrefix)")
	Write-Host ("testMailboxNamePrefixArray: $($testMailboxNamePrefixArray)")
	Write-Host ("")
	Write-Host ("azureAdSyncDelaySec: $($azureAdSyncDelaySec)")
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

$sourceLocalCreds = New-CredentialFromClear $sourceLocalLogin $sourceLocalPassword
$sourceAzureAdSyncCredentials = New-CredentialFromClear $sourceAzureAdSyncLogin $sourceAzureAdSyncPassword

$targetLocalCreds = New-CredentialFromClear $targetLocalLogin $targetLocalPassword
$targetAzureAdSyncCredentials = New-CredentialFromClear $targetAzureAdSyncLogin $targetAzureAdSyncPassword

$sessionOptions = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck

$sourceLocalSession = $null
$sourceAzureAdSyncSession = $null

$targetLocalSession = $null
$targetAzureAdSyncSession = $null

try
{
	Write-Settings

	#region Source Local Mailbox Creation
	Write-Host ("Connecting to Source Local Exchange")
	$sourceLocalSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $sourceLocalExchangePowerShellUri -Credential $sourceLocalCreds -Authentication Basic -SessionOption $sessionOptions
	Import-PSSession $sourceLocalSession

	if(!$simulationMode)
	{
		Write-Host ("Adding Distribution Group Member")
		Add-DistributionGroupMember -Identity $testGroupName -Member $testGroupMember
	}
	else
	{
		Write-Host ("SIMULATION: Adding Distribution Group Member")
	}
	
	if(!$simulationMode)
	{
		Write-Host ("Setting Distribution Group Owner")
		Set-DistributionGroup -Identity $testGroupName -managedby $testGroupOwner
	}
	else
	{
		Write-Host ("SIMULATION: Setting Distribution Group Owner")
	}

	Write-Host("Removing PSSession sourceLocalSession")
    Remove-PSSession $sourceLocalSession
    $sourceLocalSession = $null
	#endregion

	#region Target Local Mailbox Creation
	Write-Host ("Connecting to Target Local Exchange")
	$targetLocalSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $targetLocalExchangePowerShellUri -Credential $targetLocalCreds -Authentication Basic -SessionOption $sessionOptions
	Import-PSSession $targetLocalSession
	$mailboxNameArray = $testMailboxNamePrefixArray.split('|')

	$mailboxNameArray | foreach {
		$remoteMailboxes = Get-RemoteMailbox "$_*" -ErrorAction SilentlyContinue
		if(!$simulationMode)
		{
			Write-Host ("Removing remote mailboxes $($_)*")
			$remoteMailboxes | Remove-RemoteMailbox -Confirm:$false
		}
		else
		{
			Write-Host ("SIMULATION: Removing remote mailboxes $($_)*")
		}
	}

	$distGroups = Get-DistributionGroup "$testGroupNamePrefix*" -ErrorAction SilentlyContinue
	
	if(!$simulationMode)
	{
		Write-Host ("Removing Distribution Groups")
		$distGroups | Remove-DistributionGroup -Confirm:$false
	}
	else
    {
        Write-Host ("SIMULATION: Removing Distribution Groups")
    }

	Write-Host("Removing PSSession targetLocalSession")
    Remove-PSSession $targetLocalSession
    $targetLocalSession = $null

	#endregion

	#region LongWait

	Write-Host("Sleeping for $($azureAdSyncDelaySec) seconds, waiting for Azure AD Sync...");

	Start-Sleep -Seconds $azureAdSyncDelaySec

	#endregion

	##region Source Azure AD Sync
	#Write-Host("Setting Trusted Hosts")
 #   $winRMSettings = [string]::Format('@{{TrustedHosts="{0}"}}', $sourceAzureAdSyncServer)
 #   winrm set winrm/config/client $winRMSettings

	#Write-Host("Connecting to Source Azure AD Sync")
 #   $sourceAzureAdSyncSession = New-PSSession -ErrorAction Stop -ComputerName $sourceAzureAdSyncServer -Credential $sourceAzureAdSyncCredentials -SessionOption $sessionOptions

 #   if(!$simulationMode)
 #   {
 #       Start-AzureADSyncAndWait $sourceAzureAdSyncSession
 #   }
 #   else
 #   {
 #       Write-Host("SIMULATION: Invoking Azure AD Delta Sync")
 #   }
 #   Write-Host ("Removing PSSession sourceAzureAdSyncSession")
 #   Remove-PSSession $sourceAzureAdSyncSession
 #   $sourceAzureAdSyncSession = $null
	##endregion

	##region Target Azure AD Sync
	#Write-Host("Setting Trusted Hosts")
 #   $winRMSettings = [string]::Format('@{{TrustedHosts="{0}"}}', $targetAzureAdSyncServer)
 #   winrm set winrm/config/client $winRMSettings

	#Write-Host("Connecting to Target Azure AD Sync")
 #   $targetAzureAdSyncSession = New-PSSession -ErrorAction Stop -ComputerName $targetAzureAdSyncServer -Credential $targetAzureAdSyncCredentials -SessionOption $sessionOptions

 #   if(!$simulationMode)
 #   {
 #       Start-AzureADSyncAndWait $targetAzureAdSyncSession
 #   }
 #   else
 #   {
 #       Write-Host("SIMULATION: Invoking Azure AD Delta Sync")
 #   }
 #   Write-Host ("Removing PSSession targetAzureAdSyncSession")
 #   Remove-PSSession $targetAzureAdSyncSession
 #   $targetAzureAdSyncSession = $null
	##endregion
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
}