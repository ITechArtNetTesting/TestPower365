function Invoke-Validate26765{
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
		if($Script:TestResults.ContainsKey("Test26765")){
			$Folder1  = Invoke-P365FindFolder -TargetMailbox -FolderId $Script:TestResults["Test26765"].Data.Folder1 -ParentFolderPath \Contacts
			$Folder2  = Invoke-P365FindFolder -TargetMailbox -FolderId $Script:TestResults["Test26765"].Data.Folder2 -ParentFolderPath \Contacts			
			$Script:TestResults["Test26765"].ValidationLastRun = (Get-Date)
			if($Folder1 -eq $null){
				if($Folder2 -ne $null){
					$Script:TestResults["Test26765"].ValidationResult = "Succeeded"
					$Script:TestResults["Test26765"].OverAllResult = "Succeeded"  
				}

			}
			else{
				$Script:TestResults["Test26765"].ValidationResult = "Failed"	
				$Script:TestResults["Test26765"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}