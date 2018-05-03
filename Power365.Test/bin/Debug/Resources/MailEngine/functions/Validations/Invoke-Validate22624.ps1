function Invoke-Validate22624{
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
		if($Script:TestResults.ContainsKey("Test22624")){
			$TargetMessage = Invoke-P365FindContact -TargetMailbox -MessageId $Script:TestResults["Test22624"].Data 
			$Script:TestResults["Test22624"].ValidationLastRun = (Get-Date)
			if($TargetMessage -eq $null){
				$Script:TestResults["Test22624"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22624"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test22624"].ValidationResult = "Failed"	
				$Script:TestResults["Test22624"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}