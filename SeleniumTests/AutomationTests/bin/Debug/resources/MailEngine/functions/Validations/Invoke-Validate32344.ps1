function Invoke-Validate32344{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32344")){
			$Script:TestResults["Test32344"].ValidationResult = "Failed"	
			$Script:TestResults["Test32344"].OverAllResult = "Failed"	
			$Script:TestResults["Test32344"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32344"].Data.Folder -MessageId $Script:TestResults["Test32344"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
						$Script:TestResults["Test32344"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32344"].OverAllResult = "Succeeded"  	
			}
			else{
				$Script:TestResults["Test32344"].ValidationResult = "Failed"	
				$Script:TestResults["Test32344"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}