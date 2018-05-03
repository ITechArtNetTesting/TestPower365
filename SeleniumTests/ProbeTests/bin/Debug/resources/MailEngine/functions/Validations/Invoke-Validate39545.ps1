function Invoke-Validate39545{
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
		if($Script:TestResults.ContainsKey("Test39545")){			
 		        $TargetMessage = Invoke-P365FindMessageReply -TargetMailbox -MessageId $Script:TestResults["Test39545"].Data  -FolderPath ("\Inbox") 
			$Script:TestResults["Test39545"].ValidationLastRun = (Get-Date)
			if($TargetMessage -ne $null){
				$Script:TestResults["Test39545"].ValidationResult = "Succeeded"
				$Script:TestResults["Test39545"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test39545"].ValidationResult = "Failed"	
				$Script:TestResults["Test39545"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}