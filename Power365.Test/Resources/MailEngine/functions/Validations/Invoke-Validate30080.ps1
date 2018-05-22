function Invoke-Validate30080{
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
		if($Script:TestResults.ContainsKey("Test30080")){
		$Folder  = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30080"].Data.Folder2)
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$Script:TestResults["Test30080"].ValidationResult = "Failed"	
		$Script:TestResults["Test30080"].OverAllResult = "Failed"	
		$Script:TestResults["Test30080"].ValidationLastRun = (Get-Date)	
		if($folder -ne $Null){
			$okay = $false
			#test permissions
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
			$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
			$folder.Load($psPropertySet)
			foreach($fperm in $folder.Permissions){  
				if($fperm.UserId.PrimarySmtpAddress -ne $null){  
					write-host ($fperm.UserId.PrimarySmtpAddress)
					if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test30080"].Data.SecondTargetPermission.ToLower()){
						if($fperm.CanCreateItems -eq $true){$okay = $true}
						if($okay){
 							if($fperm.DeleteItems -eq [Microsoft.Exchange.WebServices.Data.PermissionScope]::All){$okay = $true}
						}
						if($okay){
							 if($fperm.ReadItems -eq [Microsoft.Exchange.WebServices.Data.FolderPermissionReadAccess]::FullDetails){$okay = $true}
						}
						if($okay){
							 if($fperm.EditItems -eq  [Microsoft.Exchange.WebServices.Data.PermissionScope]::Owned){$okay = $true}
						}    
					}  
				}  
			}
			if($okay){
								$Script:TestResults["Test30080"].OverAllResult = "Succeeded" 	
								$Script:TestResults["Test30080"].ValidationResult = "Succeeded"	
			}

		}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}