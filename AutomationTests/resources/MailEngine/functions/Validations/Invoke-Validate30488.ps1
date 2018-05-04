function Invoke-Validate30488{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30488")){
			$Script:TestResults["Test30488"].ValidationResult = "Failed"	
			$Script:TestResults["Test30488"].OverAllResult = "Failed"	
			$Script:TestResults["Test30488"].Data.Folder3
	    	        $Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30488"].Data.Folder3 -MessageId $Script:TestResults["Test30488"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Message.Subject
				$Script:TestResults["Test30488"].Data.NewSubject
				if($Message.Subject -eq $Script:TestResults["Test30488"].Data.NewSubject){
					$Script:TestResults["Test30488"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30488"].OverAllResult = "Successful"
				}

			}
			else{
				$Script:TestResults["Test30488"].ValidationResult = "Failed"	
				$Script:TestResults["Test30488"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}