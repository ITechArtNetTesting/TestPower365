function Invoke-Validate32347{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32347")){
			$Script:TestResults["Test32347"].ValidationResult = "Failed"	
			$Script:TestResults["Test32347"].OverAllResult = "Failed"	
			$Script:TestResults["Test32347"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32347"].Data.Folder -MessageId $Script:TestResults["Test32347"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Script:TestResults["Test32347"].ValidationResult = "Succeeded"
				$Script:TestResults["Test32347"].OverAllResult = "Succeeded"  	
			}
			else{
				$Script:TestResults["Test32347"].ValidationResult = "Failed"	
				$Script:TestResults["Test32347"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}