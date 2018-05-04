function Invoke-Validate30391{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30391")){
			$Script:TestResults["Test30391"].ValidationResult = "Failed"	
			$Script:TestResults["Test30391"].OverAllResult = "Failed"	
			$Script:TestResults["Test30391"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30391"].Data.Folder3 -MessageId $Script:TestResults["Test30391"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Script:TestResults["Test30391"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30391"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test30391"].ValidationResult = "Failed"	
				$Script:TestResults["Test30391"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}