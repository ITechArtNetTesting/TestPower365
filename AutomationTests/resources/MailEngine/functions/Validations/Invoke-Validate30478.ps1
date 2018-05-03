function Invoke-Validate30478{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30478")){
			$Script:TestResults["Test30478"].ValidationResult = "Failed"	
			$Script:TestResults["Test30478"].OverAllResult = "Failed"	
			$Script:TestResults["Test30478"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30478"].Data.Folder3 -MessageId $Script:TestResults["Test30478"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Message.StartDate
				$Message.DueDate
				$Script:TestResults["Test30478"].Data.StartDate
				$Script:TestResults["Test30478"].Data.DueDate
				if($Message.StartDate.ToString("yyyy-MM-ddTHH:mm:ss") -eq $Script:TestResults["Test30478"].Data.StartDate.ToString("yyyy-MM-ddTHH:mm:ss")){
					if($Message.DueDate.ToString("yyyy-MM-ddTHH:mm:ss") -eq $Script:TestResults["Test30478"].Data.DueDate.ToString("yyyy-MM-ddTHH:mm:ss")){
						$Script:TestResults["Test30478"].ValidationResult = "Succeeded"
						$Script:TestResults["Test30478"].OverAllResult = "Successful"
					}
				}

			}
			else{
				$Script:TestResults["Test30478"].ValidationResult = "Failed"	
				$Script:TestResults["Test30478"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}