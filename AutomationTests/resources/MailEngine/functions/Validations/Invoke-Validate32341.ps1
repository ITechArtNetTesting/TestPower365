function Invoke-Validate32341{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32341")){
			$Script:TestResults["Test32341"].ValidationResult = "Failed"	
			$Script:TestResults["Test32341"].OverAllResult = "Failed"	
			$Script:TestResults["Test32341"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32341"].Data.Folder -MessageId $Script:TestResults["Test32341"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				if($Message.Subject -eq "Stick Note Modified 32341"){
						$Script:TestResults["Test32341"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32341"].OverAllResult = "Succeeded"  		

				}

			}
			else{
				$Script:TestResults["Test32341"].ValidationResult = "Failed"	
				$Script:TestResults["Test32341"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}