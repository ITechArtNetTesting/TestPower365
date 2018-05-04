function Invoke-Validate30490{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30490")){
			$Script:TestResults["Test30490"].ValidationResult = "Failed"	
			$Script:TestResults["Test30490"].OverAllResult = "Failed"	
			$Script:TestResults["Test30490"].Data.Folder3
	    	        $Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30490"].Data.Folder3 -MessageId $Script:TestResults["Test30490"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Message.Start
				$Message.Due
				$Script:TestResults["Test30490"].Data.Start
				$Script:TestResults["Test30490"].Data.End
				if($Message.Start.ToString("yyyy-MM-ddTHH:mm:ss") -eq $Script:TestResults["Test30490"].Data.StartDate.ToString("yyyy-MM-ddTHH:mm:ss")){
					if($Message.End.ToString("yyyy-MM-ddTHH:mm:ss") -eq $Script:TestResults["Test30490"].Data.EndDate.ToString("yyyy-MM-ddTHH:mm:ss")){
						$Script:TestResults["Test30490"].ValidationResult = "Succeeded"
						$Script:TestResults["Test30490"].OverAllResult = "Successful"
					}
				}

			}
			else{
				$Script:TestResults["Test30490"].ValidationResult = "Failed"	
				$Script:TestResults["Test30490"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}