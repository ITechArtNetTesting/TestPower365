param(
	[Parameter(Mandatory=$true)]
	[string]$sourceLogin, 
	[Parameter(Mandatory=$true)]
	[string]$sourcePassword,

	[Parameter(Mandatory=$true)]
	[string]$targetLogin, 
	[Parameter(Mandatory=$true)]
	[string]$targetPassword,
		

	[Parameter(Mandatory=$true)]
	[string]$folderPath,

	[string]$msolUri = "https://ps.outlook.com/powershell",
	[string]$msolConnectParams = "",

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
	Write-Host ("sourceLogin: $($sourceLogin)")
	Write-Host ("sourcePassword: $($sourcePassword)")
    Write-Host ("")
	Write-Host ("targetLogin: $($targetLogin)")
	Write-Host ("targetPassword: $($targetPassword)")
    Write-Host ("")
	Write-Host ("folderPath: $($folderPath)")
	Write-Host ("")
	Write-Host ("msolUri: $($msolUri)")
	Write-Host ("msolConnectParams: $($msolConnectParams)")
	Write-Host ("")
	Write-Host ("simulationMode: $($simulationMode)")
    Write-Host ("----------------------------------------------")
    Write-Host ("")
    Write-Host ("")
}

function Create-FolderIfNotExists([string]$folderPath, [string]$owner)
{
	$pathTest = Get-PublicFolder -Identity $folderPath -ErrorAction SilentlyContinue
	if (!$pathTest)
	{
		$folderPath = $folderPath.TrimEnd('\')
		$parentFolder = $folderPath.Substring(0, $folderPath.lastIndexOf('\'))
		$leafFolder = $folderPath.Substring($folderPath.lastIndexOf('\') + 1)

		Create-FolderIfNotExists $parentFolder

		Write-Host "Creating $($leafFolder)"
		New-PublicFolder $leafFolder -Path $parentFolder
		Add-PublicFolderClientPermission -Identity $folderPath -AccessRights 'Owner' -User $owner -ErrorAction SilentlyContinue
	}
}

$sourceCredentials = New-CredentialFromClear $sourceLogin $sourcePassword
$targetCredentials = New-CredentialFromClear $targetLogin $targetPassword

$sessionOptions = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck

$sourceSession = $null
$targetSession = $null

try
{
    Write-Settings

	Write-Host ("Connecting to Source O365 Tenant")
    $sourceSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $msolUri -Credential $sourceCredentials -Authentication Basic -AllowRedirection
    Import-PSSession $sourceSession
    Import-Module msonline
	$connectMsolCommand = 'Connect-MsolService -ErrorAction Stop -Credential $sourceCredentials ' + $msolConnectParams
    Invoke-Expression $connectMsolCommand

	Get-PublicFolder -Recurse -Identity $folderPath -ErrorAction SilentlyContinue | where { $_.MailEnabled -eq $true } | Disable-MailPublicFolder -Confirm:$false
	Remove-PublicFolder $folderPath -Recurse -Confirm:$false -ErrorAction SilentlyContinue

	Create-FolderIfNotExists $folderPath $sourceLogin

	$sourceTest = Get-PublicFolder -Identity $folderPath -ErrorAction SilentlyContinue

	If (!$sourceTest)
	{
		throw "Source public folder not found"
	}

	Write-Host("Removing PSSession sourceSession")
    Remove-PSSession $sourceSession
    $sourceSession = $null

	Write-Host ("Connecting to Target O365 Tenant")
    $targetSession = New-PSSession -ErrorAction Stop -ConfigurationName Microsoft.Exchange -ConnectionUri $msolUri -Credential $targetCredentials -Authentication Basic -AllowRedirection
    Import-PSSession $targetSession
    Import-Module msonline
	$connectMsolCommand = 'Connect-MsolService -ErrorAction Stop -Credential $sourceCredentials ' + $msolConnectParams
    Invoke-Expression $connectMsolCommand

	Get-PublicFolder -Recurse -Identity $folderPath -ErrorAction SilentlyContinue | where { $_.MailEnabled -eq $true } | Disable-MailPublicFolder -Confirm:$false
	Remove-PublicFolder $folderPath -Recurse -Confirm:$false -ErrorAction SilentlyContinue

	Create-FolderIfNotExists $folderPath $targetLogin

	Write-Host("Removing PSSession targetSession")
    Remove-PSSession $targetSession
    $targetSession = $null
}
catch
{
	Write-Error ("An error occured. $($_.Exception)")
    return -1;
}
finally
{
    if($sourceSession)
    {
        Write-Host("Removing PSSession sourceSession")
        Remove-PSSession $sourceSession
    }
	if($targetSession)
    {
        Write-Host("Removing PSSession targetSession")
        Remove-PSSession $targetSession
    }
}