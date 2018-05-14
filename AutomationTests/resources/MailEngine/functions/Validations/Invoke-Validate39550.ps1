function Invoke-Validate39550{
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
		if($Script:TestResults.ContainsKey("Test39550")){
			$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test39550"].Data -Archive -FolderPath "\Test39550"	
			$Script:TestResults["Test39550"].ValidationLastRun = (Get-Date)
			if($TargetMessage -eq $null){
				$Script:TestResults["Test39550"].ValidationResult = "Succeeded"
				$Script:TestResults["Test39550"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test39550"].ValidationResult = "Failed"	
				$Script:TestResults["Test39550"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}