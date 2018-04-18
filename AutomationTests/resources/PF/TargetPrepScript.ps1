param([string]$slogin = "admin@BTCloud9.Power365.Cloud", [string]$spassword = "Password32")
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

Start-Sleep -Seconds 10

 If (Get-PublicFolder -Identity "\AutomationTests")
                        {
                         Write-Host ("Public Folder successfully created")
                        }
                        Else
                        {
                                        Write-Error 'Mailbox not found'                                        
                        }


Remove-PSSession $Session1