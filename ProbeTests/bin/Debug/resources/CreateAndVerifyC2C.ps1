param(
  [Parameter(Mandatory = $true)]
  [string]$sourceCloudLogin,
  [Parameter(Mandatory = $true)]
  [string]$sourceCloudPassword,
  [Parameter(Mandatory = $true)]
  [string]$sourceObjectUPNSuffix,

  [Parameter(Mandatory = $true)]
  [string]$targetCloudLogin,
  [Parameter(Mandatory = $true)]
  [string]$targetCloudPassword,
  [Parameter(Mandatory = $true)]
  [string]$targetObjectUPNSuffix,

  [Parameter(Mandatory = $true)]
  [string]$testObjectNamePrefix,
  [Parameter(Mandatory = $true)]
  [string]$testDiscoveryGroup,
  [Parameter(Mandatory = $true)]
  [string]$testObjectPassword,

  [string]$msolUri = "https://ps.outlook.com/powershell",
  [string]$msolConnectParams = "",

  [switch]$simulationMode
)

#.\CreateAndVerifyC2C.ps1 -sourceCloudLogin "admin@btcorp39.onmicrosoft.com" -sourceCloudPassword "BinTree123" -sourceObjectUPNSuffix "btcorp39.onmicrosoft.com" -targetCloudLogin "admin@btcorp40.onmicrosoft.com" -targetCloudPassword "BinTree123" -targetObjectUPNSuffix "btcorp40.onmicrosoft.com" -testObjectNamePrefix "cds_c2c" -testDiscoveryGroup "CDSProbeDiscovery" -testObjectPassword "Password1"  -simulationMode

function New-CredentialFromClear ([string]$username,[string]$password)
{
  $ss = New-Object System.Security.SecureString
  $password.ToCharArray() | ForEach-Object { $ss.AppendChar($_) }
  return New-Object System.Management.Automation.PSCredential ($username,$ss)
}

[System.Int32]$retryDelay = 300
[System.Int32]$retryLimit = 50

function Write-Settings ()
{
  Write-Host ("SETTINGS: ")
  Write-Host ("----------------------------------------------")
  Write-Host ("sourceCloudLogin: $($sourceCloudLogin)")
  Write-Host ("sourceCloudPassword: $($sourceCloudPassword)")
  Write-Host ("sourceObjectUPNSuffix: $($sourceObjectUPNSuffix)")
  Write-Host ("")
  Write-Host ("targetCloudLogin: $($targetCloudLogin)")
  Write-Host ("targetCloudPassword: $($targetCloudPassword)")
  Write-Host ("targetObjectUPNSuffix: $($targetObjectUPNSuffix)")
  Write-Host ("")
  Write-Host ("testObjectPassword: $($testObjectPassword)")
  Write-Host ("testObjectNamePrefix: $($testObjectNamePrefix)")
  Write-Host ("")
  Write-Host ("msolUri: $($msolUri)")
  Write-Host ("msolConnectParams: $($msolConnectParams)")
  Write-Host ("")
  Write-Host ("simulationMode: $($simulationMode)")
  Write-Host ("----------------------------------------------")
  Write-Host ("")
  Write-Host ("")
  Write-Host ("Loop delay wait: $($retryDelay) seconds")
  Write-Host ("Maximum number of loops: $($retryLimit)")
  Write-Host ("")
  Write-Host ("")
}

$date = Get-Date -Format "MMddHHmm"
$month = (Get-Date).Month

$objectNameUser = [string]::Format("{0}{1}u",$testObjectNamePrefix,$date)
$objectNameGroup = [string]::Format("{0}{1}g",$testObjectNamePrefix,$date)

$sourceCloudCreds = New-CredentialFromClear $sourceCloudLogin $sourceCloudPassword
$targetCloudCreds = New-CredentialFromClear $targetCloudLogin $targetCloudPassword

$testObjectPasswordSecureString = ConvertTo-SecureString $testObjectPassword -AsPlainText -Force

$sessionOptions = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck

$sourceCloudSession = $null
$targetLocalSession = $null

