function Invoke-Validate22384{
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
		if($Script:TestResults.ContainsKey("Test22384")){
			$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22384"].Data			
			$Script:TestResults["Test22384"].ValidationLastRun = (Get-Date)
			if($TargetMessage.IsRead -eq $true){
				$Script:TestResults["Test22384"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22384"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22384"].ValidationResult = "Failed"	
				$Script:TestResults["Test22384"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}