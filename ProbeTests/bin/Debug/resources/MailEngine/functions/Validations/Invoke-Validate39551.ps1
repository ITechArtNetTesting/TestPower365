function Invoke-Validate39551{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		if($TargetMailbox.IsPresent){
			$service = $Script:TargetService
			$MailboxName = $Script:TargetMailbox
		}
		else{
			$service = $Script:SourceService
			$MailboxName = $Script:SourceMailbox
		}
		if($Script:TestResults.ContainsKey("Test39551")){
			$ItemId = New-Object Microsoft.Exchange.WebServices.Data.ItemId($Script:TestResults["Test39551"].Data)  
 		    $TargetMessage = [Microsoft.Exchange.WebServices.Data.Item]::Bind($Script:TargetService,$ItemId)
			$Script:TestResults["Test39551"].ValidationLastRun = (Get-Date)
			if($TargetMessage -ne $null){
				$Script:TestResults["Test39551"].ValidationResult = "Succeeded"
				$Script:TestResults["Test39551"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test39551"].ValidationResult = "Failed"	
				$Script:TestResults["Test39551"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}