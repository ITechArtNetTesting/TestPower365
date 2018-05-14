function Invoke-Validate30475{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30475")){
			$Script:TestResults["Test30475"].ValidationResult = "Failed"	
			$Script:TestResults["Test30475"].OverAllResult = "Failed"	
			$Script:TestResults["Test30475"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30475"].Data.Folder3 -MessageId $Script:TestResults["Test30475"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				if($Message.Status -eq [Microsoft.Exchange.WebServices.Data.TaskStatus]::Completed){				
						$Script:TestResults["Test30475"].ValidationResult = "Succeeded"
						$Script:TestResults["Test30475"].OverAllResult = "Succeeded"  					
				}

			}
			else{
				$Script:TestResults["Test30475"].ValidationResult = "Failed"	
				$Script:TestResults["Test30475"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}