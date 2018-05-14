function Invoke-Validate30512{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30512")){
			$Script:TestResults["Test30512"].ValidationResult = "Failed"	
			$Script:TestResults["Test30512"].OverAllResult = "Failed"	
			$Script:TestResults["Test30512"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30512"].Data.Folder3 -MessageId $Script:TestResults["Test30512"].Data.MessageId -TargetMailbox
			if($Message -ne $null){	
				$Message.Load()
				if($Message.Location -eq "Appointment Location Modification - 30512"){
		
					$Script:TestResults["Test30512"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30512"].OverAllResult = "Succeeded"  

				}
				else{
					$Script:TestResults["Test30512"].ValidationResult = "Failed"	
					$Script:TestResults["Test30512"].OverAllResult = "Failed"		
				}
				Write-Host "Done" -ForegroundColor Green
			}
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}