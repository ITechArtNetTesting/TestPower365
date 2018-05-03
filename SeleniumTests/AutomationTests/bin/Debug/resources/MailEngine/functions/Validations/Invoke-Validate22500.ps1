function Invoke-Validate22500{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		if($TargetMailbox.IsPresent){
			$service = $Script:TargetService
			$MailboxName = $Script:TargetMailbox
		}
		else{
			$service = $Script:SourceService
			$MailboxName = $Script:SourceMailbox
		}
		if($Script:TestResults.ContainsKey("Test22500")){
			$Folder1  = Invoke-P365FindFolder -TargetMailbox -FolderId $Script:TestResults["Test22500"].Data.Folder1 -ParentFolderPath \Inbox						
			$Script:TestResults["Test22500"].ValidationLastRun = (Get-Date)
			if($Folder1 -ne $null){
				$Script:TestResults["Test22500"].ValidationResult = "Failed"	
				$Script:TestResults["Test22500"].OverAllResult = "Failed"		
				$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
				$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
				$Folder1.Load($psPropertySet)
				foreach($fperm in $Folder1.Permissions){  
				if($fperm.UserId.PrimarySmtpAddress -ne $null){  
					write-host ($fperm.UserId.PrimarySmtpAddress)
					if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test22500"].Data.TargetPermission.ToLower()){  
						if($fperm.PermissionLevel -eq [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Editor){
								$Script:TestResults["Test22500"].ValidationResult = "Succeeded"
								$Script:TestResults["Test22500"].OverAllResult = "Successful"
							}

						}  
					}  
				}
			}
			else{
				$Script:TestResults["Test22500"].ValidationResult = "Failed"	
				$Script:TestResults["Test22500"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}