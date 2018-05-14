function Invoke-Validate30489{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30489")){
			$Script:TestResults["Test30489"].ValidationResult = "Failed"	
			$Script:TestResults["Test30489"].OverAllResult = "Failed"	
			$Script:TestResults["Test30489"].Data.Folder3
	    	        $Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30489"].Data.Folder3 -MessageId $Script:TestResults["Test30489"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				if($Message.Location -eq $Script:TestResults["Test30489"].Data.Location){
					$Script:TestResults["Test30489"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30489"].OverAllResult = "Succeeded"  
				}

			}
			else{
				$Script:TestResults["Test30489"].ValidationResult = "Failed"	
				$Script:TestResults["Test30489"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}