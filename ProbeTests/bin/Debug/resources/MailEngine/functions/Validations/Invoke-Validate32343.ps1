function Invoke-Validate32343{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32343")){
			$Script:TestResults["Test32343"].ValidationResult = "Failed"	
			$Script:TestResults["Test32343"].OverAllResult = "Failed"	
			$Script:TestResults["Test32343"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32343"].Data.Folder -MessageId $Script:TestResults["Test32343"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
						$Script:TestResults["Test32343"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32343"].OverAllResult = "Succeeded"  	
			}
			else{
				$Script:TestResults["Test32343"].ValidationResult = "Failed"	
				$Script:TestResults["Test32343"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}