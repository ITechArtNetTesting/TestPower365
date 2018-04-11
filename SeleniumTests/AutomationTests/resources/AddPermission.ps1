param([string]$login = "Corp9O365Admin@btcorp9.onmicrosoft.com", [string]$password = "BinTree123", [string]$mailbox = "C7-Automation4@btcorp9.onmicrosoft.com", [string]$trustees = "C7-Automation1@btcorp7.onmicrosoft.com,C7-Automation2@btcorp7.onmicrosoft.com", [string]$accessRights = "FullAccess")

$ss = new-object System.Security.SecureString
$password.ToCharArray() | % { $ss.AppendChar($_) }
$creds = new-object System.Management.Automation.PSCredential($login, $ss)

$success = $true

##Connecting to Target Tenant
try
{
    Write-Host ("Connecting to Source")
    $SessionT = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $creds -Authentication Basic -AllowRedirection
    Import-PSSession $SessionT
    Import-Module msonline
    Connect-MsolService -Credential $creds
}
catch
{
	Write-Error $_.Exception.Message
	$success = $false
}

if($success)
{
	Start-Sleep -Seconds 1

	#####Adding Permission Back#####

	$trusteeList = $trustees.split(',')
	
	$mbx = Get-Mailbox $mailbox

	Foreach ($trustee in $trusteeList)
	{
		$mbx | Add-MailboxPermission -User $trustee -AccessRights $accessRights
	}
}

Write-Host ("Removing Target O365 Session")
Remove-pssession $SessionT