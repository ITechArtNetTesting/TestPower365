function Invoke-Validate30518{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30518")){
			$Script:TestResults["Test30518"].ValidationResult = "Failed"	
			$Script:TestResults["Test30518"].OverAllResult = "Failed"	
			$Script:TestResults["Test30518"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30518"].Data.Folder3 -MessageId $Script:TestResults["Test30518"].Data.MessageId -TargetMailbox
			if($Message -ne $null){	
				$Message.Load()
				$Message.TimeZone
				$tzone = $Message.TimeZone.ToString()
				Write-Host "TimeZone: $tzone"
				if($tzone -eq "(UTC) Coordinated Universal Time"){
		
				$Script:TestResults["Test30518"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30518"].OverAllResult = "Succeeded"  

				}
				else{
					$Script:TestResults["Test30518"].ValidationResult = "Failed"	
					$Script:TestResults["Test30518"].OverAllResult = "Failed"		
				}
				Write-Host "Done" -ForegroundColor Green
			}
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}