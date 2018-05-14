function Invoke-Validate22623{
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
		if($Script:TestResults.ContainsKey("Test22623")){
			$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22623"].Data -FolderPath "\Inbox"	
			$Script:TestResults["Test22623"].ValidationLastRun = (Get-Date)
			if($TargetMessage -eq $null){
				$Script:TestResults["Test22623"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22623"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22623"].ValidationResult = "Failed"	
				$Script:TestResults["Test22623"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}