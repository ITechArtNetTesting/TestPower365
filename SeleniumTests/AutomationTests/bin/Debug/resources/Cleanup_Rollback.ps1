param([string]$slogin = "BinaryTreePowerShellUser.BT-AutoQA2@btcorp7.onmicrosoft.com", [string]$spassword = "P@ssw0rd",[string]$smailbox = "C7-Automation3@btcorp7.onmicrosoft.com", [string]$SourceMailbox = "C7O365SML20@btcorp7.onmicrosoft.com")

    $userName = $slogin
	$password = ConvertTo-SecureString $spassword -AsPlainText -Force

	$creds = New-Object System.Management.Automation.PSCredential ($userName, $password)
	
	$Session1 = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $creds -Authentication Basic -AllowRedirection
	Import-PSSession $Session1

	Import-Module msonline
	Connect-MsolService -Credential $Creds

Set-Mailbox $smailbox -GrantSendOnBehalfTo @{remove=$SourceMailbox} -Confirm:$false 
Remove-RecipientPermission -Identity  $smailbox -Trustee $SourceMailbox -AccessRights SendAs -Confirm:$false 
 Remove-MailboxPermission -Identity $smailbox -User $SourceMailbox -AccessRights FullAccess -Confirm:$false  

 Remove-PSSession $Session1
