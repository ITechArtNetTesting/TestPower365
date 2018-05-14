function Invoke-Validate32348{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32348")){
			$Script:TestResults["Test32348"].ValidationResult = "Failed"	
			$Script:TestResults["Test32348"].OverAllResult = "Failed"	
			$Script:TestResults["Test32348"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32348"].Data.Folder -MessageId $Script:TestResults["Test32348"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				$Message.Load()
				foreach($attach in $Message.Attachments){
					$attach.Load()
					write-host $attach.Name
					if($attach.Name -eq $Script:TestResults["Test32348"].Data.FileName){
						$Script:TestResults["Test32348"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32348"].OverAllResult = "Succeeded"  	
					}
    			}
			}
			else{
				$Script:TestResults["Test32348"].ValidationResult = "Failed"	
				$Script:TestResults["Test32348"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}