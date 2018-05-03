function Invoke-Validate30070{
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
		if($Script:TestResults.ContainsKey("Test30070")){
		$Folder  = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30070"].Data.Folder2)
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$Script:TestResults["Test30070"].ValidationResult = "Failed"	
		$Script:TestResults["Test30070"].OverAllResult = "Failed"	
		$Script:TestResults["Test30070"].ValidationLastRun = (Get-Date)	
		if($folder -ne $Null){
			$okay = $false
			#test permissions
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
			$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
			$folder.Load($psPropertySet)
			foreach($fperm in $folder.Permissions){  
				if($fperm.UserId.PrimarySmtpAddress -ne $null){  
					write-host ($fperm.UserId.PrimarySmtpAddress)
					if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test30070"].Data.FirstTargetPermission.ToLower()){
						if($fperm.PermissionLevel -eq [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Editor){
							$okay = $true
						} 
					}  
				}  
			}
			if($okay){
				$Folder1  = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30070"].Data.Folder1)
				$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
				$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
				$Folder1.Load($psPropertySet)
				foreach($fperm in $folder1.Permissions){  
					if($fperm.UserId.PrimarySmtpAddress -ne $null){  
						write-host ($fperm.UserId.PrimarySmtpAddress)
						if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test30070"].Data.FirstTargetPermission.ToLower()){
							if($fperm.PermissionLevel -eq [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Reviewer){
								$Script:TestResults["Test30070"].OverAllResult = "Successful"	
								$Script:TestResults["Test30070"].ValidationResult = "Succeeded"	
							} 
						}  
					}  
				}

			}

		}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}