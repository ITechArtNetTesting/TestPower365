param([string]$slogin = "admin@BTCloud7.Power365.Cloud", [string]$spassword = "Password31")

	$password = ConvertTo-SecureString $spassword -AsPlainText -Force

	$creds = New-Object System.Management.Automation.PSCredential ($slogin, $password)
	
	$Session1 = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $creds -Authentication Basic -AllowRedirection
	Import-PSSession $Session1

	Import-Module msonline
	Connect-MsolService -Credential $Creds

Write-Host "Disable-MailPublicFolder"

Get-PublicFolder -Recurse -Identity "\AutomationTests" | where { $_.MailEnabled -eq $true } | Disable-MailPublicFolder -Confirm:$false
Write-Host "Remove-PublicFolder"
Remove-PublicFolder \AutomationTests -Recurse -Confirm:$false
Write-Host "New-PublicFolder"
New-PublicFolder AutomationTests -Path \
Write-Host "Add-PublicFolderClientPermission"
Add-PublicFolderClientPermission -Identity \AutomationTests -AccessRights 'Owner' -User $slogin
Write-Host "New-PublicFolder"
New-PublicFolder Test1 -Path \AutomationTests

Start-Sleep -Seconds 10
Write-Host "If Get-PublicFolder"
If (Get-PublicFolder -Identity "\AutomationTests")
{
    Write-Host ("Public Folder successfully created")
}
Else
{
	Write-Error 'Mailbox not found'                                        
}

#Add-PublicFolderClientPermission -Identity \AutomationTests\Test1 -AccessRights 'Owner' -User $userName
Write-Host "Remove-PSSession"
Remove-PSSession $Session1
