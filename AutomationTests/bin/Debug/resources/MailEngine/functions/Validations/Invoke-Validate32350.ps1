function Invoke-Validate32350{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32350")){
			$Script:TestResults["Test32350"].ValidationResult = "Failed"	
			$Script:TestResults["Test32350"].OverAllResult = "Failed"	
			$Script:TestResults["Test32350"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32350"].Data.Folder -MessageId $Script:TestResults["Test32350"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				if($Message.Department -eq "Contact Modified 32350"){
						$Script:TestResults["Test32350"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32350"].OverAllResult = "Succeeded"  		

				}

			}
			else{
				$Script:TestResults["Test32350"].ValidationResult = "Failed"	
				$Script:TestResults["Test32350"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}