param([string]$slogin = "C7O365SML20@btcorp7.onmicrosoft.com", [string]$spassword = "BinTree123", [string]$mailbox = "C7O365SML20@btcorp7.onmicrosoft.com")
function New-CredentialFromClear([string]$username, [string]$password)
{
	$ss = new-object System.Security.SecureString
	$password.ToCharArray() | % { $ss.AppendChar($_) }
	return new-object System.Management.Automation.PSCredential($username, $ss)
}
$Date = Get-Date -Format "MMddHHmm"
$Creds = New-CredentialFromClear $slogin $spassword
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $Creds -Authentication Basic -AllowRedirection
Import-PSSession $Session

Import-Module msonline
Connect-MsolService -Credential $Creds

$host.UI.RawUI.WindowTitle = "Corp7"

##Modifying the User AD Attribute
Set-MsolUser -UserPrincipalName $mailbox -Title $Date

Write-Host ("Powershell will pause 50 seconds") -ForegroundColor Green
start-sleep -Seconds 300

remove-pssession $Session 