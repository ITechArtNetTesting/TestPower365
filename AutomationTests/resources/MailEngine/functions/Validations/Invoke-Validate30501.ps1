function Invoke-Validate30501{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30501")){
			$Script:TestResults["Test30501"].ValidationResult = "Failed"	
			$Script:TestResults["Test30501"].OverAllResult = "Failed"	
			$Script:TestResults["Test30501"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30501"].Data.Folder3 -MessageId $Script:TestResults["Test30501"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Message.Sensitivity
				if($Message.Sensitivity.ToString() -eq "Private"){
		
				$Script:TestResults["Test30501"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30501"].OverAllResult = "Successful"

				}
				else{
					$Script:TestResults["Test30501"].ValidationResult = "Failed"	
					$Script:TestResults["Test30501"].OverAllResult = "Failed"		
				}
				Write-Host "Done" -ForegroundColor Green
			}
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}