function Invoke-Validate32369{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32369")){
			$Script:TestResults["Test32369"].ValidationResult = "Failed"	
			$Script:TestResults["Test32369"].OverAllResult = "Failed"	
			$Script:TestResults["Test32369"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32369"].Data.Folder -MessageId $Script:TestResults["Test32369"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Message.Load()
				if($Message.Categories.Contains("test12345")){				
						$Script:TestResults["Test32369"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32369"].OverAllResult = "Successful"					
				}

			}
			else{
				$Script:TestResults["Test32369"].ValidationResult = "Failed"	
				$Script:TestResults["Test32369"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}