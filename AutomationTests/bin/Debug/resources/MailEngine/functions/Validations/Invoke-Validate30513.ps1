function Invoke-Validate30513{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30513")){
			$Script:TestResults["Test30513"].ValidationResult = "Failed"	
			$Script:TestResults["Test30513"].OverAllResult = "Failed"	
			$Script:TestResults["Test30513"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30513"].Data.Folder3 -MessageId $Script:TestResults["Test30513"].Data.MessageId -TargetMailbox
			if($Message -ne $null){	
				$Message.Load()
				if($Message.Start.ToString() -eq $Script:TestResults["Test30513"].Data.StartTime.ToString()){
		
					$Script:TestResults["Test30513"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30513"].OverAllResult = "Succeeded"  

				}
				else{
					$Script:TestResults["Test30513"].ValidationResult = "Failed"	
					$Script:TestResults["Test30513"].OverAllResult = "Failed"		
				}
				Write-Host "Done" -ForegroundColor Green
			}
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}