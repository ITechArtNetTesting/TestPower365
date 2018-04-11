param([string]$slogin = "admin@BTCloud7.Power365.Cloud", [string]$spassword = "Password31")

	$password = ConvertTo-SecureString $spassword -AsPlainText -Force

	$creds = New-Object System.Management.Automation.PSCredential ($slogin, $password)
	
	$Session1 = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $creds -Authentication Basic -AllowRedirection
	Import-PSSession $Session1

	Import-Module msonline
	Connect-MsolService -Credential $Creds

Get-PublicFolder -Recurse -Identity "\AutomationTests" | Disable-MailPublicFolder -Confirm:$false
Remove-PublicFolder \AutomationTests -Recurse -Confirm:$false
New-PublicFolder AutomationTests -Path \
Add-PublicFolderClientPermission -Identity \AutomationTests -AccessRights 'Owner' -User $slogin
New-PublicFolder Test1 -Path \AutomationTests

Start-Sleep -Seconds 10

 If (Get-PublicFolder -Identity "\AutomationTests")
                        {
                         Write-Host ("Public Folder successfully created")
                        }
                        Else
                        {
                                        Write-Error 'Mailbox not found'                                        
                        }

#Add-PublicFolderClientPermission -Identity \AutomationTests\Test1 -AccessRights 'Owner' -User $userName
Remove-PSSession $Session1
