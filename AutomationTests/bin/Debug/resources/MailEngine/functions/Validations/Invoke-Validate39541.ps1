function Invoke-Validate39541{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		$service = $Script:TargetService
		$MailboxName = $Script:TargetMailbox
		if($Script:TestResults.ContainsKey("Test39541")){
			$ItemId1 = new-object Microsoft.Exchange.WebServices.Data.ItemId($Script:TestResults["Test39541"].Data.Message1)
			$ItemId2 = new-object Microsoft.Exchange.WebServices.Data.ItemId($Script:TestResults["Test39541"].Data.Message2)
			$message1 =	[Microsoft.Exchange.WebServices.Data.EmailMessage]::Bind($service,$ItemId1)	
			$message2 =	[Microsoft.Exchange.WebServices.Data.EmailMessage]::Bind($service,$ItemId2)			
			$Script:TestResults["Test39541"].ValidationLastRun = (Get-Date)
			$Okay = $false
			if($message1 -ne $null){
				if($message2 -ne $null){
					$Okay = $true;
				}		
			}			
			if($Okay){
				$Script:TestResults["Test39541"].ValidationResult = "Succeeded"
				$Script:TestResults["Test39541"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test39541"].ValidationResult = "Failed"	
				$Script:TestResults["Test39541"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}