try
{
  Write-Settings

  #region Source Local Mailbox Creation
  Write-Host ("Connecting to Source O365 Tenant")
  $sourceCloudSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $msolUri -Credential $sourceCloudCreds -Authentication Basic -AllowRedirection
  Import-PSSession $sourceCloudSession

  Write-Host ("Getting existing source users...")
  $mailUsers = Get-MailUser -ANR “$testObjectNamePrefix”
  $mailUsers.DisplayName

  Write-Host ("Getting existing source groups...")
  $distGroups = Get-DistributionGroup -ANR "$testObjectNamePrefix"
  $distGroups.DisplayName

  if (!$simulationMode)
  {
    if ($mailUsers.Count -eq $null) { $ucount = 1 } else { $ucount = $mailUsers.Count }
    Write-Host ("Removing existing probe source mail users: count=$($ucount)")
    if ($ucount -gt 0) 
    {
      $mailUsers | Remove-MailUser -Confirm:$false
    }

    if ($distGroups.Count -eq $null) { $dcount = 1 } else { $dcount = $distGroups.Count }
    Write-Host ("Removing existing probe source distribution groups: count=$($distGroups.Count)")
    if ($dcount -gt 0) 
    {
      $distGroups | Remove-DistributionGroup -Confirm:$false
    }
  }
  else
  {
    Write-Host ("SIMULATION: Removing existing probe mail user")
    Write-Host ("SIMULATION: Removing existing probe distribution groups")
  }

  if (!$simulationMode)
  {
    Write-Host ("Creating source Mail User: $($objectNameUser)")
    $newMailUser = New-MailUser -Name $objectNameUser -DisplayName $objectNameUser -MicrosoftOnlineServicesID $objectNameUser@$sourceObjectUPNSuffix -LastName Test -FirstName $objectNameUser -ExternalEmailAddress $objectNameUser@google.com -Password $testObjectPasswordSecureString
    if ($error -ne $null) { throw $error}

    Write-Host ("Creating Distribution Group: $($objectName)")
    $newDistGroup = New-DistributionGroup -DisplayName $objectNameGroup -Name $objectNameGroup -PrimarySmtpAddress $objectNameGroup@$sourceObjectUPNSuffix -Type Distribution
    if ($error -ne $null) { throw $error }

    Write-Host ("Adding new user to new group: $($newDistGroup)(member:$($objectNameUser))")
    Add-DistributionGroupMember -Identity $objectNameGroup -Member $objectNameUser

    Write-Host ("Adding new group to group filter: CDSProbeDiscovery(member:$($objectNameGroup))")
    Add-DistributionGroupMember -Identity CDSProbeDiscovery -Member $objectNameGroup

    Write-Host ("Adding new user to group filter: CDSProbeDiscovery(member:$($objectNameUser))")
    Add-DistributionGroupMember -Identity CDSProbeDiscovery -Member $objectNameUser
  }
  else
  {
    Write-Host ("SIMULATION: Creating Mail User: $($objectNameUser)")
    Write-Host ("SIMULATION: Creating Distribution Group: $($objectNameGroup)")
  }

  Write-Host ("Removing PSSession sourceLocalSession")
  Remove-PSSession $sourceCloudSession
  $sourceCloudSession = $null

  #region Target Local Mailbox Creation
  Write-Host ("Connecting to Target O365 Tenant")
  $targetCloudSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $msolUri -Credential $targetCloudCreds -Authentication Basic -AllowRedirection
  Import-PSSession $targetCloudSession

  $targetMailUser = $null
  $count = 0

  if (!$simulationMode)
  {
    do
    {
      Write-Host ("Trying to locate target $($objectNameUser) User in Target, attempt $($count + 1)...")
      $targetMailUser = Get-MailUser -ErrorAction SilentlyContinue $objectNameUser
      if ($targetMailUser)
      {
        Write-Host ("$($objectNameUser) found!")
        $targetMailUser | Format-List
        break;
      }
      $count++
      Start-Sleep -Seconds $retryDelay
    }
    while (!$targetMailUser -and ($count -lt $retryLimit))

    if (!$targetMailUser)
    {
      throw "$($targetMailUser) user could not be found!"
    }

    $targetDistributionGroup = $null

    do
    {
      Write-Host ("Trying to locate $($objectNameGroup) Group in Target, attempt $($count + 1)...")
      $targetDistributionGroup = Get-DistributionGroup -ErrorAction SilentlyContinue $objectNameGroup
      if ($targetMailUser)
      {
        Write-Host ("$($objectNameGroup) found!")
        $targetDistributionGroup | Format-List
        break;
      }
      $count++
      Start-Sleep -Seconds $retryDelay
    }
    while (!$targetDistributionGroup -and ($count -lt $retryLimit))

    if (!$targetDistributionGroup)
    {
      throw "$($targetDistributionGroup) group could not be found!"
    }
  }

  Write-Host ("Removing PSSession targetCloudSession")
  Remove-PSSession $targetCloudSession
  $targetCloudSession = $null
  return 0
}
catch
{
  Write-Error ("An error occurred. $($_.Exception)")
  return -1
}
finally
{
  if ($sourceCloudSession)
  {
    Write-Host ("Removing PSSession sourceCloudSession")
    Remove-PSSession $sourceCloudSession
  }

  if ($targetCloudSession)
  {
    Write-Host ("Removing PSSession targetCloudSession")
    Remove-PSSession $targetCloudSession
  }
}
