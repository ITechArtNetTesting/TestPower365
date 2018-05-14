function Invoke-Validate32362{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32362")){
			$Script:TestResults["Test32362"].ValidationResult = "Failed"	
			$Script:TestResults["Test32362"].OverAllResult = "Failed"	
			$Script:TestResults["Test32362"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32362"].Data.Folder -MessageId $Script:TestResults["Test32362"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
						$Script:TestResults["Test32362"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32362"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test32362"].ValidationResult = "Failed"	
				$Script:TestResults["Test32362"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}