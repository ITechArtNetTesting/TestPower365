function Invoke-Validate22625{
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
		if($Script:TestResults.ContainsKey("Test22625")){
			$TargetMessage = Invoke-P365FindAppointment -TargetMailbox -MessageId $Script:TestResults["Test22625"].Data 
			$Script:TestResults["Test22625"].ValidationLastRun = (Get-Date)
			if($TargetMessage -eq $null){
				$Script:TestResults["Test22625"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22625"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22625"].ValidationResult = "Failed"	
				$Script:TestResults["Test22625"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}