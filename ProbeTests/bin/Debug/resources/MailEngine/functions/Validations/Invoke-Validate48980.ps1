function Invoke-Validate48980{
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
		if($Script:TestResults.ContainsKey("Test48980")){
			$Folder1  = Invoke-P365FindFolder -TargetMailbox -FolderId $Script:TestResults["Test48980"].Data.Folder1 -ParentFolderPath \Inbox
			$Folder2  = Invoke-P365FindFolder -TargetMailbox -FolderId $Script:TestResults["Test48980"].Data.Folder2 -ParentFolderPath \Contacts			
			$Script:TestResults["Test48980"].ValidationLastRun = (Get-Date)
			if($Folder1 -ne $null){
				if($Folder2 -ne $null){
					$Script:TestResults["Test48980"].ValidationResult = "Succeeded"
					$Script:TestResults["Test48980"].OverAllResult = "Succeeded"  
				}
				else{
					$Script:TestResults["Test48980"].ValidationResult = "Failed"	
					$Script:TestResults["Test48980"].OverAllResult = "Failed"		
				}

			}
			else{
				$Script:TestResults["Test48980"].ValidationResult = "Failed"	
				$Script:TestResults["Test48980"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}