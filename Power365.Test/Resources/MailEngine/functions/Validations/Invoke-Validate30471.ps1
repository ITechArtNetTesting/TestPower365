function Invoke-Validate30471{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30471")){
			$Script:TestResults["Test30471"].ValidationResult = "Failed"	
			$Script:TestResults["Test30471"].OverAllResult = "Failed"	
			$Script:TestResults["Test30471"].Data.Folder3
	    		$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30471"].Data.Folder2 -MessageId $Script:TestResults["Test30471"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Script:TestResults["Test30471"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30471"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test30471"].ValidationResult = "Failed"	
				$Script:TestResults["Test30471"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}