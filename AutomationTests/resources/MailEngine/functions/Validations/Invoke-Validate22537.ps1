function Invoke-Validate22537{
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
		if($Script:TestResults.ContainsKey("Test22537")){
			$MovedEmail = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22537"].Data.Id -FolderPath '\Inbox\test22537'
		    $EmailOkay = $false	
			$Script:TestResults["Test22537"].ValidationLastRun = (Get-Date)
			if($MovedEmail -ne $null){
				write-host ("Moved Email found okay")
				$MovedEmail.Load()
				foreach($rcp in $MovedEmail.ToRecipients){	
					write-host $rcp	
					if($rcp.Address.ToLower() -eq $Script:TestResults["Test22537"].Data.TargetAddress.ToLower()){
						$EmailOkay = $true;
						}
					}
			}
			if($EmailOkay){
				$Script:TestResults["Test22537"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22537"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22537"].ValidationResult = "Failed"	
				$Script:TestResults["Test22537"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}