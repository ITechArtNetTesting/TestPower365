function Invoke-Validate30394{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30394")){
			$Script:TestResults["Test30394"].ValidationResult = "Failed"	
			$Script:TestResults["Test30394"].OverAllResult = "Failed"	
			$Script:TestResults["Test30394"].Data.Folder3
	    		$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30394"].Data.Folder3 -MessageId $Script:TestResults["Test30394"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Script:TestResults["Test30394"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30394"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test30394"].ValidationResult = "Failed"	
				$Script:TestResults["Test30394"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}