function Invoke-Validate48974{
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
		if($Script:TestResults.ContainsKey("Test48974")){
			$Folder1  = Invoke-P365FindFolder -TargetMailbox -FolderId $Script:TestResults["Test48974"].Data.Folder1 -ParentFolderPath \Drafts						
			$Script:TestResults["Test48974"].ValidationLastRun = (Get-Date)
			if($Folder1 -ne $null){
				$Script:TestResults["Test48974"].ValidationResult = "Failed"	
				$Script:TestResults["Test48974"].OverAllResult = "Failed"		
				$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
				$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
				$Folder1.Load($psPropertySet)
				foreach($fperm in $Folder1.Permissions){  
				if($fperm.UserId.PrimarySmtpAddress -ne $null){  
					write-host ($fperm.UserId.PrimarySmtpAddress)
					if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test48974"].Data.SecondTargetPermission.ToLower()){ 
						if($Folder1.DisplayName -eq $Script:TestResults["Test48974"].Data.NewFolderName){
							$Script:TestResults["Test48974"].ValidationResult = "Succeeded"
							$Script:TestResults["Test48974"].OverAllResult = "Succeeded"  
						}

						}  
					}  
				}
			}
			else{
				$Script:TestResults["Test48974"].ValidationResult = "Failed"	
				$Script:TestResults["Test48974"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}