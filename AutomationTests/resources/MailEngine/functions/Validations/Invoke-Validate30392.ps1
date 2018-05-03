function Invoke-Validate30392{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30392")){
			$Script:TestResults["Test30392"].ValidationResult = "Failed"	
			$Script:TestResults["Test30392"].OverAllResult = "Failed"	
			$Script:TestResults["Test30392"].Data.Folder3
	    		$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30392"].Data.Folder3 -MessageId $Script:TestResults["Test30392"].Data.MessageId -TargetMailbox
			if($Message -eq $null){
				$Script:TestResults["Test30392"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30392"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test30392"].ValidationResult = "Failed"	
				$Script:TestResults["Test30392"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}