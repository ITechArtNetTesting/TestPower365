function Invoke-Validate30522{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test30522")){
			$Script:TestResults["Test30522"].ValidationResult = "Failed"	
			$Script:TestResults["Test30522"].OverAllResult = "Failed"	
			$Script:TestResults["Test30522"].Data.Folder3
	    		$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test30522"].Data.Folder3 -MessageId $Script:TestResults["Test30522"].Data.MessageId -TargetMailbox
			if($Message -ne $null){	
				$Message.Load()
				$aptOkay = $false
				foreach($rcp in $Message.RequiredAttendees){	
					write-host $rcp	
					if($rcp.Address -eq "TestFake12345@binaryTree.com"){
						$aptOkay = $true;
					}
				}
				Write-Host "Done" -ForegroundColor Green
			}
			if($aptOkay){
					$Script:TestResults["Test30522"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30522"].OverAllResult = "Succeeded"  
			}
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}