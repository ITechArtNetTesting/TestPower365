function Invoke-Validate30519{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30519")){
			$Script:TestResults["Test30519"].ValidationResult = "Failed"	
			$Script:TestResults["Test30519"].OverAllResult = "Failed"	
			$Script:TestResults["Test30519"].Data.Folder3
	    		$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30519"].Data.Folder3 -MessageId $Script:TestResults["Test30519"].Data.MessageId -TargetMailbox
			if($Message -ne $null){	
				$Message.Load()
				if($Message.Importance -eq [Microsoft.Exchange.WebServices.Data.Importance]::High){
		
					$Script:TestResults["Test30519"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30519"].OverAllResult = "Succeeded"  

				}
				else{
					$Script:TestResults["Test30519"].ValidationResult = "Failed"	
					$Script:TestResults["Test30519"].OverAllResult = "Failed"		
				}
				Write-Host "Done" -ForegroundColor Green
			}
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}