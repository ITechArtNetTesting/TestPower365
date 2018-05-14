function Invoke-Validate32349{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32349")){
			$Script:TestResults["Test32349"].ValidationResult = "Failed"	
			$Script:TestResults["Test32349"].OverAllResult = "Failed"	
			$Script:TestResults["Test32349"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32349"].Data.Folder -MessageId $Script:TestResults["Test32349"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Message.Load()
				if($Message.Attachments.Count -eq 0){
						$Script:TestResults["Test32349"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32349"].OverAllResult = "Succeeded"  	
				}
			}
			else{
				$Script:TestResults["Test32349"].ValidationResult = "Failed"	
				$Script:TestResults["Test32349"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}