function Invoke-Validate30511{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30511")){
			$Script:TestResults["Test30511"].ValidationResult = "Failed"	
			$Script:TestResults["Test30511"].OverAllResult = "Failed"	
			$Script:TestResults["Test30511"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30511"].Data.Folder3 -MessageId $Script:TestResults["Test30511"].Data.MessageId -TargetMailbox
			if($Message -ne $null){	
				$Message.Load()
				if($Message.Subject -eq "Appointment Modification - 30511 "){
		
					$Script:TestResults["Test30511"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30511"].OverAllResult = "Succeeded"  

				}
				else{
					$Script:TestResults["Test30511"].ValidationResult = "Failed"	
					$Script:TestResults["Test30511"].OverAllResult = "Failed"		
				}
				Write-Host "Done" -ForegroundColor Green
			}
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}