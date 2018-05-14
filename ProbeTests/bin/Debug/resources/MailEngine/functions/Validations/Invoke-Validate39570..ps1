function Invoke-Validate39570{
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
		if($Script:TestResults.ContainsKey("Test39570")){
			$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test39570"].Data.Message1			
			$Script:TestResults["Test39570"].ValidationLastRun = (Get-Date)
			if($TargetMessage -eq $null){
				$Script:TestResults["Test39570"].ValidationResult = "Succeeded"
				$Script:TestResults["Test39570"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test39570"].ValidationResult = "Failed"	
				$Script:TestResults["Test39570"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}