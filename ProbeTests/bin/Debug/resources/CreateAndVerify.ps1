param(
	[Parameter(Mandatory=$true)]
	[string]$sourceLocalLogin, 
	[Parameter(Mandatory=$true)]
	[string]$sourceLocalPassword, 
	[Parameter(Mandatory=$true)]
	[string]$sourceLocalExchangePowerShellUri, 

	[Parameter(Mandatory=$true)]
	[string]$targetLocalLogin, 
	[Parameter(Mandatory=$true)]
	[string]$targetLocalPassword, 
	[Parameter(Mandatory=$true)]
	[string]$targetLocalExchangePowerShellUri, 

	[Parameter(Mandatory=$true)]
	[string]$testObjectNamePrefix, 
	[Parameter(Mandatory=$true)]
	[string]$testObjectOU, 
	[Parameter(Mandatory=$true)]
	[string]$testObjectUPNSuffix, 
	[Parameter(Mandatory=$true)]
	[string]$testObjectPassword, 

	[switch]$simulationMode
)

#.\MigrationProbe.ps1 -sourceLocalLogin "administrator@corp29.cmtsandbox.com" -sourceLocalPassword "Password29" -sourceLocalExchangePowerShellUri "https://10.1.141.20/powershell" -targetLocalLogin "administrator@corp30.cmtsandbox.com" -targetLocalPassword "Password30" -targetLocalExchangePowerShellUri "https://10.1.141.40/powershell" -testObjectNamePrefix "cds_probe" -testObjectOU "CDSProbe" -testObjectUPNSuffix "corp29.cmtsandbox.com" -testObjectPassword "Password1" -simulationMode

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


[System.Int32] $retryDelay = 60
[System.Int32] $retryLimit = 20

$date = Get-Date -Format "MMddHHmm"
$month = (get-date).Month
$objectNameUser = [string]::Format("{0}{1}u", $testObjectNamePrefix, $date)
$objectNameGroup = [string]::Format("{0}{1}g", $testObjectNamePrefix, $date)
$sourceLocalCreds = New-CredentialFromClear $sourceLocalLogin $sourceLocalPassword

$targetLocalCreds = New-CredentialFromClear $targetLocalLogin $targetLocalPassword

$testObjectPasswordSecureString  = ConvertTo-SecureString $testObjectPassword -AsPlainText -Force

$sessionOptions = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck

$sourceLocalSession = $null
$targetLocalSession = $null

try
{
	Write-Settings

	#region Source Local Mailbox Creation
	Write-Host ("Connecting to Source Local Exchange")
	$sourceLocalSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $sourceLocalExchangePowerShellUri -Credential $sourceLocalCreds -Authentication Basic -SessionOption $sessionOptions
	Import-PSSession $sourceLocalSession

	$mailUsers = Get-MailUser “*$testObjectNamePrefix*”
    $distGroups = Get-DistributionGroup "*$testObjectNamePrefix*"
    $mailUsers | fl
    $distGroups | fl

	if(!$simulationMode)
	{
        Write-Host ("Removing existing probe mail user")
		$mailUsers | Remove-MailUser -Confirm:$false
        Write-Host ("Removing existing probe distribution groups")
		$distGroups | Remove-DistributionGroup -Confirm:$false
	}
    else
    {
        Write-Host ("SIMULATION: Removing existing probe mail user")
        Write-Host ("SIMULATION: Removing existing probe distribution groups")
    }

	if(!$simulationMode)
	{
        Write-Host ("Creating Mail User: $($objectNameUser)")
		$newMailUser = New-MailUser -Name $objectNameUser -DisplayName $objectNameUser -UserPrincipalName $objectNameUser@$testObjectUPNSuffix -LastName Test -FirstName $objectNameUser -OrganizationalUnit $testObjectOU -SamAccountName $objectNameUser -PrimarySmtpAddress $objectNameUser@$testObjectUPNSuffix -ExternalEmailAddress $objectNameUser@$testObjectUPNSuffix -Password $testObjectPasswordSecureString  
        Write-Host ("Creating Distribution Group: $($objectName)")
		$newDistGroup = New-DistributionGroup -DisplayName $objectNameGroup -Name $objectNameGroup -SamAccountName $objectNameGroup -OrganizationalUnit $testObjectOU -PrimarySmtpAddress $objectNameGroup@$testObjectUPNSuffix
	}
    else
    {
        Write-Host ("SIMULATION: Creating Mail User: $($objectNameUser)")
        Write-Host ("SIMULATION: Creating Distribution Group: $($objectNameGroup)")
    }
    
	Write-Host("Removing PSSession sourceLocalSession")
    Remove-PSSession $sourceLocalSession
    $sourceLocalSession = $null
	#endregion


	#region Target Local Mailbox Creation
	Write-Host ("Connecting to Target Local Exchange")
	$targetLocalSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $targetLocalExchangePowerShellUri -Credential $targetLocalCreds -Authentication Basic -SessionOption $sessionOptions
	Import-PSSession $targetLocalSession

    $targetMailUser = $null
    $count = 0

    do
    {
        Write-Host ("Trying to locate $($objectNameUser) User in Target, attempt $($count + 1)...")
        $targetMailUser = Get-MailUser -ErrorAction SilentlyContinue $objectNameUser
        if($targetMailUser)
        {
            Write-Host ("$($objectNameUser) found!")
            $targetMailUser | fl
            break;
        }
        $count++
        Start-Sleep -Seconds $retryDelay
    }
    while(!$targetMailUser -and ($count -lt $retryLimit))

    if(!$targetMailUser -and !$simulationMode)
    {
        throw "$($targetMailUser) user could not be found!"
    }

    $targetDistributionGroup = $null

    do
    {
        Write-Host ("Trying to locate $($objectNameGroup) Group in Target, attempt $($count + 1)...")
        $targetDistributionGroup = Get-DistributionGroup -ErrorAction SilentlyContinue $objectNameGroup
        if($targetMailUser)
        {
            Write-Host ("$($objectNameGroup) found!")
            $targetDistributionGroup | fl
            break;
        }
        $count++
        Start-Sleep -Seconds $retryDelay
    }
    while(!$targetDistributionGroup -and ($count -lt $retryLimit))

    if(!$targetDistributionGroup -and !$simulationMode)
    {
        throw "$($targetDistributionGroup) group could not be found!"
    }

	Write-Host("Removing PSSession targetLocalSession")
    Remove-PSSession $targetLocalSession
    $targetLocalSession = $null

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
}