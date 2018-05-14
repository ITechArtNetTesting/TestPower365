function Invoke-Validate30479{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30479")){
			$Script:TestResults["Test30479"].ValidationResult = "Failed"	
			$Script:TestResults["Test30479"].OverAllResult = "Failed"	
			$Script:TestResults["Test30479"].Data.Folder3
	    		$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30479"].Data.Folder3 -MessageId $Script:TestResults["Test30479"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				if($Message.Subject -eq $Script:TestResults["Test30479"].Data.NewSubject){
					$Script:TestResults["Test30479"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30479"].OverAllResult = "Succeeded"  
				}

			}
			else{
				$Script:TestResults["Test30479"].ValidationResult = "Failed"	
				$Script:TestResults["Test30479"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}