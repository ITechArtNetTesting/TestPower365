function Invoke-Validate30477{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30477")){
			$Script:TestResults["Test30477"].ValidationResult = "Failed"	
			$Script:TestResults["Test30477"].OverAllResult = "Failed"	
			$Script:TestResults["Test30477"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30477"].Data.Folder3 -MessageId $Script:TestResults["Test30477"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				if($Message.PercentComplete -eq 60){				
						$Script:TestResults["Test30477"].ValidationResult = "Succeeded"
						$Script:TestResults["Test30477"].OverAllResult = "Succeeded"  					
				}

			}
			else{
				$Script:TestResults["Test30477"].ValidationResult = "Failed"	
				$Script:TestResults["Test30477"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}