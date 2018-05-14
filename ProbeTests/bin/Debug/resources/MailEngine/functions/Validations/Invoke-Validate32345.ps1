function Invoke-Validate32345{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		
		if($Script:TestResults.ContainsKey("Test32345")){
			$Script:TestResults["Test32345"].ValidationResult = "Failed"	
			$Script:TestResults["Test32345"].OverAllResult = "Failed"	
			$Script:TestResults["Test32345"].Data.Folder3
	    	$Message =  Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32345"].Data.Folder -MessageId $Script:TestResults["Test32345"].Data.MessageId -TargetMailbox
			if($Message -ne $null){
				if($Message.EmailAddresses[[Microsoft.Exchange.WebServices.Data.EmailAddressKey]::EmailAddress1].Address -eq "TestAddressFake890@binaryTree.com"){
						$Script:TestResults["Test32345"].ValidationResult = "Succeeded"
						$Script:TestResults["Test32345"].OverAllResult = "Succeeded"  		

				}

			}
			else{
				$Script:TestResults["Test32345"].ValidationResult = "Failed"	
				$Script:TestResults["Test32345"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}