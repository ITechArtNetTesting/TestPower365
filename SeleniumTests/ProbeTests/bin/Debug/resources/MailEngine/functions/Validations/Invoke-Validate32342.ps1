function Invoke-Validate32342{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32342")){
			$Script:TestResults["Test32342"].ValidationResult = "Failed"	
			$Script:TestResults["Test32342"].OverAllResult = "Failed"	
			$Script:TestResults["Test32342"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32342"].Data.Folder -MessageId $Script:TestResults["Test32342"].Data.MessageId -TargetMailbox
			if($Message -eq $null){
				$Script:TestResults["Test32342"].ValidationResult = "Succeeded"
				$Script:TestResults["Test32342"].OverAllResult = "Succeeded"  
			}
			else{
				$Script:TestResults["Test32342"].ValidationResult = "Failed"	
				$Script:TestResults["Test32342"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}